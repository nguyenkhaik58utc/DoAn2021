using iDAS.Base;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
 public   class DispatchGoAttachmentFileSV
    {
        DispatchGoAttachmentFileDA dispatchGoAttachmentFileDA = new DispatchGoAttachmentFileDA();
        public void InsertRange(List<Guid> IDs, int taskId, int userID)
        {
            var lstCusGomerContractAttachment = new List<DispatchGoAttachmentFile>();
            var datetimeNow = DateTime.Now;
            if (IDs.Count > 0)
            {
                foreach (var id in IDs)
                {
                    lstCusGomerContractAttachment.Add(
                            new DispatchGoAttachmentFile()
                            {
                                AttachmentFileID = id,
                                DispatchGoID = taskId,
                                CreateAt = datetimeNow,
                                CreateBy = userID,
                            }
                        );
                }
                dispatchGoAttachmentFileDA.InsertRange(lstCusGomerContractAttachment);
                dispatchGoAttachmentFileDA.Save();
            }
        }
        public void DeleteRange(List<Guid> IDs)
        {
            var itemDeltes = dispatchGoAttachmentFileDA.GetQuery(i => IDs.Contains(i.AttachmentFileID)).ToList();
            dispatchGoAttachmentFileDA.DeleteRange(itemDeltes);
            dispatchGoAttachmentFileDA.Save();
        }
    }
}
