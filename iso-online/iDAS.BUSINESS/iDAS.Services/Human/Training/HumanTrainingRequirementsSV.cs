using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.Services;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class HumanTrainingRequirementSV
    {
        private HumanTrainingRequirementDA TrainingRequirementDA = new HumanTrainingRequirementDA();
        /// <summary>
        /// Lấy tất cả các yêu cầu đào tạo tồn tại
         
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanTrainingRequirementItem> GetAll(ModelFilter filter, int focusId=0)
        {
            var dbo = TrainingRequirementDA.Repository;
            IQueryable<iDAS.Base.HumanTrainingRequirement> query = dbo.HumanTrainingRequirements;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }

            var users = query.Filter(filter)
                        .Select(item => new HumanTrainingRequirementItem()
                        {
                            ID = item.ID,
                            RequireBy = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(
                                i => i.ID == item.EmployeeID
                                ).Name,
                            },
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            IsAccept = item.IsAccept,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy danh sách các đề nghị đào tạo đã được phê duyệt
        ///  || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanTrainingRequirementItem> GetRequirementApproved(int page, int pageSize, out int totalCount)
        {
            var dbo = TrainingRequirementDA.Repository;
            var users = dbo.HumanTrainingRequirements
                .Where(
                // Yêu cầu đã được phê duyệt
                    r => r.IsAccept == true
                // Yêu cầu chưa được gán bởi kế hoạch nào mà đã được phê duyệt cả
                //  && dbo.HumanTrainingPlanRequirement.FirstOrDefault(i => i.RequiredID == r.ID && (i.HumanTrainingPlan.IsApproval == true)) == null
                    )
                    .Select(item => new HumanTrainingRequirementItem()
                    {
                        ID = item.ID,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        IsEdit = item.IsEdit,
                        IsApproval = item.IsApproval,
                        ApprovalAt = item.ApprovalAt,
                        ApprovalBy = item.ApprovalBy,
                        ApprovalNote = item.Note,
                        IsAccept = item.IsAccept,
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
        /// Lấy yêu cầu đào tạo theo ID
         
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanTrainingRequirementItem GetById(int Id)
        {
            var dbo = TrainingRequirementDA.Repository;
            var re = dbo.HumanTrainingRequirements.Where(i => i.ID == Id)
                        .Select(item => new HumanTrainingRequirementItem()
                        {
                            ID = item.ID,
                            RequireBy = new HumanEmployeeViewItem()
                            {
                                ID = item.EmployeeID == null ? 0 : item.EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).Name,
                                Role = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                Department = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                            },
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsAccept = item.IsAccept,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            Approval = new HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy == null ? 0 : (int)item.ApprovalBy,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                Department = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                            },
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.Name,
                            CreateUserID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.ID
                        })
                        .First();
            return re;
        }
        /// <summary>
        /// Cập nhật thông tin yêu cầu đào tạo
         
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanTrainingRequirementItem item, int userID)
        {
            var rr = TrainingRequirementDA.GetById(item.ID);
            rr.EmployeeID = item.RequireBy.ID;
            if (item.ApprovalBy != null)
            {
                rr.ApprovalBy = item.ApprovalBy;
            }
            rr.StartTime = item.StartTime;
            rr.EndTime = item.EndTime;
            rr.Content = item.Content;
            rr.UpdateAt = DateTime.Now;
            rr.UpdateBy = userID;
            TrainingRequirementDA.Save();
        }
        /// <summary>
        ///Thực hiện gửi yêu cầu hoặc Phê duyệt yêu cầu đào tạo
         
        /// </summary>
        /// <param name="item"></param>
        public void Approve(HumanTrainingRequirementItem item)
        {
            var resultItem = TrainingRequirementDA.GetById(item.ID);
            resultItem.IsEdit = item.IsEdit;
            resultItem.IsApproval = item.IsApproval;
            resultItem.ApprovalAt = item.ApprovalAt;
            resultItem.ApprovalBy = item.ApprovalBy;
            resultItem.Note = item.ApprovalNote;
            resultItem.IsAccept = item.IsAccept;
            resultItem.ApprovalBy = item.ApprovalBy;
            resultItem.UpdateBy = resultItem.UpdateBy;
            resultItem.UpdateAt = DateTime.Now;
            TrainingRequirementDA.Save();
        }
        /// Gửi bản ghi
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userID"></param>
        public void SendApprove(HumanTrainingRequirementItem item, int userID)
        {
            var rc = TrainingRequirementDA.GetById(item.ID);
            rc.ApprovalBy = item.ApprovalBy;
            rc.IsEdit = item.IsEdit;
            rc.IsApproval = item.IsApproval;
            rc.UpdateAt = DateTime.Now;
            rc.UpdateBy = userID;
            TrainingRequirementDA.Save();
        }
        /// <summary>
        /// Thêm mới yêu cầu đào tạo
         
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanTrainingRequirementItem item, int userID)
        {
            var result = new HumanTrainingRequirement()
            {
                EmployeeID = item.RequireBy.ID,
                Content = item.Content,
                IsEdit = item.IsEdit,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            TrainingRequirementDA.Insert(result);
            TrainingRequirementDA.Save();
        }
        /// <summary>
        /// Xóa yêu cầu đào tạo
         
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            TrainingRequirementDA.Delete(id);
            TrainingRequirementDA.Save();
        }
    }
}
