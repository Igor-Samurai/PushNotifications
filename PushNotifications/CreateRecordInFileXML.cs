using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PushNotifications
{
	public class CreateRecordInFileXML
	{
		const string PathInFileDateLastClear = "Какой-то путь";
		public string PathInFile { get; set; }

		public CreateRecordInFileXML(string pathInFile) 
		{
			PathInFile = pathInFile;
			if (!File.Exists(PathInFile))
			{
				//File.Create(PathInFile);
				
                XmlDocument xDoc = new XmlDocument();
				XmlDeclaration XD = xDoc.CreateXmlDeclaration("1.0", "utf-8","");
                xDoc.AppendChild(XD);

                XmlElement logsElem = xDoc.CreateElement("logs");
                xDoc.AppendChild(logsElem);
				xDoc.Save(PathInFile);
            }
		}

		public void AddLogInFile(LogRecord logRecord)
		{
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PathInFile);
            XmlElement? xRoot = xDoc.DocumentElement;

            XmlElement logRecordElem = xDoc.CreateElement("LogRecord");

            XmlElement dateTimeLogElem = xDoc.CreateElement("DateTimeLog");
            XmlElement statusMessageElem = xDoc.CreateElement("StatusMessage");
            XmlElement taskInfoElem = xDoc.CreateElement("TaskInfo");
            XmlElement messageElem = xDoc.CreateElement("Message");
            XmlElement descriptionElem = xDoc.CreateElement("Description");

            XmlText dateTimeLogText = xDoc.CreateTextNode(logRecord.DateTimeLog.ToString());
            string statusMessageString = "";
            if (logRecord.StatusMessage == StatusCode.Success)
            {
                statusMessageString = "Успех";
            }
            else if (logRecord.StatusMessage == StatusCode.Error)
            {
                statusMessageString = "Ошибка";
            }
            else if (logRecord.StatusMessage == StatusCode.Info)
            {
                statusMessageString = "Сведения";
            }
            XmlText statusMessageText = xDoc.CreateTextNode(statusMessageString);
            XmlText taskInfoText = xDoc.CreateTextNode(logRecord.TaskInfo);
            XmlText messageText = xDoc.CreateTextNode(logRecord.Message);
            XmlText descriptionText = xDoc.CreateTextNode(logRecord.Description);

            dateTimeLogElem.AppendChild(dateTimeLogText);
			statusMessageElem.AppendChild(statusMessageText);
			taskInfoElem.AppendChild(taskInfoText);
			messageElem.AppendChild(messageText);
			descriptionElem.AppendChild(descriptionText);

            logRecordElem.AppendChild(dateTimeLogElem);
			logRecordElem.AppendChild(statusMessageElem);
            logRecordElem.AppendChild(taskInfoElem);
            logRecordElem.AppendChild(messageElem);
            logRecordElem.AppendChild(descriptionElem);

			xRoot.AppendChild(logRecordElem);

            xDoc.Save(PathInFile);
        }

        //public void SaveInXML<T>(T record)
        //{

        //	XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        //	using (FileStream fs = new FileStream(PathInFile, FileMode.Append))
        //	{
        //		xmlSerializer.Serialize(fs, record);
        //	}
        //}

        private string PathInFileWithPrinterName = "\\\\192.168.7.148\\printer_logs\\NamePrinter.xml";
        public string ReadPrinterName()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PathInFileWithPrinterName);
            // получим корневой элемент
            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                return xRoot.InnerText;
            }
            else
            {
                return "Принтер склад";
            }
        }

        public List<Employees> ReadEmployeesFromXML()
        {
            try
            {
                List<Employees> records = new List<Employees>();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(PathInFile);
                // получим корневой элемент
                XmlElement? xRoot = xDoc.DocumentElement;
                if (xRoot != null)
                {
                    foreach (XmlElement xnode in xRoot)
                    {
                        Employees lr = new Employees();
                        foreach (XmlElement childnode in xnode.ChildNodes)
                        {
                            
                            if (childnode.Name == "NumberComputer")
                            {
                                lr.NumberComputer = Convert.ToInt32(childnode.InnerText);
                            }
                            if (childnode.Name == "NameComputer")
                            {
                                lr.NameComputer = childnode.InnerText;
                            }
                            if (childnode.Name == "NameUser")
                            {
                                lr.NameUser = childnode.InnerText;
                            }
                            if (childnode.Name == "FIOEmployee")
                            {
                                lr.FIOEmployee = childnode.InnerText;
                            }
                        }
                        records.Add(lr);
                    }
                }
                return records;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        public List<LogRecord> ReadLogsFromXML()
		{
			try
			{
				List<LogRecord> records = new List<LogRecord>();
				XmlDocument xDoc = new XmlDocument();
				xDoc.Load(PathInFile);
				// получим корневой элемент
				XmlElement? xRoot = xDoc.DocumentElement;
				if (xRoot != null)
				{
                    foreach (XmlElement xnode in xRoot)
                    {
                        LogRecord lr = new LogRecord();
                        foreach (XmlElement childnode in xnode.ChildNodes)
                        {
                            if (childnode.Name == "DateTimeLog")
                            {
                                lr.DateTimeLog = Convert.ToDateTime(childnode.InnerText);
                            }
                            if (childnode.Name == "StatusMessage")
                            {
                                string statusMessageString = childnode.InnerText;

                                if (statusMessageString == "Успех")
                                {
                                    lr.StatusMessage = StatusCode.Success;
                                }
                                else if (statusMessageString == "Ошибка")
                                {
                                    lr.StatusMessage = StatusCode.Error;
                                }
                                else if (statusMessageString == "Сведения")
                                {
                                    lr.StatusMessage = StatusCode.Info;
                                }
                            }
                            if (childnode.Name == "TaskInfo")
                            {
                                lr.TaskInfo = childnode.InnerText;
                            }
                            if (childnode.Name == "Message")
                            {
                                lr.Message = childnode.InnerText;
                            }
                            if (childnode.Name == "Description")
                            {
                                lr.Description = childnode.InnerText;
                            }
                        }
                        records.Add(lr);
                    }
				}
				return records;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return null;
			}
			
		}

		public void Clearlog()
		{
			using (FileStream fs = new FileStream(PathInFile, FileMode.Append))
			{
				fs.SetLength(0);
			}
		}

		//В этом файле должна была храниться дата последней чистки журнала
		private bool ReadDataClearFromFile()
		{
			using (StreamReader reader = new StreamReader(PathInFileDateLastClear))
			{
				DateTime lastDateLogWrite = DateTime.MinValue;
				string? text = reader.ReadLine();
				if (DateTime.TryParse(text, out lastDateLogWrite))
				{
					StaticClassGlobalValue.lastDateLogWrite = lastDateLogWrite;
					return true;
				}
				else
				{ 
					return false;
				}
			}
		}

		private void WriteDataClearFromFile(DateTime? lastDateLogWrite)
		{
			using (StreamWriter writer = new StreamWriter(PathInFileDateLastClear, false))
			{
				writer.WriteLine(lastDateLogWrite.ToString());
			}
		}

		public void CheckTimeClear()
		{ 
			//Ему прилетает объект с датой, он его проверяет, затем если пора чистить, читает документ, ищет файлы до даты чистки, запоминает их, всё остальное чистит
			//Можно сделат по дате, можно по количеству записей
		}

	}

}
