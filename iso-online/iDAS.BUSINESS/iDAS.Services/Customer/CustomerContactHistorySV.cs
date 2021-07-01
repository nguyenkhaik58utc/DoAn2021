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
    public class CustomerContactHistorySV
    {
        private CustomerContactHistoryDA CustomerContactHistoryDA = new CustomerContactHistoryDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 30/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerContactHistoryItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerContactHistoryDA.Repository;
            var CustomerContract = CustomerContactHistoryDA.GetQuery()
                        .Select(item => new CustomerContactHistoryItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Employee = new HumanEmployeeViewItem()
                            {
                                ID = item.EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.EmployeeID).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.EmployeeID).HumanRole.HumanDepartment.Name
                            },
                            Time = item.Time,
                            Content = item.Content,
                            FormID = item.CustomerContactFormID,
                            Requirment = item.Requirment,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerContract;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<CustomerContactHistoryItem> GetByCustomer(int page, int pageSize, out int totalCount, int CustomerID)
        {
            var dbo = CustomerContactHistoryDA.Repository;
            var contact = CustomerContactHistoryDA.GetQuery(i => i.CustomerID == CustomerID)
                        .Select(item => new CustomerContactHistoryItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Employee = new HumanEmployeeViewItem()
                            {
                                ID = item.EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.EmployeeID).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.EmployeeID).HumanRole.HumanDepartment.Name
                            },
                            Time = item.Time,
                            Content = item.Content,
                            FormID = item.CustomerContactFormID,
                            FormName = item.CustomerContactForm.Name,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return contact;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 30/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerContactHistoryItem GetById(int Id)
        {
            var dbo = CustomerContactHistoryDA.Repository;
            var result = CustomerContactHistoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerContactHistoryItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Employee = new HumanEmployeeViewItem()
                            {
                                ID = item.EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.EmployeeID).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.EmployeeID).HumanRole.HumanDepartment.Name
                            },
                            Time = item.Time,
                            Content = item.Content,
                            FormID = item.CustomerContactFormID,
                            IsSuccess = item.IsSuccess,
                            Requirment = item.Requirment,
                            RequirementField = item.CustomerContactForm.RequirementField,
                            Note = item.Note,
                            Cost = item.Cost,
                            IsOfficial = item.IsOfficial,
                            IsPotential = item.IsPotential,
                            //IsPrice = item.IsPrice,
                            CreateAt = item.CreateAt,
                        })
                        .First();
            if (result != null)
            {
                result.AttachmentFile = new AttachmentFileItem()
                {
                    Files = dbo.CustomerContactHistoryAttachmentFiles.Where(i => i.CustomerContactHistoryID == Id)
                        .Select(i => i.AttachmentFileID).ToList()
                };
            }
            return result;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 30/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerContactHistoryItem item, int userID, int employeeID)
        {
            var contact = CustomerContactHistoryDA.GetById(item.ID);
            contact.ID = item.ID;
            contact.CustomerID = item.CustomerID;
            contact.EmployeeID = employeeID;
            contact.Time = item.Time;
            contact.Content = item.Content;
            contact.CustomerContactFormID = item.FormID;
            contact.IsSuccess = item.IsSuccess;
            contact.Note = item.Note;
            contact.Cost = item.Cost;
            contact.Requirment = item.Requirment;
            contact.IsOfficial = item.IsOfficial;
            contact.IsPotential = item.IsPotential;
            //contact.IsPrice = item.IsPrice;
            contact.UpdateAt = DateTime.Now;
            contact.UpdateBy = userID;
            CustomerContactHistoryDA.Save();
            new FileSV().Upload<CustomerContactHistoryAttachmentFile>(item.AttachmentFile, contact.ID);
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 30/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerContactHistoryItem item, int userID, int employeeId)
        {
            var contact = new CustomerContactHistory()
            {
                CustomerID = item.CustomerID,
                EmployeeID = employeeId,
                Time = item.Time,
                Content = item.Content,
                CustomerContactFormID = item.FormID,
                IsOfficial = item.IsOfficial,
                IsPotential = item.IsPotential,
                //IsPrice = item.IsPrice,
                IsSuccess = item.IsSuccess,
                Requirment = item.Requirment == null ? "" : item.Requirment,
                Note = item.Note,
                Cost = item.Cost,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerContactHistoryDA.Insert(contact);
            CustomerContactHistoryDA.Save();
            new FileSV().Upload<CustomerContactHistoryAttachmentFile>(item.AttachmentFile, contact.ID);
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 30/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerContactHistoryDA.Delete(id);
            CustomerContactHistoryDA.Save();
        }

        public HumanEmployeeViewItem GetEmployeeByID(int EmployeeID)
        {
            var dbo = CustomerContactHistoryDA.Repository;
            return new HumanEmployeeViewItem()
                            {
                                ID = EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == EmployeeID).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == EmployeeID) == null ?
                                        "" : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == EmployeeID).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == EmployeeID) == null ?
                                            "" : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == EmployeeID).HumanRole.HumanDepartment.Name
                            };
        }
    }
}
