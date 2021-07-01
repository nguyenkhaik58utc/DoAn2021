using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanEmployeeViewItem
    {
        private string _AvatarUrl = "/Generic/File/LoadAvatarFile?employeeId={0}";
        private string _AvatarUrlDefault = "/Content/images/underfind.jpg";
        public int ID { get; set; }
        public string Avatar
        {
            get
            {
                if (ID == 0)
                    return _AvatarUrlDefault;
                return string.Format(_AvatarUrl, ID);
            }
        }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
    }
}
