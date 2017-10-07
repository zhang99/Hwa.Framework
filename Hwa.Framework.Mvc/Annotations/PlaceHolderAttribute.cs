using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Hwa.Framework.Mvc.Annotations
{
    /// <summary>
    /// PlaceHolder
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,
     AllowMultiple = false, Inherited = false)]
    public sealed class PlaceHolderAttribute : Attribute
    {
        public string Description { get; set; }
    }
}
