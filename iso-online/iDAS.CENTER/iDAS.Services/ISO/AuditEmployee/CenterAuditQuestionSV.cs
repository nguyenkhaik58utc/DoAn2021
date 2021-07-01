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
            var criterias = dbo.CenterAuditQuestions.Where(t => t.CenterAuditCategoryQuestionID == cateId)
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
        /// <summary>
        /// CuongPC
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="strCateIds"></param>
        /// <returns></returns>
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
        public int Insert(CenterAuditQuestionItem objNew, int userId)
        {
            var dbo = criteriaDA.Repository;
            CenterAuditQuestion obj = new CenterAuditQuestion();
            if (dbo.CenterAuditQuestions.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper() && t.CenterAuditCategoryQuestionID == objNew.CenterAuditCategoryQuestionID) != null)
            {
                return 0;
            }
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Name = objNew.Name;
            obj.IsUse = objNew.IsUse;
            obj.IsDelete = objNew.IsDelete;
            obj.CenterAuditCategoryQuestionID = objNew.CenterAuditCategoryQuestionID;
            dbo.CenterAuditQuestions.Add(obj);
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
        public bool Update(CenterAuditQuestionItem objEdit, int userId)
        {
            var dbo = criteriaDA.Repository;
            CenterAuditQuestion obj = dbo.CenterAuditQuestions.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.CenterAuditQuestions.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
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
