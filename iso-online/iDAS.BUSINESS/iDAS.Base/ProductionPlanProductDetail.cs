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
    
    public partial class ProductionPlanProductDetail
    {
        public int ID { get; set; }
        public int ProductionPlanProductID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> HumanDepartmentID { get; set; }
        public int Quantity { get; set; }
        public int CalculatorQuantity { get; set; }
        public Nullable<float> Menday { get; set; }
        public Nullable<float> CalculatorMenday { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ProductionPlanProduct ProductionPlanProduct { get; set; }
    }
}
