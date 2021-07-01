using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class V3_HumanProfileTrainingDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int EducationTypeID { get; set; }
        public string Content { get; set; }
        public string Certificate { get; set; }
        public int EducationResultID { get; set; }
        public string Reviews { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class V3_HumanProfileTrainingResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EducationTypeName { get; set; }
        public string Content { get; set; }
        public string Certificate { get; set; }
        public string EducationResultName { get; set; }
        public string Reviews { get; set; }
    }

    public class HumanProfileEducationTrainingExcel
    {
        public int HumanEmployeeID { get; set; }
        public string NameEducationTraining { get; set; }
        public string NameEducationType { get; set; }
        public string Content { get; set; }
        public string Certificate { get; set; }
        public string NameEducationResult { get; set; }
    }
}
