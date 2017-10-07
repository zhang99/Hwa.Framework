using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 可否排序
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class SortableAttribute : Attribute
    {
        public SortableAttribute(bool sortable)
        {
            this.Sortable = sortable;
        }

        public bool Sortable { get; set; }
    }
}
