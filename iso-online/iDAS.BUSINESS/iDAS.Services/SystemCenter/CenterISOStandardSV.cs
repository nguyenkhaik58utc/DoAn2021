using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ISOStandardSV
    {
        private CenterISOStandardDA iSODA = new CenterISOStandardDA();
        private ISOStandartCriteriaSV iSOStandartCriteriaCategorySV = new ISOStandartCriteriaSV();
        private ISOStandartCriteriaSV iSOStandartCriteriaSV = new ISOStandartCriteriaSV();
        public CenterISOIndexCriteriaItem GetByID(int id)
        {
            return iSOStandartCriteriaSV.GetByID(id);
        }
        public string GetNameByID(int id)
        {
            var rs = iSODA.GetById(id);
            if (rs != null)
                return rs.Name;
            return "Tiêu chuẩn đã bị xóa";
        }
        public List<CenterISOStandardItem> GetAll(int page, int pageSize, out int total)
        {

            var isos = iSODA.GetQuery().Where(t=>t.IsActive)
                          .Select(item => new CenterISOStandardItem()
                          {
                              ID = item.ID,
                              Name = item.Name,
                              CreateAt = item.CreateAt                             
                          })
                          .OrderBy(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return isos;
        }
        public string GetIndexByID(int id)
        {
            var rs = iSODA.GetById(id);
            if (rs != null)
                return rs.Code;
            return "Yêu cầu tiêu chuẩn đã bị xóa";
        }
       
    }
}
