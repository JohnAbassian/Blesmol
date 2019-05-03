using Blesmol.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace Blesmol {
	public partial class FrmMain : Form {
		private readonly Boolean _FormLoaded = false;
		private readonly ISettings Settings;
		private Int32 defaultThresholdAmount = 0;
		private Units.Unit defaultThresholdUnit = Units.Unit.GB;
		private Int32 defaultSleepInterval = 5;
		private Int32 defaultAlertInterval = 45;
		private Int32 defaultSmtpServerPort = 25;

		public FrmMain(ISettings settings) {
			InitializeComponent();
			InitializeCboUnit();
			GetDrives();

			Settings = settings;
			LoadSettings();

			_FormLoaded = true;
		}

		private void FrmMain_Load(object sender, EventArgs e) {
			Size = new Size(320, 370);
			CenterToScreen();
		}

		private void InitializeCboUnit() {
			cboUnit.DataSource = Enum.GetValues(typeof(Units.Unit))
				.Cast<Units.Unit>()
				.Select(value => new {
					Description = (value == Units.Unit.Percentage) ? "%" : value.ToString(),
					Value = value
				})
				.OrderBy(unit => unit.Value)
				.ToList();
			cboUnit.DisplayMember = "Description";
			cboUnit.ValueMember = "Value";
		}

		private void GetDrives() {
			foreach (DriveInfo dirInfo in DriveInfo.GetDrives()) {
				if (dirInfo.IsReady) {
					String DriveLabel = dirInfo.Name.Replace(@"\", "") + " " + AppendLabel(dirInfo.VolumeLabel);

					String AppendLabel(String volumeLabel) {
						if (String.IsNullOrEmpty(volumeLabel)) {
							switch (dirInfo.DriveType) {
								case DriveType.Fixed:
									return "Local Disk";
								case DriveType.Removable:
								case DriveType.CDRom:
									return "Removable Disk";
								case DriveType.Network:
									return "Networked Disk";
								default:
									return "Unknown Disk";
							}
						}
						return volumeLabel;
					}

					clbDisks.Items.Add(DriveLabel + "   [" + ConvertBytes(dirInfo.TotalFreeSpace) + " free of " + ConvertBytes(dirInfo.TotalSize) + "]", false);
				}
			}

			String ConvertBytes(Int64 byteCount) {
				Units.Unit unit;

				if (byteCount >= (Int64)Units.Unit.GB)
					unit = Units.Unit.GB;
				else if (byteCount >= (Int64)Units.Unit.MB)
					unit = Units.Unit.MB;
				else if (byteCount >= (Int64)Units.Unit.KB)
					unit = Units.Unit.KB;
				else
					unit = Units.Unit.B;

				return $"{Units.ConvertFromBytes(byteCount, unit):##.##}" + " " + unit;
			}
		}

		private void SaveAllSettings() {
			SaveDiskSettings();
			SaveThresholdSettings();
			SaveEmailSettings();
			SaveIntervalSettings();

			Settings.Save();

			RestartService();
		}

		private void SaveDiskSettings() {
			if (!_FormLoaded) return;

			String[] disks = new String[clbDisks.CheckedItems.Count];

			foreach (String i in clbDisks.CheckedItems) {
				disks[clbDisks.CheckedItems.IndexOf(i)] = i.Substring(i.IndexOf("(", StringComparison.Ordinal) + 1, 1);
			}

			Settings.Disks.Disks = disks;
		}

		private void SaveThresholdSettings() {
			if (!_FormLoaded) return;

			Settings.Threshold.Amount = Int32.TryParse(txtAmount.Text, out Int32 amount) ? amount : defaultThresholdAmount;
			Settings.Threshold.Unit = (Units.Unit)cboUnit.SelectedValue;
		}

		private void SaveEmailSettings() {
			if (!_FormLoaded) return;

			Settings.Email.MachineName = txtMachineName.Text;
			Settings.Email.SmtpServer = txtSmtpServer.Text;
			Settings.Email.SmtpServerUsername = txtUsername.Text;
			Settings.Email.SmtpServerPassword = txtPassword.Text;
			Settings.Email.SendFrom = txtSendFrom.Text;
			Settings.Email.SmtpServerPort = Int32.TryParse(txtPort.Text, out Int32 port) ? port : defaultSmtpServerPort;
			Settings.Email.EmailAddresses = txtEmails.Text;
		}

		private void SaveIntervalSettings() {
			if (!_FormLoaded) return;

			Settings.Intervals.Sleep = Int32.TryParse(txtSleepInterval.Text, out Int32 sleep) ? sleep : defaultSleepInterval;
			Settings.Intervals.Alert = Int32.TryParse(txtEmailDelay.Text, out Int32 alert) ? alert : defaultAlertInterval;
		}

		private void LoadSettings() {
			Settings.Load();

			if (Settings?.Disks?.Disks != null) {
				foreach (String k in Settings.Disks.Disks) {
					if (clbDisks.FindString(k + ":") > -1) {
						clbDisks.SetItemChecked(clbDisks.FindString(k + ":"), true);
					}
				}
			}

			txtAmount.Text = (Settings?.Threshold?.Amount ?? defaultThresholdAmount).ToString();
			cboUnit.SelectedValue = Settings.Threshold?.Unit != null && Settings.Threshold.Unit != default ? Settings.Threshold.Unit : defaultThresholdUnit;

			txtMachineName.Text = coalesceString(Settings?.Email?.MachineName) ?? System.Windows.Forms.SystemInformation.ComputerName;
			txtSmtpServer.Text = coalesceString(Settings?.Email?.SmtpServer) ?? "127.0.0.1";
			txtSendFrom.Text = coalesceString(Settings?.Email?.SendFrom) ?? "change_me@domain.com";
			txtPort.Text = (Settings?.Email?.SmtpServerPort ?? defaultSmtpServerPort).ToString();
			txtUsername.Text = Settings?.Email?.SmtpServerUsername;
			txtPassword.Text = Settings?.Email?.SmtpServerPassword;

			//';' is an acceptable (and used in stored setting) separator for backwards compatibility...
			txtEmails.Text = Settings?.Email?.EmailAddresses?.Replace(';', ',') ?? "";

			txtSleepInterval.Text = (Settings?.Intervals?.Sleep ?? defaultSleepInterval).ToString();
			txtEmailDelay.Text = (Settings?.Intervals?.Alert ?? defaultAlertInterval).ToString();

			RestartService();

			String coalesceString(String s) => String.IsNullOrEmpty(s) ? null : s;
		}

		private void RestartService() {
			try {
				ServiceController controller = new ServiceController();
				controller.MachineName = ".";
				controller.ServiceName = "Blesmol";
				String status = controller.Status.ToString();

				if (controller.Status == System.ServiceProcess.ServiceControllerStatus.Running) {
					controller.Stop();
					Thread.Sleep(1500);
					if (clbDisks.CheckedItems.Count > 0) {
						controller.Start();
					}
				}
			} catch { }
		}

		private void btnDisks_Click(object sender, EventArgs e) {
			panelDisks.Location = new Point(-2, 83);
			panelDisks.Height = 239;
			panelDisks.Width = 308;
			panelDisks.Visible = true;
			panelEmail.Visible = false;
			panelOptions.Visible = false;
		}

		private void btnEmail_Click(object sender, EventArgs e) {
			panelEmail.Location = new Point(-2, 83);
			panelEmail.Height = 239;
			panelEmail.Width = 308;
			panelEmail.Visible = true;
			panelDisks.Visible = false;
			panelOptions.Visible = false;
		}

		private void btnOptions_Click(object sender, EventArgs e) {
			panelOptions.Location = new Point(-2, 83);
			panelOptions.Height = 239;
			panelOptions.Width = 308;
			panelOptions.Visible = true;
			panelDisks.Visible = false;
			panelEmail.Visible = false;
		}

		private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) {
			SaveAllSettings();
		}

		private void clbDisks_SelectedValueChanged(object sender, EventArgs e) {
			SaveDiskSettings();
		}

		private void txtSendFrom_TextChanged(object sender, EventArgs e) {
			SaveEmailSettings();
		}

		private void txtMachineName_TextChanged(object sender, EventArgs e) {
			SaveEmailSettings();
		}

		private void txtSmtpServer_TextChanged(object sender, EventArgs e) {
			SaveEmailSettings();
		}

		private void txtPort_TextChanged(object sender, EventArgs e) {
			SaveEmailSettings();
		}

		private void txtUsername_TextChanged(object sender, EventArgs e) {
			SaveEmailSettings();
		}

		private void txtPassword_TextChanged(object sender, EventArgs e) {
			SaveEmailSettings();
		}

		private void txtEmailDelay_TextChanged(object sender, EventArgs e) {
			SaveIntervalSettings();
		}

		private void cboUnit_SelectedIndexChanged(object sender, EventArgs e) {
			SaveThresholdSettings();
		}

		private void TxtAmount_TextChanged(Object sender, EventArgs e) {
			SaveThresholdSettings();
		}

		private void TxtSleepInterval_TextChanged(Object sender, EventArgs e) {
			SaveIntervalSettings();
		}

		private void BtnSave_Click(Object sender, EventArgs e) {
			SaveAllSettings();
		}
	}
}
