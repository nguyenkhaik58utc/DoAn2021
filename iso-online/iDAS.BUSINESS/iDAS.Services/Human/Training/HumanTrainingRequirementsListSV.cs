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
    public class HumanTrainingRequirementsListSV
    {
        private HumanTrainingRequirementEmployeeDA TrainingRequirementListDA = new HumanTrainingRequirementEmployeeDA();
        public List<HumanTrainingRequirementListItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = TrainingRequirementListDA.Repository;
            var result = dbo.HumanTrainingRequirementEmployees
                        .Select(item => new HumanTrainingRequirementListItem()
                        {
                            ID = item.ID,
                            Employee = new HumanEmployeeViewItem()
                            {
                                ID = item.EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(i => i.ID == item.EmployeeID).Name,
                                Role = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                Department = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                            },
                            Content = item.Content,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Note = item.Note,
                            RequirementID = item.EmployeeID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public List<HumanTrainingRequirementListItem> GetByRequirement(int page, int pageSize, out int totalCount, int RequirementID)
        {
            var dbo = TrainingRequirementListDA.Repository;
            var result = dbo.HumanTrainingRequirementEmployees
                        .Where(i => i.HumanTrainingRequirementID == RequirementID)
                        .Select(item => new HumanTrainingRequirementListItem()
                        {
                            ID = item.ID,
                            Employee = new HumanEmployeeViewItem()
                            {
                                ID = item.EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(i => i.ID == item.EmployeeID).Name,
                                Role = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                Department = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                            },
                            Content = item.Content,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Note = item.Note,
                            RequirementID = item.HumanTrainingRequirementID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public HumanTrainingRequirementListItem GetById(int Id)
        {
            var dbo = TrainingRequirementListDA.Repository;
            var re = dbo.HumanTrainingRequirementEmployees.Where(i => i.ID == Id)
                        .Select(item => new HumanTrainingRequirementListItem()
                        {
                            ID = item.ID,
                            Employee = new HumanEmployeeViewItem()
                            {
                                ID = item.EmployeeID == null ? 0 : (int)item.EmployeeID,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).Name,
                                Role = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                Department = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                            },
                            Content = item.Content,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Note = item.Note,
                            RequirementID = item.HumanTrainingRequirementID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return re;
        }
        public void Update(HumanTrainingRequirementListItem item, int userID)
        {
            var r = TrainingRequirementListDA.GetById(item.ID);
            r.EmployeeID = item.Employee.ID;
            r.Content = item.Content;
            r.StartTime = item.StartTime;
            r.EndTime = item.EndTime;
            r.Note = item.Note;
            r.HumanTrainingRequirementID = item.RequirementID;
            r.UpdateAt = DateTime.Now;
            r.UpdateBy = userID;
            TrainingRequirementListDA.Save();
        }
        public void Insert(HumanTrainingRequirementListItem item, int userID)
        {
            var result = new HumanTrainingRequirementEmployee()
            {
                EmployeeID = item.Employee.ID,
                Content = item.Content,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Note = item.Note,
                HumanTrainingRequirementID = item.RequirementID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            TrainingRequirementListDA.Insert(result);
            TrainingRequirementListDA.Save();
        }
        public void InsertMulti(string ids, int requirementId, int userId)
        {
            var employeeIds = ids.Split(',').Select(i => Convert.ToInt32(i)).ToList();
            if (employeeIds != null && employeeIds.Count > 0)
            {
                var trainninggRequired = TrainingRequirementListDA.Repository.HumanTrainingRequirements
                                    .Where(i => i.ID == requirementId).FirstOrDefault();
                var requirementEmployeeInserts = new List<HumanTrainingRequirementEmployee>();
                foreach (var employeeId in employeeIds)
                {
                    if (TrainingRequirementListDA.GetQuery(i => i.HumanTrainingRequirementID == requirementId && i.EmployeeID == employeeId).FirstOrDefault() == null)
                    {
                        requirementEmployeeInserts.Add(new HumanTrainingRequirementEmployee()
                                          {
                                              EmployeeID = employeeId,
                                              HumanTrainingRequirementID = requirementId,
                                              Content = trainninggRequired.Content,
                                              StartTime = trainninggRequired.StartTime,
                                              EndTime = trainninggRequired.EndTime,
                                              CreateAt = DateTime.Now,
                                              CreateBy = userId
                                          });
                    }

                }
                TrainingRequirementListDA.InsertRange(requirementEmployeeInserts);
                TrainingRequirementListDA.Save();
            }
        }
        public void Delete(int id)
        {
            TrainingRequirementListDA.Delete(id);
            TrainingRequirementListDA.Save();
        }

    }
}
