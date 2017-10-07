using Hwa.Framework.Mvc.Caching;
using Hwa.Framework.Mvc.Filters;
using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace Hwa.Framework.Mvc.WebApi
{
    /// <summary>
    /// BaseApiController 
    /// </summary>
    //[HwaWebApiAuthorize]
    //[ApiErrorHandle]
    public abstract class BaseApiController : ApiController
    {
        public BaseApiController()           
        {
            //Logger.InfoFormat("BaseApiController:{0}-{1}",
            //    System.Web.HttpContext.Current.User.Identity.Name,
            //     System.Web.HttpContext.Current.Request.Path
            //    );
            
        }      
    }
}
