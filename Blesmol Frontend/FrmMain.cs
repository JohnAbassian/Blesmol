using System;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Blesmol.Core;
using Blesmol.Core.RegistrySettings;

namespace Blesmol {
	public partial class FrmMain : Form {
		private readonly Boolean _FormLoaded = false;
		private ISettings Settings;
		private Int32 defaultThresholdAmount = 0;
		private String defaultThresholdUnit = "GB";
		private Int32 defaultSleepInterval = 5;
		private Int32 defaultAlertInterval = 45;
		private Int32 defaultSmtpServerPort = 25;

		public FrmMain() {
			InitializeComponent();
			GetDrives();
			LoadSettings();
			_FormLoaded = true;
		}

		private void FrmMain_Load(object sender, EventArgs e) {
			Size = new System.Drawing.Size(311, 350);
			CenterToScreen();
		}


		public void GetDrives() {
			DriveInfo[] allDrives = DriveInfo.GetDrives();
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

					clbDisks.Items.Add(DriveLabel + "   [" + ConvertBytes(Convert.ToInt64(dirInfo.TotalFreeSpace)) + " free of " + ConvertBytes(Convert.ToInt64(dirInfo.TotalSize)) + "]", false);
				}
			}
		}

		public String ConvertBytes(Int64 byteCount) {
			String size = "0 Bytes";
			if (byteCount >= 1073741824)
				size = $"{byteCount / 1073741824:##.##}" + " GB";
			else if (byteCount >= 1048576)
				size = $"{byteCount / 1048576:##.##}" + " MB";
			else if (byteCount >= 1024)
				size = $"{byteCount / 1024:##.##}" + " KB";
			else if (byteCount > 0 && byteCount < 1024)
				size = byteCount.ToString() + " Bytes";

			return size;
		}

		private void SaveAllSettings() {
			SaveDiskSettings();
			SaveThresholdSettings();
			SaveEmailSettings();
		}

		public void SaveDiskSettings() {
			if (!_FormLoaded) return;

			String[] disks = new String[clbDisks.CheckedItems.Count];

			foreach (String i in clbDisks.CheckedItems) {
				disks[clbDisks.CheckedItems.IndexOf(i)] = i.Substring(i.IndexOf("(", StringComparison.Ordinal) + 1, 1);
			}

			Settings.Disks.Disks = disks;
		}

		public void SaveThresholdSettings() {
			if (!_FormLoaded) return;

			Settings.Threshold.Amount = Int32.TryParse(txtAmount.Text, out Int32 amount) ? amount : defaultThresholdAmount;
			Settings.Threshold.Unit = cboUnit.SelectedItem as String ?? defaultThresholdUnit;
		}

		public void SaveIntervalSettings() {
			if (!_FormLoaded) return;

			Settings.Intervals.Sleep = Int32.TryParse(txtSleepInterval.Text, out Int32 sleep) ? sleep : defaultSleepInterval;
			Settings.Intervals.Alert = Int32.TryParse(txtEmailDelay.Text, out Int32 alert) ? alert : defaultAlertInterval;
		}

		private void SaveEmailSettings() {
			if (!_FormLoaded) return;

			Settings.Email.MachineName = txtMachineName.Text;
			Settings.Email.SmtpServer = txtSmtpServer.Text;
			Settings.Email.SmtpServerUsername = txtUsername.Text;
			Settings.Email.SmtpServerPassword = txtPassword.Text;
			Settings.Email.SendFrom = txtSendFrom.Text;
			Settings.Email.SmtpServerPort = Int32.TryParse(txtPort.Text, out Int32 port) ? port : defaultSmtpServerPort;

			SaveEmailAddresses();
		}

		public void SaveEmailAddresses() {
			String emailAddresses = "";

			foreach (DataGridViewRow dr in dgEmailAddresses.Rows) {
				if (dr.Cells[0].Value is String email) {
					emailAddresses += email + ";";
				}
			}

			if (!String.IsNullOrEmpty(emailAddresses)) {
				Settings.Email.EmailAddresses = emailAddresses.Substring(0, emailAddresses.Length - 1);
			}
		}

		public void LoadSettings() {
			Settings = new Settings(true);

			if (Settings?.Disks?.Disks != null) {
				foreach (String k in Settings.Disks.Disks) {
					if (clbDisks.FindString(k + ":") > -1) {
						clbDisks.SetItemChecked(clbDisks.FindString(k + ":"), true);
					}
				}
			}

			txtAmount.Text = (Settings?.Threshold?.Amount ?? defaultThresholdAmount).ToString();
			cboUnit.SelectedIndex = cboUnit.FindStringExact(Settings.Threshold?.Unit ?? defaultThresholdUnit);

			txtMachineName.Text = coalesceString(Settings?.Email?.MachineName) ?? System.Windows.Forms.SystemInformation.ComputerName;
			txtSmtpServer.Text = coalesceString(Settings?.Email?.SmtpServer) ?? "127.0.0.1";
			txtSendFrom.Text = coalesceString(Settings?.Email?.SendFrom) ?? "change_me@domain.com";
			txtPort.Text = (Settings?.Email?.SmtpServerPort ?? defaultSmtpServerPort).ToString();
			txtUsername.Text = Settings?.Email?.SmtpServerUsername;
			txtPassword.Text = Settings?.Email?.SmtpServerPassword;

			if (Settings?.Email?.EmailAddresses != null) {
				foreach (String s in Settings.Email.EmailAddresses.Split(';')) {
					if (!String.IsNullOrEmpty(s)) {
						DataGridViewRow dr = new DataGridViewRow();
						dgEmailAddresses.Rows.Add(s);
					}
				}
			}

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
			RestartService();
		}

		private void clbDisks_SelectedValueChanged(object sender, EventArgs e) {
			SaveDiskSettings();
		}

		private void dgEmailAddresses_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e) {
			SaveEmailAddresses();
		}

		private void dgEmailAddresses_CurrentCellChanged(object sender, EventArgs e) {
			SaveEmailAddresses();
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
			SaveThresholdSettings();
		}
	}
}
