using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class V3_HumanProfileContractDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string NumberOfContracts { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int ContractTypeID { get; set; }
        public int ContractStatusID { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class V3_HumanProfileContractResponse
    {
        public int ID { get; set; }
        public string NumberOfContracts { get; set; }
        public string ContractTypeName { get; set; }
        public string ContractStatusName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
