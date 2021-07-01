using iDAS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Items
{
    public class CustomerItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Role { get; set; }
        public string Represent { get; set; }
        public string Scope { get; set; }
        public string CompanySize { get; set; }
        public string TaxCode { get; set; }
        public string Website { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; }
        public Nullable<int> WebCustomerCityID { get; set; }
        public bool IsPersonal { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsNew { get; set; }
        public Nullable<System.DateTime> RegisterAt { get; set; }
        public Nullable<System.DateTime> RefreshAt { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
