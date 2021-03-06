using Domain;
using MetaData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace MonthlyDataAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MonthlyDataUsageController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAzureSQLDB _azureSQLDB;
        private readonly IProcessMonthlyDataUsage _processDataUsage;
        private readonly IEmail _email;

        public MonthlyDataUsageController(ILogger<MonthlyDataUsageController> logger, IAzureSQLDB azureSQLDB, IProcessMonthlyDataUsage processDataUsage, IEmail email)
        {
            _logger = logger;
            _azureSQLDB = azureSQLDB;
            _processDataUsage = processDataUsage;
            _email = email;
        }

        [HttpPost]
        public JsonResult MonthlyDataUsage(DataUsageRequest request)
        {
            _logger.LogInformation("Begin MonthlyDataUsage");

            BaseResponse response = new BaseResponse();
            DateTime startTime = Utility.GetrCentralTime();
            List<string> requestingSystems = Utility.GetValidRequestingSystems();
            string errorMessage = string.Empty;
            string innerEx = string.Empty;

            try
            {
                if (requestingSystems.Contains(request.RequestingSystem))
                {
                    //add more checks before doing??
                    _processDataUsage.MonthlyUsageProcess(request);
                    response.Response = Constants.ResponseMessage.SuccessResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.Response = Constants.ResponseMessage.BadRequestResponse;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                _email.SendEmail(MethodBase.GetCurrentMethod().Name);
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

                _email.SendEmail(MethodBase.GetCurrentMethod().Name, ex);
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
                catch (Exception logEx)
                {
                    _email.SendEmail(MethodBase.GetCurrentMethod().Name, logEx, loggingRequest);
                }

                _logger.LogInformation("End MonthlyDataUsage");
            }

            return new JsonResult(response);
        }

    }
}
