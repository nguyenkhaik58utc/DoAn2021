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
    public class HumanCriteriaPointSV
    {
        private HumanAuditCriteriaPointDA criteriaDA = new HumanAuditCriteriaPointDA();
        public List<HumanAuditGradationCriteriaPointItem> GetByCateId(ModelFilter filter, int CriteriaId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.HumanAuditCriteriaPoints.Where(t => t.HumanAuditCriteriaID == CriteriaId)
                          .Select(item => new HumanAuditGradationCriteriaPointItem()
                          {
                              ID = item.ID,
                              Point = item.Point,
                              HumanAuditGradationCriteriaID = item.HumanAuditCriteriaID,
                              Note = item.Note,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .OrderByDescending(t => t.Point)
                          .ToList();
            return criterias;
        }
        public bool Insert(HumanAuditGradationCriteriaPointItem obj)
        {
            if (criteriaDA.GetQuery(i => i.Name.Trim().ToUpper() == obj.Name.Trim().ToUpper() && i.HumanAuditCriteriaID == obj.HumanAuditGradationCriteriaID).FirstOrDefault() != null)
            {
                return false;
            }
            var itm = new HumanAuditCriteriaPoint
            {
                Name = obj.Name,
                Note = obj.Note,
                Point = obj.Point,
                HumanAuditCriteriaID = obj.HumanAuditGradationCriteriaID,
                CreateAt = DateTime.Now,
                CreateBy = obj.CreateBy
            };
            criteriaDA.Insert(itm);
            criteriaDA.Save();
            return true;
        }
        public bool Update(HumanAuditGradationCriteriaPointItem obj)
        {
            var itm = criteriaDA.GetById(obj.ID);
            if (itm.Name != obj.Name && criteriaDA.GetQuery(i => i.Name.Trim().ToUpper() == obj.Name && i.HumanAuditCriteriaID == obj.HumanAuditGradationCriteriaID).FirstOrDefault() != null)
            {
                return false;
            }
            itm.Name = obj.Name;
            itm.Note = obj.Note;
            itm.Point = obj.Point;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            criteriaDA.Update(itm);
            criteriaDA.Save();
            return true;
        }
        public void Delete(string strId)
        {
            var ids = strId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            criteriaDA.DeleteRange(ids);
            criteriaDA.Save();
        }
        public HumanAuditGradationCriteriaPointItem GetByID(int id)
        {
            var obj = criteriaDA.GetById(id);
            if (obj != null)
                return ConvertToHumanAuditGradationCriteriaPointItem(obj);
            return null;
        }
        private HumanAuditGradationCriteriaPointItem ConvertToHumanAuditGradationCriteriaPointItem(HumanAuditCriteriaPoint profileSecurity)
        {
            var obj = new HumanAuditGradationCriteriaPointItem()
            {
                ID = profileSecurity.ID,
                Name = profileSecurity.Name,
                Note = profileSecurity.Note,
                Point = profileSecurity.Point,
                CreateAt = profileSecurity.CreateAt,
                CreateBy = profileSecurity.CreateBy,
                UpdateAt = profileSecurity.UpdateAt,
                UpdateBy = profileSecurity.UpdateBy
            };
            return obj;
        }
    }
}
