using Blesmol.Core;
using System;

namespace Blesmol.Registry {
	internal class FileSystemPathSettings : SettingsBase, IFileSystemLogSettings {
		protected override String Path => "FileSystemPath";


		private readonly ISetting FileSystemPath;
		String IFileSystemLogSettings.FileSystemPath {
			get => FileSystemPath.Value as String;
			set => FileSystemPath.Value = value;
		}

		private readonly ISetting MaximumToLog;
		Int32? IFileSystemLogSettings.MaximumToLog {
			get => (Int32?)MaximumToLog?.Value;
			set => MaximumToLog.Value = value;
		}

		public FileSystemPathSettings(SettingsBase parent) : base(parent) {
			AddSetting<String>("FileSystemPath", out FileSystemPath);
			AddSetting<Int32?>("MaximumToLog", out MaximumToLog);
		}

	}
}
