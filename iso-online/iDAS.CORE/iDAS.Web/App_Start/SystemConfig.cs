using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iDAS.Core;
using iDAS.Services;

namespace iDAS.Web
{
    public class SystemConfig : IConfig
    {
        private SystemConfig() { }

        private static SystemConfig instance;

        public static SystemConfig Config
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemConfig();
                }
                return instance;
            }
        }

        public Type UserService
        {
            get
            {
                return typeof(SystemUserService);
            }
        }

        public string Culture
        {
            get
            {
                return "vi-VN";
            }
        }

        public string ConfigUrl
        {
            get
            {
                return "~/Account/Config";
            }
        }
        public string AccessDeniedUrl { get; set; }
        public string SystemCode { get; set; }

        public static void Register() {
            BaseConfig.Initialize(Config);
            //BaseDatabase.DatabaseCenter = new Database()
            //{
            //    DataSource = "192.168.1.111",
            //    DataName = "iDAS_CORE22",
            //    UserId = "sa",
            //    Password = "idas@123",
            //};
        }
    }
}