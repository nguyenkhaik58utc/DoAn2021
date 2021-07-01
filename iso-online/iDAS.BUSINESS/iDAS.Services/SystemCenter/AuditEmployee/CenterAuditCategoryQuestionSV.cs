using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class CenterAuditCategoryQuestionSV
    {
        private CenterAuditCategoryQuestionDA criteriaCategoryDA = new CenterAuditCategoryQuestionDA();
        public List<CenterAuditCategoryQuestionItem> GetCombobox()
        {
            var dbo = criteriaCategoryDA.Repository;
            var checks = criteriaCategoryDA.GetQuery(t => !t.IsDelete && t.IsUse)
                          .Select(item => new CenterAuditCategoryQuestionItem()
                          {
                              ID = item.ID,
                              IsDelete = item.IsDelete,
                              Name = item.Name,
                              ParentID = item.ParentID,
                              CreateAt = item.CreateAt,
                              NumberQuestion = dbo.CenterAuditQuestions.Where(t => t.CenterAuditCategoryQuestionID == item.ID && !t.IsDelete && t.IsUse).Count()

                          })

                          .OrderByDescending(item => item.CreateAt)
                          .ToList();
            return checks;
        }
        public List<CenterAuditCategoryQuestionItem> GetData(int employeeId, bool isAdminstrator = false)
        {

            var checks = criteriaCategoryDA.GetQuery(t => !t.IsDelete)
                          .Select(item => new CenterAuditCategoryQuestionItem()
                          {
                              ID = item.ID,
                              IsDelete = item.IsDelete,
                              Name = item.Name,
                              ParentID = item.ParentID,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .ToList();
            return checks;
        }
        public List<TreeCriteriaCategoryItem> GetTreeData(string nodeId, int naceCodeID)
        {
            int parentId = nodeId == "root" ? 0 : Convert.ToInt32(nodeId);
            var dbo = criteriaCategoryDA.Repository;
            var lsDataNode = dbo.CenterAuditCategoryQuestions
                                .Where(i => i.ParentID == parentId && i.ISONaceCodeID == naceCodeID && i.IsDelete == false && i.IsUse)
                                .Select(item => new TreeCriteriaCategoryItem()
                                {
                                    ID = item.ID,
                                    IsDelete = item.IsDelete,
                                    Name = item.Name,
                                    ParentID = item.ParentID,
                                    IsUse = item.IsUse,
                                    Leaf = dbo.CenterAuditCategoryQuestions.FirstOrDefault(i => i.ParentID == item.ID) == null
                                }
                                ).ToList();
            return lsDataNode;
        }
        public CenterAuditCategoryQuestionItem GetById(int id)
        {
            var dbo = criteriaCategoryDA.Repository;
            var assessCategories = criteriaCategoryDA.GetById(id);
            var obj = new CenterAuditCategoryQuestionItem();
            if (assessCategories != null)
            {
                obj.ID = assessCategories.ID;
                obj.ISONaceCodeID = assessCategories.ISONaceCodeID;
                obj.Name = assessCategories.Name;
                obj.ParentID = assessCategories.ParentID;
                obj.IsUse = assessCategories.IsUse;
                obj.IsDelete = assessCategories.IsDelete;
                obj.CreateAt = assessCategories.CreateAt;
                obj.CreateBy = assessCategories.CreateBy;
                obj.UpdateBy = assessCategories.UpdateBy;
                obj.UpdateAt = assessCategories.UpdateAt;
            }
            return obj;
        }
        public List<CenterAuditCategoryQuestionItem> GetGroupsByParentID(int parentId)
        {
            var groups = criteriaCategoryDA.GetQuery(t => t.ParentID == parentId)
                           .Select(item => new CenterAuditCategoryQuestionItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               Name = item.Name
                           }).ToList();

            return groups;
        }
    }
}
