using Blesmol.Core;
using System;

namespace Blesmol.Registry {
	internal class IntervalSettings : SettingsBase, IIntervalSettings {
		protected override String Path => "Intervals";

		private readonly ISetting Sleep;
		Int32? IIntervalSettings.Sleep {
			get => (Int32?)Sleep.Value;
			set => Sleep.Value = value;
		}

		private readonly ISetting Alert;
		Int32? IIntervalSettings.Alert {
			get => (Int32?)Alert.Value;
			set => Alert.Value = value;
		}

		public IntervalSettings(SettingsBase parent) : base(parent) {
			AddSetting<Int32?>("Sleep", out Sleep);
			AddSetting<Int32?>("Alert", out Alert);
		}
	}
}
