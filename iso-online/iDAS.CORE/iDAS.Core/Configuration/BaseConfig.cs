using System;
using System.Reflection;

namespace iDAS.Core
{
    /// <summary>
    /// Configuration System
    /// </summary>
    public class BaseConfig
    {
        internal static Assembly System;
        internal static string Culture;
        internal static Type UserService;
        internal static string ConfigUrl;
        internal static string SystemCode;
        internal static string AccessDeniedUrl;
        internal static string AccessDeniedScript;
        internal static string ConfigFilePath;
        internal static string PathUpload;

        /// <summary>
        /// Initialize Configs System
        /// </summary>
        public static void Initialize(IConfig config)
        {
            System = config.GetType().Assembly;
            UserService = config.UserService;
            Culture = config.Culture;
            ConfigUrl = config.ConfigUrl;
            ConfigFilePath = config.ConfigFilePath;
            SystemCode = config.SystemCode;
            PathUpload = config.PathUpload;
        }

        public static void SetAccessDeniedUrl(string url)
        {
            AccessDeniedUrl = url;
        }
        public static void SetAccessDeniedScript(string script)
        {
            AccessDeniedScript = script;
        }
        public static void SetDatabaseCenter(Database database)
        {
            BaseDatabase.DatabaseCenter = database;
        }
        public static void SetDatabaseByCode(Database database)
        {
            BaseDatabase.DatabaseByCode = database;
        }
        public static void SetPathUpload(string pathUpload)
        {
            PathUpload = pathUpload;
        }
    }
}
