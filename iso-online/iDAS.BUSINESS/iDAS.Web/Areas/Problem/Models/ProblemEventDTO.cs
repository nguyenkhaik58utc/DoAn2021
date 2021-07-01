using iDAS.Items;
using System;
using System.Collections.Generic;

namespace iDAS.Web.Areas.Problem.Models
{
    public class ProblemEventDTO
    {
        public int? ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ResolvedDepartmentID { get; set; }
        public int? EmergencyTypeID { get; set; }
        public string ProblemEmergencyName { get; set; }
        public int? ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public int? CriticalLevelID { get; set; }
        public string CriticalLevelName { get; set; }
        public int? ProblemGroupID { get; set; }
        public string ProblemGroupName { get; set; }
        public DateTime? OccuredDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string Propose { get; set; }
        public string Solution { get; set; }
        public string Result { get; set; }
        public int? StatusID { get; set; }
        public string ProblemStatusName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsProblemOrEvent { get { return true; } }
        public bool? IsDelete { get; set; }
        public string Reporter { get; set; }
        public string ReporterDepartment { get; set; }
        public string ReporterName { get; set; }
        public int Receiver { get; set; }// employeeID
        public string ReceiverName { get; set; }
        public string ContactNumber { get; set; }
        public string ReporterEmail{ get; set; }
        public bool? IsTemplate { get; set; }
        // Đơn vị sửa tin bài
        public int? RequestDepID { get; set; }
        // Cơ quan thường trú
        public int? ResidentAgencyID { get; set; }
        public int? ResidentAgencyGroupID { get; set; }
        // danh sách người xử lý
        public string Resolvers { get; set; }
        // danh sách phòng ban xử lý
        public string RelateDep { get; set; }
        // đơn vị sửa tin bài
        public string RequestDepName { get; set; }

        public bool? YourselfFix { get; set; }
        public bool? OnDuty { get; set; }
        public string OnDutyText { get; set; }

        public AttachmentFileItem AttachmentFiles { get; set; }

        public ProblemEventDTO()
        {
            
        }

        public string lng { get; set; }
        public string lat { get; set; }

        // Khởi tạo cho việc insert/update
        public ProblemEventDTO(ProblemEventDTO obj)
        {
            AttachmentFiles = null;

            this.ID = obj.ID;
            this.Code = obj.Code;
            this.Name = obj.Name;
            this.ResolvedDepartmentID = obj.ResolvedDepartmentID;
            this.EmergencyTypeID = obj.EmergencyTypeID;
            this.ProblemEmergencyName = obj.ProblemEmergencyName;
            this.ProblemTypeID = obj.ProblemTypeID;
            this.ProblemTypeName = obj.ProblemTypeName;
            this.CriticalLevelID = obj.CriticalLevelID;
            this.CriticalLevelName = obj.CriticalLevelName;
            this.ProblemGroupID = obj.ProblemGroupID;
            this.ProblemGroupName = obj.ProblemGroupName;
            this.OccuredDate = obj.OccuredDate;
            this.ResolvedDate = obj.ResolvedDate;
            this.Description = obj.Description;
            this.Reason = obj.Reason;
            this.Propose = obj.Propose;
            this.Solution = obj.Solution;
            this.Result = obj.Result;
            this.StatusID = obj.StatusID;
            this.ProblemStatusName = obj.ProblemStatusName;
            this.CreatedAt = obj.CreatedAt;
            this.CreatedBy = obj.CreatedBy;
            this.UpdatedAt = obj.UpdatedAt;
            this.UpdatedBy = obj.UpdatedBy;
            this.IsDelete = obj.IsDelete;
            this.Reporter = obj.Reporter;
            this.ReporterDepartment = obj.ReporterDepartment;
            this.ReporterName = obj.ReporterName;
            this.Receiver = obj.Receiver;
            this.ReceiverName = obj.ReceiverName;
            this.ContactNumber = obj.ContactNumber;
            this.IsTemplate = obj.IsTemplate;
            this.ReporterEmail = obj.ReporterEmail;
            this.RequestDepID = obj.RequestDepID;
            this.ResidentAgencyGroupID = obj.ResidentAgencyGroupID;
            this.ResidentAgencyID = obj.ResidentAgencyID;
            this.OnDuty = obj.OnDuty;
            this.YourselfFix = obj.YourselfFix;
            this.lng = obj.lng;
            this.lat = obj.lat;
        }
    }

    
}