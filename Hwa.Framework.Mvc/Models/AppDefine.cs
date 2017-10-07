using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc.Model
{

    public class APP_DEFINE
    {
        #region Passport
        public const string PASSPORT_COOKIE_NAME = "HwaTokenID";

        public static string ENABLE_PASSPORT_LOGIN_VERIFICATION = System.Configuration.ConfigurationManager.AppSettings["HwPassport:Enabled"] ?? "";

        public static string DOMAIN = System.Configuration.ConfigurationManager.AppSettings["HwPassport:Domain"] ?? "";

        public static string APP_PASSPORT_URL = System.Configuration.ConfigurationManager.AppSettings["HwPassport:PassportUrl"] ?? "";

        public static string APP_RETAIL_HOME_URL = System.Configuration.ConfigurationManager.AppSettings["HwPassport:CloudRetailHomeUrl"] ?? "";

        public static string APP_RESTAURANT_HOME_URL = System.Configuration.ConfigurationManager.AppSettings["HwPassport:CloudRestaurantHomeUrl"] ?? "";

        public static string ALLOW_ANONYMOUS_ACCESS = System.Configuration.ConfigurationManager.AppSettings["HwCloud:AllowAnonymousAccess"] ?? "";
        #endregion

        public static string WEB_SITE_IDENTITY = System.Configuration.ConfigurationManager.AppSettings["HwCloud:WebsiteIdentity"] ?? "";

        public const string HTTP_REQUEST_SINGLETON_KEY = "Hwa.Framework.Mvc.RequestSingleton";

        public static string CACHE_MODE = System.Configuration.ConfigurationManager.AppSettings["HwCache:Mode"] ?? "";
        public static string REDIS_CONNECTION = System.Configuration.ConfigurationManager.AppSettings["HwCache:RdsConnection"] ?? "";

        public static string WEB_CACHE_KEY_PREFIX = System.Configuration.ConfigurationManager.AppSettings["WebsiteCacheKeyPrefix"] ?? "";

    }
}
