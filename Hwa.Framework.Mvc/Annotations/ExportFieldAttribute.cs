using System;

namespace Hwa.Framework.Mvc.Annotations
{
    /// <summary>
    /// 标记为可导出的字段
    /// </summary>
    public class ExportFieldAttribute : Attribute
    {
        /// <summary>
        /// 指定为必须导出的项
        /// </summary>
        public bool Required { get; set; }
    }
}
