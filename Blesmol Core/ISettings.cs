using System;

namespace Blesmol.Core {
	public interface ISettingsLifecycle {
		void Load();
		void Save();
	}

	public interface ISettings : ISettingsLifecycle {
		IDiskSettings Disks { get; }
		IThresholdSettings Threshold { get; }
		IEmailSettings Email { get; }
		IIntervalSettings Intervals { get; }
		IFileSystemLogSettings FileSystemLog { get; }
	}

	public interface IDiskSettings : ISettingsLifecycle {
		String[] Disks { get; set; }
	}

	public interface IThresholdSettings : ISettingsLifecycle {
		Int32? Amount { get; set; }
		Units.Unit Unit { get; set; }
	}

	public interface IEmailSettings : ISettingsLifecycle {
		String MachineName { get; set; }
		String SmtpServer { get; set; }
		String SmtpServerUsername { get; set; }
		String SmtpServerPassword { get; set; }
		String SendFrom { get; set; }
		Int32? SmtpServerPort { get; set; }
		String EmailAddresses { get; set; }
	}

	public interface IIntervalSettings : ISettingsLifecycle {
		Int32? Sleep { get; set; }
		Int32? Alert { get; set; }
	}

	public interface IFileSystemLogSettings : ISettingsLifecycle {
		String FileSystemPath { get; set; }
		Int32? MaximumToLog {get;set;}
	}
}
