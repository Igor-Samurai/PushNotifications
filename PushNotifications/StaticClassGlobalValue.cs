using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotifications
{
	public static class StaticClassGlobalValue
	{
		public static string PrinterStatus = "";
		public static string PrinterState = "";
		public static string PrinterName = "";
		public static DateTime? lastDateLogWrite = null;
		public static string MashineName = Environment.MachineName;
		public static List<Employees> EmployeesData = new List<Employees>();
		public static bool StatusLoadEmployeesData = true;

		public static CreateRecordInFileXML logProjectMonitorPrinter = null;
		public static CreateRecordInFileXML logEventPrinter = null;
	}
}
