using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class CustomerSurveyResultSV
    {
        private CustomerSurveyResultDA CustomerSurveyResultDA = new CustomerSurveyResultDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerSurveyResultItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerSurveyResultDA.Repository;
            var CustomerSurveyResult = CustomerSurveyResultDA.GetQuery()
                        .Select(item => new CustomerSurveyResultItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            QuestionID = item.CustomerSurveyQuestionID,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerSurveyResult;
        }
        public List<CustomerSurveyResultItem> GetByCustomer(int page, int pageSize, out int totalCount, int CustomerID)
        {
            var CustomerSurveyResult = CustomerSurveyResultDA.GetQuery(i => i.CustomerID == CustomerID)
                        .Select(item => new CustomerSurveyResultItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            QuestionID = item.CustomerSurveyQuestionID,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerSurveyResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<TreeCusotmerSurveyItem> GetByCustomer(int ParentID, int CustomerID)
        {
            var dbo = CustomerSurveyResultDA.Repository;
            var cartegoryIDs = dbo.CustomerCategoryCustomers.Where(i => i.CustomerID == CustomerID).Select(i => i.CustomerCategoryID);
            var CustomerSurveyResult = dbo.CustomerSurveyQuestions
                    .Where(i => i.CustomerSurvey.CustomerSurveyObjects.FirstOrDefault(s => cartegoryIDs.Contains(s.CustomerCategoryID)) != null && i.ParentID == ParentID)
                        .Select(item => new TreeCusotmerSurveyItem()
                        {
                            ID = item.ID,
                            CustomerID = CustomerID,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            SurveyID = item.CustomerSurveyID,
                            IsCategory = item.IsCategory,
                            IsSelect = item.CustomerSurveyResults.FirstOrDefault(i => i.CustomerSurveyQuestionID == item.ID && i.CustomerID == CustomerID) == null ? false : true,
                            ResultID = item.CustomerSurveyResults.FirstOrDefault(i => i.CustomerSurveyQuestionID == item.ID && i.CustomerID == CustomerID).ID,
                            Leaf = dbo.CustomerSurveyQuestions.FirstOrDefault(i => i.ParentID == item.ID) == null,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return CustomerSurveyResult;
        }

        public List<SurveyMailQuestion> GetQuestionByCustomer(int customerID)
        {
            var dbo = CustomerSurveyResultDA.Repository;
            var cateId = dbo.CustomerCategoryCustomers.FirstOrDefault(c => c.CustomerID == customerID).CustomerCategoryID;
            var result = dbo.CustomerSurveyQuestions
                                   .Where(i => i.CustomerSurvey.CustomerSurveyObjects.Select(o => o.CustomerCategoryID).ToList().Contains(cateId)
                                                && i.IsCategory
                                                &&i.IsUse)
                                   .Select(
                                            item  => new SurveyMailQuestion()
                                             {
                                                 ID = item.ID,
                                                 Name =item.Name,
                                                 IsMulti = item.IsMulti,
                                                 Answer = dbo.CustomerSurveyQuestions.Where(i=>i.ParentID == item.ID && i.IsCategory == false)
                                                                .Select(i => new SurveyMailAnswer() { ID = i.ID, Name = i.Name }).ToList(),
                                             }
                                           ).ToList()
                                   ;
            return result;
        }

        public void SaveAnswerSurvey(int[] uIds, int customerId, string iDasCode = "Mã bảo mật gửi cho khách hàng")
        {
            var dbo = CustomerSurveyResultDA.Repository;
            List<CustomerSurveyResult> result = new List<CustomerSurveyResult>();
            var exits = dbo.CustomerSurveyResults.Where(i => i.CustomerID == customerId && uIds.Contains(i.CustomerSurveyQuestionID)).Select(i => i.CustomerSurveyQuestionID).ToList();
            var questionIds = uIds.Where(i => !exits.Contains(i));
            foreach (var questionId in questionIds)
            {
                result.Add(new CustomerSurveyResult()
                                    {
                                        CustomerID = customerId,
                                        CustomerSurveyQuestionID = questionId,
                                        CreateAt = DateTime.Now,
                                        CreateBy = 2
                                    }
                );
            }
            CustomerSurveyResultDA.InsertRange(result);
            CustomerSurveyResultDA.Save();
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerSurveyResultItem GetById(int Id)
        {
            var CustomerSurveyResult = CustomerSurveyResultDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerSurveyResultItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            QuestionID = item.CustomerSurveyQuestionID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerSurveyResult;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerSurveyResultItem item, int userID)
        {
            var CustomerSurveyResult = CustomerSurveyResultDA.GetById(item.ID);
            CustomerSurveyResult.CustomerID = item.CustomerID;
            CustomerSurveyResult.CustomerSurveyQuestionID = item.QuestionID;
            CustomerSurveyResult.UpdateAt = DateTime.Now;
            CustomerSurveyResult.UpdateBy = userID;
            CustomerSurveyResultDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerSurveyResultItem item, int userID)
        {
            var CustomerSurveyResult = new CustomerSurveyResult()
            {
                CustomerID = item.CustomerID,
                CustomerSurveyQuestionID = item.QuestionID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerSurveyResultDA.Insert(CustomerSurveyResult);
            CustomerSurveyResultDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerSurveyResultDA.Delete(id);
            CustomerSurveyResultDA.Save();
        }

        public string GetEmail(int id)
        {
            var dbo = CustomerSurveyResultDA.Repository;
            return dbo.Customers.Where(i => i.ID == id).Select(i => i.Email).FirstOrDefault();
        }
    }
}
