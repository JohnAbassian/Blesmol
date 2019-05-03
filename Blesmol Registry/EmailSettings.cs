using Blesmol.Core;
using System;

namespace Blesmol.Registry {
	internal class EmailSettings : SettingsBase, IEmailSettings {
		protected override String Path => "Email";

		private readonly ISetting EmailAddresses;
		String IEmailSettings.EmailAddresses {
			get => EmailAddresses.Value as String;
			set => EmailAddresses.Value = value;
		}

		private readonly ISetting MachineName;
		String IEmailSettings.MachineName {
			get => MachineName.Value as String;
			set => MachineName.Value = value;
		}

		private readonly ISetting SmtpServer;
		String IEmailSettings.SmtpServer {
			get => SmtpServer.Value as String;
			set => SmtpServer.Value = value;
		}

		private readonly ISetting SmtpServerUsername;
		String IEmailSettings.SmtpServerUsername {
			get => SmtpServerUsername.Value as String;
			set => SmtpServerUsername.Value = value;
		}

		private readonly ISetting SmtpServerPassword;
		String IEmailSettings.SmtpServerPassword {
			get => SmtpServerPassword.Value as String;
			set => SmtpServerPassword.Value = value;
		}

		private readonly ISetting SendFrom;
		String IEmailSettings.SendFrom {
			get => SendFrom.Value as String;
			set => SendFrom.Value = value;
		}

		private readonly ISetting SmtpServerPort;
		Int32? IEmailSettings.SmtpServerPort {
			get => (Int32?)SmtpServerPort.Value;
			set => SmtpServerPort.Value = value;
		}

		public EmailSettings(SettingsBase parent) : base(parent) {
			AddSetting<String>("EmailAddresses", out EmailAddresses);
			AddSetting<String>("MachineName", out MachineName);
			AddSetting<String>("SmtpServer", out SmtpServer);
			AddSetting<String>("SmtpServerUsername", out SmtpServerUsername);
			AddSetting<String>("SmtpServerPassword", out SmtpServerPassword);
			AddSetting<String>("SendFrom", out SendFrom);
			AddSetting<Int32?>("SmtpServerPort", out SmtpServerPort);
		}
	}
}
