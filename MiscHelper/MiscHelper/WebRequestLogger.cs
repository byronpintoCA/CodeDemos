using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace MiscHelper
{
    public class WebRequestLogger
    {
        static Mutex _mutex; // Need a mutex to work with multiple instances of the dll

        static WebRequestLogger()
        {
            const String MUTEX_NAME = "DOH.Drive.Service.WebRequestLogger";
            try
            {
                _mutex = Mutex.OpenExisting(MUTEX_NAME);
            }
            catch 
            {
                _mutex = new Mutex(false, MUTEX_NAME);
            }
        }

        public static bool ShouldILog()
        {
            bool log = false;

            try
            {
                string logSetting = ConfigurationManager.AppSettings["LOG_REQUESTS"];
                if (logSetting != null && logSetting == "1")
                {
                    log = true;
                }
            }
            catch { }

            return log;
        }

        public static void RegisterRequest()
        {
            try
            {
                if (ShouldILog() == false) return;
                
                HttpResponse response = HttpContext.Current.Response;
                IISReqRespLog filter = new IISReqRespLog(response.Filter);
                response.Filter = filter;
                filter.ReadInputData(HttpContext.Current.Request);
                HttpContext.Current.Items.Add("RequestResponse", filter);
            }
            catch { }
        }

        public static void LogResponse(HttpApplication sender)
        {
            try
            {
                string filePath = $@"{HttpRuntime.AppDomainAppPath}App_Data\RequestResponse.log";

                if (ShouldILog() == false) return;

                var filter = (IISReqRespLog)HttpContext.Current.Items["RequestResponse"];

                String logData = $"BEGIN {DateTime.Now} {Environment.NewLine}" +
                    $"Headers {Environment.NewLine}---------------------------------------------------------- {Environment.NewLine}{filter.Headers}{Environment.NewLine}" +
                    $"QueryString : {filter.QueryString} {Environment.NewLine}" +
                    $"Request : {filter.InputData} {Environment.NewLine}" +
                    $"Response : {filter.OutputData} {Environment.NewLine}END{Environment.NewLine}";

               
                LogData(logData, filePath);
            }
            catch { }

        }

        public static void LogData(string logData, string filePath)
        {
            var data = new { fileName = filePath, dataToLog = logData };
            ThreadPool.QueueUserWorkItem((objData) =>
            {
                try
                {
                    _mutex.WaitOne();

                    dynamic dataValues = objData;
                    
                    File.AppendAllText(dataValues.fileName, dataValues.dataToLog);
                    
                }
                catch { }
                finally
                {
                    _mutex.ReleaseMutex();
                }

            }, data);

        }
        
    }
}

//public class WebApiApplication : System.Web.HttpApplication
//{
//    protected void Application_Start()
//    {
//        GlobalConfiguration.Configure(WebApiConfig.Register);
//        AutoMapperConfig.Configure();
//    }

//    protected void Application_BeginRequest(object sender, EventArgs e)
//    {
//        WebRequestLogger.RegisterRequest();
//    }

//    protected void Application_EndRequest(object sender, EventArgs e)
//    {
//        WebRequestLogger.LogResponse((HttpApplication)sender);
//    }

//}