//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iDAS.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentCategory
    {
        public DocumentCategory()
        {
            this.Documents = new HashSet<Document>();
        }
    
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<Document> Documents { get; set; }
    }
}
