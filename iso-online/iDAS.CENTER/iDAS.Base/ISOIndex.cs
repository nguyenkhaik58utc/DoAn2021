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
    
    public partial class ISOIndex
    {
        public ISOIndex()
        {
            this.ISOIndexCriterias = new HashSet<ISOIndexCriteria>();
            this.ISOIndexModules = new HashSet<ISOIndexModule>();
            this.ISOIndexFunctions = new HashSet<ISOIndexFunction>();
        }
    
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int ISOStandardID { get; set; }
        public string Clause { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Note { get; set; }
        public bool IsUpdateCriteria { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<ISOIndexCriteria> ISOIndexCriterias { get; set; }
        public virtual ISOStandard ISOStandard { get; set; }
        public virtual ICollection<ISOIndexModule> ISOIndexModules { get; set; }
        public virtual ICollection<ISOIndexFunction> ISOIndexFunctions { get; set; }
    }
}
