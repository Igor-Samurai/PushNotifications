namespace PushNotifications
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            listBox1 = new ListBox();
            button2 = new Button();
            button3 = new Button();
            btn_copy = new Button();
            btn_getAllLog = new Button();
            btn_getEventByID = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 41);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(1056, 379);
            listBox1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Location = new Point(93, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(174, 12);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 3;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // btn_copy
            // 
            btn_copy.Location = new Point(255, 12);
            btn_copy.Name = "btn_copy";
            btn_copy.Size = new Size(146, 23);
            btn_copy.TabIndex = 4;
            btn_copy.Text = "Скопировать всё";
            btn_copy.UseVisualStyleBackColor = true;
            btn_copy.Click += btn_copy_Click;
            // 
            // btn_getAllLog
            // 
            btn_getAllLog.Location = new Point(407, 12);
            btn_getAllLog.Name = "btn_getAllLog";
            btn_getAllLog.Size = new Size(174, 23);
            btn_getAllLog.TabIndex = 5;
            btn_getAllLog.Text = "Перебрать журналы";
            btn_getAllLog.UseVisualStyleBackColor = true;
            btn_getAllLog.Click += btn_getAllLog_Click;
            // 
            // btn_getEventByID
            // 
            btn_getEventByID.Location = new Point(587, 12);
            btn_getEventByID.Name = "btn_getEventByID";
            btn_getEventByID.Size = new Size(304, 23);
            btn_getEventByID.TabIndex = 6;
            btn_getEventByID.Text = "Подписаться на события от принтера";
            btn_getEventByID.UseVisualStyleBackColor = true;
            btn_getEventByID.Click += btn_getEventByID_Click;
            // 
            // button4
            // 
            button4.Location = new Point(897, 12);
            button4.Name = "button4";
            button4.Size = new Size(171, 23);
            button4.TabIndex = 7;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(12, 426);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 8;
            button5.Text = "button5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(93, 426);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 9;
            button6.Text = "button6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 468);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(btn_getEventByID);
            Controls.Add(btn_getAllLog);
            Controls.Add(btn_copy);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
		private ListBox listBox1;
		private Button button2;
		private Button button3;
		private Button btn_copy;
		private Button btn_getAllLog;
		private Button btn_getEventByID;
		private Button button4;
        private Button button5;
        private Button button6;
    }
}
