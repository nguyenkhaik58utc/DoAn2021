using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class QualityMeetingProblemSV
    {
        private QualityMeetingProblemDA MeetingProblemDA = new QualityMeetingProblemDA();
        CenterISOMeetingDA IsoMettingDA = new CenterISOMeetingDA();
        /// <summary>
        /// Lấy tất cả  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<QualityMeetingProblemItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var problems = MeetingProblemDA.GetQuery()
                        .Select(item => new QualityMeetingProblemItem()
                        {
                            ID = item.ID,
                            ISOMeetingID = item.ISOMeetingID,
                            MeetingID = item.QualityMeetingID,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return problems;
        }
        public List<QualityMeetingProblemItem> GetByMeeting(int page, int pageSize, out int totalCount, int MeetingID)
        {
            var problems = MeetingProblemDA.GetQuery(i => i.QualityMeetingID == MeetingID)
                        .Select(item => new QualityMeetingProblemItem()
                        {
                            ID = item.ID,
                            ISOMeetingID = item.ISOMeetingID,
                            MeetingID = item.QualityMeetingID,
                            Content = item.Content,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            if (problems != null && problems.Count > 0)
            {
                var dbo = IsoMettingDA.Repository;
                foreach (var problem in problems)
                {
                    problem.Name = dbo.ISOMeetings.FirstOrDefault(i => i.ID == problem.ISOMeetingID).Name;
                    problem.IsoName = dbo.ISOMeetings.FirstOrDefault(i => i.ID == problem.ISOMeetingID).ISOStandard.Name;
                }
            }
            return problems;
        }
        public QualityMeetingProblemItem GetById(int id)
        {
            var dbo = IsoMettingDA.Repository;
            var problem = MeetingProblemDA.GetQuery(i => i.ID == id)
                            .Select(item => new QualityMeetingProblemItem()
                            {
                                ID = item.ID,
                                ISOMeetingID = item.ISOMeetingID,
                                MeetingID = item.QualityMeetingID,
                                Content = item.Content,
                                CreateAt = item.CreateAt
                            }).FirstOrDefault();
            if(problem !=null)
            {
                problem.Name = dbo.ISOMeetings.FirstOrDefault(i => i.ID == problem.ISOMeetingID).Name;
                problem.IsoName = dbo.ISOMeetings.FirstOrDefault(i => i.ID == problem.ISOMeetingID).ISOStandard.Name;
            }
            return problem;
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// Cập nhật  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(QualityMeetingProblemItem item, int userID)
        {
            var MeetingProblem = MeetingProblemDA.GetById(item.ID);
            MeetingProblem.QualityMeetingID = item.MeetingID;
            MeetingProblem.ISOMeetingID = item.ISOMeetingID;
            //MeetingProblem.AuditObjectID = MeetingProblem.AuditObjectID;
            MeetingProblem.UpdateAt = DateTime.Now;
            MeetingProblem.UpdateBy = userID;
            MeetingProblemDA.Save();
        }
        public void UpdateContent(QualityMeetingProblemItem item, int userID)
        {
            var MeetingProblem = MeetingProblemDA.GetById(item.ID);
            MeetingProblem.Content = item.Content;
            MeetingProblem.UpdateAt = DateTime.Now;
            MeetingProblem.UpdateBy = userID;
            MeetingProblemDA.Save();
        }
        /// <summary>
        /// Thêm mới  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(QualityMeetingProblemItem item, int userID)
        {
            var mObject = new QualityMeetingProblem()
            {
                ID = item.ID,
                ISOMeetingID = item.ISOMeetingID,
                QualityMeetingID = item.MeetingID,
                CreateAt = DateTime.Now,
                CreateBy = userID,

            };
            MeetingProblemDA.Insert(mObject);
            MeetingProblemDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        /// <param name="userId"></param>
        /// <param name="meetingId"></param>
        public void InsertObject(string stringId, int userId, int meetingId)
        {
            var dbo = MeetingProblemDA.Repository;
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            var idsExit = dbo.QualityMeetingProblems.Where(i => i.QualityMeetingID == meetingId).Select(item => item.ISOMeetingID).ToArray();
            List<QualityMeetingProblem> objectMeeting = new List<QualityMeetingProblem>();
            foreach (var id in ids)
            {
                if (!idsExit.Contains((int)id))
                {
                    objectMeeting.Add(new QualityMeetingProblem
                    {
                        ISOMeetingID = (int)id,
                        QualityMeetingID = meetingId,
                        CreateAt = DateTime.Now,
                        CreateBy = userId,
                    });
                };
            }
            dbo.QualityMeetingProblems.AddRange(objectMeeting);
            dbo.SaveChanges();
        }
        /// <summary>
        /// Xóa  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            MeetingProblemDA.Delete(id);
            MeetingProblemDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        public void DeleteRangeByEmployeeID(string stringEmployeeId)
        {
            var dbo = MeetingProblemDA.Repository;
            var ids = stringEmployeeId.Split(',').Select(n => int.Parse(n)).ToArray();
            var listItemDelete = dbo.QualityMeetingProblems.Where(i => ids.FirstOrDefault(id => id == i.ISOMeetingID) != null);
            dbo.QualityMeetingProblems.RemoveRange(listItemDelete);
            dbo.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        public void DeleteRange(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            MeetingProblemDA.DeleteRange(ids);
            MeetingProblemDA.Save();
        }

        public List<CenterISOClauseItem> LoadISO()
        {
            return new ISOMeetingSV().GetAllActive();
        }

        public List<CenterISOMeetingItem> GetIsoProblem(int page, int pageSize, out int totalCount, int IsoID)
        {
            return new ISOMeetingSV().GetByIsoId(page,pageSize,out totalCount,IsoID);
        }
    }
}
