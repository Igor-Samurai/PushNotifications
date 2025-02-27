using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace PushNotifications
{
	public class Mail
	{
        string MailFrom = string.Empty;
        string NameSender = string.Empty;
		string PasswordMailFrom = string.Empty;
		string MailTo = string.Empty;
        bool IsHTML = false;
        bool IsSSL = false;

		public Mail(string mailFrom, string nameSender, string passwordMailFrom, string mailTo, bool isHTML, bool isSSL)
		{
			MailFrom = mailFrom;
			NameSender = nameSender;
			PasswordMailFrom = passwordMailFrom;
			MailTo = mailTo;
			IsHTML = isHTML;
			IsSSL = isSSL;
		}
		//system1@luchllc.ru
		//1w2eergtfte2
        //true
        //false
		public void SendMail(string subject, string body)
		{
            MailAddress from = new MailAddress(MailFrom, NameSender);
            // кому отправляем
            MailAddress to = new MailAddress(MailTo);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = subject;
            // текст письма
            m.Body = $"<h2>{body}</h2>";
            // письмо представляет код html
            m.IsBodyHtml = IsHTML;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("mail.luchllc.ru", 25);
            // логин и пароль
            smtp.Credentials = new NetworkCredential(MailFrom, PasswordMailFrom);
            smtp.EnableSsl = IsSSL;
            smtp.Send(m);
        }
	}
} 
