using Hwa.Framework.Data;
using Hwa.Framework.Mvc.Data.Pagination;
using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc.Data
{
    public abstract class ReportRepositoryBase<TMaster, TDetail> : IReportRepository<TMaster, TDetail>
        where TMaster : class
        where TDetail : class
    {
        public virtual IPagination<TDetail> GetPagedList(PagingModel pagingModel)
        {
            return null;
        }
    }
}
