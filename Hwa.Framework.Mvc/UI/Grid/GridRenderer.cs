using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Hwa.Framework.Mvc;
using Hwa.Framework.Mvc.Data.Pagination;
using Hwa.Framework.Mvc.Model;

namespace Hwa.Framework.Mvc.UI.Grid
{
	/// <summary>
	/// Base class for Grid Renderers. 
	/// </summary>
	public abstract class GridRenderer<T> : IGridRenderer<T> where T : class 
	{
		protected IGridModel<T> GridModel { get; private set; }
        protected IPagination<T> DataSource { get; private set; }
		protected ViewContext Context { get; private set; }
		private TextWriter _writer;
		private readonly ViewEngineCollection _engines;
        protected int _rowNum = 0;

		protected  TextWriter Writer
		{
			get { return _writer; }
		}

		protected GridRenderer() : this(ViewEngines.Engines) {}

		protected GridRenderer(ViewEngineCollection engines)
		{
			_engines = engines;
		}

        public void Render(IGridModel<T> gridModel, IPagination<T> dataSource, TextWriter output, ViewContext context)
		{
			_writer = output;
			GridModel = gridModel;
			DataSource = dataSource;
			Context = context;

			RenderGridStart();
			bool hasItems = RenderHeader();
            //单据编辑默认行数
            if (Context.ViewBag != null && Context.ViewBag.ControllerName != null && Context.ViewBag.ControllerName.EndsWith("Sheet"))
                GridModel.EmptyRows = Context.ViewBag.ActionName == "Create"
                                        ? GridOptions.SHEET_PAGE_SIZE : Context.ViewBag.ActionName == "Edit"
                                        ? DataSource.Count() < GridOptions.SHEET_PAGE_SIZE
                                        ? GridOptions.SHEET_PAGE_SIZE - DataSource.Count() : 1 : 0;

			if(hasItems)
			{
				RenderItems();
			}
			else
			{
				RenderEmpty();
			}

            if (GridModel.ShowFooter && (!IsDataSourceEmpty() || gridModel.EmptyRows > 0))
                RenderFooter();


			RenderGridEnd(!hasItems);
		}

		protected void RenderText(string text)
		{
			Writer.Write(text);
		}

        protected virtual void RenderItems()
        {
            if (IsDataSourceEmpty())
            {
                RenderEmpty();
                return;
            }

            RenderBodyStart();

            bool isAlternate = false;
            _rowNum = 0;
            foreach (var item in DataSource)
            {
                _rowNum++;
                RenderItem(new GridRowViewData<T>(item, isAlternate));
                isAlternate = !isAlternate;
            }

            RenderAdditionalEmptyRows();

            RenderBodyEnd();
        }

		protected virtual void RenderItem(GridRowViewData<T> rowData)
		{
			BaseRenderRowStart(rowData);
           
            int i = 0;
			foreach(var column in VisibleColumns())
			{                
				//A custom item section has been specified - render it and continue to the next iteration.
#pragma warning disable 612,618
				// TODO: CustomItemRenderer is obsolete in favour of custom columns. Remove this after next release.
				if (column.CustomItemRenderer != null)
				{
					column.CustomItemRenderer(new RenderingContext(Writer, Context, _engines), rowData.Item);
					continue;
				}
#pragma warning restore 612,618

				RenderStartCell(column, rowData);
                if (this.GridModel.ShowCheckBox && i == 0)
                    RenderCheckBox(column, rowData.Item);
                else
                    RenderCellValue(column, rowData);

                i++;

                if (i == VisibleColumns().Count())
                    i = 0;

				RenderEndCell();
			}

			BaseRenderRowEnd(rowData);
		}

