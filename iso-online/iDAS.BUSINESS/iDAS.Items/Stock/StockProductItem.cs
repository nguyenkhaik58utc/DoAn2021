using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class StockProductItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEN { get; set; }
        public Nullable<int> Type_ID { get; set; }
        public string Type_Name { get; set; }
        public Nullable<int> Manufacturer_ID { get; set; }
        public Nullable<int> Model_ID { get; set; }
        public Nullable<int> Group_ID { get; set; }
        public string Group_Name { get; set; }
        public Nullable<int> Provider_ID { get; set; }
        public string Origin { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public string Unit_Name { get; set; }
        public string UnitConvert { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> Org_Price { get; set; }
        public Nullable<decimal> Sale_Price { get; set; }
        public Nullable<decimal> Retail_Price { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> CurrentCost { get; set; }
        public Nullable<decimal> AverageCost { get; set; }
        public Nullable<int> Warranty { get; set; }
        public Nullable<decimal> VAT_ID { get; set; }
        public Nullable<decimal> ImportTax_ID { get; set; }
        public Nullable<decimal> ExportTax_ID { get; set; }
        public Nullable<decimal> LuxuryTax_ID { get; set; }
        public string Customer_ID { get; set; }
        public string Customer_Name { get; set; }
        public Nullable<short> CostMethod { get; set; }
        public Nullable<decimal> MinStock { get; set; }
        public Nullable<decimal> MaxStock { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public bool Expiry { get; set; }
        public Nullable<decimal> LimitOrders { get; set; }
        public Nullable<decimal> ExpiryValue { get; set; }
        public bool Batch { get; set; }
        public Nullable<decimal> Depth { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Thickness { get; set; }
        public string Size { get; set; }
        public byte[] ImageProduct { get; set; }
        public Nullable<int> Currency_ID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<int> StockID { get; set; }
        public string Stock_Name { get; set; }
        public bool Serial { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ImageUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageProduct == null) return "/Content/images/underfind.jpg";
                var data = Convert.ToBase64String(ImageProduct);
                url = string.Format(url, data);
                return url;
            }
        }
    }
}
