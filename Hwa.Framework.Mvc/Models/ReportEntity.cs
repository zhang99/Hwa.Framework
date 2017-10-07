using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hwa.Framework.Mvc.Model
{
    public class ReportEntity
    {
        /// <summary>
        /// 报表名
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Rdl文件名
        /// </summary>
        public string RdlName { get; set; }

        /// <summary>
        /// PDF
        /// </summary>
        private string _reportType = "PDF";
        public string ReportType
        {
            get { return _reportType; }
            set { _reportType = value; }
        }

        /// <summary>
        /// 数据源实体类型
        /// </summary>
        public Type SourceType { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public object DataSource { get; set; }

        /// <summary>
        /// 表头参数
        /// </summary>
        public ICollection<Microsoft.Reporting.WebForms.ReportParameter> Params { get; set; }

    }
}