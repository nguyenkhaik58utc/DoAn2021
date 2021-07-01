using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects.DataClasses;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;

namespace iDAS.Services
{
    public class CenterCustomerSV
    {
        CenterCustomerDA customerDA = new CenterCustomerDA();

        public Database GetDatabaseByCode(string code)
        {

            var database = customerDA.GetQuery(i => i.Code == code && i.IsActive && !i.IsDelete)
                    .Select(i => new Database()
                    {
                        DataSource = i.DBSource,
                        DataName = i.DBName,
                        UserId = i.DBUserID,
                        Password = i.DBPassword,
                    }).FirstOrDefault();

            return database;
        }

        public CustomerSystem GetCustomer(int customerID)
        {
            var customer = customerDA.GetQuery().Where(i => i.ID == customerID).FirstOrDefault();
            return customer;
        }
        public CustomerSystem GetCustomerByCode(string code)
        {
            var customer = customerDA.GetQuery().Where(i => i.Code == code).FirstOrDefault();
            return customer;
        }
        public string GetCustomerNameByCode(string code)
        {
            var dbo = customerDA.Repository;
            var customerId = customerDA.GetQuery().Where(i => i.Code == code).FirstOrDefault().CustomerID;
            return dbo.CustomerSystems.FirstOrDefault(t => t.CustomerID == customerId).Company;
        }
        public void Insert(Database database, string code)
        {
            CustomerSystem customer = new CustomerSystem()
            {
                Code = code,
                DBSource = database.DataSource,
                DBName = database.DataName,
                DBUserID = database.UserId,
                DBPassword = database.Password,
                IsActive = true,
                //CustomerID = 1,
            };
            customerDA.Insert(customer);
            customerDA.Save();
        }

        public byte[] GetLogoFile(string code)
        {
            var systemCode = BaseSystem.SystemCode;
            var fileId = customerDA.GetQuery()
                       .Where(i => i.CenterSystem.SystemCode == systemCode)
                       .Where(i => i.Code == code)
                       .Where(i => i.IsActive)
                       .Where(i => !i.IsDelete)
                       .Select(i => i.FileID)
                       .FirstOrDefault();
            var data = customerDA.Repository.Files.Where(i => i.ID == fileId).Select(i => i.Data).FirstOrDefault();
            if (data == null)
            {
                data = new CenterSystemSV().GetLogoSystem();
            }
            return data;
        }
    }
}
