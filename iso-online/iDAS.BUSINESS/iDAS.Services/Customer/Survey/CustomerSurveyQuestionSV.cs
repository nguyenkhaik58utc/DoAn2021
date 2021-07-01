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
    public class CustomerSurveyQuestionSV
    {
        private CustomerSurveyQuestionDA CustomerSurveyQuestionDA = new CustomerSurveyQuestionDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerSurveyQuestionItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerSurveyQuestionDA.Repository;
            var CustomerSurveyQuestion = CustomerSurveyQuestionDA.GetQuery()
                        .Select(item => new CustomerSurveyQuestionItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            SurveyID = item.CustomerSurveyID,
                            ParentID = item.ParentID,
                            IsMulti = item.IsMulti,
                            IsCategory = item.IsCategory,
                            IsUse = item.IsUse,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerSurveyQuestion;
        }
        /// <summary>
        /// Lấy danh sách câu hỏi theo cha và mã khảo sát
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public List<TreeSurveyQuestionItem> GetByParentAndSurvey(int ParentID, int SurveyID)
        {
            var dbo = CustomerSurveyQuestionDA.Repository;
            var CustomerCriteria = CustomerSurveyQuestionDA.GetQuery(i => i.CustomerSurveyID== SurveyID && i.ParentID == ParentID && i.IsCategory && !i.IsDelete)
                        .Select(item => new TreeSurveyQuestionItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            SurveyID = item.CustomerSurveyID,
                            IsCategory = item.IsCategory,
                            IsUse = item.IsUse,
                            CreateAt = item.CreateAt,
                            Leaf = dbo.CustomerSurveyQuestions.FirstOrDefault(i => i.CustomerSurveyID == SurveyID && i.ParentID == item.ID && i.IsCategory && !i.IsDelete) == null
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            ;
            return CustomerCriteria;
        }


        public List<CustomerAnswerItem> GetAnswer(int page, int pageSize, out int totalCount, int questionID)
        {
            var dbo = CustomerSurveyQuestionDA.Repository;
            var CustomerSurveyQuestion = dbo.CustomerSurveyQuestions.Where(i=>i.ParentID == questionID && !i.IsCategory && !i.IsDelete)
                        .Select(item => new CustomerAnswerItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            SurveyID = item.CustomerSurveyID,
                            ParentID = item.ParentID,
                            Sum = dbo.CustomerSurveyResults.Where(i=>i.CustomerSurveyQuestionID == item.ID).Count(),
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerSurveyQuestion;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerSurveyQuestionItem GetById(int Id)
        {
            var CustomerSurveyQuestion = CustomerSurveyQuestionDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerSurveyQuestionItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            SurveyID = item.CustomerSurveyID,
                            IsMulti = item.IsMulti,
                            IsCategory = item.IsCategory,
                            IsUse = item.IsUse,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerSurveyQuestion;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerSurveyQuestionItem item, int userID)
        {
            var CustomerSurveyQuestion = CustomerSurveyQuestionDA.GetById(item.ID);
            CustomerSurveyQuestion.Name = item.Name;
            CustomerSurveyQuestion.CustomerSurveyID = item.SurveyID;
            CustomerSurveyQuestion.ParentID = item.ParentID;
            CustomerSurveyQuestion.IsCategory = item.IsCategory;
            CustomerSurveyQuestion.IsMulti = item.IsMulti;
            CustomerSurveyQuestion.IsUse = item.IsUse;
            CustomerSurveyQuestion.Note = item.Note;
            CustomerSurveyQuestion.UpdateAt = DateTime.Now;
            CustomerSurveyQuestion.UpdateBy = userID;
            CustomerSurveyQuestionDA.Save();
        }

        public void UpdateAnswer(CustomerSurveyQuestionItem item)
        {
            var CustomerSurveyAnswer = CustomerSurveyQuestionDA.GetById(item.ID);
            CustomerSurveyAnswer.Name = item.Name;
            CustomerSurveyAnswer.UpdateAt = DateTime.Now;
            CustomerSurveyAnswer.UpdateBy = item.UpdateBy;
            CustomerSurveyQuestionDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerSurveyQuestionItem item, int userID)
        {
            var CustomerSurveyQuestion = new CustomerSurveyQuestion()
            {
                Name = item.Name,
                CustomerSurveyID = item.SurveyID,
                ParentID = item.ParentID,
                IsMulti = item.IsMulti,
                IsCategory = item.IsCategory,
                IsUse = item.IsUse,
                IsDelete = false,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerSurveyQuestionDA.Insert(CustomerSurveyQuestion);
            CustomerSurveyQuestionDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            
            var dbo = CustomerSurveyQuestionDA.Repository;
            dbo.CustomerSurveyQuestions.RemoveRange(getChilds(dbo.CustomerSurveyQuestions, id));
            dbo.SaveChanges();
        }
        private IEnumerable<CustomerSurveyQuestion> getChilds(IEnumerable<CustomerSurveyQuestion> e, int id)
        {
            var list1 = e.Where(a => a.ParentID == id);
            var list2 = e.Where(a => a.ID == id);
            list2.Concat(list1);
            return list2.Concat(list1.SelectMany(a => getChilds(e, a.ID)));
        }
    }
}
