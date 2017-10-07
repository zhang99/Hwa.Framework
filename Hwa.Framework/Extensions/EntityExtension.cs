using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa
{
    public static class EntityExtension
    {
        /// <summary>
        /// 复制对象到新实体(深度复制)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T entity) where T : class
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(entity));
            }
            catch
            {
                throw new Exception("复制对象失败!");
            }
        }
    }
}
