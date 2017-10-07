using Hwa.Framework.Mvc.Data;
using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hwa.Framework.Mvc
{
    /// <summary>
    /// HttpRequestSingleton -- zhangh 2013/06/20
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpRequestSingleton<T> 
    {
        private const string KEY = APP_DEFINE.HTTP_REQUEST_SINGLETON_KEY;
        private static readonly object _syncRoot = new object();

        private HttpRequestSingleton()
        {
        }

        private static string _keyRequestType
        {
            get
            {
                return KEY + typeof(T).Name;
            }
        }

        private T _value;
        private bool _isSet;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _isSet = true;
            }
        }

        public bool IsSet
        {
            get
            {
                return _isSet;
            }
        }

        public static HttpRequestSingleton<T> Instance
        {
            get
            {
                lock (_syncRoot)
                {
                    if (HttpContext.Current.Items[_keyRequestType] == null)
                    {
                        HttpContext.Current.Items[_keyRequestType] = new HttpRequestSingleton<T>();
                    }
                }
                return HttpContext.Current.Items[_keyRequestType] as HttpRequestSingleton<T>;
            }
        }
                
    }
}
