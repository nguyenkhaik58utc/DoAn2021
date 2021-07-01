using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;

namespace iDAS.Services
{
    public class CustomerCampaignPlanSV
    {
        private CustomerCampaignPlanDA CustomerCampaignPlanDA = new CustomerCampaignPlanDA();
        private QualityPlanSV planSV = new QualityPlanSV();
        public List<QualityPlanItem> GetAll(int page, int pageSize, out int totalCount, int campaignID)
        {
            var dbo = CustomerCampaignPlanDA.Repository;
            var plans = dbo.CustomerCampaignPlans
                .Where(i => i.CustomerCampaignID == campaignID)
                .Join(dbo.QualityPlans , c => c.QualityPlanID, plan => plan.ID, (c, item) => new QualityPlanItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    TargetID = item.QualityTargetID,
                    Department = new HumanDepartmentViewItem()
                    {
                        ID =item.DepartmentID,
                        Name = dbo.HumanDepartments.FirstOrDefault(d=>d.ID == item.DepartmentID).Name
                    },
                    ParentID = item.ParentID,
                    Type = item.Type,
                    Cost = item.Cost,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Content = item.Content,
                    IsEdit = item.IsEdit,
                    IsApproval = item.IsApproval,
                    IsAccept = item.IsAccept,
                    ApprovalAt = item.ApprovalAt,
                    ApprovalBy = item.ApprovalBy,
                    ApprovalNote = item.Note,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt,
                })
                .OrderByDescending(item => item.CreateAt)
                .Page(page, pageSize, out totalCount)
                .ToList();
            return plans;
        }

        public void Insert(QualityPlanItem planItem, int campaignID)
        {
            var planID = planSV.Insert(planItem);
            var plan = new CustomerCampaignPlan()
            {
                QualityPlanID = planID,
                CustomerCampaignID = campaignID,
                CreateBy = planItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            CustomerCampaignPlanDA.Insert(plan);
            CustomerCampaignPlanDA.Save();
        }
    }
}
