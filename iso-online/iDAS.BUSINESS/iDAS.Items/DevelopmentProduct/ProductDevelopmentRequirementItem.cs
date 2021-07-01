using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS;

namespace iDAS.Items
{
   public class ProductDevelopmentRequirementItem
    {

        public int ID { get; set; }
        public int StockProductID { get; set; }
        public string StockProductName { get; set; }
        public Nullable<int> DevelopmentFromProduct { get; set; }
        public string DevelopmentFromProductName { get; set; }
        public string Reason { get; set; }
        public string Contents { get; set; }
        public bool IsWork { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
    }
   
}
