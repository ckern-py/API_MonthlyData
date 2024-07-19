namespace MetaData
{
    public class GetDataTotalForMonthResponse : BaseResponse
    {
        public int CalendarMonthNumber { get; set; }
        public string CalendarMonthString { get; set; }
        public int CalendarYear { get; set; }
        public int? MonthlyDataIn_Mb { get; set; }
        public int? MonthlyDataOut_Mb { get; set; }
    }
}
