using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
   public class CenterRiskCategorySV
    {
       private CenterRiskCategoryDA riskCategoryDA = new CenterRiskCategoryDA();
       public List<RiskCategoryItem> GetAll()
       {
           var cate = riskCategoryDA.GetQuery()
                         .Select(item => new RiskCategoryItem()
                         {
                             ID = item.ID,
                             Name = item.Name,
                             CreateAt = item.CreateAt
                         })
                         .OrderByDescending(item => item.CreateAt)
                         .ToList();
           return cate;
       }
    }
}
