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
    public class CenterKPICriteriaSV
    {
        private CenterKPICriteriaDA criteriaDA = new CenterKPICriteriaDA();
        public List<CenterKPICriteriaItem> GetByCateId(ModelFilter filter, int cateId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterKPICriterias.Where(t => t.CenterKPICriteriaCategoryID == cateId)
                          .Select(item => new CenterKPICriteriaItem()
                          {
                              ID = item.ID,
                              CenterKPICriteriaCategoryID = item.CenterKPICriteriaCategoryID,
                              CategoryName = item.CenterKPICriteriaCategory.Name,
                              Factory = item.Factory,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public List<CenterKPICriteriaItem> GetByListCateId(ModelFilter filter, string lstCateId)
        {
            List<int> lstCateID = new List<int>();
            foreach (string s in lstCateId.Split(','))
            {
                int result = 0;
                if (int.TryParse(s, out result))
                    lstCateID.Add(result);
            }
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterKPICriterias.Where(t => lstCateID.Contains(t.CenterKPICriteriaCategoryID))
                          .Select(item => new CenterKPICriteriaItem()
                          {
                              ID = item.ID,
                              CenterKPICriteriaCategoryID = item.CenterKPICriteriaCategoryID,
                              CategoryName = item.CenterKPICriteriaCategory.Name,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public CenterKPICriteriaItem GetById(int id)
        {
            var result = criteriaDA.GetQuery(t => t.ID == id)
                .Select(i => new CenterKPICriteriaItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    CreateAt = i.CreateAt,
                    Note = i.Note,
                    Factory = i.Factory,
                    CenterKPICriteriaCategoryID = i.CenterKPICriteriaCategory.ID,
                    CategoryName = i.CenterKPICriteriaCategory.Name,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                    UpdateAt = i.UpdateAt,
                }
                ).FirstOrDefault();
            return result;
        }
    }
}
