using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using Hwa.Framework.Mvc;
using Hwa.Framework.Mvc.UI.Grid.Syntax;
using Hwa.Framework.Mvc.Annotations;
using Hwa.Framework.Mvc.Data.Pagination;
using Hwa.Framework.Mvc.Annotations.GridAnnotations;

using Hwa.Framework.Mvc.Model;
using Hwa.Framework.Util;

namespace Hwa.Framework.Mvc.UI.Grid
{  
	/// <summary>
	/// Defines a grid to be rendered.
	/// </summary>
	/// <typeparam name="T">Type of datasource for the grid</typeparam>
    public class Grid<T> : IGrid<T> where T : class
	{
		private readonly ViewContext _context;
		private IGridModel<T> _gridModel = new GridModel<T>();

		/// <summary>
		/// The GridModel that holds the internal representation of this grid.
		/// </summary>
		public IGridModel<T> Model
		{
			get { return _gridModel; }
		}

		/// <summary>
		/// Creates a new instance of the Grid class.
		/// </summary>
		/// <param name="dataSource">The datasource for the grid</param>
		/// <param name="context"></param>
        public Grid(IPagination<T> dataSource, ViewContext context)
        {
            this._context = context;
            DataSource = dataSource;
            if (dataSource != null)
                _gridModel.SortOptions = dataSource.SortOptions;
        }

        public Grid(IPagination<dynamic> dataSource, ViewContext context)
        {
            this._context = context;
            DataSourceDynamic = dataSource;
            if (dataSource != null)
                _gridModel.SortOptions = dataSource.SortOptions;
        }

		/// <summary>
		/// The datasource for the grid.
		/// </summary>
        public IPagination<T> DataSource { get; private set; }

        public IPagination<dynamic> DataSourceDynamic { get; private set; }

		public IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer)
		{
			_gridModel.Renderer = renderer;
			return this;
		}

