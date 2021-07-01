using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionStageProductItem
    {
        public int ID { get; set; }
        public int ProductionStageID { get; set; }
        public int StockProductID { get; set; }
        public string ProductName { get; set; }
        public ProductViewItem ProductView
        {
            get
            {
                return new ProductViewItem()
                {
                    ID = StockProductID,
                    Name = ProductName
                };
            }
        }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
