using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using System.Web.Mvc;
using Hwa.Framework.Mvc.Annotations.GridAnnotations;


namespace Hwa.Framework.Mvc.UI.Grid
{
	/// <summary>
	/// Builds grid columns
	/// </summary>
	public class ColumnBuilder<T> : IList<GridColumn<T>> where T : class 
	{
		private readonly ModelMetadataProvider _metadataProvider;
		private readonly List<GridColumn<T>> _columns = new List<GridColumn<T>>();

		public ColumnBuilder() : this(ModelMetadataProviders.Current)
		{
			
		}

		private ColumnBuilder(ModelMetadataProvider metadataProvider)
		{
			_metadataProvider = metadataProvider;
		}


		/// <summary>
		/// Creates a column for custom markup.
		/// </summary>
		public IGridColumn<T> Custom(Func<T, object> customRenderer)
		{
			var column = new GridColumn<T>(customRenderer, "", typeof(object));
			column.Encode(false);
			Add(column);
			return column;
		}

        /// <summary>
        /// Creates a column for custom markup.
        /// add by zhangh 2013/06/03
        /// </summary>
        public IGridColumn<T> Custom(Func<T, object> customRenderer,string fieldName)
        {
            var column = new GridColumn<T>(customRenderer, fieldName, typeof(object));
            column.Encode(false);
            Add(column);
            return column;
        }

		/// <summary>
		/// Specifies a column should be constructed for the specified property.
		/// </summary>
		/// <param name="propertySpecifier">Lambda that specifies the property for which a column should be constructed</param>
		public IGridColumn<T> For(Expression<Func<T, object>> propertySpecifier)
        {
            #region removed by zhangh 2013/08/23
            //var memberExpression = GetMemberExpression(propertySpecifier);
            //var propertyType = GetTypeFromMemberExpression(memberExpression);
            //var declaringType = memberExpression == null ? null : memberExpression.Expression.Type;
            //var inferredName = memberExpression == null ? null : memberExpression.Member.Name;
            //var column = new GridColumn<T>(propertySpecifier.Compile(), inferredName, propertyType);
            //column.FieldName = column.Name;

            //if(declaringType != null)
            //{
            //    var metadata = _metadataProvider.GetMetadataForProperty(null, declaringType, inferredName);
				
            //    if (!string.IsNullOrEmpty(metadata.DisplayName))
            //    {
            //        column.Named(metadata.DisplayName);
            //    }

            //    if (!string.IsNullOrEmpty(metadata.DisplayFormatString)) 
            //    {
            //        column.Format(metadata.DisplayFormatString);
            //    }
            //}

            //Add(column);

            //return column;
            #endregion

            return For(propertySpecifier, null);
		}

        /// <summary>
        ///  Specifies a column should be constructed for the specified property.
        ///  add by zhangh 2013/06/03
        /// </summary>
        /// <param name="propertySpecifier"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IGridColumn<T> For(Expression<Func<T, object>> propertySpecifier,string fieldName)
        {
            var memberExpression = GetMemberExpression(propertySpecifier);
            var propertyType = GetTypeFromMemberExpression(memberExpression);
            var declaringType = memberExpression == null ? string.IsNullOrEmpty(fieldName) ? null : typeof(T) : memberExpression.Expression.Type;
            var inferredName = memberExpression == null ? fieldName : memberExpression.Member.Name;
            var column = new GridColumn<T>(propertySpecifier.Compile(), inferredName, propertyType);
            if (!string.IsNullOrEmpty(fieldName))
                column.FieldName = fieldName;
            else
            {
                string tmpName = ExpressionHelper.GetExpressionText(propertySpecifier);
                if (string.IsNullOrEmpty(tmpName) && memberExpression != null)
                    tmpName = memberExpression.ToString().Substring(memberExpression.ToString().IndexOf(".") + 1);

                column.FieldName = tmpName;
            }

            if (!string.IsNullOrEmpty(column.FieldName))
            {
                string[] props = column.FieldName.Split('.');
                Type type = typeof(T);
                PropertyInfo property = null;
                string displayName = string.Empty;
                ModelMetadata metadata = null;
                int width = 0;
                Attribute attr;
                foreach (string prop in props)
                {
                    property = type.GetProperty(prop);
                    if (property == null)
                        continue;

                    metadata = _metadataProvider.GetMetadataForProperty(null, type, property.Name);                    

                    if (!string.IsNullOrEmpty(metadata.DisplayName))
                    {
                        displayName += metadata.DisplayName;
                        column.Named(displayName);
                    }

                    //宽度
                    attr = Attribute.GetCustomAttribute(property, typeof(WidthAttribute), false);
                    if (attr != null)
                        width = ((WidthAttribute)attr).Width;

                    type = property.PropertyType;                    
                }

                #region TODO: 格式化等其他信息 zhangh 2013/06/03
                if (type != null)
                {
                    if (width > 0)
                        column.Width(width);

                    FormatColumn(column, type, metadata);//格式化已加 zhangh 2013/11/19
                }
                #endregion
            }

            Add(column);

            return column;
        }

