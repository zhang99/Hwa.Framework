using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hwa.Framework.Util
{
    public static class EntityHelper
    {        
        /// <summary>
        /// 获取实体属性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object GetFieldValue(object entity, string fieldName)
        {
            object fieldValue = null;
            Type type = entity.GetType();

            dynamic innerEntity = entity;

            PropertyInfo prop = null;
            try
            {
                if (entity is Newtonsoft.Json.Linq.JObject)
                    return (entity as Newtonsoft.Json.Linq.JObject).GetValue(fieldName);

                string[] sNames = fieldName.Split('.');
                for (int i = 0; i < sNames.Length; i++)
                {
                    if (sNames.Length <= 1)
                        break;
                    if (string.IsNullOrWhiteSpace(sNames[i]))
                        continue;
                    prop = type.GetProperty(sNames[i]);
                    type = prop.PropertyType;
                    innerEntity = prop.GetValue(entity, null);
                    if (i == sNames.Length - 2)
                    {
                        fieldName = sNames[sNames.Length - 1];
                        break;
                    }
                }
                prop = type.GetProperty(fieldName);
                fieldValue = prop.GetValue(innerEntity, null);
            }
            catch { }

            return fieldValue;
        }

        /// <summary>
        /// 给实体字段赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object SetFieldValue(object entity, string fieldName, object fieldValue)
        {
            Type type = entity.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo propertyInfo = properties[j];
                if (propertyInfo.Name == fieldName)
                {
                    propertyInfo.SetValue(entity, fieldValue, null);
                    break;
                }
            }
            return entity;
        }

        /// <summary>
        /// 获取字段属性信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static PropertyInfo GetFieldProperty(Type entityType, string fieldName)
        {
            PropertyInfo prop = null;
            try
            {
                Type type = entityType;
                string[] sNames = fieldName.Split('.');
                for (int i = 0; i < sNames.Length; i++)
                {
                    if (sNames.Length <= 1)
                        break;
                    if (string.IsNullOrWhiteSpace(sNames[i]))
                        continue;
                    type = type.GetProperty(sNames[i]).PropertyType;
                    if (i == sNames.Length - 2)
                    {
                        fieldName = sNames[sNames.Length - 1];
                        break;
                    }
                }
                prop = type.GetProperty(fieldName);
            }
            catch { }
            return prop;
        }

        public static Object GetClassAttribute(Type entityType, Type attrType)
        {
           var attrs = entityType.GetCustomAttributes(attrType, false);
            if (attrs.Length > 0)
            {
                return attrs[0];
            }
            return null;
        }

        /// <summary>
        /// 获取类所属字段特性
        /// </summary>
        /// <param name="entityType">类</param>
        /// <param name="fieldName">字段</param>
        /// <param name="attrType">特性类型</param>
        /// <returns></returns>
        public static Attribute GetFieldAttribute(Type entityType, string fieldName, Type attrType)
        {
            Attribute attr = null;
            PropertyInfo prop = GetFieldProperty(entityType, fieldName);
            if (prop != null)
            {
                attr = Attribute.GetCustomAttribute(prop, attrType, false);
            }
            return attr;
        }

        /// <summary>
        /// 获取类所属字段特性
        /// </summary>
        /// <param name="entityType">类</param>
        /// <param name="fieldName">字段</param>
        /// <param name="attrType">特性类型</param>
        /// <returns></returns>
        public static Attribute GetFieldAttribute(PropertyInfo prop, Type attrType)
        {
            Attribute attr = null;
            if (prop != null)
            {
                attr = Attribute.GetCustomAttribute(prop, attrType, false);
            }
            return attr;
        }

        /// <summary>
        /// GetPropertyDefaultFormat
        /// </summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetPropertyDefaultFormat(Type type, ref string format)
        {
            switch (type.Name)
            {
                case "Decimal":
                case "Single":
                case "Double":
                    format = "{0:N2}";
                    break;
                case "DateTime":
                    format = "{0:yyyy-MM-dd}";
                    break;
                case "Nullable`1":
                    GetPropertyDefaultFormat(type.GetGenericArguments()[0],ref format);
                    break;
            }
            return format;
        }       
        
    }
}
