using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Specialized;
using System.Diagnostics;

namespace PushNotifications
{
	public class MonitorEventByPrinter
	{
		private EventLogWatcher ELW = null;
		public MonitorEventByPrinter()
		{
			EventLogQuery query = new EventLogQuery("Microsoft-Windows-PrintService/Operational", PathType.LogName);
			ELW = new EventLogWatcher(query);
			ELW.Enabled = true;
		}

		public void SetSubscription(bool value)
		{
			if (value)
			{
				ELW.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(MyOnEntryWritten);
			}
			else
			{
				ELW.EventRecordWritten -= new EventHandler<EventRecordWrittenEventArgs>(MyOnEntryWritten);
			}
		}
        List<JobPrinter> myQueue = new List<JobPrinter>();//Создать свою очередь и сравнить её с очередью винды
        private void CheckQueue(object? message)
		{
			//На данном этапе, задание находится в очереди. Мы ожидаем код 307 (печать) 20 секунд.
            bool checkEvent = false;
            EventRecord er = (message as EventRecord);
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(2000);
                for (global::System.Int32 j = 0; j < eventRecords307.Count; j++)
                {
					if (eventRecords307[j].Properties[0].Value.ToString() == er.Properties[0].Value.ToString())
					{
						checkEvent = true;
						er = eventRecords307[j];
                        break;
                    }
                }
				if (checkEvent) break;
            }

			//Если код 307 прилетел, то действуем по стандартной схеме
			if (checkEvent)
			{
				if (er.Properties[4].Value.ToString() == StaticClassGlobalValue.PrinterName)
				{
					//string idJobThread = 
					List<JobPrinter> queue = CheckQueuePrinter();
					if (queue.Count > 0) //Штатная ситуация, когда очередь изначально была пустая
					{
						JobPrinter JobCurrentThread = new JobPrinter() { JobId = er.Properties[0].Value.ToString(), StartTime = er.TimeCreated.Value };

						for (global::System.Int32 i = 0; i < queue.Count; i++)
						{
							if (queue[i].JobId == JobCurrentThread.JobId) //Нулевое свойство это id задачи
							{
								JobCurrentThread.JobName = queue[i].JobName;
								JobCurrentThread.Document = queue[i].Document;
							}
						}

						Thread.Sleep(30000);
						queue = CheckQueuePrinter();

						for (int i = 0; i < queue.Count; i++)
						{
							if (queue[i].JobId == JobCurrentThread.JobId)
							{
								LogRecord lr = new LogRecord();
								lr.DateTimeLog = DateTime.Now;
								lr.TaskInfo = $"Печать документа '{JobCurrentThread.Document}'";
								lr.Message = $"Задание с идентификатором {JobCurrentThread.JobId} при печати документа {JobCurrentThread.Document} не пропало из очереди после ожидания 30 секунд... ";
								lr.Description = "Почистите очередь печати, проверьте принтер на отсутствие бумаги или наличия замятия (позвоните на склад). Если Вы по какой либо причине не можете это сделать, обратитесь к системному администратору.";
								lr.StatusMessage = StatusCode.Error;
								lr.SendLogInFile();//Записываем лог в файл
								StatusPrintForm SPF = new StatusPrintForm(lr); //После 30 секунд простоя
								SPF.ShowDialog();
								return; //Выходим нафиг из программы
							}
						}
						SetMessageInMattermost(er); //Отправляем уведомление в матермост
					}
					else
					{
						SetMessageInMattermost(er); //Отправляем уведомление в матермост
					}
				}
			}
			else //Если код 307 не прилетел, то отправляем ошибку и завершаем поток
			{
				LogRecord lr = new LogRecord();
				lr.DateTimeLog = DateTime.Now;
				lr.TaskInfo = $"Печать документа (постановка документа в очередь печати)";
				lr.Message = $"Задание с идентификатором {er.Id} попало в очередь и осталось там спустя 20 секунд ожидания.";
				lr.Description = "Если Вы печатали задание на складской принтер, то при виде этого окна выполните РЕКОМЕНДАЦИИ. " +
					"Если Вы печатали документ на любой ДРУГОЙ принтер, то либо проигнорируйте данное сообщение, либо при наличии " +
					"проблем выполните РЕКОМЕНДАЦИИ. РЕКОМЕНДАЦИИ: попробуйте очистить очередь печати, проверить принтер на отсутствие " +
					"бумаги или наличия замятия (позвонить на склад) или обратиться к системному администратору за помощью";
				lr.StatusMessage = StatusCode.Error;
				lr.SendLogInFile();//Записываем лог в файл
				StatusPrintForm SPF = new StatusPrintForm(lr); //После 30 секунд простоя
				SPF.ShowDialog();
				return; //Выходим нафиг из программы
            }
        }

