using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class QualityNCSV
    {
        private QualityNCDA NCDA = new QualityNCDA();
        public List<QualityNCItem> GetByDepartment(int page, int pageSize, out int totalCount, int departmentId)
        {
            var dbo = NCDA.Repository;
            var source = dbo.QualityNCs.Where(i => i.DepartmentID == departmentId || departmentId == 1)
                        .Select(item => new QualityNCItem()
                        {
                            ID = item.ID,
                            Department = new HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            Time = item.Time,
                            Content = item.Content,
                            //Criteria = new AuditCriteriaViewItem()
                            //{
                            //    ID = item. == null ? 0 : item.MnCriteria.ID,
                            //    Name = item.MnCriteria == null ? "" : item.MnCriteria.Name,
                            //    Category = item.MnCriteria == null ? "-Chưa có tiêu chí" : item.MnCriteria.MnCriteriaCategory.Name
                            //},
                            Source = item.Source,
                            IsMaximum = item.IsMaximum,
                            IsMedium = item.IsMedium,
                            IsObs = item.IsObs,
                            IsVerify = item.IsVerify,
                            IsTrue = item.IsTrue,
                            IsCompleteAuto = item.IsComplete,
                            IsComplete = item.IsComplete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();

            return source;
        }
        public QualityNCItem GetByID(int ID)
        {
            var dbo = NCDA.Repository;
            var result = dbo.QualityNCs.Where(i => i.ID == ID)
                .Select(item => new QualityNCItem()
                {
                    ID = item.ID,
                    Department = new HumanDepartmentViewItem()
                    {
                        ID = item.DepartmentID,
                        Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                    },
                    Time = item.Time,
                    Content = item.Content,
                    //Criteria = new AuditCriteriaViewItem()
                    //{
                    //    ID = item.,
                    //    Name = item.MnCriteria.Name,
                    //    Category = item.MnCriteria.MnCriteriaCategory.Name
                    //},
                    Source = item.Source,
                    IsMaximum = item.IsMaximum,
                    IsMedium = item.IsMedium,
                    IsObs = item.IsObs,
                    IsVerify = item.IsVerify,
                    IsTrue = item.IsTrue,
                    IsCompleteAuto = item.IsComplete,
                    IsComplete = item.IsComplete,
                    CheckAt = item.CheckAt,
                    CheckNote = item.CheckNote,
                    CreateBy = item.CreateBy,
                }).First();
            result.LisEmployeeID = string.Join(",", dbo.QualityNCEmployees.Where(i => i.QualityNCID == ID).Select(i => i.EmployeeID).ToArray());
            result.LisRoleID = string.Join(",", dbo.QualityNCRoles.Where(i => i.QualityNCID == ID).Select(i => i.HumanRoleID).ToArray());
            return result;
        }
        public QualityNCItem GetByAudit(int auditId, int criteriaId, int departmentId)
        {
            var dbo = NCDA.Repository;
            var result = dbo.AuditResults.Where(i => i.AuditID == auditId && i.QualityCriteriaID == criteriaId)
                            .DefaultIfEmpty()
                            .Select(item => item == null ? null : item.QualityNC)
                            .Select(item =>
                                new QualityNCItem()
                                {
                                    ID = item == null ? 0 : item.ID,
                                    Department = new HumanDepartmentViewItem()
                                    {
                                        ID = departmentId,
                                        Name = dbo.HumanDepartments
                                                .FirstOrDefault(d => d.ID == departmentId).Name
                                    },
                                    Time = item == null ? DateTime.Now : item.Time,
                                    Content = item == null ? null : item.Content,
                                    Criteria = new AuditCriteriaViewItem
                                    {
                                        ID = criteriaId,
                                        Name = dbo.QualityCriterias.FirstOrDefault(i => i.ID == criteriaId).Name,
                                        Category = dbo.QualityCriterias.FirstOrDefault(i => i.ID == criteriaId).QualityCriteriaCategory.Name
                                    },
                                    Source = item == null ? null : item.Source,
                                    IsMaximum = item == null ? false : item.IsMaximum,
                                    IsMedium = item == null ? false : item.IsMedium,
                                    IsObs = item == null ? false : item.IsObs,
                                    IsVerify = item == null ? false : item.IsVerify,
                                    IsTrue = item == null ? false : item.IsTrue,
                                    IsCompleteAuto = item == null ? false : item.IsComplete,
                                    IsComplete = item == null ? false : item.IsComplete,
                                    CheckAt = item == null ? null : item.CheckAt,
                                    CheckNote = item == null ? null : item.CheckNote,
                                }
                            ).First();
            if (result.ID != 0)
            {
                result.LisEmployeeID = string.Join(",", dbo.QualityNCEmployees.Where(i => i.QualityNCID == result.ID).Select(i => i.EmployeeID).ToArray());
                result.LisRoleID = string.Join(",", dbo.QualityNCRoles.Where(i => i.QualityNCID == result.ID).Select(i => i.HumanRoleID).ToArray());
            }
            return result;
        }
        public QualityNCItem GetByAdjustment(int adjustmentId)
        {
            var dbo = NCDA.Repository;
            var result = dbo.QualityNCStockAdjustments.Where(i => i.StockAdjustmentID == adjustmentId)
                            .DefaultIfEmpty()
                            .Select(item => item == null ? null : item.QualityNC)
                            .Select(item =>
                                new QualityNCItem()
                                {
                                    ID = item == null ? 0 : item.ID,
                                    Department = new HumanDepartmentViewItem()
                                    {
                                        ID = item == null ? 0 : item.DepartmentID,
                                        Name = item == null ? string.Empty : dbo.HumanDepartments
                                                .FirstOrDefault(d => d.ID == item.DepartmentID).Name
                                    },
                                    Time = item == null ? DateTime.Now : item.Time,
                                    Content = item == null ? null : item.Content,
                                    Source = item == null ? null : item.Source,
                                    IsMaximum = item == null ? false : item.IsMaximum,
                                    IsMedium = item == null ? false : item.IsMedium,
                                    IsObs = item == null ? false : item.IsObs,
                                    IsVerify = item == null ? false : item.IsVerify,
                                    IsTrue = item == null ? false : item.IsTrue,
                                    IsCompleteAuto = item == null ? false : item.IsComplete,
                                    IsComplete = item == null ? false : item.IsComplete,
                                    CheckAt = item == null ? null : item.CheckAt,
                                    CheckNote = item == null ? null : item.CheckNote,
                                }
                            ).First();
            if (result.ID != 0)
            {
                result.LisEmployeeID = string.Join(",", dbo.QualityNCEmployees.Where(i => i.QualityNCID == result.ID).Select(i => i.EmployeeID).ToArray());
                result.LisRoleID = string.Join(",", dbo.QualityNCRoles.Where(i => i.QualityNCID == result.ID).Select(i => i.HumanRoleID).ToArray());
            }
            return result;
        }
        public QualityNCItem GetByAuditRiskControl(int auditId, int RiskControlID, int departmentId)
        {
            var dbo = NCDA.Repository;
            var result = dbo.RiskAudits.Where(i => i.RiskControlID == RiskControlID)
                            .DefaultIfEmpty()
                            .Select(item => item == null ? null : item.QualityNC)
                            .Select(item =>
                                new QualityNCItem()
                                {
                                    ID = item == null ? 0 : item.ID,
                                    Department = new HumanDepartmentViewItem()
                                    {
                                        ID = departmentId,
                                        Name = dbo.HumanDepartments
                                                .FirstOrDefault(d => d.ID == departmentId).Name
                                    },
                                    Time = item == null ? DateTime.Now : item.Time,
                                    Content = item == null ? null : item.Content,
                                    Source = item == null ? null : item.Source,
                                    IsMaximum = item == null ? false : item.IsMaximum,
                                    IsMedium = item == null ? false : item.IsMedium,
                                    IsObs = item == null ? false : item.IsObs,
                                    IsVerify = item == null ? false : item.IsVerify,
                                    IsTrue = item == null ? false : item.IsTrue,
                                    IsCompleteAuto = item == null ? false : item.IsComplete,
                                    IsComplete = item == null ? false : item.IsComplete,
                                    CheckAt = item == null ? null : item.CheckAt,
                                    CheckNote = item == null ? null : item.CheckNote,
                                }
                            ).First();
            if (result.ID != 0)
            {
                result.LisEmployeeID = string.Join(",", dbo.QualityNCEmployees.Where(i => i.QualityNCID == result.ID).Select(i => i.EmployeeID).ToArray());
                result.LisRoleID = string.Join(",", dbo.QualityNCRoles.Where(i => i.QualityNCID == result.ID).Select(i => i.HumanRoleID).ToArray());
            }
            return result;
        }

        public QualityNCItem GetByAuditQuality(int auditId, int criteriaId, int departmentId)
        {
            var dbo = NCDA.Repository;
            var result = dbo.QualityAuditResults.Where(i => i.QualityAuditProgramISOID == auditId && i.ISOIndexCriteriaID == criteriaId && i.HumanDepartmentID==departmentId)
                            .DefaultIfEmpty()
                            .Select(item => item == null ? null : item.QualityNC)
                            .Select(item =>
                                new QualityNCItem()
                                {
                                    ID = item == null ? 0 : item.ID,
                                    Department = new HumanDepartmentViewItem()
                                    {
                                        ID = departmentId,
                                        Name = dbo.HumanDepartments
                                                .FirstOrDefault(d => d.ID == departmentId).Name
                                    },
                                    Time = item == null ? DateTime.Now : item.Time,
                                    Content = item == null ? null : item.Content,
                                    Criteria = new AuditCriteriaViewItem
                                    {
                                        ID = criteriaId,
                                        Name = dbo.QualityCriterias.FirstOrDefault(i => i.ID == criteriaId).Name,
                                        Category = dbo.QualityCriterias.FirstOrDefault(i => i.ID == criteriaId).QualityCriteriaCategory.Name
                                    },
                                    Source = item == null ? null : item.Source,
                                    IsMaximum = item == null ? false : item.IsMaximum,
                                    IsMedium = item == null ? false : item.IsMedium,
                                    IsObs = item == null ? false : item.IsObs,
                                    IsVerify = item == null ? false : item.IsVerify,
                                    IsTrue = item == null ? false : item.IsTrue,
                                    IsCompleteAuto = item == null ? false : item.IsComplete,
                                    IsComplete = item == null ? false : item.IsComplete,
                                    CheckAt = item == null ? null : item.CheckAt,
                                    CheckNote = item == null ? null : item.CheckNote,
                                }
                            ).First();
            if (result.ID != 0)
            {
                result.LisEmployeeID = string.Join(",", dbo.QualityNCEmployees.Where(i => i.QualityNCID == result.ID).Select(i => i.EmployeeID).ToArray());
                result.LisRoleID = string.Join(",", dbo.QualityNCRoles.Where(i => i.QualityNCID == result.ID).Select(i => i.HumanRoleID).ToArray());
            }
            return result;
        }
        public int Insert(QualityNCItem item)
        {
            var dbo = NCDA.Repository;
            var NC = new QualityNC()
            {
                DepartmentID = item.Department.ID,
                Time = item.Time,
                Content = item.Content,
                Source = item.Source,
                IsObs = item.IsObs,
                IsMaximum = item.IsMaximum,
                IsMedium = item.IsMedium,
                IsVerify = item.IsVerify,
                IsTrue = item.IsTrue,
                IsCompleteAuto = item.IsCompleteAuto,
                IsComplete = item.IsComplete,
                CheckAt = item.CheckAt,
                CheckNote = item.CheckNote,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now,
            };
            dbo.QualityNCs.Add(NC);
            if (!string.IsNullOrEmpty(item.LisEmployeeID))
            {
                var arrId = item.LisEmployeeID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCEmployees.Add(
                                                new QualityNCEmployee()
                                                {
                                                    EmployeeID = id,
                                                    QualityNCID = NC.ID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            if (!string.IsNullOrEmpty(item.LisRoleID))
            {
                var arrId = item.LisRoleID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCRoles.Add(
                                                new QualityNCRole()
                                                {
                                                    HumanRoleID = id,
                                                    QualityNCID = NC.ID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            dbo.SaveChanges();
            return NC.ID;
        }
        /// <summary>
        /// CuongPC
        /// </summary>
        /// <param name="item"></param>
        public void InsertFromAuditTask(QualityNCItem item, int qualityCriteriaID = 0, int auditId = 0)
        {
            var dbo = NCDA.Repository;
            var NC = new QualityNC()
            {
                DepartmentID = item.Department.ID,
                Time = item.Time,
                Content = item.Content,
                Source = item.Source,
                IsObs = item.IsObs,
                IsMaximum = item.IsMaximum,
                IsMedium = item.IsMedium,
                IsVerify = item.IsVerify,
                IsTrue = item.IsTrue,
                IsCompleteAuto = item.IsCompleteAuto,
                IsComplete = item.IsComplete,
                CheckAt = item.CheckAt,
                CheckNote = item.CheckNote,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now,
            };
            var ncID = dbo.QualityNCs.Add(NC).ID;
            if (!string.IsNullOrEmpty(item.LisEmployeeID))
            {
                var arrId = item.LisEmployeeID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCEmployees.Add(
                                                new QualityNCEmployee()
                                                {
                                                    EmployeeID = id,
                                                    QualityNCID = ncID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            if (!string.IsNullOrEmpty(item.LisRoleID))
            {
                var arrId = item.LisRoleID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCRoles.Add(
                                                new QualityNCRole()
                                                {
                                                    HumanRoleID = id,
                                                    QualityNCID = ncID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            var auditResult = dbo.AuditResults.FirstOrDefault(t => t.QualityCriteriaID == qualityCriteriaID && t.AuditID == auditId);
            auditResult.QualityNCID = ncID;
            dbo.SaveChanges();
        }
        /// <summary>
        /// CuongPC
        /// </summary>
        /// <param name="item"></param>
        public void InsertFromAdjustment(QualityNCItem item, int adjustmentId = 0)
        {
            var dbo = NCDA.Repository;
            var NC = new QualityNC()
            {
                DepartmentID = item.Department.ID,
                Time = item.Time,
                Content = item.Content,
                Source = item.Source,
                IsObs = item.IsObs,
                IsMaximum = item.IsMaximum,
                IsMedium = item.IsMedium,
                IsVerify = item.IsVerify,
                IsTrue = item.IsTrue,
                IsCompleteAuto = item.IsCompleteAuto,
                IsComplete = item.IsComplete,
                CheckAt = item.CheckAt,
                CheckNote = item.CheckNote,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now,
            };
            var ncID = dbo.QualityNCs.Add(NC).ID;
            if (!string.IsNullOrEmpty(item.LisEmployeeID))
            {
                var arrId = item.LisEmployeeID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCEmployees.Add(
                                                new QualityNCEmployee()
                                                {
                                                    EmployeeID = id,
                                                    QualityNCID = ncID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            if (!string.IsNullOrEmpty(item.LisRoleID))
            {
                var arrId = item.LisRoleID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCRoles.Add(
                                                new QualityNCRole()
                                                {
                                                    HumanRoleID = id,
                                                    QualityNCID = ncID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            QualityNCStockAdjustment objQualityNCStockAdjustment = new QualityNCStockAdjustment();
            objQualityNCStockAdjustment.QualityNCID = ncID;
            objQualityNCStockAdjustment.StockAdjustmentID = adjustmentId;
            objQualityNCStockAdjustment.CreateAt = DateTime.Now;
            objQualityNCStockAdjustment.CreateBy = item.CreateBy;
            dbo.QualityNCStockAdjustments.Add(objQualityNCStockAdjustment);
            dbo.SaveChanges();
        }
        public void InsertFromAuditQuality(QualityNCItem item, int qualityCriteriaID = 0, int auditId = 0)
        {
            var dbo = NCDA.Repository;
            var NC = new QualityNC()
            {
                DepartmentID = item.Department.ID,
                Time = item.Time,
                Content = item.Content,
                Source = item.Source,
                IsObs = item.IsObs,
                IsMaximum = item.IsMaximum,
                IsMedium = item.IsMedium,
                IsVerify = item.IsVerify,
                IsTrue = item.IsTrue,
                IsCompleteAuto = item.IsCompleteAuto,
                IsComplete = item.IsComplete,
                CheckAt = item.CheckAt,
                CheckNote = item.CheckNote,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now,
            };
            var ncID = dbo.QualityNCs.Add(NC).ID;
            if (!string.IsNullOrEmpty(item.LisEmployeeID))
            {
                var arrId = item.LisEmployeeID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCEmployees.Add(
                        new QualityNCEmployee()
                        {
                            EmployeeID = id,
                            QualityNCID = ncID,
                            CreateAt = DateTime.Now,
                            CreateBy = item.CreateBy
                        }
                    );
                }
            }
            if (!string.IsNullOrEmpty(item.LisRoleID))
            {
                var arrId = item.LisRoleID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCRoles.Add(
                        new QualityNCRole()
                        {
                            HumanRoleID = id,
                            QualityNCID = ncID,
                            CreateAt = DateTime.Now,
                            CreateBy = item.CreateBy
                        }
                    );
                }
            }
            var auditResult = dbo.QualityAuditResults.FirstOrDefault(t => t.ISOIndexCriteriaID == qualityCriteriaID && t.QualityAuditProgramISOID == auditId && t.HumanDepartmentID == item.Department.ID);
            auditResult.QualityNCID = ncID;
            dbo.SaveChanges();
        }

        public void InsertFromAuditRiskControl(QualityNCItem item, int RiskControlID = 0)
        {
            var dbo = NCDA.Repository;
            var NC = new QualityNC()
            {
                DepartmentID = item.Department.ID,
                Time = item.Time,
                Content = item.Content,
                Source = item.Source,
                IsObs = item.IsObs,
                IsMaximum = item.IsMaximum,
                IsMedium = item.IsMedium,
                IsVerify = item.IsVerify,
                IsTrue = item.IsTrue,
                IsCompleteAuto = item.IsCompleteAuto,
                IsComplete = item.IsComplete,
                CheckAt = item.CheckAt,
                CheckNote = item.CheckNote,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now,
            };
            var ncID = dbo.QualityNCs.Add(NC).ID;
            if (!string.IsNullOrEmpty(item.LisEmployeeID))
            {
                var arrId = item.LisEmployeeID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCEmployees.Add(
                        new QualityNCEmployee()
                        {
                            EmployeeID = id,
                            QualityNCID = ncID,
                            CreateAt = DateTime.Now,
                            CreateBy = item.CreateBy
                        }
                    );
                }
            }
            if (!string.IsNullOrEmpty(item.LisRoleID))
            {
                var arrId = item.LisRoleID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCRoles.Add(
                            new QualityNCRole()
                            {
                                HumanRoleID = id,
                                QualityNCID = ncID,
                                CreateAt = DateTime.Now,
                                CreateBy = item.CreateBy
                            }
                    );
                }
            }
            var auditResult = dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == RiskControlID);
            auditResult.QualityNCID = ncID;
            dbo.SaveChanges();
        }



        public int InsertReturnID(QualityNCItem item)
        {
            var dbo = NCDA.Repository;
            var NC = new QualityNC()
            {
                DepartmentID = item.Department.ID,
                Time = item.Time,
                Content = item.Content,
                //CriteriaID = item.Criteria.ID,
                //FromProblem = item.FromProblem,
                IsObs = item.IsObs,
                IsMaximum = item.IsMaximum,
                IsMedium = item.IsMedium,
                IsVerify = item.IsVerify,
                IsTrue = item.IsTrue,
                IsCompleteAuto = item.IsCompleteAuto,
                IsComplete = item.IsComplete,
                CheckAt = item.CheckAt,
                CheckNote = item.CheckNote,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now,
            };
            dbo.QualityNCs.Add(NC);
            dbo.SaveChanges();
            var ncID = NC.ID;
            if (!string.IsNullOrEmpty(item.LisEmployeeID))
            {
                var arrId = item.LisEmployeeID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCEmployees.Add(
                                                new QualityNCEmployee()
                                                {
                                                    EmployeeID = id,
                                                    QualityNCID = ncID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            if (!string.IsNullOrEmpty(item.LisRoleID))
            {
                var arrId = item.LisRoleID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCRoles.Add(
                                                new QualityNCRole()
                                                {
                                                    HumanRoleID = id,
                                                    QualityNCID = ncID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            dbo.SaveChanges();
            return NC.ID;
        }
        public void UpdateIsLock(int id)
        {
            var NC = NCDA.GetById(id);
            NC.IsLock = true;
            NCDA.Save();

        }
        public void Update(QualityNCItem item, int userID)
        {
            var dbo = NCDA.Repository;
            var NC = dbo.QualityNCs.Find(item.ID);
            NC.DepartmentID = item.Department.ID;
            NC.Time = item.Time;
            NC.Content = item.Content;
            //NC.CriteriaID = item.Criteria.ID;
            NC.Source = item.Source;
            NC.IsObs = item.IsObs;
            NC.IsMaximum = item.IsMaximum;
            NC.IsMedium = item.IsMedium;
            NC.IsVerify = item.IsVerify;
            NC.IsTrue = item.IsTrue;
            NC.IsCompleteAuto = item.IsCompleteAuto;
            NC.IsComplete = item.IsComplete;
            NC.CheckAt = item.CheckAt;
            NC.CheckNote = item.CheckNote;
            NC.UpdateAt = DateTime.Now;
            NC.UpdateBy = userID;
            var oldNCEmployee = dbo.QualityNCEmployees.Where(i => i.QualityNCID == item.ID);
            dbo.QualityNCEmployees.RemoveRange(oldNCEmployee);
            if (!string.IsNullOrEmpty(item.LisEmployeeID))
            {
                var arrId = item.LisEmployeeID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCEmployees.Add(
                                                new QualityNCEmployee()
                                                {
                                                    EmployeeID = id,
                                                    QualityNCID = item.ID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            var oldNCRole = dbo.QualityNCRoles.Where(i => i.QualityNCID == item.ID);
            dbo.QualityNCRoles.RemoveRange(oldNCRole);
            if (!string.IsNullOrEmpty(item.LisRoleID))
            {
                var arrId = item.LisRoleID.Split(',').Select(n => int.Parse(n)).Distinct();
                foreach (var id in arrId)
                {
                    dbo.QualityNCRoles.Add(
                                                new QualityNCRole()
                                                {
                                                    HumanRoleID = id,
                                                    QualityNCID = item.ID,
                                                    CreateAt = DateTime.Now,
                                                    CreateBy = item.CreateBy
                                                }
                        );
                }
            }
            dbo.SaveChanges();
        }
        public void Verify(QualityNCItem item, int userID)
        {
            var dbo = NCDA.Repository;
            var NC = dbo.QualityNCs.Find(item.ID);
            //NC.DepartmentID = item.Department.ID;
            //NC.Time = item.Time;
            //NC.Content = item.Content;
            //NC.CriteriaID = item.Criteria.ID;
            //NC.FromProblem = item.FromProblem;
            //NC.IsObs = item.IsObs;
            //NC.IsMaximum = item.IsMaximum;
            //NC.IsMedium = item.IsMedium;
            NC.IsVerify = item.IsVerify;
            NC.IsTrue = item.IsTrue;
            //NC.IsCompleteAuto = item.IsCompleteAuto;
            //NC.IsComplete = item.IsComplete;
            NC.CheckAt = item.CheckAt;
            NC.CheckNote = item.CheckNote;
            NC.UpdateAt = DateTime.Now;
            NC.UpdateBy = userID;
            //var auditResultSV = new Audit.ResultService();
            //if (item.IsTrue)
            //{
            //    if (dbo.MnAuditResult.FirstOrDefault(i => i.NCID == item.ID) == null)
            //    {
            //        auditResultSV.InsertByCriteriaID(item.Criteria.ID, item.ID, userID, item.CheckBy.ID);
            //    }
            //}
            //else
            //{
            //    auditResultSV.DeleteByNCID(item.ID);
            //}
            dbo.SaveChanges();
        }
        public void Delete(int id)
        {
            var dbo = NCDA.Repository;
            dbo.QualityNCEmployees.RemoveRange(dbo.QualityNCEmployees.Where(i => i.QualityNCID == id));
            dbo.QualityNCRoles.RemoveRange(dbo.QualityNCRoles.Where(i => i.QualityNCID == id));
            dbo.QualityNCs.Remove(dbo.QualityNCs.FirstOrDefault(i => i.ID == id));
            dbo.SaveChanges();
        }
    }
}
