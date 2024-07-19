using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaData
{
    public class GetDataTotalForMonthRequest : BaseRequest
    {
        public int MonthNumber { get; set; }
        public int MonthYear { get; set; }
    }
}
