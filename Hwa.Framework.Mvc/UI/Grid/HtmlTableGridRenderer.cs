using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hwa.Framework.Mvc;
using Hwa.Framework.Mvc.Model;


namespace Hwa.Framework.Mvc.UI.Grid
{
	/// <summary>
	/// Renders a grid as an HTML table.
	/// </summary>
	public class HtmlTableGridRenderer<T> : GridRenderer<T> where T: class 
	{
		private const string DefaultCssClass = "grid";

		public HtmlTableGridRenderer(ViewEngineCollection engines) : base(engines)
		{
			
		}
		public HtmlTableGridRenderer() {}

		protected override void RenderHeaderCellEnd()
		{
            RenderText("<span></span><i class=\"wd-resize\"></i></div></th>");
		}

		protected virtual void RenderEmptyHeaderCellStart()
		{
			RenderText("<th>");
		}

		protected override void RenderHeaderCellStart(GridColumn<T> column) 
		{
			var attributes = new Dictionary<string, object>(column.HeaderAttributes);

			if(IsSortingEnabled && column.IsSortable)
			{
				//bool isSortedByThisColumn = (GridModel.SortOptions.Column == GenerateSortColumnName(column));
                bool isSortedByThisColumn = (GridModel.SortOptions.Column == column.FieldName);

				if (isSortedByThisColumn) 
				{
                    string sortClass = GridModel.SortOptions.Direction == SortDirection.Ascending ? "grid-sort-asc" : "grid-sort-desc";

					if(attributes.ContainsKey("class") && attributes["class"] != null)
					{
						sortClass = string.Join(" ", new[] { attributes["class"].ToString(), sortClass });
					}

					attributes["class"] = sortClass;
				}
			}

            //add by zhangh 2013/05/31 排序列设置
            if (column.IsSortable)
                attributes["sortable"] = "true";

            if (column.IsQueryable)
                attributes["queryable"] = "true";

            if (column.IsEditable)
                attributes["editable"] = "true";

            if (column.Selectable)
                attributes["selectable"] = "true";

            if(column.Reliable)
                attributes["reliable"] = "true";

            if(column.IsSumColumn)
                attributes["issum"] = "true";

            if (column.IsPercent)
                attributes["ispercent"] = "true";

            if(!string.IsNullOrEmpty(column.ColumnEqualTo))
                attributes["equalto"] = column.ColumnEqualTo;

            attributes["field"] = column.FieldName;

            if (!column.IsHide)
               // attributes["width"] = column.ColWidth;
                attributes.Add("style", string.Format("width:{0}px", column.ColWidth));


			string attrs = BuildHtmlAttributes(attributes);

			if (attrs.Length > 0)
				attrs = " " + attrs;

            RenderText(string.Format("<th{0}><div>", attrs));
		}

        protected override void RenderFooterCellStart(GridColumn<T> column)
        {
            var attributes = new Dictionary<string, object>(column.FooterAttributes);
            if (column.Alignment == Alignment.Center)
                attributes.Add("class", "text-align-center");
            else if (column.Alignment == Alignment.Right)
                attributes.Add("class", "text-align-right");
            else
                attributes.Add("class", "text-align-left");

            attributes["field"] = column.FieldName;
            //attributes["width"] = column.ColWidth;
            if (attributes.ContainsKey("style"))
            {
                string oldStyle = attributes["style"].ToString();
                //attributes.Add("style", string.Format("{0}; width:{1}px", oldStyle, column.ColWidth));
                attributes["style"] = string.Format("{0}; width:{1}px", oldStyle, column.ColWidth);
            }
            else
            {
                attributes.Add("style", string.Format("width:{0}px", column.ColWidth));
            }
            

            if (column.IsSumColumn)
                attributes["issum"] = "true";

            string attrs = BuildHtmlAttributes(attributes);

            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<td{0}>", attrs));
        }

        protected override void RenderFooterCellEnd()
        {
            RenderText("</td>");
        }

