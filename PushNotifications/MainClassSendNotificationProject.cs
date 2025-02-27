using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotifications
{
	public class MainClassSendNotificationProject
	{
		MonitorEventByPrinter MEBP = null;
		public MainClassSendNotificationProject()
		{
			try
			{
				MEBP = new MonitorEventByPrinter();
				NotifyIcon NI = new NotifyIcon();
				NI.Text = "Мониторинг событий от принтера";
				NI.Icon = new Icon($"{AppDomain.CurrentDomain.BaseDirectory}\\ico\\notification.ico");

				ContextMenuStrip CMS = new ContextMenuStrip();
			
				ToolStripMenuItem Subscribe = new ToolStripMenuItem("Подписаться на уведомления");
				ToolStripMenuItem Unsubscribe = new ToolStripMenuItem("Отписаться от уведомлений");
				ToolStripMenuItem ViewLog = new ToolStripMenuItem("Посмотреть логи");
				ToolStripMenuItem Close = new ToolStripMenuItem("Выйти из программы");
				CMS.Items.AddRange(new[] { Subscribe, Unsubscribe, ViewLog, Close });

				Subscribe.Click += (object sender, EventArgs e) => 
				{
					MEBP.SetSubscription(true);
                    Subscribe.Enabled = false;
					Unsubscribe.Enabled = true;

                };
				Unsubscribe.Click += (object sender, EventArgs e) =>
				{
					MEBP.SetSubscription(false);
                    Subscribe.Enabled = true;
                    Unsubscribe.Enabled = false;
                };
				ViewLog.Click += (object sender, EventArgs e) =>
				{
					FormLogView FLV = new FormLogView(NI);
					FLV.ShowDialog();
				};
				Close.Click += (object sender, EventArgs e) =>
				{
					Application.Exit();
				};

				NI.ContextMenuStrip = CMS;
				NI.Visible = true;
				MEBP.SetSubscription(true);//Подписываемся на событие сразу
                Subscribe.Enabled = false; //Блокируем кнопку подписки
                Unsubscribe.Enabled = true;//Разблокируем кнопку отписки

            }
			catch (Exception ex)
			{
                LogRecord lr = new LogRecord();
                lr.DateTimeLog = DateTime.Now;
                lr.TaskInfo = $"Запуск и стартовая настройка приложения";
                lr.Message = ex.Message;
                lr.Description = $"Ошибка произошла при запуске программы";
                lr.StatusMessage = StatusCode.Error;
                lr.SendLogInFile();//Записываем лог в файл
                StatusPrintForm SPF = new StatusPrintForm(lr);
                SPF.ShowDialog();

            }

		}
	}
}