		private void SetMessageInMattermost(EventRecord message)
		{
            Mattermost M = new Mattermost("http://mm.luchllc.ru:8065/hooks/7y6aqqtg73d1fbsgh3cf17y4do");
            string mail = CreateStringInfo((message as EventRecord).Properties);
            M.SendMail(mail);
        }
        //opCode это код операции 
        // 11 - Операция успешно выполнена
        // 1 - Операция запускается 
		public List<EventRecord> eventRecords307 = new List<EventRecord>();
        public void MyOnEntryWritten(object source, EventRecordWrittenEventArgs e)
		{
			try
			{
                EventRecord er = e.EventRecord; 
                if (er.Id == 800) //Код 800 означает, что есть намерение о печати
                {
					Thread CheckQueueThread = new Thread(CheckQueue);
                    CheckQueueThread.Start(er);

                }
				if (er.Id == 307)
				{
                    eventRecords307.Add(er); //Если прилетел код 307, то мы его записываем в общий список
                }
				else if (er.LevelDisplayName == "Ошибка")
				{

					//if (er.Id == 809)
					//{
					//	//
					//}
					//else if (er.Id == 810)
					//{
					//	//
					//}
					//else if (er.Id == 812)
					//{
					//	//
					//}
					//else if (er.Id == 368)
					//{ 
					//	//
					//}
					LogRecord lr = new LogRecord();
					if (er.TimeCreated != null)
					{
						lr.DateTimeLog = Convert.ToDateTime(er.TimeCreated); //Возможно, тут стоит организовать проверку на Null
					}
					else
					{
						lr.DateTimeLog = DateTime.Now;
					}


					lr.TaskInfo = $"{er.TaskDisplayName} {er.TimeCreated}";
					lr.Message = er.OpcodeDisplayName;
					lr.Description = $"Ошибка произошла при печати документа и была отмечена в журнале PrintService (код - {er.Id}, время - {er.TimeCreated}) по пути 'Журналы приложений и служб - Microsoft - Windows - PrintService - Работает'";
					lr.StatusMessage = StatusCode.Error;
					lr.SendLogInFile();
					StatusPrintForm EPF = new StatusPrintForm(lr);
					EPF.ShowDialog();
				}
            }
			catch (Exception ex)
			{
                LogRecord lr = new LogRecord();
				lr.DateTimeLog = DateTime.Now;
                lr.TaskInfo = $"Обработка события из журнала PrintService";
                lr.Message = ex.Message;
                lr.Description = $"Ошибка произошла при обработке события, отмеченного в журнале PrintService";
                lr.StatusMessage = StatusCode.Error;
                lr.SendLogInFile();
                StatusPrintForm SPF = new StatusPrintForm(lr);
                SPF.ShowDialog();
            }
            	



			//BeginInvoke(new Action(() =>
			//{
			//	listBox1.Items.Add("********************Новое событие от принтера********************");
			//	EventRecord er = e.EventRecord;
			//	listBox1.Items.Add(er.TimeCreated.ToString());  
			//	listBox1.Items.Add(er.MachineName);
			//	listBox1.Items.Add(er.TaskDisplayName);
			//	listBox1.Items.Add(er.LevelDisplayName);
			//	listBox1.Items.Add(er.OpcodeDisplayName);
			//	listBox1.Items.Add("");
			//}));
		}

		public class JobPrinter
		{ 
			public string JobName { get; set; }
			public string JobId { get; set; }
			public string Document { get; set; }
			public DateTime StartTime { get; set; }
		}



		private List<JobPrinter> CheckQueuePrinter()
		{
            //string printerName = "Принтер склад";

            List<JobPrinter> printJobCollection = new List<JobPrinter>();
            string searchQuery = "SELECT * FROM Win32_PrintJob";

            /*searchQuery can also be mentioned with where Attribute,
                but this is not working in Windows 2000 / ME / 98 machines 
                and throws Invalid query error*/
            ManagementObjectSearcher searchPrintJobs =
                      new ManagementObjectSearcher(searchQuery);
            ManagementObjectCollection prntJobCollection = searchPrintJobs.Get();

			List<JobPrinter> result = (from ManagementObject mo in prntJobCollection.OfType<ManagementObject>() select new JobPrinter { JobName = mo.Properties["Name"].Value.ToString().Split(',')[0], JobId = mo.Properties["JobId"].Value.ToString(), Document = mo.Properties["Document"].Value.ToString(), StartTime = Convert.ToDateTime(mo.Properties["StartTime"].Value) }).ToList();

			result.OrderBy(x=>x.StartTime);


            foreach (var prntJob in result)
            {
                //System.String jobName = prntJob.JobName;

                //Job name would be of the format [Printer name], [Job ID]
                //char[] splitArr = new char[1];
                //splitArr[0] = Convert.ToChar(",");
                //string prnterName = jobName.Split(splitArr)[0];
				//string documentName = prntJob.Document;
                //string jobId = prntJob.JobId;

                if (System.String.Compare(prntJob.JobName, StaticClassGlobalValue.PrinterName, true) == 0)
                {
					printJobCollection.Add(prntJob);
                }
            }
            return printJobCollection;
        }

		private string FindEmployeeFIO(string nameUser)
		{
			foreach (var Employee in StaticClassGlobalValue.EmployeesData)
			{
				if (Employee.NameUser.ToLower() == nameUser.ToLower())
				{ 
					return Employee.FIOEmployee;
				}
			}
			return "'Неизвестный сотрудник'";
		}

