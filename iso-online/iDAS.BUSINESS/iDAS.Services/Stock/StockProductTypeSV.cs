using iDAS.Base;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Services
{
    public class StockProductTypeSV
    {
        private StockProductTypeDA product_TypeDA;
        public string GetNameTypeById(int id)
        {
            product_TypeDA = new StockProductTypeDA();
            var rs = product_TypeDA.GetById(id);
            return rs.Name;
        }
    }
}
