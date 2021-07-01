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
    public class CareService
    {
        CareDA careDA = new CareDA();

        public List<CareItem> GetAll(int campaignId, int employeeId, int page, int pageSize, out int totalCount)
        {
            var dbo = careDA.SystemContext;
            var cares = careDA.GetQuery()
                        .Where(item => item.CampaignID == campaignId && item.EmployeeID == employeeId)
                        .Select(item => new CareItem()
                        {
                            ID = item.ID,
                            CampaignID = item.CampaignID,
                            CampaignName = item.MnCustomerCampaign.Name,
                            CustomerID = item.CustomerID,
                            CustomerName = item.MnCustomerCustomer.Name,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = item.SystemUser.FullName,
                            MethodID = item.MethodID,
                            MethodName = item.MnCustomerMethod.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Values = item.Values,
                            Status = item.Status,
                            IsDisabled = item.IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return cares;
        }

        public List<CareItem> GetAll(int campaignId, int page, int pageSize, out int totalCount)
        {
            var cares = careDA.GetQuery()
                        .Where(item => item.CampaignID == campaignId)
                        .Select(item => new CareItem()
                        {
                            ID = item.ID,
                            CampaignID = item.CampaignID,
                            CampaignName = item.MnCustomerCampaign.Name,
                            CustomerID = item.CustomerID,
                            CustomerName = item.MnCustomerCustomer.Name,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = item.SystemUser.FullName,
                            MethodID = item.MethodID,
                            MethodName = item.MnCustomerMethod.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Values = item.Values,
                            Status = item.Status,
                            IsDisabled = item.IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return cares;
        }

        public CareItem GetByID(int id)
        {
            var dbo = careDA.SystemContext;
            var care = careDA.GetQuery()
                        .Where(item => item.ID == id)
                        .Select(item => new CareItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            CustomerName = item.MnCustomerCustomer.Name,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = item.SystemUser.FullName,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Values = item.Values,
                            Status = item.Status,
                            MethodID = item.MethodID,
                            MethodName = item.MnCustomerMethod.Name,
                            Time = item.Time,
                            IsDisabled = item.IsDisabled,
                            Result = item.Result,
                            Node = item.Node,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.CreateBy).FullName,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            UpdateByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.UpdateBy).FullName,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .FirstOrDefault();
            return care;
        }

        public List<CustomerItem> GetCustomers(int campaignId, int categoryId, bool type, int page, int pageSize, out int totalCount)
        {
            var dbo = careDA.SystemContext;
            var customers = dbo.MnCustomerCustomers
                            .Where(item => item.CategoryID == categoryId && item.Type == type && item.IsActive)
                            .Where(item => item.MnCustomerCares.Count(i => i.CampaignID == campaignId) == 0)
                            .Select(item => new CustomerItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Fax = item.Fax,
                            })
                            .OrderByDescending(item => item.Name)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
            return customers;
        }

        public void Update(CareItem item)
        {
            var care = careDA.GetById(item.ID);
            care.EmployeeID = item.EmployeeID;
            care.StartTime = item.StartTime;
            care.EndTime = item.EndTime;
            care.MethodID = item.MethodID;
            care.Values = item.Values;
            care.IsDisabled = item.IsDisabled;
            care.UpdateAt = DateTime.Now;
            care.UpdateBy = item.UpdateBy;

            careDA.Save();
        }

        public void Care(CareItem item)
        {
            var care = careDA.GetById(item.ID);
            care.Time = item.Time;
            care.Node = item.Node;
            care.IsDisabled = item.IsDisabled;
            care.Result = item.Result;
            care.Status = (byte)CareItem.CareStatus.Care;
            if (item.IsDisabled){
                care.Status = item.Result?(byte)CareItem.CareStatus.Success:(byte)CareItem.CareStatus.Stop;
            }
            care.UpdateAt = DateTime.Now;
            care.UpdateBy = item.UpdateBy;

            careDA.Save();
        }

        public void InsertRange(CareItem item)
        {
            var cares = new List<MnCustomerCare>();
            var customerIDs = item.CustomerIDs.Split(',');
            foreach (var customerId in customerIDs)
            {
                if (!string.IsNullOrEmpty(customerId))
                {
                    item.CustomerID = Convert.ToInt32(customerId);
                    cares.Add(getCare(item));
                }
            }
            careDA.InsertRange(cares);
            careDA.Save();
        }

        public void DeleteRange(List<object> ids)
        {
            careDA.DeleteRange(ids);
            careDA.Save();
        }

        private MnCustomerCare getCare(CareItem item)
        {
            var care = new MnCustomerCare()
            {
                CampaignID = item.CampaignID,
                EmployeeID = item.EmployeeID,
                CustomerID = item.CustomerID,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                MethodID = item.MethodID,
                Values = item.Values,
                Status = (byte)CareItem.CareStatus.Not,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            return care;
        }
    }
}