        protected virtual void RenderCellValue(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            //序号
            if (!string.IsNullOrEmpty(column.Name) && (column.Name == "rowNum" || column.Name == "LineNum"))
            {
                RenderText(RenderRowNum());
                return;
            }
            //操作
            if (!string.IsNullOrEmpty(column.Name) && (column.Name == "operating"))
            {
                RenderText(RenderOperateColumn());
                return;
            }

            var cellValue = column.GetValue(rowData.Item);

            if (column.IsDropDownList)
            {
                StringBuilder listItems = new StringBuilder();
                string selectedValue = "";
                string selectedText = "";
                foreach (var m in column.DropDownListItems)
                {
                    listItems.AppendFormat(@"<div value=""{0}"" class=""combobox-item"">{1}</div>", m.Value, m.Text);
                    if (cellValue != null && cellValue.Equals(m.Value))
                    {
                        selectedValue = m.Value;
                        selectedText = m.Text;
                    }
                }
                
                cellValue = string.Format(@"<div class=""combobox"">
                    <span class=""combo-arrow""></span>
                    <span tabindex=""0"" class=""combo-text""><span>{2}</span></span>		            
		            <input type=""hidden"" name=""{0}Id"" id=""{0}Id"" class=""combo-value"" value=""{3}"">
		            <div class=""panel"">
			            <div class=""combo-panel panel-body"">
				            {1}
			            </div>                        
		            </div> 
                </div>", column.FieldName, listItems.ToString(), !string.IsNullOrEmpty(selectedText) ? selectedText : "--请选择--", selectedValue);
            }
            else if (column.IsDropDownTreeList)
            {
//                StringBuilder treelistItems = new StringBuilder();
//                SelectTreeListItem currentItem = null;
//                foreach (var m in column.DropDownTreeListItems)
//                {
//                    if (m.Selected) currentItem = m;
//                    if (m.ParentId != null && m.ParentId > 0) continue; 
//                    treelistItems.AppendFormat(@"<li id=""{0}"">
//                                    <span class=""{3}""></span>
//                                    <a {4}>[{1}]{2}</a>
//                                </li>", m.Id, m.Code, m.Name, m.IsLeaf == "Y" ? "unopen" : "open", m.Selected ? "class=\"current\"" : "");
//                }

//                cellValue = string.Format(@"<div class=""combobox combotree"">
//                    <span class=""combo-arrow""></span>
//                    <span tabindex=""0"" class=""combo-text""><span>{2}</span></span>		            
//		            <input type=""hidden"" name=""{0}Id"" id=""{0}Id"" class=""combo-value"" {3}>
//		            <div class=""panel"">
//			            <div class=""combo-panel panel-body"">
//                            <ul class=""si-tree"" relationship=""{0}"" controller=""{0}"">
//                                {1}       
//                            </ul>
//			            </div>
//		            </div> 
//                </div>"
//                    , column.FieldName
//                    , treelistItems.ToString()
//                    , currentItem != null ? currentItem.Name : "--请选择--"
//                    , currentItem != null ? string.Format("value=\"{0}\"", currentItem.Id) : "");
            }
            else if (cellValue == null || cellValue.ToString().IndexOf("<input ") < 0)
            {
                if (column.IsEditable && column.Selectable)
                    cellValue = string.Format("<input type=\"text\" value=\"{0}\" class=\"search-text\" ><a class=\"si-button search\"></a>", cellValue);
                else if (column.Selectable)
                    cellValue = string.Format("<input type=\"text\" value=\"{0}\" class=\"search-text\" disabled=\"disabled\"><a class=\"si-button search\"></a>", cellValue);
                else if (column.IsEditable)
                    cellValue = string.Format("<input type=\"text\" value=\"{0}\" >", cellValue);
            }

            if (cellValue != null)
            {
                RenderText(cellValue.ToString());
            }

        }

        private void RenderCheckBox(GridColumn<T> column, T instance)
        {
            if (column == null || string.IsNullOrWhiteSpace(column.FieldName))
                throw new Exception("FieldName不能为空.");

            Type type = instance.GetType();
            var cellValue = type.GetProperty(column.FieldName).GetValue(instance, null);
            string cellHtml = string.Format("<input type=\"checkbox\" value=\"{0}\">", cellValue);

            RenderText(cellHtml);
        }

        private string RenderRowNum()
        {
            if (DataSource == null)
                return _rowNum.ToString();

            return (_rowNum + (DataSource.PageNumber - 1) * DataSource.PageSize).ToString();
        }

        private string RenderOperateColumn()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<a href='#' class='add-line' title ='添加'></a>&nbsp;<a href='#' class='delete-line' title ='删除'></a>");

            return builder.ToString();
        }
        

