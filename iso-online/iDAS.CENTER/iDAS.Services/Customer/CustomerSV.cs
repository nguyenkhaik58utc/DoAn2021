using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;
using iDAS.Core;
using System.Data.SqlClient;

namespace iDAS.Services
{
    public class CustomerSV : BaseService
    {
        CustomerDA CustomerDA = new CustomerDA();

        public List<WebCustomerCityItem> GetCities() { 
            var dbo = CustomerDA.Repository;
            var lst = dbo.WebCustomerCities
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Name)
                        .Select(i => new WebCustomerCityItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return lst;
        }
        public List<CustomerItem> GetCustomers(int page, int pageSize, out int total)
        {
            var lst = CustomerDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Address = i.Address,
                            Birthday = i.Birthday,
                            Role = i.Role,
                            Represent = i.Represent,
                            CompanySize = i.CompanySize,
                            Scope = i.Scope,
                            Website = i.Website,
                            TaxCode = i.TaxCode,
                            WebCustomerCityID = i.WebCustomerCityID,
                            Email = i.Email,
                            Password = i.Password,
                            Phone = i.Phone,
                            FileID = i.FileID,
                            RegisterAt = i.RegisterAt,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public CustomerItem GetCustomer(int id)
        {
            var item = CustomerDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new CustomerItem()
                        {
                            ID = i.ID,
                            Image = new AttachmentFileItem{ID = i.FileID},
                            // HungNM. Fix error logo of company. 20201001.
                            FileID = i.FileID,
                            // End.
                            Name = i.Name,
                            Address = i.Address,
                            Birthday = i.Birthday,
                            Role = i.Role,
                            Represent = i.Represent,
                            CompanySize = i.CompanySize,
                            Scope = i.Scope,
                            Website = i.Website,
                            TaxCode = i.TaxCode,
                            WebCustomerCityID = i.WebCustomerCityID,
                            Email = i.Email,
                            Password = i.Password,
                            Phone = i.Phone,
                            IsPersonal = i.IsPersonal,
                            RegisterAt = i.RegisterAt,
                            RefreshAt = i.RefreshAt,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(CustomerItem item)
        {
            var record = new Customer()
            {
                Name = item.Name,
                Address = item.Address,
                Birthday = item.Birthday,
                Role = item.Role,
                Represent = item.Represent,
                CompanySize = item.CompanySize,
                Scope = item.Scope,
                Website = item.Website,
                TaxCode = item.TaxCode,
                WebCustomerCityID = item.WebCustomerCityID,
                Email = item.Email,
                Phone = item.Phone,
                RegisterAt = item.RegisterAt,
                IsPersonal = item.IsPersonal,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty) {
                record.FileID = fileId;
            }
            CustomerDA.Insert(record);
            CustomerDA.Save();
        }
        public void Update(CustomerItem item)
        {
            var record = CustomerDA.GetById(item.ID);
            record.Name = item.Name;
            record.Address = item.Address;
            record.Birthday = item.Birthday;
            record.Role = item.Role;
            record.Represent = item.Represent;
            record.CompanySize = item.CompanySize;
            record.Scope = item.Scope;
            record.Website = item.Website;
            record.TaxCode = item.TaxCode;
            record.WebCustomerCityID = item.WebCustomerCityID;
            record.Email = item.Email;
            record.Phone = item.Phone;
            record.RegisterAt = item.RegisterAt;
            record.IsPersonal = item.IsPersonal;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            CustomerDA.Save();
        }
        public void Delete(int id)
        {
            var item = CustomerDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            CustomerDA.Save();
        }
        public bool CheckExist(CustomerItem item)
        {
            var check = CustomerDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Email.ToUpper().Equals(item.Email.ToUpper()) && !string.IsNullOrEmpty(item.Email))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
