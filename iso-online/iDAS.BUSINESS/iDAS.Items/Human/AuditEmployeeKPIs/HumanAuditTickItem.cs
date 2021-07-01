using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanAuditTickItem
    {
        public HumanAuditTickItem()
        {
            lstHumanAuditTickResultBonusScores = new List<HumanAuditTickResultBonusScoreItem>();
        }
        public int ID { get; set; }
        public int HumanAuditGradationID { get; set; }
        public int HumanAuditDepartmentID { get; set; }
        public int HumanAuditGradationRoleID { get; set; }
        public string HumanAuditGradationRole { get; set; }
        public int HumanRoleID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public bool IsEmployeeAuditted { get; set; }
        public System.DateTime Time { get; set; }
        public Nullable<int> EmployeeAuditManagementID { get; set; }
        public string ManagementName { get; set; }
        public bool IsManagementAuditted { get; set; }
        public Nullable<int> EmployeeAuditLeaderID { get; set; }
        public bool IsLeaderAuditted { get; set; }
        public decimal EmployeeAuditResult { get; set; }
        public decimal EmployeeAuditManagementResult { get; set; }
        public decimal EmployeeAuditLeaderResult { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsPerform { get; set; }
        public FileItem ImageFile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageFile == null || ImageFile.Data == null || Convert.ToBase64String(ImageFile.Data) == "") return "";
                var data = Convert.ToBase64String(ImageFile.Data);
                url = string.Format(url, data);
                return url;
            }
        }
        public Guid? FileID { get; set; }
        public string FileName { get; set; }
        public decimal TotalEmployeeAuditResult { get; set; }
        public decimal TotalManagementAuditResult { get; set; }
        public decimal TotalLeaderAuditResult { get; set; }
        public string Comments { get; set; }
        public string LevelName { get; set; }
        public bool IsSave { get; set; }
        public int HumanAuditLevelID { get; set; }
        public HumanEmployeeViewItem EmployeeAudit { get; set; }
        public int HumanAuditLevelCategoryID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public decimal TotalPointBonus { get; set; }
        public decimal TotalPoint
        {
            get
            {
                return EmployeeAuditLeaderResult + TotalPointBonus;
            }
        }
        public List<HumanAuditTickResultBonusScoreItem> lstHumanAuditTickResultBonusScores { get; set; }
        public string HumanEmployeeName { get; set; }
        
    }
    public class ResultAudit
    {
        public decimal TotalEmployeeAuditResult { get; set; }
        public decimal TotalManagementAuditResult { get; set; }
        public decimal TotalLeaderAuditResult { get; set; }
    }
    public class ObjectAudit
    {
        public int GradationRoleID { get; set; }
        public int HumanRoleID { get; set; }
    }
}
