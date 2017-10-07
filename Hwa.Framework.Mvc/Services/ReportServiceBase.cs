using Hwa.Framework.Mvc.Data.Pagination;
using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc.Services
{
    public abstract class ReportServiceBase<TMaster, TDetail> : IReportService<TMaster, TDetail>
        where TMaster : class
        where TDetail : class
    {
        public virtual TMaster GetReportData(PagingModel pagingModel)
        {
            throw new NotImplementedException("请在子类重写此方法！");
        }
    }
}
