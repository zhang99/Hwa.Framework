using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Hwa
{
	/// <summary>
	/// Extension methods for sorting.
	/// </summary>
	public static class SortExtensions
	{
		/// <summary>
		/// Orders a datasource by a property with the specified name in the specified direction
		/// </summary>
		/// <param name="datasource">The datasource to order</param>
		/// <param name="propertyName">The name of the property to order by</param>
		/// <param name="direction">The direction</param>
		public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> datasource, string propertyName, SortDirection direction)
		{
			return datasource.AsQueryable().OrderBy(propertyName, direction);
		}

		/// <summary>
		/// Orders a datasource by a property with the specified name in the specified direction
		/// </summary>
		/// <param name="datasource">The datasource to order</param>
		/// <param name="propertyName">The name of the property to order by</param>
		/// <param name="direction">The direction</param>
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> datasource, string propertyName, SortDirection direction)
		{
            //http://msdn.microsoft.com/en-us/library/bb882637.aspx

            #region 关联表字段排序报错 Fix by zhangh 2013/08/13
            //if (string.IsNullOrEmpty(propertyName))
            //    return datasource;

            //Type type = typeof(T);
            //PropertyInfo property = type.GetProperty(propertyName);

            //if (property == null)
            //{
            //    throw new InvalidOperationException(string.Format("Could not find a property called '{0}' on type {1}", propertyName, type));
            //}

            //ParameterExpression parameter = Expression.Parameter(type, "p");
            //Expression propertyAccess = Expression.Property(parameter, property);
            //var orderByExp = Expression.Lambda(propertyAccess, parameter);
            #endregion

            if (string.IsNullOrEmpty(propertyName))
                return datasource;

            string[] props = propertyName.Split('.');
            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(type, "p");
            Expression propertyAccess = parameter;
            PropertyInfo property = null;
            foreach (string prop in props)
            {
                property = type.GetProperty(prop);
                if (property == null)
                    continue;
                propertyAccess = Expression.Property(propertyAccess, property);
                type = property.PropertyType;
            }
            type = typeof(T);
            var orderByExp = Expression.Lambda(
                propertyAccess,
                parameter
                );

			const string orderBy = "OrderBy";
			const string orderByDesc = "OrderByDescending";

			string methodToInvoke = direction == SortDirection.Ascending ? orderBy : orderByDesc;

			var orderByCall = Expression.Call(typeof(Queryable), 
				methodToInvoke, 
				new[] { type, property.PropertyType }, 
				datasource.Expression,
                Expression.Quote(orderByExp));

			return datasource.Provider.CreateQuery<T>(orderByCall);
		}
	}
}