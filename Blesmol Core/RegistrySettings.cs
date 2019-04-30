using Microsoft.Win32;
using System;

namespace Blesmol.Core.RegistrySettings {
	public abstract class SettingsBase {
		protected readonly Boolean Writeable;
		protected readonly SettingsBase Parent;
		
		protected abstract String RegistryRoot { get; }

		protected RegistryKey _Key;
		protected virtual RegistryKey Key => _Key ?? (_Key = (Parent?.Key ?? Registry.LocalMachine).CreateSubKey(RegistryRoot, Writeable));

		protected String Get(ref String inner, String name) {
			if (String.IsNullOrEmpty(inner)) {
				inner = Key.GetValue(name)?.ToString();
			}

			return inner;
		}
		protected Int32? Get(ref Int32? inner, String name) {
			if (inner == null && Int32.TryParse(Key.GetValue(name)?.ToString(), out Int32 value)) {
				inner = value;
			}

			return inner;
		}

		protected String[] Get(ref String[] inner) => inner ?? (inner = Key?.GetValueNames());

		protected void Set<T>(ref T inner, String name, T value) {
			if (!Writeable) ThrowNotWriteableException(name);

			Key?.SetValue(name, value);
			inner = default;
		}

		protected void Set(ref String[] inner, String name, String[] values) {
			if (!Writeable) ThrowNotWriteableException(name);

			Parent.Key.DeleteSubKey(name);
			_Key = null;

			if (Key != null) {
				foreach (String value in values) {
					Key.SetValue(value, 1);
				}
			}

			inner = null;
		}

		protected void ThrowNotWriteableException(String setting) {
			throw new InvalidOperationException("Cannot alter setting " + setting + "; registry is not in a writeable state");
		}

		public SettingsBase(Boolean writeable) => Writeable = writeable;

		public SettingsBase(SettingsBase parent) {
			Parent = parent;
			Writeable = Parent?.Writeable ?? false;
		}
	}

	public class Settings : SettingsBase, ISettings {
		protected override String RegistryRoot => @"Software\Abassian.net\Blesmol";

		public IDiskSettings Disks { get; }
		public IEmailSettings Email { get; }
		public IThresholdSettings Threshold { get; }
		public IIntervalSettings Intervals { get; }

		public Settings(Boolean writeable = false) : base(writeable) {
			Disks = new DiskSettings(this);
			Email = new EmailSettings(this);
			Threshold = new ThresholdSettings(this);
			Intervals = new IntervalSettings(this);
		}
	}

	internal class IntervalSettings : SettingsBase, IIntervalSettings {
		protected override String RegistryRoot => "Intervals";

		private Int32? _Sleep;
		public Int32? Sleep {
			get => Get(ref _Sleep, "Sleep");
			set => Set(ref _Sleep, "Sleep", value);
		}

		private Int32? _Alert;
		public Int32? Alert {
			get => Get(ref _Alert, "Alert");
			set => Set(ref _Alert, "Alert", value);
		}

		public IntervalSettings(SettingsBase baseSettings) : base(baseSettings) { }
	}

	internal class ThresholdSettings : SettingsBase, IThresholdSettings {
		protected override String RegistryRoot => "Threshold";

		private Int32? _Amount;
		public Int32? Amount {
			get => Get(ref _Amount, "Amount");
			set => Set(ref _Amount, "Amount", value);
		}

		private String _Unit;
		public String Unit {
			get => Get(ref _Unit, "Unit");
			set => Set(ref _Unit, "Unit", value);
		}

		public ThresholdSettings(SettingsBase baseSettings) : base(baseSettings) { }
	}

	internal class DiskSettings : SettingsBase, IDiskSettings {
		protected override String RegistryRoot => "Disks";

		private String[] _Disks;
		public String[] Disks {
			get => Get(ref _Disks);
			set => Set(ref _Disks, "Disks", value);
		}

		public DiskSettings(SettingsBase baseSettings) : base(baseSettings) { }
	}

	internal class EmailSettings : SettingsBase, IEmailSettings {
		protected override String RegistryRoot => "Email";

		private String _EmailAddresses;
		public String EmailAddresses {
			get => Get(ref _EmailAddresses, "EmailAddresses");
			set => Set(ref _EmailAddresses, "EmailAddresses", value);
		}

		private String _MachineName;
		public String MachineName {
			get => Get(ref _MachineName, "MachineName");
			set => Set(ref _MachineName, "MachineName", value);
		}

		private String _SmtpServer;
		public String SmtpServer {
			get => Get(ref _SmtpServer, "SmtpServer");
			set => Set(ref _SmtpServer, "SmtpServer", value);
		}

		private String _SmtpServerUsername;
		public String SmtpServerUsername {
			get => Get(ref _SmtpServerUsername, "SmtpServerUsername");
			set => Set(ref _SmtpServerUsername, "SmtpServerUsername", value);
		}

		private String _SmtpServerPassword;
		public String SmtpServerPassword {
			get => Get(ref _SmtpServerPassword, "SmtpServerPassword");
			set => Set(ref _SmtpServerPassword, "SmtpServerPassword", value);
		}

		private String _SendFrom;
		public String SendFrom {
			get => Get(ref _SendFrom, "SendFrom");
			set => Set(ref _SendFrom, "SendFrom", value);
		}

		private Int32? _SmtpServerPort;
		public Int32? SmtpServerPort {
			get => Get(ref _SmtpServerPort, "SmtpServerPort");
			set => Set(ref _SmtpServerPort, "SmtpServerPort", value);
		}

		public EmailSettings(SettingsBase baseSettings) : base(baseSettings) { }
	}
}
