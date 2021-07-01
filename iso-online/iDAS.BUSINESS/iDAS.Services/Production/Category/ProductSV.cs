using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ProductSV
    {
        private StockProductDA ProductDA = new StockProductDA();
        private StockProductGroupDA ProductGroupDA = new StockProductGroupDA();
        public List<StockProductGroupItem> GetProductGroup(ModelFilter filter)
        {
            var lstProductGroup = ProductGroupDA.GetQuery(i => i.Active)
                .Select(item => new StockProductGroupItem
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    CreateAt = item.CreateAt
                })
                .Filter(filter)
                .ToList();
            return lstProductGroup;

        }
        public List<StockProductItem> GetProductByGroup(ModelFilter filter, int groupId)
        {
            var lstProductGroup = ProductDA.GetQuery(i => i.StockProductGroupID == groupId)
                .Select(item => new StockProductItem
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    CreateAt = item.CreateAt
                })
                .Filter(filter)
                .ToList();
            return lstProductGroup;

        }
    }
}
