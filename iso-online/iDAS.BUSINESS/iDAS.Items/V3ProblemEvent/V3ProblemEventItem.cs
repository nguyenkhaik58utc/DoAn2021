using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class V3ProblemEventItem
    {
        public int ID { get; set; }
        public string Code { get; set; }

        public AttachmentFileItem AttachmentFiles { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
    }
}
