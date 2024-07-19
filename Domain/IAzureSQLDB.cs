using MetaData;
using System.Collections.Generic;

namespace Domain
{
    public interface IAzureSQLDB
    {
        void IsDBAlive();
        void LogToDB(MonthlyDataLogging loggingRequest);
        void InsertMonthlyData(TrafficDataMonthly monthlyData);
        void InsertDailyData(List<TrafficDataDaily> dailyData);
        TrafficDataMonthly GetMonthlyTraffic(int monthInt, int year);
    }
}
