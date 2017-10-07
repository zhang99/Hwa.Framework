using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Hwa.Framework.Mvc.Annotations
{
    /// <summary>
    /// 下拉框控件，枚举值
    /// </summary>
    public sealed class ValuesAttribute : Attribute
    {
        public ValuesAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }

    /// <summary>
    /// CheckBox返回的逻辑值
    /// </summary>
    public sealed class BoolValuesAttribute : Attribute
    {
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }
    }

    /// <summary>
    /// 标记为图片字段
    /// </summary>
    public sealed class ImageFieldAttribute : Attribute
    {
    }

    /// <summary>
    /// CheckBox型设置字段
    /// </summary>
    public sealed class SettingFieldAttribute : Attribute
    {
        public SettingFieldAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}
