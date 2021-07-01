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
    public class HumanProfileAttachSV
    {
        private HumanProfileAttachDA ProfileAttachDA = new HumanProfileAttachDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileAttachItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var result = ProfileAttachDA.GetQuery()
                        .Select(item => new HumanProfileAttachItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            Size = item.Size,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID
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
        public List<HumanProfileAttachItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileAttachDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                         .Select(item => new HumanProfileAttachItem()
                         {
                             ID = item.ID,
                             Name = item.Name,
                             Note = item.Note,
                             Size = item.Size,
                             CreateAt = item.CreateAt,
                             CreateBy = item.CreateBy,
                             UpdateAt = item.UpdateAt,
                             UpdateBy = item.UpdateBy,
                             EmployeeID = item.HumanEmployeeID
                         })
                         .OrderByDescending(item => item.CreateAt)
                         .Page(page, pageSize, out totalCount)
                         .ToList();
            return users;

        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanProfileAttachItem GetById(int Id)
        {
            var dbo = ProfileAttachDA.Repository;
            var item = ProfileAttachDA.GetQuery(i => i.ID == Id).First();
            var result = new HumanProfileAttachItem();
            result.ID = item.ID;
            result.Name = item.Name;
            result.Note = item.Note;
            result.AttachmentFiles = item.AttachmentFileID == null ? new AttachmentFileItem()
                                                : new AttachmentFileItem() { Files = new List<Guid>() { (Guid)item.AttachmentFileID } };
            result.Size = item.Size;
            result.EmployeeID = item.HumanEmployeeID;
            return result;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanProfileAttachItem item, int userID)
        {
            var attach = ProfileAttachDA.GetById(item.ID);
            attach.Name = item.Name;
            if (item.AttachmentFiles != null && item.AttachmentFiles.FileAttachments.Count > 0)
            {
                if (item.AttachmentFiles.FileRemoves != null && item.AttachmentFiles.FileRemoves.Count > 0)
                {
                    new FileSV().Delete((Guid)item.AttachmentFiles.FileRemoves.First());
                }
                if (item.AttachmentFiles.FileAttachments != null)
                {
                    var fileInsert = item.AttachmentFiles.FileAttachments.FirstOrDefault();
                    if (fileInsert != null)
                        attach.AttachmentFileID = new FileSV().Insert(fileInsert, userID);
                }
            }
            else attach.AttachmentFileID = null;
            attach.Note = item.Note;
            attach.Size = item.Size;
            attach.CreateAt = item.CreateAt;
            attach.CreateBy = item.CreateBy;
            attach.HumanEmployeeID = item.EmployeeID;
            attach.UpdateAt = DateTime.Now;
            attach.UpdateBy = userID;
            ProfileAttachDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileAttachItem item, int userID)
        {
            var attach = new HumanProfileAttachment()
            {
                Name = item.Name,
                Note = item.Note,
                Size = item.Size,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            if (item.AttachmentFiles != null && item.AttachmentFiles.FileAttachments.Count > 0)
            {
                var fileInsert = item.AttachmentFiles.FileAttachments.FirstOrDefault();
                if (fileInsert != null)
                    attach.AttachmentFileID = new FileSV().Insert(fileInsert, userID);
            }
            ProfileAttachDA.Insert(attach);
            ProfileAttachDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var deleteItem = ProfileAttachDA.GetById(id);
            if (deleteItem.AttachmentFileID != null)
            {
                new FileSV().Delete((Guid)deleteItem.AttachmentFileID);
            }
            ProfileAttachDA.Delete(id);
            ProfileAttachDA.Save();
        }
    }
}
