﻿using System;
using System.IO;
using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;

namespace Blesmol {
	public partial class Core : ServiceBase {
		private readonly Config c = new Config();
		private DateTime DoNotSendUntilAfter;
		private Thread WorkerThread;

		public Core() {
			InitializeComponent();
		}

		protected override void OnStart(String[] args) {
			c.LoadConfig();
			WorkerThread = new Thread(new ThreadStart(MonitorDisks)) {
				Name = "workerThread"
			};
			WorkerThread.Start();
		}

		private void MonitorDisks() {
			for (; ; ){
				Boolean SuppressAlerts;
				if (DoNotSendUntilAfter != null){
					TimeSpan ts = DateTime.Now - DoNotSendUntilAfter;
					SuppressAlerts = !(ts.TotalSeconds > 0);
				} else {
					SuppressAlerts = false;
				}

				if (!SuppressAlerts) {
					DriveInfo[] allDrives = DriveInfo.GetDrives();
					foreach (DriveInfo dirInfo in allDrives) {
						try {
							if (dirInfo.IsReady && c.DisksToMonitor.Contains(dirInfo.Name.Replace(@":\", "")) == true) {
								if (dirInfo.TotalFreeSpace < Utils.ConvertToBytes(c.ThresholdAmount, c.ThresholdUnit)) {
									SendAlerts(dirInfo.Name.Replace(@"\", ""), c.ThresholdAmount.ToString(), c.ThresholdUnit);
								}
							}
						} catch (Exception e){
							Exception a = e;
						}
					}
				}

				System.Threading.Thread.Sleep(System.TimeSpan.FromMinutes(c.SleepInterval));
			}
		}

		private void SendAlerts(String drive, String thresholdAmount, String thresholdUnit) {
			DoNotSendUntilAfter = DateTime.Now.AddMinutes(c.EmailDelay);

			SmtpClient mail = new SmtpClient(c.SmtpServer, Convert.ToInt32(c.SmtpServerPort)) {
				UseDefaultCredentials = false,
				Credentials = new System.Net.NetworkCredential(c.SmtpServerUsername, c.SmtpServerPassword),
				EnableSsl = true
			};

			foreach (String email in c.EmailAddresses) {
				MailMessage message = new MailMessage("alerts@reliant.org", email, "Warning! Disk space is low on " + c.MachineName, "<p style='font-family:helvetica,arial,sans-serif;font-size:83%;'>Disk space is low on " + c.MachineName + "<br/>Free space on " + drive + " is less than " + thresholdAmount + " " + thresholdUnit + "<br/><br/><small>This is an automated email generated by Blesmol</small></p>");
				message.IsBodyHtml = true;
					
				mail.Send(message);
			}

		}

		protected override void OnStop() {
			WorkerThread.Abort();
		}

		static void Main() {
#if DEBUG
			Core DebugService = new Core();
			DebugService.OnStart(null);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Core() 
			};
            ServiceBase.Run(ServicesToRun);
#endif
		}
	}
}