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
    public class ContactService
    {
        ContactDA contactDA = new ContactDA();

        public List<ContactItem> GetAll(int campaignId, int employeeId, int page, int pageSize, out int totalCount)
        {
            var contacts = contactDA.GetQuery()
                        .Where(item => item.CampaignID == campaignId && item.EmployeeID == employeeId)
                        .Select(item => new ContactItem()
                        {
                            ID = item.ID,
                            CampaignID = item.CampaignID,
                            CampaignName = contactDA.SystemContext.MnCustomerCampaigns.Where(i => i.ID == item.CampaignID).Select(i => i.Name).FirstOrDefault() ?? string.Empty,
                            CustomerID = item.CustomerID,
                            CustomerName = contactDA.SystemContext.MnCustomerCustomers.Where(i => i.ID == item.CustomerID).Select(i => i.Name).FirstOrDefault() ?? string.Empty,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = contactDA.SystemContext.SystemUsers.Where(i => i.ID == item.EmployeeID).Select(i => i.FullName).FirstOrDefault() ?? string.Empty,
                            TimeBack = item.TimeBack,
                            TimeNew = item.TimeNew,
                            Status = item.Status,
                            Total = item.Total,
                            IsDisabled = item.IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return contacts;
        }

        public List<ContactItem> GetAll(int campaignId, int page, int pageSize, out int totalCount)
        {
            var contacts = contactDA.GetQuery()
                        .Where(item => item.CampaignID == campaignId)
                        .Select(item => new ContactItem()
                        {
                            ID = item.ID,
                            CampaignID = item.CampaignID,
                            CampaignName = contactDA.SystemContext.MnCustomerCampaigns.Where(i => i.ID == item.CampaignID).Select(i => i.Name).FirstOrDefault() ?? string.Empty,
                            CustomerID = item.CustomerID,
                            CustomerName = contactDA.SystemContext.MnCustomerCustomers.Where(i => i.ID == item.CustomerID).Select(i => i.Name).FirstOrDefault() ?? string.Empty,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = contactDA.SystemContext.SystemUsers.Where(i => i.ID == item.EmployeeID).Select(i => i.FullName).FirstOrDefault() ?? string.Empty,
                            TimeBack = item.TimeBack,
                            TimeNew = item.TimeNew,
                            Status = item.Status,
                            Total = item.Total,
                            IsDisabled = item.IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return contacts;
        }
        

        public List<CustomerItem> GetCustomers(int campaignId, int categoryId, bool type, int page, int pageSize, out int totalCount)
        {
            var dbo = contactDA.SystemContext;
            var customers = dbo.MnCustomerCustomers
                            .Where(item => item.CategoryID == categoryId && item.Type == type && item.IsActive)
                            .Where(item => item.MnCustomerContacts.Count(i => i.CampaignID == campaignId) == 0)
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

        public ContactItem GetByID(int id)
        {
            var dbo = contactDA.SystemContext;
            var contact = contactDA.GetQuery()
                        .Where(item=>item.ID == id)
                        .Select(item => new ContactItem()
                        {
                            ID = item.ID,
                            Total = item.Total,
                            CustomerID = item.CustomerID,
                            CustomerName = dbo.MnCustomerCustomers.FirstOrDefault(i=>i.ID == item.CustomerID).Name,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.EmployeeID).FullName,
                            Status = item.Status,
                            TimeNew = item.TimeNew,
                            MethodNewID = item.MethodNewID,
                            MethodNewName = dbo.MnCustomerMethods.FirstOrDefault(i => i.ID == item.MethodNewID).Name,
                            TimeBack = item.TimeBack,
                            MethodBackID = item.MethodBackID,
                            MethodBackName = dbo.MnCustomerMethods.FirstOrDefault(i=>i.ID == item.MethodBackID).Name,
                            IsDisabled = item.IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.CreateBy).FullName,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            UpdateByName = dbo.SystemUsers.FirstOrDefault(i=>i.ID == item.UpdateBy).FullName,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .FirstOrDefault();
            return contact;
        }

        public void Insert(ContactItem item)
        {
            var contact = new MnCustomerContact()
            {
                CampaignID = item.CampaignID,
                CustomerID = item.CustomerID,
                EmployeeID = item.EmployeeID,
                Total = 0,
                Status = (byte)ContactItem.ContactStatus.Not,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            contactDA.Insert(contact);
            contactDA.Save();
        }

        public void Update(ContactItem item)
        {
            var contact = contactDA.GetById(item.ID);
            contact.EmployeeID = item.EmployeeID;
            contact.IsDisabled = item.IsDisabled;
            contact.UpdateAt = DateTime.Now;
            contact.UpdateBy = item.UpdateBy;
            contact.Status = contact.IsDisabled ? (byte)ContactItem.ContactStatus.Stop : (byte)ContactItem.ContactStatus.Contact;

            contactDA.Save();
        }

        public void InsertRange(ContactItem item)
        {
            var contacts = new List<MnCustomerContact>();
            var customerIDs = item.CustomerIDs.Split(',');
            foreach(var customerId in customerIDs){
                if (!string.IsNullOrEmpty(customerId)) {
                    var id  = Convert.ToInt32(customerId);
                    item.CustomerID = id;
                    contacts.Add(getContact(item));
                }
            }
            contactDA.InsertRange(contacts);
            contactDA.Save();
        }

        public void DeleteRange(List<object> ids)
        {
            contactDA.DeleteRange(ids);
            contactDA.Save();
        }

        private MnCustomerContact getContact(ContactItem item)
        {
            var contact = new MnCustomerContact()
            {
                CampaignID = item.CampaignID,
                EmployeeID = item.EmployeeID,
                CustomerID = item.CustomerID,
                Total = 0,
                IsDisabled = false,
                Status = (byte)ContactItem.ContactStatus.Not,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            return contact;
        }
    }
}