		protected override void RenderHeaderText(GridColumn<T> column) 
		{
            //modify by zhangh 2013/05/31 修改排序
            //if(IsSortingEnabled && column.Sortable)
            //{
            //    string sortColumnName = GenerateSortColumnName(column);

            //    bool isSortedByThisColumn = GridModel.SortOptions.Column == sortColumnName;

            //    var sortOptions = new GridSortOptions 
            //    {
            //        Column = sortColumnName
            //    };

            //    if(isSortedByThisColumn)
            //    {
            //        sortOptions.Direction = (GridModel.SortOptions.Direction == SortDirection.Ascending)
            //            ? SortDirection.Descending 
            //            : SortDirection.Ascending;
            //    }
            //    else //default sort order
            //    {
            //        sortOptions.Direction = column.InitialDirection ?? GridModel.SortOptions.Direction;
            //    }

            //    var routeValues = CreateRouteValuesForSortOptions(sortOptions, GridModel.SortPrefix);

            //    //Re-add existing querystring
            //    foreach(var key in Context.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
            //    {
            //        if(! routeValues.ContainsKey(key))
            //        {
            //            routeValues[key] = Context.RequestContext.HttpContext.Request.QueryString[key];
            //        }
            //    }

            //    var link = HtmlHelper.GenerateLink(Context.RequestContext, RouteTable.Routes, column.DisplayName, null, null, null, routeValues, null);
            //    RenderText(link);
            //}
            //else
            //{
				base.RenderHeaderText(column);
			//}
		}

		private RouteValueDictionary CreateRouteValuesForSortOptions(GridSortOptions sortOptions, string prefix)
		{
			if(string.IsNullOrEmpty(prefix))
			{
				return new RouteValueDictionary(sortOptions);
			}

			//There must be a nice way to do this...
			return new RouteValueDictionary(new Dictionary<string, object>()
			{
				{ prefix + "." + "Column", sortOptions.Column },
				{ prefix + "." + "Direction", sortOptions.Direction }
			});
		}

		protected virtual string GenerateSortColumnName(GridColumn<T> column)
		{
			//Use the explicit sort column name if specified. If not possible, fall back to the property name.
			//If the property name cannot be inferred (ie the expression is not a MemberExpression) then try the display name instead.
			return column.SortColumnName ?? column.Name ?? column.DisplayName;
		}

		protected override void RenderRowStart(GridRowViewData<T> rowData)
		{
			var attributes = GridModel.Sections.Row.Attributes(rowData);

			if(! attributes.ContainsKey("class"))
			{
				attributes["class"] = rowData.IsAlternate ? "gridrow_alternate" : "gridrow";
			}

			string attributeString = BuildHtmlAttributes(attributes);

			if(attributeString.Length > 0)
			{
				attributeString = " " + attributeString;	
			}

			RenderText(string.Format("<tr{0}>", attributeString));
		}

		protected override void RenderRowEnd()
		{
			RenderText("</tr>");
		}

		protected override void RenderEndCell()
		{
			RenderText("</td>");
		}

		protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
		{
            //add by zhangh 2013/06/17 增加对齐方式
            IDictionary<string, object> attributes = column.Attributes(rowData);
            if (column.Alignment == Alignment.Center)
                attributes.Add("class", "text-align-center");
            else if (column.Alignment == Alignment.Right)
                attributes.Add("class", "text-align-right");
            else
                attributes.Add("class", "text-align-left");

            attributes.Add("field", column.FieldName);
            if (!column.IsHide)
                //attributes.Add("width", column.ColWidth);
                attributes.Add("style", string.Format("width:{0}px", column.ColWidth));

            if (column.IsEditable)
                attributes.Add("editable", "true");

            if (column.Selectable)
                attributes.Add("selectable", "true");

            if (column.Reliable)
                attributes.Add("reliable", "true");

            if (column.IsSumColumn)
                attributes.Add("issum", "true");

            if (column.IsPercent)
                attributes["ispercent"] = "true";

            if (!string.IsNullOrEmpty(column.ColumnEqualTo))
                attributes.Add("equalto", column.ColumnEqualTo);

            string attrs = BuildHtmlAttributes(attributes);
			if (attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<td{0}>", attrs));
		}

		protected override void RenderHeadStart()
		{
			string attributes = BuildHtmlAttributes(GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, false)));
			if(attributes.Length > 0)
			{
				attributes = " " + attributes;
			}
            if (GridModel.AutoScroll)
                RenderText(string.Format("<thead class=\"table-head\" style=\"width:{0}px \"><tr{1}>", (TotalWidth - 1).ToString(), attributes));
            else
                RenderText(string.Format("<thead><tr{0}>", attributes));
		}

