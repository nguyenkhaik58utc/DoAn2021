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
    public class CustomerRegisterServiceConsultancySV
    {
        CustomerRegisterServiceConsultancyDA RegisterServiceTrainingDA = new CustomerRegisterServiceConsultancyDA();
        public List<CustomerRegisterServiceConsultancyItem> GetAll(int page, int pageSize, out int totalCount)
        {
            return RegisterServiceTrainingDA.GetQuery()
                   .Select(i => new CustomerRegisterServiceConsultancyItem
                   {
                       ID = i.ID,
                       CustomerID = i.CustomerID,
                       CustomerName = i.Customer.Company,
                       ISOName = i.ServiceConsultancy.ISOStandard.Code,
                       ApplyFor = i.ServiceConsultancy.ApplyFor,
                       CreateAt = i.CreateAt,
                   })
                   .OrderByDescending(item => item.CreateAt)
                   .Page(page, pageSize, out totalCount)
                   .ToList();
        }
    }
}
