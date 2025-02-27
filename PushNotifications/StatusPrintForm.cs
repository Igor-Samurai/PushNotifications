using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
using static PushNotifications.MonitorEventByPrinter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
namespace PushNotifications
{
    public partial class StatusPrintForm : Form
    {
        public StatusPrintForm(LogRecord lr)
        {
            InitializeComponent();
            this.Text = "Приложение перехвата события о печати документов на складской принтер и отправки уведомлений в Mattermost";
            l_task.Text = lr.TaskInfo;
            if (lr.StatusMessage == StatusCode.Error) //Ошибка очереди
            {
                pb_status.Image = Properties.Resources.i_error;
                //this.Text = "Ошибка при выполнении задачи";
            }
            else if (lr.StatusMessage == StatusCode.Success)//Успешное выполнение
            {
                pb_status.Image = Properties.Resources.i_success;
                //this.Text = "Успешное выполнение задачи";
            }
            else
            {
                pb_status.Image = Properties.Resources.i_info;
                //this.Text = "Информация о выполняемой задаче";
            }

            l_task.Text = $"{lr.TaskInfo} ({lr.DateTimeLog.ToString("dd.MM.yyyy HH:mm:ss")})";
            l_message.Text = lr.Message;
            l_description.Text = lr.Description;

            
        }

    }
}
