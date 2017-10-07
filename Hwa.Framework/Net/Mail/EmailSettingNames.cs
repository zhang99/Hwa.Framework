namespace Hwa.Framework.Net.Mail
{
    /// <summary>
    /// Declares names of the settings defined by <see cref="EmailSettingProvider"/>.
    /// </summary>
    public static class EmailSettingNames
    {
        /// <summary>
        /// Hwa.Net.Mail.DefaultFromAddress
        /// </summary>
        public const string DefaultFromAddress = "Hwa.Net.Mail.DefaultFromAddress";

        /// <summary>
        /// Hwa.Net.Mail.DefaultFromDisplayName
        /// </summary>
        public const string DefaultFromDisplayName = "Hwa.Net.Mail.DefaultFromDisplayName";

        /// <summary>
        /// SMTP related email settings.
        /// </summary>
        public static class Smtp
        {
            /// <summary>
            /// Hwa.Net.Mail.Smtp.Host
            /// </summary>
            public const string Host = "Hwa.Net.Mail.Smtp.Host";

            /// <summary>
            /// Hwa.Net.Mail.Smtp.Port
            /// </summary>
            public const string Port = "Hwa.Net.Mail.Smtp.Port";

            /// <summary>
            /// Hwa.Net.Mail.Smtp.UserName
            /// </summary>
            public const string UserName = "Hwa.Net.Mail.Smtp.UserName";

            /// <summary>
            /// Hwa.Net.Mail.Smtp.Password
            /// </summary>
            public const string Password = "Hwa.Net.Mail.Smtp.Password";

            /// <summary>
            /// Hwa.Net.Mail.Smtp.Domain
            /// </summary>
            public const string Domain = "Hwa.Net.Mail.Smtp.Domain";

            /// <summary>
            /// Hwa.Net.Mail.Smtp.EnableSsl
            /// </summary>
            public const string EnableSsl = "Hwa.Net.Mail.Smtp.EnableSsl";

            /// <summary>
            /// Hwa.Net.Mail.Smtp.UseDefaultCredentials
            /// </summary>
            public const string UseDefaultCredentials = "Hwa.Net.Mail.Smtp.UseDefaultCredentials";
        }
    }
}