using iDAS.Base;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
   public class TaskPerformAttachmentFileSV
    {
       private TaskPerformAttachmentFileDA taskPerformAttachmentFileDA = new TaskPerformAttachmentFileDA();
       public void InsertRange(List<Guid> IDs, int taskPerformId, int userID)
        {
            var lstTaskPerformAttachmentFile = new List<TaskPerformAttachmentFile>();
            var datetimeNow = DateTime.Now;
            if (IDs.Count > 0)
            {
                foreach (var id in IDs)
                {
                    lstTaskPerformAttachmentFile.Add(
                            new TaskPerformAttachmentFile()
                            {
                                AttachmentFileID = id,
                                TaskPerformID = taskPerformId,
                                CreateAt = datetimeNow,
                                CreateBy = userID,
                            }
                        );
                }
                taskPerformAttachmentFileDA.InsertRange(lstTaskPerformAttachmentFile);
                taskPerformAttachmentFileDA.Save();
            }
        }
        public void DeleteRange(List<Guid> IDs)
        {
            var itemDeltes = taskPerformAttachmentFileDA.GetQuery(i => IDs.Contains(i.AttachmentFileID)).ToList();
            taskPerformAttachmentFileDA.DeleteRange(itemDeltes);
            taskPerformAttachmentFileDA.Save();
        }
    }
}
