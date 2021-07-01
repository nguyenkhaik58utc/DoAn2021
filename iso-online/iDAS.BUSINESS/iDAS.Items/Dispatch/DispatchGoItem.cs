using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DispatchGoItem : ApprovalItem
    {
        public int ID { get; set; }
        public int? CategoryID { get; set; }
        public int? DisparchGoOutID { get; set; }
        public int? SecurityLevelID { get; set; }
        public int? UrgencyId { get; set; }
        public int? MethodID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Compendia { get; set; }//ND trích dẫn
        public bool Type { get; set; }//Chuyển CV đi false: Nội bộ; True: Bên ngoài
        public String StrType { get; set; }//Chuyển CV đi false: Nội bộ; True: Bên ngoài
        public bool FormH { get; set; }
        public bool FormS { get; set; }
        public string StorageFormID { get; set; }
        public DateTime Date { get; set; }
        public string DestinationAddress { get; set; }//ĐC Nơi nhận CV
        public string Address { get; set; }//ĐC Nơi nhận CV
        public string ToName { get; set; }//ĐC Nơi nhận CV
        public string ToPosition { get; set; }//ĐC Nơi nhận CV
        public string ToCompany { get; set; }//ĐC Nơi nhận CV
        public string ToGroup { get; set; }//ĐC Nơi nhận CV
        public HumanEmployeeViewItem UserReceive { get; set; }//Người nhận công văn
        public int DepartmentID { get; set; }//Phòng ban nhận công văn
        public string NoteVerify { get; set; }//Ghi chú khi Xác nhận CV
        public Nullable<DateTime> DateVerifyTime { get; set; }
        public string Security { get; set; }
        public string SecurityColor { get; set; }
        public string Urgency { get; set; }
        public string UrgencyColor { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string StatusApprove { get; set; }//Trạng thái Duyệt : Đạt hoặc không Đạt
        public bool IsSend { get; set; }//Đã chuyển CV
        public bool? IsVerify { get; set; }// Đã xác nhận
        public Nullable<System.DateTime> MoveAt { get; set; }//TG chuyển công văn
        public Nullable<int> VerifyAt { get; set; }
        public string Note { get; set; }//Ghi chú của thông tin CV đến
        public string NoteMove { get; set; }//Ghi chú khi Xác nhận CV
        public HumanEmployeeViewItem EmployeeInfo { get; set; }
        public HumanEmployeeViewItem Create { get; set; }
        //  public Nullable<int> ApproveBy { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public int EmployeeCreate { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public string Category { get; set; }
        public bool FlagApprove { get; set; }
        public bool FlagVerify { get; set; }
        public bool AlowNotApproval { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
    }

    public class DispatchGoObjectItem
    {
        public int ID { get; set; }//
        public int SendBy { get; set; }//
        public int ObjectID { get; set; }
        public int DispatchGo { get; set; }
        public int DispatchGoType { get; set; }
        public int Type { get; set; }//Loại Tổ chức : Cá nhân
        public string StrType { get; set; }//Loại Tổ chức : Cá nhân
        public string TypeCode { get; set; }//Loại Tổ chức : Cá nhân
        public string Name { get; set; }//Người nhận - người nhận cv của tổ chức
        public Boolean IsVerify { get; set; }
        public string Position { get; set; }//Chức vụ
        public string Company { get; set; }//Công ty
        public string Address { get; set; }
        public string TelCompany { get; set; }//Số đt Công ty
        public string EmailCompany { get; set; }
        public string TelPerson { get; set; }//Số đt cá nhân
        public string EmailPerson { get; set; }// email cá nhân
        public string NoteMove { get; set; }
        public string NoteVerify { get; set; }
        public HumanEmployeeViewItem UserReceive { get; set; }
        public Nullable<System.DateTime> Date { get; set; }//TG chuyển cv
        public Nullable<System.DateTime> DateVerify { get; set; }//TG nhận cv
        public Nullable<System.DateTime> UpdateAt { get; set; }//TG xác nhận
        public Nullable<int> UpdateBy { get; set; }//Người xác nhận
        public string UpdateName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public string CreateName { get; set; }
    }
    public enum DispatchGoType
    {
        InSide = 0,//Chuyển CV đi trong nội bộ
        OutSide = 1 //Chuyển CV đi ra bên ngoài
    }
}
