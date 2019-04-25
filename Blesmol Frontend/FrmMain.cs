using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Blesmol {
	public partial class FrmMain : Form {
		private readonly Boolean _FormLoaded = false;
		private readonly String _RegistryRoot = @"Software\Abassian.net\Blesmol";

		public FrmMain() {
			InitializeComponent();
			InitializeRegistry();
			GetDrives();
			LoadSettings();
			_FormLoaded = true;
		}

		private void FrmMain_Load(object sender, EventArgs e) {
			Size = new System.Drawing.Size(320, 370);
			CenterToScreen();
		}

		private void InitializeRegistry() {
			RegistryKey key = Registry.LocalMachine.OpenSubKey(_RegistryRoot, true);
			if (key == null) {
				Registry.LocalMachine.CreateSubKey(_RegistryRoot);
			}
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
			SaveAlertSettings();
			SaveEmailAddresses();
			SaveEmailOptions();
		}

		public void SaveDiskSettings() {
			if (!_FormLoaded) return;

			RegistryKey key = Registry.LocalMachine.OpenSubKey(_RegistryRoot + @"\Disks");
			if (key != null) {
				key = Registry.LocalMachine.CreateSubKey(_RegistryRoot);
				key?.DeleteSubKeyTree("Disks");
			}

			foreach (String i in clbDisks.CheckedItems) {
				key = Registry.LocalMachine.CreateSubKey(_RegistryRoot + @"\Disks");
				key?.SetValue(i.Substring(i.IndexOf("(", StringComparison.Ordinal) + 1, 1), "1");
			}
		}

		public void SaveAlertSettings() {
			if (!_FormLoaded) return;

			RegistryKey key = Registry.LocalMachine.CreateSubKey(_RegistryRoot);

			key = Registry.LocalMachine.CreateSubKey(_RegistryRoot + @"\Threshold");
			key?.SetValue("Amount", txtAmount.Text);
			if (cboUnit.SelectedItem != null) key?.SetValue("Unit", cboUnit.SelectedItem);
			key = Registry.LocalMachine.CreateSubKey(_RegistryRoot + @"\SleepInterval");
			key?.SetValue("SleepInterval", txtSleepInterval.Text ?? "5");

			Console.WriteLine(@"threshold settings saved to registry");
		}

		public void SaveEmailAddresses() {
			RegistryKey key = Registry.LocalMachine.CreateSubKey(_RegistryRoot);
			
			//';' is an acceptable (and used in stored setting) separator for backwards compatibility...
			key?.SetValue("EmailAddresses", txtEmails.Text.Replace(',', ';'));

		}

		private void SaveEmailOptions() {
			RegistryKey key = Registry.LocalMachine.CreateSubKey(_RegistryRoot);
			key.SetValue("MachineName", txtMachineName.Text);
			key.SetValue("SmtpServer", txtSmtpServer.Text);
			key.SetValue("SmtpServerUsername", txtUsername.Text);
			key.SetValue("SmtpServerPassword", txtPassword.Text);
			key.SetValue("SendFrom", txtSendFrom.Text);
			try {
				int port = Convert.ToInt32(txtPort.Text);
				key.SetValue("SmtpServerPort", txtPort.Text);
			} catch {
				key.SetValue("SmtpServerPort", "25");
			}

			try {
				Int32 mailDelay = Convert.ToInt32(txtEmailDelay.Text);
				key.SetValue("EmailDelay", txtEmailDelay.Text);
			} catch {
				key.SetValue("EmailDelay", "45");
			}
		}

		public void LoadSettings() {
			RegistryKey key = Registry.LocalMachine.OpenSubKey(_RegistryRoot + @"\Disks");
			if (key != null) {
				String[] keys = key.GetValueNames();
				foreach (String k in key.GetValueNames()) {
					if (clbDisks.FindString(k + ":") > -1) {
						clbDisks.SetItemChecked(clbDisks.FindString(k + ":"), true);
					}
				}
			}

			key = Registry.LocalMachine.CreateSubKey(_RegistryRoot + @"\Threshold");

			if (key != null) {
				txtAmount.Text = key.GetValue("Amount", "0").ToString();
				cboUnit.SelectedIndex = cboUnit.FindStringExact(key.GetValue("Unit", "GB").ToString());
			} else {
				txtAmount.Text = @"0";
				cboUnit.SelectedIndex = 0;
			}

			txtSleepInterval.Text = Registry.LocalMachine.CreateSubKey(_RegistryRoot + @"\SleepInterval")?.GetValue("SleepInterval", "5").ToString();

			key = Registry.LocalMachine.OpenSubKey(_RegistryRoot);
			//';' is an acceptable (and used in stored setting) separator for backwards compatibility...
			txtEmails.Text = key?.GetValue("EmailAddresses", "").ToString().Replace(';', ',') ?? "";

			key = Registry.LocalMachine.OpenSubKey(_RegistryRoot);

			String sMachineName = String.IsNullOrEmpty(key?.GetValue("MachineName", "").ToString()) ? System.Windows.Forms.SystemInformation.ComputerName : key.GetValue("MachineName", "").ToString();

			String sSmtpServer = String.IsNullOrEmpty(key?.GetValue("SmtpServer", "").ToString()) ? "127.0.0.1" : key.GetValue("SmtpServer", "").ToString();

			String sSendFrom = String.IsNullOrEmpty(key?.GetValue("SendFrom", "").ToString()) ? "change_me@domain.com" : key.GetValue("SendFrom", "").ToString();

			String sSmtpServerPort = String.IsNullOrEmpty(key?.GetValue("SmtpServerPort", "").ToString()) ? "25" : key.GetValue("SmtpServerPort", "").ToString();

			String sSmtpServerUserName = String.IsNullOrEmpty(key?.GetValue("SmtpServerUsername", "").ToString()) ? "" : key.GetValue("SmtpServerUsername", "").ToString();

			String sSmtpServerPassword = String.IsNullOrEmpty(key?.GetValue("SmtpServerPassword", "").ToString()) ? "" : key.GetValue("SmtpServerPassword", "").ToString();

			String sEmailDelay = String.IsNullOrEmpty(key?.GetValue("EmailDelay", "").ToString()) ? "45" : key.GetValue("EmailDelay", "").ToString();

			txtMachineName.Text = sMachineName;
			txtSmtpServer.Text = sSmtpServer;
			txtSendFrom.Text = sSendFrom;
			txtPort.Text = sSmtpServerPort;
			txtUsername.Text = sSmtpServerUserName;
			txtPassword.Text = sSmtpServerPassword;
			txtEmailDelay.Text = sEmailDelay;

			RestartService();
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
			SaveEmailOptions();
		}

		private void txtMachineName_TextChanged(object sender, EventArgs e) {
			SaveEmailOptions();
		}

		private void txtSmtpServer_TextChanged(object sender, EventArgs e) {
			SaveEmailOptions();
		}

		private void txtPort_TextChanged(object sender, EventArgs e) {
			SaveEmailOptions();
		}

		private void txtUsername_TextChanged(object sender, EventArgs e) {
			SaveEmailOptions();
		}

		private void txtPassword_TextChanged(object sender, EventArgs e) {
			SaveEmailOptions();
		}

		private void txtEmailDelay_TextChanged(object sender, EventArgs e) {
			SaveEmailOptions();
		}

		private void cboUnit_SelectedIndexChanged(object sender, EventArgs e) {
			SaveAlertSettings();
		}

		private void TxtAmount_TextChanged(Object sender, EventArgs e) {
			SaveAlertSettings();
		}

		private void TxtSleepInterval_TextChanged(Object sender, EventArgs e) {
			SaveAlertSettings();
		}

		private void BtnSave_Click(Object sender, EventArgs e) {
			SaveAllSettings();
			RestartService();
		}
	}
}
