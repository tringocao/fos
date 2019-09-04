using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Unity;

namespace FOS.API.App_Start
{
    public class LogActionWebApiFilter : ActionFilterAttribute
    {
        [Dependency]
        public ILog Log { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //This is where you will add any custom logging code
            //that will execute before your method runs.
            Log.DebugFormat(string.Format("Request {0} {1}"
               , actionContext.Request.Method.ToString()
                  , actionContext.Request.RequestUri.ToString()));
        }

        //This function will execute after the web api controller
        //Part 3
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //This is where you will add any custom logging code that will
            //execute after your method runs.
            Log.DebugFormat(string.Format("{0} Response Code: {1}"
                       , actionExecutedContext.Request.RequestUri.ToString()
                          , actionExecutedContext.Response.StatusCode.ToString()));
        }
    }
}