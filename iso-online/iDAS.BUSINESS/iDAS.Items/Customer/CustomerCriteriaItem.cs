using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerCriteriaItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsRoot { get; set; }
        public string ActionForm { get; set; }

        public int MinPoint { get; set; }

        public int MaxPoint { get; set; }

        public decimal Factory { get; set; }

        public int CategoryID { get; set; }

        public string CategoryName { get; set; }
    }
}