		public IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder)
		{
			var builder = new ColumnBuilder<T>();

            var html = new HtmlHelper(_context, new ViewPage());
            //显示Checkbox
            //if (Model.ShowCheckBox)
            //    builder.For(f => html.SiCheckBox(typeof(T).Name, ""),"Id")
            //        .Header(x => html.SiCheckBox(typeof(T).Name, ""))
            //        .Sortable(false)
            //        .Align(Alignment.Center)
            //        .Width(30)
            //        .Queryable(false);

            //显示行号
            if (Model.ShowRowNumber)
                builder.For(x => "").IsRowNumberColumn(true).Align(Alignment.Center).Width(30).Queryable(false);

            //显示添加删除
            //if (Model.ShowActive)
            //    builder.For(f => html.SiPlusAndMinus())
            //        .IsOperateColumn(true)
            //        .Header(x => "操作")
            //        .Sortable(false)
            //        .Align(Alignment.Center)
            //        .Width(30)
            //        .Queryable(false);

			columnBuilder(builder);
            
			foreach (var column in builder)
			{
                //列初始化
                InitColumns(column);

				if (column.Position == null) 
				{
					_gridModel.Columns.Add(column);
				} 
				else
				{
					_gridModel.Columns.Insert(column.Position.Value, column);	
				}

                if (DataSource != null && column.IsSumColumn)
                {
                    if (DataSource.DicSum == null)
                        DataSource.DicSum = new Dictionary<string, decimal>();

                    if (!DataSource.DicSum.Keys.Contains(column.FieldName))
                        DataSource.DicSum.Add(new KeyValuePair<string, decimal>(column.FieldName, 0M));
                }
            }

			return this;
		}

        /// <summary>
        /// 列初始化
        /// </summary>
        /// <param name="column"></param>
        public void InitColumns(GridColumn<T> column)
        {
            if (column == null || column.FieldName == null)
                return;
            
            //PropertyInfo pi = typeof(T).GetProperty(column.FieldName.Split(new char[] { '.' })[0]);
            PropertyInfo pi = EntityHelper.GetFieldProperty(typeof(T),column.FieldName);// pi edit 2014-01-27
            if (pi == null) return;

            //宽度
            //Attribute attr = Attribute.GetCustomAttribute(pi, typeof(WidthAttribute), false);
            //if (attr != null)
            //    column.Width(((WidthAttribute)attr).Width);            

            //对齐方式
            Attribute attr = Attribute.GetCustomAttribute(pi, typeof(AlignAttribute), false);
            if (attr != null)
                column.Align(((AlignAttribute)attr).Alignment);            

            //可否查询
            attr = Attribute.GetCustomAttribute(pi, typeof(QueryableAttribute), false);
            if (attr != null)
                column.Queryable(((QueryableAttribute)attr).Queryable);

            //可否排序
            attr = Attribute.GetCustomAttribute(pi, typeof(SortableAttribute), false);
            if (attr != null)
                column.Sortable(((SortableAttribute)attr).Sortable);

            //是否显示到页面
            attr = Attribute.GetCustomAttribute(pi, typeof(VisibleAttribute), false);
            if (attr != null)
                column.Visible(((VisibleAttribute)attr).Visible);

            //是否隐藏
            attr = Attribute.GetCustomAttribute(pi, typeof(HideAttribute), false);
            if (attr != null)
                column.Hide(((HideAttribute)attr).IsHidden);

            //可否编辑
            attr = Attribute.GetCustomAttribute(pi, typeof(EditableColumnAttribute), false);
            if (attr != null)
                column.Editable(((EditableColumnAttribute)attr).Editable);

            //默认等于指定列
            attr = Attribute.GetCustomAttribute(pi, typeof(EqualToAttribute), false);
            if (attr != null)
                column.EqualTo(((EqualToAttribute)attr).EqualTo);

            //是否合计列
            attr = Attribute.GetCustomAttribute(pi, typeof(IsSumAttribute), false);
            if (attr != null)
                column.IsSum(((IsSumAttribute)attr).IsSum);

            //是否为百分比列
            attr = Attribute.GetCustomAttribute(pi, typeof(IsPercentAttribute), false);
            if (attr != null)
                column.Percent(((IsPercentAttribute)attr).IsPercent);

            //是否有多个值
            attr = Attribute.GetCustomAttribute(pi, typeof(ValuesAttribute), false);
            if (attr != null)
                column.MultiValues(((ValuesAttribute)attr).Value);

        }

        public IGridWithOptions<T> AddEmptyRows(int rows)
        {
            _gridModel.EmptyRows = rows;
            return this;
        }

        public IGridWithOptions<T> ShowFooter(bool isShowFooter)
        {
            _gridModel.ShowFooter = isShowFooter;
            return this;
        }

        public IGridWithOptions<T> AutoScroll(bool isAutoScroll)
        {
            _gridModel.AutoScroll = isAutoScroll;
            return this;
        }

        public IGridWithOptions<T> ShowRowNumber(bool isShowRowNumber)
        {
            _gridModel.ShowRowNumber = isShowRowNumber;
            return this;
        }

        public IGridWithOptions<T> ShowCheckBox(bool isShowCheckBox)
        {
            _gridModel.ShowCheckBox = isShowCheckBox;
            return this;
        }

        public IGridWithOptions<T> ShowOperatingColumn(bool isShowActive)
        {
            _gridModel.ShowActive = isShowActive;
            return this;
        }

		public IGridWithOptions<T> Empty(string emptyText)
		{
			_gridModel.EmptyText = emptyText;
			return this;
		}

		public IGridWithOptions<T> Attributes(IDictionary<string, object> attributes)
		{
			_gridModel.Attributes = attributes;
			return this;
		}

		public IGrid<T> WithModel(IGridModel<T> model)
		{
			_gridModel = model;
			return this;
		}

		public IGridWithOptions<T> Sort(GridSortOptions sortOptions)
		{
			_gridModel.SortOptions = sortOptions;
			return this;
		}

		public IGridWithOptions<T> Sort(GridSortOptions sortOptions, string prefix)
		{
			_gridModel.SortOptions = sortOptions;
			_gridModel.SortPrefix = prefix;
			return this;
		}

		public override string ToString()
		{
			return ToHtmlString();
		}

		public string ToHtmlString()
		{
			var writer = new StringWriter();
			_gridModel.Renderer.Render(_gridModel, DataSource, writer, _context);
			return writer.ToString();
		}

		public IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes)
		{
			_gridModel.Sections.HeaderRowAttributes(attributes);
			return this;
		}

		[Obsolete("The Render method is deprecated. From within a Razor view, use @Html.Grid() without a call to Render.")]
		public void Render()
		{
			_gridModel.Renderer.Render(_gridModel, DataSource, _context.Writer, _context);
		}

		public IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
		{
			_gridModel.Sections.RowAttributes(attributes);
			return this;
		}
	}
}