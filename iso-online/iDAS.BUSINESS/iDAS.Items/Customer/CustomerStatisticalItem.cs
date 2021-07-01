using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerStatisticalItem
    {
        private string _AvatarUrl = "/Generic/File/LoadAvatarFile?employeeId={0}";
        private string _AvatarUrlDefault = "/Content/images/underfind.jpg";

        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int UserID { get; set; }
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
                _AvatarUrl = value;
            }
        }
        public int CustomerNormal { get; set; }
        public int CustomerNormalContacts { get; set; }
        public int CustomerNormalNeed { get; set; }

        public int CustomerPotentialSendPrice { get; set; }
        public int CustomerPotentialSignContract { get; set; }

        public int CustomerOfficialContacts { get; set; }
        public int CustomerOfficialContract { get; set; }

        public int NumberContract { get; set; }
        public decimal TotalContract { get; set; }
        public int NumberContractSucess { get; set; }
        public decimal TotalContractSucess { get; set; }
        public string TimeFix { get; set; }
    }
}