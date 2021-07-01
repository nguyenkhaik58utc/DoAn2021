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
    public class CustomerRegisterDatasizeSV : BaseService
    {
        CustomerRegisterDatasizeDA customerRegisterDatasizeDA = new CustomerRegisterDatasizeDA();

        public List<CustomerItem> GetCustomers()
        {
            var dbo = customerRegisterDatasizeDA.Repository;
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
        public List<SystemItem> GetSystems()
        {
            var dbo = customerRegisterDatasizeDA.Repository;
            var lst = dbo.CenterSystems
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Name)
                        .Select(i => new SystemItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return lst;
        }

        public List<CustomerRegisterDatasizeItem> GetCustomerRegisterDatasizes(int page, int pageSize, out int total)
        {
            var lst = customerRegisterDatasizeDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerRegisterDatasizeItem()
                        {
                            ID = i.ID,
                            CustomerID = i.CustomerID,
                            CustomerFileId = i.Customer.FileID,
                            CustomerName = i.Customer.Name,
                            CenterSystemID = i.CenterSystemID,
                            DataSize = i.DataSize,
                            RequireAt = i.RequireAt,
                            CompleteAt = i.CompleteAt,
                            ContactAt = i.ContactAt,
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
        public CustomerRegisterDatasizeItem GetCustomerRegisterDatasize(int id)
        {
            var item = customerRegisterDatasizeDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerRegisterDatasizeItem()
                        {
                            ID = i.ID,
                            CustomerID = i.CustomerID,
                            CenterSystemID = i.CenterSystemID,
                            DataSize = i.DataSize,
                            RequireAt = i.RequireAt,
                            CompleteAt = i.CompleteAt,
                            ContactAt = i.ContactAt,
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
        public void Insert(CustomerRegisterDatasizeItem item)
        {
            var record = new CustomerRegisterDataSize()
            {
                CustomerID = item.CustomerID,
                CenterSystemID = item.CenterSystemID,
                DataSize = item.DataSize,
                RequireAt = item.RequireAt,
                CompleteAt = item.CompleteAt,
                ContactAt = item.ContactAt,
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
            customerRegisterDatasizeDA.Insert(record);
            customerRegisterDatasizeDA.Save();
        }
        public void Update(CustomerRegisterDatasizeItem item)
        {
            var record = customerRegisterDatasizeDA.GetById(item.ID);
            record.CustomerID = item.CustomerID;
            record.CenterSystemID = item.CenterSystemID;
            record.DataSize = item.DataSize;
            record.RequireAt = item.RequireAt;
            record.CompleteAt = item.CompleteAt;
            record.ContactAt = item.ContactAt;
            record.IsNew = item.IsNew;
            record.IsContact = item.IsContact;
            record.IsAccept = item.IsAccept;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            customerRegisterDatasizeDA.Save();
        }
        public void Delete(int id)
        {
            var item = customerRegisterDatasizeDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            customerRegisterDatasizeDA.Save();
        }
    }
}
