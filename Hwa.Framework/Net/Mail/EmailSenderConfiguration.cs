using System;

namespace Hwa.Framework.Net.Mail
{
    /// <summary>
    /// Implementation of <see cref="IEmailSenderConfiguration"/> that reads settings
    /// from <see cref="ISettingManager"/>.
    /// </summary>
    public abstract class EmailSenderConfiguration : IEmailSenderConfiguration
    {
        public string DefaultFromAddress
        {
            get;
            set;
        }

        public string DefaultFromDisplayName
        {
            get;
            set;
        }
    }
}