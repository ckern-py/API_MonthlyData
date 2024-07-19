using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaData
{
    public class GetDailyDataForMonthResponse : BaseResponse
    {
        public List<DailyData> DailyData {  get; set; }
    }
}
