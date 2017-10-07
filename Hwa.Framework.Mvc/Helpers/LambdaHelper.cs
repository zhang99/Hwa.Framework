using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Collections;
using Hwa.Framework.Mvc.Model;

namespace Hwa.Framework.Mvc
{
    /// <summary>
    /// 查询方式
    /// </summary>
    public enum QueryMethods
    {
        Contains = 0,
        StartsWith = 1,
        EndsWith = 2,
        Equals = 3,
        Between = 4,
    }

    /// <summary>
    /// LambdaHelper -- zhangh 2013/06/28
    /// </summary>
    public class LambdaHelper
    {
        /// <summary>
        /// BuildQueryCondition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="pagingModel"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildQueryCondition<T>(PagingModel pagingModel)
        {
            var expression = PredicateBuilder.True<T>();
            if (!string.IsNullOrWhiteSpace(pagingModel.Query) && !string.IsNullOrWhiteSpace(pagingModel.QueryFields))
                expression = LambdaHelper.BuildLambdasOr<T>(pagingModel.QueryFields, QueryMethods.Contains, pagingModel.Query);

            if (pagingModel.AdvancedQuery != null && pagingModel.AdvancedQuery.Count() > 0)
            {
                Expression<Func<T, bool>> tmpExpr = null;
                var expr = PredicateBuilder.True<T>();
                foreach (var item in pagingModel.AdvancedQuery)
                {
                    tmpExpr = LambdaHelper.BuildLambda<T, bool>(item.Field, item.Operator, item.Value);
                    if (tmpExpr != null)
                        expr = expr.And(tmpExpr);
                }
                expression = expression.And(expr);
            }
            return expression;
        }

        /// <summary>
        /// BuildLambdas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="property"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildLambdasOr<T>(string property, QueryMethods method, string content)
        {
            if (string.IsNullOrWhiteSpace(property)) return null;
            if (string.IsNullOrWhiteSpace(content))
                return PredicateBuilder.True<T>();

            string[] props = property.Split(',');
            var predicate = PredicateBuilder.False<T>();
            Expression<Func<T, bool>> expr = null;
            foreach (string prop in props)
            {
                expr = BuildLambda<T, bool>(prop, method, content);
                if (expr != null)
                    predicate = predicate.Or(expr);
            }

            return predicate;
        }

        /// <summary>
        /// BuildLambdasOr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="method"></param>
        /// <param name="specialProperty"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildLambdasOr<T>(string property, QueryMethods method, string specialProperty, string[] contents)
        {
            if (string.IsNullOrWhiteSpace(property) || contents == null || contents.Length == 0)
                return null;

            var predicate = PredicateBuilder.False<T>();
            var content = contents[0];
            string[] props = property.Split(',');
            Expression<Func<T, bool>> expr = null;
            bool first = true;
            foreach (var c in contents)
            {
                expr = null;
                foreach (string prop in props)
                {
                    if (first)
                    {
                        expr = BuildLambda<T, bool>(prop, method, content);
                        if (expr != null)
                            predicate = predicate.Or(expr);
                    }
                    else if (("," + specialProperty + ",").IndexOf("," + prop + ",") >= 0)
                    {
                        expr = BuildLambda<T, bool>(prop, method, c);
                        if (expr != null)
                            predicate = predicate.Or(expr);
                    }
                }
                first = false;
            }

            return predicate;
        }

