using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hwa.Framework.Mvc.ModelBinders
{
    /// <summary>
    /// HwaArrayModelBinder数组模型绑定
    /// add by zhangh 2013/05/20
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HwaArrayModelBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var json = controllerContext.HttpContext.Request.Form[bindingContext.ModelName] as string;

            if (string.IsNullOrEmpty(json))
                json = controllerContext.HttpContext.Request.Form[bindingContext.ModelName + "[]"] as string;
            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                JObject jsonBody = JObject.Parse(json);
                JsonSerializer js = new JsonSerializer();
                object obj = js.Deserialize(jsonBody.CreateReader(), typeof(T));
                return obj;
            }

            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                IList<T> list = new List<T>();
                JArray jsonRsp = JArray.Parse(json);

                if (jsonRsp != null)
                {
                    for (int i = 0; i < jsonRsp.Count; i++)
                    {
                        JsonSerializer js = new JsonSerializer();
                        object obj = js.Deserialize(jsonRsp[i].CreateReader(), typeof(T));
                        list.Add((T)obj);
                    }
                }
                return list;
            }

            string[] arr = json.Split(new char[] { ',' });
            if (typeof(T).Name == "Int64")
                return Array.ConvertAll(arr, long.Parse);

            return arr;
        }
    }
}
