using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SuppliersGroupItem
    {
        public SuppliersGroupItem()
        {
            this.Suppliers = new HashSet<SupplierItem>();            
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> Active { get; set; }
        public bool IsParent { get; set; }
        public int ParentID { get; set; }
        public bool Leaf { get; set; }

        public virtual ICollection<SupplierItem> Suppliers { get; set; }
        
    }
}
