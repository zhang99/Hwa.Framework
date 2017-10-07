using Hwa.Framework.Mvc.Data.Pagination;
using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc.Services
{
    public interface IReportService<TMaster, TDetail>
        where TMaster : class
        where TDetail : class
    {
        TMaster GetReportData(PagingModel pagingModel);
    }
}
