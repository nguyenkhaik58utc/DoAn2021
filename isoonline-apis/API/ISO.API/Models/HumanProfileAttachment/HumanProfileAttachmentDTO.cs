using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileAttachment
{
    public class HumanProfileAttachmentDTO
    {
        public int HumanEmployeeID { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFileSize { get; set; }
        public string Note { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public double FileSize { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }
}
