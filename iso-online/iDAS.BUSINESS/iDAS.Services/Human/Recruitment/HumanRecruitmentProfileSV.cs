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
    public class HumanRecruitmentProfileSV
    {
        private HumanRecruitmentProfileDA RecruitmentProfileDA = new HumanRecruitmentProfileDA();
        /// <summary>
        /// Lấy tất cả các hồ sơ ứng viên
        ///|| Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentProfileItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = RecruitmentProfileDA.GetQuery(i=>i.IsDelete == false)
                        .Select(item => new HumanRecruitmentProfileItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Phone = item.Phone,
                            Email = item.Email,
                            Avatar = item.Avatar,
                            Address = item.Address,
                            Birthday = item.Birthday,
                            DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                            Experience = item.Experience,
                            ForeignLanguage = item.ForeignLanguage,
                            HomePhone = item.HomePhone,
                            Gender = item.Gender,
                            LevelOfComputerization = item.LevelOfComputerization,
                            ListOfCertificates = item.ListOfCertificates,
                            Literacy = item.Literacy,
                            Nationality = item.Nationality,
                            NumberOfIdentityCard = item.NumberOfIdentityCard,
                            People = item.People,
                            PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                            PlaceOfBirth = item.PlaceOfBirth,
                            PlaceOfTranning = item.PlaceOfTranning,
                            Qualifications = item.Qualifications,
                            Religion = item.Religion,
                            //Salary = item.Salary,
                            Specicalization = item.Specicalization,
                            //WeddingStatus = item.WeddingStatus,
                            YearsOfExperience = item.YearsOfExperience,
                            IsEmployee = dbo.HumanEmployees.Any(x=>x.ID == item.EmployeeID) ? !dbo.HumanEmployees.FirstOrDefault(z=>z.ID == item.EmployeeID).IsTrial:false,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return profiles;
        }
        /// <summary>
        ///|| Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentProfileItem GetById(int Id)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = RecruitmentProfileDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentProfileItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Phone = item.Phone,
                            Email = item.Email,
                            Avatar = item.Avatar,
                            Address = item.Address,
                            Birthday = item.Birthday,
                            DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                            Experience = item.Experience,
                            ForeignLanguage = item.ForeignLanguage,
                            HomePhone = item.HomePhone,
                            Gender = item.Gender,
                            LevelOfComputerization = item.LevelOfComputerization,
                            ListOfCertificates = item.ListOfCertificates,
                            Literacy = item.Literacy,
                            Nationality = item.Nationality,
                            NumberOfIdentityCard = item.NumberOfIdentityCard,
                            People = item.People,
                            PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                            PlaceOfBirth = item.PlaceOfBirth,
                            PlaceOfTranning = item.PlaceOfTranning,
                            Qualifications = item.Qualifications,
                            Religion = item.Religion,
                            Salary = item.Salary,
                            Specicalization = item.Specicalization,
                          //  WeddingStatus = item.WeddingStatus,
                            YearsOfExperience = item.YearsOfExperience,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .First();
            return profiles;
        }
        /// <summary>
        /// Lấy các thông tin nhân viên vào nhân sự
        /// </summary>
        /// <param name="RecruitmentProfileID"></param>
        /// <returns></returns>
        public HumanEmployeeItem GetEmployeeByRecruitmentProfileID(int RecruitmentProfileID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = RecruitmentProfileDA.GetQuery(i => i.ID == RecruitmentProfileID)
                        .Select(item => new HumanEmployeeItem()
                        {
                            Name = item.Name,
                            Phone = item.Phone,
                            Email = item.Email,
                           // Image = item.Image,
                            Address = item.Address,
                            Birthday = item.Birthday,
                            DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                            HomePhone = item.HomePhone,
                            Gender = item.Gender,
                            Nationality = item.Nationality,
                            NumberOfIdentityCard = item.NumberOfIdentityCard,
                            People = item.People,
                            PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                            PlaceOfBirth = item.PlaceOfBirth,
                            Religion = item.Religion

                        })
                        .First();
            return profiles;
        }
        /// <summary>
        /// Lấy danh sách hồ sơ trong đó có hồ sơ đã được lựa chọn bởi kế hoạch
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public List<RecruitmentProfileIneterviewSelectItem> GetAllByPlanID(int page, int pageSize, out int totalCount, int PlanID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = dbo.HumanRecruitmentProfiles
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID).ID,
                            Name = item.Name,
                            Avatar = item.Avatar,
                            Birthday = item.Birthday,
                            Gender = item.Gender,
                            ProfileID = item.ID,
                            RequirementName = dbo.HumanRecruitmentRequirements.FirstOrDefault(r => r.ID == dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID).HumanRecruitmentRequirementID).Name,
                            RequirementID = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID).HumanRecruitmentRequirementID,
                            IsSelect = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID) != null ? true : false,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            
            return profiles;
        }
        public List<RecruitmentProfileIneterviewSelectItem> GetAllNotIsEmpByPlanID(int page, int pageSize, out int totalCount, int PlanID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = dbo.HumanRecruitmentProfiles.Where
                        (x=>x.EmployeeID==0)
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID).ID,
                            Name = item.Name,
                            Avatar = item.Avatar,
                            Birthday = item.Birthday,
                            Gender = item.Gender,
                            ProfileID = item.ID,
                            RequirementName = dbo.HumanRecruitmentRequirements.FirstOrDefault(r => r.ID == dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID).HumanRecruitmentRequirementID).Name,
                            RequirementID = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID).HumanRecruitmentRequirementID,
                            IsSelect = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID) != null ? true : false,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();

            return profiles;
        }
        /// <summary>
        /// Lấy danh sách hồ sơ ứng với kế hoạch tuyển dụng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="RequirmentID"></param>
        /// <returns></returns>
        public List<RecruitmentProfileIneterviewSelectItem> GetByPlanID(ModelFilter filter, int focusId, int PlanID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            IQueryable<iDAS.Base.HumanRecruitmentProfileInterview> query = dbo.HumanRecruitmentProfileInterviews.Where(e => e.HumanRecruitmentPlanID == PlanID);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var profiles = query.Filter(filter)
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = item.ID,
                            Name = item.HumanRecruitmentProfile.Name,
                            Avatar = item.HumanRecruitmentProfile.Avatar,
                            Gender = item.HumanRecruitmentProfile.Gender,
                            Birthday = item.HumanRecruitmentProfile.Birthday,
                            ProfileID = item.HumanRecruitmentProfile.ID,
                            IsEdit = item.IsEdit,
                            IsAccept = dbo.HumanRecruitmentProfileResults.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == item.ID) == null?
                                        false : dbo.HumanRecruitmentProfileResults.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == item.ID).IsPass,
                            IsApproval = dbo.HumanRecruitmentProfileResults.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == item.ID) == null ?
                                        false : dbo.HumanRecruitmentProfileResults.FirstOrDefault(i => i.HumanRecruitmentProfileInterviewID == item.ID).IsApproval,
                            RequirementID = dbo.HumanRecruitmentRequirements.FirstOrDefault(r => r.ID == item.HumanRecruitmentRequirementID).ID,
                            RequirementName = dbo.HumanRecruitmentRequirements.FirstOrDefault(r => r.ID == item.HumanRecruitmentRequirementID).Name
                        })
                        .ToList();
            return profiles;
        }
        /// <summary>
        /// Lấy danh sách hồ sơ ứng với kế hoạch tuyển dụng và yêu cầu tuyển dụng của kế hoạch đó
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlanID"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns>
        public List<RecruitmentProfileIneterviewSelectItem> GetByPlanIDAndRequirementID(int page, int pageSize, out int totalCount, int PlanID, int RequirementID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = dbo.HumanRecruitmentProfiles.Where(item => dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentRequirementID == RequirementID && e.HumanRecruitmentProfileID == item.ID) != null)
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentRequirementID == RequirementID && e.HumanRecruitmentProfileID == item.ID).ID,
                            Name = item.Name,
                            Avatar = item.Avatar,
                            Birthday = item.Birthday,
                            ProfileID = item.ID,
                            RequirementName = dbo.HumanRecruitmentRequirements.FirstOrDefault(r => r.ID == dbo.HumanRecruitmentProfileInterviews.FirstOrDefault(e => e.HumanRecruitmentPlanID == PlanID && e.HumanRecruitmentProfileID == item.ID).HumanRecruitmentRequirementID).Name,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return profiles;
        }
        /// <summary>
        /// Cập nhật hồ sơ ứng viên đăng ký
        ///|| Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentProfileItem item, int userID)
        {
            var rp = RecruitmentProfileDA.GetById(item.ID);
            rp.Name = item.Name;
            rp.Phone = item.Phone;
            rp.Email = item.Email;
            if (!string.IsNullOrEmpty(item.Avatar))
                rp.Avatar = item.Avatar;
            rp.Address = item.Address;
            rp.Birthday = item.Birthday;
            rp.DateIssueOfIdentityCard = item.DateIssueOfIdentityCard;
            rp.Experience = item.Experience;
            rp.ForeignLanguage = item.ForeignLanguage;
            rp.HomePhone = item.HomePhone;
            rp.Gender = item.Gender;
            rp.LevelOfComputerization = item.LevelOfComputerization;
            rp.ListOfCertificates = item.ListOfCertificates;
            rp.Literacy = item.Literacy;
            rp.Nationality = item.Nationality;
            rp.NumberOfIdentityCard = item.NumberOfIdentityCard;
            rp.People = item.People;
            rp.PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard;
            rp.PlaceOfBirth = item.PlaceOfBirth;
            rp.PlaceOfTranning = item.PlaceOfTranning;
            rp.Qualifications = item.Qualifications;
            rp.Religion = item.Religion;
            rp.Salary = item.Salary;
            rp.Specicalization = item.Specicalization;
            //rp.WeddingStatus = item.WeddingStatus;
            rp.YearsOfExperience = item.YearsOfExperience;
            rp.UpdateAt = DateTime.Now;
            rp.UpdateBy = userID;
            RecruitmentProfileDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetAvatarById(int Id)
        {
            var profile = RecruitmentProfileDA.GetById(Id);
            string avatar = string.Empty;
            if (profile != null)
            {
                avatar = profile.Avatar;
            }
            return avatar;
        }
        /// <summary>
        /// Thêm mới hồ sơ ứng viên đăng ký
        ///|| Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentProfileItem item, int userID)
        {
            var profile = new HumanRecruitmentProfile()
            {
                Name = item.Name,
                Phone = item.Phone,
                Email = item.Email,
                Avatar = item.Avatar,
                Address = item.Address,
                Birthday = item.Birthday,
                DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                Experience = item.Experience,
                ForeignLanguage = item.ForeignLanguage,
                HomePhone = item.HomePhone,
                Gender = item.Gender,
                LevelOfComputerization = item.LevelOfComputerization,
                ListOfCertificates = item.ListOfCertificates,
                Literacy = item.Literacy,
                Nationality = item.Nationality,
                NumberOfIdentityCard = item.NumberOfIdentityCard,
                People = item.People,
                PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                PlaceOfBirth = item.PlaceOfBirth,
                PlaceOfTranning = item.PlaceOfTranning,
                Qualifications = item.Qualifications,
                Religion = item.Religion,
                Salary = item.Salary,
                Specicalization = item.Specicalization,
                //WeddingStatus = item.WeddingStatus,
                YearsOfExperience = item.YearsOfExperience,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            RecruitmentProfileDA.Insert(profile);
            RecruitmentProfileDA.Save();
        }
        /// <summary>
        /// xóa hồ sơ ứng viên đăng ký
        ///|| Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentProfileDA.Delete(id);
            RecruitmentProfileDA.Save();
        }
        /// <summary>
        /// Đổi trạng thái xóa hồ sơ ứng viên đăng ký nếu có ràng buộc dữ liệu
        ///|| Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="id"></param>
        public void IsDelete(int id)
        {
            var rp = RecruitmentProfileDA.GetById(id);
            rp.IsDelete = true;
            RecruitmentProfileDA.Save();
        }

        public List<RecruitmentProfileIneterviewSelectItem> GetByPlanID(ModelFilter filter, int planID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = dbo.HumanRecruitmentProfileInterviews.Where(e => e.HumanRecruitmentPlanID == planID)
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = item.ID,
                            Name = item.HumanRecruitmentProfile.Name,
                            Avatar = item.HumanRecruitmentProfile.Avatar,
                            Gender = item.HumanRecruitmentProfile.Gender,
                            Birthday = item.HumanRecruitmentProfile.Birthday,
                            ProfileID = item.HumanRecruitmentProfile.ID,
                            Email = item.HumanRecruitmentProfile.Email,
                            Phone = item.HumanRecruitmentProfile.Phone,
                            IsEmployee = item.HumanRecruitmentProfile.IsEmployee
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return profiles;
        }

        public List<RecruitmentProfileIneterviewSelectItem> GetByPlanIDHasResult(ModelFilter filter, int planID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = dbo.HumanRecruitmentProfileInterviews.Where(e => e.HumanRecruitmentPlanID == planID && e.HumanRecruitmentProfileResults.Count()>0)
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = item.ID,
                            Name = item.HumanRecruitmentProfile.Name,
                            Avatar = item.HumanRecruitmentProfile.Avatar,
                            Gender = item.HumanRecruitmentProfile.Gender,
                            Birthday = item.HumanRecruitmentProfile.Birthday,
                            ProfileID = item.HumanRecruitmentProfile.ID,
                            Email = item.HumanRecruitmentProfile.Email,
                            Phone = item.HumanRecruitmentProfile.Phone,
                            IsEmployee = item.HumanRecruitmentProfile.IsEmployee
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return profiles;
        }

        public List<RecruitmentProfileIneterviewSelectItem> GetByPlanIDPass(ModelFilter filter, int planID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var profiles = dbo.HumanRecruitmentProfileInterviews.Where(e => e.HumanRecruitmentPlanID == planID && e.HumanRecruitmentProfile.IsEmployee )
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = item.ID,
                            Name = item.HumanRecruitmentProfile.Name,
                            Avatar = item.HumanRecruitmentProfile.Avatar,
                            Gender = item.HumanRecruitmentProfile.Gender,
                            Birthday = item.HumanRecruitmentProfile.Birthday,
                            ProfileID = item.HumanRecruitmentProfile.ID,
                            Email = item.HumanRecruitmentProfile.Email,
                            Phone = item.HumanRecruitmentProfile.Phone,
                            IsEmployee = item.HumanRecruitmentProfile.IsEmployee
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return profiles;
        }

        public List<RecruitmentProfileIneterviewSelectItem> GetByPlanIDTrial(ModelFilter filter, int planID)
        {
            var dbo = RecruitmentProfileDA.Repository;
            var lstEmpID = dbo.HumanRecruitmentProfileInterviews.Where(x => x.HumanRecruitmentPlanID == planID).Select(x => x.HumanRecruitmentProfile.EmployeeID).ToList();
            var lstEmpIDTrial = dbo.HumanProfileWorkTrials.Where(x => lstEmpID.Contains(x.HumanEmployeeID)).Select(x => x.HumanEmployeeID);
            var profiles = dbo.HumanEmployees.Where(e => lstEmpIDTrial.Contains(e.ID))
                        .Select(item => new RecruitmentProfileIneterviewSelectItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Avatar = item.Avatar,
                            Gender = item.Gender,
                            Birthday = item.Birthday,
                            ProfileID = item.ID,
                            Email = item.Email,
                            Phone = item.Phone,
                            IsEmployee = !item.IsTrial
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return profiles;
        }

        public void UpdateEmpID(int empID)
        {
            var rps = RecruitmentProfileDA.GetQuery(x=>x.EmployeeID==empID);
            foreach (var rp in rps)
            {
                rp.EmployeeID = 0;
            }
            RecruitmentProfileDA.Save();
        }
    }
}
