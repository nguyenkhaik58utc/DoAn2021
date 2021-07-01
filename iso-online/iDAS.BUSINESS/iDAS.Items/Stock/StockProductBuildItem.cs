using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class StockProductBuildItem
    {
        public int ID { get; set; }
        public int StockProductID { get; set; }
        public string Code { get; set; }
        public string Product_Name { get; set; }
        public string Unit { get; set; }
        public string Group_Name { get; set; }
        public int Build_ID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
