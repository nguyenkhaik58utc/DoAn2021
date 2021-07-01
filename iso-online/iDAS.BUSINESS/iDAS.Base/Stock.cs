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
    
    public partial class Stock
    {
        public Stock()
        {
            this.StockInventoryDetails = new HashSet<StockInventoryDetail>();
            this.StockProducts = new HashSet<StockProduct>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobi { get; set; }
        public string Manager { get; set; }
        public byte[] ImageMap { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<StockInventoryDetail> StockInventoryDetails { get; set; }
        public virtual ICollection<StockProduct> StockProducts { get; set; }
    }
}