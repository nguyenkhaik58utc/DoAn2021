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
                                .Where(i => i.ParentID == parentId && i.ISONaceCodeID == naceCodeID && i.IsDelete == false)
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
        public bool Insert(CenterAuditCategoryQuestionItem objNew)
        {
            var dbo = criteriaCategoryDA.Repository;
            CenterAuditCategoryQuestion obj = new CenterAuditCategoryQuestion();
            obj.ISONaceCodeID = objNew.ISONaceCodeID;
            obj.Name = objNew.Name;
            obj.ParentID = objNew.ParentID;
            obj.IsUse = objNew.IsUse;
            obj.IsDelete = objNew.IsDelete;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = objNew.CreateBy;
            dbo.CenterAuditCategoryQuestions.Add(obj);
            dbo.SaveChanges();
            return true;
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            criteriaCategoryDA.DeleteRange(ids);
            criteriaCategoryDA.Save();
        }
        public bool Delete(int id)
        {
            var dbo = criteriaCategoryDA.Repository;
            var obj = criteriaCategoryDA.GetById(id);
            criteriaCategoryDA.Delete(obj);
            criteriaCategoryDA.Save();
            return true;
        }
        public bool Update(CenterAuditCategoryQuestionItem objEdit, int userId)
        {
                var dbo = criteriaCategoryDA.Repository;
                CenterAuditCategoryQuestion obj = dbo.CenterAuditCategoryQuestions.FirstOrDefault(i => i.ID == objEdit.ID);
                obj.Name = objEdit.Name;
                obj.ISONaceCodeID = objEdit.ISONaceCodeID;
                obj.ParentID = objEdit.ParentID;
                obj.IsUse = objEdit.IsUse;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userId;
                dbo.SaveChanges();
                return true;
        }
        public bool CheckNameExits(string name)
        {
                var rs = criteriaCategoryDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
                if (rs.Count > 0)
                {
                    return true;
                }
                else
                    return false;
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
        public void GetGroupsByParentID(int id, ref List<int> groupIds)
        {
            var groups = GetGroupsByParentID(id);
            if (groups.Count <= 0) return;
            foreach (var group in groups)
            {
                groupIds.Add(group.ID);
                GetGroupsByParentID(group.ID, ref groupIds);
            }
        }
    }
}
