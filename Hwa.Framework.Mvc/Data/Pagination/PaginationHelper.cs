using Hwa.Framework.Mvc.Model;
using Hwa.Framework.Mvc.UI.Grid;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Hwa.Framework.Mvc.Data.Pagination
{
	/// <summary>
	/// Extension methods for creating paged lists.
	/// </summary>
	public static class PaginationHelper
	{
		/// <summary>
		/// Converts the specified IEnumerable into an IPagination using the default page size and returns the specified page number.
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged list.</param>
		/// <param name="pageNumber">The page number to return.</param>
		/// <returns>An IPagination of T</returns>
        public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber, GridSortOptions sortOptions)
		{
            //return source.AsPagination(pageNumber, LazyPagination<T>.DefaultPageSize, sortOptions);
            return null;
		}

		/// <summary>
		/// Converts the specified IEnumerable into an IPagination using the specified page size and returns the specified page. 
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged list.</param>
		/// <param name="pageNumber">The page number to return.</param>
		/// <param name="pageSize">Number of objects per page.</param>
		/// <returns>An IPagination of T</returns>
        public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize, GridSortOptions sortOptions)
        {
            if (pageNumber < -1)
            {
                //throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
                pageNumber = 1;
            }

            //return new LazyPagination<T>(source.AsQueryable(), pageNumber, pageSize, sortOptions, new Dictionary<string, decimal>());
            return null;
        }

        /// <summary>
        /// Converts the specified IEnumerable into an IPagination using the specified page size and returns the specified page. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortOptions"></param>
        /// <returns></returns>
        public static IPagination<T> AsCustomPagination<T>(this IEnumerable<T> source, PagingModel pagingModel)
        {
            return new CustomPagination<T>(source, pagingModel);
        }

        /// <summary>
        /// 将IPagination<T>转换成PagedResultModel<T> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pagingModel"></param>
        /// <returns></returns>
        public static PagedResultModel<T> AsPagedResultModel<T>(this IEnumerable<T> source, PagingModel pagingModel) where T : class
        {
            var pagedList = new CustomPagination<T>(source, pagingModel);

            return new PagedResultModel<T>()
            {
                PageNumber = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                TotalPages = pagedList.TotalPages,
                TotalItems = pagedList.TotalItems,
                PageList = source.ToList<T>(),
                DicSum = pagedList.DicSum,
                SortOptions = pagedList.SortOptions,

            };
        }

        public static IPagination<T> AsCustomPagination<T>(this PagedResultModel<T> source) where T : class
        {
            var pagingModel = new PagingModel
            {
                PageIndex = source.PageNumber,
                PageSize = source.PageSize,
                TotalCount = source.TotalItems,
                DicSum = source.DicSum,
                SortOptions = source.SortOptions,
            };
            return new CustomPagination<T>(source.PageList, pagingModel);
        }
	}
}
