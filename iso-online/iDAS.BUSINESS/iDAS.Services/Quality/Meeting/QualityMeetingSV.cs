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
    public class QualityMeetingSV
    {
        private QualityMeetingDA MeetingDA = new QualityMeetingDA();
        /// <summary>
        /// Lấy tất cả cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<QualityMeetingItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = MeetingDA.GetQuery()
                        .Select(item => new QualityMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Address = item.Address,
                            TaskPrepare = item.TaskPrepare,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<QualityMeetingItem> GetAllNotApproved(int page, int pageSize, out int totalCount)
        {
            var users = MeetingDA.GetQuery(i => (i.IsApproval == true && i.IsAccept == true) == false)
                        .Select(item => new QualityMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<QualityMeetingItem> GetAllApproved(int page, int pageSize, out int totalCount)
        {
            var dbo = MeetingDA.Repository;
            var users = MeetingDA.GetQuery(i => i.IsApproval == true && i.IsAccept == true)
                        .Select(item => new QualityMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            CreateAt = item.CreateAt,
                            AttachmentFileIDs = dbo.QualityMeetingAttachmentFiles.Where(t => t.QualityMeetingID == item.ID).Select(x => x.AttachmentFileID).ToList()
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy cuộc họp theo đơn vị
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<QualityMeetingItem> GetByDepartmentID(int page, int pageSize, out int totalCount, int DepartmentID)
        {
            var dbo = MeetingDA.Repository;
            var users = dbo.QualityMeetings.Where(i => i.DepartmentID == DepartmentID)
                        .Select(item => new QualityMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<QualityMeetingItem> GetNotAppovedByDepartmentID(int page, int pageSize, out int totalCount, int DepartmentID, int focusId = 0)
        {
            var dbo = MeetingDA.Repository;
            IQueryable<iDAS.Base.QualityMeeting> query;
            if (focusId != 0)
            {
                query = dbo.QualityMeetings.Where(t => t.ID == focusId);
            }
            else
            {
                query = dbo.QualityMeetings.Where(i => i.DepartmentID == DepartmentID && !(i.IsApproval == true && i.IsAccept == true));
            }
            var meetings = query
                        .Select(item => new QualityMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            CreateAt = item.CreateAt,
                            AttachmentFileIDs = dbo.QualityMeetingAttachmentFiles.Where(t => t.QualityMeetingID == item.ID).Select(x => x.AttachmentFileID).ToList()
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return meetings;
        }
        /// <summary>
        /// Lấy danh sách cuộc họp đã được phê duyệt theo đơn vị
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<QualityMeetingItem> GetAppovedByDepartmentID(int page, int pageSize, out int totalCount, int DepartmentID)
        {
            var dbo = MeetingDA.Repository;
            var users = dbo.QualityMeetings.Where(i => i.DepartmentID == DepartmentID && i.IsApproval == true && i.IsAccept == true)
                        .Select(item => new QualityMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            IsFinish = item.IsFinish,
                            CreateAt = item.CreateAt,
                            AttachmentFileIDs = dbo.QualityMeetingAttachmentFiles.Where(t => t.QualityMeetingID == item.ID).Select(x => x.AttachmentFileID).ToList()

                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy cuộc họp  theo mã cuộc họp
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QualityMeetingItem GetById(int Id)
        {
            var dbo = MeetingDA.Repository;
            var MeetingItem = dbo.QualityMeetings.Where(i => i.ID == Id)
                        .Select(item => new QualityMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Department = new HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Address = item.Address,
                            TaskPrepare = item.TaskPrepare,
                            ApprovalNote = item.Note,
                            Content = item.Content,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsAccept = item.IsAccept,
                            IsEdit = item.IsEdit,
                            IsResult = item.IsApproval ? (bool?)(item.IsAccept ? true : false) : null,
                            IsFinish = item.IsFinish,
                            Approval = new HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ?(int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.ApprovalBy).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ApprovalBy).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ApprovalBy).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            },
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy,
                            AttachmentFile = new AttachmentFileItem()
                            {
                                Files = dbo.QualityMeetingAttachmentFiles.Where(i => i.QualityMeetingID == Id)
                                    .Select(i => i.AttachmentFileID).ToList()
                            }
                        })
                        .First();
            return MeetingItem;
        }
        /// <summary>
        /// Gửi bản ghi cho lãnh đạo phê duyệt IsEdit = false; Lãnh đạo trả về để sửa IsSend = false
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userID"></param>
        public void SendApproval(QualityMeetingItem item, int userID)
        {
            if (item.ID != 0)
            {
                var meeting = MeetingDA.GetById(item.ID);
                meeting.IsApproval = false;
                meeting.IsNew = false;
                meeting.IsEdit = false;
                meeting.ApprovalBy = item.ApprovalBy;
                MeetingDA.Save();
            }
        }
        /// <summary>
        ///Phê duyệt cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="ID">Mã cuộc họp</param>
        /// <param name="UserID"> Người phê duyệt</param>
        /// <param name="note"> Ghi chú phê duyệt</param>
        public void Approve(QualityMeetingItem PlanApprovalItem)
        {
            var pl = MeetingDA.GetById(PlanApprovalItem.ID);
            pl.IsApproval = true;
            pl.IsNew = false;
            pl.IsEdit = PlanApprovalItem.IsEdit;
            pl.ApprovalAt = PlanApprovalItem.ApprovalAt;
            pl.IsAccept = PlanApprovalItem.IsAccept;
            pl.Note = PlanApprovalItem.ApprovalNote;
            MeetingDA.Save();
        }
        public void Finish(int ID, int userID)
        {
            var meetingItem = MeetingDA.GetById(ID);
            meetingItem.IsNew = false;
            meetingItem.IsFinish = true;
            meetingItem.UpdateAt = DateTime.Now;
            meetingItem.UpdateBy = userID;
            MeetingDA.Save();
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// Cập nhật cuộc họp 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(QualityMeetingItem item, int userID)
        {
            var meeting = MeetingDA.GetById(item.ID);
            meeting.Name = item.Name;
            meeting.DepartmentID = item.Department.ID;
            meeting.Address = item.Address;
            meeting.TaskPrepare = item.TaskPrepare;
            meeting.Content = item.Content;
            meeting.Cost = item.Cost;
            meeting.StartTime = item.StartTime;
            meeting.EndTime = item.EndTime;
            meeting.IsApproval = item.IsApproval;
            meeting.ApprovalAt = item.ApprovalAt;
            meeting.ApprovalBy = item.ApprovalBy;
            meeting.Note = item.ApprovalNote;
            meeting.UpdateAt = DateTime.Now;
            meeting.UpdateBy = userID;
            new FileSV().Upload<QualityMeetingAttachmentFile>(item.AttachmentFile, item.ID);
            MeetingDA.Save();
        }
        /// <summary>
        /// Cập nhật các thông tin cơ bản của cuộc họp 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void UpdateInfo(QualityMeetingItem item, int userID)
        {
            var rp = MeetingDA.GetById(item.ID);
            rp.Name = item.Name;
            rp.DepartmentID = item.Department.ID;
            rp.StartTime = item.StartTime;
            rp.EndTime = item.EndTime;
            rp.Cost = item.Cost;
            rp.Content = item.Content;
            rp.UpdateAt = DateTime.Now;
            rp.UpdateBy = userID;
            MeetingDA.Save();
        }
        /// <summary>
        /// Thêm mới cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(QualityMeetingItem item, int userID)
        {
            var startTime = item.StartTime;
            var endTime = item.EndTime;
            var meeting = new QualityMeeting()
            {
                Name = item.Name,
                DepartmentID = item.Department.ID,
                Cost = item.Cost,
                StartTime = startTime,
                EndTime = endTime,
                Address = item.Address,
                TaskPrepare = item.TaskPrepare,
                Note = item.ApprovalNote,
                Content = item.Content,
                // Thêm mới mặc đinh là chưa xóa và cho phép sửa
                IsEdit = true,
                IsNew = true,
                CreateAt = DateTime.Now,
                CreateBy = userID,

            };
            MeetingDA.Insert(meeting);
            MeetingDA.Save();
            new FileSV().Upload<QualityMeetingAttachmentFile>(item.AttachmentFile, meeting.ID);
        }
        /// <summary>
        /// Xóa cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            MeetingDA.Delete(id);
            MeetingDA.Save();
        }

        public List<QualityMeetingObjectItem> GetMettingObject(int page, int pageSize, out int totalCount, int MeetingID)
        {
            return new QualityMeetingObjectSV().GetByMeeting(page, pageSize, out totalCount, MeetingID);
        }
        public List<QualityMeetingObjectItem> GetMettingObjectHasMeeting(int page, int pageSize, out int totalCount, int MeetingID)
        {
            return new QualityMeetingObjectSV().GetHasMeeting(page, pageSize, out totalCount, MeetingID);
        }

        public void InsertMeetingObject(string stringId, int userId, int meetingId)
        {
            new QualityMeetingObjectSV().InsertObject(stringId, userId, meetingId);
        }

        public void DeleteRangeObject(string listId)
        {
            new QualityMeetingObjectSV().DeleteRange(listId);
        }
    }
}
