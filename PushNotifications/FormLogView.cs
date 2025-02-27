using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PushNotifications
{
    public partial class FormLogView : Form
    {
        private NotifyIcon NotifyIcon;
        public FormLogView(NotifyIcon NI)
        {
            InitializeComponent();
            NotifyIcon = NI;
            NotifyIcon.Visible = false;
        }

        private void FormLogView_FormClosed(object sender, FormClosedEventArgs e)
        {
            NotifyIcon.Visible = true;
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            List<LogRecord> records;
            if (rb_logProjectMonitorPrinter.Checked)
            {
                records = StaticClassGlobalValue.logEventPrinter.ReadLogsFromXML();
            }
            else
            {
                records = StaticClassGlobalValue.logProjectMonitorPrinter.ReadLogsFromXML();
            }
            rtb_log.Clear();

            if (records != null)
            {
                foreach (var record in records)
                {
                    if (record.StatusMessage == StatusCode.Success)
                    {
                        rtb_log.SelectionColor = Color.Green;
                    }
                    else if (record.StatusMessage == StatusCode.Error)
                    {
                        rtb_log.SelectionColor = Color.Red;
                    }
                    else if (record.StatusMessage == StatusCode.Info)
                    { 
                        
                    }

                    rtb_log.AppendText($"********************Сообщение {record.DateTimeLog}********************" + "\n");
                    rtb_log.AppendText(record.StatusMessage+"\n");
                    rtb_log.AppendText(record.TaskInfo + "\n");
                    rtb_log.AppendText(record.Message + "\n");
                    rtb_log.AppendText(record.Description + "\n");
                }
            }
        }
    }
}
