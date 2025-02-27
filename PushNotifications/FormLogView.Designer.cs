namespace PushNotifications
{
	partial class FormLogView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rtb_log = new RichTextBox();
            rb_logProjectMonitorPrinter = new RadioButton();
            rb_logEventPrinter = new RadioButton();
            SuspendLayout();
            // 
            // rtb_log
            // 
            rtb_log.Location = new Point(12, 43);
            rtb_log.Name = "rtb_log";
            rtb_log.Size = new Size(776, 395);
            rtb_log.TabIndex = 0;
            rtb_log.Text = "";
            // 
            // rb_logProjectMonitorPrinter
            // 
            rb_logProjectMonitorPrinter.AutoSize = true;
            rb_logProjectMonitorPrinter.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
            rb_logProjectMonitorPrinter.Location = new Point(12, 12);
            rb_logProjectMonitorPrinter.Name = "rb_logProjectMonitorPrinter";
            rb_logProjectMonitorPrinter.Size = new Size(360, 25);
            rb_logProjectMonitorPrinter.TabIndex = 1;
            rb_logProjectMonitorPrinter.Text = "Журнал работы пользователя с принтером";
            rb_logProjectMonitorPrinter.UseVisualStyleBackColor = true;
            rb_logProjectMonitorPrinter.CheckedChanged += rb_CheckedChanged;
            // 
            // rb_logEventPrinter
            // 
            rb_logEventPrinter.AutoSize = true;
            rb_logEventPrinter.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
            rb_logEventPrinter.Location = new Point(378, 12);
            rb_logEventPrinter.Name = "rb_logEventPrinter";
            rb_logEventPrinter.Size = new Size(240, 25);
            rb_logEventPrinter.TabIndex = 2;
            rb_logEventPrinter.Text = "Журнал ошибок приложений";
            rb_logEventPrinter.UseVisualStyleBackColor = true;
            rb_logEventPrinter.CheckedChanged += rb_CheckedChanged;
            // 
            // FormLogView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(rb_logEventPrinter);
            Controls.Add(rb_logProjectMonitorPrinter);
            Controls.Add(rtb_log);
            Name = "FormLogView";
            Text = "Логи приложения";
            FormClosed += FormLogView_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtb_log;
        private RadioButton rb_logProjectMonitorPrinter;
        private RadioButton rb_logEventPrinter;
    }
}