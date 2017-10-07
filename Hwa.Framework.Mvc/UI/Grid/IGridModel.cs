using Hwa.Framework.Mvc.Data;
using Hwa.Framework.Mvc.Model;
using System.Collections.Generic;

namespace Hwa.Framework.Mvc.UI.Grid
{
	/// <summary>
	/// Defines a grid model
	/// </summary>
	public interface IGridModel<T> where T: class 
	{
		IGridRenderer<T> Renderer { get; set; }
		IList<GridColumn<T>> Columns { get; }
		IGridSections<T> Sections { get; }
		string EmptyText { get; set; }
        int EmptyRows { get; set; }
        bool ShowFooter { get; set; }
        bool AutoScroll { get; set; }
        bool ShowRowNumber { get; set; }
        bool ShowCheckBox { get; set; }
        bool ShowActive { get; set; }
		IDictionary<string, object> Attributes { get; set; }
		GridSortOptions SortOptions { get; set; }
		string SortPrefix { get; set; }
	}
}