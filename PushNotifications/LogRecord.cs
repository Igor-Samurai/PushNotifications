using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotifications
{
	public class LogRecord
	{
		public LogRecord() 
		{

		}

		public DateTime DateTimeLog { get; set; } //Дата и время сообщения
		public StatusCode StatusMessage { get; set; } //Статус сообщения (просто отчет, или ошибка)
		public string TaskInfo { get; set; } //Какая задача выполнялась, когда возникло сообщение
		public string Message { get; set; } //Собственно само сообщение
		public string Description { get; set; } //Подробное описание сообщения

		public void SendLogInFile()
		{
            StaticClassGlobalValue.logEventPrinter.AddLogInFile(this);
        }

	}
}
