using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hwa.Framework.Mvc;
using System.Web.Mvc;


namespace Hwa.Framework.Mvc.UI.Grid
{

    /// <summary>
	/// Grid Column fluent interface
	/// </summary>
	public interface IGridColumn<T>
	{
		/// <summary>
		/// Specified an explicit name for the column.
		/// </summary>
		/// <param name="name">Name of column</param>
		/// <returns></returns>
		IGridColumn<T> Named(string name);
        /// <summary>
        /// hide the column.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IGridColumn<T> Hide(bool isHide);
        /// <summary>
        /// Reliable flag column used for data validation. 
        /// </summary>
        /// <param name="isReliable"></param>
        /// <returns></returns>
        IGridColumn<T> Reliable(bool isReliable);        
        /// <summary>
        /// alignment
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        IGridColumn<T> Align(Alignment alignment);
        /// <summary>
        /// width of the column.
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        IGridColumn<T> Width(int width);
        /// <summary>
        /// row number column.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IGridColumn<T> IsRowNumberColumn(bool isRowNumCol);
        /// <summary>
        /// operate column
        /// </summary>
        /// <param name="isOperateCol"></param>
        /// <returns></returns>
        IGridColumn<T> IsOperateColumn(bool isOperateCol);
		/// <summary>
		/// If the property name is PascalCased, it should not be split part.
		/// </summary>
		/// <returns></returns>
		IGridColumn<T> DoNotSplit();
		/// <summary>
		/// A custom format to use when building the cell's value
		/// </summary>
		/// <param name="format">Format to use</param>
		/// <returns></returns>
		IGridColumn<T> Format(string format);
		/// <summary>
		/// Delegate used to hide the contents of the cells in a column.
		/// </summary>
		IGridColumn<T> CellCondition(Func<T, bool> func);

		/// <summary>
		/// Determines whether the column should be displayed
		/// </summary>
		/// <param name="isVisible"></param>
		/// <returns></returns>
		IGridColumn<T> Visible(bool isVisible);

		IGridColumn<T> Header(Func<object, object> customHeaderRenderer);

		/// <summary>
		/// Determines whether or not the column should be encoded. Default is true.
		/// </summary>
		IGridColumn<T> Encode(bool shouldEncode);

		/// <summary>
		/// Do not HTML Encode the output
		/// </summary>
		/// <returns></returns>
		[Obsolete("Use Encode(false) instead.")]
		IGridColumn<T> DoNotEncode(); //TODO: Jeremy to remove after next release.

		/// <summary>
		/// Defines additional attributes for the column heading.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		IGridColumn<T> HeaderAttributes(IDictionary<string, object> attributes);

        /// <summary>
        /// Defines additional attributes for the column footer.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IGridColumn<T> FooterAttributes(IDictionary<string, object> attributes);

		/// <summary>
		/// Defines additional attributes for the cell. 
		/// </summary>
		/// <param name="attributes">Lambda expression that should return a dictionary containing the attributes for the cell</param>
		/// <returns></returns>
		IGridColumn<T> Attributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes);

		/// <summary>
		/// Specifies whether or not this column should be sortable. 
		/// The default is true. 
		/// </summary>
		/// <param name="isColumnSortable"></param>
		/// <returns></returns>
		IGridColumn<T> Sortable(bool isColumnSortable);

        /// <summary>
        /// Specifies whether or not this column should be queryable. 
        /// The default is true. 
        /// </summary>
        /// <param name="isColumnQueryable"></param>
        /// <returns></returns>
        IGridColumn<T> Queryable(bool isColumnQueryable);

        /// <summary>
        /// Specifies whether or not this column should be queryable. 
        /// The default is false. 
        /// </summary>
        /// <param name="isEditable"></param>
        /// <returns></returns>
        IGridColumn<T> Editable(bool isEditable);

        /// <summary>
        /// Show the column as dropdown list.
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        IGridColumn<T> ShowAsDropDownList(IEnumerable<SelectListItem> listItem);

        /// <summary>
        /// Show the column as dropdown tree list.
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        //IGridColumn<T> ShowAsDropDownTreeList(IEnumerable<SelectTreeListItem> treeListItem);  
    
        /// <summary>
        /// IsSum
        /// </summary>
        /// <param name="isSum"></param>
        /// <returns></returns>
        IGridColumn<T> IsSum(bool isSum);        

        /// <summary>
        /// Selectable
        /// </summary>
        /// <param name="isSelectable"></param>
        /// <returns></returns>
        IGridColumn<T> Selectable(bool isSelectable);

        /// <summary>
        /// EqualTo
        /// </summary>
        /// <param name="equalToField"></param>
        /// <returns></returns>
        IGridColumn<T> EqualTo(string equalToField);        

		/// <summary>
		/// Specifies a custom name that should be used when sorting on this column
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IGridColumn<T> SortColumnName(string name);

		/// <summary>
		/// Specifies the direction of the sort link when this column is not currently sorted.  
		/// The direction will continue to toggle when it is the currently sorted column. 
		/// </summary>
		/// <param name="initialDirection"></param>
		/// <returns></returns>
		IGridColumn<T> SortInitialDirection(SortDirection initialDirection);

		/// <summary>
		/// Custom header renderer
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)] //hide from intellisense in fluent interface
		[Obsolete("CustomHeaderRenderer has been deprecated. Please use Header instead.")]
		Action<RenderingContext> CustomHeaderRenderer { get; set; }

		/// <summary>
		/// Custom item renderer
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)] //hide from intellisense in fluent interface
		[Obsolete("CustomItemRenderer has been deprecated. Please use column.Custom instead.")]
		Action<RenderingContext, T> CustomItemRenderer { get; set; }

		/// <summary>
		/// Specifies the position of a column. 
		/// This is usually used in conjunction with the AutoGenerateColumns method 
		/// in order to specify where additional custom columns should be placed.
		/// </summary>
		/// <param name="index">The index at which the column should be inserted</param>
		IGridColumn<T> InsertAt(int index);
	}
}