namespace Hwa.Framework.Net.Mail
{
    /// <summary>
    /// Defines configurations used while sending emails.
    /// </summary>
    public interface IEmailSenderConfiguration
    {
        /// <summary>
        /// Default from address.
        /// </summary>
        string DefaultFromAddress { get; set; }
        
        /// <summary>
        /// Default display name.
        /// </summary>
        string DefaultFromDisplayName { get; set; }
    }
}