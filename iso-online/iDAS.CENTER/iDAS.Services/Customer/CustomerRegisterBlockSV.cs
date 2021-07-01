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
    public class CustomerRegisterBlockSV
    {
        CustomerRegisterProductBlockDA RegisterProductBlockDA = new CustomerRegisterProductBlockDA();
        public List<CustomerRegisterProductBlockItem> GetAll(int page, int pageSize, out int totalCount)
        {
            return RegisterProductBlockDA.GetQuery()
                   .Select(i => new CustomerRegisterProductBlockItem
                   {
                       ID = i.ID,
                       CustomerID = i.CustomerID,
                       CustomerName = i.Customer.Company,
                       ProductBlockID = i.ProductBlockID,
                       ProductBlockName = i.ProductBlock.Name,
                       CreateAt = i.CreateAt,
                   })
                   .OrderByDescending(item => item.CreateAt)
                   .Page(page, pageSize, out totalCount)
                   .ToList();
        }
    }
}
