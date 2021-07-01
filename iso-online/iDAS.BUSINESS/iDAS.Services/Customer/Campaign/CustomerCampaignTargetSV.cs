using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Services;
using iDAS.Items;
namespace iDAS.Services
{
    public class CustomerCampaignTargetSV
    {
        private CustomerCampaignTargetDA CustomerCampaignTargetDA = new CustomerCampaignTargetDA();
        private QualityTargetSV targetSV = new QualityTargetSV();
        public List<QualityTargetItem> GetAll(int page, int pageSize, out int totalCount, int campaignID)
        {
            var dbo = CustomerCampaignTargetDA.Repository;
            List<QualityTargetItem> lst = new List<QualityTargetItem>();
            var targets = dbo.CustomerCampaignTargets
                .Where(i => i.CustomerCampaignID == campaignID)
                .Join(dbo.QualityTargets, c => c.QualityTargetID, target => target.ID, (c, target) => target)
                .OrderByDescending(item => item.CreateAt)
                .Page(page, pageSize, out totalCount)
                .ToList();
            foreach(var target in targets)
            {
                lst.Add(new QualityTargetItem()
                 {
                     ID = target.ID,                     
                     Name = target.Name,                     
                     Value =target.Value,
                     Unit = target.Unit,
                     Description = target.Description,
                     TypeName = iDAS.Utilities.Common.GetTypeName(target.Type),
                     CreateBy = target.CreateBy,
                     IsEnd = target.IsEnd,                 
                     UpdateAt = target.UpdateAt,
                     UpdateBy = target.UpdateBy,
                     CreateAt = target.CreateAt,
                     IsSuccess = target.IsSuccess,
                     ApprovalAt = target.ApprovalAt,
                     ApprovalBy = target.ApprovalBy,
                     DepartmentID = target.DepartmentID,
                     IsAccept = target.IsAccept,
                     IsApproval = target.IsApproval,
                     IsDelete = target.IsDelete,
                     IsEdit = target.IsEdit,
                     ParentID = target.ParentID,
                     Type = target.Type,
                     CompleteAt = target.CompleteAt,
                     ParentName = dbo.QualityTargets.FirstOrDefault(i => i.ID == target.ParentID) != null ? "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == target.ParentID).CompleteAt.ToString() + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == target.ParentID).Name + dbo.QualityTargets.FirstOrDefault(m => m.ID == target.ParentID).Value.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == target.ParentID).Unit : string.Empty
                 });
            }
            return lst;
        }

        public void Insert(QualityTargetItem targetItem, int campaignID)
        {
            var taskID = targetSV.Insert(targetItem);
            var task = new CustomerCampaignTarget()
            {
                QualityTargetID = taskID,
                CustomerCampaignID = campaignID,
                CreateBy = targetItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            CustomerCampaignTargetDA.Insert(task);
            CustomerCampaignTargetDA.Save();
        }
    }
}
