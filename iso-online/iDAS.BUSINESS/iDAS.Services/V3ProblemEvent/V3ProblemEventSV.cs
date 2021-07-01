using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;

namespace iDAS.Services
{
    public class V3ProblemEventSV
    {
        private V3ProblemEventDA problemEventDA = new V3ProblemEventDA();
        //private V3ProblemAttachmentFileDA problemAttachmentFile = new V3ProblemAttachmentFile();


        public V3ProblemEventItem GetById(int Id)
        {
            var dbo = problemEventDA.Repository;
            var ProblemEventItem = dbo.V3ProblemEvent.Where(i => i.ID == Id)
                        .Select(item => new V3ProblemEventItem()
                        {
                            AttachmentFiles = new AttachmentFileItem()
                            {
                                Files = dbo.V3ProblemAttachmentFiles.Where(i => i.ProblemEventID == Id)
                                    .Select(i => i.AttachmentFileID).ToList()
                            }
                        })
                        .First();
            return ProblemEventItem;
        }

        public AttachmentFileItem getAttachFile(int problemEventID)
        {
            var dbo = problemEventDA.Repository;
            var AttachmentFiles = new AttachmentFileItem()
            {
                Files = dbo.V3ProblemAttachmentFiles.Where(i => i.ProblemEventID == problemEventID)
                                    .Select(i => i.AttachmentFileID).ToList()
            };
            return AttachmentFiles;

        }
    }
}
