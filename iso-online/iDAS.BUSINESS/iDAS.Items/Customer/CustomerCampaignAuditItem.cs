using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerCampaignAuditItem
    {
        public int ID { get; set; }
        public int CampainID { get; set; }
        public int AuditID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public System.DateTime? UpdateAt { get; set; }
    }

}