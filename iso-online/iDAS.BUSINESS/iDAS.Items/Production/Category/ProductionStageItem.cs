using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionStageItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ProductViewItem Product { get; set; }
        public ProductViewItem ResultProduct { get; set; }
        public int Quantity { get; set; }
        public Nullable<float> ResultQuantity { get; set; }
        public Nullable<float> MenDay { get; set; }
        public string quotaStr
        {
            get
            {
                string qualitystr = ResultQuantity == null ? "" : ResultQuantity + " sản phẩm";
                string menDaystr = MenDay == null ? "" : MenDay + " Ngày công";
                return (string.IsNullOrEmpty(menDaystr) && string.IsNullOrEmpty(qualitystr)) ? "" : (qualitystr + "/" + menDaystr);
            }
        }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int Position { get; set; }
        public string Note { get; set; }
        public List<ProductionStageEquipmentItem> Equipements { get; set; }
        public List<ProductionStageMaterialItem> Materials { get; set; }
        public List<ProductionStageProductItem> Products { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ActionForm { get; set; }


    }
}
