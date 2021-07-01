using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Utilities;
namespace iDAS.Services
{
    public class CustomerSurveySV
    {
        private CustomerSurveyDA CustomerSurveyDA = new CustomerSurveyDA();
        /// <summary>
        /// Lấy danh sách khảo sát
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerSurveyItem> GetAll(ModelFilter filter)
        {
            var dbo = CustomerSurveyDA.Repository;
            var CustomerSurvey = CustomerSurveyDA.GetQuery()
                        .Select(item => new CustomerSurveyItem()
                        {
                            ID = item.ID,
                            Method = item.Method,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            Cost = item.Cost,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            ;
            return CustomerSurvey;
        }
        /// <summary>
        /// Lấy khảo sát theo ID
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerSurveyItem GetById(int Id)
        {
            var CustomerSurvey = CustomerSurveyDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerSurveyItem()
                        {
                            ID = item.ID,
                            Method = item.Method,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            Cost = item.Cost,
                            Note = item.Note,
                            StatusValue = (item.IsPause == true) ? "IsPause" : (item.IsFinish == true ? "IsFinish" : "IsNew"),
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerSurvey;
        }
        /// <summary>
        /// Cập nhật khảo sát
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerSurveyItem item, int userID)
        {
            var CustomerSurvey = CustomerSurveyDA.GetById(item.ID);
            CustomerSurvey.Method = item.Method;
            CustomerSurvey.Name = item.Name;
            CustomerSurvey.StartTime = item.StartTime;
            CustomerSurvey.EndTime = item.EndTime;
            CustomerSurvey.Cost = item.Cost;
            CustomerSurvey.IsFinish = item.IsFinish;
            CustomerSurvey.IsPause = item.IsPause;
            CustomerSurvey.Note = item.Note;
            CustomerSurvey.UpdateAt = DateTime.Now;
            CustomerSurvey.UpdateBy = userID;
            CustomerSurveyDA.Save();
        }
        /// <summary>
        /// Thêm mới khảo sát
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerSurveyItem item, int userID)
        {
            var CustomerSurvey = new CustomerSurvey()
            {
                Method = item.Method,
                Name = item.Name,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                IsFinish = item.IsFinish,
                IsPause = item.IsPause,
                Cost = item.Cost,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerSurveyDA.Insert(CustomerSurvey);
            CustomerSurveyDA.Save();
        }
        /// <summary>
        /// Xóa khảo sát
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerSurveyDA.Delete(id);
            CustomerSurveyDA.Save();
        }


        //private delegate void HandleSend(ObjectToSevicesMail mail);
        //public HandleSend Send(ObjectToSevicesMail mail);
        public void SendMail(SurveyMailObject mailobject, string request)
        {
            foreach (var customerItem in mailobject.Customers)
            {
                StringBuilder htmlSend = new StringBuilder();
                htmlSend.Append(" <form action='http://" + request + "/Account/SaveMail'>");
                var questions = mailobject.MailContent;
                for (var i = 0; i < questions.Count(); i++)
                {
                    htmlSend.Append("<div id='p" + i + "' style='border: 1px dotted gray; margin: 5px; padding: 5px;'><div>" + (i + 1).ToString() + "." + questions[i].Name + "</div><br/>");

                    for (var j = 0; j < questions[i].Answer.Count(); j++)
                    {
                        if (questions[i].IsMulti)
                        {
                            htmlSend.Append("<div><input type='checkbox' name='" + "uId." + i + "' value='" + questions[i].Answer[j].ID + "'/>"
                                        + questions[i].Answer[j].Name + "</div><br/>");
                        }
                        else
                        {
                            htmlSend.Append("<div><input type='radio' name='" + "uId." + i + "' value='" + questions[i].Answer[j].ID + "'/>"
                                    + questions[i].Answer[j].Name + "</div><br/>");
                        }
                    }
                    htmlSend.Append("</div>");
                }
                htmlSend.Append("<input type='Hidden' name ='customerId' value='" + customerItem.ID + "'/>");
                htmlSend.Append("<input type='Hidden' name ='iDasCode' value='iDasCode" + customerItem.ID + "'/>");
                htmlSend.Append("<button type='submit'>Gửi phản hồi</button></form>");
                iDAS.Utilities.Mail mail = new Utilities.Mail();
                mail.Host = "192.168.1.111";
                mail.Port = 25;
                mail.UserName = "tungnt@dastechnology.com";
                mail.UserPassword = "123456";
                mail.EmailToAddress = new[] { customerItem.Email };
                mail.Body = htmlSend.ToString();
                mail.Subject = "iDas-Test khảo sát";
                mail.IsBodyHtml = true;
                mail.EnableSSL = false;
                mail.SendEmail();
            }
        }
        public void SendMailSurvey(int id, short pageSize, string request)
        {
            Queue<SurveyMailObject> queueSend = new Queue<SurveyMailObject>();
            var dbo = CustomerSurveyDA.Repository;
            var source = dbo.CustomerSurveyObjects.Where(i => i.CustomerSurveyID == id)
                            .SelectMany(i => dbo.CustomerCategoryCustomers.Where(c => c.CustomerCategoryID == i.CustomerCategory.ID))
                            .Select(i=>i.Customer)
                            .OrderBy(i => i.ID);
            var mailContent = dbo.CustomerSurveyQuestions
                                   .Where(i => i.CustomerSurvey.ID == id
                                                    && i.IsCategory
                                                    && i.IsUse)
                                   .Select(
                                            item => new SurveyMailQuestion()
                                            {
                                                ID = item.ID,
                                                Name = item.Name,
                                                IsMulti = item.IsMulti,
                                                Answer = dbo.CustomerSurveyQuestions.Where(i => i.ParentID == item.ID && i.IsCategory == false)
                                                               .Select(i => new SurveyMailAnswer() { ID = i.ID, Name = i.Name }).ToList(),
                                            }
                                           ).ToList();
            var count = source.Count();
            var page = count / pageSize;
            while (page >= 0)
            {
                var mailItem = new SurveyMailObject
                {
                    Customers = source.Skip(page * pageSize).Take(pageSize)
                                .Select(
                                   i => new CustomerDetail()
                                   {
                                       ID = i.ID,
                                       Email = i.Email,
                                       Name = i.Name,
                                   }).ToList(),
                    MailContent = mailContent
                };
                queueSend.Enqueue(mailItem);
                SendMail(queueSend.Dequeue(), request);
                page = page - 1;
            }
        }
    }
}
