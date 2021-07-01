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
    public class HumanRecruitmentPlanSV
    {
        private HumanRecruitmentPlanDA RecruitmentPlanDA = new HumanRecruitmentPlanDA();
        /// <summary>
        /// Lấy tất cả kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanItem> GetAll(ModelFilter filter, int focusId = 0)
        {
            var dbo = RecruitmentPlanDA.Repository;
            IQueryable<iDAS.Base.HumanRecruitmentPlan> query = dbo.HumanRecruitmentPlans.Where(i => i.IsDelete == false);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var users = query.Filter(filter)
                        .Select(item => new HumanRecruitmentPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TotalCost = item.TotalCost,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })                        
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy kế hoạch theo đơn vị
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanItem> GetByDepartmentID(int page, int pageSize, out int totalCount, int DepartmentID)
        {
            var dbo = RecruitmentPlanDA.Repository;
            var users = dbo.HumanRecruitmentPlans.Where
                (i => i.IsDelete == false
                     && dbo.HumanRecruitmentPlanRequirements
                            .FirstOrDefault(r => r.HumanRecruitmentRequirement.HumanRole.HumanDepartmentID == DepartmentID && r.HumanRecruitmentPlan.ID == i.ID) != null)
                        .Select(item => new HumanRecruitmentPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TotalCost = item.TotalCost,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
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
        /// Lấy tất cả kế hoạch tuyển dụng đã được phê duyệt
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanItem> GetAllApprovedSuccess(int page, int pageSize, out int totalCount)
        {
            var users = RecruitmentPlanDA.GetQuery(i => i.IsDelete == false && i.IsApproval && i.IsAccept)
                        .Select(item => new HumanRecruitmentPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TotalCost = item.TotalCost,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Content = item.Content,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsAccept = item.IsAccept,
                            IsDelete = item.IsDelete,
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
        /// Lấy kế hoạch tuyển dụng theo mã kế hoạch
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentPlanItem GetById(int Id)
        {
            var dbo = RecruitmentPlanDA.Repository;
            var users = dbo.HumanRecruitmentPlans.Where(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TotalCost = item.TotalCost,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Content = item.Content,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsAccept = item.IsAccept,
                            IsEdit = item.IsEdit,
                            IsResult = item.IsApproval ? (bool?)(item.IsAccept ? true : false) : null,
                            IsDelete = item.IsDelete,
                            Approval = new HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name,
                            },
                            ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy,
                            CreateUserID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.ID
                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Đóng kế hoạch khi đã có người phê duyệt
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="ID"></param>
        public void Closed(int ID)
        {
            var pl = RecruitmentPlanDA.GetById(ID);
            //pl.IsClosed = true;
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        /// Gửi bản ghi cho lãnh đạo phê duyệt IsEdit = false; Lãnh đạo trả về để sửa IsSend = false
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userID"></param>
        public void SendApproval(HumanRecruitmentPlanItem rePlan)
        {
            var pl = RecruitmentPlanDA.GetById(rePlan.ID);
            pl.IsApproval = rePlan.IsApproval;
            pl.IsEdit = rePlan.IsEdit;
            pl.ApprovalBy = rePlan.ApprovalBy;
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        ///Phê duyệt kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="ID">Mã kế hoạch</param>
        /// <param name="UserID"> Người phê duyệt</param>
        /// <param name="note"> Ghi chú phê duyệt</param>
        public void Approve(HumanRecruitmentPlanItem PlanApprovalItem)
        {
            var pl = RecruitmentPlanDA.GetById(PlanApprovalItem.ID);
            pl.IsApproval = true;
            pl.IsEdit = PlanApprovalItem.IsEdit;
            pl.ApprovalAt = PlanApprovalItem.ApprovalAt;
            pl.IsAccept = PlanApprovalItem.IsAccept;
            pl.Note = PlanApprovalItem.ApprovalNote;
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        ///Trả lại kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="ID">Mã kế hoạch</param>
        /// <param name="UserID"> Người phê duyệt</param>
        /// <param name="note"> Ghi chú lý do trả lại</param>
        public void Return(int ID, int UserID, string note)
        {
            var pl = RecruitmentPlanDA.GetById(ID);
            pl.IsApproval = true;
            pl.IsAccept = false;
            pl.ApprovalBy = UserID;
            pl.ApprovalAt = DateTime.Now;
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="cost"></param>
        public void SetTotalCost(int ID, decimal cost)
        {
            var rp = RecruitmentPlanDA.GetById(ID);
            if (rp.TotalCost == null)
            {
                rp.TotalCost = cost;
            }
            {
                rp.TotalCost += cost;
            };
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// Cập nhật kế hoạch tuyển dụng
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentPlanItem item, int userID)
        {
            var rp = RecruitmentPlanDA.GetById(item.ID);
            rp.Name = item.Name;
            rp.Content = item.Content;
            rp.TotalCost = item.TotalCost;
            rp.StartDate = item.StartDate;
            rp.EndDate = item.EndDate;
            rp.IsApproval = item.IsApproval;
            rp.ApprovalAt = item.ApprovalAt;
            rp.ApprovalBy = item.ApprovalBy;
            rp.Note = item.ApprovalNote;
            rp.IsAccept = item.IsAccept;
            rp.IsEdit = item.IsEdit;
            rp.IsDelete = item.IsDelete;
            rp.UpdateAt = DateTime.Now;
            rp.UpdateBy = userID;
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        /// Cập nhật các thông tin cơ bản của kế hoạch tuyển dụng
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void UpdateInfo(HumanRecruitmentPlanItem item, int userID)
        {
            var rp = RecruitmentPlanDA.GetById(item.ID);
            rp.Name = item.Name;
            rp.StartDate = item.StartDate;
            rp.EndDate = item.EndDate;
            rp.TotalCost = item.TotalCost;
            rp.Content = item.Content;
            rp.UpdateAt = DateTime.Now;
            rp.UpdateBy = userID;
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        /// Thêm mới kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentPlanItem item, int userID, out int planID)
        {
            var dbo = RecruitmentPlanDA.Repository;
            var plan = new HumanRecruitmentPlan()
            {
                Name = item.Name,
                TotalCost = item.TotalCost,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Content = item.Content,
                // Thêm mới mặc đinh là chưa xóa và cho phép sửa
                IsEdit = true,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            dbo.HumanRecruitmentPlans.Add(plan);
            dbo.SaveChanges();
            planID = plan.ID;
            if (!string.IsNullOrEmpty(item.strRequirmentIDs))
            {
                var planRequirements = new List<HumanRecruitmentPlanRequirement>();
                var arrIds = item.strRequirmentIDs.Substring(0, item.strRequirmentIDs.Length - 1).Split(',');
                foreach (var arr in arrIds)
                {
                    var planRequirement = new HumanRecruitmentPlanRequirement()
                    {
                        HumanRecruitmentRequirementID = int.Parse(arr),
                        HumanRecruitmentPlanID = planID
                    };
                    planRequirements.Add(planRequirement);
                }
                dbo.HumanRecruitmentPlanRequirements.AddRange(planRequirements);
                dbo.SaveChanges();
            };
        }
        /// <summary>
        /// Xóa kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var dbo = RecruitmentPlanDA.Repository;
            dbo.HumanRecruitmentPlanRequirements.RemoveRange(dbo.HumanRecruitmentPlanRequirements.Where(i => i.HumanRecruitmentPlanID == id));
            dbo.HumanRecruitmentPlans.Remove(dbo.HumanRecruitmentPlans.FirstOrDefault(i => i.ID == id));
            RecruitmentPlanDA.Save();
        }
        /// <summary>
        /// Xóa tạm kế hoạch tuyển dụng khi kế hoạch đang được sử dụng ở bản ghi khác
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        public void isDelete(int id)
        {
            var rp = RecruitmentPlanDA.GetById(id);
            rp.IsDelete = true;
            RecruitmentPlanDA.Save();
        }

        public List<HumanRecruitmentPlanRequirementItem> GetRequirementSelect(int page, int pageSize, out int totalCount, int Id)
        {
            return new HumanRecruitmentPlanRequirementSV().GetRequirementSelectByPlan(page, pageSize, out totalCount, Id);
        }
        public List<HumanRecruitmentPlanRequirementItem> GetRequirement(int page, int pageSize, out int totalCount, int Id)
        {
            return new HumanRecruitmentPlanRequirementSV().GetRequirementByPlan(page, pageSize, out totalCount, Id);
        }
        public List<HumanRecruitmentRequirementItem> RequirementApproved(int page, int pageSize, out int totalCount)
        {
            return new HumanRecruitmentRequirementSV().GetRequirementApproved(page, pageSize, out totalCount);
        }
        public void InsertTask(HumanRecruitmentPlanTaskItem data, int userId)
        {
            new HumanRecruitmentPlanTaskSV().Insert(data, userId);
        }

        public List<HumanRecruitmentPlanTaskItem> GetTreeTask(int taskId,int planId)
        {
            return new HumanRecruitmentPlanTaskSV().GetTreeTask(taskId, planId);
        }

        public void UpdateRequired(HumanRecruitmentPlanRequirementItem PlanRequirementdata)
        {
            var requirementSV= new HumanRecruitmentPlanRequirementSV();
            if (PlanRequirementdata.ID == 0)
            {
                var PlanRequiredItem = new HumanRecruitmentPlanRequirementItem();
                PlanRequiredItem.RequiredID = PlanRequirementdata.RequiredID;
                PlanRequiredItem.PlanID = PlanRequirementdata.PlanID;
                requirementSV.Insert(PlanRequiredItem);
            }
            else
                requirementSV.Delete(PlanRequirementdata.ID);
        }
       
    }
}
