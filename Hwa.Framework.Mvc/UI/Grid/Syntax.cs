using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace Hwa.Framework.Mvc.UI.Grid.Syntax
{
	public interface IGrid<T> : IGridWithOptions<T> where T: class 
	{
		/// <summary>
		/// Specifies a custom GridModel to use.
		/// </summary>
		/// <param name="model">The GridModel storing information about this grid</param>
		/// <returns></returns>
		IGrid<T> WithModel(IGridModel<T> model);
	}

	public interface IGridWithOptions<T> : IHtmlString where T : class 
	{
		/// <summary>
		/// The GridModel that holds the internal representation of this grid.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)] //hide from fluent interface
		IGridModel<T> Model { get; }

		/// <summary>
		/// Specifies that the grid should be rendered using a specified renderer.
		/// </summary>
		/// <param name="renderer">Renderer to use</param>
		/// <returns></returns>
		IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer);

		/// <summary>
		/// Specifies the columns to use. 
		/// </summary>
		/// <param name="columnBuilder"></param>
		/// <returns></returns>
		IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder);

        /// <summary>
        /// Add additional empty rows.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        IGridWithOptions<T> AddEmptyRows(int rows);

        /// <summary>
        /// Show footer for the grid.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        IGridWithOptions<T> ShowFooter(bool isShowFooter);

        /// <summary>
        /// Show row number for the grid.
        /// </summary>
        /// <param name="isShowRowNumber"></param>
        /// <returns></returns>
        IGridWithOptions<T> ShowRowNumber(bool isShowRowNumber);

        /// <summary>
        /// Show check box for the grid.
        /// </summary>
        /// <param name="isShowCheckBox"></param>
        /// <returns></returns>
        IGridWithOptions<T> ShowCheckBox(bool isShowCheckBox);

        /// <summary>
        /// Show button add and delete for the grid.
        /// </summary>
        /// <param name="isShowActive"></param>
        /// <returns></returns>
        IGridWithOptions<T> ShowOperatingColumn(bool isShowActive);

        /// <summary>
        /// AutoScroll
        /// </summary>
        /// <param name="isAutoScroll"></param>
        /// <returns></returns>
        IGridWithOptions<T> AutoScroll(bool isAutoScroll);  

		/// <summary>
		/// Text to render when grid is empty.
		/// </summary>
		/// <param name="emptyText">Empty Text</param>
		/// <returns></returns>
		IGridWithOptions<T> Empty(string emptyText);

		/// <summary>
		/// Additional custom attributes
		/// </summary>
		/// <returns></returns>
		IGridWithOptions<T> Attributes(IDictionary<string, object> attributes);

		/// <summary>
		/// Additional custom attributes for each row
		/// </summary>
		/// <param name="attributes">Lambda expression that returns custom attributes for each row</param>
		/// <returns></returns>
		IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes);

		/// <summary>
		/// Additional custom attributes for the header row.
		/// </summary>
		/// <param name="attributes">Attributes for the header row</param>
		/// <returns></returns>
		IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes);


		/// <summary>
		/// Specifies that the grid is currently sorted
		/// </summary>
		IGridWithOptions<T> Sort(GridSortOptions sortOptions);

		/// <summary>
		/// Specifies that the grid is sorted. Column links will have the specified prefix prepended.
		/// </summary>
		IGridWithOptions<T> Sort(GridSortOptions sortOptions, string prefix); 

		/// <summary>
		/// Renders the grid to the TextWriter specified at creation
		/// </summary>
		/// <returns></returns>
		void Render();

	}
}