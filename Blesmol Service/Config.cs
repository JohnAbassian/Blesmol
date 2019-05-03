using System;
using System.Collections;
using System.Diagnostics;
using Blesmol.Core;
using Blesmol.Registry;

namespace Blesmol {
	public class Config {
		private ISettings Settings = new Settings();
		public String[] EmailAddresses;
		public ArrayList DisksToMonitor = new ArrayList();
		public Int32 ThresholdAmount;
		public Units.Unit ThresholdUnit;
		public String SmtpServer;
		public Int32 SmtpServerPort;
		public String SmtpServerUsername;
		public String SmtpServerPassword;
		public String SendFrom;
		public String MachineName;
		public Boolean LoadError;
		public Int32 EmailDelay = 45;
		public Int32 SleepInterval;

		public Config() { }

		public void LoadConfig() {
			try {
				Settings.Load();

				foreach (String k in Settings.Disks.Disks) {
					DisksToMonitor.Add(k);
				}

				if (DisksToMonitor.Count < 0) {
					SysUtils.LogEvent("No disks were selected for monitoring. Service will remain idle until restarted.", EventLogEntryType.Warning);
					LoadError = true;
					return;
				}

				ThresholdAmount = Settings.Threshold.Amount.Value;
				ThresholdUnit = Settings.Threshold.Unit;

				MachineName = coalesceString(Settings.Email.MachineName, "MachineName");
				SmtpServer = coalesceString(Settings.Email.SmtpServer, "SmtpServer");
				SendFrom = coalesceString(Settings.Email.SendFrom, "SendFrom");
				SmtpServerPort = Settings.Email.SmtpServerPort.Value;
				SmtpServerUsername = coalesceString(Settings.Email.SmtpServerUsername, "SmtpServerUsername");
				SmtpServerPassword = coalesceString(Settings.Email.SmtpServerPassword, "SmtpServerPassword");
				EmailAddresses = coalesceString(Settings.Email.EmailAddresses, "EmailAddresses").Split(';');

				SleepInterval = Settings.Intervals.Sleep.Value;
				EmailDelay = Settings.Intervals.Alert.Value;
			} catch (Exception ex) {
				SysUtils.LogEvent("Blesmol is incorrectly configured. Please run the Blesmol application. Service will remain idle until restarted." + ex.Message, EventLogEntryType.Error);
				LoadError = true;
				return;
			}

			String coalesceString(String value, String name) => !String.IsNullOrEmpty(value) ? value : throw new InvalidOperationException(name);
		}
	}
}
