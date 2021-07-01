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
                return typeof(HumanUserSV);
            }
        }
        public string ConfigUrl
        {
            get
            {
                return "~/Account/Config";
            }
        }
        public string Culture
        {
            get
            {
                return "vi-VN";
            }
        }
        public string SystemCode
        {
            get { return "BUSINESS"; }
        }
        public string ConfigFilePath
        { 
            get {
                return "~/App_Data/idassystem.config";
            } 
        }
        public string PathUpload { get; set; }
        public static void Register()
        {
            BaseConfig.Initialize(Config);
            BaseConfig.SetAccessDeniedScript("showAccessDenied()");
        }
    }
}