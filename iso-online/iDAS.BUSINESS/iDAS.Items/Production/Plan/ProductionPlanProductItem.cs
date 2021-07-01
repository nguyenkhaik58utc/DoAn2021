using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionPlanProductItem
    {
        public int ID { get; set; }
        public ProductViewItem Product { get; set; }
        public int ProductionPlanID { get; set; }
        public string StageName { get; set; }
        public ProductViewItem ProductResult { get; set; }
        public float QuantityCalculator { get; set; }
        public int QuantityStock { get; set; }
        public Nullable<int> Quantity { get; set; }
        public System.DateTime? StartTime { get; set; }
        public System.DateTime? EndTime { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public int StageID { get; set; }
    }
}
