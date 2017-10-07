using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Hwa.Framework.Mvc.ModelBinders
{
    public class JsonModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string json = string.Empty;

            var provider = bindingContext.ValueProvider;

            var providerValue = provider.GetValue(bindingContext.ModelName);

            if (providerValue != null)
                json = providerValue.AttemptedValue;
            else
                json = controllerContext.HttpContext.Request.Form[0] ;
            
            // Basic expression to make sure the string starts and ends
            // with JSON object ( {} ) or array ( [] ) characters
            if (Regex.IsMatch(json, @"^(\[.*\]|{.*})$"))
            {
                //return new JavaScriptSerializer().Deserialize(json, bindingContext.ModelType);
                return JsonConvert.DeserializeObject(json, bindingContext.ModelType);
            }           

            return base.BindModel(controllerContext, bindingContext);
        }        
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum |
     AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Method |
     AttributeTargets.Struct | AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class JsonModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonModelBinder();
        }
    }


}