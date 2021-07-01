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
    public class HumanRecruitmentResultSV
    {
        private HumanRecruitmentResultDA RecruitmentResultDA = new HumanRecruitmentResultDA();
        /// <summary>
        /// Lấy tất cả danh sách kết quả tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentResultItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = RecruitmentResultDA.GetQuery()
                        .Select(item => new HumanRecruitmentResultItem()
                        {
                            ID = item.ID,
                            ProfileInterviewID = item.HumanRecruitmentProfileInterviewID,
                            Salary = item.Salary,
                            // Thời gian bắt đầu làm việc
                            StartTime = item.StartTime,
                            TotalPoint = item.TotalPoint,
                            IsAccept = item.IsPass,
                            IsApproval = item.IsApproval,
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
        /// Lấy danh sách hồ sơ trúng tuyển với kế hoạch tuyển dụng và yêu cầu tuyển dụng của kế hoạch đó
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlanID"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns>
        public List<RecruitmentProfileResultItem> GetProfilePass(int page, int pageSize, out int totalCount, int PlanID, int RequirementID)
        {
            var dbo = RecruitmentResultDA.Repository;
            var profiles = dbo.HumanRecruitmentProfileResults
                           .Where(item =>
                                item.IsPass == true
                                && item.HumanRecruitmentProfileInterview.HumanRecruitmentPlanID == PlanID
                                && item.HumanRecruitmentProfileInterview.HumanRecruitmentRequirementID == RequirementID
                                )

                        .Select(item => new RecruitmentProfileResultItem()
                        {
                            ID = item.ID,
                            Name = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Name,
                            Avatar = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Avatar,
                            Birthday = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Birthday,
                            ProfileID = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.ID,
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return profiles;
        }

        /// <summary>
        /// Lấy danh sách các hồ sơ đã đề xuất của kế hoạch
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public List<RecruitmentProfileResultItem> GetProfileResultByPlan(int page, int pageSize, out int totalCount, int PlanID)
        {
            var dbo = RecruitmentResultDA.Repository;
            var profiles = dbo.HumanRecruitmentProfileResults
                           .Where(item => item.HumanRecruitmentProfileInterview.HumanRecruitmentPlanID == PlanID)

                        .Select(item => new RecruitmentProfileResultItem()
                        {
                            ID = item.ID,
                            Name = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Name,
                            Avatar = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Avatar,
                            Birthday = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Birthday,
                            Gender = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Gender,
                            IsEmployee = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.IsEmployee,
                            IsPass = item.IsPass,
                            EmployeeID = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.EmployeeID,
                            ProfileID = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.ID,
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            foreach(var item in profiles)
            {
                if(item.EmployeeID>0 && dbo.HumanEmployees.Any(x=>x.ID == item.EmployeeID))
                {
                    item.IsTrial = dbo.HumanEmployees.FirstOrDefault(x=>x.ID == item.EmployeeID).IsTrial;
                    item.IsEmployee = !item.IsTrial;
                }
            }
            return profiles;
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentResultItem GetById(int Id)
        {
            var dbo = RecruitmentResultDA.Repository;
            var resultProfile = dbo.HumanRecruitmentProfileResults.Where(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentResultItem()
                        {
                            ID = item.ID,
                            ProfileName = item.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.Name,
                            PlanName = item.HumanRecruitmentProfileInterview.HumanRecruitmentPlan.Name,
                            PlanID = item.HumanRecruitmentProfileInterview.HumanRecruitmentPlan.ID,
                            RequirementID = item.HumanRecruitmentProfileInterview.HumanRecruitmentRequirementID,
                            RoleName = dbo.HumanRecruitmentRequirements.FirstOrDefault(re => re.ID == item.HumanRecruitmentProfileInterview.HumanRecruitmentRequirementID).HumanRole.Name,
                            ProfileInterviewID = item.HumanRecruitmentProfileInterviewID,
                            ApprovalNote = item.Note,
                            Salary = item.Salary,
                            // Thời gian bắt đầu làm việc
                            StartTime = item.StartTime,
                            TotalPoint = item.TotalPoint,
                            CriteriaMinPoint = dbo.HumanRecruitmentCriterias
                                                 .Where(i => (dbo.HumanRecruitmentRequirements.FirstOrDefault(re => re.HumanRoleID == i.HumanRoleID &&
                                                    re.ID == item.HumanRecruitmentProfileInterview.HumanRecruitmentRequirementID) != null)
                                                    && i.IsActive == true).Sum(i => i.MinPoint * i.Factor),
                            CriteriaMaxPoint = dbo.HumanRecruitmentCriterias
                                                                             .Where(i => (dbo.HumanRecruitmentRequirements.FirstOrDefault(re => re.HumanRoleID == i.HumanRoleID &&
                                                                                re.ID == item.HumanRecruitmentProfileInterview.HumanRecruitmentRequirementID) != null)
                                                                                && i.IsActive == true).Sum(i => i.MaxPoint * i.Factor),
                            IsEdit = item.HumanRecruitmentProfileInterview.IsEdit,
                            IsAccept = item.IsPass,
                            IsApproval = item.IsApproval,
                            Approval = new HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name,
                            },
                            ApprovalBy = item.ApprovalBy,
                            ApprovalAt = item.ApprovalAt,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.Name,
                            CreateUserID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.ID
                        })
                        .First();
            return resultProfile;
        }
        /// <summary>
        /// Lấy kết quả theo mã hồ sơ xét tuyển
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <returns></returns>
        public void GetResultByProfileInterViewID(int ProfileInterviewID, out HumanRecruitmentResultItem RecruitmentResult)
        {
            var dbo = RecruitmentResultDA.Repository;
            RecruitmentResult =
                dbo.HumanRecruitmentProfileInterviews.Where(i => i.ID == ProfileInterviewID)
                    .Select(item => new HumanRecruitmentResultItem()
                    {
                        ID = item.ID,
                        ProfileInterviewID = item.ID,
                        ProfileName = item.HumanRecruitmentProfile.Name,
                        PlanName = item.HumanRecruitmentPlan == null ? "" : item.HumanRecruitmentPlan.Name,
                        PlanID = item.HumanRecruitmentPlan == null ? 0 : item.HumanRecruitmentPlan.ID,
                        RequirementID = item.HumanRecruitmentRequirementID,
                        // Lấy tên chức danh ứng với yêu cầu mà hồ sơ chọn
                        RoleName = dbo.HumanRecruitmentRequirements.FirstOrDefault(re => re.ID == item.HumanRecruitmentRequirementID).HumanRole.Name,
                        TotalPoint = dbo.HumanRecruitmentReviews
                                    .Where(i => i.HumanRecruitmentProfileID == item.HumanRecruitmentProfileID)
                                     .Sum(i => i.Point),
                        // Điểm bé nhất của hệ thống tính theo tiêu chí
                        CriteriaMinPoint = dbo.HumanRecruitmentCriterias
                                             .Where(i => (dbo.HumanRecruitmentRequirements.FirstOrDefault(re => re.HumanRoleID == i.HumanRoleID &&
                                                re.ID == item.HumanRecruitmentRequirementID) != null)
                                                && i.IsActive == true).Sum(i => i.MinPoint * i.Factor),
                        // Điểm lớn nhất của hệ thống
                        CriteriaMaxPoint = dbo.HumanRecruitmentCriterias
                                             .Where(i => (dbo.HumanRecruitmentRequirements.FirstOrDefault(re => re.HumanRoleID == i.HumanRoleID &&
                                                re.ID == item.HumanRecruitmentRequirementID) != null)
                                                && i.IsActive == true).Sum(i => i.MaxPoint * i.Factor),
                        StartTime = dbo.HumanRecruitmentProfileResults.Where(i => i.HumanRecruitmentProfileInterviewID == item.ID).Select(i => i.StartTime).FirstOrDefault(),
                        Salary = dbo.HumanRecruitmentProfileResults.Where(i => i.HumanRecruitmentProfileInterviewID == item.ID).Select(i => i.Salary).FirstOrDefault(),
                        Approval = new HumanEmployeeViewItem()
                        {
                            ID = dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).ApprovalBy != null ? (int)dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).ApprovalBy : 0,
                            Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).ApprovalBy).Name,
                            Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).ApprovalBy).HumanRole.Name,
                            Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).ApprovalBy).HumanRole.HumanDepartment.Name,
                        },
                        ApprovalBy = dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).ApprovalBy,
                        ApprovalAt = dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).ApprovalAt,
                        IsAccept = dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID) == null ? false : dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).IsPass,
                        IsApproval = dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID) == null ? false : dbo.HumanRecruitmentProfileResults.FirstOrDefault(rpr => rpr.HumanRecruitmentProfileInterviewID == item.ID).IsApproval,
                        IsEdit = item.IsEdit,
                        CreateByName = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.Name,
                        CreateUserID = dbo.HumanUsers.FirstOrDefault(m => m.ID == item.CreateBy).HumanEmployee.ID
                    }).FirstOrDefault();
            //  }
        }
        /// <summary>
        /// Gửi đề xuất tuyển dụng cho bộ hồ sơ
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void SendProfile(HumanRecruitmentResultItem item, int userID)
        {
            var dbo = RecruitmentResultDA.Repository;
            // Cập nhật hồ sơ ứng tuyển ở trạng thái không cho phép sửa
            var profileInterview = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(i => i.ID == item.ProfileInterviewID);
            profileInterview.IsEdit = false;
            var resultUpdateItem = dbo.HumanRecruitmentProfileResults.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == item.ID);
            if (resultUpdateItem != null)
            {
                resultUpdateItem.Salary = item.Salary;
                // Thời gian bắt đầu làm việc
                if (item.StartTime != null)
                    resultUpdateItem.StartTime = (DateTime)item.StartTime;
                if (item.TotalPoint != null)
                    resultUpdateItem.TotalPoint = (int)item.TotalPoint;
                resultUpdateItem.UpdateAt = DateTime.Now;
                resultUpdateItem.UpdateBy = userID;
                resultUpdateItem.IsApproval = item.IsApproval;
                resultUpdateItem.ApprovalAt = item.ApprovalAt;
                resultUpdateItem.ApprovalBy = item.ApprovalBy;
                resultUpdateItem.Note = item.ApprovalNote;
            }
            else
            {
                var result = new HumanRecruitmentProfileResult()
                {
                    HumanRecruitmentProfileInterviewID = item.ProfileInterviewID,
                    Salary = item.Salary,
                    // Thời gian bắt đầu làm việc
                    StartTime = (DateTime)item.StartTime,
                    TotalPoint = (int)item.TotalPoint,
                    IsApproval = item.IsApproval,
                    IsPass = item.IsAccept,
                    ApprovalAt = item.ApprovalAt,
                    ApprovalBy = item.ApprovalBy,
                    Note = item.ApprovalNote,
                    CreateAt = DateTime.Now,
                    CreateBy = userID
                };
                dbo.HumanRecruitmentProfileResults.Add(result);
            }
            dbo.SaveChanges();
        }
        /// <summary>
        /// Cập nhật kết quả tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentResultItem item, int userID)
        {
            var dbo = RecruitmentResultDA.Repository;
            var rr = dbo.HumanRecruitmentProfileResults.FirstOrDefault(i => i.ID == item.ID);
            rr.HumanRecruitmentProfileInterviewID = item.ProfileInterviewID;
            rr.Salary = item.Salary;
            // Thời gian bắt đầu làm việc
            if (item.StartTime != null)
                rr.StartTime = (DateTime)item.StartTime;
            if (item.TotalPoint != null)
                rr.TotalPoint = (int)item.TotalPoint;
            rr.IsPass = item.IsAccept;
            rr.HumanRecruitmentProfileInterview.IsEdit = item.IsEdit;
            rr.IsApproval = item.IsApproval;
            rr.ApprovalAt = item.ApprovalAt;
            rr.ApprovalBy = item.ApprovalBy;
            rr.Note = item.ApprovalNote;
            rr.UpdateAt = DateTime.Now;
            rr.UpdateBy = userID;
            dbo.SaveChanges();
        }
        /// <summary>
        /// Thêm mới kết quả tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentResultItem item, int userID)
        {
            var result = new HumanRecruitmentProfileResult()
            {
                HumanRecruitmentProfileInterviewID = item.ProfileInterviewID,
                Note = item.ApprovalNote,
                Salary = item.Salary,
                // Thời gian bắt đầu làm việc
                StartTime = (DateTime)item.StartTime,
                TotalPoint = (int)item.TotalPoint,
                IsPass = item.IsAccept,
                IsApproval = item.IsApproval,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            RecruitmentResultDA.Insert(result);
            RecruitmentResultDA.Save();
        }
        /// <summary>
        /// Thêm mới nhân sự vào hệ thống
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="userID"></param>
        public int AddNewEmployee(int ProfileID, int userID,bool isTrial=false)
        {
            var dbo = RecruitmentResultDA.Repository;
            var item = dbo.HumanRecruitmentProfiles.FirstOrDefault(i => i.ID == ProfileID);
            item.IsEmployee = !isTrial;
            if (item != null)
            {
                #region Profile Employee Add

                // Thêm mới thông tin nhân viên
                var employee = new HumanEmployee()
                {
                    Name = item.Name,
                    Email = item.Email == null ? "Nhập vào email thiếu" : item.Email,
                    Avatar = item.Avatar,
                    //Code = item.Code,
                    Phone = item.Phone,
                    Address = item.Address,
                    Birthday = item.Birthday,
                    Gender = item.Gender,
                    CreateAt = DateTime.Now,
                    CreateBy = userID,
                    IsTrial  = isTrial
                };
                dbo.HumanEmployees.Add(employee);
                dbo.SaveChanges();
                item.EmployeeID = employee.ID;
                // thêm mới thông tin cá nhân
                var pr = new HumanProfileCuriculmViate()
                {
                    HumanEmployeeID = employee.ID,
                    //Aliases = item.Aliases,
                    //ArmyRank = item.ArmyRank,
                    //Bank = item.Bank,
                    //DateAtParty = item.DateAtParty,
                    NumberOfIdentityCard = item.NumberOfIdentityCard,
                    DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                    // DateJoinRevolution = item.DateJoinRevolution,
                    //DateOfIssuePassport = item.DateOfIssuePassport,
                    //DateOfJoinParty = item.DateOfJoinParty,
                    //DateOnArmy = item.DateOnArmy,
                    //DateOnGroup = item.DateOnGroup,
                    //Defect = item.Defect,
                    //Forte = item.Forte,
                    HomePhone = item.HomePhone,
                    //Likes = item.Likes,
                    Nationality = item.Nationality,
                    //NumberOfBankAccounts = item.NumberOfBankAccounts,
                    //NumberOfPartyCard = item.NumberOfPartyCard,
                    //NumberOfPassport = item.NumberOfPassport,
                    //OfficePhone = item.OfficePhone,
                    //PassportExpirationDate = item.PassportExpirationDate,
                    People = item.People,
                    PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                    PlaceOfBirth = item.PlaceOfBirth,
                    //PlaceOfLoadedGroup = item.PlaceOfLoadedGroup,
                    //PlaceOfLoadedParty = item.PlaceOfLoadedParty,
                    //PlaceOfPassport = item.PlaceOfPassport,
                    //PositionArmy = item.PositionArmy,
                    //PositionGroup = item.PositionGroup,
                    //PosititonParty = item.PosititonParty,
                    Religion = item.Religion,
                    //TaxCode = item.TaxCode,
                    //WeddingStatus = item.WeddingStatus,
                    CreateAt = DateTime.Now,
                    CreateBy = userID,
                };
                dbo.HumanProfileCuriculmViates.Add(pr);
                dbo.SaveChanges();
               
                #endregion

                
                return item.EmployeeID;
            }
            return 0;
        }
        /// <summary>
        /// Xóa kết quả tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentResultDA.Delete(id);
            RecruitmentResultDA.Save();
        }
    }
}
