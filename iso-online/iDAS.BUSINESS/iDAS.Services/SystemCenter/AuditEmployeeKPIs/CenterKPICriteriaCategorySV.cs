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
    }
}
