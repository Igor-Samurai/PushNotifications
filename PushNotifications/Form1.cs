using System;
using System.Management;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Drawing.Printing;
using System.Collections.Specialized;
namespace PushNotifications
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] pStatus = { "Other", "Unknown", "Idle", "Printing", "WarmUp", "Stopped Printing", "Offline" };
        string[] pState = {"Paused","Error","Pending Deletion","Paper Jam","Paper Out","Manual Feed","Paper Problem",
                                                         "Offline","IO Active","Busy","Printing","Output Bin Full","Not Available","Waiting",
                                                         "Processing","Initialization","Warming Up","Toner Low","No Toner","Page Punt",
                                                         "User Intervention Required","Out of Memory","Door Open","Server_Unknown","Power Save"};
        private void button1_Click(object sender, EventArgs e)
        {
            string result = "";
            System.Management.ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            foreach (ManagementObject service in searcher.Get())
            {
                foreach (System.Management.PropertyData pData in service.Properties)
                {
                    //if (pData.Value != null)
                    //{
                    //	listBox1.Items.Add("********************");
                    //	listBox1.Items.Add(pData.Name + " = " + pData.Value.ToString());
                    //	result += pData.Value.ToString() + "\n"; ;
                    //}


                    //Clipboard.SetText(result);
                    if (pData.Name == "Caption" || pData.Name == "Default" || pData.Name == "ServerName")//Ищет по свойству и выводит имя (если свойство называется так, то трактовать его как имя принтера)
                        listBox1.Items.Add(pData.Name + " = " + pData.Value);
                    else if ((pData.Name == "PrinterState") && (Convert.ToInt32(pData.Value) != 128) && (Convert.ToInt32(pData.Value) != 131072))
                        listBox1.Items.Add(pData.Name + " = " + pState[Convert.ToInt32(pData.Value)]);
                    else if (pData.Name == "PrinterStatus")
                        listBox1.Items.Add(pData.Name + " = " + pStatus[Convert.ToInt32(pData.Value)]);
                }
            }
            listBox1.Items.Add("********** Конец свойств **********");

        }

        private string GetPrintJobStatus(string printername, string documentname)
        {
            string searchQuery = "SELECT * FROM Win32_PrintJob";
            /*searchQuery can also be mentioned with where Attribute,
                but this is not working in Windows 2000 / ME / 98 machines 
                and throws Invalid query error*/
            ManagementObjectSearcher searchPrintJobs = new ManagementObjectSearcher(searchQuery);
            ManagementObjectCollection prntJobCollection = searchPrintJobs.Get();
            foreach (ManagementObject prntJob in prntJobCollection)
            {
                System.String jobName = prntJob.Properties["Name"].Value.ToString();
                //Job name would be of the format [Printer name], [Job ID]
                char[] splitArr = new char[1];
                splitArr[0] = Convert.ToChar(",");
                string prnterName = jobName.Split(splitArr)[0];
                string documentJobstatus = prntJob.Properties["JobStatus"].Value.ToString();
                string documentName = prntJob.Properties["Document"].Value.ToString();
                if (System.String.Compare(prnterName, printername, true) == 0 && documentName == documentname)
                {
                    return documentJobstatus;
                }
            }
            return "Not Status";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // создаем таймер

            TimerCallback tm = new TimerCallback(
                (object obj) =>
                {
                    System.Management.ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer where " +
                "(Caption = 'Принтер для тестирования перехвата событий')");
                    string PrinterStatus = "";
                    string PrinterState = "";

                    foreach (ManagementObject service in searcher.Get())
                    {
                        foreach (System.Management.PropertyData pData in service.Properties)
                        {
                            if (pData.Name == "Caption" || pData.Name == "Default" || pData.Name == "ServerName")//Ищет по свойству и выводит имя (если свойство называется так, то трактовать его как имя принтера)
                                StaticClassGlobalValue.PrinterName = pData.Name + " = " + pData.Value;
                            else if ((pData.Name == "PrinterState") && (Convert.ToInt32(pData.Value) != 128) && (Convert.ToInt32(pData.Value) != 131072))
                                PrinterState = pData.Name + " = " + pState[Convert.ToInt32(pData.Value)];
                            else if (pData.Name == "PrinterStatus")
                                PrinterStatus = pData.Name + " = " + pStatus[Convert.ToInt32(pData.Value)];
                        }
                    }

                    if (StaticClassGlobalValue.PrinterStatus != PrinterStatus)
                    {
                        BeginInvoke(new Action(() =>
                        { listBox1.Items.Add($"Статус принтера изменился с {StaticClassGlobalValue.PrinterStatus} на {PrinterStatus}"); }
                        ));

                        StaticClassGlobalValue.PrinterStatus = PrinterStatus;
                    }

                    if (StaticClassGlobalValue.PrinterState != PrinterState)
                    {
                        BeginInvoke(new Action(() =>
                        { listBox1.Items.Add($"Состояние принтера изменилось с {StaticClassGlobalValue.PrinterState} на {PrinterState}"); }
                        ));

                        StaticClassGlobalValue.PrinterState = PrinterState;
                    }



                });
            System.Threading.Timer timer = new System.Threading.Timer(tm, null, 0, 2000);
        }

        public AutoResetEvent signal;
        private void button3_Click(object sender, EventArgs e)
        {
            signal = new AutoResetEvent(false);
            EventLog myNewLog = new EventLog("Microsoft-Windows-PrintService/Operational", "luch-2077");



            //myNewLog.EntryWritten += new EntryWrittenEventHandler(MyOnEntryWritten);
            myNewLog.EnableRaisingEvents = true;

            myNewLog.WriteEntry("Test message", EventLogEntryType.Information);
            signal.WaitOne();
        }

        public void MyOnEntryWritten(object source, EventRecordWrittenEventArgs e)
        {

            BeginInvoke(new Action(() =>
            {
                listBox1.Items.Add("********************Новое событие от принтера********************");
                EventRecord er = e.EventRecord;
                listBox1.Items.Add(er.TimeCreated.ToString());
                listBox1.Items.Add(er.MachineName);
                listBox1.Items.Add(er.TaskDisplayName);
                listBox1.Items.Add(er.LevelDisplayName);
                listBox1.Items.Add(er.OpcodeDisplayName);
                listBox1.Items.Add("");
            }));


            //signal.Set();
        }

        private void btn_copy_Click(object sender, EventArgs e)
        {
            string resuld = "";
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                resuld += listBox1.Items[i].ToString() + "\n";
            }
            Clipboard.SetText(resuld);

        }

        private void btn_getAllLog_Click(object sender, EventArgs e)
        {
            EventLog[] remoteEventLogs;

            remoteEventLogs = EventLog.GetEventLogs("./luch-2077/c/System32");//luch-2077//System32

            listBox1.Items.Add("Количество журналов на компьютере " + remoteEventLogs.Length);

            foreach (EventLog log in remoteEventLogs)
            {

                listBox1.Items.Add(log.Log);
            }
        }

        private void btn_getEventByID_Click(object sender, EventArgs e)
        {
            int eventId = 812;
            string queryString = "*[System/EventID=" + eventId + "]";
            EventLogQuery query = new EventLogQuery("Microsoft-Windows-PrintService/Operational", PathType.LogName, queryString);

            EventLogWatcher ELW = new EventLogWatcher(query);
            ELW.Enabled = true;
            ELW.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(MyOnEntryWritten);




            EventLogReader eventReader = new EventLogReader(query);
            //Непонятно, как узнать, что все события считались
            int index = 0;
            while (true)
            {
                EventRecord er = eventReader.ReadEvent();
                //listBox1.Items.Add(index.ToString() + " - " + er.TaskDisplayName);
                index++;
            }







            //EventLog log = new EventLog("");
            //log.Log = "Microsoft-Windows-PrintService/Operational";
            //foreach (EventLogEntry entry in log.Entries)
            //{
            //	if (entry.InstanceId == eventId)
            //	{
            //		string fieldname = "ProcessName";
            //		listBox1.Items.Add(entry.Message);
            //		//events.Add(entry.Message);
            //	}
            //}

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //for (int i = 1; i <= 10; i++)
            //{
            //	Thread.Sleep(3000);
            //	SetRequestInTheServer($"Установка трояна... Прогресс: {i*10} процентов");
            //}
            SetRequestInTheServer($"Троян успешно установлен, теперь данные о Мише принадлежат публичному сообществу");

        }


        public async void SetRequestInTheServer(string message)
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://mm.luchllc.ru:8065/hooks/7y6aqqtg73d1fbsgh3cf17y4do");
            string c1 = "{\"text\" : \"1C BASE: RESTORE  COMPLETE\"}";
            string c2 = "{\"text\" :\"" + message + "\"}";
            request.Content = new StringContent("{\"text\" : \"1C BASE: RESTORE  COMPLETE\"}");
            request.Content = new StringContent(c2);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StringCollection printerNameCollection = new StringCollection();
            string searchQuery = "SELECT * FROM Win32_Printer";
            ManagementObjectSearcher searchPrinters =
                  new ManagementObjectSearcher(searchQuery);
            ManagementObjectCollection printerCollection = searchPrinters.Get();
            foreach (ManagementObject printer in printerCollection)
            {
                printerNameCollection.Add(printer.Properties["Name"].Value.ToString());
            }
            listBox1.DataSource = printerNameCollection;
        }

        public static StringCollection GetPrintJobsCollection(string printerName)
        {
            StringCollection printJobCollection = new StringCollection();
            string searchQuery = "SELECT * FROM Win32_PrintJob";

            /*searchQuery can also be mentioned with where Attribute,
                but this is not working in Windows 2000 / ME / 98 machines 
                and throws Invalid query error*/
            ManagementObjectSearcher searchPrintJobs =
                      new ManagementObjectSearcher(searchQuery);
            ManagementObjectCollection prntJobCollection = searchPrintJobs.Get();
            foreach (ManagementObject prntJob in prntJobCollection)
            {
                System.String jobName = prntJob.Properties["Name"].Value.ToString();

                //Job name would be of the format [Printer name], [Job ID]
                char[] splitArr = new char[1];
                splitArr[0] = Convert.ToChar(",");
                string prnterName = jobName.Split(splitArr)[0];
                string documentName = prntJob.Properties["Document"].Value.ToString();
                
                if (System.String.Compare(prnterName, printerName, true) == 0)
                {
                    printJobCollection.Add(documentName);
                }
            }
            return printJobCollection;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = GetPrintJobsCollection("Принтер для тестирования перехвата событий");
        }
    }
}

            