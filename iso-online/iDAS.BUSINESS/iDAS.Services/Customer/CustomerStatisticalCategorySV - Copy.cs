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
    public class CustomerStatisticalCategorySV
    {
        private CustomerCategoryDA customerCategoryDA = new CustomerCategoryDA();
        private iDASBusinessEntities dbo = new CustomerCategoryDA().Repository;
        private IEnumerable<CustomerCategory> getCateChilds(IEnumerable<CustomerCategory> e, int id)
        {
            var customerCategory = e.Where(a => a.ParentID == id);
            var customerCategoryFirst = e.Where(a => a.ID == id);
            customerCategoryFirst.Concat(customerCategory);
            return customerCategoryFirst.Concat(customerCategory.SelectMany(a => getCateChilds(e, a.ID)));
        }
        private IEnumerable<HumanDepartment> getDepartmentChilds(IEnumerable<HumanDepartment> e, int id)
        {
            var customerCategory = e.Where(a => a.ParentID == id);
            var customerCategoryFirst = e.Where(a => a.ID == id);
            customerCategoryFirst.Concat(customerCategory);
            return customerCategoryFirst.Concat(customerCategory.SelectMany(a => getDepartmentChilds(e, a.ID)));
        }

        #region Thống kê khách hàng
        // Thống kê khách hàng
        public List<CustomerStatisticalCategoryItem> GetCustomerCategorys(int id, DateTime startTime, DateTime endTime)
        {
            var categories = dbo.CustomerCategories
                        .Where(i => (i.ParentID != null && i.ParentID == id))
                        .Select(item => new CustomerStatisticalCategoryItem()
                        {
                            CategoryID = item.ID,
                            ParentID = item.ParentID,
                            CategoryName = item.Name,
                            Leaf = !dbo.CustomerCategories.Any(i => i.ParentID == item.ID)
                        });
            var results = new List<CustomerStatisticalCategoryItem>();
            foreach (var category in categories)
            {
                var cateIds = getCateChilds(dbo.CustomerCategories, category.CategoryID).Select(i => i.ID);
                var newCategory = dbo.Customers.Where(c => c.CustomerCategoryCustomers.Select(i => i.CustomerCategoryID).Any(i => cateIds.Contains(i))
                            && c.CreateAt <= endTime && c.CreateAt >= startTime && !c.IsDelete)
                            .GroupBy(g => 1)
                    .Select(item => new CustomerStatisticalCategoryItem
                    {
                        CategoryID = category.CategoryID,
                        ParentID = category.ParentID,
                        CategoryName = category.CategoryName,
                        Leaf = category.Leaf,
                        // Khách hàng tiếp cận hiện có 
                        CustomerNormal = item.Count(c => c.TimeNormal <= endTime && c.TimeNormal >= startTime && !c.IsOfficial && !c.IsPotential),
                        // Khách hàng liên hệ
                        CustomerNormalContacts = item.Count(c =>
                                      (c.TimeNormal <= endTime && c.TimeNormal >= startTime && !c.IsOfficial && !c.IsPotential) &&
                                     c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential)
                                ),
                        //// Khách hàng có nhu cầu
                        //CustomerNormalNeed = item.Count(c =>
                        //                    (c.TimeNormal <= endTime && c.TimeNormal >= startTime && !c.IsOfficial && !c.IsPotential) &&
                        //                    c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential && i.IsSuccess)
                        //            ),
                        //// Khách hàng tiềm năng hiện có
                        //CustomerPotential = item
                        //        .Count(c => c.TimePotential <= endTime && c.TimePotential >= startTime && c.IsPotential),
                        //// Khách hàng tiềm năng đã báo giá
                        //CustomerPotentialSendPrice = item.Count(c =>
                        //            (c.TimePotential <= endTime && c.TimePotential >= startTime && c.IsPotential) &&
                        //            c.CustomerOrders.Any(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsPrice)
                        //        ),
                        //// Khách hàng tiềm năng có hợp đồng
                        //CustomerPotentialSignContract = item.Count(c =>
                        //            (c.TimePotential <= endTime && c.TimePotential >= startTime && c.IsPotential) &&
                        //            c.CustomerContracts.Any(i => i.CreateAt <= endTime && i.CreateAt >= startTime)
                        //        ),
                        //// Khách hàng thực tế hiện có
                        //CustomerOfficial = item.Count(c => c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial),
                        //// Khách hàng thực tế đã liên hệ
                        //CustomerOfficialContacts = item.Count(c =>
                        //            (c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial) &&
                        //                c.CustomerContactHistories.Any(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential)
                        //        ),
                        //// Khách hàng thực tế có tiềm năng
                        //CustomerOfficialHasPotential = item.Count(c =>
                        //                            (c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial) && c.IsPotentialView
                        //                        ),
                        //// Khách hàng thực tế có hợp đồng
                        //CustomerOfficialContract = item.Count(c =>
                        //                            (c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial) &&
                        //                            c.CustomerContracts.Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime).FirstOrDefault() != null
                        //                        ),
                        //NumberContract = dbo.CustomerContracts.Count(i => i.CreateAt <= endTime && i.CreateAt >= startTime
                        //                                            && item.Select(ci => ci.ID).Contains(i.CustomerID)),
                        //TotalContract = dbo.CustomerContracts.Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime
                        //                                            && item.Select(ci => ci.ID).Contains(i.CustomerID)).DefaultIfEmpty()
                        //                                            .Sum(i => i.Total.HasValue ? i.Total.Value : 0),
                        //NumberContractSucess = dbo.CustomerContracts.Count(i => i.IsFinish && i.FinishDate <= endTime && i.FinishDate >= startTime
                        //                                            && item.Select(ci => ci.ID).Contains(i.CustomerID)),
                        //TotalContractSucess = dbo.CustomerContracts.Where(i => i.IsFinish && i.FinishDate <= endTime && i.FinishDate >= startTime
                        //                                           && item.Select(ci => ci.ID).Contains(i.CustomerID)).DefaultIfEmpty()
                        //                                           .Sum(i => i.Total.HasValue ? i.Total.Value : 0)
                        
                    });
                results.AddRange(newCategory);
            }
            return results;
        }

        public List<CustomerStatisticalCategoryItem> GetCategoryChart(int id, int year)
        {
            var dbo = customerCategoryDA.Repository;
            var customers = getCateChilds(dbo.CustomerCategories, id).SelectMany(i => i.CustomerCategoryCustomers)
                             .Select(i => i.Customer).Distinct()
                             .Where(i => !i.IsDelete).ToList();
            List<CustomerStatisticalCategoryItem> lst = new List<CustomerStatisticalCategoryItem>();
            for (int i = 1; i <= 12; i++)
            {
                lst.Add(new CustomerStatisticalCategoryItem
                {
                    TimeFix = "Tháng " + i,
                    SumCustomer = customers.Where(c => c.CreateAt.HasValue && c.CreateAt.Value.Month == i && c.CreateAt.Value.Year == year).Count(),
                    CustomerNormal = customers.Where(c => c.TimeNormal.HasValue && c.TimeNormal.Value.Month == i && c.TimeNormal.Value.Year == year).Count(),
                    CustomerPotential = customers.Where(c => c.TimePotential.HasValue && c.TimePotential.Value.Month == i && c.TimePotential.Value.Year == year).Count(),
                    CustomerOfficial = customers.Where(c => c.TimeOfficial.HasValue && c.TimeOfficial.Value.Month == i && c.TimeOfficial.Value.Year == year).Count(),
                });
            }
            return lst;
        }
        #region Thống kê cho khách hàng tiếp cận
        public List<CustomerItem> CustomerNormal(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(c => c.TimeNormal <= endTime && c.TimeNormal >= startTime && !c.IsOfficial && !c.IsPotential)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer.Select(item => new CustomerItem()
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
        public List<CustomerItem> CustomerNormalContacts(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer.Where(c =>
                                     (c.TimeNormal <= endTime && c.TimeNormal >= startTime && !c.IsOfficial && !c.IsPotential) &&
                                     c.CustomerContactHistories.Where(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential).FirstOrDefault() != null
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
        public List<CustomerItem> CustomerNormalNeed(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer.Where(c =>
                                (c.TimeNormal <= endTime && c.TimeNormal >= startTime && !c.IsOfficial && !c.IsPotential) &&
                                     c.CustomerContactHistories.Where(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential && i.IsSuccess).FirstOrDefault() != null
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
        public List<CustomerItem> CustomerPotential(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(c => c.TimePotential <= endTime && c.TimePotential >= startTime && c.IsPotential)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer.Select(item => new CustomerItem()
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
        // Khách hàng tiềm năng đã báo giá
        public List<CustomerItem> CustomerPotentialSendPrice(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer
                .Where(c =>
                           (c.TimePotential <= endTime && c.TimePotential >= startTime && c.IsPotential) &&
                                   c.CustomerOrders.Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsPrice).FirstOrDefault() != null
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
        // Khách hàng tiềm năng có hợp đồng
        public List<CustomerItem> CustomerPotentialSignContract(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer
                .Where(c =>
                     (c.TimePotential <= endTime && c.TimePotential >= startTime && c.IsPotential) &&
                                   c.CustomerContracts.Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime).FirstOrDefault() != null
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
        public List<CustomerItem> CustomerOfficial(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                        .Where(c => c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer.Select(item => new CustomerItem()
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
        public List<CustomerItem> CustomerOfficialContact(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer
                .Where(c =>
                   (c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial) &&
                                     c.CustomerContactHistories.Where(i => i.Time <= endTime && i.Time >= startTime && !i.IsOfficial && !i.IsPotential).FirstOrDefault() != null
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
        public List<CustomerItem> CustomerOfficialHasPotential(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer
                .Where(c =>
                      (c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial) && c.IsPotentialView
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
        public List<CustomerItem> CustomerOfficialContract(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            result = customer
                .Where(c =>
                     (c.TimeOfficial <= endTime && c.TimeOfficial >= startTime && c.IsOfficial) &&
                                   c.CustomerContracts.Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime).FirstOrDefault() != null
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
        public List<CustomerContractItem> NumberContract(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            var customerIds = customer.Select(i => i.ID);
            var CustomerContract = dbo.CustomerContracts.Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime
                                                                    && customerIds.Contains(i.CustomerID))
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
        public List<CustomerContractItem> NumberContractSucess(ModelFilter filter, int categoryId, DateTime startTime, DateTime endTime)
        {
            var result = new List<CustomerItem>();
            var groupCustomerIds = getCateChilds(dbo.CustomerCategories, categoryId).Select(i => i.ID);
            var customer = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                                      .Select(i => i.Customer).Distinct()
                                      .Where(i => i.CreateAt <= endTime && i.CreateAt >= startTime && i.IsDelete == false)
                                      .Filter(filter, false)
                                      .ToList();
            var customerIds = customer.Select(i => i.ID);
            var CustomerContract = dbo.CustomerContracts.Where(i => i.IsFinish && i.FinishDate <= endTime && i.FinishDate >= startTime
                                                                    && customerIds.Contains(i.CustomerID))
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

        #endregion
    }
}
