using ISO.API.Models.DepartmentTitle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.WorkOut
{
    public class WorkOutDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }

        public string Reason { get; set; }

        public int ApprovedBy { get; set; }
        public int Status { get; set; }
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