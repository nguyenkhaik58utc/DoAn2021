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
                return typeof(UserSV);
            }
        }
        public string ConfigUrl
        {
            get
            {
                return "~/Account/Config";
            }
        }
        public string AccessDeniedUrl
        {
            get {
                return "~/Account/AccessDenied";
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
            get {
                return "CENTER";
            }
        }
        public string ConfigFilePath
        {
            get
            {
                return "~/App_Data/idassystem.config";
            }
        }
        public static void Register()
        {
            BaseConfig.Initialize(Config);
        }

        public string PathUpload
        {
            get
            {
                //return "";
                //service get url from database
                //SystemSV CenterSystemSV = new SystemSV();
                //var stringUrl = CenterSystemSV.GetUrlByCode("CENTER");
                //HttpContext.Current.Application["HostURL"] = stringUrl;
                //return HttpContext.Current.Application["HostURL"] as string;
                return null;
            }
        }
    }
}