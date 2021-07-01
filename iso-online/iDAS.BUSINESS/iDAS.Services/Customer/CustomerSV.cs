using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System.Data;
using System.IO;

namespace iDAS.Services
{
    public class CustomerSV
    {
        private const int totalCustomerField = 14;
        private CustomerDA CustomerDA = new CustomerDA();
        private iDASBusinessEntities dbo = new CustomerDA().Repository;
        public List<CustomerItem> GetBeginCustomerByGroupID(ModelFilter filter, int GroupID, int employeeID, bool isAdministrator)
        {
            return GetCustomerByGroupID(filter, GroupID, employeeID, isAdministrator, isBegin: true);
        }
        public List<CustomerItem> GetNormalCustomerByGroupID(ModelFilter filter, int GroupID, int employeeID, bool isAdministrator)
        {
            return GetCustomerByGroupID(filter, GroupID, employeeID, isAdministrator);
        }
        public List<CustomerItem> GetPotentialCustomerByGroupID(ModelFilter filter, int GroupID, int employeeID, bool isAdministrator)
        {
            return GetCustomerByGroupID(filter, GroupID, employeeID, isAdministrator, isPotential: true);
        }
        public List<CustomerItem> GetOfficicalCustomerByGroupID(ModelFilter filter, int GroupID, int employeeID, bool isAdministrator)
        {
            return GetCustomerByGroupID(filter, GroupID, employeeID, isAdministrator, isOfficial: true);
        }
        public List<CustomerItem> GetCustomerByGroupID(ModelFilter filter, int groupId, int employeeId, bool isAdmin, bool isPotential = false, bool isOfficial = false, bool isBegin = false)
        {
            IQueryable<Customer> Customer;
            if (isAdmin)
            {
                Customer = GetCustomerByAdmin(filter: filter, groupId: groupId, isPotential: isPotential, isOfficial: isOfficial,isBegin: isBegin);
            }
            else
            {
                if (CheckPermission(groupId: groupId, employeeId: employeeId))
                    Customer = GetCustomerByEmployee(filter: filter, groupId: groupId, isPotential: isPotential, isOfficial: isOfficial, isBegin: isBegin);
                else
                    return null;
            }
            //var customerResult = Customer.Filter(filter).ToList();
            return Customer.Select(item => new CustomerItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                EstablishDate = item.EstablishDate,
                                AttachmentFileID = item.AttachmentFileID,
                                FileName = item.AttachmentFileID == null ? "" : dbo.AttachmentFiles.Where(i => i.ID == item.AttachmentFileID).Select(i => i.Name).FirstOrDefault(),
                                Scope = item.Scope,
                                CustomerTypeID = item.CustomerTypeID,
                                TypeName = dbo.CustomerTypes.Where(i => i.ID == item.CustomerTypeID).Select(i => i.Name).FirstOrDefault(),
                                IsPotential = item.IsPotential,
                                IsOfficial = item.IsOfficial,
                                IsPotentialView = item.IsPotentialView,
                                IsNotContract = item.IsNotContract,
                                RequireContent = item.RequireContent,
                                RequireTime = item.RequireTime,
                                Address = item.Address,
                                LastContact = dbo.CustomerContactHistories.Where(i => i.CustomerID == item.ID).Select(i => i.Time).OrderByDescending(i => i).FirstOrDefault(),
                                IsBackContact = dbo.CustomerContactCalendars.Any(i => i.CustomerID == item.ID && DateTime.Today <= i.Time),
                                IsDelete = item.IsDelete,
                                CreateAt = item.CreateAt,
                            }).Filter(filter).ToList();
        }
        private IQueryable<Customer> GetCustomerByAdmin(ModelFilter filter, int groupId, bool isPotential, bool isOfficial, bool isBegin = false)
        {
            var groupCustomerIds = GetGroupChilds(dbo.CustomerCategories, groupId).Select(i => i.ID);
            var query = dbo.CustomerCategoryCustomers.Where(i => groupCustomerIds.Contains(i.CustomerCategoryID))
                        .Select(i => i.Customer).Distinct()
                        .Where(i => i.IsDelete == false &&
                                ((i.IsOfficial == isOfficial && i.IsPotential == isPotential)
                                || (isPotential && i.IsOfficial && i.IsPotentialView)));
            if (isBegin)
            {
                query = query.Where(i => !i.CustomerContactHistories.Any());
            }
            else
            {
                if (!isPotential && !isOfficial)
                {
                    query = query.Where(i => i.CustomerContactHistories.Any());
                }
            }
            return query.Filter(filter, false);
        }
        private bool CheckPermission(int groupId, int employeeId)
        {
            var depIds = dbo.HumanEmployees.Where(i => i.ID == employeeId)
                               .SelectMany(i => i.HumanOrganizations)
                               .Select(i => i.HumanRole)
                               .Select(i => i.HumanDepartment.ID);
            var groupIdsForEmployee = dbo.CustomerCategoryDepartments.Where(i => depIds.Contains(i.HumanDepartmentID)).Select(i => i.CustomerCategoryID);
            foreach (var grId in groupIdsForEmployee)
            {
                bool permiss = GetGroupChilds(dbo.CustomerCategories, grId).Any(i => i.ID == groupId);
                if (permiss) return true;
            }
            return false;
        }
        private IQueryable<Customer> GetCustomerByEmployee(ModelFilter filter, int groupId, bool isPotential, bool isOfficial, bool isBegin = false)
        {
            var groupIds = GetGroupChilds(dbo.CustomerCategories, groupId).Select(i => i.ID);
            var query = dbo.CustomerCategoryCustomers.Where(i => groupIds.Contains(i.CustomerCategoryID))
                        .Select(i => i.Customer).Distinct()
                        .Where(i => i.IsDelete == false && ((i.IsOfficial == isOfficial && i.IsPotential == isPotential) || (isPotential && i.IsOfficial && i.IsPotentialView)))
                        .Filter(filter, false);
            if (isBegin)
            {
                query = query.Where(i => !i.CustomerContactHistories.Any());
            }
            else
            {
                if (!isPotential && !isOfficial)
                {
                    query = query.Where(i => i.CustomerContactHistories.Any());
                }
            }
            return query.Filter(filter, false);
        }
        private IEnumerable<CustomerCategory> GetGroupChilds(IEnumerable<CustomerCategory> e, int id)
        {
            var customerCategory = e.Where(a => a.ParentID == id);
            var customerCategoryFirst = e.Where(a => a.ID == id);
            customerCategoryFirst.Concat(customerCategory);
            return customerCategoryFirst.Concat(customerCategory.SelectMany(a => GetGroupChilds(e, a.ID)));
        }
        public CustomerItem GetById(int Id)
        {
            var customer = dbo.Customers.Where(i => i.ID == Id)
                                .Select(item => new CustomerItem()
                                {
                                    ID = item.ID,
                                    Name = item.Name,
                                    RepresentName = item.RepresentName,
                                    RepresentRole = item.RepresentRole,
                                    TaxCode = item.TaxCode,
                                    Status = (item.IsPotential == true) ? "IsPotential" : (item.IsOfficial == true ? "IsOfficial" : "IsNew"),
                                    Email = item.Email,
                                    Phone = item.Phone,
                                    EstablishDate = item.EstablishDate,
                                    AttachmentFileID = item.AttachmentFileID,
                                    //FileName = item.AttachmentFileID == null ? "" : item.AttachmentFile.Name,
                                    Scope = item.Scope,
                                    CustomerTypeID = item.CustomerTypeID,
                                    TypeName = dbo.CustomerTypes.FirstOrDefault(i => i.ID == item.CustomerTypeID).Name,
                                    IsPotential = item.IsPotential,
                                    IsOfficial = item.IsOfficial,
                                    RequireContent = item.RequireContent,
                                    RequireTime = item.RequireTime,
                                    Address = item.Address,
                                    Note = item.Note,
                                    SuccessRate = item.SuccessRate,
                                    EditFields = item.EditFields,
                                    IsBackContact = item.CustomerContactCalendars.FirstOrDefault() == null ? false : item.CustomerContactCalendars.FirstOrDefault().IsNew,
                                    IsDelete = item.IsDelete,
                                    CreateAt = item.CreateAt
                                }).First();
            var groupCustomers = dbo.CustomerCategoryCustomers.Where(i => i.CustomerID == Id)
                                        .Select(item => new { ID = item.CustomerCategory.ID, Name = item.CustomerCategory.Name }).ToList();
            if (groupCustomers != null && groupCustomers.Count > 0)
            {
                StringBuilder GroupIds = new StringBuilder();
                StringBuilder GroupName = new StringBuilder();
                for (var i = 0; i < groupCustomers.Count; i++)
                {
                    if (i > 0)
                    {
                        GroupIds.Append(',');
                        GroupName.Append(',');
                    }
                    GroupIds.Append(groupCustomers[i].ID.ToString());
                    GroupName.Append(groupCustomers[i].Name.ToString());
                }
                customer.CustomerCategory = new CustomerCategorySelectItem()
                {
                    GroupIds = GroupIds.ToString(),
                    Name = "Gồm " + groupCustomers.Count + " Nhóm khách hàng( " + GroupName.ToString() + ")"
                };
            }
            customer.EditDataRate = string.IsNullOrEmpty(customer.EditFields) ? 0 : customer.EditFields.Split(',').Length * 100 / totalCustomerField;
            return customer;
        }
        public void Update(CustomerItem item, int userID)
        {
            var customer = dbo.Customers.FirstOrDefault(i => i.ID == item.ID);
            if (customer.IsOfficial || customer.IsPotential)
            {
                var oldEditRate = new List<string>();
                if (!string.IsNullOrEmpty(customer.EditFields))
                {
                    oldEditRate = customer.EditFields.Split(',').ToList();
                }
                var newEditRate = GetEditRate(customer, item, oldEditRate);
                if (newEditRate.Count > 0)
                    customer.EditFields = newEditRate.Aggregate((curent, next) => curent + ',' + next);
            }
            if (item.ImageFile != null)
            {
                if (customer.AttachmentFileID == null)
                {
                    item.ImageFile.ID = Guid.NewGuid();
                    var file = new AttachmentFile()
                    {
                        ID = item.ImageFile.ID,
                        Name = item.ImageFile.Name,
                        Extension = item.ImageFile.Extension,
                        Size = item.ImageFile.Size,
                        Type = item.ImageFile.Type,
                        Data = item.ImageFile.Data,
                        IsDeleted = false,
                        CreateAt = DateTime.Now,
                        CreateBy = userID
                    };
                    //customer.AttachmentFile = file;
                    customer.AttachmentFileID = item.ImageFile.ID;
                }
                else
                {
                    item.ImageFile.ID = (Guid)customer.AttachmentFileID;
                    var file = dbo.AttachmentFiles.Where(i => i.ID == item.ImageFile.ID).FirstOrDefault();
                    if (file != null)
                    {
                        file.ID = item.ImageFile.ID;
                        file.Name = item.ImageFile.Name;
                        file.Extension = item.ImageFile.Extension;
                        file.Size = item.ImageFile.Size;
                        file.Type = item.ImageFile.Type;
                        file.Data = item.ImageFile.Data;
                        file.UpdateAt = DateTime.Now;
                        file.UpdateBy = userID;
                    }
                    else
                    {
                        var imgId = new FileSV().Insert(item.ImageFile, userID);
                        customer.AttachmentFileID = imgId;
                    }
                }
            }
            customer.Name = item.Name;
            customer.RepresentName = item.RepresentName;
            customer.RepresentRole = item.RepresentRole;
            customer.Email = item.Email;
            customer.Phone = item.Phone;
            customer.EstablishDate = item.EstablishDate;
            customer.Scope = item.Scope;
            customer.CustomerTypeID = item.CustomerTypeID;
            customer.RequireContent = item.RequireContent;
            customer.RequireTime = item.RequireTime;
            customer.Address = item.Address;
            customer.Note = item.Note;
            customer.SuccessRate = item.SuccessRate;
            //customer.IsPotential = item.IsPotential;
            //customer.IsOfficial = item.IsOfficial;
            customer.IsDelete = item.IsDelete;
            customer.UpdateAt = DateTime.Now;
            customer.UpdateBy = userID;
            if (!string.IsNullOrWhiteSpace(item.CustomerCategory.GroupIds))
            {
                var oldGroupIds = dbo.CustomerCategoryCustomers.Where(i => i.CustomerID == item.ID).Select(i => i.CustomerCategoryID).ToList();
                var groupIds = item.CustomerCategory.GroupIds.Split(',').Select(i => Convert.ToInt32(i));
                if (oldGroupIds != null && oldGroupIds.Count > 0)
                {
                    var insertIds = groupIds.Where(i => !oldGroupIds.Contains(i));
                    var deleteIds = oldGroupIds.Where(i => !groupIds.Contains(i));
                    foreach (var insertId in insertIds)
                    {
                        customer.CustomerCategoryCustomers.Add(new CustomerCategoryCustomer()
                        {
                            CustomerCategoryID = insertId,
                            CreateAt = DateTime.Now,
                            CreateBy = userID
                        });
                    }
                    dbo.CustomerCategoryCustomers.RemoveRange(
                        dbo.CustomerCategoryCustomers.Where(i => deleteIds.Contains(i.CustomerCategoryID) && i.CustomerID == item.ID));
                }
                else
                {
                    foreach (var groupId in groupIds)
                    {
                        customer.CustomerCategoryCustomers.Add(new CustomerCategoryCustomer()
                        {
                            CustomerCategoryID = groupId,
                            CreateAt = DateTime.Now,
                            CreateBy = userID
                        });
                    }
                }
            }
            else
            {
                dbo.CustomerCategoryCustomers.RemoveRange(dbo.CustomerCategoryCustomers.Where(i => i.CustomerID == item.ID));
            }
            dbo.SaveChanges();
        }
        public int Insert(CustomerItem item, int userID)
        {
            var customer = new Base.Customer()
            {
                Name = item.Name,
                TaxCode = item.TaxCode,
                RepresentName = item.RepresentName,
                RepresentRole = item.RepresentRole,
                Email = item.Email,
                Phone = item.Phone,
                EstablishDate = item.EstablishDate,
                Scope = item.Scope,
                CustomerTypeID = item.CustomerTypeID,
                RequireContent = item.RequireContent,
                RequireTime = item.RequireTime,
                Address = item.Address,
                Note = item.Note,
                IsPotential = item.IsPotential,
                IsOfficial = item.IsOfficial,
                SuccessRate = item.SuccessRate,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            if (customer.IsPotential) customer.TimePotential = DateTime.Now;
            if (customer.IsOfficial) customer.TimeOfficial = DateTime.Now;
            if (!customer.IsPotential && !customer.IsOfficial) customer.TimeNormal = DateTime.Now;
            dbo.Customers.Add(customer);
            if (!string.IsNullOrWhiteSpace(item.CustomerCategory.GroupIds))
            {
                var groupIds = item.CustomerCategory.GroupIds.Split(',').Select(i => Convert.ToInt32(i));
                foreach (var groupId in groupIds)
                {
                    dbo.CustomerCategoryCustomers.Add(new CustomerCategoryCustomer()
                    {
                        CustomerID = customer.ID,
                        CustomerCategoryID = groupId,
                        CreateAt = DateTime.Now,
                        CreateBy = userID
                    });
                }
            }
            if (item.ImageFile != null)
            {
                var imgId = new FileSV().Insert(item.ImageFile, userID);
                customer.AttachmentFileID = imgId;
            }
            dbo.SaveChanges();
            return customer.ID;
        }
        public void Delete(int id)
        {
            var customerItem = dbo.Customers.FirstOrDefault(i => i.ID == id);
            var customerCategoryItems = dbo.CustomerCategoryCustomers.Where(i => i.CustomerID == id);
            dbo.CustomerCategoryCustomers.RemoveRange(customerCategoryItems);
            dbo.CustomerContacts.RemoveRange(dbo.CustomerContacts.Where(i => i.CustomerID == id));
            //dbo.CustomerHistories.RemoveRange(dbo.CustomerHistories.Where(i => i.CustomerID == id));
            dbo.Customers.Remove(customerItem);
            dbo.SaveChanges();
        }
        public List<ComboboxItem> Combobox(int page, int pageSize, out int total, string name)
        {
            var data = dbo.Customers
                        .Where(i => string.IsNullOrEmpty(name) || i.Name.Contains(name))
                        .Select(
                                item => new ComboboxItem()
                                {
                                    ID = item.ID,
                                    Name = item.Name,
                                }
                                )
                        .OrderBy(i => i.Name)
                        .Page(page, pageSize, out total)
                        .ToList();
            return data;
        }
        /// <summary>
        /// Hàm của CuongPC cấm xóa
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ComboboxItem> ComboboxDepartment(int page, int pageSize, out int total, string name)
        {
            var data = dbo.HumanDepartments
                        .Where(i => string.IsNullOrEmpty(name) || i.Name.Contains(name))
                        .Where(i => i.IsActive == true || i.IsCancel || i.IsMerge || i.IsDelete)
                        .Where(i => i.IsCancel == false)
                        .Where(i => i.IsMerge == false)
                        .Where(i => i.IsDelete == false)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(
                                item => new ComboboxItem()
                                {
                                    ID = item.ID,
                                    Name = item.Name,
                                }
                                )
                        .Page(page, pageSize, out total)
                        .ToList();
            return data;
        }
        /// <summary>
        /// Hàm của CuongPC cấm xóa
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public CustomerItem GetDepartmentById(int Id)
        {
            var customer = dbo.HumanDepartments.Where(i => i.ID == Id)
                                .Select(item => new CustomerItem()
                                {
                                    ID = item.ID,
                                    Name = !string.IsNullOrEmpty(item.Name) ? item.Name : string.Empty,
                                    Phone = !string.IsNullOrEmpty(item.Phone) ? item.Phone : string.Empty,
                                    Address = !string.IsNullOrEmpty(item.Address) ? item.Address : string.Empty,
                                    CreateAt = item.CreateAt

                                }).First();
            return customer;
        }
        public void SetAppointment(CustomerContactCalendarItem item, int userID)
        {
            var customerCalendar = dbo.CustomerContactCalendars.FirstOrDefault(i => i.CustomerID == item.CustomerID);
            if (customerCalendar != null)
            {
                customerCalendar.Time = item.Time;
                customerCalendar.CreateBy = userID;
                customerCalendar.CustomerContactFormID = item.CustomerContactFormID;
                customerCalendar.Note = item.Note;
                customerCalendar.Content = item.Content;
                customerCalendar.IsNew = item.IsNew;
                customerCalendar.UpdateAt = DateTime.Now;
                customerCalendar.UpdateBy = userID;
            }
            else
            {
                customerCalendar = new CustomerContactCalendar();
                customerCalendar.CustomerID = item.CustomerID;
                customerCalendar.Time = item.Time;
                customerCalendar.CustomerContactFormID = item.CustomerContactFormID;
                customerCalendar.Note = item.Note;
                customerCalendar.Content = item.Content;
                customerCalendar.IsNew = item.IsNew;
                customerCalendar.CreateAt = DateTime.Now;
                customerCalendar.CreateBy = userID;
                dbo.CustomerContactCalendars.Add(customerCalendar);
            }
            CustomerDA.Save();
        }
        public CustomerContactCalendarItem GetAppointment(int customerId, int employeeId)
        {
            var result = new CustomerContactCalendarItem() { CustomerID = customerId };
            var appointment = dbo.CustomerContactCalendars.Where(i => i.CustomerID == customerId)
               .Select(item => new CustomerContactCalendarItem
               {
                   ID = item.ID,
                   CustomerContactFormName = item.Customer == null ? string.Empty : item.CustomerContactForm.Name,
                   CustomerContactFormID = item.CustomerContactFormID,
                   CustomerID = item.CustomerID,
                   Note = item.Note,
                   Time = item.Time,
                   Content = item.Content,
                   IsNew = item.IsNew,
                   CreateAt = item.CreateAt,
                   AppointmentEmployee = new HumanEmployeeViewItem()
                   {
                       ID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy) != null ? (int)dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployeeID : 0,
                       Name = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.Name,
                       Role = dbo.HumanOrganizations.FirstOrDefault(m => dbo.HumanUsers.FirstOrDefault(u => u.HumanEmployeeID == m.HumanEmployeeID).ID == item.CreateBy).HumanRole.Name,
                       Department = dbo.HumanOrganizations.FirstOrDefault(m => dbo.HumanUsers.FirstOrDefault(u => u.HumanEmployeeID == m.HumanEmployeeID).ID == item.CreateBy).HumanRole.HumanDepartment.Name,
                   },
               }).FirstOrDefault();
            if (appointment != null) result = appointment;
            if (result.AppointmentEmployee == null || result.AppointmentEmployee.ID == 0)
            {
                result.AppointmentEmployee = new iDAS.Items.HumanEmployeeViewItem()
                {
                    ID = employeeId,
                    Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId).Name,
                    Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId) == null ?
                            "" : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.Name,
                    Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId) == null ?
                                "" : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.HumanDepartment.Name
                };
            }
            return result;
        }
        private List<string> GetEditRate(Customer oldData, CustomerItem newData, List<string> editIds)
        {
            if (oldData.Name != newData.Name) { if (!editIds.Contains("Name")) editIds.Add("Name"); }
            if (oldData.RepresentName != newData.RepresentName) { if (!editIds.Contains("RepresentName")) editIds.Add("RepresentName"); }
            if (oldData.RepresentRole != newData.RepresentRole) { if (!editIds.Contains("RepresentRole"))  editIds.Add("RepresentRole"); }
            if (oldData.CustomerTypeID != newData.CustomerTypeID) { if (!editIds.Contains("CustomerTypeID"))  editIds.Add("CustomerTypeID"); }
            if (oldData.TaxCode != newData.TaxCode) { if (!editIds.Contains("TaxCode")) editIds.Add("TaxCode"); }
            if (oldData.EstablishDate != newData.EstablishDate) { if (!editIds.Contains("EstablishDate"))  editIds.Add("EstablishDate"); }
            if (oldData.Phone != newData.Phone) { if (!editIds.Contains("Phone"))  editIds.Add("Phone"); }
            if (oldData.Email != newData.Email) { if (!editIds.Contains("Email"))  editIds.Add("Email"); }
            if (oldData.Address != newData.Address) { if (!editIds.Contains("Address"))  editIds.Add("Address"); }
            if (oldData.Scope != newData.Scope) { if (!editIds.Contains("Scope"))  editIds.Add("Scope"); }
            if (oldData.SuccessRate != newData.SuccessRate) { if (!editIds.Contains("SuccessRate"))  editIds.Add("SuccessRate"); }
            if (oldData.RequireTime != newData.RequireTime) { if (!editIds.Contains("RequireTime"))  editIds.Add("RequireTime"); }
            if (oldData.RequireContent != newData.RequireContent) { if (!editIds.Contains("RequireContent"))  editIds.Add("RequireContent"); }
            if (oldData.Note != newData.Note) { if (!editIds.Contains("Note"))  editIds.Add("Note"); }
            return editIds;
        }
        public CustomerProfileCustomerItem GetCustomerProfile(int id)
        {
            var Profile = new CustomerProfileCustomerItem()
            {
                ID = id
            };
            Profile.InfoContactCount = dbo.CustomerContacts.Where(i => i.CustomerID == id).Count();
            Profile.HistoryContactCount = dbo.CustomerContactHistories.Where(i => i.CustomerID == id).Count();
            Profile.OrderCount = dbo.CustomerOrders.Where(i => i.CustomerID == id).Count();
            Profile.ContractCount = dbo.CustomerContracts.Where(i => i.CustomerID == id).Count();
            return Profile;
        }
        public List<SurveyMailQuestion> GetQuestionSurvey(int id)
        {
            return new CustomerSurveyResultSV().GetQuestionByCustomer(id);
        }
        public void NormalTransfer(int id)
        {
            var customer = dbo.Customers.Where(i => i.ID == id).FirstOrDefault();
            customer.IsPotential = false;
            customer.IsOfficial = false;
            customer.TimeNormal = DateTime.Now;
            dbo.SaveChanges();
        }
        public void PotentialTransfer(int id)
        {
            var customer = dbo.Customers.Where(i => i.ID == id).FirstOrDefault();
            customer.IsPotential = true;
            customer.IsOfficial = false;
            customer.TimePotential = DateTime.Now;
            dbo.SaveChanges();
        }
        public void OfficialTransfer(int id)
        {
            var customer = dbo.Customers.Where(i => i.ID == id).FirstOrDefault();
            customer.IsPotential = false;
            customer.IsOfficial = true;
            customer.IsNotContract = false;
            customer.TimeOfficial = DateTime.Now;
            dbo.SaveChanges();
        }
        public void HasPotential(int id)
        {
            var customer = dbo.Customers.Where(i => i.ID == id).FirstOrDefault();
            customer.IsPotentialView = true;
            dbo.SaveChanges();
        }
        public void Import(List<CustomerItem> customerImport, int groupId, int userID)
        {
            var lstCustomers = new List<Customer>();
            foreach (var item in customerImport)
            {
                var customer = new Customer()
                {
                    Name = item.Name,
                    TaxCode = item.TaxCode,
                    Email = item.Email,
                    Phone = item.Phone,
                    Scope = item.Scope,
                    CustomerTypeID = dbo.CustomerTypes.FirstOrDefault().ID,
                    Address = item.Address,
                    Note = item.Note,
                    IsPotential = false,
                    IsOfficial = false,
                    IsDelete = false,
                    TimeNormal = DateTime.Now,
                    CreateAt = DateTime.Now,
                    CreateBy = userID
                };
                customer.CustomerCategoryCustomers.Add(new CustomerCategoryCustomer()
                {
                    CustomerCategoryID = groupId,
                    CreateAt = DateTime.Now,
                    CreateBy = userID
                });
                lstCustomers.Add(customer);
            }
            dbo.Customers.AddRange(lstCustomers);
            dbo.SaveChanges();
        }
        public DataTable ReadFileExcel(string directory)
        {
            FileStream fStream = new FileStream(directory, FileMode.Open);
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(fStream);
            Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
            DataTable datatable = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.Rows.Count, worksheet.Cells.Columns.Count, true);
            fStream.Close();
            return datatable;
        }

        public CustomerItem GetNotContract(int id)
        {
            return dbo.Customers.Where(i => i.ID == id)
                .Select(
                item => new CustomerItem()
                {
                    ID = item.ID,
                    IsNotContract = item.IsNotContract,
                    ReasonNotContract = item.ReasonNotContract,
                }).FirstOrDefault();
        }

        public void SetNotContract(CustomerItem item, int userId)
        {
            var customer = dbo.Customers.FirstOrDefault(i => i.ID == item.ID);
            customer.IsNotContract = item.IsNotContract;
            customer.ReasonNotContract = item.ReasonNotContract;
            dbo.SaveChanges();

        }
        public bool IsOfficial(int id)
        {
            return dbo.Customers.FirstOrDefault(i => i.ID == id).IsOfficial;
        }
        public bool IsPotential(int id)
        {
            return dbo.Customers.FirstOrDefault(i => i.ID == id).IsPotential;
        }
    }
}
