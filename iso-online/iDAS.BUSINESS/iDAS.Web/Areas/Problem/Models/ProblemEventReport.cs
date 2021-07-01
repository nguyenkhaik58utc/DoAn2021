using System;

namespace iDAS.Web.Areas.Problem.Models
{
    // In báo cáo
    public class ProblemEventReport
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProblemEmergencyName { get; set; }
        public string ProblemTypeName { get; set; }
        public string CriticalLevelName { get; set; }
        public string ProblemGroupName { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string Propose { get; set; }
        public string Solution { get; set; }
        public string Result { get; set; }
        public string ProblemStatusName { get; set; }
        public string ReporterDepartment { get; set; }
        public string ReporterName { get; set; }
        public string ReceiverName { get; set; }
        public string ContactNumber { get; set; }
        public string ReporterEmail { get; set; }
        public string ToDay { get { return string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Today.ToString("dd"), DateTime.Today.ToString("MM"), DateTime.Today.ToString("yyyy")); } }
        public string OccuredDate { get; set; }
        public string ResolvedDate { get; set; }
        public string Resolvers { get; set; }
        public string RelateDep { get; set; }
        public string RequestDepName { get; set; }

        public string ReporterName1 { get; set; }
        public string ReportDate1 { get; set; }
        public string From1 { get; set; }
        public string To1 { get; set; }
        public string Content1 { get; set; }
        public int Percent1 { get; set; }

        public void getData(ProblemEventDTO objProblemEvent)
        {
            this.Code = objProblemEvent.Code;
            this.Name = objProblemEvent.Name;
            this.ContactNumber = objProblemEvent.ContactNumber;
            this.CriticalLevelName = objProblemEvent.CriticalLevelName;
            this.Description = objProblemEvent.Description;
            if (objProblemEvent.OccuredDate != null)
                this.OccuredDate = objProblemEvent.OccuredDate.Value.ToString("dd/MM/yyyy");
            this.ProblemEmergencyName = objProblemEvent.ProblemEmergencyName;
            this.ProblemGroupName = objProblemEvent.ProblemGroupName;
            this.ProblemStatusName = objProblemEvent.ProblemStatusName;
            this.ProblemTypeName = objProblemEvent.ProblemTypeName;
            this.Propose = objProblemEvent.Propose;
            this.Reason = objProblemEvent.Reason;
            this.ReceiverName = objProblemEvent.ReceiverName;
            this.ReporterDepartment = objProblemEvent.ReporterDepartment;
            this.ReporterEmail = objProblemEvent.ReporterEmail;
            this.ReporterName = objProblemEvent.ReporterName;
            if (objProblemEvent.ResolvedDate != null)
                this.ResolvedDate = objProblemEvent.ResolvedDate.Value.ToString("dd/MM/yyyy");
            this.Result = objProblemEvent.Result;
            this.Solution = objProblemEvent.Solution;
            this.Resolvers = objProblemEvent.Resolvers;
            this.RelateDep = objProblemEvent.RelateDep;
            if (!string.IsNullOrWhiteSpace(objProblemEvent.RequestDepName))
                this.ReporterDepartment = objProblemEvent.RequestDepName;
        }
    }
}