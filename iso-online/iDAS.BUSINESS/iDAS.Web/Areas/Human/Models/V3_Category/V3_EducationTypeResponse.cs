using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models.V3_Category
{
    public class V3_EducationTypeResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class V3_EducationFieldResponse : V3_EducationTypeResponse
    {
    }

    public class V3_EducationOrgResponse : V3_EducationTypeResponse
    {

    }

    public class V3_EducationLevelResponse : V3_EducationTypeResponse
    {

    }

    public class V3_EducationResultResponse : V3_EducationTypeResponse
    {

    }
    
}
