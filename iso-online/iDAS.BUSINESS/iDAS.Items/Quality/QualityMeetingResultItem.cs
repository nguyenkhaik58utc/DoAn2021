using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityMeetingResultItem
    {
        public int ID { get; set; }
        public int MeetingID { get; set; }
        public string MeetingName { get; set; }
        public string DepartmentName { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public decimal Cost { get; set; }
        public string Content { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public AttachmentFileItem AttachmentFile { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
