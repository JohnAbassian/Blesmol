using Blesmol.Core;
using System;
using System.Linq;
using Win32 = Microsoft.Win32;

namespace Blesmol.Registry {
	internal class DiskSettings : SettingsBase, IDiskSettings {
		protected override String Path => "Disks";

		protected override void Load(Win32.RegistryKey key) {
			Disks = key.GetValueNames();
		}

		protected override void Save(Win32.RegistryKey key) {
			String[] drives = key.GetValueNames();

			foreach (var drive in drives.Except(Disks)) key.DeleteValue(drive);
			foreach (var drive in Disks.Except(drives)) key.SetValue(drive, 1);
		}

		public String[] Disks { get; set; }

		public DiskSettings(SettingsBase parent) : base(parent) { }
	}
}
