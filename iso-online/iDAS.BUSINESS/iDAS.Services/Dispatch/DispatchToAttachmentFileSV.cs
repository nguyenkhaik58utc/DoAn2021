using iDAS.Base;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
   public class DispatchToAttachmentFileSV
    {
       DispatchToAttachmentFileDA dispatchToAttachmentFileDA = new DispatchToAttachmentFileDA();
        public void InsertRange(List<Guid> IDs, int taskId, int userID)
        {
            var lstCustomerContractAttachment = new List<DispatchToAttachmentFile>();
            var datetimeNow = DateTime.Now;
            if (IDs.Count > 0)
            {
                foreach (var id in IDs)
                {
                    lstCustomerContractAttachment.Add(
                            new DispatchToAttachmentFile()
                            {
                                AttachmentFileID = id,
                                 DispatchToID = taskId,
                                CreateAt = datetimeNow,
                                CreateBy = userID,
                            }
                        );
                }
                dispatchToAttachmentFileDA.InsertRange(lstCustomerContractAttachment);
                dispatchToAttachmentFileDA.Save();
            }
        }
        public void DeleteRange(List<Guid> IDs)
        {
            var itemDeltes = dispatchToAttachmentFileDA.GetQuery(i => IDs.Contains(i.AttachmentFileID)).ToList();
            dispatchToAttachmentFileDA.DeleteRange(itemDeltes);
            dispatchToAttachmentFileDA.Save();
        }
    }
}
