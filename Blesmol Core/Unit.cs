using System;

namespace Blesmol.Core {
	public static class Units {
		public static Int64 ConvertFromBytes(Int64 bytes, Unit unit) => bytes > 0 ? bytes / (Int64)unit : 0;

		public static Int64 ConvertToBytes(Int64 bytes, Unit unit) => bytes * (Int64)unit;

		public enum Unit : Int64 {
			B = 1,
			KB = 1024,
			MB = 1048576,
			GB = 1073741824,
			TB = 1099511627776
		}
	}
}
