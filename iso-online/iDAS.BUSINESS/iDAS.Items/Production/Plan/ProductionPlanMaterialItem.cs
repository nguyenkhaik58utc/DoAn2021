using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionPlanMaterialItem
    {
        public int ID { get; set; }
        public int ProductionPlanID { get; set; }
        public int StockMaterialID { get; set; }
        public float Quantity { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public string ActionForm { get; set; }

        public string MaterialName { get; set; }
    }
}
