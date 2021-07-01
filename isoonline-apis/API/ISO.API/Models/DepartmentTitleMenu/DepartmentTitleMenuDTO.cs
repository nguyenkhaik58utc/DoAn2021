using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class DepartmentTitleMenuDTO
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int DepartmentTitleID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
    public class DepartmentTitleMenuInsModel
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int DepartmentTitleID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int[] ArrayMenuID { get; set; }
        public string StrArrMenuID
        {
            get
            {
                return string.Join(",", ArrayMenuID);
            }
        }
    }
    public class DepartmentTitleMenuReqModel
    {
        public int DepartmentID { get; set; }
        public int TitleID { get; set; }
    }
}