		private string CreateStringInfo(IList<EventProperty> events)
		{
			return $"ВНИМАНИЕ! ПОЛЬЗОВАТЕЛЬ {FindEmployeeFIO(events[2].Value.ToString())} РАСПЕЧАТАЛ НОВОЕ ЗАДАНИЕ НА ПРИНТЕРЕ! ПРОВЕРЬТЕ ПОЖАЛУЙСТА.\nПодробнее:выполняется операция '{events[1].Value}' на компьютере {events[3].Value.ToString().Replace("\\", "")} пользователем {events[2].Value} через порт {events[5].Value} на принтер '{events[4].Value}'. Размер в байтах: {events[6].Value}. Страниц напечатано: {events[7].Value}.";
			//return $"Выполняется операция '{events[1].Value}' на компьютере {events[3].Value.ToString().Replace("\\","")} пользователем {events[2].Value} ({FindEmployeeFIO(events[2].Value.ToString())}) через порт {events[5].Value} на принтер '{events[4].Value}'. Размер в байтах: {events[6].Value}. Страниц напечатано: {events[7].Value}.";
		}

		//List<Employees> EmployeesData = new List<Employees>();
		//EmployeesData.Add(new Employees() {NumberComputer = 2078, NameComputer="luch-2078", NameUser = "user-2078", FIOEmployee = "Саранча Вадим Павлович" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2025, NameComputer = "luch-34", NameUser = "ermolenko", FIOEmployee = "Ермоленко Иван Сергеевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2063, NameComputer = "luch-2063", NameUser = "manager-46", FIOEmployee = "Демченко Яна Анатольевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2055, NameComputer = "luch-2055", NameUser = "buh2-luch", FIOEmployee = "Домогацкая Юлия Юрьевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2076, NameComputer = "luch-2076", NameUser = "user-2076", FIOEmployee = "Сальников Олег Александрович" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2065, NameComputer = "luch-2065", NameUser = "user-2065", FIOEmployee = "Таранов Александр Андреевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2070, NameComputer = "luch-2070", NameUser = "user-2070", FIOEmployee = "Червякова Лиана Леонидовна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2073, NameComputer = "luch-2073", NameUser = "user-2073", FIOEmployee = "Руденко Ольга Александровна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2085, NameComputer = "luch-2085", NameUser = "user-2085", FIOEmployee = "Лукьянов Роман Олегович" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2051, NameComputer = "luch-2051", NameUser = "manager-35", FIOEmployee = "Панина Анастасия Александровна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2084, NameComputer = "luch-2084", NameUser = "user-2084", FIOEmployee = "Шалихманова Ольга Николаевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2074, NameComputer = "luch-2074", NameUser = "user-2074", FIOEmployee = "Карпов Никита Андреевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2067, NameComputer = "luch-2067", NameUser = "user-2067", FIOEmployee = "Луценко Сергей Валентинович" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2079, NameComputer = "luch-2079", NameUser = "user-2079-2", FIOEmployee = "Алипова Ольга Николаевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2062, NameComputer = "luch-2062", NameUser = "manager-43", FIOEmployee = "Новикова Елена Анатольевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2054, NameComputer = "luch-2054", NameUser = "manager-38", FIOEmployee = "Вериницын Денис Андреевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2086, NameComputer = "luch-2086", NameUser = "user-2086", FIOEmployee = "Дарьев Николай Евгеньевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2058, NameComputer = "luch-2058", NameUser = "manager-6", FIOEmployee = "Жмурин Александр Алексеевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2008, NameComputer = "LUCH-2008", NameUser = "user-2008", FIOEmployee = "Петрова Алина Дмитриевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2060, NameComputer = "luch-2060", NameUser = "manager-42", FIOEmployee = "Никишенко Андрей Андреевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2071, NameComputer = "luch-2071", NameUser = "user-2071", FIOEmployee = "Симоненко Захар Васильевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2027, NameComputer = "luch-2027", NameUser = "user-2027", FIOEmployee = "Фабрый Михаил Михайлович" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2020, NameComputer = "LUCH-601-1", NameUser = "machula", FIOEmployee = "Мачула Анастасия Евгеньевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2066, NameComputer = "luch-2066", NameUser = "user-2066", FIOEmployee = "Макеева Екатерина Григорьевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2081, NameComputer = "luch-2081", NameUser = "user-2081", FIOEmployee = "Жмурина Валерия Алексеевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2015, NameComputer = "LUCH-22", NameUser = "dolgaleva", FIOEmployee = "Зинченко Дмитрий Федорович" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2059, NameComputer = "luch-2059", NameUser = "manager-39", FIOEmployee = "Мищенко Евгений Николаевич" });
		//EmployeesData.Add(new Employees() { NumberComputer = 2083, NameComputer = "luch-2083", NameUser = "user-2083", FIOEmployee = "Шудрик Наталья Николаевна" });
		//EmployeesData.Add(new Employees() { NumberComputer = 1022, NameComputer = "atd-1022", NameUser = "user-1022", FIOEmployee = "Грушко Владимир Иванович" });

	}





}
