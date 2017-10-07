using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Hwa.Framework.Mvc.Annotations
{
    /// <summary>
    /// IsRequiredAttribute标记的属性将在页面显示必填*星号
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public sealed class IsRequiredAttribute : Attribute
    {
        public string Description { get; set; }
    }
}
