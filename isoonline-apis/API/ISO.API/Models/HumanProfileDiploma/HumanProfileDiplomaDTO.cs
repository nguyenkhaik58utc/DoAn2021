using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileDiploma
{
    public class HumanProfileDiplomaDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public int EducationFieldID { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int EducationLevelID { get; set; }
        public int EducationTypeID { get; set; }
        public int CertificateLevelID { get; set; }
        public int EducationOrgID { get; set; }
        public string Condition { get; set; }
        public Nullable<DateTime> DateOfGraduation { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class HumanProfileDiplomaResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string EducationFieldName { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string EducationLevelName { get; set; }
        public string EducationTypeName { get; set; }
        public string CertificateLevelName { get; set; }
        public string EducationOrgName { get; set; }
        public string Condition { get; set; }
        public Nullable<DateTime> DateOfGraduation { get; set; }
    }
    public class HumanProfileDiplomaExcel
    {
        public int HumanEmployeeID { get; set; }
        public string NameDiploma { get; set; }
        public string Faculty { get; set; }
        public string NameEducationField { get; set; }
        public string NameEducationOrg { get; set; }
        public string NameEducationLevel { get; set; }
    }
}
