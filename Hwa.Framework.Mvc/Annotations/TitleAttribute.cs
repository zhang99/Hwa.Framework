using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Hwa.Framework.Mvc.Annotations
{
    /// <summary>
    /// ControllerTitleAttribute
    /// </summary>
    public sealed class TitleAttribute : Attribute
    {
        public string _title = string.Empty;

        public TitleAttribute(string title)
        {
            this._title = title;
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }
}
