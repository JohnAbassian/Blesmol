using System;
using System.Windows.Forms;
using Blesmol;
using Blesmol.Registry;

namespace Blesmol_Frontend {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FrmMain(new Settings(true)));
		}
	}
}
