namespace PushNotifications
{
    partial class StatusPrintForm
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
            pb_status = new PictureBox();
            l_message = new Label();
            l_task = new Label();
            l_description = new Label();
            ((System.ComponentModel.ISupportInitialize)pb_status).BeginInit();
            SuspendLayout();
            // 
            // pb_status
            // 
            pb_status.Location = new Point(12, 12);
            pb_status.Name = "pb_status";
            pb_status.Size = new Size(186, 209);
            pb_status.SizeMode = PictureBoxSizeMode.Zoom;
            pb_status.TabIndex = 0;
            pb_status.TabStop = false;
            // 
            // l_message
            // 
            l_message.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            l_message.Location = new Point(204, 46);
            l_message.Name = "l_message";
            l_message.Size = new Size(614, 41);
            l_message.TabIndex = 2;
            l_message.Text = "Тут будет само сообщение";
            // 
            // l_task
            // 
            l_task.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 204);
            l_task.Location = new Point(204, 12);
            l_task.Name = "l_task";
            l_task.Size = new Size(614, 23);
            l_task.TabIndex = 3;
            l_task.Text = "Тут будет задача и её время";
            l_task.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // l_description
            // 
            l_description.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            l_description.Location = new Point(204, 99);
            l_description.Name = "l_description";
            l_description.Size = new Size(614, 122);
            l_description.TabIndex = 4;
            l_description.Text = "Тут будет описание или советы по устранению ошибки";
            // 
            // StatusPrintForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(830, 233);
            Controls.Add(l_description);
            Controls.Add(l_task);
            Controls.Add(l_message);
            Controls.Add(pb_status);
            Name = "StatusPrintForm";
            Text = "Ошибка печати на складской принтер";
            ((System.ComponentModel.ISupportInitialize)pb_status).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pb_status;
        private Label l_message;
        private Label l_task;
        private Label l_description;
    }
}