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
    public class QualityMeetingResultSV
    {
        private QualityMeetingResultDA MeetingResultDA = new QualityMeetingResultDA();
        /// <summary>
        /// Lấy tất cả  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<QualityMeetingResultItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = MeetingResultDA.GetQuery()
                        .Select(item => new QualityMeetingResultItem()
                        {
                            ID = item.ID,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Address = item.Address,
                            MeetingID = item.QualityMeetingID,
                            Content = item.Content,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy   theo mã 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QualityMeetingResultItem GetById(int Id)
        {
            var dbo = MeetingResultDA.Repository;
            var MeetingResultItem = dbo.QualityMeetingResults.Where(i => i.ID == Id)
                        .Select(item => new QualityMeetingResultItem()
                        {
                            ID = item.ID,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Address = item.Address,
                            MeetingID = item.QualityMeetingID,
                            Content = item.Content,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                         //   CreateByName = dbo.HumanUser.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                           // UpdateByName = dbo.HumanUser.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return MeetingResultItem;
        }

        public QualityMeetingResultItem GetByMeetingId(int meetingId)
        {
            var dbo = MeetingResultDA.Repository;
            var MeetingResultItem = dbo.QualityMeetingResults.Where(i => i.QualityMeetingID == meetingId)
                        .Select(item => new QualityMeetingResultItem()
                        {
                            ID = item.ID,
                            MeetingName = item.QualityMeeting.Name,
                            DepartmentName =item.QualityMeeting.DepartmentID!=null&&dbo.HumanDepartments.FirstOrDefault(t=>t.ID==item.QualityMeeting.DepartmentID)!=null?dbo.HumanDepartments.FirstOrDefault(t=>t.ID==item.QualityMeeting.DepartmentID).Name:string.Empty,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Address = item.Address,
                            MeetingID = item.QualityMeetingID,
                            Content = item.Content,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            //CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            //UpdateByName = dbo.HumanUser.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy,
                            AttachmentFile = new AttachmentFileItem()
                            {
                                Files = dbo.QualityMeetingResultAttachmentFiles.Where(i => i.QualityMeetingResultID == item.ID)
                                    .Select(i => i.AttachmentFileID).ToList()
                            }
                        })
                        .FirstOrDefault();
            if (MeetingResultItem == null)
            {
                MeetingResultItem = dbo.QualityMeetings.Where(i => i.ID == meetingId)
                         .Select(item => new QualityMeetingResultItem()
                         {
                             MeetingName = item.Name,
                             DepartmentName = item.DepartmentID != null && dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.DepartmentID) != null 
                                            ? dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.DepartmentID).Name : string.Empty,
                             MeetingID = meetingId,
                         })
                         .FirstOrDefault();
            }
            return MeetingResultItem;
        }
        public QualityMeetingResultItem UseMeetingInfo(int meetingId)
        {
            var dbo = MeetingResultDA.Repository;
            var MeetingResultItem = dbo.QualityMeetings.Where(i => i.ID == meetingId)
                        .Select(item => new QualityMeetingResultItem()
                        {
                            MeetingID = item.ID,
                            MeetingName = item.Name,
                            DepartmentName = dbo.HumanDepartments.Where(t=>t.ID==item.DepartmentID).Select(t=>t.Name).FirstOrDefault(),
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Address = item.Address,
                            Content = item.Content,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            //CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            //UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return MeetingResultItem;
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// Cập nhật  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(QualityMeetingResultItem item, int userID)
        {
            var meetinResult = MeetingResultDA.GetById(item.ID);
            meetinResult.Content = item.Content;
            meetinResult.Cost = item.Cost;
            meetinResult.StartTime = item.StartTime;
            meetinResult.EndTime = item.EndTime;
            meetinResult.QualityMeetingID = item.MeetingID;
            meetinResult.Address = item.Address;
            meetinResult.Content = item.Content;
            meetinResult.Note = item.Note;
            meetinResult.UpdateAt = DateTime.Now;
            meetinResult.UpdateBy = userID;
            new FileSV().Upload<QualityMeetingResultAttachmentFile>(item.AttachmentFile, item.ID);
            MeetingResultDA.Save();
        }
        /// <summary>
        /// Thêm mới  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(QualityMeetingResultItem item, int userID)
        {
            var dbo = MeetingResultDA.Repository;;
            var mResult = new QualityMeetingResult()
            {
                Cost = item.Cost,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Address = item.Address,
                QualityMeetingID = item.MeetingID,
                Content = item.Content,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID,

            };
            dbo.QualityMeetingResults.Add(mResult);
            var meetingItem = dbo.QualityMeetings.FirstOrDefault(i => i.ID == item.MeetingID);
            meetingItem.IsFinish = true;
            dbo.SaveChanges();
            new FileSV().Upload<QualityMeetingResultAttachmentFile>(item.AttachmentFile, mResult.ID);
        }
        /// <summary>
        /// Xóa  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            MeetingResultDA.Delete(id);
            MeetingResultDA.Save();
        }
    }
}
