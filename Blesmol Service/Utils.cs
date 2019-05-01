using System;
using System.Diagnostics;

namespace Blesmol {
	public static class SysUtils {
		public static void LogEvent(String message, EventLogEntryType type) {
			using (EventLog eventLog = new EventLog("Application")) {
				eventLog.Source = "Application";
				eventLog.WriteEntry(message, type);
			}
		}
	}
}
