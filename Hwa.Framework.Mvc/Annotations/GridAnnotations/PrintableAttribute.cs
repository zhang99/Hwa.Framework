using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    [AttributeUsage(AttributeTargets.Property,
    AllowMultiple = false, Inherited = false)]
    public class PrintableAttribute : Attribute
    {
        public PrintableAttribute(bool isPrintable)
        {
            this.IsPrintable = isPrintable;
        }

        public bool IsPrintable { get; set; }
    }
}
