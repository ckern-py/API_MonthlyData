using MetaData;
using System;

namespace Domain
{
    public interface IEmail
    {
        void SendEmail(string currentMethod);
        void SendEmail(string currentMethod, Exception exception);
        void SendEmail(string currentMethod, Exception exception, MonthlyDataLogging loggingRequest);
    }
}
