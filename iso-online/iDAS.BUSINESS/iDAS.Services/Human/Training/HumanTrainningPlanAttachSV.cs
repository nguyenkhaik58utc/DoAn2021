using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using System.Security.Principal;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class HumanTrainningPlanAttachSV
    {
        private HumanTrainningPlanAttachDA HumanTrainningAttachDA = new HumanTrainningPlanAttachDA();
        public List<HumanTrainingPlanAttachmentItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var result = HumanTrainningAttachDA.GetQuery()
                        .Select(item => new HumanTrainingPlanAttachmentItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            Size = item.Size,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            HumanTrainingPlanID  = item.HumanTrainingPlanID
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<HumanTrainingPlanAttachmentItem> GetAllByHumanTrainingPlanID(int page, int pageSize, out int totalCount, int _Id)
        {
            var users = HumanTrainningAttachDA.GetQuery(i => i.HumanTrainingPlanID == _Id)
                         .Select(item => new HumanTrainingPlanAttachmentItem()
                         {
                             ID = item.ID,
                             Name = item.Name,
                             Note = item.Note,
                             Size = item.Size,
                             CreateAt = item.CreateAt,
                             CreateBy = item.CreateBy,
                             UpdateAt = item.UpdateAt,
                             UpdateBy = item.UpdateBy,
                             HumanTrainingPlanID = item.HumanTrainingPlanID
                         })
                         .OrderByDescending(item => item.CreateAt)
                         .Page(page, pageSize, out totalCount)
                         .ToList();
            return users;

        }
        /// <summary>
        /// Author: VinhDQ 07/2015 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanTrainingPlanAttachmentItem GetById(int Id)
        {
            var dbo = HumanTrainningAttachDA.Repository;
            var item = HumanTrainningAttachDA.GetQuery(i => i.ID == Id).First();
            var result = new HumanTrainingPlanAttachmentItem();
            result.ID = item.ID;
            result.Name = item.Name;
            result.Note = item.Note;
            result.AttachmentFile = item.AttachmentFileID == null ? new AttachmentFileItem()
                                                : new AttachmentFileItem() { Files = new List<Guid>() { (Guid)item.AttachmentFileID } };
            result.Size = item.Size;
            result.HumanTrainingPlanID = item.HumanTrainingPlanID;
            return result;
        }
        /// <summary>
        /// Author: VinhDQ 07/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanTrainingPlanAttachmentItem item, int userID)
        {
            var attach = HumanTrainningAttachDA.GetById(item.ID);
            attach.Name = item.Name;
            if (item.AttachmentFile != null && item.AttachmentFile.FileAttachments.Count > 0)
            {
                if (attach.AttachmentFileID != null)
                {
                    new FileSV().Delete((Guid)attach.AttachmentFileID);
                }
                var fileInsert = item.AttachmentFile.FileAttachments.First();
                attach.AttachmentFileID = new FileSV().Insert(fileInsert, userID);

            }
            else attach.AttachmentFileID = null;
            attach.Note = item.Note;
            attach.Size = item.Size;
            attach.CreateAt = item.CreateAt;
            attach.CreateBy = item.CreateBy;
            attach.HumanTrainingPlanID = item.HumanTrainingPlanID;
            attach.UpdateAt = DateTime.Now;
            attach.UpdateBy = userID;
            HumanTrainningAttachDA.Save();
        }
        /// <summary>
        /// Author: VinhDQ 07/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanTrainingPlanAttachmentItem item, int userID)
        {
            var attach = new HumanTrainingPlanAttachment()
            {
                Name = item.Name,
                Note = item.Note,
                Size = item.Size,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanTrainingPlanID = item.HumanTrainingPlanID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            if (item.AttachmentFile != null && item.AttachmentFile.FileAttachments.Count > 0)
            {
                var fileInsert = item.AttachmentFile.FileAttachments.First();
                attach.AttachmentFileID = new FileSV().Insert(fileInsert, userID);
            }
            HumanTrainningAttachDA.Insert(attach);
            HumanTrainningAttachDA.Save();
        }
        /// <summary>
        /// Author: VingDQ DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var deleteItem = HumanTrainningAttachDA.GetById(id);
            if (deleteItem.AttachmentFileID != null)
            {
                new FileSV().Delete((Guid)deleteItem.AttachmentFileID);
            }
            HumanTrainningAttachDA.Delete(id);
            HumanTrainningAttachDA.Save();
        }
    }
}
