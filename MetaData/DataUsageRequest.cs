namespace MetaData
{
    public class DataUsageRequest : BaseRequest
    {
        public DataInfo[] DataInfo { get; set; }
        public MonthTotal MonthTotal { get; set; }
    }
}
