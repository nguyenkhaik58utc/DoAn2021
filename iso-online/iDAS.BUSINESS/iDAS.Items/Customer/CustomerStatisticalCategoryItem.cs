using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerStatisticalCategoryItem
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? ParentID { get; set; }
        public string CateParent
        {
            get;
            set;
        }
        public int CustomerNormal { get; set; }
        public int CustomerNormalContacts { get; set; }
        public int CustomerNormalNeed { get; set; }

        public int CustomerPotential { get; set; }
        public int CustomerPotentialSendPrice { get; set; }
        public int CustomerPotentialSignContract { get; set; }
        public int CustomerNotContract { get; set; }
        public string CustomerPotentialRateSuccess
        {
            get
            {
                return CustomerPotentialSendPrice == 0 ? "N/A" : (CustomerPotentialSignContract * 100 / CustomerPotentialSendPrice).ToString();
            }
        }

        public int CustomerOfficial { get; set; }
        public int CustomerOfficialContacts { get; set; }
        public int CustomerOfficialHasPotential { get; set; }
        public int CustomerOfficialContract { get; set; }

        public int NumberContract { get; set; }
        public decimal TotalContract { get; set; }
        public int NumberContractSucess { get; set; }
        public decimal TotalContractSucess { get; set; }
        public int SumCustomer { get; set; }
        public bool Leaf { get; set; }
        public string TimeFix { get; set; }
    }
}