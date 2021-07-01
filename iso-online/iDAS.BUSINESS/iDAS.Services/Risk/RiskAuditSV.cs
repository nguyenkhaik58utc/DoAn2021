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
    public class RiskAuditSV
    {
        private RiskAuditDA RiskAuditDA = new RiskAuditDA();
        public void Insert(RiskAuditItem item, int userId)
        {
            var obj = new RiskAudit();
            obj.AuditTime = item.AuditTime;
            obj.RiskControlID = item.RiskControlID;
            obj.IsAccept = item.IsAccept;
            obj.Note = item.Note;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            RiskAuditDA.Insert(obj);
            RiskAuditDA.Save();
        }
        public void Update(RiskAuditItem item, int userId)
        {
            var obj = RiskAuditDA.GetById(item.ID);
            obj.AuditTime = item.AuditTime;
            obj.RiskControlID = item.RiskControlID;
            obj.IsAccept = item.IsAccept;
            obj.Note = item.Note;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            RiskAuditDA.Update(obj);
            RiskAuditDA.Save();
        }
        public RiskAuditItem GetByID(int id)
        {
            return RiskAuditDA.GetQuery(t => t.ID == id).Select(t => new RiskAuditItem
            {
                ID = t.ID,
                Note = t.Note,
                AuditTime = t.AuditTime,
                IsAccept = t.IsAccept,
                RiskControlID = t.RiskControlID,
                QualityNCID = t.QualityNCID,
                CreateAt = t.CreateAt,
                CreateBy = t.CreateBy,
                UpdateAt = t.UpdateAt,
                UpdateBy = t.UpdateBy
            }).FirstOrDefault();

        }
    }
}
