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
    public class HumanRecruitmentPlanRequirementSV
    {
        private HumanRecruitmentPlanRequirementDA PlanRequiredDA = new HumanRecruitmentPlanRequirementDA();
        /// <summary>
        /// Lấy tất cả kế họach gắn với yêu cầu tuyển dụng
        /// || Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanRequirementItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var rprs = PlanRequiredDA.GetQuery()
                        .Select(item => new HumanRecruitmentPlanRequirementItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            RequiredID = item.HumanRecruitmentRequirementID,
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return rprs;
        }

        /// <summary>
        /// Lấy tất cả các yêu cầu tuyển dụng
        /// Trong đó có cả các yêu cầu tuyển dụng đã được lựa chọn bởi kế hoạch
        /// || Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanRequirementItem> GetRequirementSelectByPlan(int page, int pageSize, out int totalCount, int planId)
        {
            var dbo = PlanRequiredDA.Repository;
            var requirements = dbo.HumanRecruitmentRequirements.Where(
                // Yêu cầu đã được phê duyệt thành công
                    r => r.IsAccept == true
                        // Yêu cầu chưa bị xóa
                    && r.IsDelete == false
                        // Yêu cầu chưa được gán bởi kế hoạch nào mà đã được phê duyệt cả ngoại trừ kế hoạch đang được cập nhật
                        // && dbo.HumanRecruitmentPlanRequirement.FirstOrDefault(i => (i.RequiredID == r.ID && i.PlanID != planId) && (i.HumanRecruitmentPlan.IsApproval == true)) == null
                    && !(dbo.HumanRecruitmentProfileInterviews.Where(i => i.HumanRecruitmentRequirementID == r.ID).SelectMany(i => i.HumanRecruitmentProfileResults.Where(pr => pr.IsPass)).Count() >= r.Number)
                    )
                        .Select(item => new HumanRecruitmentPlanRequirementItem()
                        {
                            ID = dbo.HumanRecruitmentPlanRequirements.FirstOrDefault(i => i.HumanRecruitmentPlanID == planId && i.HumanRecruitmentRequirementID == item.ID) != null ? dbo.HumanRecruitmentPlanRequirements.FirstOrDefault(i => i.HumanRecruitmentPlanID == planId && i.HumanRecruitmentRequirementID == item.ID).ID : 0,
                            RequiredID = item.ID,
                            PlanID = planId,
                            Name = item.Name,
                            Reason = item.Reason,
                            IsSelect = dbo.HumanRecruitmentPlanRequirements.FirstOrDefault(i => i.HumanRecruitmentPlanID == planId && i.HumanRecruitmentRequirementID == item.ID) != null
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return requirements;
        }

        /// <summary>
        ///Lấy  chính xác thông của yêu cầu của kế hoạch tuyển dụng
        ///|| Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanRequirementItem> GetRequirementByPlan(int page, int pageSize, out int totalCount, int planId)
        {
            var dbo = PlanRequiredDA.Repository;
            var requirements = dbo.HumanRecruitmentPlanRequirements.Where(i => i.HumanRecruitmentPlanID == planId)
                        .Select(item => new HumanRecruitmentPlanRequirementItem()
                        {
                            ID = item.ID,
                            RequiredID = item.HumanRecruitmentRequirementID,
                            PlanID = planId,
                            Name = item.HumanRecruitmentRequirement.Name,
                            Reason = item.HumanRecruitmentRequirement.Reason
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return requirements;
        }
        /// <summary>
        /// Lấy danh sách yêu cầu cho kế hoạch
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanRequirementItem> GetRequirementByPlan(int planId)
        {
            var dbo = PlanRequiredDA.Repository;
            var requirements = dbo.HumanRecruitmentPlanRequirements.Where(i => i.HumanRecruitmentPlanID == planId)
                        .Select(item => new HumanRecruitmentPlanRequirementItem()
                        {
                            ID = item.ID,
                            RequiredID = item.HumanRecruitmentRequirementID,
                            PlanID = planId,
                            Name = item.HumanRecruitmentRequirement.Name,
                            RoleName = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRecruitmentRequirement.HumanRoleID).Name
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            return requirements;
        }
        /// <summary>
        /// Lấy yêu cầu tuyển dụng găn với kế hoạch theo ID 
        /// || Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentPlanRequirementItem GetById(int Id)
        {
            var dbo = PlanRequiredDA.Repository;
            var rprs = PlanRequiredDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentPlanRequirementItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            RequiredID = item.HumanRecruitmentRequirementID,
                        })
                        .First();
            return rprs;
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentPlanRequirementItem item, int userID)
        {
            var rc = PlanRequiredDA.GetById(item.ID);
            rc.HumanRecruitmentPlanID = item.PlanID;
            rc.HumanRecruitmentRequirementID = item.RequiredID;
            PlanRequiredDA.Save();
        }

        /// <summary>
        /// Thêm mới yêu cầu tuyển dụng trong kế hoạch
        /// || Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="item"></param>
        public void Insert(HumanRecruitmentPlanRequirementItem item)
        {
            var rpr = new HumanRecruitmentPlanRequirement()
            {
                HumanRecruitmentPlanID = item.PlanID,
                HumanRecruitmentRequirementID = item.RequiredID,
            };
            PlanRequiredDA.Insert(rpr);
            PlanRequiredDA.Save();
        }
        /// <summary>
        /// Thêm mới yêu cầu cho danh sách các yêu cầu tuyển dụng trong kế hoạch
        /// || Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="items">danh sách các yêu cầu của kế hoạch</param>
        /// <param name="userID"></param>
        public void InsertRange(List<HumanRecruitmentPlanRequirementItem> items, int userID)
        {
            var rprs = new List<HumanRecruitmentPlanRequirement>();
            rprs = items.Select(item => new HumanRecruitmentPlanRequirement()
            {
                HumanRecruitmentPlanID = item.PlanID,
                HumanRecruitmentRequirementID = item.RequiredID,
            }).ToList();
            PlanRequiredDA.InsertRange(rprs);
            PlanRequiredDA.Save();
        }
        /// <summary>
        ///Xóa yêu cầu tuyển dụng trong kế hoạch
        /// || Author: Thanhnv || CreateDate: 29/12/2014 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            PlanRequiredDA.Delete(id);
            PlanRequiredDA.Save();
        }
    }
}
