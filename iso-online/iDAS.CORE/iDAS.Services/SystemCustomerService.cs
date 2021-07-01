using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DAL;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class SystemCustomerService 
    {
        SystemCustomerDA customerDA = new SystemCustomerDA();

        public List<SystemCustomerItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var customers = customerDA.GetQuery()
                        .Select(item => new SystemCustomerItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            Company = item.Company,
                            Email = item.Email,
                            Phone = item.Phone,
                            Logo =  item.Logo,
                            IsActive = item.IsActive.Value,
                            ActiveAt = item.ActiveAt,
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

        public string GetLogoById(int id) {
            var customer = customerDA.GetById(id);
            string logo = string.Empty;
            if (customer != null)
            {
                logo = customer.Logo;
            }
            return logo;
        }

        public void Insert(SystemCustomerItem item, int userID)
        {
            var customer = new SystemCustomer()
            {
                ID = item.ID,
                Code = item.Code,
                Name = item.Name,
                Address = item.Address,
                Company = item.Company,
                Email = item.Email,
                Phone = item.Phone,
                Logo = item.Logo,
                IsActive = item.IsActive,
                ActiveAt = item.ActiveAt,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            customerDA.Insert(customer);
            customerDA.Save();
        }

        public void Update(SystemCustomerItem item, int userID)
        {
            var customer = customerDA.GetById(item.ID);
            if (customer != null)
            {
                customer.ID = item.ID;
                customer.Code = item.Code;
                customer.Name = item.Name;
                customer.Company = item.Company;
                customer.Email = item.Email;
                customer.Phone = item.Phone;
                customer.IsActive = item.IsActive;
                customer.ActiveAt = item.ActiveAt;
                customer.CreateAt = item.CreateAt;
                customer.CreateBy = item.CreateBy;
                customer.UpdateAt = item.UpdateAt;
                customer.UpdateBy = item.UpdateBy;
                customerDA.Save();
            }
        }

        public void Update(int id , string logo){
            var customer = customerDA.GetById(id);
            if (customer != null)
            {
                customer.Logo = logo;
                customerDA.Save();
            }
        }

        public void Delete(int id)
        {
            customerDA.Delete(id);
            customerDA.Save();
        }

        public void CreateDatabase(int id, int userID)
        {
            var customer = customerDA.GetById(id);
            if (customer != null)
            {
                var dbName = customer.Code + "Database";
                //BaseDatabase.CreateDatabase<iDASDataEntities>(dbName);
                
                int _id;
                var service = new SystemUserService(dbName);
                //IUser admin = ManagerCustomer.GetAccountAdminCustomer<SystemUserItem>();
                //service.Insert(admin, out _id);

                //var infoCustomer = ManagerCustomer.GetCustomer<SystemCustomerItem>(customer.Code, _id.ToString());
                //customer.GUID = infoCustomer.GUID;
                customer.DatabaseName = dbName;
                //customer.DatabaseDataSource = infoCustomer.DatabaseDataSource;
                //customer.DatabaseUserID = infoCustomer.DatabaseUserID;
                //customer.DatabasePassword = infoCustomer.DatabasePassword;
                customer.UpdateBy = userID;
                customer.UpdateAt = DateTime.Now;
                customerDA.Save();
                copyDatabase(dbName);
            }
        }

        private void copyDatabase(string dbName) {
            //var dbcontext1 = BaseDatabase.GetDbContextCenter<iDASDataEntities>();
            //var dbcontext2 = BaseDatabase.GetDbContext<iDASDataEntities>(dbName);
            //ManagerDatabase.CopyDatabase<InitCity>(dbcontext1, dbcontext2);
            //ManagerDatabase.CopyDatabase<InitCountry>(dbcontext1, dbcontext2);
            //ManagerDatabase.CopyDatabase<iDASDataEntities,MnCustomerSurvey>(dbName);
        }

        public void DeleteDatabase(int id, Database database, int userID)
        {
            var customer = customerDA.GetById(id);
            database.DataName = customer.Code + "Database";
            customer.UpdateBy = userID;
            customer.UpdateAt = DateTime.Now;
            customerDA.Save();
            //BaseDatabase.Instance.DeleteDatabase<iDASDataEntities>(database);
        }

        public Database GetDatabaseByCode(string code)
        {
            var database = customerDA.GetQuery(c => c.Code == code && c.IsActive.Value)
                    .Select(c => new Database()
                             {
                                 DataSource = c.DatabaseDataSource,
                                 DataName = c.DatabaseName,
                                 UserId = c.DatabaseUserID,
                                 Password = c.DatabasePassword,
                             }).FirstOrDefault();
            return database;
        }

        //public ICustomer GetCustomerByGuid(Guid guid) {
        //    var customer = customerDA.GetQuery(c => c.GUID == guid && c.IsActive.Value).Select(
        //                         c => new SystemCustomerItem()
        //                         {
        //                             ID = c.ID,
        //                             Code = c.Code,
        //                             Logo = c.Logo,
        //                             DatabaseDataSource = c.DatabaseDataSource,
        //                             DatabaseName = c.DatabaseName,
        //                             DatabaseUserID = c.DatabaseUserID,
        //                             DatabasePassword = c.DatabasePassword,
        //                         }).FirstOrDefault();
        //    return customer;
        //}
    }
}