        /// <summary>
        /// 格式化列 zhangh 2013/11/19
        /// </summary>
        /// <param name="column"></param>
        /// <param name="propertyType"></param>
        /// <param name="metadata"></param>
        protected void FormatColumn(GridColumn<T> column, Type propertyType, ModelMetadata metadata)
        {
            switch (propertyType.Name)
            {
                case "Decimal":
                case "Single":
                case "Double":
                    if (!string.IsNullOrEmpty(metadata.DisplayFormatString))
                        column.Format(metadata.DisplayFormatString);
                    else
                        column.Format("{0:N2}");
                    column.Align(Alignment.Right);      
                    break;
                case "Int64":
                case "Int32":
                case "Int16":
                    if (!string.IsNullOrEmpty(metadata.DisplayFormatString))
                        column.Format(metadata.DisplayFormatString);
                    break;
                case "DateTime":
                    if (!string.IsNullOrEmpty(metadata.DisplayFormatString))
                        column.Format(metadata.DisplayFormatString);
                    else
                        column.Format("{0:yyyy-MM-dd}");
                    break;
                case "Nullable`1":
                    FormatColumn(column, propertyType.GetGenericArguments()[0], metadata);
                    break;
            }
        }

		protected IList<GridColumn<T>> Columns
		{
			get { return _columns; }
		}

		public IEnumerator<GridColumn<T>> GetEnumerator()
		{
			return _columns
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		public static MemberExpression GetMemberExpression(LambdaExpression expression)
		{
			return RemoveUnary(expression.Body) as MemberExpression;
		}

		private static Type GetTypeFromMemberExpression(MemberExpression memberExpression) 
		{
			if (memberExpression == null) return null;

			var dataType = GetTypeFromMemberInfo(memberExpression.Member, (PropertyInfo p) => p.PropertyType);
			if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (MethodInfo m) => m.ReturnType);
			if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (FieldInfo f) => f.FieldType);

			return dataType;
		}

		private static Type GetTypeFromMemberInfo<TMember>(MemberInfo member, Func<TMember, Type> func) where TMember : MemberInfo 
		{
			if (member is TMember) 
			{
				return func((TMember)member);
			}
			return null;
		}

		private static Expression RemoveUnary(Expression body)
		{
			var unary = body as UnaryExpression;
			if(unary != null)
			{
				return unary.Operand;
			}
			return body;
		}

		protected virtual void Add(GridColumn<T> column)
		{
			_columns.Add(column);
		}

		void ICollection<GridColumn<T>>.Add(GridColumn<T> column)
		{
			Add(column);
		}

		void ICollection<GridColumn<T>>.Clear()
		{
			_columns.Clear();
		}

		bool ICollection<GridColumn<T>>.Contains(GridColumn<T> column)
		{
			return _columns.Contains(column);
		}

		void ICollection<GridColumn<T>>.CopyTo(GridColumn<T>[] array, int arrayIndex)
		{
			_columns.CopyTo(array, arrayIndex);
		}

		bool ICollection<GridColumn<T>>.Remove(GridColumn<T> column)
		{
			return _columns.Remove(column);
		}

		int ICollection<GridColumn<T>>.Count
		{
			get { return _columns.Count; }
		}

		bool ICollection<GridColumn<T>>.IsReadOnly
		{
			get { return false; }
		}

		int IList<GridColumn<T>>.IndexOf(GridColumn<T> item)
		{
			return _columns.IndexOf(item);
		}

		void IList<GridColumn<T>>.Insert(int index, GridColumn<T> item)
		{
			_columns.Insert(index, item);
		}

		void IList<GridColumn<T>>.RemoveAt(int index)
		{
			_columns.RemoveAt(index);
		}

		GridColumn<T> IList<GridColumn<T>>.this[int index]
		{
			get { return _columns[index]; }
			set { _columns[index] = value; }
		}
	}
}