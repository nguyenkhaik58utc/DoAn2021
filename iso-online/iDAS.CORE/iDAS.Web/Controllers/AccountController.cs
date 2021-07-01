using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using iDAS.Web.Filters;
using iDAS.Web.Models;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using Newtonsoft.Json;
using iDAS.Items;
using iDAS.Base;
namespace iDAS.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public ActionResult Config()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Config1(string value = "")
        {

            Database database = new Database()
            {
                DataSource = "192.168.1.111",
                DataName = "iDasClientData",
                UserId = "sa",
                Password = "idas@123",
            };
            //BaseDatabase.DatabaseCenter = database;

            X.Redirect("~/");
            return this.Direct();
        }

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
            SystemCustomerService sv = new SystemCustomerService();
            sv.GetLogoById(1);

            ViewBag.Languagues = getLanguagues();
            ViewBag.LanguagueDefault = getLanguagueDefault();
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(SystemUserLoginItem user)
        {
            if (ModelState.IsValid)
            {
                var error = SystemAuthenticate.Login(user);
                if (error == ELogin.Success)
                {
                    X.Redirect(Url.Action("Index","Home"));
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

        [HttpPost]
        public ActionResult Register(SystemUserRegisterItem user)
        {
            if (!FormsAuthentication.CookiesSupported || ModelState.IsValid)
            {
                //var error = SystemAuthenticate.Register(user);
                //string message = getMessageError(error);
                //Ultility.CreateMessageBox(Message.Notify, message, MessageBox.Icon.WARNING);
            }

            return this.FormPanel(this.ModelState);
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

        private List<ListItem> getLanguagues() {
            List<ListItem> languagues = new List<ListItem>();

            foreach (var culture in BaseCulture.GetCultures()) {
                ListItem languague = new ListItem { Text = culture.CultureName, Value = culture.CultureCode };
                languagues.Add(languague);
            }

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
                //case ELogin.AccountExist: message = Message.AccountExist; break;
               // case ELogin.RegisterSuccess: message = Message.RegisterSuccess; break;
                //case ELogin.RegisterFail: message = Message.RegisterFail; break;
                case ELogin.Success: message = Message.LoginSuccess; break;
                case ELogin.Fail: message = Message.LoginFail; break;
            }
            return message;
        }
    }
}
