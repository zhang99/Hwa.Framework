using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 默认等于指定列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class EqualToAttribute : Attribute
    {
        public EqualToAttribute(string equalTo)
        {
            this.EqualTo = equalTo;
        }

        public string EqualTo { get; set; }
    }
}
