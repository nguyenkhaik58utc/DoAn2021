using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class V3_HumanDashboardDTO
    {
        public string Name { get; set; }
        public int Data1 { get; set; }

        public V3_HumanDashboardDTO()
        {
        }

        public V3_HumanDashboardDTO(string name, int data1)
        {
            Name = name;
            Data1 = data1;
        }
    }

    public class V3_HumanDashboardDTO2
    {
        public string Name2 { get; set; }
        public int Data2 { get; set; }

        public V3_HumanDashboardDTO2()
        {
        }

        public V3_HumanDashboardDTO2(string name, int data1)
        {
            Name2 = name;
            Data2 = data1;
        }
    }
    public class DashboardDTO
    {
        public string Name { get; set; }

    }

    public class ChartDTO : DashboardDTO
    {
        public int Amount { get; set; }
    }

    public class RewardDTO : DashboardDTO
    {
        public string Content { get; set; }
    }

    public class NewEmployeeDTO : DashboardDTO
    {
        public string DepartmentName { get; set; }
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