		protected virtual bool RenderHeader()
		{
			//No items - do not render a header.
			//if(! ShouldRenderHeader()) return false;

			RenderHeadStart();

			foreach(var column in VisibleColumns())
			{

				//Allow for custom header overrides.
#pragma warning disable 612,618
				if(column.CustomHeaderRenderer != null)
				{
					column.CustomHeaderRenderer(new RenderingContext(Writer, Context, _engines));
				}
#pragma warning restore 612,618
				else
				{
					RenderHeaderCellStart(column);
                    RenderHeaderText(column);
					RenderHeaderCellEnd();
				}
			}

			RenderHeadEnd();

			return true;
		}

        protected virtual bool RenderFooter()
        {
            //No items - do not render a header.
            //if(! ShouldRenderHeader()) return false;

            RenderFootStart();

            foreach (var column in VisibleColumns())
            {
                RenderFooterCellStart(column);
                RenderFooterText(column);
                RenderFooterCellEnd();
            }

            RenderFootEnd();

            return true;
        }

        protected virtual void RenderHeaderText(GridColumn<T> column)
        {
			var customHeader = column.GetHeader();

			if (customHeader != null) 
			{
				RenderText(customHeader);
			}
			else 
			{

				RenderText(column.DisplayName);
			}
        }

        protected virtual void RenderFooterText(GridColumn<T> column)
        {
            var customFooter = column.GetFooter();

            if (customFooter != null)
            {
                RenderText(customFooter);
            }
            else
            {
                string value = string.Empty;
                if (!string.IsNullOrEmpty(column.Name) && (column.Name == "rowNum" || column.Name == "LineNum"))
                {
                    value = "合计";
                }
                else
                {
                    if (column.FieldName != null && DataSource != null && DataSource.DicSum != null & DataSource.DicSum.ContainsKey(column.FieldName))
                    {
                        if (!string.IsNullOrEmpty(column.FormatString))
                            value = string.Format(column.FormatString, DataSource.DicSum[column.FieldName]);
                        else
                            value = DataSource.DicSum[column.FieldName].ToString();                      
                    }
                }

                RenderText(value);
            }
        }

		protected virtual bool ShouldRenderHeader()
		{
			return !IsDataSourceEmpty();
		}

		protected bool IsDataSourceEmpty()
		{
			return DataSource == null || !DataSource.Any();
		}

		protected IEnumerable<GridColumn<T>> VisibleColumns()
		{
			return GridModel.Columns.Where(x => x.IsVisible);
		}

        protected int TotalWidth
        {
            get
            {
                return VisibleColumns().Where(c => c.IsHide != true).Sum(c => c.ColWidth) + 7*(VisibleColumns().Where(c => c.IsHide != true).Count()) + 1;
            }
        }

		protected void BaseRenderRowStart(GridRowViewData<T> rowData)
		{
			bool rendered = GridModel.Sections.Row.StartSectionRenderer(rowData, new RenderingContext(Writer, Context, _engines));

			if(! rendered)
			{
				RenderRowStart(rowData);
			}
		}

		protected void BaseRenderRowEnd(GridRowViewData<T> rowData)
		{
			bool rendered = GridModel.Sections.Row.EndSectionRenderer(rowData, new RenderingContext(Writer, Context, _engines));

			if(! rendered)
			{
				RenderRowEnd();
			}
		}

		protected bool IsSortingEnabled
		{
			get { return GridModel.SortOptions != null; }
		}

		protected abstract void RenderHeaderCellEnd();
		protected abstract void RenderHeaderCellStart(GridColumn<T> column);
		protected abstract void RenderRowStart(GridRowViewData<T> rowData);
		protected abstract void RenderRowEnd();
		protected abstract void RenderEndCell();
		protected abstract void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowViewData);
		protected abstract void RenderHeadStart();
		protected abstract void RenderHeadEnd();
		protected abstract void RenderGridStart();
		protected abstract void RenderGridEnd(bool isEmpty);
		protected abstract void RenderEmpty();
        protected abstract void RenderAdditionalEmptyRows();
		protected abstract void RenderBodyStart();
		protected abstract void RenderBodyEnd();
        protected abstract void RenderFooterCellStart(GridColumn<T> column);
        protected abstract void RenderFooterCellEnd();
        protected abstract void RenderFootStart();
        protected abstract void RenderFootEnd();
	}
}
