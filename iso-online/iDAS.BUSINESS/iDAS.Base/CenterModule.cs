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
    
    public partial class CenterModule
    {
        public CenterModule()
        {
            this.CustomerSystemModules = new HashSet<CustomerSystemModule>();
            this.ISOStandardModules = new HashSet<ISOStandardModule>();
            this.ISOIndexModules = new HashSet<ISOIndexModule>();
        }
    
        public int ID { get; set; }
        public string SystemCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
        public int Position { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual ICollection<CustomerSystemModule> CustomerSystemModules { get; set; }
        public virtual ICollection<ISOStandardModule> ISOStandardModules { get; set; }
        public virtual ICollection<ISOIndexModule> ISOIndexModules { get; set; }
    }
}
