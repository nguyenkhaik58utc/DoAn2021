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
    
    public partial class HumanProfileRelationship
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public Nullable<short> Age { get; set; }
        public Nullable<bool> IsMale { get; set; }
        public string Relationship { get; set; }
        public string Job { get; set; }
        public string PlaceOfJob { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
    
        public virtual HumanEmployee HumanEmployee { get; set; }
    }
}
