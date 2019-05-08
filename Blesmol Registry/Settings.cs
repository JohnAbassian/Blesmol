using Blesmol.Core;
using System;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using Win32 = Microsoft.Win32;

namespace Blesmol.Registry {
	public class Settings : SettingsBase, ISettings {
		private const String EventName = @"Global\Blesmol.Registry.Settings.Change";
		private EventWaitHandle _Event;
		private EventWaitHandle Event => _Event ?? (_Event = EventWaitHandle.TryOpenExisting(EventName, out EventWaitHandle @event) ? @event : new EventWaitHandle(false, EventResetMode.AutoReset, EventName));
		// EventWaitHandleRights.Synchronize | EventWaitHandleRights.Modify

		protected override String Path => @"Software\Abassian.net\Blesmol";

		public IDiskSettings Disks { get; }
		public IThresholdSettings Threshold { get; }
		public IEmailSettings Email { get; }
		public IIntervalSettings Intervals { get; }

		protected override void Load(Win32.RegistryKey key) {
			Disks.Load();
			Threshold.Load();
			Email.Load();
			Intervals.Load();

			Task.Run(() => {
				Event.WaitOne();
				((ISettings)this).Load();
			});
		}

		protected override void Save(Win32.RegistryKey key) {
			Disks.Save();
			Threshold.Save();
			Email.Save();
			Intervals.Save();

			Event.Set();
		}

		public Settings(Boolean writeable = false) : base(writeable) {
			Disks = new DiskSettings(this);
			Threshold = new ThresholdSettings(this);
			Email = new EmailSettings(this);
			Intervals = new IntervalSettings(this);
		}
	}
}
