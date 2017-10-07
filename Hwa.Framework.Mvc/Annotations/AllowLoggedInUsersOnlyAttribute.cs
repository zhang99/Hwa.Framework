using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc.Annotations
{
    /// <summary>
    /// 允许登录用户访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class AllowLoggedInUsersOnlyAttribute : Attribute
    {
        public AllowLoggedInUsersOnlyAttribute() { }
    }
}
