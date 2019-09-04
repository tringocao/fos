using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace FOS.API
{
    public class LoggingInterceptor : IInterceptionBehavior
    {
        public bool WillExecute {
            get { return true; }
        }

        [Dependency]
        public ILog Log { get; set; }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var startTime = DateTime.Now;


            WriteInfoLog("Start " + input.MethodBase.Name + " at " + startTime.ToLongTimeString());

            var result = getNext()(input, getNext);

            var timeSpan = DateTime.Now - startTime;

            if (result.Exception != null)
            {
                WriteErrorLog("Method " + input.MethodBase.Name + " returns exception " + result.Exception.Message + " for " + timeSpan.TotalMilliseconds);
            }
            else
            {
                WriteInfoLog("Method " + input.MethodBase.Name + " returns " + result.ReturnValue + " for " + timeSpan.TotalMilliseconds);
            }
            return result;
        }

        private void WriteDebugLog(string message)
        {
            if (Log!=null)
            {
                Log.DebugFormat("Profiler: " + message);
            }
        }

        private void WriteInfoLog(string message)
        {
            if (Log != null)
            {
                Log.InfoFormat("Profiler: " + message);
            }
        }

        private void WriteErrorLog(string message)
        {
            if (Log != null)
            {
                Log.ErrorFormat("Profiler: " + message);
            }
        }
    }
}