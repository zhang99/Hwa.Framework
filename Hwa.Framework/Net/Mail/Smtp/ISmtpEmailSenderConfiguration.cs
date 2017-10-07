using System.Net.Mail;

namespace Hwa.Framework.Net.Mail.Smtp
{
    /// <summary>
    /// Defines configurations to used by <see cref="SmtpClient"/> object.
    /// </summary>
    public interface ISmtpEmailSenderConfiguration : IEmailSenderConfiguration
    {
        /// <summary>
        /// SMTP Host name/IP.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// SMTP Port.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// User name to login to SMTP server.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Password to login to SMTP server.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Domain name to login to SMTP server.
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// Is SSL enabled?
        /// </summary>
        bool EnableSsl { get; set; }

        /// <summary>
        /// Use default credentials?
        /// </summary>
        bool UseDefaultCredentials { get; set; }
    }
}