using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemEventListDTO
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public DateTime? OccuredDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public int? StatusID { get; set; }
        public string ProblemStatusName { get; set; }
        public string UserResolve { get; set; }
        public string CriticalLevelName { get; set; }
        public string ProblemEmergencyName { get; set; }
        public string ProblemGroupName { get; set; }
        public string ReporterName { get; set; }
        public string ReceiverName { get; set; }
        public string RelateDep { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        
        // hiển thị màu sắc
        public double MinuteCheck
        {
            get
            {
                if(OccuredDate != null)
                {
                    TimeSpan ts = DateTime.Now.Subtract(OccuredDate.Value);
                    return ts.TotalMinutes;
                }
                return 0;
            }
        }
    }
}