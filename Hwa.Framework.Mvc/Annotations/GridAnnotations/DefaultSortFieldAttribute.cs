using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hwa.Framework.Mvc.UI.Grid;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 默认排序列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class DefaultSortFieldAttribute : Attribute
    {      
        public SortDirection SortDirection { get; set; }
    }
}