        /// <summary>
        /// WrapContainsExpression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">值类型数组如long[]</param>
        /// <param name="fieldName"></param>
        /// <returns>options.Contains(f.fieldName)</returns>
        public static Expression<Func<T, bool>> BuildLambdaWrapContains<T>(IEnumerable options, string fieldName)
        {
            //变量
            PropertyInfo fieldInfo = null;
            Expression<Func<T, bool>> expr = null;
            Expression expression = Expression.Constant(true);
            //1.f=>
            ParameterExpression left = Expression.Parameter(typeof(T), "f");

            //2.f.Property/f.Property.Value
            MemberExpression innerexpr = null;
            foreach (var field in fieldName.Split('.'))
            {
                fieldInfo = fieldInfo == null ? typeof(T).GetProperty(field) : fieldInfo.PropertyType.GetProperty(field);
                innerexpr = innerexpr == null ? Expression.Property(left, fieldInfo) : Expression.Property(innerexpr, field);
            }
            if (fieldInfo.PropertyType.Name == "Nullable`1")
                innerexpr = Expression.Property(innerexpr, "Value");

            //3.fieldtype
            var fieldType = fieldInfo.PropertyType;
            if (fieldInfo.PropertyType.Name == "Nullable`1") fieldType = fieldType.GetGenericArguments()[0];
            if (!fieldType.IsValueType) throw new Exception("只能构建值类型数组Contains表达式!");

            //4.Method返回泛型的Contains方法(Enumerable.Contains)
            var method = typeof(Enumerable).GetMethods()
            .Where(f => f.Name == "Contains")
            .Where(f => f.GetParameters().Length == 2 && f.GetParameters()[1].ParameterType.Name == "TSource")
            .FirstOrDefault();
            if (method != null)
                method = method.MakeGenericMethod(fieldType);

            //5.Expression构建
            Expression right = Expression.Call
            (
                method,
                Expression.Constant(options),
                innerexpr
            );
            expression = Expression.And(right, expression);

            expr = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[] { left });

            return expr;
        }

        /// <summary>
        /// BuildLambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="property"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Expression<Func<T, TValue>> BuildLambda<T, TValue>(string property, QueryMethods method, object content)
        {
            if (string.IsNullOrWhiteSpace(property))
                return null;

            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "f");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                    continue;
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            bool nullFlag = false;
            QueryDataProcessing(type, ref method, ref content, ref nullFlag);
            if (nullFlag)
                return null;

            Expression searchExpr = null;
            MethodInfo callMethod = null;
            Expression expression = null;
            switch (method)
            {
                case QueryMethods.Equals:
                    searchExpr = Expression.Constant(content, type);
                    expression = Expression.Equal(expr, searchExpr);
                    break;
                case QueryMethods.StartsWith:
                case QueryMethods.EndsWith:
                case QueryMethods.Contains:
                    searchExpr = Expression.Constant(content, type);
                    callMethod = type.GetMethod(method.ToString(), new[] { type });                    
                    expression = Expression.Call(expr, callMethod, new[] { searchExpr });
                    break;
                case QueryMethods.Between:
                    var values = content as HashSet<object>;
                    var minExpr = Expression.Constant(values.First(), type);
                    var maxExpr = Expression.Constant(values.Last(), type);
                    var leftExpr = Expression.GreaterThanOrEqual(expr, minExpr);
                    var rightExpr = Expression.LessThanOrEqual(expr, maxExpr);
                    expression = Expression.And(leftExpr, rightExpr);
                    break;
                default:
                    callMethod = type.GetMethod(method.ToString(), new[] { type });
                    expression = Expression.Call(expr, callMethod, new[] { searchExpr });
                    break;
            }

