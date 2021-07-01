using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class TaskDTO
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string HumanDepartmentID { get; set; }
        public string HumanEmployeeID { get; set; }
        public string TaskCategoryID { get; set; }
        public string TaskLevelID { get; set; }
        public string AuditID { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string IsPrivate { get; set; }
        public string IsNew { get; set; }
        public string IsEdit { get; set; }
        public string IsComplete { get; set; }
        public string IsPass { get; set; }
        public string IsPause { get; set; }
        public string Rate { get; set; }
        public string Cost { get; set; }
        public string CompleteTime { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public string CreateAt { get; set; }
        public string CreateBy { get; set; }
        public string UpdateAt { get; set; }
        public string UpdateBy { get; set; }
        public string TaskType { get; set; }
        public string RefID { get; set; }
    }
    public class TaskReqModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
