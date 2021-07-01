using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TaxCode { get; set; }
        public string RepresentName { get; set; }
        public string RepresentRole { get; set; }
        public CustomerCategorySelectItem CustomerCategory { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CustomerTypeID { get; set; }
        public string TypeName { get; set; }
        public bool IsPotential { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsBackContact { get; set; }
        public bool IsNotContract { get; set; }
        public Nullable<System.DateTime> LastContact { get; set; }
        public bool IsPotentialView { get; set; }
        public Nullable<System.DateTime> TimeNormal { get; set; }
        public Nullable<System.DateTime> TimePotential { get; set; }
        public Nullable<System.DateTime> TimeOfficial { get; set; }
        public string RequireContent { get; set; }
        public Nullable<System.DateTime> RequireTime { get; set; }
        public Nullable<System.DateTime> EstablishDate { get; set; }
        public string Scope { get; set; }
        public string Address { get; set; }
        public Nullable<System.Guid> AttachmentFileID { get; set; }
        public string FileName { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Status { get; set; }
        public string ActionForm { get; set; }
        public FileItem ImageFile { get; set; }
        public string EditFields { get; set; }
        public decimal EditDataRate { get; set; }
        public Nullable<int> SuccessRate { get; set; }
        public string ReasonNotContract { get; set; }
    }
    public class CustomerDetail
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}