using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class CampaignService
    {
        CampaignDA campaignDA = new CampaignDA();

        public List<SystemUserItem> GetUserApprovals()
        {
            var dbo = campaignDA.SystemContext;
            var users = dbo.SystemUsers
                        .Select(item => new SystemUserItem()
                        {
                            ID = item.ID,
                            FullName = item.FullName,
                        }).ToList();
            return users;
        }

        public List<CampaignItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var campaigns = campaignDA.GetQuery()
                        .Select(item => new CampaignItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            TotalExpense = item.TotalExpense,
                            TotalRevenue = item.TotalRevenue,
                            OrderMaxTime = item.OrderMaxTime,
                            OrderContactBackTime = item.OrderContactBackTime,
                            IsActive = item.IsActive,
                            IsDisabled = item.IsDisabled,
                            ApprovalResult = item.ApprovalResult,
                            ApprovalBy= item.ApprovalBy,
                            Description = item.Description,
                            Status = item.Status,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return campaigns;
        }

        public CampaignItem GetByID(int id)
        {
            var dbo = campaignDA.SystemContext;
            var campaign = campaignDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Select(item => new CampaignItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            TotalExpense = item.TotalExpense,
                            TotalRevenue = item.TotalRevenue,
                            OrderMaxTime = item.OrderMaxTime,
                            OrderContactBackTime = item.OrderContactBackTime,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            Status = item.Status,
                            ApprovalAt = item.AppovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.ApprovalBy).FullName,
                            ApprovalResult = item.ApprovalResult,
                            ApprovalNode = item.ApprovalNode,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            UpdateByName = dbo.SystemUsers.FirstOrDefault(i=>i.ID == item.UpdateBy).FullName,
                        })
                        .FirstOrDefault();
            return campaign;
        }

        public void Insert(CampaignItem item)
        {
            var campaign = new MnCustomerCampaign()
            {
                Name = item.Name,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                TotalExpense = item.TotalExpense,
                TotalRevenue = item.TotalRevenue,
                OrderContactBackTime = item.OrderContactBackTime,
                OrderMaxTime = item.OrderMaxTime,
                IsActive = item.IsActive,
                Description = item.Description,
                ApprovalBy = item.ApprovalBy,
                Status = (int)CampaignItem.CampaignStatus.New,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            campaignDA.Insert(campaign);
            campaignDA.Save();
        }

        public void Update(CampaignItem item)
        {
            var campaign = campaignDA.GetById(item.ID);
            campaign.Name = item.Name;
            campaign.StartTime = item.StartTime;
            campaign.EndTime = item.EndTime;
            campaign.TotalExpense = item.TotalExpense;
            campaign.TotalRevenue = item.TotalRevenue;
            campaign.OrderMaxTime = item.OrderMaxTime;
            campaign.OrderContactBackTime = item.OrderContactBackTime;
            campaign.Status = (int)CampaignItem.CampaignStatus.Update;
            campaign.Description = item.Description;
            campaign.ApprovalBy = item.ApprovalBy;
            campaign.IsDisabled = item.IsDisabled;
            campaign.UpdateAt = DateTime.UtcNow;
            campaign.UpdateBy = item.UpdateBy;
            if (campaign.AppovalAt.HasValue) {
                campaign.Status = (int)CampaignItem.CampaignStatus.Fix;
            }
            if (item.IsDisabled) {
                campaign.AppovalAt = null;
                campaign.ApprovalNode = string.Empty;
                campaign.Status = (int)CampaignItem.CampaignStatus.ApprovalWait;
            }
            campaignDA.Save();
        }

        public void SendApproval(int id) {
            var campaign = campaignDA.GetById(id);
            campaign.IsDisabled = true;
            campaign.AppovalAt = null;
            campaign.ApprovalNode = string.Empty;
            campaign.Status = (int)CampaignItem.CampaignStatus.ApprovalWait;
            campaignDA.Save();
        }

        public void Approval(CampaignItem item)
        {
            var campaign = campaignDA.GetById(item.ID);
            campaign.ApprovalResult = item.ApprovalResult;
            campaign.ApprovalNode = item.ApprovalNode;
            campaign.AppovalAt = DateTime.Now;
            campaign.IsDisabled = campaign.ApprovalResult;
            campaign.Status = item.ApprovalResult ? (int)CampaignItem.CampaignStatus.ApprovalSuccess : (int)CampaignItem.CampaignStatus.ApprovalFail;
            campaignDA.Save();
        }

        public void Active(int id)
        {
            var campaign = campaignDA.GetById(id);
            campaign.IsActive = !campaign.IsActive;
            campaignDA.Save();
        }

        public void DeleteRange(List<object> ids)
        {
            campaignDA.DeleteRange(ids);
            campaignDA.Save();
        }
    }
}
