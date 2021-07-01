using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public partial class SuppliersOrderItem
    {
        public SuppliersOrderItem()
        {
            this.SuppliersBills = new HashSet<SuppliersBillItem>();
            this.SuppliersOrderDetails = new HashSet<SuppliersOrderDetailItem>();
            this.SuppliersBidOrders = new HashSet<SuppliersBidOrderItem>();
        }

        public int ID { get; set; }
        public string CODE { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public Nullable<System.DateTime> ReciepDate { get; set; }
        public string ReciepDateDisp { 
            get {
                if (ReciepDate.HasValue && ShipDate.HasValue)
                    return ReciepDate.Value > ShipDate.Value ? "<span style=\"color:red\">" + ReciepDate.Value.ToString("dd/MM/yyyy") + "</span>" : "<span style=\"color:green\">" + ReciepDate.Value.ToString("dd/MM/yyyy") + "</span>";
                else
                    return "";
            }
            set { }
        }
        public Nullable<byte> Payment { get; set; }
        public string PaymentDisp
        {
            get
            {
                switch (Payment)
                {
                    case 1: return "Trả trước";
                    case 2: return "Chốt sổ";
                    default: return "";
                }
            }
            set { }
        }
        public string ReciepPlace { get; set; }
        public Nullable<int> Status { get; set; }
        public string SupplierName
        {
            get;
            set;
        }
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
                case EStatus.New: result = "<span style=\"color:#41519f\">Mới</span>"; break;                
                case EStatus.Bid: result = "<span style=\"color:orange\">Đang chờ báo giá </span>"; break;
                case EStatus.Signing: result = "<span style=\"color:red\">Đang tạo HĐ</span>"; break;
                case EStatus.Signed: result = "<span style=\"color:blue\">Đã ký HĐ</span>"; break;
                case EStatus.Done: result = "<span style=\"color:green\">Hoàn thành</span>"; break;
                case EStatus.Cancel: result = "<span style=\"color:pink\">Hủy</span>"; break;
                default: result = "<span style=\"color:#045fb8\">Mới</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            New = 4,            
            Bid = 6,
            Signing = 7,
            Signed = 8,
            Done = 9,
            Cancel = 10
        }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string ShipType { get; set; }
        public decimal Payed { 
            get {
                decimal rs = 0;
                foreach(SuppliersBillItem suppBill in SuppliersBills)
                {
                    rs += suppBill.PayedMoney.HasValue?suppBill.PayedMoney.Value:0;
                }
                return rs;
            }
            set { }
        }
        public decimal AmountRecive
        {
            get
            {
                decimal rs = 0;
                foreach (SuppliersOrderDetailItem suppBill in SuppliersOrderDetails)
                {
                    rs += suppBill.ReciveQuantity.HasValue ? suppBill.ReciveQuantity.Value * suppBill.Price.Value : 0;
                }
                if (!Discount.HasValue) Discount = 0;
                return rs-(rs*Discount.Value/100);
            }
            set { }
        }
        public decimal Owe { get { return Amount.HasValue ? (AmountRecive - Payed) : 0; } set { } }
        public HumanEmployeeViewItem CreateUser { get; set; }
        public virtual SupplierItem Supplier { get; set; }
        public virtual ICollection<SuppliersBillItem> SuppliersBills { get; set; }
        public virtual ICollection<SuppliersOrderDetailItem> SuppliersOrderDetails { get; set; }
        public virtual ICollection<SuppliersBidOrderItem> SuppliersBidOrders { get; set; }
    }
}