            return Expression.Lambda<Func<T, TValue>>(expression, arg);
        }

        /// <summary>
        /// BuildLambda
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Expression BuildLambda(Type type, string property, QueryMethods method, object content)
        {
            if (string.IsNullOrWhiteSpace(property))
                return null;

            string[] props = property.Split('.');
            ParameterExpression arg = Expression.Parameter(type, "f");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                    continue;
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            bool nullFlag = false;
            QueryDataProcessing(type, ref method, ref content, ref nullFlag);
            if (nullFlag)
                return null;

            Expression searchExpr = null;
            MethodInfo callMethod = null;
            Expression expression = null;
            switch (method)
            {
                case QueryMethods.Equals:
                    searchExpr = Expression.Constant(content, type);
                    expression = Expression.Equal(expr, searchExpr);
                    break;
                case QueryMethods.StartsWith:
                case QueryMethods.EndsWith:
                case QueryMethods.Contains:
                    searchExpr = Expression.Constant(content, type);
                    callMethod = type.GetMethod(method.ToString(), new[] { type });
                    expression = Expression.Call(expr, callMethod, new[] { searchExpr });
                    break;
                case QueryMethods.Between:
                    var values = content as HashSet<object>;
                    var minExpr = Expression.Constant(values.First(), type);
                    var maxExpr = Expression.Constant(values.Last(), type);
                    var leftExpr = Expression.GreaterThanOrEqual(expr, minExpr);
                    var rightExpr = Expression.LessThanOrEqual(expr, maxExpr);
                    expression = Expression.And(leftExpr, rightExpr);
                    break;
                default:
                    callMethod = type.GetMethod(method.ToString(), new[] { type });
                    expression = Expression.Call(expr, callMethod, new[] { searchExpr });
                    break;
            }

            return Expression.Lambda(expression, arg);
        }

        /// <summary>
        /// QueryDataProcessing
        /// </summary>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool QueryDataProcessing(Type type, ref QueryMethods method, ref object content, ref bool nullFlag)
        {
            HashSet<object> hs = new HashSet<object>();         
            switch (type.Name)
            {
                case "String":
                    break;
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                case "Single":
                case "Double":
                    if (method == QueryMethods.Contains || method == QueryMethods.StartsWith || method == QueryMethods.EndsWith)
                        nullFlag = true;
                    else if (content != null && content.ToString().Trim() != "")
                    {
                        if (method == QueryMethods.Between)
                        {
                            CustomRangeQueryProcessing(type, content, ref hs);
                            content = hs;
                        }
                        else
                        {
                            content = Convert.ChangeType(content, type);
                        }
                    }
                    break;
                case "DateTime":
                    if (method == QueryMethods.Contains || method == QueryMethods.StartsWith || method == QueryMethods.EndsWith)
                    {
                        nullFlag = true;
                        break;
                    }
                    method = QueryMethods.Between;
                    DateTime today;
                    DateTime start;
                    DateTime timeZoneToday = System.DateTime.Now;//HwaUserContext.NowZoneTime.Date;
                    switch (content.ToString())
                    {
                        case "today":
                            hs.Add(timeZoneToday);
                            hs.Add(timeZoneToday.AddDays(1).AddMilliseconds(-1));
                            break;
                        case "yesterday":
                            hs.Add(timeZoneToday.AddDays(-1));
                            hs.Add(timeZoneToday.AddMilliseconds(-1));
                            break;
                        case "thisweek":
                            today = timeZoneToday;
                            start = today.AddDays(1 - Convert.ToInt32(today.DayOfWeek.ToString("d")));
                            hs.Add(start);
                            hs.Add(start.AddDays(7).AddMilliseconds(-1));
                            break;
                        case "lastweek":
                            today = timeZoneToday;
                            start = today.AddDays(Convert.ToInt32(1 - Convert.ToInt32(timeZoneToday.DayOfWeek)) - 7);
                            hs.Add(start);
                            hs.Add(start.AddDays(7).AddMilliseconds(-1));
                            break;
                        case "thismonth":
                            start = DateTime.Parse(timeZoneToday.ToString("yyyy-MM-01"));
                            hs.Add(start);
                            hs.Add(start.AddMonths(1).AddMilliseconds(-1));
                            break;
                        case "lastmonth":
                            start = DateTime.Parse(timeZoneToday.ToString("yyyy-MM-01")).AddMonths(-1);
                            hs.Add(start);
                            hs.Add(start.AddMonths(1).AddMilliseconds(-1));
                            break;
                        case "thisseason":
                            start = timeZoneToday.AddMonths(0 - ((timeZoneToday.Month - 1) % 3)).AddDays(1 - timeZoneToday.Day);
                            hs.Add(start);
                            hs.Add(DateTime.Parse(timeZoneToday.AddMonths(3 - ((timeZoneToday.Month - 1) % 3)).ToString("yyyy-MM-01")).AddMilliseconds(-1));
                            break;
                        case "lastseason":
                            start = timeZoneToday.AddMonths(-3 - ((timeZoneToday.Month - 1) % 3)).AddDays(1 - timeZoneToday.Day);
                            hs.Add(start);
                            hs.Add(timeZoneToday.AddMonths(0 - ((timeZoneToday.Month - 1) % 3)).AddDays(1 - timeZoneToday.Day).AddMilliseconds(-1));
                            break;
                        case "thisyear":
                            start = DateTime.Parse(timeZoneToday.ToString("yyyy-01-01"));
                            hs.Add(start);
                            hs.Add(start.AddYears(1).AddMilliseconds(-1));
                            break;
                        case "lastyear":
                            start = DateTime.Parse(timeZoneToday.ToString("yyyy-01-01"));
                            hs.Add(start.AddYears(-1));
                            hs.Add(start.AddMilliseconds(-1));
                            break;
                        default:
                            CustomRangeQueryProcessing(type, content, ref hs);
                            break;
                    }
                    content = hs;
                    break;
                case "Nullable`1":
                    QueryDataProcessing(type.GetGenericArguments()[0], ref method, ref content, ref nullFlag);
                    break;
                default:
                    nullFlag = true;
                    break;
            }
            return nullFlag;
        }

        /// <summary>
        /// CustomRangeQueryProcessing
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="hs"></param>
        private static void CustomRangeQueryProcessing(Type type, object content, ref HashSet<object> hs)
        {
            var arr = content.ToString().Split(new char[] { ',' });
            if (arr.Length == 2)
            {
                hs.Add(Convert.ChangeType(arr[0], type));
                if (type.Name == "DateTime")
                    hs.Add(DateTime.Parse(arr[1]).AddDays(1).AddMilliseconds(-1));
                else
                    hs.Add(Convert.ChangeType(arr[1], type));
            }
        }

        /// <summary>
        /// BuildPropertySpecifier
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="?"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Expression<Func<T, TValue>> BuildPropertySpecifier<T, TValue>(string property)
        {
            if (string.IsNullOrWhiteSpace(property))
                return null;

            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "f");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                    continue;
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var expression = Expression.Lambda(
                typeof(Func<T, TValue>),
                expr,
                arg
                );

            return (Expression<Func<T, TValue>>)expression;
        }

        /// <summary>
        /// BuildPropertySpecifier
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Expression BuildPropertySpecifier(Type type, string property)
        {
            if (string.IsNullOrWhiteSpace(property))
                return null;

            string[] props = property.Split('.');
            ParameterExpression arg = Expression.Parameter(type, "f");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                    continue;
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var expression = Expression.Lambda(
                expr,
                arg
                );

            return expression;
        }

        /// <summary>
        /// GetMemberExpression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression(LambdaExpression expression)
        {
            return RemoveUnary(expression.Body) as MemberExpression;
        }

        /// <summary>
        /// GetTypeFromMemberExpression
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public static Type GetTypeFromMemberExpression(MemberExpression memberExpression)
        {
            if (memberExpression == null) return null;

            var dataType = GetTypeFromMemberInfo(memberExpression.Member, (PropertyInfo p) => p.PropertyType);
            if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (MethodInfo m) => m.ReturnType);
            if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (FieldInfo f) => f.FieldType);

            return dataType;
        }

        /// <summary>
        /// GetTypeFromMemberInfo
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="member"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static Type GetTypeFromMemberInfo<TMember>(MemberInfo member, Func<TMember, Type> func) where TMember : MemberInfo
        {
            if (member is TMember)
            {
                return func((TMember)member);
            }
            return null;
        }

        /// <summary>
        /// RemoveUnary
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private static Expression RemoveUnary(Expression body)
        {
            var unary = body as UnaryExpression;
            if (unary != null)
            {
                return unary.Operand;
            }
            return body;
        }
    }

    /// <summary>
    /// PredicateBuilder
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression True(Type type) { return Expression.Lambda(Expression.Constant(true), Expression.Parameter(type, "f")); }
        public static Expression False(Type type) { return Expression.Lambda(Expression.Constant(false), Expression.Parameter(type, "f")); }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            if (second == null) return first;

            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            var not = Expression.Not(expr.Body);
            return Expression.Lambda<Func<T, bool>>(not, expr.Parameters);
        }
    }

    /// <summary>
    /// ParameterRebinder
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }   
}
