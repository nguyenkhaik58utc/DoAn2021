using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
   
    public class ProfileItem
    {
        public string _AvatarUrl = "/Generic/File/LoadAvatarFile?employeeId={0}";
        public string _AvatarUrlDefault = "/Content/images/underfind.jpg";
        public int ID { get; set; }
        public int ReceivedDepartmentID { get; set; }
        public int CategoryID { get; set; }
        public int CategoryBorowID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ReceivedDepartment { get; set; }
        public int ReceivedBy { get; set; }
        public Nullable<System.DateTime> ReceivedAt { get; set; }
        public int? StoragePartTime { get; set; }
        public int? StorageRoomTime { get; set; }
        public string StorageRoomPosition { get; set; }
        public bool FormH { get; set; }
        public bool FormS { get; set; }
        public string StorageFormID { get; set; }
        public string Security { get; set; }
        public int SecurityID { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public Nullable<System.DateTime> ExpectedCancelDate { get; set; }
        public string Note { get; set; }
        public bool IsDelete { get; set; }
        public int? FileID { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public double FileSize { get; set; }
        public string Path { get; set; }
        public string Category { get; set; }
        public int? StoreRoomTime { get; set; }
        public int StorageFormInt { get; set; }
        public string ReceivedName { get; set; }
        public HumanEmployeeViewItem EmployeeInfo { get; set; }
        public string Position { get; set; }
        public bool FlagBorrow { get; set; }//Cờ đánh dấu được phép mượn: true/không mượn: false
        public AttachmentFileItem AttachmentFile { get; set; }
        public bool IsUse { get; set; }
        public Nullable<System.DateTime> NotUseAt { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
        public string Avatar
        {
            get
            {
                if (ReceivedBy == 0)
                    return _AvatarUrlDefault;
                return string.Format(_AvatarUrl, ReceivedBy);
            }
            set
            {
                _AvatarUrl = value;
            }
        }
    }
    public enum ProfileStatus
    {
        New = 11,//Tạo mới
        Borrow = 1,//Đang mượn
        Paid = 2,//Đã trả
        OverPaid = 3,//Quá hạn
        Destroy = 4,//Hủy
        PreDestroy=5,
    }
    public enum ProfileForm
    {
        Insert = 0,
        Edit = 1,//Nội bộ
        ChoseProfileCancel = 2,//bên ngoàis
        Borrow = 3,
        UpdateBorrow = 7,
        UpdateProfileCancel = 4,
        Paid = 5,
        Detail = 6,
        Destroy=8,
        BorrowByCate=9,
        UpdateDepartment=10,
        ListProfileCancel=11,
        BorrowCateBackup=12,
        Verify=13//Xác nhận HỦy hồ sơ
    }
}
