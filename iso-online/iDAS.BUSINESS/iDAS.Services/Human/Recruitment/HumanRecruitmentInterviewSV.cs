using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class HumanRecruitmentInterviewSV
    {
        private HumanRecruitmentInterviewDA RecruitmentInterviewDA = new HumanRecruitmentInterviewDA();
        /// <summary>
        /// Lấy tất cả kết quả thi tuyển của ứng viên
        /// Author: Thanhnv CreateDate: 26/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentInterviewItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var ri = RecruitmentInterviewDA.GetQuery()
                        .Select(item => new HumanRecruitmentInterviewItem()
                        {
                            ID = item.ID,
                            //PorfileInterviewID = item.PorfileInterviewID,
                            Result = item.Result,
                            Note = item.Note,
                            //PlanInterviewID = item.PlanInterviewID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return ri;
        }
        /// <summary>
        /// Lấy kết quả thi của thí sinh
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="ProfileInterviewID"></param>
        /// <returns></returns>
        public List<HumanRecruitmentInterviewItem> GetByProfieInterviewID(int page, int pageSize, out int totalCount, int ProfileInterviewID, int PlanID)
        {
            var dbo = RecruitmentInterviewDA.Repository;
            var ris = dbo.HumanRecruitmentPlanInterviews.Where(i => i.HumanRecruitmentPlanID == PlanID)
                .Select(item => new HumanRecruitmentInterviewItem()
                {
                    ID = item.HumanRecruitmentInterviews.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == ProfileInterviewID).ID,
                    PlanInterviewID = item.ID,
                    PlanInterviewName = item.Name,
                    StartTime = item.StartTime ,
                    EndTime = item.EndTime,
                    InterviewContent = item.Content,
                    PorfileInterviewID = ProfileInterviewID,
                    Result = item.HumanRecruitmentInterviews.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == ProfileInterviewID).Result,
                    Note = item.HumanRecruitmentInterviews.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == ProfileInterviewID).Note,
                    Time = item.HumanRecruitmentInterviews.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == ProfileInterviewID).Time

                }).OrderByDescending(item => item.StartTime)
                        .Page(page, pageSize, out totalCount)
                        .ToList(); 
            return ris;
        }

        /// <summary>
        /// Lấy kết quả thi tuyển theo ID
        /// Author: Thanhnv CreateDate: 26/12/2014  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentInterviewItem GetById(int Id)
        {
            var dbo = RecruitmentInterviewDA.Repository;
            var ri = RecruitmentInterviewDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentInterviewItem()
                        {
                            ID = item.ID,
                            PorfileInterviewID = item.HumanRecruitmentProfileInterviewID,
                            Result = item.Result,
                            Note = item.Note,
                            PlanInterviewID = item.HumanRecruitmentPlanInterviewID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .First();
            return ri;
        }
        /// <summary>
        /// Cập nhật kết quả thi tuyển
        /// Author: Thanhnv CreateDate: 26/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentInterviewItem item, int userID)
        {
            var rc = RecruitmentInterviewDA.GetById(item.ID);
            rc.HumanRecruitmentProfileInterviewID = item.PorfileInterviewID;
            rc.Result = item.Result;
            rc.Note = item.Note;
            rc.HumanRecruitmentPlanInterviewID = item.PlanInterviewID;
            rc.UpdateAt = DateTime.Now;
            rc.UpdateBy = userID;
            RecruitmentInterviewDA.Save();
        }
        /// <summary>
        /// Cập nhật kết quả thi tuyển nếu đã có nếu chưa có thì thêm mới
        /// Author: Thanhnv CreateDate: 26/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void UpdateOrInsert(HumanRecruitmentInterviewItem item, int userID)
        {
            var dbo = RecruitmentInterviewDA.Repository;
            var riExits = dbo.HumanRecruitmentInterviews.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == item.PorfileInterviewID && i.HumanRecruitmentPlanInterviewID == item.PlanInterviewID);
            if (riExits == null)
            {
                var ri = new HumanRecruitmentInterview()
                {
                    HumanRecruitmentProfileInterviewID = item.PorfileInterviewID,
                    Result = item.Result,
                    Time = item.Time == null ? DateTime.Now : (DateTime)item.Time,
                    Note = item.Note,
                    HumanRecruitmentPlanInterviewID = item.PlanInterviewID,
                    CreateAt = DateTime.Now,
                    CreateBy = userID
                };
                dbo.HumanRecruitmentInterviews.Add(ri);
            }
            else
            {
                riExits.HumanRecruitmentProfileInterviewID = item.PorfileInterviewID;
                riExits.Result = item.Result;
                riExits.Note = item.Note;
                riExits.HumanRecruitmentPlanInterviewID = item.PlanInterviewID;
                if (item.Time != null)
                    riExits.Time = (DateTime)item.Time;
                riExits.UpdateAt = DateTime.Now;
                riExits.UpdateBy = userID;
            }
            dbo.SaveChanges();
        }
        /// <summary>
        /// Thêm mới kết quả thi tuyển
        /// Author: Thanhnv CreateDate: 26/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentInterviewItem item, int userID)
        {
            var dbo = RecruitmentInterviewDA.Repository;
            var ri = new HumanRecruitmentInterview()
            {
                HumanRecruitmentProfileInterviewID = item.PorfileInterviewID,
                Result = item.Result,
                Note = item.Note,
                HumanRecruitmentPlanInterviewID = item.PlanInterviewID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            var riExits = dbo.HumanRecruitmentInterviews.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID== item.PorfileInterviewID && i.HumanRecruitmentPlanInterviewID == item.PlanInterviewID);
            if(riExits == null)
            {
                dbo.HumanRecruitmentInterviews.Add(ri);
                dbo.SaveChanges();
            }
        }
        /// <summary>
        /// Xóa kết quả thi tuyển
        /// Author: Thanhnv CreateDate: 26/12/2014  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentInterviewDA.Delete(id);
            RecruitmentInterviewDA.Save();
        }
        public List<RecruitmentProfileIneterviewSelectItem> GetByPlanIDAndRequirementID(int page, int pageSize, out int totalCount, int PlanID, int RequirementID)
        {
            return new HumanRecruitmentProfileSV().GetByPlanIDAndRequirementID(page, pageSize, out totalCount, PlanID, RequirementID);
        }
    }
}
