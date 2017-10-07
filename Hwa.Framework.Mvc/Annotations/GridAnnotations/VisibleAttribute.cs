using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 是否显示渲染到页面
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class VisibleAttribute : Attribute
    {
        public VisibleAttribute(bool visible)
        {
            this.Visible = visible;
        }

        public bool Visible { get; set; }
    }
}
