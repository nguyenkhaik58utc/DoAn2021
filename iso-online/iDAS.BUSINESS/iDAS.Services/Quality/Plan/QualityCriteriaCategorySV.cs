using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;


namespace iDAS.Services
{
    public class QualityCriteriaCategorySV
    {
        private QualityCriteriaCategoryDA criteriaCategoryDA = new QualityCriteriaCategoryDA();
        public List<QualityCriteriaCategoryItem> GetData(int employeeId, bool isAdminstrator = false)
        {

            var checks = criteriaCategoryDA.GetQuery(t => !t.IsDelete && t.IsActive && (t.EmployeeID == employeeId || isAdminstrator))
                          .Select(item => new QualityCriteriaCategoryItem()
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
        public List<TreeCriteriaCategoryItem> GetTreeData(string nodeId)
        {
            int parentId = nodeId == "root" ? 0 : Convert.ToInt32(nodeId);
            var dbo = criteriaCategoryDA.Repository;
            var lsDataNode = dbo.QualityCriteriaCategories.Where(i => i.ParentID == parentId && i.IsDelete == false)
                                            .Select(item => new TreeCriteriaCategoryItem()
                                            {
                                                ID = item.ID,
                                                IsDelete = item.IsDelete,
                                                Name = item.Name,
                                                ParentID = item.ParentID,
                                                IsUse = item.IsActive,
                                                Leaf = dbo.QualityCriteriaCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                                            }
                                           ).ToList();
            return lsDataNode;
        }
        public List<TreeCriteriaCategoryItem> GetProductionCategory(int parentId)
        {
            var dbo = criteriaCategoryDA.Repository;
            var lsDataNode = dbo.QualityCriteriaCategories.Where(i => i.ParentID == parentId && i.IsProduction && !i.IsDelete)
                                            .Select(item => new TreeCriteriaCategoryItem()
                                            {
                                                ID = item.ID,
                                                IsDelete = item.IsDelete,
                                                Name = item.Name,
                                                ParentID = item.ParentID,
                                                IsUse = item.IsActive,
                                                Leaf = dbo.QualityCriteriaCategories.Where(i => i.IsProduction && i.ParentID == item.ID).FirstOrDefault() == null
                                            }
                                           ).ToList();
            return lsDataNode;
        }
        public QualityCriteriaCategoryItem GetById(int id)
        {
            var dbo = criteriaCategoryDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            var assessCategories = criteriaCategoryDA.GetById(id);
            var obj = new QualityCriteriaCategoryItem();
            if (assessCategories != null)
            {
                obj.ID = assessCategories.ID;
                obj.Name = assessCategories.Name;
                obj.ParentID = assessCategories.ParentID;
                obj.IsUse = assessCategories.IsActive;
                obj.IsDelete = assessCategories.IsDelete;
                obj.Audit = employeeSV.GetEmployeeView(assessCategories.EmployeeID);
                obj.CreateAt = assessCategories.CreateAt;
                obj.CreateBy = assessCategories.CreateBy;
                obj.UpdateBy = assessCategories.UpdateBy;
                obj.UpdateAt = assessCategories.UpdateAt;
            }
            return obj;
        }
        public bool Insert(QualityCriteriaCategoryItem objNew)
        {
            var dbo = criteriaCategoryDA.Repository;
            QualityCriteriaCategory obj = new QualityCriteriaCategory();
            if (dbo.QualityCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper()) != null)
            {
                return false;
            }
            obj.Name = objNew.Name;
            obj.ParentID = objNew.ParentID;
            if (objNew.Audit != null)
                obj.EmployeeID = objNew.Audit.ID;
            obj.IsActive = objNew.IsUse;
            obj.IsDelete = objNew.IsDelete;
            obj.IsProduction = objNew.IsProduction;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = objNew.CreateBy;
            dbo.QualityCriteriaCategories.Add(obj);
            dbo.SaveChanges();
            if (objNew.IsTerm)
            {
                //iSOStandartCriteriaCategorySV.Insert(obj.ID, (int)objNew.ISOStandartID, (int)objNew.CreateBy);

            }
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
            if (dbo.QualityCriteriaCategories.FirstOrDefault(i => i.ParentID == id) != null)
            {
                dbo.QualityCriteriaCategories.RemoveRange(dbo.QualityCriteriaCategories.Where(i => i.ID == id));
                dbo.SaveChanges();
                return true;
            }
            else
            {
                var obj = criteriaCategoryDA.GetById(id);
                criteriaCategoryDA.Delete(obj);
                criteriaCategoryDA.Save();
                return true;
            }
            return false;
        }
        public bool Update(QualityCriteriaCategoryItem objEdit, int userId)
        {
            try
            {
                var dbo = criteriaCategoryDA.Repository;
                QualityCriteriaCategory obj = dbo.QualityCriteriaCategories.FirstOrDefault(i => i.ID == objEdit.ID);
                if (dbo.QualityCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                     && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
                {
                    return false;
                }
                obj.Name = objEdit.Name;
                obj.ParentID = objEdit.ParentID;
                if (objEdit.Audit != null)
                    obj.EmployeeID = objEdit.Audit.ID;
                obj.IsActive = objEdit.IsUse;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userId;
                dbo.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CheckNameExits(string name)
        {
            try
            {
                var rs = criteriaCategoryDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
                if (rs.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<QualityCriteriaCategoryItem> GetGroupsByParentID(int parentId)
        {
            var groups = criteriaCategoryDA.GetQuery(t => t.ParentID == parentId)
                           .Select(item => new QualityCriteriaCategoryItem()
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
