using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ServicePlanItem : QualityPlanItem 
    {
        public int ServiceCommandContractID { get; set; }
        public int ServicePlanID { get; set; }
        public int CommandID { get; set; }
        public int ServiceID { get; set; }
        public int CustomerID { get; set; }  
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string CommandServiceName { get; set; }
        public decimal RateFinish { get; set; }
        public Nullable<int> CreateByEmployeeID { get; set; }
        public Nullable<int> UpdateByEmployeeID { get; set; }

    }
}
