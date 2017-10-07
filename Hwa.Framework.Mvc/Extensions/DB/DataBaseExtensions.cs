using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Hwa.Framework.Mvc;
using Hwa.Framework.Mvc.Caching;
using Hwa.Framework.Mvc.Annotations.GridAnnotations;
using Hwa.Framework.Data;
using Hwa.Framework.Mvc.Model;
using Hwa.Framework.Mvc.Data.Pagination;


namespace Hwa.Framework.Mvc
{  
    /// <summary>
    /// DB扩展类 -- zhangh 2013/12/26
    /// </summary>
    public static class DataBaseExtensions
    {        
        /// <summary>
        /// 简单模糊查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlKeyword"></param>
        /// <param name="prefixName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string BuildQueryCondition<T>(string sqlKeyword, string innerSql, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return innerSql;

            StringBuilder sb = new StringBuilder();

            var propers = typeof(T).GetProperties()
                                   .Where(x => (x.GetCustomAttributes(typeof(QueryableAttribute), false).Count() > 0) &&
                                               (x.GetCustomAttributes(typeof(QueryableAttribute), false)[0] as QueryableAttribute).Queryable == true);

            if (propers.Count() > 0)
            {
                foreach (var proper in propers)
                {
                    sb.Append("(")
                      .Append(" SELECT * FROM (")
                      .Append(innerSql)
                      .Append(") t WHERE ");

                    sb.Append("t.").Append(proper.Name).Append(" LIKE ").Append("@QueryContent");

                    sb.Append(") ");

                    if (proper != propers.Last())
                        sb.Append(" UNION ");
                }
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, string.Format(" {0} ", sqlKeyword));
                return sb.ToString();
            }

