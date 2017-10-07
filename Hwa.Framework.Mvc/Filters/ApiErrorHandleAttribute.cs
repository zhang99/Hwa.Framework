using Hwa.Framework.Mvc.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Hwa.Framework.Mvc.Filters
{
    //public class ApiErrorHandleAttribute : ExceptionFilterAttribute
    //{
    //    public override void OnException(HttpActionExecutedContext context)
    //    {
    //        Exception exception = context.Exception;
    //        while (exception.InnerException != null)
    //        {
    //            exception = exception.InnerException;
    //        }

    //        HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
    //        {
    //            Content = new StringContent(exception.Message),
    //            ReasonPhrase = "调用Web API时发生错误!"
    //        };
    //        context.Response = msg;

    //        Logger.Error(exception.ToString());
    //    }
    //}
}
