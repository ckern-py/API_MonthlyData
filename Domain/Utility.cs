using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Utility
    {
        public static string GetSqlConnection(IConfiguration configuration)
        {
            return configuration["MY_DB_CONNECTION"];
        }

        public static DateTime GetrCentralTime()
        {
            TimeZoneInfo timeZone;
            OperatingSystem os = Environment.OSVersion;
            if (os.Platform == PlatformID.Unix)
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");
            }
            else
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            }
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        }

        public static List<string> GetValidRequestingSystems(IConfiguration config)
        {
            return new List<string>(config["REQUESTING_SYSTEMS"].Split(';'));
        }
    }
}
