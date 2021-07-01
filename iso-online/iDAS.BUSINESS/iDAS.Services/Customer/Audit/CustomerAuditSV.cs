using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Base;
using iDAS.DA;
namespace iDAS.Services
{
    public class CustomerAssessSV
    {
        private CustomerAssessDA CustomerAssessDA = new CustomerAssessDA();
        /// <summary>
        /// Lấy danh sách đánh giá khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerAssessItem> GetAll(ModelFilter filter)
        {
            var dbo = CustomerAssessDA.Repository;
            var CustomerCare = CustomerAssessDA.GetQuery()
                        .Select(item => new CustomerAssessItem()
                        {
                            ID = item.ID,
                            CriteriaCategoryID = item.CustomerCriteriaCategoryID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsActive = item.IsActive,
                            Method = item.Method,
                            Cost = item.Cost,
                            Note = item.Note,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            ;
            return CustomerCare;
        }

        public List<CustomerAssessItem> GetByCustomer(int page, int pageSize, out int totalCount, int CustomerID)
        {
            var dbo = CustomerAssessDA.Repository;
            var cartegoryIDs = dbo.CustomerCategoryCustomers.Where(i => i.CustomerID == CustomerID).Select(i => i.CustomerCategoryID);
            var CustomerCare = CustomerAssessDA
                                .GetQuery(i => dbo.CustomerAssessObjects
                                                .FirstOrDefault(o => cartegoryIDs.Contains(o.CustomerCategoryID )&& o.CustomerAssessID == i.ID) != null
                                                    && i.IsActive == true
                                        )
                 //&& i.StartTime <= DateTime.Now && DateTime.Now<= i.EndTime
                        .Select(item => new CustomerAssessItem()
                        {
                            ID = item.ID,
                            CriteriaCategoryID = item.CustomerCriteriaCategoryID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsActive = item.IsActive,
                            Method = item.Method,
                            Cost = item.Cost,
                            Note = item.Note,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return CustomerCare;
        }
        /// <summary>
        /// Lấy đánh giá khách hàng theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerAssessItem GetById(int Id)
        {
            var CustomerCare = CustomerAssessDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerAssessItem()
                        {
                            ID = item.ID,
                            CriteriaCategoryID = item.CustomerCriteriaCategoryID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsActive = item.IsActive,
                            Method = item.Method,
                            Cost = item.Cost,
                            Note = item.Note
                        })
                        .First();
            return CustomerCare;
        }

        /// <summary>
        /// Cập nhật đánh giá khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerAssessItem item, int userID)
        {
            var CustomerAssess = CustomerAssessDA.GetById(item.ID);
           CustomerAssess.CustomerCriteriaCategoryID = item.CriteriaCategoryID;
            CustomerAssess.Name = item.Name;
            CustomerAssess.Method = item.Method;
            CustomerAssess.Cost = item.Cost;
            CustomerAssess.StartTime = item.StartTime;
            CustomerAssess.EndTime = item.EndTime;
            CustomerAssess.IsActive = item.IsActive;
            CustomerAssess.Note = item.Note;
            CustomerAssess.UpdateAt = DateTime.Now;
            CustomerAssess.UpdateBy = userID;
            CustomerAssessDA.Save();
        }
        /// <summary>
        /// Thêm mới đánh giá khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerAssessItem item, int userID)
        {
            var CustomerAssess = new CustomerAssess()
            {
                CustomerCriteriaCategoryID = item.CriteriaCategoryID,
                Name = item.Name,
                Note = item.Note,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                IsActive = item.IsActive,
                Method = item.Method,
                Cost = item.Cost,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerAssessDA.Insert(CustomerAssess);
            CustomerAssessDA.Save();
        }
        /// <summary>
        /// Xóa đánh giá khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerAssessDA.Delete(id);
            CustomerAssessDA.Save();
        }
        public List<SumCustomerAuditItem> GetCriteriasByParentID(int parentId, int auditId, int categoryId)
        {
            List<SumCustomerAuditItem> lst = new List<SumCustomerAuditItem>();
            var listCateIds = getIDsbyParentID(categoryId);
            var dbo = CustomerAssessDA.Repository;
            lst = dbo.CustomerCriterias.Where(t =>
                                               listCateIds.Contains(t.CustomerCriteriaCategoryID)
                                                && !t.IsDelete && t.IsActive)
                        .Select(c =>
                                    new SumCustomerAuditItem()
                                            {
                                                ID = c.ID,
                                                Name = c.Name,
                                                SumPoint = dbo.CustomerAssessResults.Where(r => r.CustomerCriteriaID 
                                                    == c.ID && r.CustomerAssessID == auditId).FirstOrDefault() != null ? dbo.CustomerAssessResults.Where(r => r.CustomerCriteriaID == c.ID && r.CustomerAssessID == auditId).Sum(p => p.Point) : 0,
                                                SumCustomerAudit= dbo.CustomerAssessResults.Where(r => r.CustomerCriteriaID == c.ID && r.CustomerAssessID == auditId).FirstOrDefault() != null ? dbo.CustomerAssessResults.Where(r => r.CustomerCriteriaID == c.ID && r.CustomerAssessID == auditId).Count() : 0,
                                            })
                        .ToList();
            return lst;
        }
        private List<int> getIDsbyParentID(int id)
        {
            List<int> resut = new List<int>();
            var dbo = CustomerAssessDA.Repository;
            var Ids = dbo.QualityCriteriaCategories.Where(i => i.ParentID == id).Select(i => i.ID).ToList();
            resut.Add(id);
            if (Ids.Count() > 0)
            {
                foreach (var item in Ids)
                {
                    resut.AddRange(getIDsbyParentID(item));
                }
            }
            return resut;
        }

        public StringBuilder ComboboxRender(int min, int max, string name)
        {
            StringBuilder strComboxbox = new StringBuilder();
            strComboxbox.Append("<select name='" + name + "' style='width: 50px;'>");
            strComboxbox.Append("<option>   </option>");
            for (int i = min; i <= max; i++)
            {
                strComboxbox.Append("<option value='" + i + "'>");
                strComboxbox.Append(i);
                strComboxbox.Append("</option>");
            }
            strComboxbox.Append("</select>");
            return strComboxbox;
        }
        public void SendMail(AuditMailObject mailobject, string request)
        {
            foreach (var customerItem in mailobject.Customers)
            {
                StringBuilder htmlSend = new StringBuilder();
                var questions = mailobject.MailContent;
                htmlSend.Append(" <form action='http://" + request + "/Account/SaveAuditMail'><table style='width: 100%;' >");
                htmlSend.Append(@"<tr>
                                       <td style='border: 1px dotted gray;text-align: center;'>Tên tiêu chí</td>
                                       <td style='border: 1px dotted gray; width: 50px;text-align: center;'>Điểm</td>
                                   </tr>");
                for (var i = 0; i < questions.Count(); i++)
                {
                    htmlSend.Append("<tr>");
                    htmlSend.Append("<td style='padding-left: 10px; word-wrap: break-word; border: 1px dotted gray;'>"
                                                    + (i + 1).ToString() + "." + questions[i].Name + "</td>");
                    htmlSend.Append("<td  style='border: 1px dotted gray;'>" + ComboboxRender(questions[i].MinPoint, questions[i].MaxPoint, "uId." + questions[i].ID));
                    htmlSend.Append("</td></tr>");
                }
                htmlSend.Append("</table><input type='Hidden' name ='auditId' value='" + mailobject.AuditId + "'/>");
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
                mail.Subject = "iDas-Test đánh giá";
                mail.IsBodyHtml = true;
                mail.EnableSSL = false;
                mail.SendEmail();
            }
        }

        public void SendMailAudit(int id, short pageSize, string request)
        {
            //Queue<AuditMailObject> queueSend = new Queue<AuditMailObject>();
            //var dbo = CustomerAssessDA.Repository;
            //var customerSend = dbo.CustomerAssessObjects.Where(i => i.CustomerAssessID == id)
            //    .SelectMany(i => dbo.CustomerCategoryCustomers.Where(c => c.Customer.IsOfficial && c.CustomerCategoryID == i.CustomerCategory.ID))
            //    .Select(i => i.Customer)
            //    .OrderBy(i => i.ID);
            //var listCateIds = getIDsbyParentID(dbo.CustomerAssesses.FirstOrDefault(i => i.ID == id).CustomerAssessID);
            //var mailContent = dbo.QualityCriterias.Where(t =>
            //                 listCateIds.Contains(t.CriteriaCategoryID) && !t.IsDelete && t.IsActive)
            //                 .Select(c => new AuditMailQuestion
            //                 {
            //                     ID = c.ID,
            //                     Name = c.Name,
            //                     MinPoint = c.MinPoint,
            //                     MaxPoint = c.MaxPoint,
            //                 })
            //                .ToList();
            //var count = customerSend.Count();
            //var page = count / pageSize;
            //while (page >= 0)
            //{
            //    var mailItem = new AuditMailObject
            //    {
            //        Customers = customerSend.Skip(page * pageSize).Take(pageSize).Select(
            //                       i => new CustomerDetail()
            //                       {
            //                           ID = i.ID,
            //                           Email = i.Email,
            //                           Name = i.Name,
            //                       }).ToList(),
            //        MailContent = mailContent,
            //        AuditId = id
            //    };
            //    queueSend.Enqueue(mailItem);
            //    SendMail(queueSend.Dequeue(), request);
            //    page = page - 1;
            //}
        }
    }
}
