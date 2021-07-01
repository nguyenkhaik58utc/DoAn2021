using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class QualityMeetingObjectItem
    {
        private string _AvatarUrl = "/Generic/File/LoadAvatarFile?employeeId={0}";
        private string _AvatarUrlDefault = "/Content/images/underfind.jpg";
        private bool _IsMeeting = true;
        public int? ID { get; set; }
        public int MeetingID { get; set; }
        public bool IsMeeting
        {
            get
            {
                return _IsMeeting;
            }
            set
            {
                _IsMeeting = value;
            }
        }
        public int EmployeeID { get; set; }
        public string Avatar
        {
            get
            {
                if (EmployeeID == 0)
                    return _AvatarUrlDefault;
                return string.Format(_AvatarUrl, EmployeeID);
            }
            set
            { 
            }
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public List<HumanGroupPositionItem> lstHumanGrPos { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
