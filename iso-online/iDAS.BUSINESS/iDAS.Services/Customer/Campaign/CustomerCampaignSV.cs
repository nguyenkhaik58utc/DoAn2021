using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class CustomerCampaignSV
    {
        private CustomerCampaignDA CustomerCampaignDA = new CustomerCampaignDA();
        /// <summary>
        /// Lấy danh sách chiến dịch
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCampaignItem> GetAll(ModelFilter filter)
        {
            var dbo = CustomerCampaignDA.Repository;
            var CustomerCampaign = CustomerCampaignDA.GetQuery()
                        .Select(item => new CustomerCampaignItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Income = item.Income,
                            Cost = item.Cost,
                            IsEdit = item.IsEdit,
                            IsSuccess = item.IsSuccess,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            ;
            return CustomerCampaign;
        }
        /// <summary>
        /// Lấy chiến dịch theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCampaignItem GetById(int Id)
        {
            var dbo = CustomerCampaignDA.Repository;
            var CustomerCampaign = dbo.CustomerCampaigns.Where(i => i.ID == Id)
                        .Select(item => new CustomerCampaignItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            StartTime = item.StartTime,
                            StatusValue = (item.IsPause == true) ? "IsPause" : (item.IsFinish == true ? "IsFinish" : "IsNew"),
                            EndTime = item.EndTime,
                            Income = item.Income,
                            Cost = item.Cost,
                            IsEdit = item.IsEdit,
                            IsSuccess = item.IsSuccess,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            IsResult = item.IsApproval ? (bool?)(item.IsAccept ? true : false) : null,
                            ApprovalAt = item.ApprovalAt,
                            Approval = new HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name,
                            },
                            ApprovalNote = item.ApprovalNote,
                            CreateAt = item.CreateAt,
                        })
                        .First();
            return CustomerCampaign;
        }
        public void SendApproval(CustomerCampaignItem campItem)
        {
            var camp = CustomerCampaignDA.GetById(campItem.ID);
            if (campItem.IsSendApproval)
            {
                camp.IsEdit = false;
                camp.IsApproval = false;
            }            
            camp.ApprovalBy = campItem.ApprovalBy;
            CustomerCampaignDA.Save();
        }
        public void Approve(CustomerCampaignItem campItem)
        {
            var camp = CustomerCampaignDA.GetById(campItem.ID);
            camp.IsApproval = true;
            camp.IsEdit = campItem.IsEdit;
            camp.ApprovalAt = campItem.ApprovalAt;
            camp.IsAccept = campItem.IsAccept;
            camp.IsPause = campItem.IsPause;
            camp.IsFinish = campItem.IsFinish;
            camp.ApprovalNote = campItem.ApprovalNote;
            CustomerCampaignDA.Save();
        }
        /// <summary>
        /// Cập nhật chiến dịch
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerCampaignItem item, int userID)
        {
            var CustomerCampaign = CustomerCampaignDA.GetById(item.ID);
            CustomerCampaign.Name = item.Name;
            CustomerCampaign.Income = item.Income;
            CustomerCampaign.Cost = item.Cost;
            CustomerCampaign.StartTime = item.StartTime;
            CustomerCampaign.EndTime = item.EndTime;
            CustomerCampaign.IsFinish = item.IsFinish;
            CustomerCampaign.IsPause = item.IsPause;
            CustomerCampaign.Note = item.Note;
            CustomerCampaign.UpdateAt = DateTime.Now;
            CustomerCampaign.UpdateBy = userID;
            CustomerCampaignDA.Save();
        }
        /// <summary>
        /// Thêm mới chiến dịch
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerCampaignItem item, int userID)
        {
            var CustomerCampaign = new CustomerCampaign()
            {
                Name = item.Name,
                Note = item.Note,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Income = item.Income,
                Cost = item.Cost,
                IsFinish = item.IsFinish,
                IsPause = item.IsPause,
                IsEdit = true,
                IsDelete = item.IsDelete,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerCampaignDA.Insert(CustomerCampaign);
            CustomerCampaignDA.Save();
        }
        /// <summary>
        /// Xóa chiến dịch
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerCampaignDA.Delete(id);
            CustomerCampaignDA.Save();
        }
    }
}
