using System;

namespace Blesmol.Core {
	public static class Units {
		private static Int64 TryConvert(Int64 bytes, Unit unit, Func<Int64, Units.Unit, Int64> convert) =>
			unit == Unit.Percentage ? throw new ArgumentOutOfRangeException("Unit.Percentage is not supported for byte conversion")
									: bytes < 0 ? throw new ArgumentOutOfRangeException("byte count must be greater than or equal to 0")
												: convert(bytes, unit);

		public static Int64 ConvertFromBytes(Int64 bytes, Unit unit) => TryConvert(bytes, unit, (b, u) => b / (Int64)u);

		public static Int64 ConvertToBytes(Int64 bytes, Unit unit) => TryConvert(bytes, unit, (b, u) => b * (Int64)u);

		public enum Unit : Int64 {
			Percentage = -1,
			B = 1,
			KB = 1024,
			MB = 1048576,
			GB = 1073741824,
			TB = 1099511627776
		}
	}
}
