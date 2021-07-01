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
    public class CustomerRegisterTrainingSV
    {
        CustomerRegisterServiceTrainingDA RegisterServiceTrainingDA = new CustomerRegisterServiceTrainingDA();
        public List<CustomerRegisterServiceTrainingItem> GetAll(int page, int pageSize, out int totalCount)
        {
            return RegisterServiceTrainingDA.GetQuery()
                   .Select(i => new CustomerRegisterServiceTrainingItem
                   {
                       ID = i.ID,
                       CustomerID = i.CustomerID,
                       CustomerName = i.Customer.Company,
                       ISOName = i.ServiceTraining.ISOStandard.Code,
                       ApplyFor = i.ServiceTraining.ApplyFor,
                       CreateAt = i.CreateAt,
                   })
                   .OrderByDescending(item => item.CreateAt)
                   .Page(page, pageSize, out totalCount)
                   .ToList();
        }
    }
}
