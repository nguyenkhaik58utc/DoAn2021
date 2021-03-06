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
    
    public partial class ProductionPerform
    {
        public ProductionPerform()
        {
            this.ProductionPerformHistories = new HashSet<ProductionPerformHistory>();
            this.ProductionPerformMaterias = new HashSet<ProductionPerformMateria>();
            this.ProductionPerformProductErrors = new HashSet<ProductionPerformProductError>();
            this.ProductionPerformResults = new HashSet<ProductionPerformResult>();
        }
    
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public int HumanDepartmentID { get; set; }
        public int ProductionCommandID { get; set; }
        public int ProductionShiftID { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ProductionCommand ProductionCommand { get; set; }
        public virtual ICollection<ProductionPerformHistory> ProductionPerformHistories { get; set; }
        public virtual ICollection<ProductionPerformMateria> ProductionPerformMaterias { get; set; }
        public virtual ICollection<ProductionPerformProductError> ProductionPerformProductErrors { get; set; }
        public virtual ICollection<ProductionPerformResult> ProductionPerformResults { get; set; }
        public virtual ProductionShift ProductionShift { get; set; }
    }
}
