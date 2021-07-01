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
    public class HumanTrainingPlanRequirementSV
    {
        private HumanTrainingPlanRequirementDA TrainingPlanRequirementDA = new HumanTrainingPlanRequirementDA();
        /// <summary>
        /// Lấy tất cả các yêu cầu đào tạo tồn tại
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanTrainingRequirementSelectItem> GetSelectByPlan(int page, int pageSize, out int totalCount,int PlanId)
        {
            var dbo = TrainingPlanRequirementDA.Repository;
            var result = dbo.HumanTrainingRequirements
                        .Where(i=>
                            i.IsApproval && i.IsAccept
                            && (i.HumanTrainingPlanRequirements.FirstOrDefault(j=>j.HumanTrainingPlanID != PlanId) == null
                                //||
                                //i.HumanTrainingPlanRequirements
                                //    .FirstOrDefault(j => j.HumanTrainingPlan.QualityPlans.IsApproval&& j.HumanTrainingPlan.QualityPlan.IsAccept) == null
                                )
                            )
                        .Select(item => new HumanTrainingRequirementSelectItem()
                        {
                            ID = item.ID,
                            RequireBy = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(i => i.ID == item.EmployeeID).Name,
                            },
                            Content = item.Content,
                            IsSelected = item.HumanTrainingPlanRequirements.FirstOrDefault(i=>i.HumanTrainingPlanID == PlanId) == null? false : true ,
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public List<HumanTrainingRequirementItem> GetByPlan(int page, int pageSize, out int totalCount, int PlanId)
        {
            var dbo = TrainingPlanRequirementDA.Repository;
            var result = dbo.HumanTrainingPlanRequirements
                        .Where(i => i.HumanTrainingPlanID == PlanId
                            )
                        .Select(item=>item.HumanTrainingRequirement)
                        .Select(item => new HumanTrainingRequirementItem()
                        {
                            ID = item.ID,
                            RequireBy = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(i => i.ID == item.EmployeeID).Name,
                            },
                            Content = item.Content
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public void Insert(HumanTrainingPlanRequirementItem item, int userID)
        {
            var result = new HumanTrainingPlanRequirement()
            {
                HumanTrainingPlanID = item.PlanID,
                HumanTrainingRequirementID = item.RequirementID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            TrainingPlanRequirementDA.Insert(result);
            TrainingPlanRequirementDA.Save();
        }
        public void Delete(int id)
        {
            TrainingPlanRequirementDA.Delete(id);
            TrainingPlanRequirementDA.Save();
        }
    }
}
