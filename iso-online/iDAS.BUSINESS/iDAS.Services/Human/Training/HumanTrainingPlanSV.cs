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
    public class HumanTrainingPlanSV
    {
        private HumanTrainingPlanDA TrainingPlanDA = new HumanTrainingPlanDA();
        public List<HumanTrainingPlanItem> GetPlanByDepartment(ModelFilter filter, int focusId, int Department)
        {
            var dbo = TrainingPlanDA.Repository;
            var query = TrainingPlanDA.GetQuery(i => Department == 1 || Department == 0
                || i.QualityPlan.DepartmentID == Department
                );
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.QualityPlanID == focusId ? false : true);
            }
            var data = query.Filter(filter)
                            .Select(item =>
                                new HumanTrainingPlanItem()
                                    {
                                        ID = item.ID,
                                        QuanlityPlanID = item.QualityPlan.ID,
                                        Name = item.QualityPlan.Name,
                                        TargetID = item.QualityPlan.QualityTargetID,
                                        Department = new HumanDepartmentViewItem()
                                        {
                                            ID = item.QualityPlan.DepartmentID,
                                            Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.QualityPlan.DepartmentID).Name
                                        },
                                        ParentID = item.QualityPlan.ParentID,
                                        Type = item.QualityPlan.Type,
                                        Cost = item.QualityPlan.Cost,
                                        StartTime = item.QualityPlan.StartTime,
                                        EndTime = item.QualityPlan.EndTime,
                                        Content = item.QualityPlan.Content,
                                        IsEdit = item.QualityPlan.IsEdit,
                                        IsApproval = item.QualityPlan.IsApproval,
                                        IsAccept = item.QualityPlan.IsAccept,
                                        ApprovalAt = item.QualityPlan.ApprovalAt,
                                        ApprovalBy = item.QualityPlan.ApprovalBy,
                                        IsDelete = item.QualityPlan.IsDelete,
                                        CreateAt = item.QualityPlan.CreateAt,
                                        CreateBy = item.QualityPlan.CreateBy,
                                        UpdateAt = item.QualityPlan.UpdateAt,
                                        UpdateBy = item.QualityPlan.UpdateBy
                                    })
            .ToList();
            return data;
        }
        /// <summary>
        /// Author:CuongPC : Lấy danh sách kế hoạch được phê duyệt cho quản lý đào tạo
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<HumanTrainingPlanItem> GetPlanIsAccept(int page, int pageSize, out int total)
        {
            var dbo = TrainingPlanDA.Repository;
            var data = dbo.HumanTrainingPlans.Join(dbo.QualityPlans.Where(s => s.IsAccept && s.IsApproval), t => t.QualityPlanID, p => p.ID, (t, p) => new HumanTrainingPlanItem()
                            {
                                ID = t.ID,
                                Name = p.Name,
                                CreateAt = p.CreateAt
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out total)
                            .ToList();
            return data;
        }

        public HumanTrainingPlanItem GetById(int Id)
        {
            var dbo = TrainingPlanDA.Repository;
            var item = dbo.HumanTrainingPlans.Where(i => i.ID == Id)
                            .Select(i => i.QualityPlan)
                            .Select(i => i)
                            .First();
            if (item != null)
            {
                return new HumanTrainingPlanItem()
                            {
                                ID = Id,
                                QuanlityPlanID = item.ID,
                                Name = item.Name,
                                TargetID = item.QualityTargetID,
                                TargetName = item.QualityTargetID == null ? "" : iDAS.Utilities.Common.GetFormName(0) + item.QualityTarget.Value + " " + item.QualityTarget.Unit,
                                Department = new HumanDepartmentViewItem()
                                {
                                    ID = item.DepartmentID,
                                    Name = dbo.HumanDepartments.Where(d => d.ID == item.DepartmentID).Select(i => i.Name).FirstOrDefault(),
                                },
                                ParentID = item.ParentID,
                                ParentName = dbo.QualityPlans.Where(i => i.ID == item.ParentID).Select(i => i.Name).FirstOrDefault(),
                                Type = item.Type,
                                ApprovalNote = item.Note,
                                Cost = item.Cost,
                                StartTime = item.StartTime,
                                EndTime = item.EndTime,
                                Content = item.Content,
                                IsApproval = item.IsApproval,
                                ApprovalAt = item.ApprovalAt,
                                ApprovalBy = item.ApprovalBy,
                                IsAccept = item.IsAccept,
                                IsEdit = item.IsEdit,
                                IsDelete = item.IsDelete,
                                Approval = new HumanEmployeeViewItem()
                                {
                                    ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                    Name = dbo.HumanEmployees.Where(m => m.ID == item.ApprovalBy).Select(i => i.Name).FirstOrDefault(),
                                    Role = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == item.ApprovalBy).Select(i => i.HumanRole == null ? "" : i.HumanRole.Name).FirstOrDefault(),
                                    Department = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == item.ApprovalBy).Select(i => i.HumanRole == null ? "" : i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                                },
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                                UpdateAt = item.UpdateAt,
                                //UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                                UpdateBy = item.UpdateBy,
                                CreateUserID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.ID
                            };
            }
            return null;

        }
        public int Insert(HumanTrainingPlanItem item, int userID)
        {
            var dbo = TrainingPlanDA.Repository;
            var result = new HumanTrainingPlan()
            {
                QualityPlan = new QualityPlan()
                {
                    Name = item.Name,
                    QualityTargetID = item.TargetID,
                    DepartmentID = item.Department.ID,
                    ParentID = item.ParentID,
                    Type = item.Type,
                    Note = item.ApprovalNote,
                    Cost = item.Cost,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Content = item.Content,
                    IsDelete = false,
                    IsEdit = item.IsEdit,
                    IsApproval = item.IsApproval,
                    ApprovalBy = item.ApprovalBy,
                    CreateAt = DateTime.Now,
                    CreateBy = userID,
                },
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            dbo.HumanTrainingPlans.Add(result);
            dbo.SaveChanges();
            return result.ID;
        }
        public void Delete(int id)
        {
            var dbo = TrainingPlanDA.Repository;
            var trainingPlanDelete = dbo.HumanTrainingPlans.FirstOrDefault(i => i.ID == id);
            var planQuality = dbo.QualityPlans.FirstOrDefault(i => i.ID == trainingPlanDelete.QualityPlanID);
            dbo.HumanTrainingPlans.Remove(trainingPlanDelete);
            dbo.QualityPlans.Remove(planQuality);
            dbo.SaveChanges();
        }

        public List<HumanTrainingRequirementItem> GetRequireByPlan(int page, int pageSize, out int totalCount, int PlanId)
        {
            return new HumanTrainingPlanRequirementSV().GetByPlan(page, pageSize, out totalCount, PlanId);
        }

        public List<HumanTrainingRequirementSelectItem> GetRequireSelectByPlan(int page, int pageSize, out int totalCount, int PlanId)
        {
            return new HumanTrainingPlanRequirementSV().GetSelectByPlan(page, pageSize, out totalCount, PlanId);
        }

        public void InsertRequirement(HumanTrainingPlanRequirementItem PlanRequiredItem, int userID)
        {
            new HumanTrainingPlanRequirementSV().Insert(PlanRequiredItem, userID);
        }

        public void DeleteRequire(int id)
        {
            new HumanTrainingPlanRequirementSV().Delete(id);
        }
    }
}
