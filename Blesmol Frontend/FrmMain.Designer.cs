namespace Blesmol {
	partial class FrmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			this.panelHeader = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.labelTitle = new System.Windows.Forms.Label();
			this.panelViewControls = new System.Windows.Forms.Panel();
			this.btnOptions = new System.Windows.Forms.Button();
			this.btnEmail = new System.Windows.Forms.Button();
			this.btnDisks = new System.Windows.Forms.Button();
			this.panelDisks = new System.Windows.Forms.Panel();
			this.txtAmount = new System.Windows.Forms.TextBox();
			this.clbDisks = new System.Windows.Forms.CheckedListBox();
			this.cboUnit = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panelEmail = new System.Windows.Forms.Panel();
			this.txtEmails = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panelOptions = new System.Windows.Forms.Panel();
			this.lblSleepInterval = new System.Windows.Forms.Label();
			this.txtSleepInterval = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.txtEmailDelay = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtMachineName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtSmtpServer = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtSendFrom = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.panelHeader.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panelViewControls.SuspendLayout();
			this.panelDisks.SuspendLayout();
			this.panelEmail.SuspendLayout();
			this.panelOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelHeader
			// 
			this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(67)))), ((int)(((byte)(70)))));
			this.panelHeader.Controls.Add(this.pictureBox1);
			this.panelHeader.Controls.Add(this.labelTitle);
			this.panelHeader.Location = new System.Drawing.Point(0, 0);
			this.panelHeader.Name = "panelHeader";
			this.panelHeader.Size = new System.Drawing.Size(1228, 47);
			this.panelHeader.TabIndex = 1;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(-3, -8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(62, 57);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// labelTitle
			// 
			this.labelTitle.AutoSize = true;
			this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(201)))), ((int)(((byte)(227)))));
			this.labelTitle.Location = new System.Drawing.Point(56, 12);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(238, 24);
			this.labelTitle.TabIndex = 0;
			this.labelTitle.Text = "Blesmol Storage Monitor";
			// 
			// panelViewControls
			// 
			this.panelViewControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
			this.panelViewControls.Controls.Add(this.btnSave);
			this.panelViewControls.Controls.Add(this.btnOptions);
			this.panelViewControls.Controls.Add(this.btnEmail);
			this.panelViewControls.Controls.Add(this.btnDisks);
			this.panelViewControls.Location = new System.Drawing.Point(-2, 47);
			this.panelViewControls.Name = "panelViewControls";
			this.panelViewControls.Size = new System.Drawing.Size(396, 36);
			this.panelViewControls.TabIndex = 2;
			// 
			// btnOptions
			// 
			this.btnOptions.Location = new System.Drawing.Point(149, 6);
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.Size = new System.Drawing.Size(63, 23);
			this.btnOptions.TabIndex = 2;
			this.btnOptions.Text = "Options";
			this.btnOptions.UseVisualStyleBackColor = true;
			this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
			// 
			// btnEmail
			// 
			this.btnEmail.Location = new System.Drawing.Point(80, 6);
			this.btnEmail.Name = "btnEmail";
			this.btnEmail.Size = new System.Drawing.Size(63, 23);
			this.btnEmail.TabIndex = 1;
			this.btnEmail.Text = "Email";
			this.btnEmail.UseVisualStyleBackColor = true;
			this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
			// 
			// btnDisks
			// 
			this.btnDisks.Location = new System.Drawing.Point(11, 6);
			this.btnDisks.Name = "btnDisks";
			this.btnDisks.Size = new System.Drawing.Size(63, 23);
			this.btnDisks.TabIndex = 0;
			this.btnDisks.Text = "Disks";
			this.btnDisks.UseVisualStyleBackColor = true;
			this.btnDisks.Click += new System.EventHandler(this.btnDisks_Click);
			// 
			// panelDisks
			// 
			this.panelDisks.Controls.Add(this.txtAmount);
			this.panelDisks.Controls.Add(this.clbDisks);
			this.panelDisks.Controls.Add(this.cboUnit);
			this.panelDisks.Controls.Add(this.label1);
			this.panelDisks.Location = new System.Drawing.Point(-2, 83);
			this.panelDisks.Name = "panelDisks";
			this.panelDisks.Size = new System.Drawing.Size(306, 239);
			this.panelDisks.TabIndex = 3;
			// 
			// txtAmount
			// 
			this.txtAmount.Location = new System.Drawing.Point(188, 204);
			this.txtAmount.Name = "txtAmount";
			this.txtAmount.Size = new System.Drawing.Size(60, 20);
			this.txtAmount.TabIndex = 6;
			this.txtAmount.TextChanged += new System.EventHandler(this.TxtAmount_TextChanged);
			// 
			// clbDisks
			// 
			this.clbDisks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.clbDisks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.clbDisks.CheckOnClick = true;
			this.clbDisks.IntegralHeight = false;
			this.clbDisks.Location = new System.Drawing.Point(11, 10);
			this.clbDisks.Name = "clbDisks";
			this.clbDisks.ScrollAlwaysVisible = true;
			this.clbDisks.Size = new System.Drawing.Size(285, 179);
			this.clbDisks.TabIndex = 3;
			this.clbDisks.Click += new System.EventHandler(this.btnDisks_Click);
			this.clbDisks.SelectedValueChanged += new System.EventHandler(this.clbDisks_SelectedValueChanged);
			// 
			// cboUnit
			// 
			this.cboUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboUnit.FormattingEnabled = true;
			this.cboUnit.Items.AddRange(new object[] {
            "TB",
            "GB",
            "MB",
            "KB",
            "%"});
			this.cboUnit.Location = new System.Drawing.Point(252, 204);
			this.cboUnit.MaxDropDownItems = 3;
			this.cboUnit.Name = "cboUnit";
			this.cboUnit.Size = new System.Drawing.Size(40, 21);
			this.cboUnit.TabIndex = 5;
			this.cboUnit.SelectedIndexChanged += new System.EventHandler(this.cboUnit_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 208);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(172, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Alert when free space is lower than";
			// 
			// panelEmail
			// 
			this.panelEmail.Controls.Add(this.txtEmails);
			this.panelEmail.Controls.Add(this.label2);
			this.panelEmail.Location = new System.Drawing.Point(615, 83);
			this.panelEmail.Name = "panelEmail";
			this.panelEmail.Size = new System.Drawing.Size(306, 189);
			this.panelEmail.TabIndex = 6;
			// 
			// txtEmails
			// 
			this.txtEmails.Location = new System.Drawing.Point(13, 25);
			this.txtEmails.Multiline = true;
			this.txtEmails.Name = "txtEmails";
			this.txtEmails.Size = new System.Drawing.Size(280, 146);
			this.txtEmails.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(237, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Notify these email addresses: (comma seperated)";
			// 
			// panelOptions
			// 
			this.panelOptions.Controls.Add(this.lblSleepInterval);
			this.panelOptions.Controls.Add(this.txtSleepInterval);
			this.panelOptions.Controls.Add(this.label10);
			this.panelOptions.Controls.Add(this.label9);
			this.panelOptions.Controls.Add(this.txtEmailDelay);
			this.panelOptions.Controls.Add(this.txtPassword);
			this.panelOptions.Controls.Add(this.txtUsername);
			this.panelOptions.Controls.Add(this.label8);
			this.panelOptions.Controls.Add(this.label7);
			this.panelOptions.Controls.Add(this.txtPort);
			this.panelOptions.Controls.Add(this.label6);
			this.panelOptions.Controls.Add(this.txtMachineName);
			this.panelOptions.Controls.Add(this.label5);
			this.panelOptions.Controls.Add(this.txtSmtpServer);
			this.panelOptions.Controls.Add(this.label4);
			this.panelOptions.Controls.Add(this.txtSendFrom);
			this.panelOptions.Controls.Add(this.label3);
			this.panelOptions.Location = new System.Drawing.Point(628, 358);
			this.panelOptions.Name = "panelOptions";
			this.panelOptions.Size = new System.Drawing.Size(306, 239);
			this.panelOptions.TabIndex = 7;
			// 
			// lblSleepInterval
			// 
			this.lblSleepInterval.AutoSize = true;
			this.lblSleepInterval.Location = new System.Drawing.Point(14, 202);
			this.lblSleepInterval.Name = "lblSleepInterval";
			this.lblSleepInterval.Size = new System.Drawing.Size(117, 13);
			this.lblSleepInterval.TabIndex = 19;
			this.lblSleepInterval.Text = "Sleep Interval (minutes)";
			// 
			// txtSleepInterval
			// 
			this.txtSleepInterval.Location = new System.Drawing.Point(13, 216);
			this.txtSleepInterval.Name = "txtSleepInterval";
			this.txtSleepInterval.Size = new System.Drawing.Size(52, 20);
			this.txtSleepInterval.TabIndex = 17;
			this.txtSleepInterval.TextChanged += new System.EventHandler(this.TxtSleepInterval_TextChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(75, 183);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(43, 13);
			this.label10.TabIndex = 16;
			this.label10.Text = "minutes";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(9, 164);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(176, 13);
			this.label9.TabIndex = 15;
			this.label9.Text = "Delay between sending email alerts:";
			// 
			// txtEmailDelay
			// 
			this.txtEmailDelay.Location = new System.Drawing.Point(12, 180);
			this.txtEmailDelay.Name = "txtEmailDelay";
			this.txtEmailDelay.Size = new System.Drawing.Size(61, 20);
			this.txtEmailDelay.TabIndex = 14;
			this.txtEmailDelay.TextChanged += new System.EventHandler(this.txtEmailDelay_TextChanged);
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(162, 141);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(129, 20);
			this.txtPassword.TabIndex = 12;
			this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
			// 
			// txtUsername
			// 
			this.txtUsername.Location = new System.Drawing.Point(12, 141);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(129, 20);
			this.txtUsername.TabIndex = 11;
			this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(159, 125);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 13);
			this.label8.TabIndex = 13;
			this.label8.Text = "Password:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(10, 125);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(58, 13);
			this.label7.TabIndex = 12;
			this.label7.Text = "Username:";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(240, 102);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(52, 20);
			this.txtPort.TabIndex = 10;
			this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(237, 86);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(29, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Port:";
			// 
			// txtMachineName
			// 
			this.txtMachineName.Location = new System.Drawing.Point(12, 63);
			this.txtMachineName.Name = "txtMachineName";
			this.txtMachineName.Size = new System.Drawing.Size(280, 20);
			this.txtMachineName.TabIndex = 8;
			this.txtMachineName.TextChanged += new System.EventHandler(this.txtMachineName_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(10, 47);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(118, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "This computer is called:";
			// 
			// txtSmtpServer
			// 
			this.txtSmtpServer.Location = new System.Drawing.Point(13, 102);
			this.txtSmtpServer.Name = "txtSmtpServer";
			this.txtSmtpServer.Size = new System.Drawing.Size(206, 20);
			this.txtSmtpServer.TabIndex = 9;
			this.txtSmtpServer.TextChanged += new System.EventHandler(this.txtSmtpServer_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(10, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(187, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "SMTP server to send email alerts with:";
			// 
			// txtSendFrom
			// 
			this.txtSendFrom.Location = new System.Drawing.Point(12, 26);
			this.txtSendFrom.Name = "txtSendFrom";
			this.txtSendFrom.Size = new System.Drawing.Size(280, 20);
			this.txtSendFrom.TabIndex = 7;
			this.txtSendFrom.TextChanged += new System.EventHandler(this.txtSendFrom_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(186, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Alerts are sent from this email address:";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(217, 6);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1228, 670);
			this.Controls.Add(this.panelOptions);
			this.Controls.Add(this.panelEmail);
			this.Controls.Add(this.panelDisks);
			this.Controls.Add(this.panelViewControls);
			this.Controls.Add(this.panelHeader);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Blesmol Storage Monitor";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.panelHeader.ResumeLayout(false);
			this.panelHeader.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panelViewControls.ResumeLayout(false);
			this.panelDisks.ResumeLayout(false);
			this.panelDisks.PerformLayout();
			this.panelEmail.ResumeLayout(false);
			this.panelEmail.PerformLayout();
			this.panelOptions.ResumeLayout(false);
			this.panelOptions.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelHeader;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Panel panelViewControls;
		private System.Windows.Forms.Button btnOptions;
		private System.Windows.Forms.Button btnEmail;
		private System.Windows.Forms.Button btnDisks;
		private System.Windows.Forms.Panel panelDisks;
		private System.Windows.Forms.ComboBox cboUnit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckedListBox clbDisks;
		private System.Windows.Forms.Panel panelEmail;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelOptions;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtSendFrom;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtMachineName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtSmtpServer;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtEmailDelay;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtAmount;
		private System.Windows.Forms.TextBox txtSleepInterval;
		private System.Windows.Forms.Label lblSleepInterval;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox txtEmails;
		private System.Windows.Forms.Button btnSave;
	}
}

