using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Net
{
    public static class HttpClientHelper
    {
        /// <summary>
        /// Api Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Get<T>(string url) where T : class
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var httpResult = httpClient.GetAsync(url).Result;

                if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new HttpRequestException(httpResult.Content.ReadAsStringAsync().Result);

                return httpResult.Content.ReadAsAsync<T>().Result;
            };
        }

        /// <summary>
        /// Api Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static object Get(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var httpResult = httpClient.GetAsync(url).Result;

                return httpResult.Content.ReadAsStringAsync().Result;
            };
        }

        /// <summary>
        /// Api POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T Post<T>(string url, T content) where T : class
        {
            using (HttpClient httpClient = new HttpClient())
            {

                var httpResult = httpClient.PostAsJsonAsync<T>(url, content).Result;

                if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new HttpRequestException(httpResult.Content.ReadAsStringAsync().Result);

                return httpResult.Content.ReadAsAsync<T>().Result;
            };
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
        public static TResult Post<TModel, TResult>(string url, TModel content)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var httpResult = httpClient.PostAsJsonAsync<TModel>(url, content).Result;

                if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new HttpRequestException(httpResult.Content.ReadAsStringAsync().Result);

                return httpResult.Content.ReadAsAsync<TResult>().Result;
            };
        }
    }
}
