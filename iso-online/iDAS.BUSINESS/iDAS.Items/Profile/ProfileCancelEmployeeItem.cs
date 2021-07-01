using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProfileCancelEmployeeItem
    {
        public int ID { get; set; }
        public string Code { get; set; }//Ma HS
        public string Name { get; set; }//Ten HS
        public int CancelID { get; set; }//Ma HS
        public int EmployeeID { get; set; }//Ma HS
        public string DepartmentIDs { get; set; }//Ma HS
        public int? UpdateBy { get; set; }//
        public DateTime? UpdateAt { get; set; }//Ngay hủy
        public int? CreateBy { get; set; }//
        public DateTime? CreateAt { get; set; }//Ngay hủy
    }
}
