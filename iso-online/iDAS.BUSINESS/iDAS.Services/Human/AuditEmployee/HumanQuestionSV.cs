using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class HumanQuestionSV
    {
        private HumanQuestionDA criteriaDA = new HumanQuestionDA();
        private HumanPhaseAuditDA humanPhaseAuditDA = new HumanPhaseAuditDA();
        public List<HumanQuestionItem> GetByCateId(ModelFilter filter, int cateId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.HumanQuestions.Where(t => t.HumanCategoryQuestionID == cateId)
                          .Select(item => new HumanQuestionItem()
                          {
                              ID = item.ID,
                              HumanCategoryQuestionID = item.HumanCategoryQuestionID,
                              CategoryName = item.HumanCategoryQuestion.Name,
                              Name = item.Name,
                              IsUse = item.IsUse,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public List<HumanQuestionItem> GetByCateId(int cateId, int humanPhaseAuditId, int humanEmployeeAuditId)
        {
            var dbo = criteriaDA.Repository;
            var obj = humanPhaseAuditDA.GetById(humanPhaseAuditId);
            var checkexitday = dbo.HumanResultQuestions
                                  .Where(t => t.HumanEmployeeAuditID == humanEmployeeAuditId && t.CreateAt.Value.Day==DateTime.Now.Day)
                                  .Select(t => t.HumanQuestionID)
                                  .ToArray();
            if (checkexitday.Count() == 0)
            {
                var lstQuestionAnswer = dbo.HumanResultQuestions
                                         .Where(t => t.HumanEmployeeAuditID == humanEmployeeAuditId)
                                         .Select(t => t.HumanQuestionID)
                                         .ToArray();
                var listQuestions = dbo.HumanQuestions
                                        .Where(t => t.HumanCategoryQuestionID == cateId && t.IsUse && !t.IsDelete && !lstQuestionAnswer.Contains(t.ID))
                                        .Select(t => t.ID).ToArray();
                var lst = listQuestions.OrderBy(t => Guid.NewGuid()).Take(obj.NumberSendInDay).ToArray();
                var criterias = dbo.HumanQuestions.Where(t => t.HumanCategoryQuestionID == cateId && t.IsUse && !t.IsDelete && lst.Contains(t.ID))
                              .Select(item => new HumanQuestionItem()
                              {
                                  ID = item.ID,
                                  HumanCategoryQuestionID = item.HumanCategoryQuestionID,
                                  CategoryName = item.HumanCategoryQuestion.Name,
                                  Name = item.Name,
                                  IsMulti = item.HumanAnswers.Count(ans => ans.IsTrue) > 1 ? true : false,
                                  Answer = item.HumanAnswers.Select(ans => new HumanAnswerItem()
                                  {
                                      ID = ans.ID,
                                      Answer = ans.Answer,
                                  }).ToList(),
                                  IsDelete = item.IsDelete,
                                  CreateAt = item.CreateAt
                              })
                              .ToList();
                return criterias;
            }
            else
            {
                List<HumanQuestionItem> lst = new List<HumanQuestionItem>(); 
                return lst;
            }
           
        }
        public List<HumanQuestionItem> GetByCateIsUse(ModelFilter filter, int cateId, int humanPhaseAuditId, int humanEmployeeAuditId)
        {
            var dbo = criteriaDA.Repository;
            var obj = humanPhaseAuditDA.GetById(humanPhaseAuditId);
            var lstQuestionAnswer = dbo.HumanResultQuestions
                                     .Where(t => t.HumanEmployeeAuditID == humanEmployeeAuditId)
                                     .Select(t => t.HumanQuestionID)
                                     .ToArray();
            var listQuestions = dbo.HumanQuestions
                                    .Where(t => t.HumanCategoryQuestionID == cateId && t.IsUse && !t.IsDelete && !lstQuestionAnswer.Contains(t.ID))
                                    .Select(t => t.ID).ToArray();
            var lst = listQuestions.OrderBy(t => Guid.NewGuid()).Take(obj.NumberSendInDay).ToArray();
            var criterias = dbo.HumanQuestions.Where(t => t.HumanCategoryQuestionID == cateId && t.IsUse && !t.IsDelete && lst.Contains(t.ID))
                          .Select(item => new HumanQuestionItem()
                          {
                              ID = item.ID,
                              HumanCategoryQuestionID = item.HumanCategoryQuestionID,
                              CategoryName = item.HumanCategoryQuestion.Name,
                              Name = item.Name,
                              IsUse = item.IsUse,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }

        public List<HumanQuestionItem> GetCriteriaUsedByCateIds(int page, int pageSize, out int total, string strCateIds)
        {
            var ids = strCateIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var dbo = criteriaDA.Repository;
            var criterias = dbo.HumanQuestions.Where(t =>
                t.IsDelete == false)
                          .Select(item => new HumanQuestionItem()
                          {
                              ID = item.ID,
                              Name = item.Name,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return criterias;
        }
        /// <summary>
        /// CuongPC
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="strCateIds"></param>
        /// <returns></returns>
        public List<HumanQuestionItem> GetCriteriaUsedByCateId(int page, int pageSize, out int total, string strCateId)
        {
            int cateID = int.Parse(strCateId);
            var dbo = criteriaDA.Repository;
            var criterias = dbo.HumanQuestions.Where(t =>
                t.HumanCategoryQuestionID == cateID
                && t.IsDelete == false)
                          .Select(item => new HumanQuestionItem()
                          {
                              ID = item.ID,
                              CategoryName = item.HumanCategoryQuestion.Name,
                              Name = item.Name,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return criterias;
        }
        public HumanQuestionItem GetById(int id)
        {
            var result = criteriaDA.GetQuery(t => t.ID == id)
                .Select(i => new HumanQuestionItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    IsUse = i.IsUse,
                    IsDelete = i.IsDelete,
                    CreateAt = i.CreateAt,
                    HumanCategoryQuestionID = i.HumanCategoryQuestion.ID,
                    CategoryName = i.HumanCategoryQuestion.Name,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                    UpdateAt = i.UpdateAt,
                }
                ).FirstOrDefault();
            return result;
        }
        public int Insert(HumanQuestionItem objNew, int userId)
        {
            var dbo = criteriaDA.Repository;
            HumanQuestion obj = new HumanQuestion();
            if (dbo.HumanQuestions.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper() && t.HumanCategoryQuestionID == objNew.HumanCategoryQuestionID) != null)
            {
                return 0;
            }
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Name = objNew.Name;
            obj.IsUse = objNew.IsUse;
            obj.IsDelete = objNew.IsDelete;
            obj.HumanCategoryQuestionID = objNew.HumanCategoryQuestionID;
            dbo.HumanQuestions.Add(obj);
            dbo.SaveChanges();
            return obj.ID;
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            foreach (var item in ids)
            {
                criteriaDA.GetById(item).IsDelete = true;

            }
            criteriaDA.Save();
        }
        public void Delete(int id)
        {
            criteriaDA.Delete(id);
            criteriaDA.Save();
        }
        public bool Update(HumanQuestionItem objEdit, int userId)
        {
            var dbo = criteriaDA.Repository;
            HumanQuestion obj = dbo.HumanQuestions.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.HumanQuestions.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = objEdit.Name;
            obj.IsUse = objEdit.IsUse;
            obj.IsDelete = objEdit.IsDelete;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            dbo.SaveChanges();
            return true;
        }
    }
}
