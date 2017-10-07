using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 对齐方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class AlignAttribute : Attribute
    {
        public AlignAttribute(Alignment alignment)
        {
            this.Alignment = alignment;
        }

        public Alignment Alignment { get; set; }
    }
}
