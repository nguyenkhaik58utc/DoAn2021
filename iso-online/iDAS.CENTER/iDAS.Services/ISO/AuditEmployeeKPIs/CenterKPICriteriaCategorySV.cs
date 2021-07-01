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
    public class CenterKPICriteriaCategorySV
    {
        private CenterKPICriteriaCategoryDA CenterKPICriteriaCategoryDA = new CenterKPICriteriaCategoryDA();
        public List<CenterKPICriteriaCategoryItem> GetData(ModelFilter filter, int naceCodeId)
        {
            List<CenterKPICriteriaCategoryItem> lst = new List<CenterKPICriteriaCategoryItem>();
            var checks = CenterKPICriteriaCategoryDA.GetQuery(i => i.ISONaceCodeID == naceCodeId)
                          .Filter(filter)
                          .ToList();
            if (checks.Count > 0)
            {
                foreach (var item in checks)
                {
                    lst.Add(new CenterKPICriteriaCategoryItem()
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
        public CenterKPICriteriaCategoryItem GetById(int id)
        {
            var dbo = CenterKPICriteriaCategoryDA.Repository;
            var criteraCategory = CenterKPICriteriaCategoryDA.GetById(id);
            var obj = new CenterKPICriteriaCategoryItem();
            if (criteraCategory != null)
            {
                obj.ID = criteraCategory.ID;
                obj.ISONaceCodeID = criteraCategory.ISONaceCodeID;
                obj.Name = criteraCategory.Name;
                obj.IsUse = criteraCategory.IsUse;
                obj.Note = criteraCategory.Note;
                obj.CreateAt = criteraCategory.CreateAt;
                obj.CreateBy = criteraCategory.CreateBy;
                obj.UpdateBy = criteraCategory.UpdateBy;
                obj.UpdateAt = criteraCategory.UpdateAt;

            }
            return obj;
        }
        public bool Insert(CenterKPICriteriaCategoryItem objNew)
        {
            var dbo = CenterKPICriteriaCategoryDA.Repository;
            CenterKPICriteriaCategory obj = new CenterKPICriteriaCategory();
            obj.ISONaceCodeID = objNew.ISONaceCodeID;
            obj.Name = objNew.Name;
            obj.IsUse = objNew.IsUse;
            obj.Note = objNew.Note;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = objNew.CreateBy;
            CenterKPICriteriaCategoryDA.Insert(obj);
            CenterKPICriteriaCategoryDA.Save();
            return true;
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            CenterKPICriteriaCategoryDA.DeleteRange(ids);
            CenterKPICriteriaCategoryDA.Save();
        }
        public bool Delete(int id)
        {
            var dbo = CenterKPICriteriaCategoryDA.Repository;
            var obj = CenterKPICriteriaCategoryDA.GetById(id);
            CenterKPICriteriaCategoryDA.Delete(obj);
            CenterKPICriteriaCategoryDA.Save();
            return true;
        }
        public bool Update(CenterKPICriteriaCategoryItem objEdit, int userId)
        {
            var dbo = CenterKPICriteriaCategoryDA.Repository;
            CenterKPICriteriaCategory obj = dbo.CenterKPICriteriaCategories.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.CenterKPICriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
            {
                return false;
            }
            obj.ISONaceCodeID = objEdit.ISONaceCodeID;
            obj.Name = objEdit.Name;
            obj.IsUse = objEdit.IsUse;
            obj.Note = objEdit.Note;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            dbo.SaveChanges();
            return true;
        }
        public bool CheckNameExits(string name)
        {
            var rs = CenterKPICriteriaCategoryDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
            if (rs.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
