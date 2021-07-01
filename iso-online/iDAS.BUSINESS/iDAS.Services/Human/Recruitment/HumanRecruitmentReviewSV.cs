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
    public class HumanRecruitmentReviewSV
    {
        private HumanRecruitmentReviewDA RecruitmentReviewDA = new HumanRecruitmentReviewDA();
        /// <summary>
        /// Lấy tất cả kết quả xét tuyển
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentReviewItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = RecruitmentReviewDA.GetQuery()
                        .Select(item => new HumanRecruitmentReviewItem()
                        {
                            ID = item.ID,
                            //ProfileInterviewID = item.ProfileInterviewID,
                            CriteriaID = item.HumanRecruitmentCriteriaID,
                            Note = item.Note,
                            Point = item.Point,
                            Time = item.Time,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy danh sách hồ sơ xét tuyển với kế hoạch tuyển dụng và yêu cầu tuyển dụng của kế hoạch đó
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlanID"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns>
        public List<RecruitmentProfileReviewItem> GetByPlanIDAndRequirementID(int page, int pageSize, out int totalCount, int PlanID, int RequirementID)
        {
            var dbo = RecruitmentReviewDA.Repository;
            var profiles = dbo.HumanRecruitmentProfiles.Where(item => dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentRequirementID == RequirementID && e.HumanRecruitmentProfileID == item.ID) != null)
                        .Select(item => new RecruitmentProfileReviewItem()
                        {
                            ID = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentRequirementID == RequirementID && e.HumanRecruitmentProfileID == item.ID).ID,
                            Name = item.Name,
                            Avatar = item.Avatar,
                            Birthday = item.Birthday,
                            ProfileID = item.ID,
                            //IsSend = dbo.HumanRecruitmentProfileResult.FirstOrDefault(r => r.HumanRecruitmentProfileInterview.ProfileID == item.ID) == null ?
                            // false : dbo.HumanRecruitmentProfileResult.FirstOrDefault(r => r.HumanRecruitmentProfileInterview.ProfileID == item.ID).IsSend,
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return profiles;
        }
        /// <summary>
        /// Lấy tất cả kết quả xét tuyển theo từng tiêu chí
        /// || Author: Thanhnv || CreateDate: 09/01/2015 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="profileInterviewID"></param>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public List<HumanRecruitmentReviewItem> GetCriteria(int page, int pageSize, out int totalCount, int profileInterviewID, int RequirementID)
        {
            var dbo = RecruitmentReviewDA.Repository;
            var roleID = dbo.HumanRecruitmentRequirements.Where(re => re.ID == RequirementID).Select(i => i.HumanRoleID).FirstOrDefault();
            var profileId =  dbo.HumanRecruitmentProfileInterviews.Where(i => i.ID == profileInterviewID).Select(i => i.HumanRecruitmentProfileID).FirstOrDefault();
            var ris = dbo.HumanRecruitmentCriterias
                        .Where(i => (i.HumanRoleID == roleID) && i.IsActive && !i.IsDelete)
                        .Select(item => new HumanRecruitmentReviewItem()
                        {
                            ID = dbo.HumanRecruitmentReviews.FirstOrDefault(re => re.HumanRecruitmentProfileID == profileId && re.HumanRecruitmentCriteriaID == item.ID).ID,
                            CriteriaID = item.ID,
                            ProfileID = profileId,
                            CriteriaName = item.Name,
                            Point = dbo.HumanRecruitmentReviews.FirstOrDefault(re => re.HumanRecruitmentProfileID == profileId && re.HumanRecruitmentCriteriaID == item.ID).Point,
                            Time = dbo.HumanRecruitmentReviews.FirstOrDefault(re => re.HumanRecruitmentProfileID == profileId && re.HumanRecruitmentCriteriaID == item.ID).Time,
                            Note = dbo.HumanRecruitmentReviews.FirstOrDefault(re => re.HumanRecruitmentProfileID == profileId && re.HumanRecruitmentCriteriaID == item.ID).Note
                        })
                 .OrderByDescending(item => item.CriteriaName)
                 .Page(page, pageSize, out totalCount)
                 .ToList();
            return ris;
        }
        /// <summary>
        /// Lấy kết quả xét tuyển theo mã xét tuyển
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentReviewItem GetById(int Id)
        {
            var dbo = RecruitmentReviewDA.Repository;
            var users = RecruitmentReviewDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentReviewItem()
                        {
                            ID = item.ID,
                            ProfileID = item.HumanRecruitmentProfileID,
                            CriteriaID = item.HumanRecruitmentCriteriaID,
                            Note = item.Note,
                            Point = item.Point,
                            Time = item.Time,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Cập nhật kết quả xét tuyển
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentReviewItem item, int userID)
        {
            var rr = RecruitmentReviewDA.GetById(item.ID);
            rr.HumanRecruitmentProfileID = item.ProfileID;
            rr.HumanRecruitmentCriteriaID = item.CriteriaID;
            rr.Note = item.Note;
            if (item.Point != null)
                rr.Point = (int)item.Point;
            if (item.Time != null)
                rr.Time = (DateTime)item.Time;
            rr.CreateAt = item.CreateAt;
            rr.CreateBy = item.CreateBy;
            rr.UpdateAt = DateTime.Now;
            rr.UpdateBy = userID;
            RecruitmentReviewDA.Save();
        }
        /// <summary>
        /// Thêm mới hồ sơ xét tuyển
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentReviewItem item, int userID)
        {
            var user = new HumanRecruitmentReview()
            {
                HumanRecruitmentProfileID = item.ProfileID,
                HumanRecruitmentCriteriaID = item.CriteriaID,
                Note = item.Note,
                Point = (int)item.Point,
                Time = item.Time == null ? DateTime.Now : (DateTime)item.Time,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RecruitmentReviewDA.Insert(user);
            RecruitmentReviewDA.Save();
        }
        /// <summary>
        /// Cập nhập hoặc thêm mới kết quả xét tuyển
        /// || Author: Thanhnv || CreateDate: 09/01/2015 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void UpdateOrInsert(HumanRecruitmentReviewItem item, int userID)
        {
            var dbo = RecruitmentReviewDA.Repository;
            var rExits = dbo.HumanRecruitmentReviews.FirstOrDefault(i => i.HumanRecruitmentProfileID == item.ProfileID && i.HumanRecruitmentCriteriaID == item.CriteriaID);
            if (rExits == null)
            {
                var review = new HumanRecruitmentReview()
                {
                    HumanRecruitmentProfileID = item.ProfileID,
                    HumanRecruitmentCriteriaID = item.CriteriaID,
                    Note = item.Note,
                    Point = (int)item.Point,
                    Time = item.Time == null ? DateTime.Now : (DateTime)item.Time,
                    CreateAt = DateTime.Now,
                    CreateBy = userID
                };
                dbo.HumanRecruitmentReviews.Add(review);
            }
            else
            {
                rExits.HumanRecruitmentProfileID = item.ProfileID;
                rExits.HumanRecruitmentCriteriaID = item.CriteriaID;
                rExits.Note = item.Note;
                rExits.Point = (int)item.Point;
                if (item.Time != null)
                    rExits.Time = (DateTime)item.Time;
                rExits.UpdateAt = DateTime.Now;
                rExits.UpdateBy = userID;
            }
            dbo.SaveChanges();
        }
        /// <summary>
        /// Xóa kết quả xét tuyển
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentReviewDA.Delete(id);
            RecruitmentReviewDA.Save();
        }
    }
}
