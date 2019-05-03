using Blesmol.Core;
using System;

namespace Blesmol.Registry {
	internal class ThresholdSettings : SettingsBase, IThresholdSettings {
		protected override String Path => "Threshold";

		private readonly ISetting Amount;
		Int32? IThresholdSettings.Amount {
			get => (Int32?)Amount.Value;
			set => Amount.Value = value;
		}

		private readonly ISetting Unit;
		Units.Unit IThresholdSettings.Unit {
			get => (Units.Unit)Unit.Value;
			set => Unit.Value = value;
		}

		public ThresholdSettings(SettingsBase baseSettings) : base(baseSettings) {
			AddSetting<Int32?>("Amount", out Amount);
			AddSetting<Units.Unit>("Unit", out Unit);
		}
	}
}
