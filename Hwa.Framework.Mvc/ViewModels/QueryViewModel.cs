using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hwa.Framework.Mvc;
using Hwa.Framework.Mvc.Annotations.GridAnnotations;

namespace Hwa.Framework.Mvc.ViewModels
{
    /// <summary>
    /// 通用选择，自动完成功能
    /// </summary>
    public class QueryViewModel
    {
        public long Id { get; set; }

        [Queryable(true)]
        public string Code { get; set; }

        [Queryable(true)]
        public string Name { get; set; }

        //备用字段
        public string Type { get; set; }
    }
}