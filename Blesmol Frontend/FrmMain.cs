using Blesmol.Core;
using Blesmol_FileSystem;
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
		private readonly Boolean _FormLoaded;
		private readonly ISettings Settings;
		private readonly Int32 defaultThresholdAmount = 0;
		private readonly Units.Unit defaultThresholdUnit = Units.Unit.GB;
		private readonly Int32 defaultSleepInterval = 5;
		private readonly Int32 defaultAlertInterval = 45;
		private readonly Int32 defaultSmtpServerPort = 25;
		private List<String> panels = new List<String>() { "pnlSettings", "panelDisks", "panelEmail", "panelErrors", "panelOptions" };

		public FrmMain(ISettings settings) {
			InitializeComponent();
			InitializeCboUnit();
			GetDrives();

			Settings = settings;
			LoadSettings();

			foreach (String s in panels) {
				Panel p = (Panel)Controls.Find(s, true).First();
				p.Location = new Point(-2, 83);
				p.Height = 239;
				p.Width = 398;
			}

			BindLog(new FileLogger());


			SwitchToPanel(panelDisks);

			_FormLoaded = true;
		}

		private void FrmMain_Load(object sender, EventArgs e) {
			Size = new Size(414, 370);
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
			SaveFileSystemSettings();

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

		private void SaveFileSystemSettings() {
			if (!_FormLoaded) return;

			Settings.FileSystemLog.FileSystemPath = txtLogPath.Text;
			Settings.FileSystemLog.MaximumToLog = Int32.Parse(txtMaximumLogSize?.Text ?? "1000");
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
			txtLogPath.Text = Settings?.FileSystemLog?.FileSystemPath ?? AppDomain.CurrentDomain.BaseDirectory;
			txtMaximumLogSize.Text = Settings?.FileSystemLog?.MaximumToLog?.ToString() ?? "1000";
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

		private void btnDisks_Click(object sender, EventArgs e) => SwitchToPanel(panelDisks);

		private void btnEmail_Click(object sender, EventArgs e) => SwitchToPanel(panelEmail);

		private void btnOptions_Click(object sender, EventArgs e) => SwitchToPanel(panelOptions);

		private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) => SaveAllSettings();

		private void clbDisks_SelectedValueChanged(object sender, EventArgs e) => SaveDiskSettings();

		private void txtSendFrom_TextChanged(object sender, EventArgs e) => SaveEmailSettings();

		private void txtMachineName_TextChanged(object sender, EventArgs e) => SaveEmailSettings();

		private void txtSmtpServer_TextChanged(object sender, EventArgs e) => SaveEmailSettings();

		private void txtPort_TextChanged(object sender, EventArgs e) => SaveEmailSettings();

		private void txtUsername_TextChanged(object sender, EventArgs e) => SaveEmailSettings();

		private void txtPassword_TextChanged(object sender, EventArgs e) => SaveEmailSettings();

		private void txtEmailDelay_TextChanged(object sender, EventArgs e) => SaveIntervalSettings();

		private void cboUnit_SelectedIndexChanged(object sender, EventArgs e) => SaveThresholdSettings();

		private void TxtAmount_TextChanged(Object sender, EventArgs e) => SaveThresholdSettings();

		private void TxtSleepInterval_TextChanged(Object sender, EventArgs e) => SaveIntervalSettings();

		private void BtnSave_Click(Object sender, EventArgs e) => SaveAllSettings();

		private void btnErrors_Click(Object sender, EventArgs e) => SwitchToPanel(panelErrors);

		private void BtnSettings_Click(Object sender, EventArgs e) => SwitchToPanel(pnlSettings);

		private void SwitchToPanel(Panel panel) {
			foreach (String s in panels.Where(x => x == panel.Name)) Toggle(s);
			foreach (String s in panels.Where(x => x != panel.Name)) Toggle(s, false);

			void Toggle(String name, Boolean show = true) {
				Controls.Find(name, true).First().Visible = show;
			}
		}

		private void BtnDeleteLog_Click(Object sender, EventArgs e) {
			FileLogger logger = new FileLogger();
			logger.DeleteAll();

			BindLog(logger);
		}

		private void BindLog(FileLogger logger) {
			List<LoggedError> errors = logger.GetLoggedErrors().OrderBy(x => x.OccurredOn).ToList();
			btnProblems.Visible = errors.Count > 0;
			if (errors.Count > 0) {
				btnProblems.Text = errors.Count.ToString();
				dataGridErrors.DataSource = new BindingSource(new BindingList<LoggedError>(errors), null);
			}else SwitchToPanel(panelDisks);
		}
	}
}
