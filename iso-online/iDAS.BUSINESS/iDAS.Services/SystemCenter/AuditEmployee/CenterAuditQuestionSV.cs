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
    public class CenterAuditQuestionSV
    {
        private CenterAuditQuestionDA criteriaDA = new CenterAuditQuestionDA();
        public List<CenterAuditQuestionItem> GetByCateId(ModelFilter filter, int cateId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterAuditQuestions.Where(q => q.CenterAuditCategoryQuestionID == cateId && q.IsUse)
                          .Select(item => new CenterAuditQuestionItem()
                          {
                              ID = item.ID,
                              CenterAuditCategoryQuestionID = item.CenterAuditCategoryQuestionID,
                              CategoryName = item.CenterAuditCategoryQuestion.Name,
                              Name = item.Name,
                              IsUse = item.IsUse,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public List<CenterAuditQuestionItem> GetCriteriaUsedByCateIds(int page, int pageSize, out int total, string strCateIds)
        {
            var ids = strCateIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterAuditQuestions.Where(t =>
                t.IsDelete == false)
                          .Select(item => new CenterAuditQuestionItem()
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
        public List<CenterAuditQuestionItem> GetCriteriaUsedByCateId(int page, int pageSize, out int total, string strCateId)
        {
            int cateID = int.Parse(strCateId);
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterAuditQuestions.Where(t =>
                t.CenterAuditCategoryQuestionID == cateID
                && t.IsDelete == false)
                          .Select(item => new CenterAuditQuestionItem()
                          {
                              ID = item.ID,
                              CategoryName = item.CenterAuditCategoryQuestion.Name,
                              Name = item.Name,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return criterias;
        }
        public CenterAuditQuestionItem GetById(int id)
        {
            var result = criteriaDA.GetQuery(t => t.ID == id)
                .Select(i => new CenterAuditQuestionItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    IsUse = i.IsUse,
                    IsDelete = i.IsDelete,
                    CreateAt = i.CreateAt,
                    CenterAuditCategoryQuestionID = i.CenterAuditCategoryQuestion.ID,
                    CategoryName = i.CenterAuditCategoryQuestion.Name,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                    UpdateAt = i.UpdateAt,
                }
                ).FirstOrDefault();
            return result;
        }
    }
}
