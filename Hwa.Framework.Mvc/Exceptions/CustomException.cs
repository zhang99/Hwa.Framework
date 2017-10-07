using Hwa.Framework.Mvc.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc
{
    /// <summary>
    /// 自定义错误，抛出需用户确认
    /// </summary>
    public class ConfirmException : Exception
    {
        public ConfirmException(string message) 
            : base(message)
        {
            //MsgType = MsgType.confirm;
        }

        //public MsgType MsgType { get; set; }
    }

    /// <summary>
    /// 自定义错误:返回正常的成功提示信息，如果有事务，事务仍然提交
    /// </summary>
    public class WarningException : Exception
    {
        public WarningException(string message,object data) 
            : base(message)
        {
            Data = data;
        }

        public new object Data { get; set; }
    }

    /// <summary>
    /// 自定义错误：系统中的自定义的验证、权限错误，非程序错误(不用记录到日志)
    /// </summary>
    public class NormalException : Exception
    {
        public NormalException(string message)
            : base(message) { }

        public NormalException(string message, object data)
            : base(message)
        {
            Data = data;
        }

        public new object Data { get; set; }
    }
}
