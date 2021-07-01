using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Items;
using iDAS.Core;

namespace iDAS.Services
{
    public class DispatchToSV
    {
        private iDASBusinessEntities db = new iDASBusinessEntities();
        DispatchToDA dispatchToDA = new DispatchToDA();
        DispatchToDepartmentDA dispatchToDeptDA = new DispatchToDepartmentDA();
        DispatchToEmployeeDA dispatchToEmployeeDA = new DispatchToEmployeeDA();
        HumanUserSV userSV = new HumanUserSV();
        HumanEmployeeSV empSV = new HumanEmployeeSV();
        public AttachmentFileItem GetAttachmentByID(int id)
        {
            var dbo = dispatchToDA.Repository;
            AttachmentFileItem AttachmentFiles = new AttachmentFileItem();
            AttachmentFiles.Files = dbo.DispatchToAttachmentFiles.Where(i => i.DispatchToID == id)
                    .Select(i => i.AttachmentFileID).ToList();

            return AttachmentFiles;
        }
        public List<DispatchToItem> GetAll(ModelFilter filter, List<int> userLogonDepts = null, int focusId = 0)
        {
            try
            {
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var query = dispatchToDA.GetQuery();
                if (focusId != 0)
                {
                    filter.SortName = null;
                    query = query.OrderBy(i => i.ID == focusId ? false : true);
                }
                var lst = query
                    .Filter(filter)
                    .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToTypeItemIndex(itm, null, userLogonDepts));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DispatchToItem> GetAllByUserLogOn(ModelFilter filter, int id, int focusId = 0)
        {
            try
            {
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var dbo = dispatchToDA.Repository;
                var query = dbo.DispatchToes
                .Join(dbo.DispatchToEmployees, d => d.ID, e => e.DispatchToID, (d, e) => new { d, e.EmployeeID, e.IsVerify })
                .Where(de => de.EmployeeID == id)
                .OrderByDescending(item => item.d.DateMoved);
                if (focusId != 0)
                {
                    filter.SortName = null;
                    query = query.OrderBy(i => i.d.ID == focusId ? false : true);
                }
                var lst = query
                    .Filter(filter)
                    .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        itm.d.IsVerify = itm.IsVerify;
                        var employee = empSV.GetEmployeeView(id);
                        lstObj.Add(convertToTypeItem(itm.d, employee));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Insert(DispatchToItem obj)
        {
            try
            {
                var itm = new DispatchTo
                {
                    ID = obj.ID,
                    Name = obj.Name,
                    Code = obj.Code,
                    Compendia = obj.Compendia,
                    NumberTo = obj.NumberTo,
                    DispatchSecurityID = obj.SecurityLevelID,
                    DispatchUrgencyID = obj.UrgencyId,
                    DispatchMethodID = obj.MethodID,
                    FormHard = obj.FormH,
                    FormSoft = obj.FormS,
                    Date = obj.Date,
                    DispatchCategoryID = obj.CategoryID,
                    SendFrom = obj.SendFrom,
                    SendTo = obj.SendTo,
                    IsMoved = obj.IsMoved,
                    DateMoved = obj.DateMoved,
                    IsVerify = false,
                    Note = obj.Note,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy
                };

                dispatchToDA.Insert(itm);
                dispatchToDA.Save();
                new FileSV().Upload<DispatchToAttachmentFile>(obj.AttachmentFiles, itm.ID);
                return itm.ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(DispatchToItem obj)
        {
            try
            {
                var itm = dispatchToDA.GetById(obj.ID);
                itm.Name = obj.Name;
                itm.Note = obj.Note;
                itm.Code = obj.Code;
                itm.Compendia = obj.Compendia;
                itm.NumberTo = obj.NumberTo;
                itm.DispatchSecurityID = obj.SecurityLevelID;
                itm.DispatchUrgencyID = obj.UrgencyId;
                itm.DispatchMethodID = obj.MethodID;
                itm.FormHard = obj.FormH;
                itm.FormSoft = obj.FormS;
                itm.Date = obj.Date;
                itm.DispatchCategoryID = obj.CategoryID;
                itm.SendFrom = obj.SendFrom;
                itm.SendTo = obj.SendTo;
                itm.Note = obj.Note;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                dispatchToDA.Update(itm);
                dispatchToDA.Save();
                new FileSV().Upload<DispatchToAttachmentFile>(obj.AttachmentFiles, itm.ID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateMove(DispatchToItem obj, List<int> lst, int userId)
        {
            try
            {
                var itm = dispatchToDA.GetById(obj.ID);
                itm.Code = obj.Code;
                itm.Compendia = obj.Compendia;
                itm.DispatchSecurityID = obj.SecurityLevelID;
                itm.DispatchUrgencyID = obj.UrgencyId;
                itm.DispatchCategoryID = obj.CategoryID;
                itm.IsMoved = true;
                itm.DateMoved = DateTime.Now;
                itm.IsVerify = false;
                itm.NoteMove = obj.NoteMove;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                dispatchToDA.Update(itm);
                dispatchToDA.Save();
                if (obj.Type.Equals(iDAS.Items.DispatchObjectType.Department.ToString()))
                    UpdateDepartment(obj, lst, userId);
                else if (obj.Type.Equals(iDAS.Items.DispatchObjectType.Employee.ToString()))
                    UpdateEmployee(obj, lst, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateVerify(DispatchToItem obj, bool isEmployee)
        {
            try
            {
                var itm = dispatchToDA.GetById(obj.ID);
                itm.IsVerify = true;
                itm.DateVerifyTime = DateTime.Now;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                dispatchToDA.Update(itm);
                dispatchToDA.Save();
                if (!isEmployee)
                {
                    UpdateDepartmentVerify(obj, obj.DepartmentID);
                }
                else
                {
                    UpdateEmployeeVerify(obj, obj.UserReceive.ID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateDepartment(DispatchToItem obj, List<int> lstDept, int userId)
        {
            try
            {
                var arIDDeptIn = dispatchToDeptDA
                    .GetQuery(p => p.DispatchToID == obj.ID)
                    .Select(p => p.DepartmentID)
                    .ToArray();
                if (arIDDeptIn != null && arIDDeptIn.Count() > 0)
                {
                    //Những Dept đã có thì ko làm j
                    foreach (var itm in lstDept)
                    {
                        if (!arIDDeptIn.Contains(itm))
                        {
                            var item = new DispatchToDepartment
                            {
                                DispatchToID = obj.ID,
                                DepartmentID = itm,
                                IsVerify = false,
                                Note = obj.NoteVerify,
                                SendBy = userId,
                                CreateAt = DateTime.Now,
                                CreateBy = obj.UpdateBy
                            };
                            dispatchToDeptDA.Insert(item);
                        }
                    }
                }
                else
                {
                    foreach (var itm in lstDept)
                    {
                        var item = new DispatchToDepartment
                        {
                            DispatchToID = obj.ID,
                            DepartmentID = itm,
                            IsVerify = false,
                            SendBy = userId,
                            Note = obj.NoteVerify,
                            CreateAt = DateTime.Now,
                            CreateBy = obj.UpdateBy
                        };
                        dispatchToDeptDA.Insert(item);
                    }
                }
                dispatchToDeptDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateDepartmentVerify(DispatchToItem obj, int idDept)
        {
            try
            {
                var itm = dispatchToDeptDA.GetQuery(p => p.DepartmentID == idDept && p.DispatchToID == obj.ID).FirstOrDefault();
                if (itm != null)
                {
                    itm.IsVerify = true;
                    itm.Note = obj.NoteVerify;
                    itm.UpdateAt = DateTime.Now;
                    itm.UpdateBy = obj.UpdateBy;
                    dispatchToDeptDA.Update(itm);
                    dispatchToDeptDA.Save();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<int> GetEmployeeNotify(int dispatchToId = 0, List<int> employee = null)
        {
            List<int> employeess = new List<int>();
            var arIDEmptIn = dispatchToEmployeeDA
                  .GetQuery(p => p.DispatchToID == dispatchToId)
                  .Select(p => p.EmployeeID)
                  .ToArray();
            foreach (var itm in employee)
            {
                if (!arIDEmptIn.Contains(itm))
                {
                    employeess.Add(itm);
                }
            }
            return employeess;
        }
        public void UpdateEmployee(DispatchToItem obj, List<int> lstEmployee, int userId)
        {
            try
            {
                var arIDEmptIn = dispatchToEmployeeDA
                    .GetQuery(p => p.DispatchToID == obj.ID)
                    .Select(p => p.EmployeeID)
                    .ToArray();
                if (arIDEmptIn != null && arIDEmptIn.Count() > 0)
                {
                    //Những Dept đã có thì ko làm j
                    foreach (var itm in lstEmployee)
                    {
                        if (!arIDEmptIn.Contains(itm))
                        {
                            var item = new DispatchToEmployee
                            {
                                DispatchToID = obj.ID,
                                EmployeeID = itm,
                                IsVerify = false,
                                SendBy = userId,
                                Note = obj.NoteVerify,
                                CreateAt = DateTime.Now,
                                CreateBy = obj.UpdateBy
                            };
                            dispatchToEmployeeDA.Insert(item);
                        }
                    }
                }
                else
                {
                    foreach (var itm in lstEmployee)
                    {
                        var item = new DispatchToEmployee
                        {
                            DispatchToID = obj.ID,
                            EmployeeID = itm,
                            Note = obj.NoteVerify,
                            IsVerify = false,
                            SendBy = userId,
                            CreateAt = DateTime.Now,
                            CreateBy = obj.UpdateBy
                        };
                        dispatchToEmployeeDA.Insert(item);
                    }
                }
                dispatchToEmployeeDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateEmployeeVerify(DispatchToItem obj, int idEmp)
        {
            try
            {
                var itm = dispatchToEmployeeDA.GetQuery(p => p.EmployeeID == idEmp && p.DispatchToID == obj.ID).FirstOrDefault();
                if (itm != null)
                {
                    itm.IsVerify = true;
                    itm.Note = obj.NoteVerify;
                    itm.UpdateAt = DateTime.Now;
                    itm.UpdateBy = obj.UpdateBy;
                    dispatchToEmployeeDA.Update(itm);
                    dispatchToEmployeeDA.Save();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(int id)
        {
            try
            {
                var item = dispatchToDA.GetById(id);
                dispatchToDA.Delete(item);
                dispatchToDA.Save();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public DispatchToItem GetByID(int id)
        {
            var dbo = dispatchToDA.Repository;
            var obj = dispatchToDA.GetById(id);
            DispatchToItem objItem = new DispatchToItem();
            if (obj != null)
            {
                objItem = convertToTypeItemIndex(obj);
                objItem.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.DispatchToAttachmentFiles.Where(z => z.DispatchToID == objItem.ID)
                        .Select(d => d.AttachmentFileID).ToList()
                };
                objItem.UserReceive = new HumanEmployeeViewItem()
                {
                    ID = dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID) != null ? (int)dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID).EmployeeID : 0,
                    Name = dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID) == null ? string.Empty : dbo.HumanEmployees.FirstOrDefault(m => m.ID == dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID).EmployeeID).Name,
                    Role = dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID) == null ? string.Empty : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID).EmployeeID).HumanRole.Name,
                    Department = dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID) == null ? string.Empty : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == dbo.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID).EmployeeID).HumanRole.HumanDepartment.Name,
                };
            }
            return objItem;
        }
        public DispatchToItem GetByIDForPerson(int id, int empId)
        {
            var dbo = dispatchToDA.Repository;
            var obj = dispatchToDA.GetById(id);
            DispatchToItem objItem = new DispatchToItem();
            if (obj != null)
            {
                objItem.UserReceive = new HumanEmployeeViewItem()
                {
                    ID = empId,
                    Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == empId).Name,
                    Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == empId).HumanRole.Name,
                    Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == empId).HumanRole.HumanDepartment.Name,
                };
                objItem = convertToTypeItemForPerson(obj, objItem.UserReceive);
                objItem.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.DispatchToAttachmentFiles.Where(z => z.DispatchToID == objItem.ID)
                        .Select(d => d.AttachmentFileID).ToList()
                };

            }
            return objItem;
        }
        public int GetCount()
        {
            int ct = 0;
            var obj = dispatchToDA.GetQuery().OrderByDescending(p => p.CreateAt);
            //if (obj.Count() > 0)
            // ct = obj.FirstOrDefault().NumberTo + 1;
            return ct;

        }
        public DispatchToObjectItem GetObjectVerifyByID(int id, int type)
        {
            DispatchToObjectItem obj = new DispatchToObjectItem();
            if (type == (int)DispatchObjectType.Department)
            {
                var itm = dispatchToDeptDA.GetById(id);
                obj.Name = dispatchToDeptDA.Repository.HumanDepartments.FirstOrDefault(i => i.ID == itm.DepartmentID).Name;
                obj.IsVerify = itm.IsVerify;
                obj.NoteMove = itm.DispatchTo.NoteMove;
                obj.NoteVerify = itm.Note;
                obj.CreateAt = itm.CreateAt;
                obj.CreateBy = itm.CreateBy;
                obj.CreateName = userSV.GetNameByUserID(itm.CreateBy);
                obj.UpdateAt = itm.UpdateAt;
                obj.UpdateBy = itm.UpdateBy;
                obj.UpdateName = userSV.GetNameByUserID(itm.UpdateBy);

            }
            else if (type == (int)DispatchObjectType.Employee)
            {
                var itm = dispatchToEmployeeDA.GetById(id);
                obj.Name = dispatchToDeptDA.Repository.HumanEmployees.FirstOrDefault(i => i.ID == itm.EmployeeID).Name;
                obj.IsVerify = itm.IsVerify;
                obj.NoteMove = itm.DispatchTo.NoteMove;
                obj.NoteVerify = itm.Note;
                obj.CreateAt = itm.CreateAt;
                obj.CreateBy = itm.CreateBy;
                obj.CreateName = userSV.GetNameByUserID(itm.CreateBy);
                obj.UpdateAt = itm.UpdateAt;
                obj.UpdateBy = itm.UpdateBy;
                obj.UpdateName = userSV.GetNameByUserID(itm.UpdateBy);
            }
            return obj;
        }
        public DispatchToItem GetVerifyInfoByID(int dispatchToID, int empID, bool isEmployee = false)
        {
            var dbo = dispatchToDA.Repository;
            var obj = dispatchToDA.GetById(dispatchToID);
            DispatchToItem obi = new DispatchToItem();
            if (obj != null)
            {
                if (!isEmployee)
                {
                    var lst = dispatchToDeptDA.GetQuery(p => p.DispatchTo.ID == dispatchToID).ToList();
                    obj.DispatchToDepartments = lst;
                    var employee = empSV.GetEmployeeView(empID);
                    obi = convertToTypeItem(obj, employee);
                }
                else
                {
                    var lst = dispatchToEmployeeDA.GetQuery(p => p.DispatchTo.ID == dispatchToID).ToList();
                    obj.DispatchToEmployees = lst;
                    var employee = empSV.GetEmployeeView(empID);
                    obi = convertToTypeItem(obj, employee);
                }
                obi.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.DispatchToAttachmentFiles.Where(z => z.DispatchToID == obj.ID)
                        .Select(d => d.AttachmentFileID).ToList()
                };
            }
            return obi;
        }
        public List<DispatchToObjectItem> GetDepartmentByID(int id)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                var itemTmp = dbo.DispatchToes.Find(id);
                var arIDAtt = itemTmp.DispatchToDepartments
                            .Where(p => p.DispatchTo.ID == id)
                            .Select(p => new DispatchToObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.DepartmentID,
                                DispatchTo = id,
                                Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID) != null ? dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID).Name : string.Empty,
                                NoteMove = p.Note,
                                IsVerify = p.IsVerify
                            })
                            .ToList();
                return arIDAtt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DispatchToObjectItem> GetEmployeeByID(int id)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                var itemTmp = dbo.DispatchToes.Find(id);
                var arIDAtt = itemTmp.DispatchToEmployees
                            .Where(p => p.DispatchTo.ID == id)
                            .Select(p => new DispatchToObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.EmployeeID,
                                DispatchTo = id,
                                Name = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID) != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).Name : string.Empty,
                                Role = (dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID) != null && dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault() != null && dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole != null) ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty,
                                NoteMove = p.Note,
                                IsVerify = p.IsVerify
                            })
                            .ToList();
                return arIDAtt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<TaskItem> GetTreeTask(int taskId, int dispatchToID)
        {
            var dbo = dispatchToDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = dbo.Tasks.Join(dbo.DispatchToTasks.Where(x => x.DispatchToID == dispatchToID), t => t.ID, dt => dt.TaskID, (t, dt) => new
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
                result = dbo.Tasks.Join(dbo.DispatchToTasks.Where(x => x.DispatchToID == dispatchToID), t => t.ID, dt => dt.TaskID, (t, dt) => new
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
        public List<DispatchToObjectItem> GetAllObjectVerifyByID(int page, int pageSize, out int totalCount, int id)
        {
            try
            {
                List<DispatchToObjectItem> lst = new List<DispatchToObjectItem>();
                var dbo = dispatchToDA.Repository;
                var itemTmp = dbo.DispatchToes.Find(id);
                //Lấy các phòng ban nhận công văn
                lst = itemTmp.DispatchToDepartments
                     .Where(p => p.DispatchTo.ID == id)
                            .Select(p => new DispatchToObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.DepartmentID,
                                DispatchTo = id,
                                Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID) != null ? dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID).Name : string.Empty,
                                NoteMove = p.DispatchTo.NoteMove,
                                NoteVerify = p.Note,
                                IsVerify = p.IsVerify,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Department
                            })
                            .OrderByDescending(item => item.CreateAt)
                         .Page(page, pageSize, out totalCount)
                         .ToList();
                var arIDAtt = itemTmp.DispatchToEmployees
                    .Where(p => p.DispatchTo.ID == id)
                            .Select(p => new DispatchToObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.EmployeeID,
                                DispatchTo = id,
                                Name = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID) != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).Name : string.Empty,
                                Role = (dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID) != null && dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault() != null && dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole != null) ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty,
                                NoteMove = p.DispatchTo.NoteMove,
                                NoteVerify = p.Note,
                                IsVerify = p.IsVerify,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Employee
                            })
                            .OrderByDescending(item => item.CreateAt)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
                if (arIDAtt != null && arIDAtt.Count() > 0)
                {
                    foreach (var itm in arIDAtt)
                    {
                        lst.Add(itm);
                    }
                }
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        itm.UpdateName = userSV.GetNameByUserID(itm.UpdateBy);
                        itm.CreateName = userSV.GetNameByUserID(itm.CreateBy);
                    }
                }
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Lấy những phòng ban chưa xác nhận Công văn theo ID của bảng Công văn
        public List<DispatchToObjectItem> GetDeptVerifyByID(int id, List<int> idUserDept)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                var itemTmp = dbo.DispatchToes.Find(id);
                var arIDAtt = itemTmp.DispatchToDepartments
                            .Where(p => p.DispatchTo.ID == id && idUserDept.Contains(p.DepartmentID) && !p.IsVerify)
                            .Select(p => new DispatchToObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(m => m.ID == p.DepartmentID).Name
                            })
                            .ToList();
                return arIDAtt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Lấy thông tin số đến của cv
        public bool CheckNotExitNumberTo(string numberTo, int id = 0)
        {
            try
            {
                numberTo = numberTo.ToLower();
                if (id > 0)
                {
                    var objOld = dispatchToDA.GetById(id);
                    if (objOld != null && objOld.NumberTo.Trim().ToLower().Equals(numberTo))
                        return true;
                }
                var itm = dispatchToDA.GetQuery(p => p.NumberTo.ToLower().Equals(numberTo));
                if (itm.Count() > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Kiểm tra Phòng ban được Chuyển CV đã có trong DS đối tượng nhận công văn chưa
        public bool CheckExitDepatmentInDispatchTo(int id, int depID)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                var itemTmp = dbo.DispatchToes.Find(id);
                var arIDAtt = itemTmp.DispatchToDepartments
                            .Where(p => p.DepartmentID == depID)
                            .ToList();
                if (arIDAtt != null && arIDAtt.Count > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Kiểm tra Cá nhân được Chuyển CV đã có trong DS đối tượng nhận công văn chưa
        public bool CheckExitEmployeeInDispatchTo(int id, int empID)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                var itemTmp = dbo.DispatchToes.Find(id);
                var arIDAtt = itemTmp.DispatchToEmployees
                            .Where(p => p.EmployeeID == empID)
                            .ToList();
                if (arIDAtt != null && arIDAtt.Count > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteDepatment(int id)
        {
            try
            {
                var itm = dispatchToDeptDA.GetById(id);
                dispatchToDeptDA.Delete(itm);
                dispatchToDeptDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteEmployee(int id)
        {
            try
            {
                var itm = dispatchToEmployeeDA.GetById(id);
                dispatchToEmployeeDA.Delete(itm);
                dispatchToEmployeeDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteRange(List<object> ids)
        {
            throw new NotImplementedException();
        }
        private DispatchToItem convertToTypeItem(DispatchTo i, HumanEmployeeViewItem employee = null, List<int> userLogonDepts = null)
        {
            var dbo = dispatchToEmployeeDA.Repository;
            var obj = new DispatchToItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                NumberTo = i.NumberTo,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : "",
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : "",
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : "",
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : "",
                MethodID = (int)i.DispatchMethodID,
                Method = i.DispatchMethod.Name != null ? i.DispatchMethod.Name : "",
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                SendFrom = i.SendFrom,
                SendTo = i.SendTo,
                IsMoved = i.IsMoved,
                DateMoved = i.DateMoved,
                IsVerify = i.IsVerify,
                DateVerifyTime = i.DateVerifyTime,
                Note = i.Note,
                NoteMove = i.NoteMove,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy,
                AttachmentFileIDs = dbo.DispatchToAttachmentFiles.Where(t => t.DispatchToID == i.ID).Select(x => x.AttachmentFileID).ToList()
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                obj.FlagVerify = true;
                obj.FlagRequired = true;
                if (i.DispatchToDepartments != null && i.DispatchToDepartments.Count() > 0)
                {
                    obj.DepartmentID = i.DispatchToDepartments.ToList()[0].DepartmentID;
                    obj.IsVerify = i.DispatchToDepartments.ToList()[0].IsVerify;
                    obj.NoteVerify = i.DispatchToDepartments.ToList()[0].Note;
                    obj.SendBy = i.DispatchToDepartments.ToList()[0].SendBy.HasValue ? i.DispatchToDepartments.ToList()[0].SendBy.Value : 0;
                    //Gắn cờ check đã xác nhận
                    // obj.FlagVerify = true;
                    if (userLogonDepts != null)
                    {
                        foreach (var dep in i.DispatchToDepartments)
                        {
                            if (userLogonDepts.Contains(dep.DepartmentID) && !dep.IsVerify)
                            {
                                obj.FlagVerify = false;
                                break;
                            }
                        }
                    }
                }
                if (i.DispatchToEmployees != null && i.DispatchToEmployees.Count() > 0)
                {
                    obj.IsVerify = i.DispatchToEmployees.Where(t => t.EmployeeID == employee.ID).FirstOrDefault() != null ? i.DispatchToEmployees.Where(t => t.EmployeeID == employee.ID).FirstOrDefault().IsVerify : false;
                    obj.NoteVerify = i.DispatchToEmployees.Where(t => t.EmployeeID == employee.ID).FirstOrDefault() != null ? i.DispatchToEmployees.Where(t => t.EmployeeID == employee.ID).FirstOrDefault().Note : string.Empty;
                    obj.SendBy = i.DispatchToEmployees.Where(t => t.EmployeeID == employee.ID).FirstOrDefault() != null ? i.DispatchToEmployees.Where(t => t.EmployeeID == employee.ID).FirstOrDefault().SendBy.HasValue ? i.DispatchToEmployees.Where(t => t.EmployeeID == employee.ID).FirstOrDefault().SendBy.Value : 0 : 0;
                }
                if (employee != null)
                    obj.UserReceive = employee;
            }
            return obj;
        }
        private DispatchToItem convertToTypeItemForPerson(DispatchTo i, HumanEmployeeViewItem employee = null, List<int> userLogonDepts = null)
        {
            var dbo = dispatchToEmployeeDA.Repository;
            var obj = new DispatchToItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                NumberTo = i.NumberTo,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : "",
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : "",
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : "",
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : "",
                MethodID = (int)i.DispatchMethodID,
                Method = i.DispatchMethod.Name != null ? i.DispatchMethod.Name : "",
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                SendFrom = i.SendFrom,
                SendTo = i.SendTo,
                IsMoved = i.IsMoved,
                DateMoved = i.DateMoved,
                IsVerify = i.IsVerify,
                DateVerifyTime = i.DateVerifyTime,
                Note = i.Note,
                NoteMove = i.NoteMove,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                obj.FlagVerify = true;
                obj.FlagRequired = true;
                if (i.DispatchToEmployees != null && i.DispatchToEmployees.Count() > 0)
                {
                    if (userLogonDepts != null)
                    {
                        foreach (var dep in i.DispatchToDepartments)
                        {
                            if (userLogonDepts.Contains(dep.DepartmentID) && !dep.IsVerify)
                            {
                                obj.FlagVerify = false;
                                break;
                            }
                        }
                    }
                    obj.IsVerify = employee != null && i.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID && t.EmployeeID == employee.ID) != null ? i.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID && t.EmployeeID == employee.ID).IsVerify : false;
                    obj.NoteVerify = employee != null && i.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID && t.EmployeeID == employee.ID) != null ? i.DispatchToEmployees.FirstOrDefault(t => t.DispatchToID == obj.ID && t.EmployeeID == employee.ID).Note : string.Empty;
                    obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                }
                if (employee != null)
                    obj.UserReceive = employee;
            }
            return obj;
        }
        private DispatchToItem convertToTypeItemIndex(DispatchTo i, HumanEmployeeViewItem employee = null, List<int> userLogonDepts = null)
        {
            var dbo = dispatchToEmployeeDA.Repository;
            var obj = new DispatchToItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                NumberTo = i.NumberTo,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : "",
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : "",
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : "",
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : "",
                MethodID = (int)i.DispatchMethodID,
                Method = i.DispatchMethod.Name != null ? i.DispatchMethod.Name : "",
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                SendFrom = i.SendFrom,
                SendTo = i.SendTo,
                IsMoved = i.IsMoved,
                DateMoved = i.DateMoved,
                IsVerify = i.IsVerify,
                DateVerifyTime = i.DateVerifyTime,
                Note = i.Note,
                NoteMove = i.NoteMove,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy,
                AttachmentFileIDs = dbo.DispatchToAttachmentFiles.Where(t => t.DispatchToID == i.ID).Select(x => x.AttachmentFileID).ToList()
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                obj.FlagVerify = true;
                obj.FlagRequired = true;
                obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                if (i.DispatchToDepartments != null && i.DispatchToDepartments.Count() > 0)
                {
                    obj.DepartmentID = i.DispatchToDepartments.FirstOrDefault().DepartmentID;
                    obj.IsVerify = userLogonDepts != null && i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).IsVerify : false;
                    obj.NoteVerify = userLogonDepts != null && i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).Note : string.Empty;
                    if (userLogonDepts != null)
                    {
                        foreach (var dep in i.DispatchToDepartments)
                        {
                            if (userLogonDepts.Contains(dep.DepartmentID) && !dep.IsVerify)
                            {
                                obj.FlagVerify = false;
                                break;
                            }

                        }

                    }
                    obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                }
                if (employee != null)
                    obj.UserReceive = employee;
            }
            return obj;
        }
        private string getStatus(bool isMove, bool isConfirm)
        {
            if (isMove && !isConfirm)
                return "Đã chuyển";
            else if (isMove && isConfirm)
                return "Đã xác nhận";
            return "Mới";
        }
        public List<TaskItem> GetAllTaskByID(ModelFilter filter, int dispatchToID)
        {
            var dbo = dispatchToDA.Repository;
            List<TaskItem> lst = new List<TaskItem>();
            var task1s = dbo.Tasks.Join(dbo.DispatchToTasks, t => t.ID, dt => dt.TaskID, (t, dt) => new
                TaskItem
            {
                ID = t.ID,
                Name = t.Name,
                CategoryID = t.TaskCategoryID,
                CategoryName = t.TaskCategory.Name,
                IDRef = dt.DispatchTo.ID,
                Content = t.Content,
                IsComplete = t.IsComplete,
                CreateBy = t.CreateBy,
                LevelColor = t.TaskLevel.Color,
                IsNew = t.IsNew,
                IsEdit = t.IsEdit,
                IsPrivate = t.IsPrivate,
                StartTime = t.StartTime,
                UpdateAt = t.UpdateAt,
                UpdateBy = t.UpdateBy,
                CreateAt = t.CreateAt,
                LevelID = t.TaskLevelID,
                EndTime = t.EndTime,
                IsPause = t.IsPause,
                IsPass = t.IsPass,
                LevelName = t.TaskLevel.Name,
                Rate = t.Rate
            })
                .Where(T => T.IDRef == dispatchToID)
                .Filter(filter)
                .ToList();
            return task1s;
        }
        public void InsertTask(int taskID, int dispatchID, int useID, string note)
        {
            DispatchToTaskDA dispatchToTaskDA = new DA.DispatchToTaskDA();
            var itm = new DispatchToTask
            {
                TaskID = taskID,
                DispatchToID = dispatchID,
                CreateAt = DateTime.Now,
                CreateBy = useID,
                Note = note
            };
            dispatchToTaskDA.Insert(itm);
            dispatchToTaskDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            dispatchToDA.DeleteRange(ids);
            dispatchToDA.Save();
        }
        /// <summary>
        /// Dung cho thong ke
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private DispatchToItem convertToItemFromStaistical(DispatchTo i, List<int> userLogonDepts = null)
        {
            var dbo = dispatchToEmployeeDA.Repository;
            var obj = new DispatchToItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                NumberTo = i.NumberTo,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : "",
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : "",
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : "",
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : "",
                MethodID = (int)i.DispatchMethodID,
                Method = i.DispatchMethod.Name != null ? i.DispatchMethod.Name : "",
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                SendFrom = i.SendFrom,
                SendTo = i.SendTo,
                IsMoved = i.IsMoved,
                DateMoved = i.DateMoved,
                IsVerify = i.IsVerify,
                DateVerifyTime = i.DateVerifyTime,
                Note = i.Note,
                NoteMove = i.NoteMove,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                obj.FlagVerify = true;
                obj.FlagRequired = true;
                obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                if (i.DispatchToDepartments != null && i.DispatchToDepartments.Count() > 0)
                {
                    obj.DepartmentID = i.DispatchToDepartments.FirstOrDefault().DepartmentID;
                    obj.IsVerify = userLogonDepts != null && i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).IsVerify : false;
                    obj.NoteVerify = userLogonDepts != null && i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchToDepartments.FirstOrDefault(t => t.DispatchToID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).Note : string.Empty;
                    if (userLogonDepts != null)
                    {
                        foreach (var dep in i.DispatchToDepartments)
                        {
                            if (userLogonDepts.Contains(dep.DepartmentID) && !dep.IsVerify)
                            {
                                obj.FlagVerify = false;
                                break;
                            }

                        }

                    }
                    obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                }
            }
            return obj;
        }
        public List<DispatchToItem> GetTotalDispatchToFromDepartment(ModelFilter filter, int departmentId, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var lst = dbo.DispatchToDepartments
                        .Where(t => t.DepartmentID == departmentId)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Distinct()
                        .Filter(filter)
                        .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToItemFromStaistical(itm, new List<int> { departmentId }));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private DispatchToItem convertToItemFromParentStatistical(DispatchTo i)
        {
            var dbo = dispatchToEmployeeDA.Repository;
            var obj = new DispatchToItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                NumberTo = i.NumberTo,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : "",
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : "",
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : "",
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : "",
                MethodID = (int)i.DispatchMethodID,
                Method = i.DispatchMethod.Name != null ? i.DispatchMethod.Name : "",
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                SendFrom = i.SendFrom,
                SendTo = i.SendTo,
                IsMoved = i.IsMoved,
                DateMoved = i.DateMoved,
                IsVerify = i.IsVerify,
                DateVerifyTime = i.DateVerifyTime,
                Note = i.Note,
                NoteMove = i.NoteMove,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                if (i.DispatchToDepartments != null && i.DispatchToDepartments.Count() > 0)
                {
                    obj.DepartmentID = i.DispatchToDepartments.FirstOrDefault().DepartmentID;
                    obj.IsVerify = i.DispatchToDepartments.Where(t => t.DispatchToID == obj.ID && t.IsVerify) != null ? true : false;
                    obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                }
            }
            return obj;
        }
        public List<DispatchToItem> GetTotalDispatchToFromParentDepartment(ModelFilter filter, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var lst = dbo.DispatchToDepartments
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Distinct()
                        .Filter(filter)
                        .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToItemFromParentStatistical(itm));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DispatchToItem> GetTotalDispatchToVerifyFromDepartment(ModelFilter filter, int departmentId, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var lst = dbo.DispatchToDepartments
                        .Where(t => t.DepartmentID == departmentId)
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                        .Filter(filter)
                        .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToTypeItemIndex(itm, null, new List<int> { departmentId }));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DispatchToItem> GetTotalDispatchToVerifyFromParentDepartment(ModelFilter filter, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var lst = dbo.DispatchToDepartments
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Distinct()
                        .Filter(filter)
                        .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToItemFromParentStatistical(itm));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DispatchToItem> GetTotalDispatchToFromPerson(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                HumanEmployeeViewItem employee = new HumanEmployeeViewItem()
                {
                    ID = employeeId,
                    Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId) != null ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId).Name : string.Empty,
                    Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.Name : string.Empty,
                    Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.HumanDepartment.Name : string.Empty,
                };
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var lst = dbo.DispatchToEmployees
                        .Where(t => t.EmployeeID == employeeId)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Filter(filter)
                        .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToTypeItem(itm, employee));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private DispatchToItem convertToItemFromPerson(DispatchTo i, HumanEmployeeViewItem employee = null)
        {
            var dbo = dispatchToEmployeeDA.Repository;
            var obj = new DispatchToItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                NumberTo = i.NumberTo,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : "",
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : "",
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : "",
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : "",
                MethodID = (int)i.DispatchMethodID,
                Method = i.DispatchMethod.Name != null ? i.DispatchMethod.Name : "",
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                SendFrom = i.SendFrom,
                SendTo = i.SendTo,
                IsMoved = i.IsMoved,
                DateMoved = i.DateMoved,
                IsVerify = i.IsVerify,
                DateVerifyTime = i.DateVerifyTime,
                Note = i.Note,
                NoteMove = i.NoteMove,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                obj.FlagVerify = true;
                obj.FlagRequired = true;
                obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                if (i.DispatchToEmployees != null && i.DispatchToEmployees.Count() > 0)
                {
                    obj.IsVerify = i.DispatchToEmployees.Where(t => t.DispatchToID == obj.ID && t.EmployeeID == employee.ID && t.IsVerify) != null ? true : false;
                    obj.Status = getStatus(obj.IsMoved, obj.IsVerify);
                }
                if (employee != null)
                    obj.UserReceive = employee;
            }
            return obj;
        }
        public List<DispatchToItem> GetTotalDispatchToVerifyFromPerson(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var dbo = dispatchToDA.Repository;
                HumanEmployeeViewItem employee = new HumanEmployeeViewItem()
                {
                    ID = employeeId,
                    Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId) != null ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId).Name : string.Empty,
                    Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.Name : string.Empty,
                    Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.HumanDepartment.Name : string.Empty,
                };
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var lst = dbo.DispatchToEmployees
                        .Where(t => t.EmployeeID == employeeId)
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Filter(filter)
                        .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToItemFromPerson(itm, employee));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DispatchToItem> GetCC(ModelFilter filter, int employeeId, int focusId = 0)
        {
            try
            {
                List<DispatchToItem> lstObj = new List<DispatchToItem>();
                var query = dispatchToDA.GetQuery(t => t.DispatchToCCs.Any(tx => tx.HumanEmployeeID == employeeId));
                if (focusId != 0)
                {
                    filter.SortName = null;
                    query = query.OrderBy(i => i.ID == focusId ? false : true);
                }
                var lst = query
                    .Filter(filter)
                    .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        lstObj.Add(convertToTypeItemIndex(itm, null, null));
                    }
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<HumanEmployeeItem> GetEmployeeCC(ModelFilter filter, int dispatchToId)
        {
            var dbo = dispatchToDA.Repository;
            var data = dbo.DispatchToCCs.Where(t => t.DispatchToID == dispatchToId)
                        .Select(item => new HumanEmployeeItem()
                        {
                            ID = item.ID,
                            FileID = item.HumanEmployee.AttachmentFileID,
                            FileName = item.HumanEmployee.AttachmentFile.Name,
                            Role = item.HumanEmployee.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.Name).FirstOrDefault(),
                            DepartmentName = item.HumanEmployee.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID)
                                                    .Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            Code = item.HumanEmployee.Code,
                            Name = item.HumanEmployee.Name,
                            Email = item.HumanEmployee.Email,
                            Phone = item.HumanEmployee.Phone,
                            Address = item.HumanEmployee.Address,
                            Birthday = item.HumanEmployee.Birthday,
                            Gender = item.HumanEmployee.Gender,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            lstHumanGrPos = item.HumanEmployee.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID
                                && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                ).Select(i => new HumanGroupPositionItem()
                                {
                                    ID = i.ID,
                                    Name = i.HumanRole.Name,
                                    GroupName = i.HumanRole.HumanDepartment.Name
                                }).ToList()
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return data;
        }
        public void AddEmployeeCC(int dispatchToId, int employeeId, int userId)
        {
            var dbo = dispatchToDA.Repository;
            var obj = dbo.DispatchToCCs.Where(t => t.DispatchToID == dispatchToId && t.HumanEmployeeID == employeeId).FirstOrDefault();
            if (obj == null && employeeId != 0)
            {
                var cc = new DispatchToCC();
                cc.HumanEmployeeID = employeeId;
                cc.DispatchToID = dispatchToId;
                cc.CreateAt = DateTime.Now;
                cc.CreateBy = userId;
                dbo.DispatchToCCs.Add(cc);
                dbo.SaveChanges();
            }
        }
        public void DeleteCC(string stringId)
        {
            var dbo = dispatchToDA.Repository;
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            if (ids.Count > 0)
                foreach (var item in ids)
                {
                    dbo.DispatchToCCs.Remove(dbo.DispatchToCCs.Where(t => t.ID == (int)item).FirstOrDefault());
                    dbo.SaveChanges();
                }

        }
    }
}
