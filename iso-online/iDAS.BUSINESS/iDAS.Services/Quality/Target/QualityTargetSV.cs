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
    public class QualityTargetSV
    {
        private QualityTargetDA targetDA = new QualityTargetDA();
        DepartmentSV depSV = new DepartmentSV();
        public QualityTargetItem GetByID(int id)
        {
            var dbo = targetDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            QualityTargetItem obj = new QualityTargetItem();
            var target = targetDA.GetById(id);
            obj.ID = target.ID;
            obj.QualityTargetCategoryID = target.QualityTargetCategoryID;
            obj.QualityTargetCategoryName = target.QualityTargetCategoryID.HasValue ? target.QualityTargetCategory.Name : "";
            obj.ApprovalAt = target.ApprovalAt;
            obj.DepartmentName = dbo.HumanDepartments.Where(t => t.ID == target.DepartmentID).FirstOrDefault().Name;
            obj.ApprovalBy = target.ApprovalBy;
            obj.Name = target.Name;
            obj.Description = target.Description;
            obj.CreateAt = target.CreateAt;
            obj.CreateBy = target.CreateBy;
            obj.Approval = employeeSV.GetEmployeeView(target.ApprovalBy);
            obj.CompleteAt = target.CompleteAt;
            obj.DepartmentID = target.DepartmentID;
            obj.IsAccept = target.IsAccept;
            obj.IsApproval = target.IsApproval;
            obj.Department = new HumanDepartmentViewItem()
            {
                ID = target.DepartmentID,
                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == target.DepartmentID).Name
            };
            obj.IsDelete = target.IsDelete;
            obj.IsEdit = target.IsEdit;
            obj.IsEnd = target.IsEnd;
            obj.IsSuccess = target.IsSuccess;
            obj.Unit = target.Unit;
            obj.Value = target.Value;
            obj.ParentID = target.ParentID;// "Đến ngày: " + target.CompleteAt.ToShortDateString() + " " + item.Name + item.Value.ToString() + " " + item.Unit;
            obj.ParentName = dbo.QualityTargets.FirstOrDefault(i => i.ID == target.ParentID) != null ? "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(i => i.ID == target.ParentID).CompleteAt.ToShortDateString() + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == target.ParentID).Name + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == target.ParentID).Value + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == target.ParentID).Unit : string.Empty;
            obj.Type = target.Type;
            obj.UpdateAt = target.UpdateAt;
            obj.ApprovalAt = target.ApprovalAt;
            obj.ApprovalNote = target.ApprovalNote;
            obj.QualityTargetLevelID = target.QualityTargetLevelID;
            obj.UpdateBy = target.UpdateBy;
            obj.CreateByEmployeeID = target.CreateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == target.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == target.CreateBy).HumanEmployeeID : null;
            obj.UpdateByEmployeeID = target.UpdateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == target.UpdateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == target.UpdateBy).HumanEmployeeID : null;
            return obj;
        }
        public List<QualityTargetItem> GetIsAccept(int page, int pageSize, out int totalCount, int departmentId)
        {
            var dbo = targetDA.Repository;
            var lstDepChild = depSV.GetListDepartmentByParentID(departmentId).Select(i => i.ID).ToList();
            List<QualityTargetItem> lst = new List<QualityTargetItem>();
            var targets = targetDA.GetQuery()
                        .Where(t => lstDepChild.Contains(t.DepartmentID) && t.IsEnd == false)
                         .Where(t => t.IsApproval && t.IsAccept && t.CompleteAt >= DateTime.Now)
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            if (targets.Count > 0)
            {
                foreach (var item in targets)
                {
                    lst.Add(new QualityTargetItem
                    {
                        ID = item.ID,
                        TargetName = "Đến ngày: " + item.CompleteAt.ToString("dd/MM/yyyy") + " " + item.Name + " " + item.Value.ToString() + " " + item.Unit,
                        Name = item.Name,
                        Value = item.Value,
                        Unit = item.Unit,
                        Description = item.Description,
                        QualityTargetCategoryID = item.QualityTargetCategoryID,
                        QualityTargetCategoryName = item.QualityTargetCategoryID.HasValue ? item.QualityTargetCategory.Name : "",
                        CreateBy = item.CreateBy,
                        IsEnd = item.IsEnd,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        CreateAt = item.CreateAt,
                        IsSuccess = item.IsSuccess,
                        ApprovalAt = item.ApprovalAt,
                        ApprovalBy = item.ApprovalBy,
                        DepartmentID = item.DepartmentID,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        IsDelete = item.IsDelete,
                        IsEdit = item.IsEdit,
                        ParentID = item.ParentID,
                        Type = item.Type,
                        CompleteAt = item.CompleteAt,
                        //ParentName = dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID) != null ? "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Date.ToString() + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID).QualityTargetType.Name + iDAS.Utilities.Common.GetFormName(item.Form.Value) + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Number.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Unit : string.Empty
                        ParentName = dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID) != null ? "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).CompleteAt.ToString("dd/MM/yyyy") + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID).Name + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Value.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Unit : string.Empty
                    });
                }
            }
            return lst;
        }
        public List<QualityTargetItem> GetAll(ModelFilter filter, int departmentId, int focusId = 0)
        {
            var dbo = targetDA.Repository;
            //  var lstDepChild = depSV.GetListDepartmentByParentID(departmentId).Select(i => i.ID).ToList();
            List<QualityTargetItem> lst = new List<QualityTargetItem>();
            IQueryable<iDAS.Base.QualityTarget> query = dbo.QualityTargets
                //  .Where(t => lstDepChild.Contains(t.DepartmentID))
                      .Where(t => t.DepartmentID == departmentId)
                        .OrderByDescending(i => i.CreateAt);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var targets = query
                        .Filter(filter)
                        .ToList();
            if (targets.Count > 0)
            {
                foreach (var item in targets)
                {
                    lst.Add(new QualityTargetItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Value = item.Value,
                        //TargetName = iDAS.Utilities.Common.GetFormName(item.Form.Value) + item.Number + " " + item.Unit,
                        Unit = item.Unit,
                        Description = item.Description,
                        QualityTargetCategoryID = item.QualityTargetCategoryID,
                        QualityTargetCategoryName = item.QualityTargetCategoryID.HasValue ? item.QualityTargetCategory.Name : "",
                        CreateBy = item.CreateBy,
                        CateName = item.QualityTargetCategory != null ? item.QualityTargetCategory.Name : string.Empty,
                        IsEnd = item.IsEnd,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        CreateAt = item.CreateAt,
                        IsSuccess = item.IsSuccess,
                        ApprovalAt = item.ApprovalAt,
                        ApprovalBy = item.ApprovalBy,
                        DepartmentID = item.DepartmentID,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        IsDelete = item.IsDelete,
                        IsEdit = item.IsEdit,
                        ParentID = item.ParentID,
                        Type = item.Type,
                        CompleteAt = item.CompleteAt,
                        //ParentName = dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID) != null ? "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Date.ToString() + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID).QualityTargetType.Name + iDAS.Utilities.Common.GetFormName(item.Form.Value) + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Number.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Unit : string.Empty
                        ParentName = dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID) != null ? "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).CompleteAt.ToString("dd/MM/yyyy") + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID).Name + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Value.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Unit : string.Empty
                    });
                }
            }
            return lst;
        }
        /// <summary>
        /// Lấy danh sách các mục tiêu
        /// </summary>
        /// <returns></returns>
        public List<QualityTargetItem> GetAll()
        {
            var dbo = targetDA.Repository;
            List<QualityTargetItem> lst = new List<QualityTargetItem>();
            var targets = targetDA.GetQuery()
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            if (targets.Count > 0)
            {
                foreach (var item in targets)
                {
                    lst.Add(new QualityTargetItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Value = item.Value,
                        Unit = item.Unit,
                        Description = item.Description,
                        QualityTargetCategoryID = item.QualityTargetCategoryID,
                        QualityTargetCategoryName = item.QualityTargetCategoryID.HasValue ? item.QualityTargetCategory.Name : "",
                        CreateBy = item.CreateBy,
                        IsEnd = item.IsEnd,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        CreateAt = item.CreateAt,
                        IsSuccess = item.IsSuccess,
                        ApprovalAt = item.ApprovalAt,
                        ApprovalBy = item.ApprovalBy,
                        DepartmentID = item.DepartmentID,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        IsDelete = item.IsDelete,
                        IsEdit = item.IsEdit,
                        ParentID = item.ParentID,
                        Type = item.Type,
                        CompleteAt = item.CompleteAt,
                        TargetName = "Đến ngày: " + item.CompleteAt.ToString("dd/MM/yyyy") + " " + item.Name + item.Value.ToString() + " " + item.Unit,
                        ParentName = dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID) != null ? "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).CompleteAt.ToString("dd/MM/yyyy") + " " + dbo.QualityTargets.FirstOrDefault(i => i.ID == item.ParentID).Name + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Value.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == item.ParentID).Unit : string.Empty
                    });
                }
            }
            return lst;
        }
        public List<QualityTargetItem> GetParentTarget()
        {
            var dbo = targetDA.Repository;
            List<QualityTargetItem> lst = new List<QualityTargetItem>();
            var targets = targetDA.GetQuery()
                // .Where(t => t.IsAccept == true && t.IsEnd == false)
                .OrderByDescending(item => item.CreateAt)
                .ToList();
            if (targets.Count > 0)
            {
                foreach (var item in targets)
                {
                    lst.Add(new QualityTargetItem
                    {
                        ID = item.ID,
                        TargetName = "Đến ngày: " + item.CompleteAt.ToShortDateString() + " " + item.Name + item.Value.ToString() + " " + item.Unit,
                        CompleteAt = item.CompleteAt
                    });
                }
            }
            return lst;
        }
        public int Insert(QualityTargetItem objNew, int userId)
        {
            QualityTarget obj = new QualityTarget();
            obj.Value = objNew.Value;
            obj.Unit = objNew.Unit;
            obj.QualityTargetCategoryID = objNew.QualityTargetCategoryID;
            obj.Description = objNew.Description;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.CompleteAt = (DateTime)objNew.CompleteAt;
            obj.DepartmentID = objNew.Department.ID;
            obj.IsAccept = false;
            obj.ApprovalBy = objNew.Approval.ID;
            obj.IsApproval = false;
            obj.IsDelete = false;
            obj.IsEdit = objNew.IsEdit;
            obj.QualityTargetLevelID = objNew.QualityTargetLevelID;
            obj.IsEnd = false;
            obj.IsSuccess = false;
            obj.Name = objNew.Name;
            obj.ParentID = objNew.ParentID == null ? 0 : objNew.ParentID;
            obj.Type = objNew.Type;
            targetDA.Insert(obj);
            targetDA.Save();
            return obj.ID;
        }
        public int Insert(QualityTargetItem objNew)
        {
            QualityTarget obj = new QualityTarget();
            obj.Unit = objNew.Unit;
            obj.QualityTargetCategoryID = objNew.QualityTargetCategoryID;
            obj.Value = objNew.Value;
            obj.Description = objNew.Description;
            obj.CreateAt = DateTime.Now;
            obj.CompleteAt = objNew.CompleteAt;
            obj.DepartmentID = objNew.Department.ID;
            obj.QualityTargetLevelID = objNew.QualityTargetLevelID;
            obj.IsAccept = false;
            obj.ApprovalBy = objNew.Approval != null ? objNew.Approval.ID : obj.ApprovalBy;
            obj.IsApproval = false;
            obj.IsDelete = false;
            obj.IsEdit = objNew.IsEdit;
            obj.IsEnd = false;
            obj.IsSuccess = false;
            obj.Name = objNew.Name;
            obj.ParentID = objNew.ParentID == null ? 0 : objNew.ParentID;
            obj.Type = objNew.Type;
            targetDA.Insert(obj);
            targetDA.Save();
            return obj.ID;
        }
        public void Update(QualityTargetItem objEdit, int userId)
        {
            QualityTarget obj = targetDA.GetById(objEdit.ID);
            obj.Value = objEdit.Value;
            obj.Unit = objEdit.Unit;
            obj.Description = objEdit.Description;
            obj.QualityTargetLevelID = objEdit.QualityTargetLevelID;
            obj.QualityTargetCategoryID = objEdit.QualityTargetCategoryID;
            obj.UpdateAt = DateTime.Now;
            obj.IsApproval = objEdit.IsApproval;
            obj.UpdateBy = userId;
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.CompleteAt = objEdit.CompleteAt;
            obj.DepartmentID = objEdit.Department.ID;
            obj.IsEdit = objEdit.IsEdit;
            obj.Name = objEdit.Name;
            obj.ParentID = objEdit.ParentID == null ? 0 : objEdit.ParentID;
            obj.Type = objEdit.Type;
            targetDA.Update(obj);
            targetDA.Save();

        }
        public void Delete(int id)
        {
            targetDA.Delete(id);
            targetDA.Save();
        }
        public void Approve(QualityTargetItem objEdit, int userId)
        {
            QualityTarget obj = targetDA.GetById(objEdit.ID);
            obj.IsAccept = objEdit.IsAccept;
            obj.IsApproval = true;
            obj.ApprovalAt = objEdit.ApprovalAt;
            obj.ApprovalNote = objEdit.ApprovalNote;
            obj.IsEdit = objEdit.IsEdit;
            targetDA.Update(obj);
            targetDA.Save();

        }
        public QualityTargetItem GetTargetEnd(int id)
        {
            return targetDA.GetQuery(i => i.ID == id)
                .Select(item => new QualityTargetItem()
                {
                    ID = item.ID,
                    IsEnd = item.IsEnd,
                    EndAt = item.EndAt,
                }).FirstOrDefault();
        }
        public void End(QualityTargetItem item, int userId)
        {
            var endItem = targetDA.GetById(item.ID);
            endItem.EndAt = item.EndAt;
            endItem.IsEnd = item.IsEnd;
            endItem.UpdateBy = userId;
            endItem.UpdateAt = DateTime.Now;
            targetDA.Save();
        }
        public List<QualityTargetItem> GetChildTarget(int id, bool isRoot)
        {
            var dbo = targetDA.Repository;
            return dbo.QualityTargets.Where(i => (isRoot && i.ID == id) || (i.ParentID == id && !isRoot))
                            .Select(item => new QualityTargetItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                                CompleteAt = item.CompleteAt,
                                Leaf = !dbo.QualityTargets.Any(i => i.ParentID == item.ID)
                            }
                                    ).ToList();
        }
        public List<QualityPlanItem> GetChildPlan(int parentId, bool isRoot)
        {
            var dbo = targetDA.Repository;
            return dbo.QualityPlans
                            .Where(i => (isRoot && i.QualityTargetID == parentId) || (i.ParentID == parentId && !isRoot))
                            .Select(item => new QualityPlanItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                                IsEdit = item.IsEdit,
                                IsApproval = item.IsApproval,
                                IsAccept = item.IsAccept,
                                Leaf = !dbo.QualityPlans.Any(i => i.ParentID == item.ID)
                            }
                                    ).ToList();
        }
        public List<QualityTargetItem> GetTreeTarget(int targetId, int cateId, int departmentId, List<int> GroupIds, bool isAdmin, int employeeID, int userId, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string choice, int focusId = 0)
        {
            var dbo = targetDA.Repository;
            var result = new List<QualityTargetItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            if (targetId == 0)
            {
                IQueryable<iDAS.Base.QualityTarget> query;
                if (focusId != 0)
                {
                    query = dbo.QualityTargets.Where(t => t.ID == focusId);
                }
                else
                {
                    query = dbo.QualityTargets
                      .Where(t => cateId == 0 || t.QualityTargetCategoryID == cateId)
                      .Where(t => t.DepartmentID == departmentId)
                      .Where(t => t.DepartmentID == departmentId || t.ParentID == 0)
                      .Where(t => isAdmin || t.CreateBy == userId || t.UpdateBy == userId || t.ApprovalBy == employeeID || GroupIds.Contains(t.DepartmentID))
                      .Where(t => (choice.Equals("App.Time") && (t.CompleteAt >= fromDateForQr && t.CompleteAt <= toDateQr)) || choice.Equals("App.All"))
                      .Distinct()
                      .OrderByDescending(t => t.ID);
                }
                result = query
                          .Select(item => new QualityTargetItem()
                          {
                              ID = item.ID,
                              Name = item.Name,
                              Value = item.Value,
                              Unit = item.Unit,
                              Description = item.Description,
                              QualityTargetCategoryID = item.QualityTargetCategoryID,
                              QualityTargetCategoryName = item.QualityTargetCategoryID.HasValue ? item.QualityTargetCategory.Name : "",
                              CreateBy = item.CreateBy,
                              CateName = item.QualityTargetCategory != null ? item.QualityTargetCategory.Name : string.Empty,
                              IsEnd = item.IsEnd,
                              UpdateAt = item.UpdateAt,
                              UpdateBy = item.UpdateBy,
                              CreateAt = item.CreateAt,
                              IsSuccess = item.IsSuccess,
                              ApprovalAt = item.ApprovalAt,
                              ApprovalBy = item.ApprovalBy,
                              DepartmentID = item.DepartmentID,
                              IsAccept = item.IsAccept,
                              IsApproval = item.IsApproval,
                              IsDelete = item.IsDelete,
                              IsEdit = item.IsEdit,
                              ParentID = item.ParentID,
                              Type = item.Type,
                              CompleteAt = item.CompleteAt,
                              EndAt = item.EndAt,
                              LevelName = item.QualityTargetLevel != null ? item.QualityTargetLevel.Name : string.Empty,
                              LevelColor = item.QualityTargetLevel != null ? item.QualityTargetLevel.Color : string.Empty,
                              Leaf = dbo.QualityTargets.FirstOrDefault(i => i.ParentID == item.ID) == null
                          })
                          .ToList();
                var lis = result.Select(t => t.ID).ToArray();
                List<QualityTargetItem> taskremove = new List<QualityTargetItem>();
                foreach (var item in result)
                {
                    if (!lis.Contains(item.ParentID.Value))
                    {
                        taskremove.Add(item);
                    }
                }
                result = taskremove;
            }
            else
            {
                result = targetDA
                           .GetQuery()
                           .Where(t => t.ParentID == targetId)
                           .Where(t => isAdmin || t.CreateBy == userId || t.UpdateBy == userId || t.ApprovalBy == employeeID || GroupIds.Contains(t.DepartmentID))
                           .Where(t => (choice.Equals("App.Time") && (t.CompleteAt >= fromDateForQr && t.CompleteAt <= toDateQr)) || choice.Equals("App.All"))
                           .Distinct()
                           .OrderByDescending(t => t.ID)
                           .Select(item => new QualityTargetItem()
                           {
                               ID = item.ID,
                               Name = item.Name,
                               Value = item.Value,
                               Unit = item.Unit,
                               Description = item.Description,
                               QualityTargetCategoryID = item.QualityTargetCategoryID,
                               QualityTargetCategoryName = item.QualityTargetCategoryID.HasValue ? item.QualityTargetCategory.Name : "",
                               CreateBy = item.CreateBy,
                               CateName = item.QualityTargetCategory != null ? item.QualityTargetCategory.Name : string.Empty,
                               IsEnd = item.IsEnd,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               CreateAt = item.CreateAt,
                               IsSuccess = item.IsSuccess,
                               ApprovalAt = item.ApprovalAt,
                               ApprovalBy = item.ApprovalBy,
                               DepartmentID = item.DepartmentID,
                               IsAccept = item.IsAccept,
                               IsApproval = item.IsApproval,
                               IsDelete = item.IsDelete,
                               IsEdit = item.IsEdit,
                               ParentID = item.ParentID,
                               Type = item.Type,
                               CompleteAt = item.CompleteAt,
                               EndAt = item.EndAt,
                               LevelName = item.QualityTargetLevel != null ? item.QualityTargetLevel.Name : string.Empty,
                               LevelColor = item.QualityTargetLevel != null ? item.QualityTargetLevel.Color : string.Empty,
                               Leaf = dbo.QualityTargets.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .ToList();
            }
            return result;
        }
    }
}
