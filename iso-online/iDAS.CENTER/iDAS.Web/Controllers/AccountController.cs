using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using Newtonsoft.Json;
using iDAS.Items;
namespace iDAS.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public ActionResult Captcha(string prefix, bool effect = true)
        {
            Captcha security = new Captcha();
            return security.CreateCaptcha(prefix, effect);
        }

        public ActionResult ChangeLanguague(string languague)
        {
            BaseCulture.SetCultureOfUserPrincipal(languague);
            X.Redirect(Url.Action("Login"));

            return this.Direct();
        }

        public ActionResult Login()
        {
            ViewBag.Languagues = getLanguagues();
            ViewBag.LanguagueDefault = getLanguagueDefault();
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginItem user)
        {
            if (ModelState.IsValid)
            {
                var error = SystemAuthenticate.Login(user);
                if (error == ELogin.Success)
                {
                    X.Redirect(SystemAuthenticate.LoginSuccessUrl);
                }
                else
                {
                    string message = getMessageError(error);
                    Ultility.CreateMessageBox(Message.Notify, message, MessageBox.Icon.WARNING);
                }
            }

            return this.FormPanel(this.ModelState);
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Config()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Config(Database database)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var check = BaseDatabase.CheckDatabaseExist(database);
                    BaseConfig.SetDatabaseCenter(database);
                    if (!check)
                    {
                        var systemSV = new SystemSV(); 
                        systemSV.SetupSystem();
                        systemSV.UpdateSystem();
                        systemSV.RegisterSystem();
                    }
                    X.Redirect("~/Account/Login");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this.Direct();
        }
   
        public ActionResult Logout()
        {
            SystemAuthenticate.Logout();
            return Redirect(SystemAuthenticate.LoginUrl);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        private ListItem getLanguagueDefault() {
            string culture = BaseCulture.GetCultureOfUserPrincipal(User);
            string text = BaseCulture.GetTextCulture(culture);

            return new ListItem { Text = text, Value = culture };
        }

        private SelectList getLanguagues()
        {
            //List<ListItem> languagues = new List<ListItem>();

            //foreach (var culture in ManagerCulture.GetCultures()) {
            //    ListItem languague = new ListItem { Text = culture.CultureName, Value = culture.CultureCode };
            //    languagues.Add(languague);
            //}

            //return languagues;
            var languagueDefault = BaseCulture.GetCultureOfUserPrincipal(User);
            var languagues = new SelectList(BaseCulture.GetCultures(), "CultureCode", "CultureName", languagueDefault);

            return languagues;
        }

        private string getMessageError(ELogin error)
        {
            string message = string.Empty;
            switch (error)
            {
                case ELogin.NotSupportCookie: message = Message.CookiesNotSupport; break;
                case ELogin.ErrorInfo: message = Message.LoginInfoIncorrect; break;
                case ELogin.NotActivated: message = Message.LoginNotActivated; break;
                case ELogin.ErrorCode: message = Message.LoginCodeIncorrect; break;
                case ELogin.Success: message = Message.LoginSuccess; break;
                case ELogin.Fail: message = Message.LoginFail; break;
            }
            return message;
        }
    }
}
