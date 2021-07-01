using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;

namespace iDAS.Services
{
    public class HumanRecruitmentRequirementSV
    {
        private HumanRecruitmentRequirementDA RecruitmentRequirementDA = new HumanRecruitmentRequirementDA();
        /// <summary>
        /// Lấy tất cả các yêu cầu tuyển dụng tồn tại
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentRequirementItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = RecruitmentRequirementDA.Repository;
            var users = dbo.HumanRecruitmentRequirements.Where(i => i.IsDelete == false)
                        .Select(item => new HumanRecruitmentRequirementItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Role = new HumanRoleViewItem()
                            {
                                ID = item.HumanRoleID,
                                Name = dbo.HumanRoles.FirstOrDefault(m => m.ID == item.HumanRoleID).Name,
                                Department = dbo.HumanRoles.FirstOrDefault(m => m.ID == item.HumanRoleID).HumanDepartment.Name,
                            },
                            Number = item.Number,
                            Reason = item.Reason,
                            Form = item.Form,
                            DateRequired = item.DateRequired,
                            IsComplete = dbo.HumanRecruitmentProfileResults
                                     .Where(i => i.HumanRecruitmentProfileInterview.HumanRecruitmentRequirementID == item.ID) != null
                                     && dbo.HumanRecruitmentProfileResults
                                     .Where(i => i.HumanRecruitmentProfileInterview.HumanRecruitmentRequirementID == item.ID)
                                     .Count() >= item.Number,
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
        public List<HumanRecruitmentRequirementItem> GetAllByDeptID(ModelFilter filter, int focusId, int DepartmentID)
        {
            var dbo = RecruitmentRequirementDA.Repository;
            var query = dbo.HumanRecruitmentRequirements.Where(i => i.IsDelete == false
                                && (i.HumanRole.HumanDepartment.ID == DepartmentID || DepartmentID == 1));
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var result = query.Filter(filter)
                                .Select(re => new HumanRecruitmentRequirementItem()
                                        {
                                            ID = re.ID,
                                            Name = re.Name,
                                            Role = new HumanRoleViewItem()
                                            {
                                                ID = re.HumanRoleID,
                                                Name = dbo.HumanRoles.FirstOrDefault(m => m.ID == re.HumanRoleID).Name,
                                                Department = dbo.HumanRoles.FirstOrDefault(m => m.ID == re.HumanRoleID).HumanDepartment.Name,
                                            },
                                            Number = re.Number,
                                            Reason = re.Reason,
                                            Form = re.Form,
                                            DateRequired = re.DateRequired,
                                            IsComplete = dbo.HumanRecruitmentProfileInterviews.Where(i=>i.HumanRecruitmentRequirementID == re.ID)
                                                                .SelectMany(i=>i.HumanRecruitmentProfileResults).Count()>= re.Number,
                                            IsEdit = re.IsEdit,
                                            IsApproval = re.IsApproval,
                                            ApprovalAt = re.ApprovalAt,
                                            ApprovalBy = re.ApprovalBy,
                                            ApprovalNote = re.Note,
                                            IsAccept = re.IsAccept,
                                            CreateAt = re.CreateAt,
                                            CreateBy = re.CreateBy,
                                            UpdateAt = re.UpdateAt,
                                            UpdateBy = re.UpdateBy
                                        })
                                        .ToList();
            return result;
        }
        /// <summary>
        /// Lấy danh sách các đề nghị tuyển dụng đã được phê duyệt
        ///  || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentRequirementItem> GetRequirementApproved(int page, int pageSize, out int totalCount)
        {
            var dbo = RecruitmentRequirementDA.Repository;
            var users = dbo.HumanRecruitmentRequirements
                .Where(
                // Yêu cầu đã được phê duyệt
                    r => r.IsAccept == true
                        // Yêu cầu chưa bị xóa
                    && r.IsDelete == false
                        // Yêu cầu chưa được gán bởi kế hoạch nào mà đã được phê duyệt cả
                        //  && dbo.HumanRecruitmentPlanRequirement.FirstOrDefault(i => i.RequiredID == r.ID && (i.HumanRecruitmentPlan.IsApproval == true)) == null
                  && !(dbo.HumanRecruitmentProfileInterviews.Where(i=>i.HumanRecruitmentRequirementID == r.ID).SelectMany(i=>i.HumanRecruitmentProfileResults.Where(pr => pr.IsPass)).Count()>= r.Number)
                    )
                    .Select(item => new HumanRecruitmentRequirementItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Role = new HumanRoleViewItem()
                        {
                            ID = item.HumanRoleID,
                            Name = dbo.HumanRoles.FirstOrDefault(m => m.ID == item.HumanRoleID).Name,
                            Department = dbo.HumanRoles.FirstOrDefault(m => m.ID == item.HumanRoleID).HumanDepartment.Name,
                        },
                        Number = item.Number,
                        Reason = item.Reason,
                        Form = item.Form,
                        DateRequired = item.DateRequired,
                        // IsDelete = item.IsDelete,
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
        /// Lấy yêu cầu tuyển dụng theo ID
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentRequirementItem GetById(int Id)
        {
            var dbo = RecruitmentRequirementDA.Repository;
            var re = dbo.HumanRecruitmentRequirements.Where(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentRequirementItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Role = new HumanRoleViewItem()
                            {
                                ID = item.HumanRoleID,
                                Name = dbo.HumanRoles.FirstOrDefault(m => m.ID == item.HumanRoleID).Name,
                                Department = dbo.HumanRoles.FirstOrDefault(m => m.ID == item.HumanRoleID).HumanDepartment.Name,
                            },
                            Number = item.Number,
                            NumberIsEmployee = dbo.HumanRecruitmentProfileInterviews.Where(i => i.HumanRecruitmentRequirementID == item.ID).SelectMany(i => i.HumanRecruitmentProfileResults.Where(r=>r.IsPass)).Count(),
                            Reason = item.Reason,
                            Form = item.Form,
                            DateRequired = item.DateRequired,
                            // IsDelete = item.IsDelete,
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
                            CreateByName = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.Name,
                            CreateUserID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.ID,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return re;
        }
        /// <summary>
        /// Cập nhật thông tin yêu cầu tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentRequirementItem item, int userID)
        {
            var rr = RecruitmentRequirementDA.GetById(item.ID);
            rr.Name = item.Name;
            if (item.Role.ID != 0)
                rr.HumanRoleID = item.Role.ID;
            rr.Number = item.Number;
            rr.Reason = item.Reason;
            rr.Form = item.Form;
            rr.DateRequired = item.DateRequired;
            if (item.ApprovalBy != null)
            {
                rr.ApprovalBy = item.ApprovalBy;
            }
            rr.IsEdit = item.IsEdit;
            rr.IsApproval = item.IsApproval;
            rr.UpdateAt = DateTime.Now;
            rr.UpdateBy = userID;
            RecruitmentRequirementDA.Save();
        }
        /// <summary>
        ///Thực hiện gửi yêu cầu hoặc Phê duyệt yêu cầu tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        public void Approve(HumanRecruitmentRequirementItem item)
        {
            var rr = RecruitmentRequirementDA.GetById(item.ID);
            rr.IsEdit = item.IsEdit;
            rr.IsApproval = item.IsApproval;
            rr.ApprovalAt = item.ApprovalAt;
            rr.ApprovalBy = item.ApprovalBy;
            rr.Note = item.ApprovalNote;
            rr.IsAccept = item.IsAccept;
            rr.ApprovalBy = item.ApprovalBy;
            rr.UpdateBy = rr.UpdateBy;
            rr.UpdateAt = DateTime.Now;
            RecruitmentRequirementDA.Save();
        }

        /// <summary>
        /// Đóng yêu cầu để thực hiện khi đã có người phê duyệt
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="ID"></param>
        public void Close(int ID)
        {
            var rr = RecruitmentRequirementDA.GetById(ID);
            rr.IsAccept = true;
            RecruitmentRequirementDA.Save();
        }
        /// <summary>
        /// Gửi bản ghi
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userID"></param>
        public void SendApprove(HumanRecruitmentRequirementItem  item, int userID)
        {
            var rc = RecruitmentRequirementDA.GetById(item.ID);
            rc.ApprovalBy = item.ApprovalBy;
            rc.IsEdit = item.IsEdit;
            rc.UpdateAt = DateTime.Now;
            rc.UpdateBy = userID;
            RecruitmentRequirementDA.Save();
        }
        /// <summary>
        /// Thêm mới yêu cầu tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentRequirementItem item, int userID, out int Id)
        {
            var user = new HumanRecruitmentRequirement()
            {
                Name = item.Name,
                HumanRoleID = item.Role.ID,
                Number = item.Number,
                Reason = item.Reason,
                Form = item.Form,
                DateRequired = item.DateRequired,
                IsDelete = false,
                IsEdit = item.IsEdit,
                IsApproval = item.IsApproval,
                ApprovalAt = item.ApprovalAt,
                ApprovalBy = item.ApprovalBy,
                Note = item.ApprovalNote,
                IsAccept = item.IsAccept,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RecruitmentRequirementDA.Insert(user);
            RecruitmentRequirementDA.Save();
            Id = user.ID;
        }
        /// <summary>
        /// Xóa yêu cầu tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentRequirementDA.Delete(id);
            RecruitmentRequirementDA.Save();
        }
        /// <summary>
        /// Xóa trạng thái yêu cầu tuyển dụng với bản ghi đang có ràng buộc dữ liệu
        /// || Author: Thanhnv || CreateDate: 27/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void IsDelete(int id)
        {
            var rr = RecruitmentRequirementDA.GetById(id);
            rr.IsDelete = true;
            RecruitmentRequirementDA.Save();
        }
        /// <summary>
        /// Xóa yêu cầu tuyển dụng và các thông tin phê duyệt
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRequirAndApprov(int id)
        {
            var dbo = RecruitmentRequirementDA.Repository;
            dbo.HumanRecruitmentRequirements.Remove(dbo.HumanRecruitmentRequirements.First(i => i.ID == id));
            RecruitmentRequirementDA.Save();
        }
    }
}
