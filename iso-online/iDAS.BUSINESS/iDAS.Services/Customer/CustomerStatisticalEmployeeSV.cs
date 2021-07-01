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
    public class CustomerStatisticalEmployeeSV
    {
        private CustomerCategoryDA customerCategoryDA = new CustomerCategoryDA();
        public List<HumanDepartmentItem> GetDepartmentCustomer(int id)
        {
            var dbo = customerCategoryDA.Repository;
            var departmentNodes = new List<HumanDepartment>();
            var departmentOfCustomers = dbo.CustomerCategoryDepartments.Select(i => i.HumanDepartment).Where(i => i.IsActive && !i.IsCancel && !i.IsDelete).DefaultIfEmpty();
            var departmentTree = dbo.HumanDepartments.Where(i => i.IsActive && !i.IsCancel && !i.IsDelete);
            if (id == 0)
            {
                departmentNodes = departmentTree.Where(i => departmentTree.Select(d => d.ID).Contains(i.ParentID) == false).ToList();
            }
            else
            {
                departmentNodes = departmentTree.Where(i => i.ParentID == id).ToList();
            }
            var data = departmentNodes
                        .OrderBy(i => i.Position)
                        .Select(item => new HumanDepartmentItem()
                        {
                            ID = item.ID,
                            ParentID = item.ParentID,
                            Name = item.Name,
                            IsParent = dbo.HumanDepartments.Where(d => d.ParentID == item.ID)
                                            .Where(i => i.IsActive == true && !i.IsCancel && !i.IsDelete).Count() > 0,
                            IsActive = departmentOfCustomers.Where(i => i.ID == item.ID).FirstOrDefault() != null
                        }).ToList();
            return data;
        }
        public List<CustomerStatisticalItem> GetEmployees(int page, int pageSize, out int totalCount, int departmentId, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var employees = dbo.HumanRoles.Where(i => i.HumanDepartmentID == departmentId)
                             .SelectMany(i => i.HumanOrganizations)
                             .Select(i => i.HumanEmployee).Distinct()
                             .Select(i => new
                             {
                                 employeeId = i.ID,
                                 employeeName = i.Name,
                                 userId = i.HumanUsers.Select(u => u.ID).FirstOrDefault()
                             })
                             .OrderByDescending(item => item.employeeName)
                             .Page(page, pageSize, out totalCount)
                             .Select(item => new CustomerStatisticalItem()
                             {
                                 EmployeeID = item.employeeId,
                                 UserID = item.userId,
                                 EmployeeName = item.employeeName,
                                 CustomerNormal = dbo.Customers
                                        .Count(i => i.TimeNormal <= endTime && i.TimeNormal >= startTime && i.CreateBy == item.userId),
                                 CustomerNormalContacts = dbo.Customers
                                        .Count(c => !c.IsPotential && !c.IsOfficial && c.CustomerContactHistories.Where(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential && i.CreateBy == item.userId).FirstOrDefault() != null),
                                 CustomerNormalNeed = dbo.Customers
                                        .Count(c => !c.IsPotential && !c.IsOfficial && c.CustomerContactHistories.Where(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential && i.IsSuccess && i.CreateBy == item.userId).FirstOrDefault() != null),
                                 //CustomerPotentialSendPrice = dbo.Customers
                                 //            .Count(c => c.IsPotential && c.CustomerOrders.Any(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsPrice
                                 //                           && i.CreateBy == item.userId)),
                                 CustomerPotentialSendPrice = dbo.Customers
                                             .Count(c => c.IsPotential && c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime 
                                                            && i.CreateBy == item.userId)),
                                 CustomerPotentialSignContract = dbo.Customers
                                             .Count(c => c.IsPotential && c.CustomerContracts.Any(i => i.CreateAt <= endTime && i.CreateAt >= startTime
                                                             && i.CreateBy == item.userId)
                                                ),
                                 CustomerOfficialContacts = dbo.Customers
                                            .Count(c => c.IsOfficial &&
                                                      c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential
                                                          && i.CreateBy == item.userId)
                                            ),
                                 CustomerOfficialContract = dbo.Customers
                                            .Count(c => c.IsOfficial &&
                                                      c.CustomerContracts.Any(i => i.CreateAt <= endTime && i.CreateAt >= startTime
                                                        && i.CreateBy == item.userId)
                                             ),
                                 NumberContract = dbo.CustomerContracts
                                              .Count(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.CreateBy == item.userId),
                                 TotalContract = dbo.CustomerContracts
                                              .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.CreateBy == item.userId).DefaultIfEmpty()
                                              .Sum(i => i.Total.HasValue ? i.Total.Value : 0),
                                 NumberContractSucess = dbo.CustomerContracts
                                             .Count(i => i.IsFinish && i.FinishDate <= endTime && i.FinishDate >= startTime && i.CreateBy == item.userId),
                                 TotalContractSucess = dbo.CustomerContracts
                                             .Where(i => i.IsFinish && i.FinishDate <= endTime && i.FinishDate >= startTime && i.CreateBy == item.userId)
                                             .DefaultIfEmpty().Sum(i => i.Total.HasValue ? i.Total.Value : 0)
                             })
                             .ToList();
            return employees;
        }
        public List<CustomerStatisticalItem> GetEmployeeChart(int id, int year)
        {
            var dbo = customerCategoryDA.Repository;
            List<CustomerStatisticalItem> lst = new List<CustomerStatisticalItem>();
            var userId = dbo.HumanUsers.Where(u => u.HumanEmployeeID == id).Select(u => u.ID).FirstOrDefault();
            for (int i = 1; i <= 12; i++)
            {
                var employeeStatis = new CustomerStatisticalItem();
                employeeStatis.TimeFix = "Tháng " + i;
                employeeStatis.CustomerNormal = dbo.Customers
                                            .Where(c => c.TimeNormal.HasValue && c.TimeNormal.Value.Month == i && c.CreateBy == userId).Count();
                employeeStatis.CustomerNormalNeed = dbo.Customers
                                              .Where(c =>
                                                        (c.TimeNormal.HasValue && c.TimeNormal.Value.Month == i && c.CreateBy == userId) &&
                                                             c.CustomerContactHistories.Where(h => h.Time.Month == i && !h.IsOfficial && !h.IsPotential && h.IsSuccess).FirstOrDefault() != null
                                            )
                                            .Where(c => c.CreateAt.HasValue && c.CreateAt.Value.Month == i && c.CreateBy == userId).Count();
                employeeStatis.NumberContract = dbo.CustomerContracts
                                            .Where(c => c.CreateAt.HasValue && c.CreateAt.Value.Month == i && c.CreateBy == userId).Count();
                employeeStatis.NumberContractSucess = dbo.CustomerContracts
                                            .Where(c => c.IsFinish && c.FinishDate.Month == i && c.CreateBy == userId).Count();
                lst.Add(employeeStatis);
            }
            return lst;
        }

        #region Thống kê cho khách hàng tiếp cận
        public List<CustomerItem> CustomerNormal(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = dbo.Customers.Where(i => i.TimeNormal <= endTime && i.TimeNormal >= startTime && i.CreateBy == id).Select(item => new CustomerItem()
            {
                ID = item.ID,
                Name = item.Name,
                Email = item.Email,
                Phone = item.Phone,
                EstablishDate = item.EstablishDate,
                AttachmentFileID = item.AttachmentFileID,
                //FileName = item.AttachmentFileID == null ? "" : item.AttachmentFile.Name,
                Scope = item.Scope,
                CustomerTypeID = item.CustomerTypeID,
                TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                IsPotential = item.IsPotential,
                IsOfficial = item.IsOfficial,
                RequireContent = item.RequireContent,
                RequireTime = item.RequireTime,
                Address = item.Address,
                LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                IsBackContact = dbo.CustomerContactCalendars.Where(i => i.CustomerID == item.ID && DateTime.Today <= i.Time).FirstOrDefault() != null,
                IsDelete = item.IsDelete,
                CreateAt = item.CreateAt,
            })
                                   .Filter(filter)
                                   .ToList();
            return result;
        }
        // Khách hàng đã liên hệ
        public List<CustomerItem> CustomerNormalContacts(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = dbo.Customers
                        .Where(c =>
                            (!c.IsPotential && !c.IsOfficial) &&
                                    c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential && i.CreateBy == id)
                                ).Select(item => new CustomerItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone,
                    EstablishDate = item.EstablishDate,
                    AttachmentFileID = item.AttachmentFileID,
                    Scope = item.Scope,
                    CustomerTypeID = item.CustomerTypeID,
                    TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                    IsPotential = item.IsPotential,
                    IsOfficial = item.IsOfficial,
                    RequireContent = item.RequireContent,
                    RequireTime = item.RequireTime,
                    Address = item.Address,
                    LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                    IsBackContact = dbo.CustomerContactCalendars.Where(i => i.CustomerID == item.ID && DateTime.Today <= i.Time).FirstOrDefault() != null,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt,
                })
            .Filter(filter)
            .ToList();
            return result;
        }
        public List<CustomerItem> CustomerNormalNeed(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = dbo.Customers
                    .Where(c => (!c.IsPotential && !c.IsOfficial) &&
                    c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential && i.IsSuccess && i.CreateBy == id)
                    )
                .Select(item => new CustomerItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone,
                    EstablishDate = item.EstablishDate,
                    AttachmentFileID = item.AttachmentFileID,
                    Scope = item.Scope,
                    CustomerTypeID = item.CustomerTypeID,
                    TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                    IsPotential = item.IsPotential,
                    IsOfficial = item.IsOfficial,
                    RequireContent = item.RequireContent,
                    RequireTime = item.RequireTime,
                    Address = item.Address,
                    LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                    IsBackContact = dbo.CustomerContactCalendars.Where(i => i.CustomerID == item.ID && DateTime.Today <= i.Time).FirstOrDefault() != null,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt,
                })
            .Filter(filter)
            .ToList();
            return result;
        }
        #endregion

        #region Thống kê cho khách hàng tiềm năng
        // Khách hàng tiềm năng đã báo giá
        public List<CustomerItem> CustomerPotentialSendPrice(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = dbo.Customers
                .Where(c => c.IsPotential && c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime 
                                                        && i.CreateBy == id))
                .Select(item => new CustomerItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone,
                    EstablishDate = item.EstablishDate,
                    AttachmentFileID = item.AttachmentFileID,
                    Scope = item.Scope,
                    CustomerTypeID = item.CustomerTypeID,
                    TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                    IsPotential = item.IsPotential,
                    IsOfficial = item.IsOfficial,
                    RequireContent = item.RequireContent,
                    RequireTime = item.RequireTime,
                    Address = item.Address,
                    LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                    IsBackContact = dbo.CustomerContactCalendars.Where(i => i.CustomerID == item.ID && DateTime.Today <= i.Time).FirstOrDefault() != null,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt,
                })
            .Filter(filter)
            .ToList();
            return result;
        }
        // Khách hàng tiềm năng có hợp đồng
        public List<CustomerItem> CustomerPotentialSignContract(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = dbo.Customers
                        .Where(c => c.IsPotential && c.CustomerContracts.Any(i => i.CreateAt <= endTime && i.CreateAt >= startTime
                                        && i.CreateBy == id)
                        )
                .Select(item => new CustomerItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone,
                    EstablishDate = item.EstablishDate,
                    AttachmentFileID = item.AttachmentFileID,
                    Scope = item.Scope,
                    CustomerTypeID = item.CustomerTypeID,
                    TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                    IsPotential = item.IsPotential,
                    IsOfficial = item.IsOfficial,
                    RequireContent = item.RequireContent,
                    RequireTime = item.RequireTime,
                    Address = item.Address,
                    LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                    IsBackContact = dbo.CustomerContactCalendars.Where(i => i.CustomerID == item.ID && DateTime.Today <= i.Time).FirstOrDefault() != null,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt,
                })
            .Filter(filter)
            .ToList();
            return result;
        }
        #endregion

        #region Thống kê cho khách hàng thực tế
        public List<CustomerItem> CustomerOfficialContact(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = dbo.Customers
                .Where(c => c.IsOfficial &&
                            c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential
                                && i.CreateBy == id)
                        )
                .Select(item => new CustomerItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone,
                    EstablishDate = item.EstablishDate,
                    AttachmentFileID = item.AttachmentFileID,
                    Scope = item.Scope,
                    CustomerTypeID = item.CustomerTypeID,
                    TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                    IsPotential = item.IsPotential,
                    IsOfficial = item.IsOfficial,
                    RequireContent = item.RequireContent,
                    RequireTime = item.RequireTime,
                    Address = item.Address,
                    LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                    IsBackContact = dbo.CustomerContactCalendars.Where(i => i.CustomerID == item.ID && DateTime.Today <= i.Time).FirstOrDefault() != null,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt,
                })
            .Filter(filter)
            .ToList();
            return result;
        }
        public List<CustomerItem> CustomerOfficialContract(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = dbo.Customers
                            .Where(c => c.IsOfficial &&
                                        c.CustomerContracts.Any(i => i.CreateAt <= endTime && i.CreateAt >= startTime
                                        && i.CreateBy == id)
                                )
                .Select(item => new CustomerItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone,
                    EstablishDate = item.EstablishDate,
                    AttachmentFileID = item.AttachmentFileID,
                    Scope = item.Scope,
                    CustomerTypeID = item.CustomerTypeID,
                    TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                    IsPotential = item.IsPotential,
                    IsOfficial = item.IsOfficial,
                    RequireContent = item.RequireContent,
                    RequireTime = item.RequireTime,
                    Address = item.Address,
                    LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                    IsBackContact = dbo.CustomerContactCalendars.Where(i => i.CustomerID == item.ID && DateTime.Today <= i.Time).FirstOrDefault() != null,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt,
                })
            .Filter(filter)
            .ToList();
            return result;
        }
        #endregion

        #region Hợp đồng
        public List<CustomerContractItem> NumberContract(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var result = new List<CustomerItem>();
            var CustomerContract = dbo.CustomerContracts
                                        .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.CreateBy == id)
                                        .OrderByDescending(item => item.CreateAt)
                                        .Filter(filter, false)
                                        .ToList();
            List<CustomerContractItem> lst = new List<CustomerContractItem>();
            if (CustomerContract.Count > 0)
            {
                foreach (var item in CustomerContract)
                {
                    lst.Add(new CustomerContractItem()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        CustomerID = item.CustomerID,
                        CustomerName = item.Customer.Name,
                        Total = item.Total,
                        Name = item.Name,
                        FinishDate = item.FinishDate,
                        IsFinish = item.IsFinish,
                        IsPause = item.IsPause,
                        IsSuccess = item.IsSuccess,
                        IsSend = item.IsSend,
                        IsSignReview = item.IsSignReview,
                        IsSignCompany = item.IsSignCompany,
                        IsSignCustomer = item.IsSignCustomer,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        RateFinish = item.IsFinish ? 100 : GetRateFinishContract(item.ID),
                        IsCancel = item.IsCancel,
                        IsEdit = item.IsEdit,
                        IsStart = item.IsStart,
                        CreateAt = item.CreateAt,
                    });
                }
            }
            lst = lst.Filter(filter).ToList();
            return lst;
        }
        public List<CustomerContractItem> NumberContractSucess(ModelFilter filter, int id, DateTime startTime, DateTime endTime)
        {
            var dbo = customerCategoryDA.Repository;
            var CustomerContract = dbo.CustomerContracts
                                        .Where(i => i.IsFinish && i.FinishDate <= endTime && i.FinishDate >= startTime && i.CreateBy == id)
                                        .OrderByDescending(item => item.CreateAt)
                                        .Filter(filter, false)
                                        .ToList();
            List<CustomerContractItem> lst = new List<CustomerContractItem>();
            if (CustomerContract.Count > 0)
            {
                foreach (var item in CustomerContract)
                {
                    lst.Add(new CustomerContractItem()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        CustomerID = item.CustomerID,
                        CustomerName = item.Customer.Name,
                        Total = item.Total,
                        Name = item.Name,
                        FinishDate = item.FinishDate,
                        IsFinish = item.IsFinish,
                        IsPause = item.IsPause,
                        IsSuccess = item.IsSuccess,
                        IsSend = item.IsSend,
                        IsSignReview = item.IsSignReview,
                        IsSignCompany = item.IsSignCompany,
                        IsSignCustomer = item.IsSignCustomer,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        RateFinish = item.IsFinish ? 100 : GetRateFinishContract(item.ID),
                        IsCancel = item.IsCancel,
                        IsEdit = item.IsEdit,
                        IsStart = item.IsStart,
                        CreateAt = item.CreateAt,
                    });
                }
            }
            lst = lst.Filter(filter).ToList();
            return lst;
        }
        private decimal GetRateFinishContract(int contractId)
        {
            var dbo = customerCategoryDA.Repository;
            var s = dbo.ServiceCommandContracts.Where(t => t.CustomerContractID == contractId).Select(t => t.ID).ToArray();
            var q = dbo.ServicePlans.Where(t => s.Contains(t.ServiceCommandContractID.Value) && t.QualityPlan.ParentID != 0).Select(t => t.QualityPlanID).ToArray();
            decimal rate = 0;
            foreach (var item in q)
            {
                var lstTaskID = dbo.QualityPlanTasks.Where(x => x.QualityPlanID == item).Select(x => x.TaskID).ToArray();
                if (dbo.Tasks.Where(x => lstTaskID.Contains(x.ID) && x.IsComplete).Count() != 0)
                {
                    rate += (System.Math.Round((decimal)dbo.Tasks.Where(x => lstTaskID.Contains(x.ID) && x.IsComplete).Count() / (decimal)dbo.Tasks.Where(x => lstTaskID.Contains(x.ID) && !x.IsPause && !x.IsNew).Count(), 2)) * 100;

                }
            }
            return rate != 0 ? System.Math.Round(rate / (decimal)q.Count(), 2) : 0;
        }
        #endregion


    }
}
