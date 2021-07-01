using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   public class SystemTablePriceItem
    {
        public int ID { get; set; }
        public int CenterSystemID { get; set; }
        public string CenterSystemName { get; set; }
        public int SystemFormID { get; set; }
        public string FormName { get; set; }
        public int ISOID { get; set; }
        public string ISOName { get; set; }
        public int ISONaceCodeID { get; set; }
        public string ISONaceCodeName { get; set; }
        public float FactoryNaceCode { get; set; }
        public decimal Price { get; set; }
        public decimal PriceActive { get; set; }
        public int PriceDataSizeID { get; set; }
        public string PriceDataSizeName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PriceOfYearNext { get; set; }
        public string Description { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

        public string OrderName { get; set; }
    }
}
