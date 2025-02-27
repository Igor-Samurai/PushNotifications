using System.Diagnostics;
using System.Xml.Serialization;

namespace PushNotifications
{

	internal static class Program
	{
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
            var curId = Process.GetCurrentProcess().Id;
            var procName = Process.GetCurrentProcess().ProcessName;
            var processes = Process.GetProcesses().Where(p => p.ProcessName == procName && p.Id != curId).ToList();
            if (processes.Count < 1)
            {
                //if (!Process.GetProcessesByName("PushNotifications").Any())
                //{
                StaticClassGlobalValue.logEventPrinter = new CreateRecordInFileXML($"\\\\192.168.7.148\\printer_logs\\{Environment.MachineName}_log_printer.xml");
                StaticClassGlobalValue.logProjectMonitorPrinter = new CreateRecordInFileXML($"\\\\192.168.7.148\\printer_logs\\{Environment.MachineName}_log_program.xml");
                StaticClassGlobalValue.PrinterName = StaticClassGlobalValue.logEventPrinter.ReadPrinterName();//Очень плохой код
                try
                {
                    CreateRecordInFileXML CRIFXML = new CreateRecordInFileXML("\\\\192.168.7.148\\printer_logs\\EmployeesData.xml");
                    StaticClassGlobalValue.EmployeesData = CRIFXML.ReadEmployeesFromXML();
                    if ((StaticClassGlobalValue.EmployeesData == null) || (StaticClassGlobalValue.EmployeesData.Count == 0))
                    {
                        StaticClassGlobalValue.StatusLoadEmployeesData = false;
                    }

                }
                catch (Exception ex)
                {
                    LogRecord lr = new LogRecord();
                    lr.DateTimeLog = DateTime.Now;
                    lr.TaskInfo = $"Считываение данных о пользователях";
                    lr.Message = ex.Message;
                    lr.Description = $"Ошибка произошла при считывании данных о пользователях из файла, расположенного по пути \\\\192.168.7.148\\printer_logs\\EmployeesData.xml";
                    lr.StatusMessage = StatusCode.Error;
                    lr.SendLogInFile();//Записываем лог в файл
                    StatusPrintForm SPF = new StatusPrintForm(lr);
                    SPF.ShowDialog();
                }

                MainClassSendNotificationProject MCSNP = new MainClassSendNotificationProject();
                Application.Run();
            }
            else
            {
                MessageBox.Show("Программа для оповещения склада о печати заданий уже запущена!");
            }
			
		}
	}


}