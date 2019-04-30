using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Blesmol {
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer {
		public ProjectInstaller() {
			InitializeComponent();
		}

		private void ServiceInstaller1_AfterInstall(Object sender, InstallEventArgs e) {
			using (ServiceController sc = new ServiceController("Blesmol")) {
				sc.Start();
			}
		}

		private void ServiceProcessInstaller1_AfterInstall(Object sender, InstallEventArgs e) {
			
		}
	}
}
