using Hwa.Framework.Mvc.Data.Pagination;
using Hwa.Framework.Mvc.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        T GetById(object Id);

        T GetById(long Id);

        T Get(Expression<Func<T, bool>> where);

        T Get(Expression<Func<T, bool>> where, bool includes);

        T Get(Expression<Func<T, bool>> where, string includes);

        T Get(Expression<Func<T, bool>> where, bool autoInclude, bool autoFilter);

        /// <summary>
        /// 获取实体的部分属性
        /// </summary>
        /// <typeparam name="TProperties"></typeparam>
        /// <param name="where"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        TProperties Get<TProperties>(Expression<Func<T, bool>> where, Expression<Func<T, TProperties>> selector);

        /// <summary>
        /// 获取实体的分组查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElements"></typeparam>
        /// <param name="where"></param>
        /// <param name="groupSelector"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        TElements GetTotalProperties<TKey, TElements>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> groupBy, Expression<Func<IGrouping<TKey, T>, TElements>> selector);

        bool Exists(Expression<Func<T, bool>> where);

        T Add(T entity);

        T Update(T entity, bool needCheck = true);

        void Delete(long id);

        void Delete(long[] ids);

        void Delete(Expression<Func<T, bool>> where);

        void Delete(T entity);

        ICollection<T> GetList();

        ICollection<T> GetList(Expression<Func<T, bool>> where);

        ICollection<T> GetList(Expression<Func<T, bool>> where, bool autoInclude);        

        ICollection<T> GetList<TOrder>(Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order);

        IPagination<T> GetPagedList(PagingModel pagingModel, Expression<Func<T, bool>> where);

        ObjectStateEntry GetObjectStateEntry(T entity);
        
        DbRawSqlQuery SqlQuery(Type elementType, string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, params object[] parameters);

        void Commit();

    }
}
