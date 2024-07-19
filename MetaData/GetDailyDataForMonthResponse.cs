using System.Collections.Generic;

namespace MetaData
{
    public class GetDailyDataForMonthResponse : BaseResponse
    {
        public List<DailyData> DailyData { get; set; }
    }
}
