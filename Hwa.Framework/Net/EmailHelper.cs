using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using Hwa.Framework.Net.Mail.Smtp;
using System.Collections.Generic;

namespace Hwa.Framework.Net{

    public static class EmailHelper
    {
        public static SmtpEmailSenderConfiguration BuildEmailConfiguration()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["SiCloud:EmailHost"] == null)
                throw new Exception("未找到SiCloud:EmailHost配置项!");

            string[] emailHostSettings = System.Configuration.ConfigurationManager.AppSettings["SiCloud:EmailHost"].Split(new char[] { ';' });

            IDictionary<string, string> kv = new Dictionary<string, string>();
            string[] kvTmp;
            foreach (var item in emailHostSettings)
            {
                kvTmp = item.Split(new char[] { '=' });
                kv.Add(kvTmp[0].Trim(), kvTmp[1].Trim());
            }

            var emailConfiguration = new SmtpEmailSenderConfiguration()
            {
                DefaultFromAddress = kv["userName"],
                DefaultFromDisplayName = kv["displayName"],                
                Host = kv["host"],
                Port = 25,
                UserName = kv["userName"],
                Password = Hwa.Framework.Cryptography.RsaCryptoHelper.DecryptConfigString(kv["pwd"]),
                UseDefaultCredentials = false
            };
            return emailConfiguration;
        }

        public static bool Send(string to, string subject, string body)
        {
            try
            {
                new SmtpEmailSender(BuildEmailConfiguration()).Send(to, subject, body);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
    }
}
