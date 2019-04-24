using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Blesmol {
	public class Utils {
		public Utils() { }

		public static String ConvertBytes(Int64 byteCount) {
			String size = "0 Bytes";
			if (byteCount >= 1099511627776)
				size = String.Format("{0:##.##}", byteCount / 1099511627776) + " TB";
			else if (byteCount >= 1073741824)
				size = String.Format("{0:##.##}", byteCount / 1073741824) + " GB";
			else if (byteCount >= 1048576)
				size = String.Format("{0:##.##}", byteCount / 1048576) + " MB";
			else if (byteCount >= 1024)
				size = String.Format("{0:##.##}", byteCount / 1024) + " KB";
			else if (byteCount > 0 && byteCount < 1024)
				size = byteCount.ToString() + " Bytes";

			return size;
		}

		public static Int64 ConvertToBytes(int amount, String unit) {
			switch (unit.ToLower(System.Globalization.CultureInfo.CurrentCulture)) {
				case "tb":
					return ((Int64)amount) * 1099511627776;
				case "gb":
					return ((Int64)amount) * 1073741824;
				case "mb":
					return ((Int64)amount) * 1048576;
				case "kb":
					return ((Int64)amount) * 1024;
				default:
					return 0;
			}
		}

		public static long ConvertBytes(Int64 valueCount, String unit) {
			switch (unit.ToLower(System.Globalization.CultureInfo.CurrentCulture)) {
				case "tb":
					return valueCount / 1099511627776;
				case "gb":
					return valueCount / 1073741824;
				case "mb":
					return valueCount / 1048576;
				case "kb":
					return valueCount / 1024;
				default:
					return 0;
			}
		}

		
	}
	public static class SysUtils {
		public static void LogEvent(String message, EventLogEntryType type) {
			using (EventLog eventLog = new EventLog("Application")) {
				eventLog.Source = "Application";
				eventLog.WriteEntry(message, type);
			}
		}
	}
}
