using MetaData;
using System.Collections.Generic;

namespace Domain
{
    public interface IProcessMonthlyDataUsage
    {
        void MonthlyUsageProcess(DataUsageRequest dataRequest);
        GetDataTotalForMonthResponse GetDataTotalForGivenMonth(int monthInt, int year);
        List<DailyData> GetDailyDataForGivenMonth(int monthNumber, int monthYear);
    }
}
