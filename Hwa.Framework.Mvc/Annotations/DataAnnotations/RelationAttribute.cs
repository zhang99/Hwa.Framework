using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hwa.Framework.Mvc.Annotations.DataAnnotations
{
    /// <summary>
    /// 关联实体数据过滤
    /// </summary>
    public sealed class RelationAttribute : Attribute
    {
        private string _condition = string.Empty;
        public RelationAttribute(string condition)
        {
            this._condition = condition;
        }

        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
    }
}