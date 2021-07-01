using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iDAS.Items
{
    public class AttachmentFileItem
    {
        public AttachmentFileItem(){
            Files = new List<Guid>();
            FileAttachments = new List<HttpPostedFileBase>();
        }
        public List<Guid> Files { get; set; }
        public string ListFile
        {
            get
            {
                var value = Files.Select(i => i.ToString()).Aggregate((current, next) => current + "," + next);
                return value;
            }
        }
        public List<Guid> FileRemoves
        {
            get
            {
                var value = iDAS.Utilities.Convert.ToListGuid(ListFileRemove);
                return value;
            }
        }
        public string ListFileRemove { get; set; }

        public List<HttpPostedFileBase> FileAttachments { get; set; }
    }
}
