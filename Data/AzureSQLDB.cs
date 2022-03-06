using Data.Database;
using Domain;
using MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Data
{
    public class AzureSQLDB : IAzureSQLDB
    {
        private readonly IConfiguration _configuration;
        private readonly DbContextOptionsBuilder<MonthlydataDBContext> _dbOptions;
        private readonly ILogger _logger;

        public AzureSQLDB(IConfiguration configuration, ILogger<AzureSQLDB> logger)
        {
            _configuration = configuration;
            _dbOptions = BuildOptions();
            _logger = logger;
        }

        public void IsDBAlive()
        {
            LogSQL(MethodBase.GetCurrentMethod().Name, "Begin");

            using (MonthlydataDBContext context = new MonthlydataDBContext(_dbOptions.Options))
            {
                context.Database.OpenConnection();
                context.Database.CloseConnection();
            }

            LogSQL(MethodBase.GetCurrentMethod().Name, "End");
        }

        public void InsertDailyData(List<TrafficDataDaily> dailyData)
        {
            LogSQL(MethodBase.GetCurrentMethod().Name, "Begin");

            using (MonthlydataDBContext context = new MonthlydataDBContext(_dbOptions.Options))
            {
                context.AddRange(dailyData);
                context.SaveChanges();
            }

            LogSQL(MethodBase.GetCurrentMethod().Name, "End");
        }

        public void InsertMonthlyData(TrafficDataMonthly monthlyData)
        {
            LogSQL(MethodBase.GetCurrentMethod().Name, "Begin");

            using (MonthlydataDBContext context = new MonthlydataDBContext(_dbOptions.Options))
            {
                context.TrafficDataMonthly.Add(monthlyData);
                context.SaveChanges();
            }

            LogSQL(MethodBase.GetCurrentMethod().Name, "End");
        }

        public void LogToDB(MonthlyDataLogging loggingRequest)
        {
            LogSQL(MethodBase.GetCurrentMethod().Name, "Begin");

            using (MonthlydataDBContext context = new MonthlydataDBContext(_dbOptions.Options))
            {
                context.MonthlyDataLogging.Add(loggingRequest);
                context.SaveChanges();
            }

            LogSQL(MethodBase.GetCurrentMethod().Name, "End");
        }

        private DbContextOptionsBuilder<MonthlydataDBContext> BuildOptions()
        {
            DbContextOptionsBuilder<MonthlydataDBContext> optionsBuilder = new DbContextOptionsBuilder<MonthlydataDBContext>();
            optionsBuilder.UseSqlServer(Utility.GetSqlConnection(_configuration));
            optionsBuilder.EnableSensitiveDataLogging();
            return optionsBuilder;
        }

        private void LogSQL(string method, string logString)
        {
            _logger.LogInformation($"{ method} - {logString} - {DateTime.Now}");
        }
    }
}
