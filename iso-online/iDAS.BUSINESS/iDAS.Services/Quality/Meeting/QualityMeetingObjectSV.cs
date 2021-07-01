using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class QualityMeetingObjectSV
    {
        private QualityMeetingObjectDA meetingObjectDA = new QualityMeetingObjectDA();
        /// <summary>
        /// Lấy tất cả  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<QualityMeetingObjectItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var meeting = meetingObjectDA.GetQuery()
                        .Select(item => new QualityMeetingObjectItem()
                        {
                            EmployeeID = item.EmployeeID,
                            
                            MeetingID = item.QualityMeetingID,
                            IsMeeting = item.IsMeeting,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return meeting;
        }
        public List<QualityMeetingObjectItem> GetByMeeting(int page, int pageSize, out int totalCount, int MeetingID)
        {
            var dbo=meetingObjectDA.Repository;
            var meetings = meetingObjectDA
                        .GetQuery(i => i.QualityMeetingID == MeetingID)
                        .Select(item => new QualityMeetingObjectItem()
                        {
                            ID = item.ID,
                            EmployeeID = item.EmployeeID,
                            Name = dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).Select(i => i.Name).FirstOrDefault(),
                            MeetingID = item.QualityMeetingID,
                            Email = dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).Select(i => i.Email).FirstOrDefault(),
                            Phone = dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).FirstOrDefault()!=null?dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).FirstOrDefault().Phone:string.Empty,
                            IsMeeting = item.IsMeeting,
                            CreateAt = item.CreateAt,
                            lstHumanGrPos = dbo.HumanOrganizations
                            .Where(i => i.HumanEmployeeID == item.EmployeeID && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete)
                            .Select(i => new HumanGroupPositionItem()
                                {
                                    ID = i.ID,
                                    Name = i.HumanRole.Name,
                                    GroupName = i.HumanRole.HumanDepartment.Name
                                }).ToList()
                        })
                        .OrderBy(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return meetings;
        }
        public List<QualityMeetingObjectItem> GetHasMeeting(int page, int pageSize, out int totalCount, int MeetingID)
        {
            var dbo = meetingObjectDA.Repository;
            var meetings = meetingObjectDA
                        .GetQuery(i => i.QualityMeetingID == MeetingID && i.IsMeeting)
                        .Select(item => new QualityMeetingObjectItem()
                        {
                            ID = item.ID,
                            EmployeeID = item.EmployeeID,
                            Name = dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).Select(i => i.Name).FirstOrDefault(),
                            MeetingID = item.QualityMeetingID,
                            Email = dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).Select(i => i.Email).FirstOrDefault(),
                            Phone = dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).FirstOrDefault() != null ? dbo.HumanEmployees.Where(i => i.ID == item.EmployeeID).FirstOrDefault().Phone : string.Empty,
                            IsMeeting = item.IsMeeting,
                            CreateAt = item.CreateAt,
                            lstHumanGrPos = dbo.HumanOrganizations
                            .Where(i => i.HumanEmployeeID == item.EmployeeID && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete)
                            .Select(i => new HumanGroupPositionItem()
                            {
                                ID = i.ID,
                                Name = i.HumanRole.Name,
                                GroupName = i.HumanRole.HumanDepartment.Name
                            }).ToList()
                        })
                        .OrderBy(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return meetings;
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// Cập nhật  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(QualityMeetingObjectItem item, int userID)
        {
            var meetingObject = meetingObjectDA.GetById(item.ID);
            meetingObject.QualityMeetingID = item.MeetingID;
            meetingObject.EmployeeID = item.EmployeeID;
            //meetingObject.AuditObjectID = meetingObject.AuditObjectID;
            meetingObject.IsMeeting = item.IsMeeting;
            meetingObject.UpdateAt = DateTime.Now;
            meetingObject.UpdateBy = userID;
            meetingObjectDA.Save();
        }
        /// <summary>
        /// Thêm mới  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(QualityMeetingObjectItem item, int userID)
        {
            var mObject = new QualityMeetingObject()
            {
                QualityMeetingID = item.MeetingID,
                EmployeeID = item.EmployeeID,
                IsMeeting = item.IsMeeting,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            meetingObjectDA.Insert(mObject);
            meetingObjectDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        /// <param name="userId"></param>
        /// <param name="meetingId"></param>
        public void InsertObject(string stringId, int userId, int meetingId)
        {
            var dbo = meetingObjectDA.Repository;
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            var idsExit = dbo.QualityMeetingObjects.Where(i => i.QualityMeetingID == meetingId).Select(item => item.EmployeeID).ToArray();
            List<QualityMeetingObject> objectMeeting = new List<QualityMeetingObject>();
            foreach (var id in ids)
            {
                if (!idsExit.Contains((int)id))
                {
                    objectMeeting.Add(new QualityMeetingObject
                    {
                        EmployeeID = (int)id,
                        QualityMeetingID = meetingId,
                        IsMeeting = false,
                        CreateAt = DateTime.Now,
                        CreateBy = userId,
                    });
                };
            }
            dbo.QualityMeetingObjects.AddRange(objectMeeting);
            dbo.SaveChanges();
        }
        /// <summary>
        /// Xóa  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            meetingObjectDA.Delete(id);
            meetingObjectDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        public void DeleteRangeByEmployeeID(string stringEmployeeId)
        {
            var dbo = meetingObjectDA.Repository;
            var ids = stringEmployeeId.Split(',').Select(n => int.Parse(n)).ToArray();
            var listItemDelete = dbo.QualityMeetingObjects.Where(i=>ids.FirstOrDefault(id=> id == i.EmployeeID)!=null);
            dbo.QualityMeetingObjects.RemoveRange(listItemDelete);
            dbo.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        public void DeleteRange(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            meetingObjectDA.DeleteRange(ids);
            meetingObjectDA.Save();
        }
    }
}
