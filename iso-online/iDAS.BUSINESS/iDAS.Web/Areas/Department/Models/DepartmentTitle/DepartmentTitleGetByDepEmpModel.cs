using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models
{
    public class DepartmentTitleGetByDepEmpModel
    {
        public int DepartmentTitleID { get; set; }
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        public string TitleCode { get; set; }
    }

    public class DepartmentTitleV3DTO
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Boolean IsSelect { get; set; }
        public int TitleID { get; set; }
        public string TitleName { get; set; }

        public DepartmentTitleV3DTO(int iD, int departmentID, string departmentName, bool isSelect, int titleID, string titleName)
        {
            ID = iD;
            DepartmentID = departmentID;
            DepartmentName = departmentName;
            IsSelect = isSelect;
            TitleID = titleID;
            TitleName = titleName;
        }
    }
    public class DepartmentTitleUserInsUpdModel
    {
        public int? ID { get; set; }
        public int DepartmentTitleID { get; set; }
        public int HumanEmployeeID { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public class DepartmentTitleUserDeleteReqModel
    {
        public int DepartmentTitleID { get; set; }
        public int HumanEmployeeID { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}