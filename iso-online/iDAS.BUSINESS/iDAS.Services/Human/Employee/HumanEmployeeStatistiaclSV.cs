using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    
    public class HumanEmployeeStatistiaclSV
    {
        HumanDepartmentDA depDA = new HumanDepartmentDA();
        public List<EquipmentStatisticalModel> GenerateDataStatistical(int id,int age = 30)
        {
            var dbo = depDA.Repository;            
            var lst = dbo.HumanDepartments.Where(i => i.ParentID == id && i.IsActive && !i.IsDelete && !i.IsCancel && !i.IsMerge && !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(item => new EquipmentStatisticalModel()
                             {
                                 CateID = item.ID,
                                 ParentID = item.ParentID,
                                 Name = item.Name,
                                 Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID && i.IsActive && !i.IsDelete && !i.IsCancel && !i.IsMerge && !i.IsDestroy)                                 
                             }).ToList();
            int year = DateTime.Now.Year - age;
            foreach(var item in lst)
            {
                
                var lstEmp = dbo.HumanRoles
                            .Where(i => i.HumanDepartmentID == item.CateID)
                            .SelectMany(i => i.HumanOrganizations)
                            .Select(i => i.HumanEmployee).Distinct();
                if (id == 0)
                {
                    lstEmp = dbo.HumanEmployees;
                }
                item.Data1 = lstEmp.Count();
                item.Data2 = lstEmp.Where(x => x.Birthday.HasValue && x.Birthday.Value.Year <= year).Count();
                item.Data9 = item.Data1 - item.Data2;
                item.Data7 = lstEmp.Where(x => x.Gender == true).Count();
                item.Data8 = item.Data1 - item.Data7;
                List<int> lstID = lstEmp.Select(x=>x.ID).ToList();
                item.Data3 = dbo.HumanProfileContracts.Where(x => lstID.Contains(x.HumanEmployeeID) && x.EndDate.Value.Year == DateTime.Now.Year && x.EndDate.Value.Month == DateTime.Now.Month).Count();
            }
            return lst;
        }

        public List<Items.HumanEmployeeItem> GetByDepID(ModelFilter filter, int depId)
        {
            var dbo = depDA.Repository;
            var rs =        dbo.HumanRoles
                            .Where(i => i.HumanDepartmentID == depId)
                            .SelectMany(i => i.HumanOrganizations).Select(i => i.HumanEmployee).Distinct()
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                {
                                    ID = i.ID,
                                    Name = i.HumanRole.Name,
                                    GroupName = i.HumanRole.HumanDepartment.Name
                                }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            if(depId==1)
            {
                rs = dbo.HumanEmployees
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                    {
                                        ID = i.ID,
                                        Name = i.HumanRole.Name,
                                        GroupName = i.HumanRole.HumanDepartment.Name
                                    }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            }
            return rs;
        }

        public List<HumanEmployeeItem> GetByDepIDO30(ModelFilter filter, int depId,int age=30)
        {
            var dbo = depDA.Repository;
            int year = DateTime.Now.Year - age;
            var rs = dbo.HumanRoles
                            .Where(i => i.HumanDepartmentID == depId)
                            .SelectMany(i => i.HumanOrganizations).Select(i => i.HumanEmployee).Distinct()
                            .Where(x => x.Birthday.HasValue && x.Birthday.Value.Year <= year)
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                    {
                                        ID = i.ID,
                                        Name = i.HumanRole.Name,
                                        GroupName = i.HumanRole.HumanDepartment.Name
                                    }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            if (depId == 1)
            {
                rs = dbo.HumanEmployees.Where(x => x.Birthday.HasValue && x.Birthday.Value.Year <= year)
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                    {
                                        ID = i.ID,
                                        Name = i.HumanRole.Name,
                                        GroupName = i.HumanRole.HumanDepartment.Name
                                    }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            }
            return rs;
        }

        public List<HumanEmployeeItem> GetByDepIDU30(ModelFilter filter, int depId,int age=30)
        {
            var dbo = depDA.Repository;
            int year = DateTime.Now.Year - age;
            var rs = dbo.HumanRoles
                            .Where(i => i.HumanDepartmentID == depId)
                            .SelectMany(i => i.HumanOrganizations).Select(i => i.HumanEmployee).Distinct()
                            .Where(x => !x.Birthday.HasValue || x.Birthday.Value.Year > year)
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                    {
                                        ID = i.ID,
                                        Name = i.HumanRole.Name,
                                        GroupName = i.HumanRole.HumanDepartment.Name
                                    }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            if (depId == 1)
            {
                rs = dbo.HumanEmployees.Where(x => !x.Birthday.HasValue || x.Birthday.Value.Year > year)
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                    {
                                        ID = i.ID,
                                        Name = i.HumanRole.Name,
                                        GroupName = i.HumanRole.HumanDepartment.Name
                                    }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            }
            return rs;
        }

        public List<HumanEmployeeItem> GetByDepIDGender(ModelFilter filter, int depId, bool p)
        {
            var dbo = depDA.Repository;
            var rs = dbo.HumanRoles
                            .Where(i => i.HumanDepartmentID == depId)
                            .SelectMany(i => i.HumanOrganizations).Select(i => i.HumanEmployee).Distinct()
                            .Where(x => x.Gender==p)
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                    {
                                        ID = i.ID,
                                        Name = i.HumanRole.Name,
                                        GroupName = i.HumanRole.HumanDepartment.Name
                                    }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            if (depId == 1)
            {
                rs = dbo.HumanEmployees.Where(x => x.Gender == p)
                            .Select(item => new HumanEmployeeItem()
                            {
                                ID = item.ID,
                                FileID = item.AttachmentFileID,
                                FileName = item.AttachmentFile.Name,
                                Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                                DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                                Code = item.Code,
                                Name = item.Name,
                                Email = item.Email,
                                Phone = item.Phone,
                                Address = item.Address,
                                Birthday = item.Birthday,
                                Gender = item.Gender,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                                lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                    && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                    ).Select(i => new HumanGroupPositionItem()
                                    {
                                        ID = i.ID,
                                        Name = i.HumanRole.Name,
                                        GroupName = i.HumanRole.HumanDepartment.Name
                                    }).ToList()
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            }
            return rs;
        }

        public List<HumanProfileContractItem> GetContractByDepID(ModelFilter filter, int depId)
        {
            var dbo = depDA.Repository;
            var lst = dbo.HumanRoles
                            .Where(i => i.HumanDepartmentID == depId)
                            .SelectMany(i => i.HumanOrganizations).Select(i => i.HumanEmployee).Distinct().Select(i=>i.ID).ToList();
            if (depId == 1)
                lst = dbo.HumanEmployees.Select(i => i.ID).ToList();
            var users = dbo.HumanProfileContracts.Where(i =>lst.Contains(i.HumanEmployeeID) && i.EndDate.Value.Month == DateTime.Now.Month)
                        .Select(item => new HumanProfileContractItem()
                        {
                            ID = item.ID,
                            Condition = item.Condition,
                            NumberOfContracts = item.NumberOfContracts,
                            Type = item.Type,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            EmployeeName = item.HumanEmployee.Name
                        })
                        .OrderByDescending(item => item.CreateAt)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            return users;
        
        }
    }

    public class HumanRecruitmentStatisticalSV
    {
        HumanRecruitmentPlanDA RecruitmentPlanDA = new HumanRecruitmentPlanDA();


        public List<HumanRecruitmentPlanItem> GetAllApprove(int page, int pageSize, out int total, DateTime fromDate, DateTime toDate)
        {
            var dbo= RecruitmentPlanDA.Repository;
            var plans = RecruitmentPlanDA.GetQuery(i => i.IsDelete == false && i.IsApproval && i.IsAccept)
                        .Where(i => (i.StartDate >= fromDate && i.StartDate<= toDate) || (i.EndDate >= fromDate && i.EndDate <= toDate) || (i.StartDate <= fromDate && i.EndDate >= toDate))
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
                        .Page(page, pageSize, out total)
                        .ToList();
            foreach(var item in plans)
            {
                var lstProfile = dbo.HumanRecruitmentProfileInterviews.Where(x => x.HumanRecruitmentPlanID == item.ID).Select(x=>x.ID).ToList();
                var lstEmpID = dbo.HumanRecruitmentProfileInterviews.Where(x => x.HumanRecruitmentPlanID == item.ID).Select(x=>x.HumanRecruitmentProfile.EmployeeID).ToList();
                item.TotalProfile = lstProfile.Count();
                item.TotalInterview = dbo.HumanRecruitmentProfileResults.Where(x => lstProfile.Contains(x.HumanRecruitmentProfileInterviewID)).Count();
                item.TotalTrial = dbo.HumanProfileWorkTrials.Count(x => lstEmpID.Contains(x.HumanEmployeeID));
                item.TotalPass = dbo.HumanRecruitmentProfileResults.Where(x => lstProfile.Contains(x.HumanRecruitmentProfileInterviewID) && x.HumanRecruitmentProfileInterview.HumanRecruitmentProfile.IsEmployee).Count();
            }
            return plans;
        }
    }
}
