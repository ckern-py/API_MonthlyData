﻿using Domain;
using MetaData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace MonthlyDataAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CheckHealthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAzureSQLDB _azureSQLDB;
        private readonly IConfiguration _configuration;

        public CheckHealthController(ILogger<CheckHealthController> logger, IAzureSQLDB azureSQLDB, IConfiguration configuration)
        {
            _logger = logger;
            _azureSQLDB = azureSQLDB;
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult CheckHealth(BaseRequest request)
        {
            _logger.LogInformation("Begin CheckHealth");

            BaseResponse response = new BaseResponse();
            DateTime startTime = Utility.GetrCentralTime();
            List<string> requestingSystems = Utility.GetValidRequestingSystems(_configuration);
            string errorMessage = string.Empty;
            string innerEx = string.Empty;

            try
            {
                if (requestingSystems.Contains(request.RequestingSystem))
                {
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
                    ApiMethod = "CheckHealth",
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

                _logger.LogInformation("End CheckHealth");
            }

            return new JsonResult(response);
        }

        [HttpPost]
        public JsonResult CheckHealthDB(BaseRequest request)
        {
            _logger.LogInformation("Begin CheckHealthDB");

            BaseResponse response = new BaseResponse();
            DateTime startTime = Utility.GetrCentralTime();
            List<string> requestingSystems = Utility.GetValidRequestingSystems(_configuration);
            string errorMessage = string.Empty;
            string innerEx = string.Empty;

            try
            {
                if (requestingSystems.Contains(request.RequestingSystem))
                {
                    _azureSQLDB.IsDBAlive();
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
                    ApiMethod = "CheckHealthDB",
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

                _logger.LogInformation("End CheckHealthDB");
            }

            return new JsonResult(response);
        }
    }
}
