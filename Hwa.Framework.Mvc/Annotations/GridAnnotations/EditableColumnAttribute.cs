using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 可否编辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class EditableColumnAttribute : Attribute
    {
        public EditableColumnAttribute(bool editable)
        {
            this.Editable = editable;
        }

        public bool Editable{ get; set; }
    }
}
