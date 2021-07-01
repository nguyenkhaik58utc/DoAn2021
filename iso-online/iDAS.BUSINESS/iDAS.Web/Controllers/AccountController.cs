using System;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;

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
            X.Redirect(SystemAuthenticate.LoginUrl);

            return this.Direct();
        }
        public ActionResult SaveMail(int customerId = 0, string iDasCode = "")
        {
            var t = Request.Params.AllKeys.Where(a => a.Contains("uId")).Select(a => System.Convert.ToInt32(Request.Params[a])).ToArray();
            if (t != null)
            {
                CustomerSurveyResultSV customerSv = new CustomerSurveyResultSV();
                customerSv.SaveAnswerSurvey(t, customerId, iDasCode);
                return PartialView("Success");
            }
            else
            {
                return PartialView("Error");
            }
        }
        public ActionResult SaveAuditMail(int customerId = 0, string iDasCode = "", int auditId = 0)
        {
            if (auditId != 0 && customerId != 0 && !string.IsNullOrEmpty(iDasCode))
            {
                try
                {
                    var result = Request.Params.AllKeys.Where(p => p.Contains("uId"))
                                             .Select(p => new CustomerAssessResultItem { CriteriaID = int.Parse(p.ToString().Substring(4), 0), Point = int.Parse(Request.Params[p]) })
                                             .ToList();
                    var CustomerAuditResultService = new CustomerAssessResultSV();
                    CustomerAuditResultService.SaveMailAudit(result, auditId, customerId);
                    return PartialView("Success");
                }
                catch
                {

                }

            }
            return PartialView("Error");
        }

        public ActionResult Config()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Config(iDAS.Core.Database database = null)
        {
            try
            {
                if (ModelState.IsValid && BaseDatabase.CheckDatabaseExist(database))
                {
                    BaseConfig.SetDatabaseCenter(database);
                    var systemSV = new CenterSystemSV();
                    systemSV.RegisterSystem();
                    X.Redirect("~/Account/Login");
                }
                else {
                    Ultility.ShowMessageBox(message: "Database Not Exist!!!", icon: MessageBox.Icon.WARNING);
                }
            }
            catch(Exception ex){
                throw ex;
            }
            return this.Direct();
        }
        
        public ActionResult Login()
        {
            ViewBag.Languagues = getLanguagues();
            return View();
        }

        [HttpPost]
        public ActionResult Login(BusinessUserLoginItem user)
        {
            // HungNM. Add errorCode for logging in.
            Ultility.SetErrorCodeLogin("0");
            // End.

            if (ModelState.IsValid)
            {
                var error = SystemAuthenticate.Login(user);
                if (error == ELogin.Success)
                {
                    X.Redirect(SystemAuthenticate.LoginSuccessUrl);
                }
                else
                {
                    // HungNM. Add errorCode for logging in.
                    if (Ultility.GetErrorCodeLogin() == "NotActive")
                    {
                        // Account is not activated
                        string message = getMessageError(error);
                        Ultility.CreateMessageBox(RequestMessage.Notify, message, MessageBox.Icon.WARNING);
                    }
                    else if (Ultility.GetErrorCodeLogin() == "errorPassword")
                    {
                        // Password is not correct
                        Ultility.CreateMessageBox(RequestMessage.Notify, "Mật khẩu không đúng!", MessageBox.Icon.WARNING);
                    }
                    // End.
                }
            }

            return this.FormPanel(this.ModelState);
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Logout()
        {
            SystemAuthenticate.Logout();
            return Redirect(SystemAuthenticate.LoginUrl);
        }

        public ActionResult AccessDenied()
        {
            Ultility.CreateMessageBox(RequestMessage.Notify, "Bạn không có quyền truy cập chức năng này !", MessageBox.Icon.WARNING);
            return this.Direct(false);
        }

        private SelectList getLanguagues()
        {
            var languagueDefault = BaseCulture.GetCultureOfUserPrincipal(User);
            var languagues = new SelectList(BaseCulture.GetCultures(), "CultureCode", "CultureName", languagueDefault);

            return languagues;
        }

        private string getMessageError(ELogin error)
        {
            string message = string.Empty;
            switch (error)
            {
                case ELogin.NotSupportCookie: message = RequestMessage.CookiesNotSupport; break;
                case ELogin.ErrorInfo: message = RequestMessage.LoginInfoIncorrect; break;
                case ELogin.NotActivated: message = RequestMessage.LoginNotActivated; break;
                case ELogin.ErrorCode: message = RequestMessage.LoginCodeIncorrect; break;
                case ELogin.Success: message = RequestMessage.LoginSuccess; break;
                case ELogin.Fail: message = RequestMessage.LoginFail; break;
            }
            return message;
        }
    }
}
