using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Hwa.Framework.Mvc.Logging
{
    public static class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("RollingFile");       
         
       
        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void Debug(object message, Exception ex)
        {
            log.Debug(message, ex);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception ex)
        {
            log.Error(message, ex);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        public static void Fatal(object message, Exception ex)
        {
            log.Fatal(message, ex);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }

        public static void Info(object message)
        {
            log.Info(message);
        }

        public static void Info(object message, Exception ex)
        {
            log.Info(message, ex);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public static void Warn(object message)
        {
            log.Warn(message);

        }

        public static void Warn(object message, Exception ex)
        {
            log.Warn(message, ex);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

    }
}