        protected override void RenderFootStart()
        {
            string attributes = BuildHtmlAttributes(GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, false)));
            if (attributes.Length > 0)
            {
                attributes = " " + attributes;
            }
            if (GridModel.AutoScroll)
                RenderText(string.Format("<tfoot class=\"table-foot\" style=\"width:{0}px \"><tr{1}>", (TotalWidth - 1).ToString(), attributes));
            else
                RenderText(string.Format("<tfoot><tr{0}>", attributes));
        }

		protected override void RenderHeadEnd()
		{
			RenderText("</tr></thead>");
		}

        protected override void RenderFootEnd()
        {
            RenderText("</tr></tfoot>");
        }

        protected override void RenderGridStart()
        {
            //modify by zhangh 2013/05/31 table不设置class
            //if(! GridModel.Attributes.ContainsKey("class"))
            //{
            //    GridModel.Attributes["class"] = DefaultCssClass;
            //}  

            //空数据，让Table100%显示 add by zhangh 2013/06/03
            //if (!ShouldRenderHeader())
            //{
            //GridModel.Attributes.Add("width", "100%");
            //}
            //else
            //{
            //int w = TotalWidth + VisibleColumns().Where(c => c.IsHide != true).Count() + 1;
           // GridModel.Attributes.Add("width", TotalWidth.ToString() + "px");
            GridModel.Attributes.Add("style", string.Format("width:{0}px", TotalWidth.ToString()));
            //}

            string attrs = BuildHtmlAttributes(GridModel.Attributes);
            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<div class=\"grid-inner\"><table{0}>", attrs));
        }

		protected override void RenderGridEnd(bool isEmpty)
		{
            RenderText("</table></div>");
		}

		protected override void RenderEmpty()
		{
            RenderBodyStart();

            if (GridModel.EmptyRows == 0)
            {
                //RenderHeadStart();
                //RenderEmptyHeaderCellStart();
                //RenderHeaderCellEnd();
                //RenderHeadEnd();            
                RenderText("<tr><td colspan='" + VisibleColumns().Where(c => c.IsHide != true).Count() + "'>" + GridModel.EmptyText + "</td></tr>");
            }
            else
            {
                RenderAdditionalEmptyRows();
            }

            RenderBodyEnd();
		}

        protected override void RenderAdditionalEmptyRows()
        {
            bool isAlternate = false;
            for (int i = 0; i < GridModel.EmptyRows; i++)
            {
                _rowNum++;
                RenderItem(new GridRowViewData<T>(null, isAlternate));
                isAlternate = !isAlternate;
            }
        }

		protected override void RenderBodyStart() 
		{
            if (GridModel.AutoScroll) 
                RenderText(string.Format("<tbody class=\"table-body\" style=\"width:{0}px \">", (TotalWidth -1).ToString()));
            else
                RenderText("<tbody>");
		}

		protected override void RenderBodyEnd() 
		{
			RenderText("</tbody>");
		} 

		/// <summary>
		/// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
		/// </summary>
		/// <returns></returns>
		protected string BuildHtmlAttributes(IDictionary<string, object> attributes)
		{
			return DictionaryExtensions.ToHtmlAttributes(attributes);
		}
	}
}