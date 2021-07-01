using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class AttachmentItem
    {
        public int ID { get; set; }
        public int ReferenceID { get; set; }//ID của bảng tham chiếu đến bảng genericAtt...
        //public int DocumentID { get; set; }//
        public string Code { get; set; }//Mã 
        public string FileName { get; set; }//Ten sinh ngau nhien cua file dinh kem
        public string OriFileName { get; set; }//Ten goc cua file dinh kem

        public double FileSize { get; set; }
        public string FilePath { get; set; }

        public string FileFormat { get; set; }
        public string FileType { get; set; }
        public int Note { get; set; }

        public string FileNewName { get; set; }
        public string Extention { get; set; }
        public string Picture { get; set; }
        public string OriginalFileName { get; set; }//Ten goc cua file dinh kem

        public int CreateBy { get; set; }//Người đính kèm
        public string CreateName { get; set; }//
        public DateTime CreateAt { get; set; }//Thời gian đính kèm

        public string UpdateName { get; set; }
        public int UpdateBy { get; set; }//Thời gian xóa
        public DateTime UpdateAt { get; set; }//Thời gian xóa
    }
}
