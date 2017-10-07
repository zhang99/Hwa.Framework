using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc
{
    public static class HtmlToolHelper
    {
        #region 保存为Html静态文件

        /// <summary>
        /// 保存为Html静态文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool SaveAsHtml(string path, string content, string title)
        {
            return SaveAsHtml(path, content, title, true, string.Empty);
        }

        /// <summary>
        /// 保存为Html静态文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="appendBody"></param>
        /// <returns></returns>
        public static bool SaveAsHtml(string path, string content, string title, bool appendBody)
        {
            return SaveAsHtml(path, content, title, appendBody, string.Empty);
        }

        /// <summary>
        /// 保存为Html静态文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="appendBody"></param>
        /// <param name="headHtml"></param>
        /// <returns></returns>
        public static bool SaveAsHtml(string path, string content, bool appendBody, string headHtml)
        {
            return SaveAsHtml(path, content, "", appendBody, headHtml);
        }

        /// <summary>
        /// 保存为Html静态文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool SaveAsHtml(string path, string content, string title, bool appendBody, string headHtml)
        {
            const string htmlTemplate = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" >
{$Head}
{$Content}
</html>
";
            if (string.IsNullOrEmpty(headHtml))
                headHtml =
@"<head>
    <title>{$Title}</title>
</head>".Replace("{$Title}", title);

            string html = htmlTemplate.Replace("{$Head}", headHtml).Replace("{$Content}", appendBody ? "<body>" + content + "</body>" : content);

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
            {
                using (System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(writer))
                {
                    writer.Write(html);
                    htmlWriter.Flush();
                    htmlWriter.Close();
                }
            }

            return true;
        }

        #endregion

        #region 替换Html指定节点的内容

        /// <summary>
        /// 替换Html指定节点的内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nodeFilter">筛选条件</param>
        /// <param name="replaceHtml">待替换的innerhtml</param>
        /// <param name="savePath">待保存的路径(为空则替换原文件)</param>
        public static void ReplaceHtmlNode(string path, string nodeFilter, string replaceHtml, string savePath = null)
        {
            if (!File.Exists(path)) throw new Exception("指定的文件不存在!");

            string documentHtml = "";
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                documentHtml = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(documentHtml)) throw new Exception("文档内容为空!");

            var Document = new HtmlDocument();
            Document.LoadHtml(documentHtml);

            var htmlNode = Document.DocumentNode;
            var hnc = htmlNode.SelectSingleNode(nodeFilter);

            if (hnc == null) throw new Exception("未找到指定节点!");

            hnc.InnerHtml = replaceHtml;
            path = savePath ?? path;
            Document.Save(path, Encoding.UTF8);
        }

        /// <summary>
        /// 获取Html指定节点的内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nodeFilter">筛选条件</param>
        /// <param name="containSelfNode">是否包含节点本身</param>
        public static string GetHtmlNode(string path, string nodeFilter, bool containSelfNode = false)
        {
            if (!File.Exists(path)) throw new Exception("指定的文件不存在!");

            string documentHtml = "";
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                documentHtml = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(documentHtml)) throw new Exception("文档内容为空!");

            var Document = new HtmlDocument();
            Document.LoadHtml(documentHtml);

            var htmlNode = Document.DocumentNode;
            var hnc = htmlNode.SelectSingleNode(nodeFilter);

            if (hnc == null) throw new Exception("未找到指定节点!");

            if (containSelfNode) return hnc.OuterHtml;

            return hnc.InnerHtml;
        }

        #endregion

    }
}
