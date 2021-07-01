using iDAS.Base;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
   public class TaskAttachmentFileSV
    {
       TaskAttachmentFileDA taskAttachmentFileDA = new TaskAttachmentFileDA();
        public void InsertRange(List<Guid> IDs, int taskId, int userID)
        {
            var lstCustomerContractAttachment = new List<TaskAttachmentFile>();
            var datetimeNow = DateTime.Now;
            if (IDs.Count > 0)
            {
                foreach (var id in IDs)
                {
                    lstCustomerContractAttachment.Add(
                            new TaskAttachmentFile()
                            {
                                AttachmentFileID = id,
                                TaskID = taskId,
                                IsDelete =false,
                                IsChange = false,
                                CreateAt = datetimeNow,
                                CreateBy = userID,
                            }
                        );
                }
                taskAttachmentFileDA.InsertRange(lstCustomerContractAttachment);
                taskAttachmentFileDA.Save();
            }
        }
        public void InsertRangeByChange(List<Guid> IDs, int taskId, int userID)
        {
            var lstCustomerContractAttachment = new List<TaskAttachmentFile>();
            var datetimeNow = DateTime.Now;
            if (IDs.Count > 0)
            {
                foreach (var id in IDs)
                {
                    lstCustomerContractAttachment.Add(
                            new TaskAttachmentFile()
                            {
                                AttachmentFileID = id,
                                TaskID = taskId,
                                IsDelete = false,
                                IsChange = true,
                                CreateAt = datetimeNow,
                                CreateBy = userID,
                            }
                        );
                }
                taskAttachmentFileDA.InsertRange(lstCustomerContractAttachment);
                taskAttachmentFileDA.Save();
            }
        }
        public void DeleteRangeByChange(List<Guid> IDs)
        {
            var itemDeltes = taskAttachmentFileDA.GetQuery(i => IDs.Contains(i.AttachmentFileID)).ToList();
            foreach (var item in itemDeltes)
            {
                item.IsDelete = true;
            }
            taskAttachmentFileDA.UpdateRange(itemDeltes);
            taskAttachmentFileDA.Save();
        }
        public void DeleteRange(List<Guid> IDs)
        {
            var itemDeltes = taskAttachmentFileDA.GetQuery(i => IDs.Contains(i.AttachmentFileID)).ToList();
            taskAttachmentFileDA.DeleteRange(itemDeltes);
            taskAttachmentFileDA.Save();
        }
    }
}
