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
    public class HumanCriteriaCategorySV
    {
        private HumanAuditCriteriaCategoryDA criteriaCategoryDA = new HumanAuditCriteriaCategoryDA();
        public List<HumanAuditCriteriaCategoryItem> GetCombobox()
        {
            List<HumanAuditCriteriaCategoryItem> lst = new List<HumanAuditCriteriaCategoryItem>();
            var dbo = criteriaCategoryDA.Repository;
            var checks = criteriaCategoryDA.GetQuery(t => t.IsUse)
                          .OrderByDescending(item => item.CreateAt)
                          .ToList();
            foreach (var item in checks)
            {
                lst.Add(new HumanAuditCriteriaCategoryItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    CreateAt = item.CreateAt,
                    IsUse = item.IsUse,
                    Note = item.Note
                });
            }
            return lst;
        }
        public List<HumanAuditCriteriaCategoryItem> GetDataCoppy(ModelFilter filter, int cateId = 0)
        {
            List<HumanAuditCriteriaCategoryItem> lst = new List<HumanAuditCriteriaCategoryItem>();
            var checks = criteriaCategoryDA.GetQuery(t => t.ID != cateId && t.IsUse)
                          .Filter(filter)
                          .ToList();
            if (checks.Count > 0)
            {
                foreach (var item in checks)
                {
                    lst.Add(new HumanAuditCriteriaCategoryItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        CreateAt = item.CreateAt,
                        IsUse = item.IsUse,
                        Note = item.Note
                    });
                }
            }
            return lst;
        }
        public List<HumanAuditCriteriaCategoryItem> GetDataCoppy(ModelFilter filter)
        {
            List<HumanAuditCriteriaCategoryItem> lst = new List<HumanAuditCriteriaCategoryItem>();
            var checks = criteriaCategoryDA.GetQuery(t => t.IsUse)
                          .Filter(filter)
                          .ToList();
            if (checks.Count > 0)
            {
                foreach (var item in checks)
                {
                    lst.Add(new HumanAuditCriteriaCategoryItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        CreateAt = item.CreateAt,
                        IsUse = item.IsUse,
                        Note = item.Note
                    });
                }
            }
            return lst;
        }
        public List<HumanAuditCriteriaCategoryItem> GetData(ModelFilter filter)
        {
            List<HumanAuditCriteriaCategoryItem> lst = new List<HumanAuditCriteriaCategoryItem>();
            var checks = criteriaCategoryDA.GetQuery()
                          .Filter(filter)
                          .ToList();
            if (checks.Count > 0)
            {
                foreach (var item in checks)
                {
                    lst.Add(new HumanAuditCriteriaCategoryItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        CreateAt = item.CreateAt,
                        IsUse = item.IsUse,
                        Note = item.Note
                    });
                }
            }
            return lst;
        }
        public HumanAuditCriteriaCategoryItem GetById(int id)
        {
            var dbo = criteriaCategoryDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            var assessCategories = criteriaCategoryDA.GetById(id);
            var obj = new HumanAuditCriteriaCategoryItem();
            if (assessCategories != null)
            {
                obj.ID = assessCategories.ID;
                obj.Name = assessCategories.Name;
                obj.IsUse = assessCategories.IsUse;
                obj.Note = assessCategories.Note;
                obj.CreateAt = assessCategories.CreateAt;
                obj.CreateBy = assessCategories.CreateBy;
                obj.UpdateBy = assessCategories.UpdateBy;
                obj.UpdateAt = assessCategories.UpdateAt;

            }
            return obj;
        }
        public bool Insert(HumanAuditCriteriaCategoryItem objNew)
        {
            var dbo = criteriaCategoryDA.Repository;
            HumanAuditCriteriaCategory obj = new HumanAuditCriteriaCategory();
            if (dbo.QualityCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper()) != null)
            {
                return false;
            }
            obj.Name = objNew.Name;
            obj.IsUse = objNew.IsUse;
            obj.Note = objNew.Note;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = objNew.CreateBy;
            criteriaCategoryDA.Insert(obj);
            criteriaCategoryDA.Save();
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
        public bool Update(HumanAuditCriteriaCategoryItem objEdit, int userId)
        {
            try
            {
                var dbo = criteriaCategoryDA.Repository;
                HumanAuditCriteriaCategory obj = dbo.HumanAuditCriteriaCategories.FirstOrDefault(i => i.ID == objEdit.ID);
                if (dbo.HumanAuditCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                     && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
                {
                    return false;
                }
                obj.Name = objEdit.Name;
                obj.IsUse = objEdit.IsUse;
                obj.Note = objEdit.Note;
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

        public void UseIDASData(string strCriteriaCateIds, int parentId)
        {
            var criteriaCateIds = strCriteriaCateIds.Split(',').Select(i => int.Parse(i)).ToList();
            CenterKPICriteriaCategoryDA centerCategoryQuestionDA = new CenterKPICriteriaCategoryDA();
            var criteriaCates = centerCategoryQuestionDA.GetQuery(i => criteriaCateIds.Contains(i.ID)).ToList();
            criteriaCategoryDA.InsertRange(
                   criteriaCates
                        .Select(item => new HumanAuditCriteriaCategory()
                        {
                            Name = item.Name,
                            Note = item.Note,
                            IsUse = item.IsUse,
                            HumanAuditCriterias = item.CenterKPICriterias
                                                    .Select(
                                                               q => new HumanAuditCriteria()
                                                               {
                                                                   Name = q.Name,
                                                                   Note = q.Note,
                                                                   Factory = q.Factory,
                                                                   HumanAuditCriteriaPoints = q.CenterKPICriteriaPoints
                                                                                   .Select(
                                                                                       a => new HumanAuditCriteriaPoint()
                                                                                       {
                                                                                           Name = a.Name,
                                                                                           Note = a.Note,
                                                                                           Point = a.Point
                                                                                       }
                                                                                   ).ToList()
                                                               }
                                                    ).ToList()
                        }
                                ).ToList()
                );
            criteriaCategoryDA.Save();
        }
    }
}
