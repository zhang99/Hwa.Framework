using Hwa.Framework.Mvc.Data;
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

namespace Hwa.Framework.Mvc.Services
{
    public abstract class ServiceBase<T> : IService<T> where T : EntityBase
    {
        public ServiceBase()
        {
        }

        public ServiceBase(IRepository<T> repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// 服务接口
        /// </summary>
        public IRepository<T> _repository
        {
            get;
            protected set;
        }

        public virtual T GetById(object Id)
        {
            return _repository.GetById(Id);
        }

        public virtual T GetById(long Id)
        {
            return _repository.GetById(Id);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _repository.Get(where);
        }

        public virtual T Get(Expression<Func<T, bool>> where, bool includes)
        {
            return _repository.Get(where, includes);
        }

        public virtual T Get(Expression<Func<T, bool>> where, string includes)
        {
            return _repository.Get(where, includes);
        }

        /// <summary>
        /// 获取实体的部分属性
        /// </summary>
        /// <typeparam name="TProperties"></typeparam>
        /// <param name="where"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public TProperties Get<TProperties>(Expression<Func<T, bool>> where, Expression<Func<T, TProperties>> selector)
        {
            return _repository.Get<TProperties>(where, selector);
        }

        /// <summary>
        /// 获取实体的分组查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElements"></typeparam>
        /// <param name="where"></param>
        /// <param name="groupSelector"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public TElements GetTotalProperties<TKey, TElements>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> groupBy, Expression<Func<IGrouping<TKey, T>, TElements>> selector)
        {
            return _repository.GetTotalProperties<TKey, TElements>(where, groupBy, selector);
        }

        public virtual bool Exists(Expression<Func<T, bool>> where)
        {
            return _repository.Exists(where);
        }

        public virtual T Add(T entity)
        {
            BeforeAddOrUpdate(entity);
            entity = _repository.Add(entity);
            AfterAddOrUpdate(entity);
            return entity;
        }

        public virtual T Update(T entity, bool needCheck = true)
        {
            BeforeAddOrUpdate(entity);
            entity = _repository.Update(entity, needCheck);
            AfterAddOrUpdate(entity);

            return entity;
        }

        public virtual void Delete(long id)
        {
            T entity = Get(f => f.Id == id, false);
            Delete(entity);
        }

        public virtual void Delete(long[] ids)
        {
            BeforeDelete(ids);
            _repository.Delete(ids);
            AfterDelete(ids);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            _repository.Delete(where);
        }

        public virtual void Delete(T entity)
        {
            BeforeDelete(entity);
            _repository.Delete(entity);
            AfterDelete(entity);
        }

        public virtual ICollection<T> GetList()
        {
            return _repository.GetList();
        }

        public virtual ICollection<T> GetList(Expression<Func<T, bool>> where)
        {
            return _repository.GetList(where);
        }

        public virtual ICollection<T> GetList<TOrder>(Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order)
        {
            return _repository.GetList(where, order);
        }

        public virtual IPagination<T> GetPagedList(PagingModel pagingModel, Expression<Func<T, bool>> where)
        {
            return _repository.GetPagedList(pagingModel, where);
        }

        public ObjectStateEntry GetObjectStateEntry(T entity)
        {
            return _repository.GetObjectStateEntry(entity);
        }

        public virtual DbRawSqlQuery SqlQuery(Type elementType, string sql, params object[] parameters)
        {
            return _repository.SqlQuery(elementType, sql, parameters);
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _repository.ExecuteSqlCommand(sql, parameters);
        }

        public virtual void Commit()
        {
            _repository.Commit();
        }

        #region 业务逻辑抽象
        /// <summary>
        /// 添加、修改前业务处理
        /// </summary>
        /// <param name="entity"></param>
        public virtual void BeforeAddOrUpdate(T entity)
        {
        }

        /// <summary>
        /// 添加、修改后业务处理
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AfterAddOrUpdate(T entity)
        {
        }

        /// <summary>
        /// 删除前业务处理  TODO:抽象方法？后面考虑
        /// </summary>
        /// <param name="entity"></param>
        public virtual void BeforeDelete(T entity) { }

        public virtual void BeforeDelete(long[] ids) { }

        /// <summary>
        /// AfterDelete
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AfterDelete(T entity) { }

        public virtual void AfterDelete(long[] ids) { }
        #endregion
    }
}
