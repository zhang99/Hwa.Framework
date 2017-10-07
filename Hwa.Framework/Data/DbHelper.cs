using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Data
{
    public static class DbHelper
    {
        /// <summary>
        /// 读未提交
        /// </summary>
        /// <param name="action"></param>
        public static void WithNoLock(Action action)
        {
            var transactionOptions = new System.Transactions.TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted; ;
            using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    action();
                    transactionScope.Complete();
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 读提交
        /// </summary>
        /// <param name="action"></param>
        public static void WithLock(Action action)
        {
            var transactionOptions = new System.Transactions.TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted; ;
            using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    action();
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
