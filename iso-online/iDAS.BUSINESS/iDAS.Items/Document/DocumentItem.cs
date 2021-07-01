using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DocumentItem : ApprovalItem
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public HumanDepartmentViewItem Department { get; set; }
        public int? CategoryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DocumentParent { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public bool FormH { get; set; }
        public bool FormS { get; set; }
        public string StorageFormID { get; set; }
        public string Security { get; set; }
        public int SecurityID { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string StatusApprove { get; set; }//Trạng thái Duyệt : Đạt hoặc không Đạt
        public bool IsDeleted { get; set; }
        public bool IsCancel { get; set; }
        public string ApproveNote { get; set; }//Trạng thái Duyệt : Đạt hoặc không Đạt
        public Nullable<int> CreateBy { get; set; }
        public int EmployeesCreateID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ObsoleteDate { get; set; }
        public int? IssuedBy { get; set; }
        public int? ObsoleteBy { get; set; }
        public string IssuedName { get; set; }
        public string ObsoleteName { get; set; }
        public string Position { get; set; }
        public string Note { get; set; }
        public int? IssuedTime { get; set; }//Lần ban hành
        public Nullable<System.DateTime> IssuedDate { get; set; }//Ngày ban hành
        public Nullable<System.DateTime> DistributionDate { get; set; }//Ngày ban hành
        public string Approver { get; set; }
        public HumanEmployeeViewItem EmployeeInfo { get; set; }
        public string Color { get; set; }
        public string StoreDepartment { get; set; }
        public string DateObsolete { get; set; }
        public bool IsPublic { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public int Number { get; set; }
        public string NoteIssues { get; set; }
        public string NoteObs { get; set; }
        public int NumberObsolete { get; set; }
        public bool IsObs { get; set; }
        public bool? FormSO { get; set; }
        public bool? FormHO { get; set; }
        public string IssueForm { get; set; }
        public string ObsoleteForm { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public bool HasRequest { get; set; }
        public bool FlagModified { get; set; }//Đánh dấu là TL Ban hành đã có TL sửa đổi
        public string DepartmentOfCategory { get; set; }
        public AttachmentFileItem AttachmentFile { get; set; }
        /// <summary>
        /// CuongPC
        /// Dung cho phat trien san pham moi
        /// </summary>
        public int ProductDevelopmentRequirementPlanID { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }

        public bool AllUserAccess { get; set; }
        public int? FolderID { get; set; }
        public string QuickDownloadLink { get; set; }
        public bool QuickDownload { get; set; }
        public DateTime? DayDownloadLimited { get; set; }
    }
    public enum RequestType
    {
        New = 0,
        Modified=1
    }
    public enum DocumentType
    {
        In = 0,//Nội bộ
        Out = 1//bên ngoàis
    }
    public enum DocumentForm
    {
        Insert = 0,
        Edit = 1,//Nội bộ
        Approve = 2,//bên ngoàis
        Issued=3,//Ban hành
        Distribution=7,
        Destroy=4,
        Modifield=5,
        Detail=6,
        AttachFile=8
    }
    public enum StorageForm
    {
        SoftCopy = 0,
        HardCopy = 1,
        Both=2
    }
}
