using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Routing;
using System.Collections;

namespace Hwa.Framework.Mvc.UI
{
    /// <summary>
    /// ToolbarItem
    /// </summary>
    public class ToolbarItem    
    {
        public ToolbarItem(string text):this(text,null)
        {
            this.Text = text;
        }

        public ToolbarItem(string text, Hashtable attibutes)
        {
            this.Text = text;
            this.Attibutes = attibutes;
        }

        public ToolbarItem(string text, Hashtable attibutes, ToolbarItem[] childItem)
        {
            this.Text = text;
            this.Attibutes = attibutes;
            this.ChildItem = childItem;
        }

        public string Text {get;set; }

        public Hashtable Attibutes { get; set; }

        public ToolbarItem[] ChildItem { get; set; }

    }

    /// <summary>
    /// Toolbar
    /// </summary>
    public class Toolbar :IHtmlString, IDisposable 
    {       
        private HtmlHelper _helper;        
        private IEnumerable<ToolbarItem> _toolbarItems;
        private string _block;
        private StringBuilder _builder = new StringBuilder();
        private bool _isQuery = true;
        private string _placeHolder = null;
 
        /// <summary>
        /// Toolbar
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="items"></param>
        /// <param name="isQuery"></param>
        /// <param name="block"></param>
        public Toolbar(HtmlHelper helper, IEnumerable<ToolbarItem> items, bool isQuery = true, string block = null,string placeHolder = null)
        {
            _helper = helper;
            _toolbarItems = items;
            _block = block;
            _isQuery = isQuery;
            _placeHolder = placeHolder;

            WriteStartTag();
            WriteToolBarItems();
            WriteEndTag();
        }

        /// <summary>
        /// WriteStartTag
        /// </summary>
        private void WriteStartTag()
        {
            _builder.Append(BuildStartTag());            
        }

        /// <summary>
        /// BuildStartTag
        /// </summary>
        /// <returns></returns>
        private string BuildStartTag()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<div class=""si-tbar"">{0}", Environment.NewLine);
            return builder.ToString();
        }

        /// <summary>
        /// WriteToolBarItems
        /// </summary>
        private void WriteToolBarItems()
        {          
            StringBuilder sb = new StringBuilder();
            //if (this._toolbarItems.Count() > 0)
            //{
                sb.AppendFormat(@"<ul>{0}", Environment.NewLine);
                foreach (ToolbarItem item in this._toolbarItems)
                {
                    bool haveMenu = item.ChildItem != null && item.ChildItem.Length > 0;
                    if (haveMenu)
                        sb.AppendFormat(@"<li class=""si-btn-menu""><a ");
                    else
                        sb.AppendFormat(@"<li><a ", Environment.NewLine, item.Text);
                    if (item.Attibutes != null)
                    {
                        foreach (var i in item.Attibutes.Keys)
                        {
                            if (i.ToString().ToLower().Equals("class"))
                                sb.AppendFormat(@" {0}=""si-btn {1}"" ", i, item.Attibutes[i]);
                            else
                                sb.AppendFormat(@" {0}=""{1}"" ", i, item.Attibutes[i]);
                        }
                    }
                    sb.AppendFormat(@"><b></b>{0}</a>", item.Text);
                    if (haveMenu)
                    {
                        sb.Append("<div>");
                        foreach (ToolbarItem child in item.ChildItem)
                        {
                            sb.Append("<a ");
                            foreach (var i in child.Attibutes.Keys)
                            {
                                sb.AppendFormat(@" {0}=""{1}"" ", i, child.Attibutes[i]);
                            }
                            sb.AppendFormat(@">{1}</a>{0}", Environment.NewLine, child.Text);
                        }
                        sb.Append("</div>");
                    }

                    sb.AppendFormat("</li>{0}", Environment.NewLine);
                }
                if (this._isQuery)
                {
                    sb.AppendFormat(@"<li class=""search""><input type=""text"" class=""search-text"" {0}  autofocus=""autofocus"" /><button class=""si-btn query"">查询</button></li>",
                        !string.IsNullOrEmpty(_placeHolder) ? "placeholder=\"" + _placeHolder + "\"" + " title=\"" + _placeHolder + "\"" : "");
                }

                if (!String.IsNullOrEmpty(this._block))
                    sb.Append(@"<li><a class=""adv"" id=""adv-search"">高级查询<i></i><b></b></a></li>");               
               
                sb.AppendFormat(@"</ul>{0}", Environment.NewLine);
                 if (!String.IsNullOrEmpty(this._block))
                     sb.AppendFormat(@"<div class=""adv-search"" id=""adv-search-panel"" style=""display:none;"">{0}</div>", this._block);     

                _builder.Append(sb.ToString());                           
            //}                                           
        }        

        /// <summary>
        /// WriteEndTag
        /// </summary>
        private void WriteEndTag()
        {
            _builder.Append("</div>");
        }

        /// <summary>
        /// Add items
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Toolbar Add(ToolbarItem item)
        {
            return this;
        }


        #region IDisposable Members
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="dispose"></param>
        public void Dispose(bool dispose)
        {
            if (dispose)
                WriteEndTag();
        }

        #endregion

        #region IHtmlString 成员
        /// <summary>
        /// ToHtmlString
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString()
        {
            return _builder.ToString();
        }

        #endregion
    }
}
