using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerAssessObjectItem
    {
        public int? ID { get; set; }
        public int AuditID { get; set; }
        public int CustomerCategoryID { get; set; }
        public string Name { get; set; }
        public bool IsSelect { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
    }

}