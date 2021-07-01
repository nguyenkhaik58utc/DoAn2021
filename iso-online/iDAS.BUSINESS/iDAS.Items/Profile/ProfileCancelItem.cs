using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProfileCancelItem
    {
        public int ID { get; set; }
        public string Code { get; set; }//Ma HS
        public string Name { get; set; }//Ten HS
        public int ProfileID { get; set; }//Ma HS
        public string DepartmentIDs { get; set; }//Ma HS
        public DateTime CancelAt { get; set; }//Ngay hủy
        //Thong tin file dinh kem
        public int TotalPage { get; set; }//Tổng số trang hủy
        public string Reason { get; set; }//Ma HS
        public double StorageTime { get; set; }//TG đã lưu Ho so
        public string StrStorageTime { get; set; }//TG luu tru Ho so
        public string Cancel { get; set; }//Biên bản hủy
        public int CancelID { get; set; }
        public string Status { get; set; }//Trạng thái
        public int StatusID { get; set; }//Trạng thai: ID trong bang genericStatus

        //Hinh thuc huy
        public int MethodID { get; set; }
        public string CancelMethod { get; set; }
        public bool IsCancel { get; set; }
        public bool FlagCancel { get; set; }//Cờ đánh dấu đã hủy /được phép hủy

        public int? UpdateBy { get; set; }//
        public DateTime? UpdateAt { get; set; }//Ngay hủy

        public int? CreateBy { get; set; }//
        public DateTime? CreateAt { get; set; }//Ngay hủy

        public string Note { get; set; }//Lý do hủy


        public string CreateByName { get; set; }

        public string UpdateByName { get; set; }

        public DateTime? ProfileNotUseAt { get; set; }
    }
}
