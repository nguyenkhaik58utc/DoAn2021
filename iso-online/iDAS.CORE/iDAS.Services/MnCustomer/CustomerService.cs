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
    public class CustomerService
    {
        CustomerDA customerDA = new CustomerDA();

        public List<CustomerItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var customers = customerDA.GetQuery()
                        .Select(item => new CustomerItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ShortName = item.ShortName,
                            BirthDay = item.BirthDay,
                            DateOfIncorporation = item.BirthDay,
                            Representative = item.Representative,
                            Positions = item.Positions,
                            IsActive = item.IsActive,
                            Phone = item.Phone,
                            Email = item.Email,
                            Fax = item.Fax,
                            Address = item.Address,
                            CountryID = item.CountryID,
                            CityID = item.CityID,
                            DistrictID = item.DistrictID,
                            CategoryID = item.CategoryID,
                            Type = item.Type,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return customers;
        }

        public List<CustomerItem> GetAll(int categoryId, bool type, int page, int pageSize, out int totalCount)
        {
            var customers = customerDA.GetQuery()
                        .Where(item => item.CategoryID == categoryId && item.Type == type)
                        .Select(item => new CustomerItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ShortName = item.ShortName,
                            BirthDay = item.BirthDay,
                            DateOfIncorporation = item.BirthDay,
                            Representative = item.Representative,
                            Positions = item.Positions,
                            IsActive = item.IsActive,
                            Phone = item.Phone,
                            Email = item.Email,
                            Fax = item.Fax,
                            Address = item.Address,
                            CountryID = item.CountryID,
                            CityID = item.CityID,
                            DistrictID = item.DistrictID,
                            CategoryID = item.CategoryID,
                            Type = item.Type,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return customers;
        }

        public List<CustomerItem> GetAll(int campaignId, int categoryId, bool type, int page, int pageSize, out int totalCount)
        {
            var dbo =  customerDA.SystemContext;
            //var customerIDs;//dbo.MnCustomerCampaigns
            //        //.Join(dbo.MnCustomerDemands, c => c.ProductID, d => d.ProductID, (c, d) => d.CustomerID).Distinct().ToList();
            ////var productId = profileDA.SystemContext.MnCustomerCampaigns.FirstOrDefault(i => i.ID == campaignId).ProductID;
            ////var customerIDs = profileDA.SystemContext.MnCustomerDemands.Where(i=>i.ProductID == productId).Select(i=>i.CustomerID);
            //var customers = customerDA.GetQuery()
            //            .Where(item => item.CategoryID == categoryId && item.Type == type
            //                && customerIDs.Contains(item.ID)
            //            )
            //            .Select(item => new CustomerItem()
            //            {
            //                ID = item.ID,
            //                Name = item.Name,
            //                ShortName = item.ShortName,
            //                BirthDay = item.BirthDay,
            //                DateOfIncorporation = item.BirthDay,
            //                Representative = item.Representative,
            //                Positions = item.Positions,
            //                IsActive = item.IsActive,
            //                Phone = item.Phone,
            //                Email = item.Email,
            //                Fax = item.Fax,
            //                Address = item.Address,
            //                CountryID = item.CountryID,
            //                CityID = item.CityID,
            //                DistrictID = item.DistrictID,
            //                CategoryID = item.CategoryID,
            //                Type = item.Type,
            //                CreateAt = item.CreateAt,
            //                CreateBy = item.CreateBy,
            //                UpdateAt = item.UpdateAt,
            //                UpdateBy = item.UpdateBy,
            //            })
            //            .OrderByDescending(item => item.CreateAt)
            //            .Page(page, pageSize, out totalCount)
            //            .ToList();
            //return customers;
            totalCount = 0;
            return null;
        }

        public CustomerItem GetByID(int id) {
            var customer = customerDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Select(item => new CustomerItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ShortName = item.ShortName,
                            BirthDay = item.BirthDay,
                            DateOfIncorporation = item.BirthDay,
                            Representative = item.Representative,
                            Positions = item.Positions,
                            IsActive = item.IsActive,
                            Phone = item.Phone,
                            Email = item.Email,
                            Fax = item.Fax,
                            Address = item.Address,
                            CountryID = item.CountryID,
                            CountryName = customerDA.SystemContext.InitCountries.FirstOrDefault(i=>i.ID == item.CountryID).Name,
                            CityID = item.CityID,
                            CityName = customerDA.SystemContext.InitCities.FirstOrDefault(i => i.ID == item.CityID).Name,
                            DistrictID = item.DistrictID,
                            DistrictName = customerDA.SystemContext.InitDistricts.FirstOrDefault(i => i.ID == item.DistrictID).Name,
                            CategoryID = item.CategoryID,
                            CategoryName = customerDA.SystemContext.MnCustomerCategories.FirstOrDefault(i=>i.ID==item.CategoryID).Name,
                            Type = item.Type,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = customerDA.SystemContext.SystemUsers.FirstOrDefault(i=>i.ID == item.CreateBy).FullName,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            UpdateByName = customerDA.SystemContext.SystemUsers.FirstOrDefault(i => i.ID == item.UpdateBy).FullName,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .FirstOrDefault();
            return customer;
        }

        public void Insert(CustomerItem item)
        {
            var customer = new MnCustomerCustomer()
            {
                CategoryID = item.CategoryID,
                Type = item.Type,
                Name = item.Name,
                ShortName = item.ShortName,
                BirthDay = item.Type? item.BirthDay:item.DateOfIncorporation,
                Representative = item.Representative,
                Positions = item.Positions,
                IsActive = item.IsActive,
                Email = item.Email,
                Phone = item.Phone,
                Fax = item.Fax,
                Address = item.Address,
                CountryID = item.CountryID,
                CityID = item.CityID,
                DistrictID = item.DistrictID,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            customerDA.Insert(customer);
            customerDA.Save();
        }
        public void Update(CustomerItem item)
        {
            var customer = customerDA.GetById(item.ID);
            customer.CategoryID = item.CategoryID;
            customer.Type = item.Type;
            customer.Name = item.Name;
            customer.ShortName = item.ShortName;
            customer.BirthDay = item.BirthDay;
            customer.Representative = item.Representative;
            customer.Positions = item.Positions;
            customer.IsActive = item.IsActive;
            customer.Phone = item.Phone;
            customer.Email = item.Email;
            customer.Fax = item.Fax;
            customer.Address = item.Address;
            customer.CountryID = item.CountryID;
            customer.CityID = item.CityID;
            customer.DistrictID = item.DistrictID;
            customer.UpdateAt = DateTime.UtcNow;
            customer.UpdateBy = item.UpdateBy;
            customerDA.Save();
        }
    }
}
