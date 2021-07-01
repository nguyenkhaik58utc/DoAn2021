using iDAS.Base;
using iDAS.Items;
using iDAS.Web.Areas.Department.Models.Title;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Timekeeping.Models
{
    public class WorkOutDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }
        public string StartTimeHour { get; set; }
        public string StartTimeMinitu { get; set; }
        public string Reason { get; set; }
        public string EndTimeHour { get; set; }
        public string EndTimeMinitu { get; set; }
        public int ApprovedBy { get; set; }
        public HumanEmployeeViewItem Perform { get; set; }

        // 1-Gửi duyệt
        // 2-Duyệt
        // 3-Ko duyệt
        public int? Status { get; set; }

        public bool IsDelete { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public int UpdateBy { get; set; }
    }

    public class WorkOutViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public List<DepartmentTitleResponse> ListDepartmentTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Reason { get; set; }

        public string ApprovedName { get; set; }
        public int Status { get; set; }

        public string StatusForView
        {
            get
            {
                if (Status == 0)
                {
                    return "Chờ duyệt";
                }
                if (Status == 1)
                {
                    return "Phê duyệt";
                }
                return "Từ chối";
            }
        }

        public string DepartmentTitleForView
        {
            get
            {
                if (ListDepartmentTitle.Count > 0)
                {
                    string strChucDanh = "";

                    foreach (DepartmentTitleResponse item in ListDepartmentTitle)
                    {
                        strChucDanh += item.TitleName + " -- " + item.DepartmentName + "</br>";
                    }
                    return strChucDanh;
                }
                return "";
            }
        }
    }

    public class GetWorkOutByMonthRequest
    {
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Status { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRows { get; set; }
    }

    public class InsertWorkOutRequest
    {
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }

        public string Reason { get; set; }

        public int ApprovedBy { get; set; }
    }

    public class UpdateWorkOutRequest
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }

        public string Reason { get; set; }

        public int ApprovedBy { get; set; }
    }

    public class CheckExistWorkOutRequest
    {
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class GetWorkOutByIdRequest
    {
        public int Id { get; set; }
    }

    public enum WorkOutStatus
    {
        Deny = 0,
        Approved = 1
    }
}