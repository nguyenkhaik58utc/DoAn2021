using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Base;
using iDAS.Utilities;
using iDAS.Items;

namespace iDAS.Services
{
    public class DocumentRequirementSV
    {
        DocumentRequirementDA requestDA = new DocumentRequirementDA();
        HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        HumanUserSV userSV = new HumanUserSV();
        DocumentRequriedTaskDA reuqestTaskDA = new DocumentRequriedTaskDA();
        public List<DocumentRequirementItem> GetAll(ModelFilter filter, int groupId, int focusId=0)
        {
            var dbo = requestDA.Repository;
            IQueryable<iDAS.Base.DocumentRequirement> query = dbo.DocumentRequirements
                        .Where(p => p.DepartmentID == groupId || (groupId==0 && p.ID==focusId))
                        .OrderByDescending(item => item.CreateAt);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var services = query.Filter(filter)
                    .Select(i => new DocumentRequirementItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Content = i.Content,
                        ContentChange = i.ContentChange,
                        ContentOld = i.ContentOld,
                        Type = i.Type,
                        StrType = i.Type ? "Sửa đổi" : "Viết mới",
                        IsApproval = i.IsApproval,
                        IsEdit = i.IsEdit,
                        IsAccept = i.IsAccept,
                        DocumentID = i.DocumentID,
                        Document = i.Document.Name,
                        Note = i.Note,
                        ApproveBy = i.ApprovalBy,
                        ApproveAt = i.ApprovalAt,
                        Status = i.IsApproval ? "Đã Duyệt" : "Chờ duyệt",
                        ReasonChange = i.ReasonChange,
                        //IsActive = i.IsActive,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        UpdateBy = i.UpdateBy,
                        CreateBy = i.CreateBy,
                        AttachmentFileIDs = dbo.DocumentAttachmentFiles.Where(t => t.DocumentID == i.DocumentID).Select(x => x.AttachmentFileID).ToList()
                    })
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                foreach (var item in services)
                {
                    item.Approval = sysUserSV.GetEmployeeView(item.ApproveBy);
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept);
                }
            }
            return services;
        }
        public DocumentRequirementItem GetByID(int Id)
        {
            var item = requestDA.GetQuery(p => p.ID == Id)
                    .FirstOrDefault();
            if (item != null)
            {
                DocumentRequirementItem obj = convertToRequestItem(item);
                return obj;
            }
            return null;
        }
        public List<TaskItem> GetTreeTask(int taskId, int requestId)
        {
            var dbo = reuqestTaskDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = dbo.Tasks.Join(dbo.DocumentTasks.Where(x => x.DocumentRequirementID == requestId), t => t.ID, dt => dt.TaskID, (t, dt) => new
                TaskItem
                    {
                        ID = t.ID,
                        Name = t.Name,
                        Content = t.Content,
                        IsComplete = t.IsComplete,
                        ParentID = t.ParentID,
                        CreateBy = t.CreateBy,
                        LevelColor = t.TaskLevel.Color,
                        IsEdit = t.IsEdit,
                        IsNew = t.IsNew,
                        IsPrivate = t.IsPrivate,
                        CategoryName = t.TaskCategory.Name,
                        Rate = t.Rate,
                        StartTime = t.StartTime,
                        UpdateAt = t.UpdateAt,
                        UpdateBy = t.UpdateBy,
                        CreateAt = t.CreateAt,
                        LevelID = t.TaskLevelID,
                        EndTime = t.EndTime,
                        IsPause = t.IsPause,
                        IsPass = t.IsPass,
                        LevelName = t.TaskLevel.Name,
                        PerformBy = t.HumanEmployeeID,
                    Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null
                })
                .OrderByDescending(item => item.CreateAt)
                .ToList();
                var lis = result.Select(t => t.ID).ToArray();
                List<TaskItem> taskremove = new List<TaskItem>();
                foreach (var item in result)
                {
                    if (!lis.Contains(item.ParentID))
                    {
                        taskremove.Add(item);
                    }
                }
                result = taskremove.OrderByDescending(t => t.CreateAt.Value).ToList();
            }
            else
            {
                result = dbo.Tasks.Join(dbo.DocumentTasks.Where(x => x.DocumentRequirementID == requestId), t => t.ID, dt => dt.TaskID, (t, dt) => new
                TaskItem
                    {
                        ID = t.ID,
                        Name = t.Name,
                        Content = t.Content,
                        IsComplete = t.IsComplete,
                        CreateBy = t.CreateBy,
                        LevelColor = t.TaskLevel.Color,
                        IsEdit = t.IsEdit,
                        IsNew = t.IsNew,
                        ParentID = t.ParentID,
                        IsPrivate = t.IsPrivate,
                        CategoryName = t.TaskCategory.Name,
                        Rate = t.Rate,
                        StartTime = t.StartTime,
                        UpdateAt = t.UpdateAt,
                        UpdateBy = t.UpdateBy,
                        CreateAt = t.CreateAt,
                        LevelID = t.TaskLevelID,
                        EndTime = t.EndTime,
                        IsPause = t.IsPause,
                        IsPass = t.IsPass,
                        LevelName = t.TaskLevel.Name,
                        PerformBy = t.HumanEmployeeID,
                    Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null
                })
               .Where(i => i.ParentID == taskId)
               .OrderByDescending(item => item.CreateAt)
               .Distinct()
               .ToList();
            }
            return result;
        }
        public List<TaskItem> GetAllTaskByID(ModelFilter filter, int requestId)
        {
            var dbo = reuqestTaskDA.Repository;
            List<TaskItem> lst = new List<TaskItem>();
            var task1s = dbo.Tasks.Join(dbo.DocumentTasks.Where(x => x.DocumentRequirementID == requestId), t => t.ID, dt => dt.TaskID, (t, dt) => new
                TaskItem
                    {
                        ID = t.ID,
                        Name = t.Name,
                        Content = t.Content,
                        IsComplete = t.IsComplete,
                        CreateBy = t.CreateBy,
                        LevelColor = t.TaskLevel.Color,
                        IsEdit = t.IsEdit,
                        IsNew = t.IsNew,
                        IsPrivate = t.IsPrivate,
                        CategoryName = t.TaskCategory.Name,
                        Rate = t.Rate,
                        StartTime = t.StartTime,
                        UpdateAt = t.UpdateAt,
                        UpdateBy = t.UpdateBy,
                        CreateAt = t.CreateAt,
                        LevelID = t.TaskLevelID,
                        EndTime = t.EndTime,
                        IsPause = t.IsPause,
                        IsPass = t.IsPass,
                        LevelName = t.TaskLevel.Name,
                        PerformBy = t.HumanEmployeeID

                    })

                    .OrderByDescending(t => t.Name).Filter(filter)
                    .ToList();
            return task1s;
        }


        //add\
        public void Insert(DocumentRequirementItem obj)
        {
            var itm = new DocumentRequirement
            {
                Name = obj.Name.Trim(),
                DepartmentID = obj.Department.ID,
                Type = obj.TypeID == (int)RequestType.Modified ? true : false,
                Content = obj.Content,
                ContentChange = obj.ContentChange,
                ContentOld = obj.ContentOld,
                ReasonChange = obj.ReasonChange,
                DocumentID = (obj.DocumentID != null && obj.DocumentID > 0) ? obj.DocumentID : null,
                IsEdit = obj.IsEdit,
                IsApproval = false,
                IsAccept = false,
                ApprovalBy = obj.Approval.ID,
                FinishDate = obj.FinishDateExpected,
                CreateAt = DateTime.Now,
                CreateBy = obj.CreateBy

            };
            requestDA.Insert(itm);
            requestDA.Save();
        }
        public int InsertRequest(DocumentRequirementItem obj)
        {
            var itm = new DocumentRequirement
            {
                Name = obj.Name.Trim(),
                DepartmentID = obj.Department.ID,
                Type = obj.TypeID == (int)RequestType.Modified ? true : false,
                Content = obj.Content,
                ContentChange = obj.ContentChange,
                ContentOld = obj.ContentOld,
                ReasonChange = obj.ReasonChange,
                DocumentID = (obj.DocumentID != null && obj.DocumentID > 0) ? obj.DocumentID : null,
                IsEdit = obj.IsEdit,
                IsApproval = false,
                IsAccept = false,
                ApprovalBy = obj.Approval.ID,
                FinishDate = obj.FinishDateExpected,
                CreateAt = DateTime.Now,
                CreateBy = obj.CreateBy

            };
            requestDA.Insert(itm);
            requestDA.Save();
            return itm.ID;
        }
        public void InsertTask(int taskID, int ReqID, int useID, string note)
        {
            var itm = new DocumentTask
            {
                TaskID = taskID,
                DocumentRequirementID = ReqID,
                CreateAt = DateTime.Now,
                CreateBy = useID,
                Note = note
            };
            reuqestTaskDA.Insert(itm);
            reuqestTaskDA.Save();
        }

        public void Update(DocumentRequirementItem obj)
        {
            var itm = requestDA.GetById(obj.ID);

            itm.Name = obj.Name.Trim();
            itm.DepartmentID = obj.Department.ID;
            itm.Type = obj.TypeID == 1 ? true : false;
            itm.DocumentID = obj.DocumentID;
            itm.Content = obj.Content;
            itm.ContentChange = obj.ContentChange;
            itm.ContentOld = obj.ContentOld;
            itm.ReasonChange = obj.ReasonChange;
            if (obj.Document != null && obj.DocumentID > 0 && obj.DocumentID != itm.DocumentID)
                itm.DocumentID = obj.DocumentID;
            itm.IsEdit = obj.IsEdit;            //IsApproval = false,
            if (!obj.IsEdit)//Gửi Duyệt Yc
            {
                itm.IsApproval = false;
                //  itm.IsSuccess = false;
            }

            //IsSuccess = false,
            itm.ApprovalBy = obj.Approval.ID;
            itm.FinishDate = obj.FinishDateExpected;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            requestDA.Update(itm);
            requestDA.Save();
        }
        public void UpdateApprove(DocumentRequirementItem obj)
        {
            var itm = requestDA.GetById(obj.ID);
            itm.IsApproval = true;
            if (!obj.IsAccept) itm.IsEdit = obj.IsEdit;
            else
                itm.IsEdit = false;
            itm.IsAccept = obj.IsAccept;
            itm.Note = obj.Note;
            itm.ApprovalAt = DateTime.Now;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            requestDA.Update(itm);
            requestDA.Save();
        }
        private DocumentRequirementItem convertToRequestItem(DocumentRequirement i)
        {
            var dbo = requestDA.Repository;
            DocumentRequirementItem obj = new DocumentRequirementItem()
                       {
                           ID = i.ID,
                           Name = i.Name,
                           Content = i.Content,
                           ContentChange = i.ContentChange,
                           ContentOld = i.ContentOld,
                           Department = new HumanDepartmentViewItem()
                           {
                               ID = i.DepartmentID,
                               Name = dbo.HumanDepartments.Where(d => d.ID == i.DepartmentID).Select(d => d.Name).FirstOrDefault()
                           },
                           Type = i.Type,
                           TypeID = i.Type ? 1 : 0,
                           StrType = i.Type ? "Sửa đổi" : "Viết mới",
                           IsApproval = i.IsApproval,
                           IsEdit = i.IsEdit,
                           IsAccept = i.IsAccept,
                           Note = i.Note,
                           DocumentID = i.DocumentID,
                           Document = i.Document != null ? i.Document.Name : "",
                           Version = i.Document != null ? i.Document.Version : "",
                           IsObsolete = i.Document != null ? i.Document.IsObsolete : false,
                           Times = i.Document != null ? i.Document.PublicNumber.ToString() : "",
                           EmployeesCreateID = dbo.HumanUsers.FirstOrDefault(t => t.ID == i.CreateBy).HumanEmployee.ID,
                           ReasonChange = i.ReasonChange,
                           FinishDateExpected = i.FinishDate,
                           ApproveBy = i.ApprovalBy,
                           ApproveAt = i.ApprovalAt,
                           //IsActive=i.IsActive,
                           CreateAt = i.CreateAt,
                           UpdateAt = i.UpdateAt,
                           CreateBy = i.CreateBy,
                           UpdateBy = i.UpdateBy
                       };
            if (obj != null)
            {
                obj.Status = getStatus(obj.IsEdit, obj.IsApproval, obj.IsAccept);
                obj.StatusApprove = getStatusApprove(obj.IsApproval, obj.IsAccept);
                obj.StatusEdit = obj.IsEdit == true ? Common.ApproveStatus.Accept.ToString() : Common.ApproveStatus.Reject.ToString();

                obj.Approval = sysUserSV.GetEmployeeView(obj.ApproveBy);
                if (obj.Approval != null)
                {
                    //obj.Approval = obj.EmployeeInfo.Name;
                    obj.Position = getEmployeeInfo(obj.Approval);
                }
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
            }
            return obj;
        }
        public string getStatus(bool isEdit, bool isApp, bool isSucc)
        {
            if (isEdit && !isApp)
                return "<span style=\"color:#045fb8\">Tạo mới</span>";
            else if (!isEdit && !isApp)
                return "<span style=\"color:#8e210b\">Chờ duyệt</span>";
            else if (isApp && !isSucc && isEdit)
                return "<span style=\"color:red\">Sửa đổi</span>";
            else if (isApp && !isSucc && !isEdit)
                return "<span style=\"color:#41519f\">Không duyệt</span>";
            else if (isApp && isSucc)
                return "<span style=\"color:blue\">Duyệt</span>";
            else
                return "";
        }
        public string getStatusApprove(bool isApp, bool isSucc)
        {
            if (isApp && !isSucc)
                return Common.ApproveStatus.Reject.ToString();
            else if (isApp && isSucc)
                return Common.ApproveStatus.Accept.ToString();
            else
                return "";

        }
        private string getEmployeeInfo(HumanEmployeeViewItem obj)
        {
            string role = "N/A", dept = "N/A";
            if (obj.Role != null && !obj.Role.Trim().Equals(""))
                role = obj.Role;
            if (obj.Department != null && !obj.Department.Trim().Equals(""))
                dept = obj.Department;

            return "Phòng ban:" + role + "<br> Chức danh:" + dept;

        }
        public void Delete(int id)
        {
            var itm = requestDA.GetById(id);
            requestDA.Delete(itm);
            requestDA.Save();
        }
    }
}
