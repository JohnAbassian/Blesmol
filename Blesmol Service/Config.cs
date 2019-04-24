using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;

namespace Blesmol {
	public class Config {
		private String _RegistryRoot = @"Software\Abassian.net\Blesmol";
		public String[] EmailAddresses;
		public ArrayList DisksToMonitor = new ArrayList();
		public Int32 ThresholdAmount;
		public String ThresholdUnit;
		public String SmtpServer;
		public String SmtpServerPort;
		public String SmtpServerUsername;
		public String SmtpServerPassword;
		public String SendFrom;
		public String MachineName;
		public Boolean LoadError;
		public Int32 EmailDelay = 45;
		public Int32 SleepInterval;

		public Config() { }

		public void LoadConfig() {
			RegistryKey key = Registry.LocalMachine.OpenSubKey(_RegistryRoot + @"\Disks", true);

			if (key != null) {
				foreach (String k in key.GetValueNames()) {
					DisksToMonitor.Add(k);
				}
			} else {
				SysUtils.LogEvent("Blesmol is incorrectly configured. Please run the Blesmol application. Service will remain idle until restarted.", EventLogEntryType.Error);

				LoadError = true;
				return;
			}

			if (DisksToMonitor.Count < 0) {
				SysUtils.LogEvent("No disks were selected for monitoring. Service will remain idle until restarted.", EventLogEntryType.Warning);

				LoadError = true;
				return;
			}

			try {
				key = Registry.LocalMachine.OpenSubKey(_RegistryRoot + @"\Threshold");
				ThresholdAmount = Convert.ToInt32(key.GetValue("Amount", "1").ToString());
				ThresholdUnit = key.GetValue("Unit", "MB").ToString();

				Int32.TryParse(Registry.LocalMachine.OpenSubKey(_RegistryRoot + @"\SleepInterval").GetValue("SleepInterval").ToString(), out SleepInterval);

				key = Registry.LocalMachine.OpenSubKey(_RegistryRoot);
				if (String.IsNullOrEmpty(key.GetValue("EmailAddresses", "").ToString())) {
					SysUtils.LogEvent("No email addresses have been specified for alerts. Service will remain idle until restarted.", EventLogEntryType.Warning);

					LoadError = true;
					return;
				}

				EmailAddresses = key.GetValue("EmailAddresses", "").ToString().IndexOf(";") > 0 ? key.GetValue("EmailAddresses", "").ToString().Split(char.Parse(";")) : new String[] { key.GetValue("EmailAddresses", "").ToString() };

				if (String.IsNullOrEmpty(key.GetValue("MachineName", "").ToString()) || String.IsNullOrEmpty(key.GetValue("SmtpServer", "").ToString()) || String.IsNullOrEmpty(key.GetValue("SendFrom", "").ToString())) {
					SysUtils.LogEvent("Blesmol is incorrectly configured. Please run the Blesmol application. Service will remain idle until restarted.", EventLogEntryType.Error);
					LoadError = true;
					return;
				}

				MachineName = key.GetValue("MachineName", "").ToString();
				SmtpServer = key.GetValue("SmtpServer", "").ToString();
				SendFrom = key.GetValue("SendFrom", "").ToString();
				SmtpServerPort = key.GetValue("SmtpServerPort", "25").ToString();
				SmtpServerUsername = key.GetValue("SmtpServerUsername", "").ToString();
				SmtpServerPassword = key.GetValue("SmtpServerPassword", "").ToString();
				Int32.TryParse(key.GetValue("EmailDelay", "45").ToString(), out EmailDelay);
			} catch {
				SysUtils.LogEvent("Blesmol is incorrectly configured. Please run the Blesmol application. Service will remain idle until restarted.", EventLogEntryType.Error);
				LoadError = true;
				return;
			}
		}
	}
}
