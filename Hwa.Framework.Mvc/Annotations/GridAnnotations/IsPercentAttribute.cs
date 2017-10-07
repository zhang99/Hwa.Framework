using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 是否为百分比列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class IsPercentAttribute : Attribute
    {
        public IsPercentAttribute(bool isPercent)
        {
            this.IsPercent = isPercent;
        }

        public bool IsPercent { get; set; }
    }
}
