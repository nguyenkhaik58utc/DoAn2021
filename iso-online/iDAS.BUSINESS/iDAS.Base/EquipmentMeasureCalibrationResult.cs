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
    
    public partial class EquipmentMeasureCalibrationResult
    {
        public int ID { get; set; }
        public int EquipmentMeasureCalibrationID { get; set; }
        public bool IsPass { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual EquipmentMeasureCalibration EquipmentMeasureCalibration { get; set; }
    }
}