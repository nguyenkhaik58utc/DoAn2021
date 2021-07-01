using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CenterKPICriteriaItem
    {
        public int ID { get; set; }
        public int CenterKPICriteriaCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<int> Factory { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
