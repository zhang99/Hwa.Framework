using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;

namespace Hwa.Framework.Mvc.UI.Grid
{
	/// <summary>
	/// Default model for grid
	/// </summary>
	public class GridModel<T>  : IGridModel<T> where T : class
	{
		private readonly ColumnBuilder<T> _columnBuilder;
		private readonly GridSections<T> _sections = new GridSections<T>();
		private IGridRenderer<T> _renderer = new HtmlTableGridRenderer<T>();
		private string _emptyText;
        private int _emptyRows = 0;
        private bool _showFooter = false;
        private bool _showRowNumber = false;
        private bool _showCheckBox = false;
        private bool _autoScroll = false;
        private bool _showActive = false;
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();
		private GridSortOptions _sortOptions;
		private string _sortPrefix;

		GridSortOptions IGridModel<T>.SortOptions
		{
			get { return _sortOptions; }
			set { _sortOptions = value; }
		}

		IList<GridColumn<T>> IGridModel<T>.Columns
		{
			get { return _columnBuilder; }
		}

		IGridRenderer<T> IGridModel<T>.Renderer
		{
			get { return _renderer; }
			set { _renderer = value; }
		}

		string IGridModel<T>.EmptyText
		{
			get { return _emptyText; }
			set { _emptyText = value; }
		}

        bool IGridModel<T>.ShowFooter
        {
            get { return _showFooter; }
            set { _showFooter = value; } 
        }

        bool IGridModel<T>.ShowRowNumber
        {
            get { return _showRowNumber; }
            set { _showRowNumber = value; }
        }

        bool IGridModel<T>.ShowActive
        {
            get { return _showActive; }
            set { _showActive = value; }
        }

        bool IGridModel<T>.ShowCheckBox
        {
            get { return _showCheckBox; }
            set { _showCheckBox = value; }
        }

        bool IGridModel<T>.AutoScroll
        {
            get { return _autoScroll; }
            set { _autoScroll = value; }
        }

        int IGridModel<T>.EmptyRows
        {
            get { return _emptyRows; }
            set { _emptyRows = value; }
        }

		IDictionary<string, object> IGridModel<T>.Attributes
		{
			get { return _attributes; }
			set { _attributes = value; }
		}

		string IGridModel<T>.SortPrefix
		{
			get { return _sortPrefix; }
			set { _sortPrefix = value; }
		}

		/// <summary>
		/// Creates a new instance of the GridModel class
		/// </summary>
		public GridModel()
		{
			_emptyText = "没有任何数据.";
			_columnBuilder = CreateColumnBuilder();
		}

		/// <summary>
		/// Column builder for this grid model
		/// </summary>
		public ColumnBuilder<T> Column
		{
			get { return _columnBuilder; }
		}

		/// <summary>
		/// Section overrides for this grid model.
		/// </summary>
		IGridSections<T> IGridModel<T>.Sections
		{
			get { return _sections; }
		}

		/// <summary>
		/// Section overrides for this grid model.
		/// </summary>
		public GridSections<T> Sections
		{
			get { return _sections; }
		}

		/// <summary>
		/// Text that will be displayed when the grid has no data.
		/// </summary>
		/// <param name="emptyText">Text to display</param>
		public void Empty(string emptyText)
		{
			_emptyText = emptyText;
		}        

		/// <summary>
		/// Defines additional attributes for the grid.
		/// </summary>
		/// <param name="hash"></param>
		public void Attributes(params Func<object, object>[] hash)
		{
			Attributes(new Hash(hash));
		}

		/// <summary>
		/// Defines additional attributes for the grid
		/// </summary>
		/// <param name="attributes"></param>
		public void Attributes(IDictionary<string, object> attributes)
		{
			_attributes = attributes;
		}

		/// <summary>
		/// Specifies the Renderer to use with this grid. If omitted, the HtmlTableGridRenderer will be used. 
		/// </summary>
		/// <param name="renderer">The Renderer to use</param>
		public void RenderUsing(IGridRenderer<T> renderer)
		{
			_renderer = renderer;
		}

		/// <summary>
		/// Secifies that the grid is currently being sorted by the specified column in a particular direction.
		/// </summary>
		public void Sort(GridSortOptions sortOptions)
		{
			_sortOptions = sortOptions;
		}

		/// <summary>
		/// Specifies that the grid is currently being sorted by the specified column in a particular direction.
		/// This overload allows you to specify a prefix.
		/// </summary>
		public void Sort(GridSortOptions sortOptions, string prefix)
		{
			_sortOptions = sortOptions;
			_sortPrefix = prefix;
		}

		protected virtual ColumnBuilder<T> CreateColumnBuilder()
		{
			return new ColumnBuilder<T>();
		}
	}
}
