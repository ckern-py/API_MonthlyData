namespace MetaData
{
    public class DailyData
    {
        public int CalendarDay { get; set; }
        public string CalendarWeekDay { get; set; }
        public int CalendarMonthNumber { get; set; }
        public string CalendarMonthString { get; set; }
        public int CalendarYear { get; set; }
        public int? DailyDataIn_Mb { get; set; }
        public int? DailyDataOut_Mb { get; set; }
    }
}
