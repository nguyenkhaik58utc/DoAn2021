using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class LibraryContentItem
    {
        public int ID { get; set; }
        public int LibraryID { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public AttachmentFileItem Image { get; set; }
        public string VideoUrl { get; set; }
        public bool IsFirst { get; set; }
        public bool IsVideo { get; set; }
        public bool IsImage {
            get {
                return !IsVideo;
            }
            set {
                IsVideo = !value;
            }
        }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
