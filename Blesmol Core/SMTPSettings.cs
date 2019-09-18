using System;
using System.Collections.Generic;
using System.Text;

namespace Blesmol.Core {
	public class SMTPSettings {
		public String Server { get; set; }
		public Int32 Port { get; set; }
		public String Username { get; set; }
		public String Password { get; set; }
		public Boolean UseSSLTLS { get; set; }

	}
}
