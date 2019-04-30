using System;

namespace Blesmol.Core {
	public interface ISettings {
		IDiskSettings Disks { get; }
		IThresholdSettings Threshold { get; }
		IEmailSettings Email { get; }
		IIntervalSettings Intervals { get; }
	}

	public interface IDiskSettings {
		String[] Disks { get; set; }
	}

	public interface IThresholdSettings {
		Int32? Amount { get; set; }
		Units.Unit Unit { get; set; }
	}

	public interface IEmailSettings {
		String MachineName { get; set; }
		String SmtpServer {get;set;}
		String SmtpServerUsername {get;set;}
		String SmtpServerPassword {get;set;}
		String SendFrom {get;set;}
		Int32? SmtpServerPort {get;set;}
		String EmailAddresses { get; set; }
	}

	public interface IIntervalSettings {
		Int32? Sleep { get; set; }
		Int32? Alert { get; set; }
	}
}
