using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace PushNotifications
{
	public class Mattermost
	{
		public string URLChanel { get; set; } = "http://mm.luchllc.ru:8065/hooks/7y6aqqtg73d1fbsgh3cf17y4do";

		public Mattermost(string uRLChanel)
		{
			URLChanel = uRLChanel;
		}

		public async void SendMail(string message)
		{
			string requestText = "{\"text\" :\"" + message + "\"}";

            try
			{
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URLChanel);
                request.Content = new StringContent(requestText);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Можно закомментировать эти три строчки, чтобы не отправлялись уведомления
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                LogRecord lr = new LogRecord();
                lr.DateTimeLog = DateTime.Now;
                lr.TaskInfo = $"Уведомление об успешной печати на складской принтер";
                lr.Message = $"Журнал Windows оповестил о печати документа";
                lr.Description = $"Было отправлено сообщение: {message}";
                lr.StatusMessage = StatusCode.Success;
                lr.SendLogInFile();//Записываем лог в файл
                StatusPrintForm SPF = new StatusPrintForm(lr);
                SPF.ShowDialog();
            }
			catch (Exception ex)
			{
                LogRecord lr = new LogRecord();
                lr.DateTimeLog = DateTime.Now;
                lr.TaskInfo = $"Отправка уведомления в чат Mattermost";
                lr.Message = ex.Message;
                lr.Description = $"Документ успешно распечатан, но уведомление на ТСД не поступило, поэтому РАБОТНИКИ СКЛАДА МОГУТ НЕ УЗНАТЬ о печати документа. Ошибка произошла при отправке запроса в чат Mattermost. Текст запроса: {requestText}";
                lr.StatusMessage = StatusCode.Error;
                lr.SendLogInFile();//Записываем лог в файл
                StatusPrintForm SPF = new StatusPrintForm(lr);
                SPF.ShowDialog();
                throw;
            }
		}
	}
}
