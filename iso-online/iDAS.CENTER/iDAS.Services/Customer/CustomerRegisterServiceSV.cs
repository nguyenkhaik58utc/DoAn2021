using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class CustomerRegisterServiceSV : BaseService
    {
        CustomerRegisterServiceDA customerRegisterServiceDA = new CustomerRegisterServiceDA();

        public List<CustomerItem> GetCustomers()
        {
            var dbo = customerRegisterServiceDA.Repository;
            var lst = dbo.Customers
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Name)
                        .Select(i => new CustomerItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return lst;
        }
        public List<ServiceItem> GetServices()
        {
            var dbo = customerRegisterServiceDA.Repository;
            var lst = dbo.WebServices
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Name)
                        .Select(i => new ServiceItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return lst;
        }
        public List<ServiceProgramItem> GetServiceItems(int serviceId)
        {
            var dbo = customerRegisterServiceDA.Repository;
            var lst = dbo.WebServiceItems
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Where(i => i.WebServiceID == serviceId)
                        .OrderBy(i => i.Name)
                        .Select(i => new ServiceProgramItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return lst;
        }

        public List<CustomerRegisterServiceItem> GetCustomerRegisterServices(int page, int pageSize, out int total)
        {
            var lst = customerRegisterServiceDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerRegisterServiceItem()
                        {
                            ID = i.ID,
                            CustomerID = i.CustomerID,
                            CustomerFileId = i.Customer.FileID,
                            CustomerName = i.Customer.Name,
                            WebServiceItemID = i.WebServiceItemID,
                            RegisterAt = i.RegisterAt,
                            CompleteAt = i.CompleteAt,
                            ContactAt = i.ContactAt,
                            Note = i.Note,
                            IsNew = i.IsNew,
                            IsContact = i.IsContact,
                            IsAccept = i.IsAccept,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public CustomerRegisterServiceItem GetCustomerRegisterService(int id)
        {
            var item = customerRegisterServiceDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerRegisterServiceItem()
                        {
                            ID = i.ID,
                            CustomerID = i.CustomerID,
                            WebServiceItemID = i.WebServiceItemID,
                            ServiceID = i.WebServiceItem.WebServiceID,
                            RegisterAt = i.RegisterAt,
                            CompleteAt = i.CompleteAt,
                            ContactAt = i.ContactAt,
                            Note = i.Note,
                            IsNew = i.IsNew,
                            IsContact = i.IsContact,
                            IsAccept = i.IsAccept,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(CustomerRegisterServiceItem item)
        {
            var record = new CustomerRegisterService()
            {
                CustomerID = item.CustomerID,
                WebServiceItemID = item.WebServiceItemID,
                RegisterAt = item.RegisterAt,
                CompleteAt = item.CompleteAt,
                ContactAt = item.ContactAt,
                Note = item.Note,
                IsNew = item.IsNew,
                IsContact = item.IsContact,
                IsAccept = item.IsAccept,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            customerRegisterServiceDA.Insert(record);
            customerRegisterServiceDA.Save();
        }
        public void Update(CustomerRegisterServiceItem item)
        {
            var record = customerRegisterServiceDA.GetById(item.ID);
            record.CustomerID = item.CustomerID;
            record.WebServiceItemID = item.WebServiceItemID;
            record.RegisterAt = item.RegisterAt;
            record.CompleteAt = item.CompleteAt;
            record.ContactAt = item.ContactAt;
            record.Note = item.Note;
            record.IsNew = item.IsNew;
            record.IsContact = item.IsContact;
            record.IsAccept = item.IsAccept;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            customerRegisterServiceDA.Save();
        }
        public void Delete(int id)
        {
            var item = customerRegisterServiceDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            customerRegisterServiceDA.Save();
        }
    }
}
