using iDAS.Base;
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
    public class CustomerCampaignAuditSV
    {
        private CustomerCampaignAuditDA CustomerCampaignAuditDA = new CustomerCampaignAuditDA();
        private AuditSV auditServices = new AuditSV();
        public List<AuditItem> GetAll(int page, int pageSize, out int totalCount, int campaignID)
        {
            var dbo = CustomerCampaignAuditDA.Repository;
            var targets = dbo.CustomerCampaignAudits
                .Where(i => i.CustomerCampaignID == campaignID)
                .Join(dbo.Audits, c => c.AuditID, audit => audit.ID, (c, item) => new AuditItem()
                {
                    ID = item.ID,
                    //Name = item.Name,
                    //ApprovalAt = item.ApprovalAt,
                    //Code = item.Code,
                    //CreateBy = item.CreateBy,
                    //EndTime = item.EndTime,
                    //IsAccept = item.IsAccept,
                    //IsApproval = item.IsApproval,
                    //IsEdit = item.IsEdit,
                    //IsNew = item.IsNew,
                    //Note = item.Note,
                    //Require = item.Require,
                    //Scope = item.Scope,
                    //StartTime = item.StartTime,
                    //Type = item.Type,
                    //UpdateAt = item.UpdateAt,
                    //UpdateBy = item.UpdateBy,
                    //CreateAt = item.CreateAt
                })
                .OrderByDescending(item => item.ID)
                .Page(page, pageSize, out totalCount)
                .ToList();
            return targets;
        }

        public void Insert(AuditItem auditItem, int campaignID)
        {
            var auditId = auditServices.Insert(auditItem);
            var task = new CustomerCampaignAudit()
            {
                AuditID = auditId,
                CustomerCampaignID = campaignID,
                //CreateBy = auditItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            CustomerCampaignAuditDA.Insert(task);
            CustomerCampaignAuditDA.Save();
        }
    }
}
