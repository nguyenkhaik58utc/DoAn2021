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
    
    public partial class EquipmentProductionMaintain
    {
        public int ID { get; set; }
        public int EquipmentProductionID { get; set; }
        public Nullable<int> HumanDepartmentID { get; set; }
        public Nullable<System.DateTime> PlanTime { get; set; }
        public Nullable<System.DateTime> RealTime { get; set; }
        public bool IsComplete { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual EquipmentProduction EquipmentProduction { get; set; }
        public virtual HumanDepartment HumanDepartment { get; set; }
    }
}