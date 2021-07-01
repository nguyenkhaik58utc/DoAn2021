using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;
using iDAS.Core;
using iDAS.Utilities;

namespace iDAS.Services
{
    public class CustomerRegisterPakageSV
    {
        CustomerRegisterProductPakageDA RegisterProductPakageDA = new CustomerRegisterProductPakageDA();
        public List<CustomerRegisterProductPakageItem> GetAll(int page, int pageSize, out int totalCount)
        {
            return RegisterProductPakageDA.GetQuery()
                   .Select(item => new CustomerRegisterProductPakageItem
                   {
                       ID = item.ID,
                       CustomerID = item.CustomerID,
                       CustomerName = item.Customer.Company,
                       ISONaceCodeID = item.ISONaceCodeID,
                       ProductPakageID = item.ProductPakageID,
                       SystemName = item.ProductPakage.SystemCode == "BUSINESS" ? "Doanh nghiệp" : "Hành chính công",
                       ISOStandardCode = item.ProductPakage.ISOStandard.Code,
                       ApplyFor = item.ProductPakage.ApplyFor,
                       CreateAt = item.CreateAt,
                       IsSend = item.IsSend
                   })
                   .OrderByDescending(item => item.CreateAt)
                   .Page(page, pageSize, out totalCount)
                   .ToList();

        }

        public void Send(int id, int userId)
        {
            var dbo = RegisterProductPakageDA.Repository;
            var productPakage = dbo.CustomerRegisterProductPakages.Where(i => i.ID == id).FirstOrDefault();
            productPakage.IsSend = true;
            productPakage.UpdateBy = userId;
            productPakage.UpdateAt = DateTime.Now;
            var customerId = productPakage.CustomerID;
            var systemId = dbo.CenterSystems.Where(i => i.SystemCode == productPakage.ProductPakage.SystemCode).Select(i => i.ID).FirstOrDefault();
            var customerSystem = dbo.CustomerSystems.Where(i => i.CustomerID == customerId && i.CenterSystemID == systemId).FirstOrDefault();
            if (customerSystem == null)
            {
                dbo.CustomerSystems.Add(new CustomerSystem()
                    {
                        CustomerID = customerId,
                        CenterSystemID = systemId,
                        IsNew = true,
                        CreateBy = userId,
                        CreateAt = DateTime.Now
                    });
            }
            dbo.SaveChanges();
        }
    }
}
