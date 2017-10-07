using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Hwa.Framework.Mvc.UI.Grid;
using Hwa.Framework.Mvc;


namespace Hwa.Framework.Mvc.Model
{
    /// <summary>
    /// 分页辅助类
    /// </summary>
    public class PagingModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        private int _pageIndex = 1;
        public int PageIndex {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 每页记录显示条数
        /// </summary>
        private int _pageSize = 30;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// 排序信息
        /// </summary>
        private GridSortOptions _sortOptions = new GridSortOptions { Column = "Id", Direction = SortDirection.Descending };
        public GridSortOptions SortOptions {
            get
            {
                return _sortOptions;
            }
            set
            {
                _sortOptions = value;
            }
        }

        /// <summary>
        /// 简单模糊查询字段逗号隔开
        /// </summary>
        private string _queryFields = string.Empty;
        public string QueryFields
        {
            get
            {
                return _queryFields;
            }
            set
            {
                _queryFields = value;
            }
        }

        /// <summary>
        /// 简单模糊查询字串
        /// </summary>
        private string _query = string.Empty;
        public string Query {
            get
            {
                return _query;
            }
            set
            {
                _query = value;
            }
        }

        /// <summary>
        /// 是否打印
        /// </summary>
        private bool _isPrint = false;
        public bool IsPrint
        {
            get
            {
                return _isPrint;
            }
            set
            {
                _isPrint = value;
            }
        }

        /// <summary>
        /// 高级查询
        /// </summary>
        private IEnumerable<AdvancedQueryItem> _AdvancedQuery = new HashSet<AdvancedQueryItem>();
        public IEnumerable<AdvancedQueryItem> AdvancedQuery
        {
            get
            {
                return _AdvancedQuery;
            }
            set
            {
                _AdvancedQuery = value;
            }
        }

        /// <summary>
        /// 合计列
        /// </summary>
        private IDictionary<string, decimal> _sumDict = new Dictionary<string, decimal>();
        public IDictionary<string, decimal> DicSum
        {
            get
            {
                return _sumDict;
            }
            set
            {
                _sumDict = value;
            }
        }

        /// <summary>
        /// 总记录
        /// </summary>
        private int _totalCount = 0;
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
            }
        }

        /// <summary>
        /// 支持jQuery.DataTable
        /// </summary>
        public int draw
        {
            get;
            set;
        }

        /// <summary>
        /// 支持jQuery.DataTable
        /// </summary>
        public int recordsTotal
        {
            get { return _totalCount; }
        }

        /// <summary>
        /// 支持jQuery.DataTable
        /// </summary>
        public int recordsFiltered 
        {
            get { return _totalCount; }
        }
    }

    /// <summary>
    /// 高级查询项
    /// </summary>
    public class AdvancedQueryItem
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public QueryMethods Operator { get; set; }
        /// <summary>
        /// 值(多个逗号隔开)
        /// </summary>
        public string Value { get; set; }

    }

    /// <summary>
    /// 分页结果Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResultModel<T> where T : class
    {
        /// <summary>
        /// The current page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// The number of items in each page.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// The total number of items.
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 排序信息
        /// </summary>
        private GridSortOptions _sortOptions = new GridSortOptions { Column = "Id", Direction = SortDirection.Descending };
        public GridSortOptions SortOptions
        {
            get
            {
                return _sortOptions;
            }
            set
            {
                _sortOptions = value;
            }
        }

        public IList<T> PageList { get; set; }

        /// <summary>
        /// 合计列
        /// </summary>
        private IDictionary<string, decimal> _sumDict = new Dictionary<string, decimal>();
        public IDictionary<string, decimal> DicSum
        {
            get
            {
                return _sumDict;
            }
            set
            {
                _sumDict = value;
            }
        }

        /// <summary>
        /// 支持jQuery.DataTable
        /// </summary>
        public int draw
        {
            get;
            set;
        }

        /// <summary>
        /// 支持jQuery.DataTable
        /// </summary>
        public int recordsTotal
        {
            get { return TotalItems; }
        }

        /// <summary>
        /// 支持jQuery.DataTable
        /// </summary>
        public int recordsFiltered
        {
            get { return TotalItems; }
        }
    }
}
