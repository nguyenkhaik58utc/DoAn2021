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
    public class HumanRecruitmentProfileInterviewSV
    {
        private HumanRecruitmentProfileInterviewDA RecruitmentProfileInterviewDA = new HumanRecruitmentProfileInterviewDA();
        /// <summary>
        /// Lấy tất cả các bộ hồ sơ lọc để phỏng vấn
        ///|| Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentProfileInterviewItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = RecruitmentProfileInterviewDA.GetQuery()
                        .Select(item => new HumanRecruitmentProfileInterviewItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            ProfileID = item.HumanRecruitmentProfileID,
                            RequirementID = item.HumanRecruitmentRequirementID,
                            IsEdit = item.IsEdit,
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
        /// Lấy hồ sơ ứng tuyển theo ID
        ///|| Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentProfileInterviewItem GetById(int Id)
        {
            var dbo = RecruitmentProfileInterviewDA.Repository;
            var users = RecruitmentProfileInterviewDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentProfileInterviewItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            ProfileID = item.HumanRecruitmentProfileID,
                            RequirementID = item.HumanRecruitmentRequirementID,
                            IsEdit = item.IsEdit,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Cập nhật hồ sơ ứng tuyển
        ///|| Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentProfileInterviewItem item, int userID)
        {
            var rp = RecruitmentProfileInterviewDA.GetById(item.ID);
            rp.ID = item.ID;
            rp.HumanRecruitmentPlanID = item.PlanID;
            rp.HumanRecruitmentProfileID = item.ProfileID;
            rp.HumanRecruitmentRequirementID = item.RequirementID;
            rp.CreateAt = item.CreateAt;
            rp.CreateBy = item.CreateBy;
            rp.UpdateAt = item.UpdateAt;
            rp.UpdateBy = item.UpdateBy;
            RecruitmentProfileInterviewDA.Save();
        }
        /// <summary>
        /// Thêm mới hồ sơ ứng tuyển
        ///|| Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentProfileInterviewItem item, int userID)
        {
            var user = new HumanRecruitmentProfileInterview()
            {
                HumanRecruitmentPlanID = item.PlanID,
                HumanRecruitmentProfileID = item.ProfileID,
                HumanRecruitmentRequirementID = item.RequirementID,
                IsEdit =true,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            RecruitmentProfileInterviewDA.Insert(user);
            RecruitmentProfileInterviewDA.Save();
        }
        /// <summary>
        /// Thêm mới hồ sơ ứng tuyển nếu chưa có mã yêu cầu mặc định lấy yêu cầu đầu tiên
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void InsertOrDefaulRequirment(HumanRecruitmentProfileInterviewItem item, int userID, int PlanID)
        {
            var dbo = RecruitmentProfileInterviewDA.Repository; 
            var ProfileInterview = new HumanRecruitmentProfileInterview()
            {
                HumanRecruitmentPlanID = item.PlanID,
                HumanRecruitmentProfileID = item.ProfileID,
                IsEdit = true,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            if (item.RequirementID != 0)
            {
                ProfileInterview.HumanRecruitmentProfileID= item.RequirementID;
            }
            else ProfileInterview.HumanRecruitmentRequirementID = dbo.HumanRecruitmentPlanRequirements.First(i => i.HumanRecruitmentPlanID == PlanID).HumanRecruitmentRequirementID;
            dbo.HumanRecruitmentProfileInterviews.Add(ProfileInterview);
            dbo.SaveChanges();
        }
        /// <summary>
        /// Cập nhật mã yêu cầu tuyển dụng
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="RequirementID"></param>
        public void UpdateRequirement(int ProfileID, int RequirementID)
        {
            var dbo = RecruitmentProfileInterviewDA.Repository;
            var profileInterview=dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(i => i.HumanRecruitmentProfileID == ProfileID);
            if(profileInterview != null)
            {
                profileInterview.HumanRecruitmentRequirementID = RequirementID;
                dbo.SaveChanges();
            };
        }
        /// <summary>
        /// Xóa hồ sơ ứng tuyển
        ///|| Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentProfileInterviewDA.Delete(id);
            RecruitmentProfileInterviewDA.Save();
        }

        public List<HumanRecruitmentPlanRequirementItem> GetRequirementByPlan(int PlanID)
        {
            return new HumanRecruitmentPlanRequirementSV().GetRequirementByPlan(PlanID);
        }

        public List<RecruitmentProfileIneterviewSelectItem> GetAllByPlanID(int page, int pageSize, out int totalCount, int PlanID)
        {
            return new HumanRecruitmentProfileSV().GetAllByPlanID(page, pageSize,out totalCount, PlanID);
        }
        public List<RecruitmentProfileIneterviewSelectItem> GetAllNotIsEmpByPlanID(int page, int pageSize, out int totalCount, int PlanID)
        {
            return new HumanRecruitmentProfileSV().GetAllNotIsEmpByPlanID(page, pageSize, out totalCount, PlanID);
        }
        public void GetResultByProfileInterViewID(int ProfileInterviewID, out HumanRecruitmentResultItem data)
        {
            new HumanRecruitmentResultSV().GetResultByProfileInterViewID(ProfileInterviewID, out data);
        }

        public HumanRecruitmentResultItem GetResultByID(int ProfileResultID)
        {
            return new HumanRecruitmentResultSV().GetById(ProfileResultID);
        }

        public List<RecruitmentProfileIneterviewSelectItem> GetProfileByPlan(ModelFilter filter, int focusId, int PlanID)
        {
            return new HumanRecruitmentProfileSV().GetByPlanID(filter,focusId, PlanID);
        }

        public void SendProfile(HumanRecruitmentResultItem updateData, int userID)
        {
            new HumanRecruitmentResultSV().SendProfile(updateData, userID);
        }

        public void UpdateResult(HumanRecruitmentResultItem updateData, int userID)
        {
            new HumanRecruitmentResultSV().Update(updateData, userID);
        }
    }
}
