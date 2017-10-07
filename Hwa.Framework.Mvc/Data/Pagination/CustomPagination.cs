using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hwa.Framework.Mvc.UI.Grid;
using Hwa.Framework.Mvc.Model;

namespace Hwa.Framework.Mvc.Data.Pagination
{
	/// <summary>
	/// Implementation of IPagination that wraps a pre-paged data source. 
	/// </summary>
	public class CustomPagination<T> : IPagination<T>
	{
		private readonly IList<T> _dataSource;

		/// <summary>
		/// Creates a new instance of CustomPagination
		/// </summary>
		/// <param name="dataSource">A pre-paged slice of data</param>
		/// <param name="pageNumber">The current page number</param>
		/// <param name="pageSize">The number of items per page</param>
		/// <param name="totalItems">The total number of items in the overall datasource</param>
        public CustomPagination(IEnumerable<T> dataSource, PagingModel pagingModel)
		{
			_dataSource = dataSource.ToList();           
            PageSize = pagingModel.PageSize;
            TotalItems = pagingModel.TotalCount;

            PageNumber = pagingModel.PageIndex;
            SortOptions = pagingModel.SortOptions;
            DicSum = pagingModel.DicSum;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _dataSource.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
		public int TotalItems { get; private set; }

		public int TotalPages
		{
			get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
		}

		public int FirstItem
		{
			get
			{
				return ((PageNumber - 1) * PageSize) + 1;
			}
		}

		public int LastItem
		{
			get { return FirstItem + _dataSource.Count - 1; }
		}

		public bool HasPreviousPage
		{
			get { return PageNumber > 1; }
		}

		public bool HasNextPage
		{
			get { return PageNumber < TotalPages; }
		}

        public IDictionary<string, decimal> DicSum 
        { 
            get; 
            set;
        }

        public GridSortOptions SortOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Ö§³ÖjQuery.DataTable
        /// </summary>
        public int draw { get; set; }
	}
}