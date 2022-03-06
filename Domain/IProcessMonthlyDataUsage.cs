using MetaData;

namespace Domain
{
    public interface IProcessMonthlyDataUsage
    {
        void MonthlyUsageProcess(DataUsageRequest dataRequest);
    }
}