            return innerSql;
        }

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlKeyword"></param>
        /// <param name="prefixName"></param>
        /// <param name="pagingModel"></param>
        /// <returns></returns>
        public static string BuildAdvancedQueryCondition<T>(string sqlKeyword, string prefixName, PagingModel pagingModel, ref DynamicParameters parms)
        {
            StringBuilder sb = new StringBuilder();

            if (pagingModel.AdvancedQuery != null && pagingModel.AdvancedQuery.Count() > 0)
            {
                string newField;
                var type = typeof(T);

                foreach (var item in pagingModel.AdvancedQuery)
                {
                    newField = string.Empty;
                    item.Field = item.Field.Replace(".", "");

                    if (prefixName.HasValue())
                        newField = string.Format("{0}.{1}", prefixName, item.Field);

                    sb.Append(BuildCondition(newField, type, item));
                    parms = BuildQueryParams(item, type, parms);

                    if (item != pagingModel.AdvancedQuery.Last())
                        sb.Append(" And ");
                }
            }

            if (sb.Length > 0)
                sb.Insert(0, "(").Append(")").Insert(0, string.Format(" {0} ", sqlKeyword));

            return sb.ToString();
        }

        /// <summary>
        /// 高级查询条件单项 TODO: 参数DataType待优化，现在都是默认nvarchar(4000)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newField"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string BuildCondition(string newField, Type type, AdvancedQueryItem item)
        {
            string field = item.Field.Replace(".", "");

            StringBuilder sb = new StringBuilder();
            switch (item.Operator)
            {
                case QueryMethods.Equals:
                    sb.Append(newField).Append("=").Append("@").Append(field);
                    break;
                case QueryMethods.Contains:
                    sb.Append(newField).Append(" LIKE ").Append("@").Append(field);
                    break;
                case QueryMethods.StartsWith:
                    sb.Append(newField).Append(" LIKE ").Append("@").Append(field);
                    break;
                case QueryMethods.EndsWith:
                    sb.Append(newField).Append(" LIKE ").Append("@").Append(field);
                    break;
                case QueryMethods.Between:
                    var arr = item.Value.ToString().Split(new char[] { ',' });
                    var hs = new HashSet<object>();
                    if (arr.Length == 2)
                    {
                        var fieldType = type.GetProperty(field) == null ? type : type.GetProperty(field).PropertyType;
                        if (fieldType.IsGenericType)
                            fieldType = fieldType.GetGenericArguments()[0];

                        hs.Add(Convert.ChangeType(arr[0], fieldType));

                        if (fieldType.Name == "DateTime")
                            hs.Add(DateTime.Parse(arr[1]).AddDays(1).AddMilliseconds(-1));
                        else
                            hs.Add(Convert.ChangeType(arr[1], fieldType));
                    }
                    sb.Append(newField).Append(" BETWEEN ").Append("@").Append(field).Append("Begin").Append(" And ").Append("@").Append(field).Append("End");
                    break;
                default:
                    sb.Append(" 1=1 ");
                    break;
            }

            return sb.ToString();
        }


        /// <summary>
        /// 高级查询条件参数单项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static DynamicParameters BuildQueryParams(AdvancedQueryItem item, Type type, DynamicParameters parms)
        {
            string field = item.Field.Replace(".", "");

            switch (item.Operator)
            {
                case QueryMethods.Equals:
                    parms.Add(field, item.Value);
                    break;
                case QueryMethods.Contains:
                    parms.Add(field, string.Format("{0}%", item.Value));
                    break;
                case QueryMethods.StartsWith:
                    parms.Add(field, string.Format("{0}%", item.Value));
                    break;
                case QueryMethods.EndsWith:
                    parms.Add(field, string.Format("%{0}", item.Value));
                    break;
                case QueryMethods.Between:
                    var arr = item.Value.ToString().Split(new char[] { ',' });
                    var hs = new HashSet<object>();
                    if (arr.Length == 2)
                    {
                        var fieldType = type.GetProperty(field) == null ? type : type.GetProperty(field).PropertyType;
                        if (fieldType.IsGenericType)
                            fieldType = fieldType.GetGenericArguments()[0];

                        hs.Add(Convert.ChangeType(arr[0], fieldType));

                        if (fieldType.Name == "DateTime")
                            hs.Add(DateTime.Parse(arr[1]).AddDays(1).AddMilliseconds(-1));
                        else
                            hs.Add(Convert.ChangeType(arr[1], fieldType));
                    }
                    parms.Add(string.Format("{0}Begin", field), hs.First().ToString());
                    parms.Add(string.Format("{0}End", field), hs.Last().ToString());
                    break;
                default:
                    break;
            }

            return parms;
        }


        /// <summary>
        /// 获取高级查询项
        /// </summary>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static AdvancedQueryItem Get(this IEnumerable<AdvancedQueryItem> items, string key)
        {
            return items.Where(p => p.Field == key).FirstOrDefault();
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="items"></param>
        /// <param name="key"></param>
        public static void Remove(this IEnumerable<AdvancedQueryItem> items, string key)
        {
            var lst = items as List<AdvancedQueryItem>;
            var item = items.Where(p => p.Field == key).FirstOrDefault();
            if (lst != null && item != null)
                lst.Remove(item);
            else if (lst == null && items is HashSet<AdvancedQueryItem>)
            {
                var list = items as HashSet<AdvancedQueryItem>;
                list.Remove(item);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void Clear(this IEnumerable<AdvancedQueryItem> items)
        {
            items = items as List<AdvancedQueryItem>;
            if (items == null)
                items = new List<AdvancedQueryItem>();
            else
                items.Clear();
        }

        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> items, string separator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.AppendFormat("{0}{1}", separator, string.Join(separator, item.GetType().GetProperties().Select(p => p.GetValue(item, null).ToString())));
            }
            return sb.ToString();
        }

        public static string ToString(this DynamicParameters parms, string separator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var name in parms.ParameterNames)
            {
                var value = parms.Get<object>(name);
                sb.AppendFormat("{0}{1}", separator, value);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="innerSql"></param>
        /// <param name="pagingModel"></param>
        /// <returns></returns>
        public static IPagination<T> Query<T>(this Database db, string innerSql, PagingModel pagingModel, DynamicParameters parms = null)
        {
            string cacheKey = string.Format("{0}-{1}", pagingModel.Query, pagingModel.AdvancedQuery.ToString("-"));

            if (pagingModel.PageIndex < 1)
                pagingModel.PageIndex = 1;

            innerSql = BuildQueryCondition<T>("", innerSql, pagingModel.Query);

            //查询条件
            parms = parms ?? new DynamicParameters();

            StringBuilder condition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(pagingModel.Query))
                parms.Add("QueryContent", string.Format("{0}%", pagingModel.Query));

            condition.Append(BuildAdvancedQueryCondition<T>(" ", "t", pagingModel, ref parms));

            StringBuilder sql = new StringBuilder();
            //分页数据SQL
            sql.AppendFormat("SELECT TOP {0} tt.* FROM (", pagingModel.PageSize)
                .AppendFormat(" SELECT row_number() OVER(Order By t.{0} {1}) AS rowNum,t.* FROM (", pagingModel.SortOptions.Column, pagingModel.SortOptions.Direction == SortDirection.Descending ? "DESC" : "ASC")
                .Append(innerSql)
                .Append(") t ");
            if (condition.Length > 0)
                sql.Append(" WHERE ").Append(condition.ToString());

            sql.AppendFormat(") tt WHERE (tt.rowNum BETWEEN {0} AND {1}) ",
                pagingModel.PageSize == int.MaxValue ? 1 : (pagingModel.PageIndex - 1) * pagingModel.PageSize + 1, pagingModel.PageSize == int.MaxValue ? pagingModel.PageSize : ((pagingModel.PageIndex - 1) * pagingModel.PageSize) + pagingModel.PageSize);

            //总条数SQL
            StringBuilder sqlCount = new StringBuilder();
            sqlCount.AppendLine("SELECT Count(1) FROM (")
            .Append(innerSql)
            .Append(") t ");
            if (condition.Length > 0)
                sqlCount.Append(" WHERE ").Append(condition.ToString());

            string cacheCountKey = string.Format("{0}-{1}", sqlCount.ToString(), cacheKey);
            //if (!CacheHelper.HasKey(cacheCountKey))
                sql.AppendLine().Append(sqlCount);

            //合计列SQL
            StringBuilder sqlSum = new StringBuilder();
            var sumFields = typeof(T).GetProperties().Where(x => x.GetCustomAttributes(typeof(IsSumAttribute), false).Count() > 0);
            if (sumFields.Count() > 0)
            {
                sqlSum.Append(" SELECT ");
                foreach (var proper in sumFields)
                {
                    sqlSum.Append("ISNULL(Sum(")
                        .Append(proper.Name)
                        .Append("),0) as ")
                        .Append(proper.Name);

                    if (proper != sumFields.Last())
                        sqlSum.Append(",");
                }

                sqlSum.Append(" FROM (")
                    .Append(innerSql)
                    .Append(") t ");

                if (condition.Length > 0)
                    sqlSum.Append(" WHERE ").Append(condition.ToString());

            }
            string cacheSumKey = string.Format("{0}-{1}", sqlSum.ToString(), cacheKey);
            if (sqlSum.Length > 0)
                sql.AppendLine().Append(sqlSum);

            //执行SQL
            var result = db.Connection.QueryMultiple(sql.ToString(), parms);

            //分页数据
            var list = result.Read<T>();

            //总条数
            //if (CacheHelper.HasKey(cacheCountKey))
            //{
            //    pagingModel.TotalCount = CacheHelper.Get<Int32>(cacheCountKey);
            //}
            //else
            //{
                pagingModel.TotalCount = result.Read<int>().Single();
                //CacheHelper.Add(cacheCountKey, pagingModel.TotalCount, 1);
            //}

            //合计列
            if (sumFields.Count() > 0)
            {
                cacheSumKey = String.Format("{0}-{1}", cacheSumKey, parms.ToString("-"));
                //if (CacheHelper.HasKey(cacheSumKey))
                //{
                //    pagingModel.DicSum = CacheHelper.Get<IDictionary<string, decimal>>(cacheSumKey);
                //}
                //else
                //{
                    var dapperRows = result.Read<Hwa.Framework.Data.SqlMapper.DapperRow>().Single();
                    foreach (var item in dapperRows)
                    {
                        if (!pagingModel.DicSum.Keys.Contains(item.Key))
                            pagingModel.DicSum.Add(item.Key, Decimal.Parse(item.Value.ToString()));
                    }
                    //CacheHelper.Add(cacheSumKey, pagingModel.DicSum, 1);
                //}
            }

            return list.AsCustomPagination<T>(pagingModel);
        }

    }
}
