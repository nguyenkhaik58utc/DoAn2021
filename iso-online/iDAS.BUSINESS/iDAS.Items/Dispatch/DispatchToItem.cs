using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DispatchToItem
    {

        public int ID { get; set; }
        public int? CategoryID { get; set; }
        public int? SecurityLevelID { get; set; }
        public int? UrgencyId { get; set; }
        public int MethodID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Compendia { get; set; }//ND trích dẫn
        public string NumberTo { get; set; }
        public bool FormH { get; set; }
        public bool FormS { get; set; }
        public string StorageFormID { get; set; }
        public DateTime Date { get; set; }
        public string SendFrom { get; set; }//Nơi gửi
        public string SendTo { get; set; }//Nơi nhận
        public string Category { get; set; }
        public string Security { get; set; }
        public string SecurityColor { get; set; }
        public string Urgency { get; set; }
        public string UrgencyColor { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string StatusApprove { get; set; }//Trạng thái Duyệt : Đạt hoặc không Đạt
        public bool IsMoved { get; set; }
        public bool IsVerify { get; set; }
        public bool FlagVerify { get; set; }//Đánh dấu đã xác nhận hết các Phòng ban
        public bool FlagRequired { get; set; }//Đánh dấu là Bắt buộc nếu Người chuyển không phải là   Văn thư
        public Nullable<DateTime> DateMoved { get; set; }
        public Nullable<DateTime> DateVerifyTime { get; set; }
        public string ApproveNote { get; set; }//Trạng thái Duyệt : Đạt hoặc không Đạt
        public string Note { get; set; }//Ghi chú của thông tin CV đến
        public string NoteVerify { get; set; }//Ghi chú khi Xác nhận CV
        public string NoteMove { get; set; }//Ghi chú chuyển Cv
        public HumanEmployeeViewItem UserReceive { get; set; }//Người nhận công văn
        public int DepartmentID { get; set; }//Phòng ban nhận công văn
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public string Type { get; set; }
        public string Department { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
        public int SendBy { get; set; }
    }

    public class DispatchToObjectItem
    {
        public int ID { get; set; }
        public int DispatchTo { get; set; }
        public int DispatchToType { get; set; }//Chuyển CV Nội bộ hay bên ngoài
        public int Type { get; set; }
        public int ObjectID { get; set; }
        public string Name { get; set; }
        public Boolean IsVerify { get; set; }
        public string Role { get; set; }
        public string NoteMove { get; set; }
        public string NoteVerify { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }//TG xác nhận
        public Nullable<int> UpdateBy { get; set; }//Người xác nhận
        public string UpdateName { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public string CreateName { get; set; }
    }
    public enum DispatchForm
    {
        Insert = 0,
        Edit = 1,//Nội bộ
        Approve = 2,//Phê duyệt
        Move = 3,//Chuyển CV
        MoveOut = 6,//Chuyển CV đi ra bên ngoài Tổ chức
        Verify = 4, // Xác nhận CV      
        Detail = 5
    }
    public enum DispatchObjectType
    {
        Department = 0,
        Employee = 1,//Nội bộ
    }
}
