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
    public interface IReportRepository<TMaster, TDetail>
        where TMaster : class
        where TDetail : class
    {
        IPagination<TDetail> GetPagedList(PagingModel pagingModel);
    }
}
