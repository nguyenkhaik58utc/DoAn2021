using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class CustomerSystemModuleSV
    {
        CustomerSystemModuleDA CustomerSystemModuleDA = new CustomerSystemModuleDA();
        public List<ModuleItem> GetByCustomer(int customerSystemID)
        {
            return CustomerSystemModuleDA.GetQuery(i => i.CustomerSystemID == customerSystemID)
                    .Select(item => new ModuleItem()
                    {
                        SystemCode = item.CenterModule.SystemCode,
                        Code = item.CenterModule.Code,
                        Name = item.CenterModule.Name,
                        IsActive = item.CenterModule.IsActive,
                        IsShow = item.CenterModule.IsShow,
                        IsDelete = item.CenterModule.IsDelete,
                        Position = item.CenterModule.Position,
                        Icon = item.CenterModule.Icon,
                    }).ToList();
        }
        public void Update(CustomerSystemModuleItem item, int userID)
        {
            var CustomerSystemModule = CustomerSystemModuleDA.GetById(item.ID);
            CustomerSystemModule.CustomerSystemID = item.CustomerSystemID;
            CustomerSystemModule.CenterModuleID = item.CustomerSystemID;
            CustomerSystemModule.UpdateAt = DateTime.Now;
            CustomerSystemModule.UpdateBy = userID;
            CustomerSystemModuleDA.Save();
        }
        public void Insert(CustomerSystemModuleItem item, int userID)
        {
            var cusSyModule = new CustomerSystemModule
            {
                CustomerSystemID = item.CustomerSystemID,
                CenterModuleID = item.CustomerSystemID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerSystemModuleDA.Insert(cusSyModule);
            CustomerSystemModuleDA.Save();
        }
        public void UpdateModule(int customerSystemId)
        {
            var dbo = CustomerSystemModuleDA.Repository;
            var modules = dbo.CustomerSystemModules.Where(i => i.CustomerSystemID == customerSystemId);
            var customerModuleUpdate = CustomerSystemModuleDA.GetQuery(i => i.CustomerSystemID == customerSystemId);
            //var lstUpdate = new List<CustomerSystemModule>();
            //foreach (var cusModuleItem in customerModuleUpdate)
            //{
            //    lstUpdate.Add(cusModuleItem);
            //}
            //if (lstUpdate != null && lstUpdate.Count > 0)
            //    CustomerSystemModuleDA.DeleteRange(lstUpdate);
            CustomerSystemModuleDA.Save();
        }

    }
}
