using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace iDAS.Core
{
    /// <summary>
    /// Base Culture
    /// </summary>
    public static class BaseCulture
    {
        private static readonly List<string> _validCultures = new List<string> { "en", "en-US", "vi", "vi-VN" };

        private static readonly List<string> _cultures = new List<string> { "en-US", "vi-VN" };

        public static readonly string DefaultCulture = "en-US";

        public static string GetDefaultCulture()
        {
            //case culture of user is null, return culture default of system
            if (string.IsNullOrEmpty(BaseConfig.Culture)) return BaseCulture.DefaultCulture;

            return BaseConfig.Culture;
        }

        public static string GetCorrectCulture(string culture)
        {
            //case culture is null or empty
            if (string.IsNullOrEmpty(culture))
            {
                return DefaultCulture;
            }

            //case culture not in list valid cultures
            if (_validCultures.Where(c => c.Equals(culture, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
            {
                return DefaultCulture;
            }

            //case culture is correct
            if (_cultures.Where(c => c.Equals(culture, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return culture;
            }

            //case culture is neutral culture
            return GetCultureByNeutralCulture(culture);
        }

        public static string GetCultureByNeutralCulture(string culture)
        {
            var n = GetNeutralCulture(culture);
            return (from c in _cultures where c.StartsWith(n) select c).First();
        }

        public static string GetNeutralCulture(string culture)
        {
            return culture.Split('-').First();
        }

        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static string GetCurretnNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }

        public static string GetTextCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                culture = DefaultCulture;
            }
            switch (culture)
            {
                case "vi-VN": return "Tiếng Việt";
                case "en-US": return "English";
                default: return string.Empty;
            }
        }

        public static void SetCultureOfUserPrincipal(string culture)
        {
            culture = BaseCulture.GetCorrectCulture(culture);
            var user = new UserPrincipal()
            {
                Languague = culture,
            };
            SystemAuthenticate.SetAuthCookies(user);
        }

        public static void SetCultureOfUserPrincipal(string culture, UserPrincipal user)
        {
            culture = BaseCulture.GetCorrectCulture(culture);
            user.Languague = culture;
            SystemAuthenticate.SetAuthCookies(user);
        }

        public static string GetCultureOfUserPrincipal()
        {
            var user = SystemAuthenticate.GetAuthCookies();
            string culture = BaseCulture.GetDefaultCulture();
            if (user != null && !string.IsNullOrEmpty(user.Languague))
            {
                culture = BaseCulture.GetCorrectCulture(user.Languague);
            }
            return culture;
        }

        public static string GetCultureOfUserPrincipal(UserPrincipal user)
        {
            string culture = BaseCulture.GetDefaultCulture();
            if (user != null && !string.IsNullOrEmpty(user.Languague))
            {
                culture = BaseCulture.GetCorrectCulture(user.Languague);
            }
            return culture;
        }

        public static List<Culture> GetCultures()
        {
            Culture vi = new Culture { CultureName = "Tiếng Việt", CultureCode = "vi-VN", NeutralCulture = "vi" };
            Culture en = new Culture { CultureName = "English", CultureCode = "en-US", NeutralCulture = "en" };

            List<Culture> cultures = new List<Culture> { vi, en };
            return cultures;
        }
    }
}
