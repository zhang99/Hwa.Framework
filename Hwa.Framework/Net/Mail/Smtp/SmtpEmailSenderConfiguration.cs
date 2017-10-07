
namespace Hwa.Framework.Net.Mail.Smtp
{
    /// <summary>
    /// Implementation of <see cref="ISmtpEmailSenderConfiguration"/> that reads settings
    /// from <see cref="ISettingManager"/>.
    /// </summary>
    public class SmtpEmailSenderConfiguration : EmailSenderConfiguration, ISmtpEmailSenderConfiguration
    {    
        /// <summary>
        /// SMTP Host name/IP.
        /// </summary>
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// SMTP Port.
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// User name to login to SMTP server.
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Password to login to SMTP server.
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Domain name to login to SMTP server.
        /// </summary>
        public string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Is SSL enabled?
        /// </summary>
        public bool EnableSsl
        {
            get;
            set;
        }

        /// <summary>
        /// Use default credentials?
        /// </summary>
        public bool UseDefaultCredentials
        {
            get;
            set;
        }
       
    }
}