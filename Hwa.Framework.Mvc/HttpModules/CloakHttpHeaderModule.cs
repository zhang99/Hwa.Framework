using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hwa.Framework.Mvc.HttpModules
{
    public class CloakHttpHeaderModule : IHttpModule
    {
        private List<string> _headersToCloak;

        public CloakHttpHeaderModule()
        {
            _headersToCloak = new List<string>
                                      {
                                              "Server",
                                              "X-AspNet-Version",
                                              "X-AspNetMvc-Version",
                                              "X-Powered-By",
                                      };
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                HttpContext context = ((HttpApplication)sender).Context;
                if (context != null)
                {
                    _headersToCloak.ForEach(header => context.Response.Headers.Remove(header));
                    context.Response.Headers.Add("Cloud-Server", "Hwa");
                }
            }
        }
    }
}