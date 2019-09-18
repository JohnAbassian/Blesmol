using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Blesmol.Core;

namespace Blesmol_SMTP {
	public class BlesmolSMTPNotifier : INotifier {
		public  String Message{ get; set; }
		public String Subject { get; set; }

		private readonly SMTPSettings _Settings;
		private readonly List<String> _Recipients;

		/// <inheritdoc />
		public void Notify(){
			SmtpClient mail = new SmtpClient(_Settings.Server, _Settings.Port) {
				UseDefaultCredentials = false,
				Credentials = new System.Net.NetworkCredential(_Settings.Username, _Settings.Password),
				EnableSsl = _Settings.UseSSLTLS
			};

			foreach (MailMessage message in _Recipients.Select(email => new MailMessage(_Settings.Username, email,
				Subject,
				Message) {
				IsBodyHtml = true
			})) {
				mail.Send(message);
			}
		}

		public BlesmolSMTPNotifier(SMTPSettings settings, List<String> recipient, String message, String subject){
			_Settings = settings;
			_Recipients = recipient ?? new List<String>();
			Message = message ?? String.Empty;
			Subject= subject ?? String.Empty;
		}

		public void AddRecipient(String email) => _Recipients.Add(email);



	}
}
