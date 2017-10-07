using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations.GridAnnotations
{
    /// <summary>
    /// 可否查询
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
     AllowMultiple = false, Inherited = false)]
    public class QueryableAttribute : Attribute
    {
        public QueryableAttribute(bool queryable)
        {
            this.Queryable = queryable;
        }

        public bool Queryable { get; set; }
    }
}
