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
    
    public partial class CalendarCategory
    {
        public CalendarCategory()
        {
            this.CalendarOverrides = new HashSet<CalendarOverride>();
        }
    
        public int ID { get; set; }
        public Nullable<int> HumanDepartmentID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual HumanDepartment HumanDepartment { get; set; }
        public virtual ICollection<CalendarOverride> CalendarOverrides { get; set; }
    }
}