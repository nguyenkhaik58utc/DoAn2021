using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class HumanProfileWorkTrialSV
    {
        private HumanProfileWorkTrialDA pwtDA = new HumanProfileWorkTrialDA();
        private HumanProfileWorkTrialResultDA rsDA = new HumanProfileWorkTrialResultDA();
        public void Insert(HumanProfileWorkTrialItem data, int empID)
        {
            var item = new HumanProfileWorkTrial()
            {
                HumanEmployeeID = data.Employee.ID,
                HumanRoleID = data.Role.ID,
                FromDate = data.FromDate,
                ToDate = data.ToDate,
                Note= data.Note,
                ManagerID = data.Manager.ID,
                CreateAt = DateTime.Now,
                CreateBy = empID,
                IsEdit = data.IsEdit,
                IsApproval = data.IsApproval,
                IsAccept = data.IsAccept,
                ApprovalAt = data.ApprovalAt,
                ApprovalBy = data.ApprovalBy
            };
            pwtDA.Insert(item);
            pwtDA.Save();
        }
        public void Update(HumanProfileWorkTrialItem updateData, int p)
        {
            var item = pwtDA.GetById(updateData.ID);
            item.HumanEmployeeID = updateData.Employee.ID;
            item.FromDate = updateData.FromDate;
            item.ToDate = updateData.ToDate;
            item.Note = updateData.Note;
            item.ManagerID = updateData.Manager.ID;
            item.HumanRoleID = updateData.Role.ID;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = p;
            pwtDA.Save();
        }
        public List<HumanProfileWorkTrialItem> GetAll(ModelFilter filter, DateTime searchFromDate, DateTime searchToDate, int focusId = 0)
        {
            var dbo = pwtDA.Repository;
            IQueryable<iDAS.Base.HumanProfileWorkTrial> query = dbo.HumanProfileWorkTrials;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var data = query.Where(s =>(s.ToDate <= searchToDate && s.FromDate >= searchFromDate) || (s.ToDate <= searchToDate && s.ToDate >= searchFromDate) || (s.FromDate <= searchToDate && s.FromDate >= searchFromDate)).Select(item => new HumanProfileWorkTrialItem()
            {
                ID= item.ID,
                HumanEmployeeID = item.HumanEmployeeID,
                HumanRoleID = item.HumanRoleID,
                HumanEmployee = new HumanEmployeeItem(){
                    Name = item.HumanEmployee.Name,
                    Birthday = item.HumanEmployee.Birthday,
                    Gender = item.HumanEmployee.Gender,
                    IsTrial = item.HumanEmployee.IsTrial
                },
                FromDate = item.FromDate,
                ToDate = item.ToDate,
                Note = item.Note,
                ManagerID = item.ManagerID,
                IsEdit = item.IsEdit,
                IsApproval = item.IsApproval,
                IsAccept = item.IsAccept,
                ApprovalAt = item.ApprovalAt,
                ApprovalBy = item.ApprovalBy,
                Status = false
            }).OrderByDescending(item => item.ID)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            foreach(var item in data)
            {
                if(item.HumanRoleID !=0)
                {
                    var _role = dbo.HumanRoles.FirstOrDefault(x => x.ID == item.HumanRoleID);
                    item.Role.Name = _role.Name;
                    item.Role.Department = _role.HumanDepartment.Name;
                }
                if (dbo.HumanProfileWorkTrialResults.Any(x => x.HumanProfileWorkTrialID == item.ID)) item.Status = true;
 
            }
            return data;
        }

        public List<HumanProfileWorkTrialItem> GetByEmployee(ModelFilter filter, int employeeId, int focusId)
        {
            var dbo = pwtDA.Repository;
            IQueryable<iDAS.Base.HumanProfileWorkTrial> query = dbo.HumanProfileWorkTrials;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var data = query
                 .Where(i => i.HumanEmployeeID == employeeId)
                 .Select(item => new HumanProfileWorkTrialItem()
                 {
                     ID = item.ID,
                     HumanEmployeeID = item.HumanEmployeeID,
                     HumanRoleID = item.HumanRoleID,
                     HumanEmployee = new HumanEmployeeItem()
                     {
                         Name = item.HumanEmployee.Name,
                         Birthday = item.HumanEmployee.Birthday,
                         Gender = item.HumanEmployee.Gender,
                         IsTrial = item.HumanEmployee.IsTrial
                     },
                     FromDate = item.FromDate,
                     ToDate = item.ToDate,
                     Note = item.Note,
                     ManagerID = item.ManagerID,
                     IsEdit = item.IsEdit,
                     IsApproval = item.IsApproval,
                     IsAccept = item.IsAccept,
                     ApprovalAt = item.ApprovalAt,
                     ApprovalBy = item.ApprovalBy,
                     Status = true
                 }).OrderByDescending(item => item.ID)
                            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                            .ToList();
            foreach (var item in data)
            {
                if (item.HumanRoleID != 0)
                {
                    var _role = dbo.HumanRoles.FirstOrDefault(x => x.ID == item.HumanRoleID);
                    item.Role.Name = _role.Name;
                    item.Role.Department = _role.HumanDepartment.Name;
                }
                if (dbo.HumanProfileWorkTrialResults.Any(x => x.HumanProfileWorkTrialID == item.ID && x.ManagerPoint.HasValue))
                {
                    item.Status = false;
                }
            }
            return data;
        }

        public HumanProfileWorkTrialItem getByID(int id)
        {
            var dbo = pwtDA.Repository;
            var item = pwtDA.GetById(id);
            
            var data =  new HumanProfileWorkTrialItem()
            {
                ID = item.ID,
                HumanEmployeeID = item.HumanEmployeeID,
                HumanRoleID = item.HumanRoleID,
                HumanEmployee = new HumanEmployeeItem()
                {
                    Name = item.HumanEmployee.Name,
                    Birthday = item.HumanEmployee.Birthday,
                    Gender = item.HumanEmployee.Gender,
                    IsTrial = item.HumanEmployee.IsTrial
                },
                Employee = new HumanEmployeeViewItem()
                {
                    Name = item.HumanEmployee.Name,
                    ID = item.HumanEmployeeID,
                },
                FromDate = item.FromDate,
                ToDate = item.ToDate,
                Note = item.Note,
                ManagerID = item.ManagerID,
                CreateAt = item.CreateAt,
                CreateBy = item.CreateBy,
                IsApproval = item.IsApproval,
                ApprovalAt = item.ApprovalAt,
                ApprovalBy = item.ApprovalBy,
                IsAccept = item.IsAccept,
                IsEdit = item.IsEdit,
                IsResult = item.IsApproval ? (bool?)(item.IsAccept ? true : false) : null,
                ApprovalNote = item.ApprovalNote,
                ContractStartTime= item.ContractStartTime,
                ContractType = item.ContractType,
                CreateByName = dbo.HumanEmployees.Any(i => i.ID == item.CreateBy)?dbo.HumanEmployees.FirstOrDefault(i => i.ID == item.CreateBy).Name : ""
            };
            if (item.HumanRoleID != 0)
            {
                var _role = dbo.HumanRoles.FirstOrDefault(x => x.ID == item.HumanRoleID);
                data.Role.ID = _role.ID;
                data.Role.Name = _role.Name;
                data.Role.Department = _role.HumanDepartment.Name;
            }
            if(item.ManagerID!=0)
            {
                data.Manager.ID = item.ManagerID.Value;
                data.Manager.Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ManagerID).Name;
                data.Manager.Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ManagerID).HumanRole.Name;
                data.Manager.Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ManagerID).HumanRole.HumanDepartment.Name;
            }
            if (item.ApprovalBy != null)
            {
                data.Approval.ID = item.ApprovalBy.Value;
                data.Approval.Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name;
                data.Approval.Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name;
                data.Approval.Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name;
            }
            return data;
        }

        public void InsertCriterialResult(HumanProfileWorkTrialResultItem data)
        {
            var item = new HumanProfileWorkTrialResult()
            {
                HumanProfileWorkTrialID = data.HumanProfileWorkTrialID,
                QualityCriteriaID = data.QualityCriteriaID,
                ManagerPoint = data.ManagerPoint,
                Note = data.Note
            };
            rsDA.Insert(item);
            rsDA.Save();
        }

        public List<HumanProfileWorkTrialResultItem> GetResultAudit(int humanProfileWorkTrialID)
        {
            var rs = rsDA.GetQuery(x => x.HumanProfileWorkTrialID == humanProfileWorkTrialID).Select(item => new HumanProfileWorkTrialResultItem()
            {
                ID= item.ID,
                Note = item.Note,
                ManagerPoint = item.ManagerPoint,
                EmployeePoint = item.EmployeePoint,
                QualityCriteriaName = item.QualityCriteria.Name,
                QualityCriteriaID = item.QualityCriteriaID,
                QualityCriteriaCateName = item.QualityCriteria.QualityCriteriaCategory.Name,
                HumanProfileWorkTrialID = item.HumanProfileWorkTrialID
            }).ToList();
            return rs;
        }

        public void UpdateCriterialResult(HumanProfileWorkTrialResultItem data)
        {
            var item = rsDA.GetById(data.ID);
            item.ManagerPoint = data.ManagerPoint;
            item.EmployeePoint = data.EmployeePoint;
            item.Note = data.Note;
            rsDA.Save();
        }



        public void SendApproval(HumanProfileWorkTrialItem updateData)
        {
            var pl = pwtDA.GetById(updateData.ID);
            pl.IsApproval = updateData.IsApproval;
            pl.IsEdit = updateData.IsEdit;
            pl.ApprovalBy = updateData.ApprovalBy;
            pl.ContractType = updateData.ContractType;
            pl.ContractStartTime = updateData.ContractStartTime;
            pl.ApprovalNote = updateData.ApprovalNote;
            pwtDA.Save();
        }

        public List<HumanProfileWorkTrialItem> GetAllByManagerID(ModelFilter filter, DateTime searchFromDate, DateTime searchToDate, int empID, int focusId=0)
        {
            var dbo = pwtDA.Repository;
            IQueryable<iDAS.Base.HumanProfileWorkTrial> query = dbo.HumanProfileWorkTrials;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var data = query.Where(s => s.ManagerID == empID && ((s.ToDate <= searchToDate && s.FromDate >= searchFromDate) || (s.ToDate <= searchToDate && s.ToDate >= searchFromDate) || (s.FromDate <= searchToDate && s.FromDate >= searchFromDate))).Select(item => new HumanProfileWorkTrialItem()
            {
                ID = item.ID,
                HumanEmployeeID = item.HumanEmployeeID,
                HumanRoleID = item.HumanRoleID,
                HumanEmployee = new HumanEmployeeItem()
                {
                    Name = item.HumanEmployee.Name,
                    Birthday = item.HumanEmployee.Birthday,
                    Gender = item.HumanEmployee.Gender,
                    IsTrial = item.HumanEmployee.IsTrial
                },
                FromDate = item.FromDate,
                ToDate = item.ToDate,
                Note = item.Note,
                ManagerID = item.ManagerID,
                IsEdit = item.IsEdit,
                IsApproval = item.IsApproval,
                IsAccept = item.IsAccept,
                ApprovalAt = item.ApprovalAt,
                ApprovalBy = item.ApprovalBy,
                Status = false
            }).OrderByDescending(item => item.ID)
                            .Page(filter.PageIndex,filter.PageSize, out filter.PageTotal)
                            .ToList();
            foreach (var item in data)
            {
                if (item.HumanRoleID != 0)
                {
                    var _role = dbo.HumanRoles.FirstOrDefault(x => x.ID == item.HumanRoleID);
                    item.Role.Name = _role.Name;
                    item.Role.Department = _role.HumanDepartment.Name;
                }
                if (dbo.HumanProfileWorkTrialResults.Any(x => x.HumanProfileWorkTrialID == item.ID && x.ManagerPoint.HasValue)) item.Status = true;

            }
            return data;
        }

        public void Approve(HumanProfileWorkTrialItem updateData)
        {
            var pl = pwtDA.GetById(updateData.ID);
            pl.IsApproval = true;
            pl.IsEdit = updateData.IsEdit;
            pl.ApprovalAt = updateData.ApprovalAt;
            pl.IsAccept = updateData.IsAccept;
            //pl.ApprovalNote = updateData.ApprovalNote;
            pwtDA.Save();
        }




        public void Delete(int id)
        {
            pwtDA.Delete(id);
            pwtDA.Save();
            //int empID = pwtDA.GetById(id).HumanEmployeeID;
            //var empDA = new EmployeeDA();
            //var emp = empDA.GetById(empID);
            ////Neu trang thai Dang thu viec
            //if(emp.IsTrial)
            //{
            //    empDA.Delete(emp);
            //    new HumanRecruitmentProfileSV().UpdateEmpID(empID);
            //}
        }

        public bool CheckEmpIDandRoleExits(int empID, int roleID)
        {
            var data = pwtDA.GetQuery(x => x.HumanEmployeeID == empID && x.HumanRoleID == roleID);
            if (data != null && data.Count() > 0)
                return true;
            else
                return false;
        }
    }
}
