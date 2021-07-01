using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize =false)]
    public class CustomerEmailController : BaseController
    {
        private CustomerSurveyResultSV surveyResultSV = new CustomerSurveyResultSV();
        #region Gửi mail khảo sát
        [BaseSystem(Name = "Gửi Email khảo sát")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult SendMailSurvey(string ids)
        {
            var request = Request.Url.Authority;
            try
            {
                var lsId = ids.Split(',').Select(i => int.Parse(i));
                foreach (var id in lsId)
                {
                    string htmlSend = " <form action='http://" + request + "/Account/SaveMail'>";
                    var questions = surveyResultSV.GetQuestionByCustomer(id);
                    for (var i = 0; i < questions.Count(); i++)
                    {
                        htmlSend += "<div id='p" + i + "' style='border: 1px dotted gray; margin: 5px; padding: 5px;'><div>" + (i + 1).ToString() + "." + questions[i].Name + "</div><br/>";

                        for (var j = 0; j < questions[i].Answer.Count(); j++)
                        {
                            if (questions[i].IsMulti)
                            {
                                htmlSend += "<div><input type='checkbox' name='" + "uId." + i + "' value='" + questions[i].Answer[j].ID + "'/>"
                                            + questions[i].Answer[j].Name + "</div><br/>";
                            }
                            else
                            {
                                htmlSend += "<div><input type='radio' name='" + "uId." + i + "' value='" + questions[i].Answer[j].ID + "'/>"
                                       + questions[i].Answer[j].Name + "</div><br/>";
                            }
                        }
                        htmlSend += "</div>";
                    }
                    htmlSend += "<input type='Hidden' name ='customerId' value='" + id + "'/>";
                    htmlSend += "<input type='Hidden' name ='iDasCode' value='iDasCode" + id + "'/>";
                    htmlSend += "<button type='submit'>Gửi phản hồi</button></form>";
                    //HtmlString m = new HtmlString("<b>Alooooo!</b>");
                    //PartialViewRenderer n = new PartialViewRenderer();
                    //var sw = new System.IO.StringWriter();
                    //var viewresult = ViewEngines.Engines.FindPartialView(this.ControllerContext, "Partial1");
                    //var viewcontext = new ViewContext(this.ControllerContext, viewresult.View, ViewData, TempData, sw);
                    //viewresult.View.Render(viewcontext, sw);
                    //var r = sw.GetStringBuilder().ToString();
                    iDAS.Utilities.Mail mail = new Utilities.Mail();
                    mail.Host = "192.168.1.111";
                    mail.Port = 25;
                    mail.UserName = "tungnt@dastechnology.com";
                    mail.UserPassword = "123456";
                    mail.EmailToAddress = new[] { surveyResultSV.GetEmail(id) };
                    mail.Body = htmlSend;
                    mail.Subject = "iDas-Test khảo sát";
                    mail.IsBodyHtml = true;
                    mail.EnableSSL = false;
                    //mail.SendEmail();
                    Ultility.CreateNotification(message: RequestMessage.SendSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.Direct();
        }
        #endregion
    }
}
