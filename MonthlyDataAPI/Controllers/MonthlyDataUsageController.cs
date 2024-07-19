using Domain;
using MetaData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace MonthlyDataAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MonthlyDataUsageController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAzureSQLDB _azureSQLDB;
        private readonly IProcessMonthlyDataUsage _processDataUsage;
        private readonly IConfiguration _configuration;

        public MonthlyDataUsageController(ILogger<MonthlyDataUsageController> logger, IAzureSQLDB azureSQLDB, IProcessMonthlyDataUsage processDataUsage, IConfiguration configuration)
        {
            _logger = logger;
            _azureSQLDB = azureSQLDB;
            _processDataUsage = processDataUsage;
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult InsertMonthlyDataUsage(DataUsageRequest request)
        {
            _logger.LogInformation("Begin MonthlyDataUsage");

            BaseResponse response = new BaseResponse();
            DateTime startTime = Utility.GetrCentralTime();
            List<string> requestingSystems = Utility.GetValidRequestingSystems(_configuration);
            string errorMessage = string.Empty;
            string innerEx = string.Empty;

            try
            {
                if (requestingSystems.Contains(request.RequestingSystem))
                {
                    _processDataUsage.MonthlyUsageProcess(request);
                    response.Response = Constants.ResponseMessage.SuccessResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.Response = Constants.ResponseMessage.BadRequestResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                response.Response = Constants.ResponseMessage.BadRequestResponse;
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                errorMessage = ex.Message;
                _logger.LogError(errorMessage);

                if (ex.InnerException != null)
                {
                    innerEx = $"InnerEx: {ex.InnerException.Message}";
                    _logger.LogError(innerEx);
                }
            }
            finally
            {
                DateTime endTime = Utility.GetrCentralTime();
                MonthlyDataLogging loggingRequest = new MonthlyDataLogging()
                {
                    ApplicationName = "MonthlyDataAPI",
                    RequestingSystem = request.RequestingSystem,
                    ApiMethod = "MonthlyDataUsage",
                    RequestingStartDt = startTime.ToString("MMddyyy HH:mm:ss.ffff"),
                    RequestingEndDt = endTime.ToString("MMddyyy HH:mm:ss.ffff"),
                    ErrorMessage = errorMessage,
                    RequestMessage = JsonConvert.SerializeObject(request),
                    ResponseMessage = JsonConvert.SerializeObject(response),
                    Debug1 = innerEx,
                    Debug2 = string.Join(", ", HttpContext.Request.Headers),
                    ReturnCode = HttpContext.Response.StatusCode.ToString()
                };

                try
                {
                    _azureSQLDB.LogToDB(loggingRequest);
                }
                catch (Exception)
                {
                    //catch so that failed logging doesn't change request response
                }

                _logger.LogInformation("End MonthlyDataUsage");
            }

            return new JsonResult(response);
        }

        [HttpPost]
        public JsonResult GetDataTotalForMonth(GetDataTotalForMonthRequest request)
        {
            _logger.LogInformation("Begin GetDataTotalForMonth");

            GetDataTotalForMonthResponse response = new GetDataTotalForMonthResponse();
            DateTime startTime = Utility.GetrCentralTime();
            List<string> requestingSystems = Utility.GetValidRequestingSystems(_configuration);
            string errorMessage = string.Empty;
            string innerEx = string.Empty;

            try
            {
                if (requestingSystems.Contains(request.RequestingSystem) && request.MonthNumber > 0 && request.MonthYear > 0)
                {
                    response = _processDataUsage.GetDataTotalForGivenMonth(request.MonthNumber, request.MonthYear);
                    response.Response = Constants.ResponseMessage.SuccessResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.Response = Constants.ResponseMessage.BadRequestResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                response.Response = Constants.ResponseMessage.BadRequestResponse;
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                errorMessage = ex.Message;
                _logger.LogError(errorMessage);

                if (ex.InnerException != null)
                {
                    innerEx = $"InnerEx: {ex.InnerException.Message}";
                    _logger.LogError(innerEx);
                }
            }
            finally
            {
                DateTime endTime = Utility.GetrCentralTime();
                MonthlyDataLogging loggingRequest = new MonthlyDataLogging()
                {
                    ApplicationName = "MonthlyDataAPI",
                    RequestingSystem = request.RequestingSystem,
                    ApiMethod = "GetDataTotalForMonth",
                    RequestingStartDt = startTime.ToString("MMddyyy HH:mm:ss.ffff"),
                    RequestingEndDt = endTime.ToString("MMddyyy HH:mm:ss.ffff"),
                    ErrorMessage = errorMessage,
                    RequestMessage = JsonConvert.SerializeObject(request),
                    ResponseMessage = JsonConvert.SerializeObject(response),
                    Debug1 = innerEx,
                    Debug2 = string.Join(", ", HttpContext.Request.Headers),
                    ReturnCode = HttpContext.Response.StatusCode.ToString()
                };

                try
                {
                    _azureSQLDB.LogToDB(loggingRequest);
                }
                catch (Exception)
                {
                    //catch so that failed logging doesn't change request response
                }

                _logger.LogInformation("End GetDataTotalForMonth");
            }

            return new JsonResult(response);
        }

        [HttpPost]
        public JsonResult GetDailyDataForMonth(GetDailyDataForMonthRequest request)
        {
            _logger.LogInformation("Begin GetDailyDataForMonth");

            GetDailyDataForMonthResponse response = new GetDailyDataForMonthResponse();
            DateTime startTime = Utility.GetrCentralTime();
            List<string> requestingSystems = Utility.GetValidRequestingSystems(_configuration);
            string errorMessage = string.Empty;
            string innerEx = string.Empty;

            try
            {
                if (requestingSystems.Contains(request.RequestingSystem))
                {
                    response.DailyData = _processDataUsage.GetDailyDataForGivenMonth(request.MonthNumber, request.MonthYear);
                    response.Response = Constants.ResponseMessage.SuccessResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.Response = Constants.ResponseMessage.BadRequestResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                response.Response = Constants.ResponseMessage.BadRequestResponse;
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                errorMessage = ex.Message;
                _logger.LogError(errorMessage);

                if (ex.InnerException != null)
                {
                    innerEx = $"InnerEx: {ex.InnerException.Message}";
                    _logger.LogError(innerEx);
                }
            }
            finally
            {
                DateTime endTime = Utility.GetrCentralTime();
                MonthlyDataLogging loggingRequest = new MonthlyDataLogging()
                {
                    ApplicationName = "MonthlyDataAPI",
                    RequestingSystem = request.RequestingSystem,
                    ApiMethod = "GetDailyDataForMonth",
                    RequestingStartDt = startTime.ToString("MMddyyy HH:mm:ss.ffff"),
                    RequestingEndDt = endTime.ToString("MMddyyy HH:mm:ss.ffff"),
                    ErrorMessage = errorMessage,
                    RequestMessage = JsonConvert.SerializeObject(request),
                    ResponseMessage = JsonConvert.SerializeObject(response),
                    Debug1 = innerEx,
                    Debug2 = string.Join(", ", HttpContext.Request.Headers),
                    ReturnCode = HttpContext.Response.StatusCode.ToString()
                };
                try
                {
                    _azureSQLDB.LogToDB(loggingRequest);
                }
                catch (Exception)
                {
                    //catch so that failed logging doesn't change request response
                }

                _logger.LogInformation("End GetDailyDataForMonth");
            }

            return new JsonResult(response);
        }
    }
}
