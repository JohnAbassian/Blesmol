using Blesmol.Core;
using System;
using Win32 = Microsoft.Win32;

namespace Blesmol.Registry {
	public class Settings : SettingsBase, ISettings {
		protected override String Path => @"Software\Abassian.net\Blesmol";

		public IDiskSettings Disks { get; }
		public IThresholdSettings Threshold { get; }
		public IEmailSettings Email { get; }
		public IIntervalSettings Intervals { get; }
		public IFileSystemLogSettings FileSystemLog { get;}

		protected override void Load(Win32.RegistryKey key) {
			Disks.Load();
			Threshold.Load();
			Email.Load();
			Intervals.Load();
			FileSystemLog.Load();
		}

		protected override void Save(Win32.RegistryKey key) {
			Disks.Save();
			Threshold.Save();
			Email.Save();
			Intervals.Save();
			FileSystemLog.Save();
		}

		public Settings(Boolean writeable = false) : base(writeable) {
			Disks = new DiskSettings(this);
			Threshold = new ThresholdSettings(this);
			Email = new EmailSettings(this);
			Intervals = new IntervalSettings(this);
			FileSystemLog = new FileSystemPathSettings(this);
		}
	}
}
