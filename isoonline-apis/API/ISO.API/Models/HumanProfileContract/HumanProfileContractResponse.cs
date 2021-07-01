using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileContract
{
    public class HumanProfileContractResponse
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string NumberOfContracts { get; set; }
        public string ContractTypeName { get; set; }
        public string ContractStatusName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
