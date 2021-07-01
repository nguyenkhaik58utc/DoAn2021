using iDAS.Base;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class DocumentAttachmentFileSV
    {
        private DocumentAttachmentFileDA documentAttachmentFileDA = new DocumentAttachmentFileDA();
        public void InsertRange(List<Guid> attachmentFileIDs, int documentID, int userID)
        {
            foreach (var attachmentFileID in attachmentFileIDs)
            {
                var attachmentFile = new DocumentAttachmentFile()
                        {
                            AttachmentFileID = attachmentFileID,
                            DocumentID = documentID,
                            CreateAt = DateTime.Now,
                            CreateBy = userID,
                        };
                documentAttachmentFileDA.Insert(attachmentFile);
            }
            documentAttachmentFileDA.Save();
        }
    }
}
