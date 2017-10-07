using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Hwa.Framework;
using Newtonsoft.Json;

namespace Hwa.Framework.Mvc
{
    /// <summary>
    /// Api枚举
    /// </summary>
    public enum Api
    {
        None,
        WebSite,
        Platform,
        SupplyChain
    }

    /// <summary>
    /// ApiHelper帮助类 TODO:待重构
    /// </summary>
    public static class ApiHelper
    {
        private static HttpClient _httpClient = null;

        /// <summary>
        /// HttpClient
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetClientInstance()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }

            return _httpClient;
        }

        /// <summary>
        /// Api地址
        /// </summary>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string MakeUrl(Api api, string url)
        {
            if (!url.HasValue() || url.Length < 2)
                throw new ArgumentException("url参数无效!");

            if (url.StartsWith("/"))
                url = url.Substring(1);

            if (api == Api.Platform)
            {
                string platformApiAddress = System.Configuration.ConfigurationManager.AppSettings["PlatformApiAddress"];
                if (!platformApiAddress.HasValue())
                    throw new ArgumentException("Web.config中PlatformApiAddress配置不正确!");

                if (!platformApiAddress.EndsWith("/"))
                    platformApiAddress = string.Format("{0}{1}", platformApiAddress, "/");

                url = string.Format("{0}{1}", platformApiAddress, url);
            }
            else if (api == Api.WebSite)
            {
                string webSiteApiAddress = System.Configuration.ConfigurationManager.AppSettings["WebSiteApiAddress"];
                if (!webSiteApiAddress.HasValue())
                    throw new ArgumentException("Web.config中WebSiteApiAddress配置不正确!");

                if (!webSiteApiAddress.EndsWith("/"))
                    webSiteApiAddress = string.Format("{0}{1}", webSiteApiAddress, "/");

                url = string.Format("{0}{1}", webSiteApiAddress, url);
            }
            else if (api == Api.SupplyChain)
            {
                string supplyChainApiAddress = System.Configuration.ConfigurationManager.AppSettings["SupplyChainApiAddress"];
                if (!supplyChainApiAddress.HasValue())
                    throw new ArgumentException("Web.config中SupplyChainApiAddress配置不正确!");

                if (!supplyChainApiAddress.EndsWith("/"))
                    supplyChainApiAddress = string.Format("{0}{1}", supplyChainApiAddress, "/");

                url = string.Format("{0}{1}", supplyChainApiAddress, url);
            }

            return url;
        }

        /// <summary>
        /// Api Get直接调用给定url地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static object Get(string url)
        {
            var httpClient = GetClientInstance();

            long tenantId = -1;

            //httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + string.Format(SissUserContext.API_AUTHORIZATION_KEY, tenantId).ToBase64());
            //var httpResult = httpClient.GetAsync(url).Result;

            //modify by zhangh on 2016/09/14
            string authtoken = string.Format("", tenantId).ToBase64();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Basic " + authtoken);
            var httpResult = httpClient.SendAsync(request).Result;
            //end modify

            return httpResult.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Api Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Get<T>(Api api, string url) where T : class
        {
            var httpResult = SendAsync<T>(HttpMethod.Get, api, url, null);
            return httpResult.Content.ReadAsAsync<T>().Result;
        }

        /// <summary>
        /// Api POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T Post<T>(Api api, string url, T content) where T : class
        {
            var httpResult = SendAsync<T>(HttpMethod.Post, api, url, content);
            return httpResult.Content.ReadAsAsync<T>().Result;
        }

        /// <summary>
        /// Api POST
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static TResult Post<TModel, TResult>(string url, TModel content)
        {
            return Post<TModel, TResult>(Api.None, url, content);
        }

        /// <summary>
        /// Api POST
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static TResult Post<TModel, TResult>(Api api, string url, TModel content)
        {
            var httpResult = SendAsync<TModel>(HttpMethod.Post, api, url, content);

            return httpResult.Content.ReadAsAsync<TResult>().Result;
        }

        /// <summary>
        /// Api POST
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="api"></param>
        /// <param name="tenantId"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static TResult Post<TModel, TResult>(Api api, long tenantId, string url, TModel content)
        {
            url = ApiHelper.MakeUrl(api, url);

            var httpClient = GetClientInstance();

            long tentId = -1;

            //httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + string.Format(SissUserContext.API_AUTHORIZATION_KEY, tentId).ToBase64());
            //var httpResult = httpClient.PostAsJsonAsync<TModel>(url, content).Result;

            //modify by zhangh on 2016/09/14  TODO:待测试验证
            string authtoken = string.Format("", tenantId).ToBase64();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", "Basic " + authtoken);
            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var httpResult = httpClient.SendAsync(request).Result;
            //end modify

            if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException(httpResult.Content.ReadAsStringAsync().Result);

            return httpResult.Content.ReadAsAsync<TResult>().Result;

        }

        /// <summary>
        /// POST，读取结果为json字符串
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string PostForJsonString<TModel>(Api api, string url, TModel content)
        {
            var httpResult = SendAsync<TModel>(HttpMethod.Post, api, url, content);
            return httpResult.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static HttpResponseMessage SendAsync<T>(HttpMethod httpMethod, Api api, string url, T content)
        {
            url = ApiHelper.MakeUrl(api, url);

            var httpClient = GetClientInstance();

            long tenantId =  -1;

            //httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + string.Format(SissUserContext.API_AUTHORIZATION_KEY, tenantId).ToBase64());
            //var httpResult = httpClient.PostAsJsonAsync<T>(url, content).Result;

            //modify by zhangh on 2016/09/14  TODO:待测试验证
            string authtoken = string.Format("", tenantId).ToBase64();
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, url);
            request.Headers.Add("Authorization", "Basic " + authtoken);

            if (httpMethod == HttpMethod.Post && content != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            var httpResult = httpClient.SendAsync(request).Result;
            //end modify

            if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException(httpResult.Content.ReadAsStringAsync().Result);
            return httpResult;
        }
    }
}
