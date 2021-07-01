using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SuppliersOrderDetailItem
    {
        public int ID { get; set; }
        public Nullable<int> SuppliersOrderID { get; set; }
        public Nullable<int> StocksProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Note { get; set; }
        public Nullable<int> ReciveQuantity { get; set; }
        public Nullable<int> ReciveQuality { get; set; }
        public Nullable<int> SuppliersOrderRequirementDetailID { get; set; }
        public decimal Amount
        {
            get
            {
                if (Quantity.HasValue && Price.HasValue)
                    return Quantity.Value * Price.Value;
                else
                    return 0;
            }
            set { }
        }

        public string ProductCode
        {
            get
            {
                if (StockProduct != null)
                    return StockProduct.Code;
                else
                    return "";
            }
            set { }
        }
        public string ProductName
        {
            get
            {
                if (StockProduct != null)
                    return StockProduct.Name;
                else
                    return "";
            }
        }
        public string ProductUnitName
        {
            get
            {
                if (StockProduct != null)
                    return StockProduct.Unit_Name;
                else
                    return "";
            }
        }
        public Nullable<decimal> ProductRetail_Price
        {
            get
            {
                if (StockProduct != null)
                    return StockProduct.Retail_Price.HasValue ? StockProduct.Retail_Price.Value : 0;
                else
                    return null;
            }
        }
        public virtual StockProductItem StockProduct { get; set; }
        public virtual SuppliersOrderItem SuppliersOrder { get; set; }
    }
}
