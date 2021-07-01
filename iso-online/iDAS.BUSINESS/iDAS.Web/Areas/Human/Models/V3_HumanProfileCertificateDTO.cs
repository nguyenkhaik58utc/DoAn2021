using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class V3_HumanProfileCertificateDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public int CertificateTypeID { get; set; }
        public int CertificateLevelID { get; set; }
        public string PlaceOfTraining { get; set; }
        public Nullable<System.DateTime> DateIssuance { get; set; }
        public Nullable<System.DateTime> DateExpiration { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

       
    }

    public class V3_HumanProfileCertificateResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CertificateTypeName { get; set; }
        public string CertificateLevelName { get; set; }
        public string PlaceOfTraining { get; set; }
        public Nullable<System.DateTime> DateIssuance { get; set; }
        public Nullable<System.DateTime> DateExpiration { get; set; }
    }

    public class HumanProfileCertificateExcel
    {
        public int HumanEmployeeID { get; set; }
        public string NameCertificate { get; set; }
        public string NameCertificateLevel { get; set; }
        public string NameCertificateType { get; set; }
        public string PlaceOfTraining { get; set; }
        public Nullable<System.DateTime> DateIssuance { get; set; }
        public Nullable<System.DateTime> DateExpiration { get; set; }
    }
}