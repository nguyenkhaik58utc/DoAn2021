using ISO.API.Models.DepartmentTitle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanDashboard
{
    public class DashboardDTO
    {
        public string Name { get; set; }
       
    }

    public class ChartDTO : DashboardDTO
    {
        public string Amount { get; set; }
    }

    public class EmpployeeTotalDTO
    {
        public int Total { get; set; }
    }

    public class RewardDTO : DashboardDTO
    {
        public string Content { get; set; }
    }

    public class NewEmployeeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public List<DepartmentTitleResponse> ListDepartmentTitle { get; set; }
    }

    public class AlmostExpireEmployeeDTO : DashboardDTO
    {
        public Nullable<DateTime> ExpireDate { get; set; }
    }

    public class RecruitmentDTO : DashboardDTO
    {
        public int ID { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public string PlanName { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
    }
    public class TrainingDTO : DashboardDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public string PlanName { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
    }

    public class EmployeeByDepartmentDTO : DashboardDTO
    {
        public string Amount { get; set; }
    }
}
