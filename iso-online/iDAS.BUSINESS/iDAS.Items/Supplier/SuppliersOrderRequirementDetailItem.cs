using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SuppliersOrderRequirementDetailItem 
    {
        public int ID { get; set; }
        public Nullable<int> SuppliersOrderRequirementID { get; set; }
        public Nullable<int> StocksProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusDisp
        {
            get { return getStatus(); }
            set { }
        }

        private string getStatus()
        {
            string result = "";
            EStatus status = (EStatus)Enum.ToObject(typeof(EStatus), Status.HasValue ? Status.Value : 1);
            switch (status)
            {
                case EStatus.New: result = "<span style=\"color:green\">Mới</span>"; break;
                case EStatus.Fail: result = "<span style=\"color:red\">Không mua</span>"; break;               
                case EStatus.Success: result = "<span style=\"color:blue\">Đã mua</span>"; break;
                case EStatus.Wait: result = "<span style=\"color:#41519f\">Đang đặt</span>"; break;
                default: result = "<span style=\"color:#045fb8\">Mới</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            New = 1,
            Wait = 2,            
            Success = 4,
            Fail = 5
        }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string ProductCode
        {
            get
            {
                return StockProduct.Code;
            }
            set { }
        }
        public string ProductName
        {
            get
            {
                return StockProduct.Name;
            }
        }
        public string ProductUnitName
        {
            get
            {
                return StockProduct.Unit_Name;
            }
        }
    

        public virtual StockProductItem StockProduct { get; set; }
        public virtual SuppliersOrderRequirementItem SuppliersOrderRequirement { get; set; }
    }
}
