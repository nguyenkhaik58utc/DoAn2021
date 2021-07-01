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
    public class CenterKPICriteriaPointSV
    {
        private CenterKPICriteriaPointDA criteriaDA = new CenterKPICriteriaPointDA();
        public void Delete(string strId)
        {
            var ids = strId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            criteriaDA.DeleteRange(ids);
            criteriaDA.Save();
        }
        public List<CenterKPICriteriaPointItem> GetByCateId(ModelFilter filter, int CriteriaId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterKPICriteriaPoints.Where(t => t.CenterKPICriteriaID == CriteriaId)
                          .Select(item => new CenterKPICriteriaPointItem()
                          {
                              ID = item.ID,
                              Point = item.Point,
                              CenterKPICriteriaID = item.CenterKPICriteriaID,
                              Note = item.Note,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .OrderByDescending(t => t.Point)
                          .ToList();
            return criterias;
        }
    }
}
