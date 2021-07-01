using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SupplierItem
    {
        public int ID { get; set; }
        public int SuppliersGroupId { get; set; }
        public string SuppliersGroupName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Tax { get; set; }
        public string AcountNumber { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public string Representative { get; set; }
        public Nullable<double> TotalImportValue { get; set; }
        public Nullable<double> IsOwedOn { get; set; }
        public string Position { get; set; }
        public bool IsCustomer { get; set; }
        public Nullable<System.Guid> AttachmentFileID { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public FileItem ImageFile { get; set; }
        public string ImageUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageFile == null || ImageFile.Data == null || Convert.ToBase64String(ImageFile.Data) == "") return "";
                var data = Convert.ToBase64String(ImageFile.Data);
                url = string.Format(url, data);
                return url;
            }
        }
         public SupplierItem()
        {
            this.SuppliersOrders = new HashSet<SuppliersOrderItem>();
            this.SuppliersBidOrders = new HashSet<SuppliersBidOrderItem>();
        }
        public string Unit { get; set; }
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> TotalPayment { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Commodity { get; set; }

        public virtual ICollection<SuppliersOrderItem> SuppliersOrders { get; set; }
        public virtual ICollection<SuppliersBidOrderItem> SuppliersBidOrders { get; set; }

    }
    public class FieldItem
    {
        public string dataIndex { get; set; }
        public string text { get; set; }
        public bool hidden { get; set; }
        public int position { get; set; }        
    }
    public class FileExportItem
    {
        public string Title { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Sort { get; set; }
        public string SortName { get; set; }
        public int TypeExport { get; set; }
        List<FieldItem> lstFieldItem { get; set; }
        public string _source { get; set; }
        public List<string> lstParam { get; set; }
        public FileExportItem()
        {
            PageIndex = 10;
            PageSize = 1;
            Sort = "asc";
            SortName = "ID";
            TypeExport = 1;
            lstFieldItem = new List<FieldItem>();
            lstParam = new List<string>();
        }
        
    }
}
