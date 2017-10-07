using Hwa.Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hwa.Framework.Mvc
{
    /// <summary>
    /// WebApplication
    /// 
    /// ---------Usage: Global.asax-------------
    /// public class MvcApplication : System.Web.HttpApplication
    ///{
    ///     public override void Init()
    ///     {
    ///        base.Init();   
    ///        new Hwa.Framework.Mvc.WebApplication().Initialize(this);
    ///     }
    /// }
    /// ----------------------------    
    /// </summary>
    public class WebApplication
    {
        public void Initialize(HttpApplication application)
        {
            application.PostAuthenticateRequest += 
                (sender, e) => HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);

            application.EndRequest += application_EndRequest;

        }

        private void application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                IList<string> keys = new List<string>();
                foreach (var key in HttpContext.Current.Items.Keys)
                {
                    if ((key is string) && key.ToString().Contains(Hwa.Framework.Mvc.Model.APP_DEFINE.HTTP_REQUEST_SINGLETON_KEY))
                        keys.Add(key.ToString());
                }

                foreach (string key in keys)
                {
                    var obj = EntityHelper.GetFieldValue(HttpContext.Current.Items[key], "Value");
                    if (obj != null)
                        ((IDisposable)obj).Dispose();
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.ErrorFormat("WebApplication.application_EndRequest..{0}", ex.Message);
            }
        }
    }
}
