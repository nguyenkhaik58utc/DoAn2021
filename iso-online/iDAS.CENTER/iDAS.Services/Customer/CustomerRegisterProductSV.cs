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
    public class CustomerRegisterProductSV : BaseService
    {
        CustomerRegisterProductDA customerRegisterProductDA = new CustomerRegisterProductDA();

        public List<CustomerItem> GetCustomers() {
            var dbo = customerRegisterProductDA.Repository;
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
        public List<SystemItem> GetSystems() {
            var dbo = customerRegisterProductDA.Repository;
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
        public List<SystemItem> GetProducts(int systemId)
        {
            var dbo = customerRegisterProductDA.Repository;
            var lst = dbo.WebProducts
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Where(i => i.WebProductCategory.CenterSystem.ID == systemId)
                        .OrderBy(i => i.Name)
                        .Select(i => new SystemItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return lst;
        }
        public List<ProductMethodItem> GetProductMethods()
        {
            var dbo = customerRegisterProductDA.Repository;
            var lst = dbo.WebProductMethods
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Name)
                        .Select(i => new ProductMethodItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return lst;
        }
        public List<ProductDataSizeItem> GetProductDataSizes()
        {
            var dbo = customerRegisterProductDA.Repository;
            var lst = dbo.WebProductDataSizes
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Size)
                        .Select(i => new ProductDataSizeItem()
                        {
                            ID = i.ID,
                            Size = i.Size,
                            Unit = i.Unit,
                        })
                        .ToList();
            return lst;
        }
        public List<ProductScopeItem> GetProductScopes()
        {
            var dbo = customerRegisterProductDA.Repository;
            var lst = dbo.WebProductScopes
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.NaceCodes)
                        .Select(i => new ProductScopeItem()
                        {
                            ID = i.ID,
                            NaceCodes = i.NaceCodes,
                        })
                        .ToList();
            return lst;
        }

        public List<CustomerRegisterProductItem> GetCustomerRegisterProducts(int page, int pageSize, out int total)
        {
            var lst = customerRegisterProductDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerRegisterProductItem()
                        {
                            ID = i.ID,
                            CustomerFileId = i.Customer.FileID,
                            CustomerName = i.Customer.Name,
                            ProductName = i.WebProduct.Name,
                            ProductMethodName = i.WebProductMethod.Name,
                            ProductDatesizeName = i.WebProductDataSize.Size + " " + i.WebProductDataSize.Unit,
                            IsContact = i.IsContact,
                            IsAccept = i.IsAccept,
                            IsActive = i.IsActive,
                            RegisterAt = i.RegisterAt,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public CustomerRegisterProductItem GetCustomerRegisterProduct(int id)
        {
            var item = customerRegisterProductDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerRegisterProductItem()
                        {
                            ID = i.ID,
                            CustomerID = i.CustomerID,
                            SystemID = i.WebProduct.WebProductCategory.CenterSystem.ID,
                            WebProductID = i.WebProductID,
                            WebProductDateSizeID = i.WebProductDateSizeID,
                            WebProductMethodID = i.WebProductMethodID,
                            WebProductScopeID = i.WebProductScopeID,
                            Note = i.Note,
                            RegisterAt = i.RegisterAt,
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
        public void Insert(CustomerRegisterProductItem item)
        {
            var record = new CustomerRegisterProduct()
            {
                CustomerID = item.CustomerID,
                WebProductID = item.WebProductID,
                WebProductDateSizeID = item.WebProductDateSizeID,
                WebProductMethodID = item.WebProductMethodID,
                WebProductScopeID= item.WebProductScopeID,
                Note = item.Note,
                RegisterAt = item.RegisterAt,
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
            customerRegisterProductDA.Insert(record);
            customerRegisterProductDA.Save();
        }
        public void Update(CustomerRegisterProductItem item)
        {
            var record = customerRegisterProductDA.GetById(item.ID);
            record.CustomerID = item.CustomerID;
            record.WebProductID = item.WebProductID;
            record.WebProductDateSizeID = item.WebProductDateSizeID;
            record.WebProductMethodID = item.WebProductMethodID;
            record.WebProductScopeID = item.WebProductScopeID;
            record.Note = item.Note;
            record.RegisterAt = item.RegisterAt;
            record.CompleteAt = item.CompleteAt;
            record.ContactAt = item.ContactAt;
            record.IsNew = item.IsNew;
            record.IsContact = item.IsContact;
            record.IsAccept = item.IsAccept;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            customerRegisterProductDA.Save();
        }
        public void Delete(int id)
        {
            var item = customerRegisterProductDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            customerRegisterProductDA.Save();
        }
    }
}
