using Domain;
using MetaData;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Data
{
    public class ProcessMonthlyDataUsage : IProcessMonthlyDataUsage
    {
        private readonly ILogger _logger;
        private readonly IAzureSQLDB _azureSQLDB;

        private enum DaysoftheWeek { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

        public ProcessMonthlyDataUsage(ILogger<ProcessMonthlyDataUsage> logger, IAzureSQLDB azureSQLDB)
        {
            _logger = logger;
            _azureSQLDB = azureSQLDB;
        }

        public void MonthlyUsageProcess(DataUsageRequest dataRequest)
        {
            DataUsageLog(MethodBase.GetCurrentMethod().Name, "Begin");

            ProcessDailyData(dataRequest);
            ProcessMonthlyData(dataRequest.MonthTotal);

            DataUsageLog(MethodBase.GetCurrentMethod().Name, "End");
        }

        private void ProcessDailyData(DataUsageRequest fullDataRequest)
        {
            DataUsageLog(MethodBase.GetCurrentMethod().Name, "Begin");

            List<TrafficDataDaily> dataDailyList = new List<TrafficDataDaily>();
            MonthTotal monthInfo = fullDataRequest.MonthTotal;

            foreach (DataInfo dataInfo in fullDataRequest.DataInfo)
            {
                TrafficDataDaily dataDaily = new TrafficDataDaily()
                {
                    CalendarDay = dataInfo.CalDay,
                    CalendarWeekDay = Enum.GetName(typeof(DaysoftheWeek), dataInfo.CalWeekday),
                    CalendarMonthIndex = monthInfo.CalMonth,
                    CalendarMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthInfo.CalMonth),
                    CalendarYear = monthInfo.CalYear,
                    DailyDataIn = dataInfo.DataIn,
                    DailyDataOut = dataInfo.DataOut
                };

                dataDailyList.Add(dataDaily);
            }

            _azureSQLDB.InsertDailyData(dataDailyList);

            DataUsageLog(MethodBase.GetCurrentMethod().Name, "End");
        }

        private void ProcessMonthlyData(MonthTotal monthlyTotal)
        {
            DataUsageLog(MethodBase.GetCurrentMethod().Name, "Begin");

            TrafficDataMonthly dataMonthly = new TrafficDataMonthly()
            {
                CalendarMonthIndex = monthlyTotal.CalMonth,
                CalendarMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthlyTotal.CalMonth),
                CalendarYear = monthlyTotal.CalYear,
                MonthlyDataIn = monthlyTotal.TotalDataIn,
                MonthlyDataOut = monthlyTotal.TotalDataOut
            };

            _azureSQLDB.InsertMonthlyData(dataMonthly);

            DataUsageLog(MethodBase.GetCurrentMethod().Name, "End");
        }

        private void DataUsageLog(string method, string logString)
        {
            _logger.LogInformation($"{method} - {logString} - {DateTime.Now}");
        }
    }
}
