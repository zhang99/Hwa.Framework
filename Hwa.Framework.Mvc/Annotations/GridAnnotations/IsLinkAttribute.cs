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
    public class IsLinkAttribute : Attribute
    {
        public IsLinkAttribute(bool isLink)
        {
            this.IsLink = isLink;
        }

        public IsLinkAttribute(bool isLink, string url)
        {
            this.IsLink = isLink;
            this.Url = url;
        }

        public bool IsLink { get; set; }
        public string Url { get; set; }


    }
}
