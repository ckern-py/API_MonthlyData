using Domain;
using MetaData;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Reflection;

namespace Data
{
    public class Email : IEmail
    {
        private readonly ILogger _logger;

        public Email(ILogger<Email> log)
        {
            _logger = log;
        }

        public void SendEmail(string currentMethod)
        {
            EmailLog("SendEmail1", "Begin");

            BuildSendEmail("SUCCESS: MonthlyDataAPI", $"{currentMethod} has successfully written the data to the DataBase");

            EmailLog("SendEmail1", "End");
        }

        public void SendEmail(string currentMethod, Exception exception)
        {
            EmailLog("SendEmail2", "Begin");

            BuildSendEmail("ERROR: MonthlyDataAPI", $"{currentMethod} ran into a problem.\n\nMessage: {exception.Message}\n\nStackTrace: {exception.StackTrace}");

            EmailLog("SendEmail2", "End");
        }

        public void SendEmail(string currentMethod, Exception exception, MonthlyDataLogging loggingRequest)
        {
            EmailLog("SendEmail3", "Begin");

            string jsonLoggingRequest = JsonConvert.SerializeObject(loggingRequest);
            BuildSendEmail("ERROR: MonthlyDataAPI", $"{currentMethod} ran into a problem.\n\nRequest trying to be logged:\n\n{jsonLoggingRequest}\n\nError Message: {exception.Message}\n\nStackTrace: {exception.StackTrace}");

            EmailLog("SendEmail3", "End");
        }

        private void BuildSendEmail(string emailSubject, string emailMessageContent)
        {
            EmailLog(MethodBase.GetCurrentMethod().Name, "Start");

            string apiKey = Environment.GetEnvironmentVariable("SendgridAPIKey");
            SendGridClient client = new SendGridClient(apiKey);

            string apiEmail = Environment.GetEnvironmentVariable("APIEmailAddress");
            SendGridMessage emailMessage = new SendGridMessage()
            {
                From = new EmailAddress(apiEmail, "MonthlyData_API"),
                Subject = emailSubject,
                PlainTextContent = emailMessageContent
            };

            string receivingEmail = Environment.GetEnvironmentVariable("MyEmailAddress");
            emailMessage.AddTo(new EmailAddress(receivingEmail));

            EmailLog(MethodBase.GetCurrentMethod().Name, "Send");
            Response _ = client.SendEmailAsync(emailMessage).Result;

            EmailLog(MethodBase.GetCurrentMethod().Name, "Return");
        }

        private void EmailLog(string method, string logString)
        {
            _logger.LogInformation($"{method} - {logString} - {DateTime.Now}");
        }
    }
}
