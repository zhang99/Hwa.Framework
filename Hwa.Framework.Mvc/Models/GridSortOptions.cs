
using Hwa.Framework.Mvc;

namespace Hwa.Framework.Mvc.Model
{
	/// <summary>
	/// Sorting information for use with the grid.
	/// </summary>
	public class GridSortOptions
	{
		public string Column { get; set; }
		public SortDirection Direction { get; set; }
	}

    /// <summary>
    /// Setting information for use with the grid.
    /// </summary>
    public class GridOptions
    {
        public const int SHEET_PAGE_SIZE = 8;
    }
}