using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;
namespace iDAS.Services
{
    public class DispatchGoSV
    {
        private DispatchGoDA dispatchGoDA = new DispatchGoDA();
        private DispatchGoDepartmentDA dispatchGoDepartmentDA = new DispatchGoDepartmentDA();
        private DispatchGoEmployeeDA dispatchGoEmployeeDA = new DispatchGoEmployeeDA();
        private DispatchGoOutSideDA dispatchGoOutSideDA = new DispatchGoOutSideDA();
        private HumanUserSV userSV = new HumanUserSV();
        private HumanEmployeeSV employSV = new HumanEmployeeSV();
        public List<DispatchGoItem> GetAll(ModelFilter filter, List<int> userDepIDs = null, int userId = 0, int focusId = 0)
        {
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var query = dispatchGoDA.GetQuery(p => p.Type == true);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var lstItm = query.Filter(filter).ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItem(itm, null, userDepIDs, userId));
                }
            }
            return lst;
        }
        public List<DispatchGoItem> GetAllDispatchInSide(ModelFilter fillter, List<int> userDepIDs = null, int userEmloyeeID = 0, int focusId = 0)
        {
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var dbo = dispatchGoDA.Repository;
            var query = dispatchGoDA
                .GetQuery()
                .OrderByDescending(item => item.MoveAt)
                .Where(item => (item.DispatchGoDepartments
                    .Where(t => userDepIDs.Contains(t.DepartmentID))
                    .ToList().Count > 0) || (item.DispatchGoEmployees
                    .Where(t => t.EmployeeID == userEmloyeeID)
                    .ToList()
                    .Count() > 0)
                    );
            if (focusId != 0)
            {
                fillter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var lstID1 = query
            .Filter(fillter)
            .ToList();
            if (lstID1 != null && lstID1.Count() > 0)
            {
                foreach (var itm in lstID1)
                {
                    var employee = employSV.GetEmployeeView(userEmloyeeID);
                    lst.Add(convertToDispatchGoItem(itm, employee, userDepIDs, userEmloyeeID));
                }
            }
            return lst;
        }
        public DispatchGoItem GetByID(int id)
        {
            var dbo = dispatchGoDA.Repository;
            var obj = dispatchGoDA.GetById(id);
            DispatchGoItem objItem = new DispatchGoItem();
            if (obj != null)
            {
                var employeeSV = new HumanEmployeeSV();
                objItem = convertToDispatchGoItem(obj);
                objItem.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.DispatchGoAttachmentFiles.Where(z => z.DispatchGoID == objItem.ID)
                        .Select(d => d.AttachmentFileID).ToList()
                };
                objItem.Create = employeeSV.GetEmployeeView(dbo.HumanUsers.FirstOrDefault(t => t.ID == obj.CreateBy).HumanEmployeeID);
            }
            return objItem;
        }
        public DispatchGoItem GetObjMoveOutByID(int id)
        {
            var dbo = dispatchGoDA.Repository;
            var obj = dispatchGoDA.GetById(id);
            if (obj != null)
            {
                var itm = convertToDispatchGoItem(obj);
                var objR = dispatchGoOutSideDA.GetQuery(p => p.DispatchGoID == id);
                if (objR.Count() > 0)
                {
                    foreach (var item in objR.ToList())
                    {
                        if (!item.Type)
                        {
                            itm.ToName = item.Name;
                            itm.ToPosition = item.Position;
                            itm.DisparchGoOutID = item.ID;
                            itm.ToCompany = item.Company;
                            itm.Address = item.Address;
                            itm.MoveAt = item.Date;
                            itm.StrType = DispatchObjectType.Employee.ToString();
                            itm.NoteMove = item.Note;
                            itm.IsVerify = item.IsVerify;
                            break;
                        }
                    }
                }
                itm.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.DispatchGoAttachmentFiles.Where(z => z.DispatchGoID == itm.ID)
                        .Select(d => d.AttachmentFileID).ToList()
                };
                return itm;
            }
            return null;
        }
        public List<DispatchToObjectItem> GetDepartmentByID(int id)
        {
            try
            {
                var dbo = dispatchGoDA.Repository;
                var itemTmp = dbo.DispatchGoes.Find(id);
                var arIDAtt = itemTmp.DispatchGoDepartments
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p => new DispatchToObjectItem { ID = p.ID, ObjectID = p.DepartmentID, DispatchTo = id, Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID).Name, NoteMove = p.Note, IsVerify = p.IsVerify })
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
                var dbo = dispatchGoDA.Repository;
                var itemTmp = dbo.DispatchGoes.Find(id);
                var arIDAtt = itemTmp.DispatchGoEmployees
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p => new DispatchToObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.EmployeeID,
                                DispatchTo = id,
                                Name = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).Name,
                                Role = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault() == null ?
                                       "" : dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
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
        public DispatchGoObjectItem GetMoveOutByDispatchGoID(int id, string type)
        {
            try
            {
                var ck = false;
                if (type.Equals(DispatchObjectType.Department.ToString()))
                    ck = true;
                var dbo = dispatchGoDA.Repository;
                var itemTmp = dbo.DispatchGoes.Find(id);
                var obj = itemTmp.DispatchGoOutSides
                            .Where(p => p.DispatchGo.ID == id && p.Type == ck)
                            .Select(p => new DispatchGoObjectItem { ID = p.ID, DispatchGo = id, Name = p.Name, Position = p.Position, Company = p.Company, Address = p.Address, NoteMove = p.Note, IsVerify = p.IsVerify, Date = p.Date })
                            .FirstOrDefault();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DispatchGoObjectItem GetMoveOutByID(int id)
        {
            try
            {
                var p = dispatchGoOutSideDA.GetById(id);
                if (p != null)
                {
                    var obj = new DispatchGoObjectItem
                    {
                        ID = p.ID,
                        DispatchGo = p.DispatchGoID,
                        Type = p.Type == true ? (int)DispatchObjectType.Department : (int)DispatchObjectType.Employee,
                        Name = p.Name,
                        Position = p.Position,
                        Company = p.Company,
                        Address = p.Address,
                        TelCompany = p.Telephone,
                        TelPerson = p.Phone,
                        EmailCompany = p.CompanyEmail,
                        EmailPerson = p.PersonEmail,
                        NoteMove = p.Note,
                        NoteVerify = p.NoteVerify,
                        IsVerify = p.IsVerify,
                        Date = p.Date,
                        CreateAt = p.CreateAt,
                        CreateBy = p.CreateBy,
                        UpdateAt = p.UpdateAt,
                        UpdateBy = p.UpdateBy
                    };
                    return obj;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DispatchGoObjectItem GetVerifyDetailByID(int id, int dispatchId, int type)
        {
            try
            {
                var dbo = dispatchGoOutSideDA.Repository;
                if (dispatchId == (int)DispatchGoType.OutSide)
                {
                    var p = dispatchGoOutSideDA.GetById(id);
                    if (p != null)
                    {
                        var obj = new DispatchGoObjectItem
                        {
                            ID = p.ID,
                            DispatchGo = p.DispatchGoID,
                            Type = p.Type == true ? (int)DispatchObjectType.Department : (int)DispatchObjectType.Employee,
                            Name = p.Name,
                            Position = p.Position,
                            Company = p.Company,
                            Address = p.Address,
                            TelCompany = p.Telephone,
                            TelPerson = p.Phone,
                            EmailCompany = p.CompanyEmail,
                            EmailPerson = p.PersonEmail,
                            NoteMove = p.Note,
                            NoteVerify = p.NoteVerify,
                            IsVerify = p.IsVerify,
                            Date = p.Date,
                            DateVerify = p.DateVerify,
                            CreateAt = p.CreateAt,
                            CreateBy = p.CreateBy,
                            UpdateAt = p.UpdateAt,
                            UpdateBy = p.UpdateBy
                        };
                        if (obj.IsVerify)
                            obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);
                        obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                        return obj;
                    }
                }
                else if (type == (int)DispatchObjectType.Department)
                {
                    var p = dispatchGoDepartmentDA.GetById(id);
                    if (p != null)
                    {
                        var obj = new DispatchGoObjectItem
                        {
                            ID = p.ID,
                            DispatchGo = p.DispatchGoID,
                            Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID).Name,
                            NoteMove = p.DispatchGo.NoteMove,
                            NoteVerify = p.Note,
                            IsVerify = p.IsVerify,
                            DateVerify = p.VerifyAt,
                            CreateAt = p.CreateAt,
                            CreateBy = p.CreateBy,
                            UpdateAt = p.UpdateAt,
                            UpdateBy = p.UpdateBy
                        };
                        if (obj.IsVerify)
                            obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);
                        obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                        return obj;
                    }
                }
                else if (type == (int)DispatchObjectType.Employee)
                {
                    var p = dispatchGoEmployeeDA.GetById(id);
                    if (p != null)
                    {
                        var obj = new DispatchGoObjectItem
                        {
                            ID = p.ID,
                            DispatchGo = p.DispatchGoID,
                            Name = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).Name,
                            Position = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.Count() > 0 ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name : "",
                            NoteMove = p.DispatchGo.NoteMove,
                            NoteVerify = p.Note,
                            IsVerify = p.IsVerify,
                            DateVerify = p.VerifyAt,
                            CreateAt = p.CreateAt,
                            CreateBy = p.CreateBy,
                            UpdateAt = p.UpdateAt,
                            UpdateBy = p.UpdateBy
                        };
                        if (obj.IsVerify)
                            obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);
                        obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                        return obj;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DispatchGoItem> GetAllByUserLogOn(ModelFilter fillter, int id, int employeeID, int focusId = 0)
        {
            try
            {
                List<DispatchGoItem> lstObj = new List<DispatchGoItem>();
                var dbo = dispatchGoDA.Repository;
                var query = dbo.DispatchGoes
                .Where(de => ((de.CreateBy == id && de.Type == false) || (!de.IsEdit && de.ApprovalBy == employeeID) || de.DispatchGoEmployees.Any(t => t.EmployeeID == employeeID && t.DispatchGoID == de.ID)));
                if (focusId != 0)
                {
                    fillter.SortName = null;
                    query = query.OrderBy(i => i.ID == focusId ? false : true);
                }
                var lst = query
                .Filter(fillter)
                .ToList();
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        itm.IsVerify = itm.IsVerify;
                        var employee = employSV.GetEmployeeView(employeeID);
                        lstObj.Add(convertToDispatchGoItem(itm, employee));
                    }
                }
                if (focusId != 0)
                {
                    lstObj = lstObj.OrderBy(i => i.ID == focusId ? false : true).ToList();
                }
                return lstObj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DispatchGoItem GetVerifyInfoByID(int id, int empID, bool isEmployee = false)
        {
            var dbo = dispatchGoDA.Repository;
            var obj = dispatchGoDA.GetById(id);
            DispatchGoItem obi = new DispatchGoItem();
            if (obj != null)
            {
                var employeeSV = new HumanEmployeeSV();
                if (!isEmployee)
                {
                    var lst = dispatchGoDepartmentDA.GetQuery(p => p.DispatchGo.ID == id).ToList();
                    obj.DispatchGoDepartments = lst;
                    var employee = employSV.GetEmployeeView(empID);
                    obi = convertToDispatchGoItem(obj, employee);
                }
                else
                {
                    var lst = dispatchGoEmployeeDA.GetQuery(p => p.DispatchGo.ID == id).ToList();
                    obj.DispatchGoEmployees = lst;
                    var employee = employSV.GetEmployeeView(empID);
                    obi = convertToDispatchGoItem(obj, employee);
                }
                obi.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.DispatchGoAttachmentFiles.Where(z => z.DispatchGoID == obj.ID)
                        .Select(d => d.AttachmentFileID).ToList()
                };
                obi.Create = employeeSV.GetEmployeeView(dbo.HumanUsers.FirstOrDefault(t => t.ID == obj.CreateBy).HumanEmployeeID);
            }
            return obi;
        }
        public DispatchGoObjectItem GetObjectVerifyByID(int id, int empID, bool isEmployee = false)
        {
            if (!isEmployee)
            {
                var obj = dispatchGoDepartmentDA.GetById(id);
                var employee = employSV.GetEmployeeView(empID);
                var objN = new DispatchGoObjectItem
                {
                    ID = obj.ID,
                    DispatchGo = obj.DispatchGoID,
                    ObjectID = obj.DepartmentID,
                    UserReceive = employee,
                    NoteMove = obj.DispatchGo.NoteMove,
                    NoteVerify = obj.Note,
                    CreateAt = obj.CreateAt,
                    CreateBy = obj.CreateBy,
                    UpdateBy = obj.UpdateBy,
                    SendBy = obj.SendBy.HasValue ? obj.SendBy.Value : 0,
                    DateVerify = obj.VerifyAt,
                    Type = (int)DispatchObjectType.Department,
                    IsVerify = obj.IsVerify
                };
                return objN;
            }
            else
            {
                var obj = dispatchGoEmployeeDA.GetById(id);
                var employee = employSV.GetEmployeeView(empID);
                var objN = new DispatchGoObjectItem
                {
                    ID = obj.ID,
                    DispatchGo = obj.DispatchGoID,
                    ObjectID = obj.EmployeeID,
                    UserReceive = employee,
                    NoteMove = obj.DispatchGo.NoteMove,
                    NoteVerify = obj.Note,
                    Type = (int)DispatchObjectType.Employee,
                    CreateAt = obj.CreateAt,
                    CreateBy = obj.CreateBy,
                    UpdateBy = obj.UpdateBy,
                    SendBy = obj.SendBy.HasValue ? obj.SendBy.Value : 0,
                    DateVerify = obj.VerifyAt,
                    IsVerify = obj.IsVerify
                };
                return objN;
            }
        }
        public List<DispatchGoObjectItem> GetAllObjectVerifyByID(int page, int pageSize, out int totalCount, int id)
        {
            try
            {
                List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
                var dbo = dispatchGoDA.Repository;
                var itemTmp = dbo.DispatchGoes.Find(id);
                lst = itemTmp.DispatchGoDepartments
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p => new DispatchGoObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.DepartmentID,
                                DispatchGo = id,
                                Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID).Name,
                                NoteVerify = p.Note,
                                NoteMove = p.DispatchGo.NoteMove,
                                IsVerify = p.IsVerify,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Department,
                                DispatchGoType = (int)DispatchGoType.InSide,
                                DateVerify = p.VerifyAt
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
                var arIDAtt = itemTmp.DispatchGoEmployees
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p => new DispatchGoObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.EmployeeID,
                                DispatchGo = id,
                                Name = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).Name,
                                Position = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.Count() > 0 ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name : "",
                                NoteVerify = p.Note,
                                NoteMove = p.DispatchGo.NoteMove,
                                IsVerify = p.IsVerify,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Employee,
                                DispatchGoType = (int)DispatchGoType.InSide,
                                DateVerify = p.VerifyAt
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
                var arOut = itemTmp.DispatchGoOutSides
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p => new DispatchGoObjectItem
                            {
                                ID = p.ID,
                                DispatchGo = id,
                                Name = p.Type == true && (p.Name == null || p.Name.Equals("")) ? p.Company : p.Name,
                                Position = p.Position,
                                NoteMove = p.DispatchGo.Note,
                                IsVerify = p.IsVerify,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = p.Type == true ? (int)DispatchObjectType.Department : (int)DispatchObjectType.Employee,
                                DispatchGoType = (int)DispatchGoType.OutSide,
                                DateVerify = p.DateVerify,
                                NoteVerify = p.NoteVerify
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
                if (arOut != null && arOut.Count() > 0)
                {
                    foreach (var itm in arOut)
                    {
                        lst.Add(itm);
                    }
                }
                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        if (itm.IsVerify)
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
        public List<DispatchGoObjectItem> GetAllObjectVerifyInByID(int id, List<int> userDeptIDs, int userID)
        {
            try
            {
                List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
                var dbo = dispatchGoDA.Repository;
                var itemTmp = dbo.DispatchGoes.Find(id);
                lst = itemTmp.DispatchGoDepartments
                            .Where(p => p.DispatchGo.ID == id && userDeptIDs.Contains(p.DepartmentID))
                            .Select(p => new DispatchGoObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.DepartmentID,
                                DispatchGo = p.DispatchGoID,
                                Company = dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID).Name,
                                Name = p.IsVerify ? dbo.HumanUsers.FirstOrDefault(t => t.ID == p.CreateBy).HumanEmployee.Name : string.Empty,
                                NoteMove = p.Note,
                                NoteVerify = p.Note,
                                DateVerify = p.VerifyAt,
                                IsVerify = p.IsVerify,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Department
                            })
                            .ToList();
                var arIDAtt = itemTmp.DispatchGoEmployees
                            .Where(p => p.DispatchGo.ID == id && p.EmployeeID == userID)
                            .Select(p => new DispatchGoObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.EmployeeID,
                                DispatchGo = p.DispatchGoID,
                                Name = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).Name,
                                Position = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault() == null ?
                                       "" : dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                NoteMove = p.Note,
                                IsVerify = p.IsVerify,
                                DateVerify = p.VerifyAt,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Employee,
                                NoteVerify = p.Note
                            })
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
        public List<DispatchGoObjectItem> GetAllObjectMoveOutByID(int id)
        {
            try
            {
                List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
                var dbo = dispatchGoDA.Repository;
                var itemTmp = dbo.DispatchGoes.Find(id);
                lst = itemTmp.DispatchGoOutSides
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p =>
                                new DispatchGoObjectItem
                                {
                                    ID = p.ID,
                                    DispatchGo = id,
                                    Name = p.Name,
                                    Position = p.Position,
                                    Company = p.Company,
                                    EmailCompany = p.CompanyEmail,
                                    TelCompany = p.Telephone,
                                    EmailPerson = p.PersonEmail,
                                    TelPerson = p.Phone,
                                    Address = p.Address,
                                    NoteMove = p.Note,
                                    IsVerify = p.IsVerify,
                                    UpdateAt = p.UpdateAt,
                                    UpdateBy = p.UpdateBy,
                                    CreateAt = p.CreateAt,
                                    CreateBy = p.CreateBy,
                                    Date = p.Date,
                                    TypeCode = p.Type == true ? DispatchObjectType.Department.ToString() : DispatchObjectType.Employee.ToString(),
                                    Type = p.Type == true ? (int)DispatchObjectType.Department : (int)DispatchObjectType.Employee,
                                    StrType = p.Type == true ? "Tổ chức" : "Cá nhân",
                                })
                            .ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Insert(DispatchGoItem obj)
        {
            try
            {
                if (obj.Type == true)
                {
                    obj.AlowNotApproval = false;
                };
                if (obj.AlowNotApproval == true && !obj.IsEdit)
                {
                    obj.IsApproval = true;
                }
                var itm = new DispatchGo
                {
                    Name = obj.Name.Trim(),
                    Code = obj.Code,
                    Compendia = obj.Compendia,
                    ApprovalBy = obj.ApprovalBy.HasValue ? obj.ApprovalBy : obj.EmployeeInfo.ID,
                    DispatchSecurityID = obj.SecurityLevelID,
                    DispatchUrgencyID = obj.UrgencyId,
                    DispatchMethodID = obj.MethodID,
                    FormHard = obj.FormH,
                    FormSoft = obj.FormS,
                    Date = obj.Date,
                    DispatchCategoryID = obj.CategoryID,
                    DestinationAddress = obj.DestinationAddress.Trim(),
                    Type = obj.Type,
                    IsEdit = obj.IsEdit,
                    IsApproval = obj.IsApproval,
                    IsAccept = (obj.AlowNotApproval == true && !obj.IsEdit) ? true : false,
                    IsMove = false,
                    IsVerify = false,
                    Note = obj.Note,
                    AlowNotAproval = obj.AlowNotApproval,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy
                };
                dispatchGoDA.Insert(itm);
                dispatchGoDA.Save();
                new FileSV().Upload<DispatchGoAttachmentFile>(obj.AttachmentFiles, itm.ID);
                return itm.ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(DispatchGoItem obj)
        {
            var itm = dispatchGoDA.GetById(obj.ID);
            if (itm.Type == true)
            {
                obj.AlowNotApproval = false;
            };
            if (obj.AlowNotApproval == true && !obj.IsEdit)
            {
                itm.IsApproval = true;
                itm.IsAccept = true;
            }
            if (!itm.AlowNotAproval && itm.IsApproval && !itm.IsAccept && itm.IsEdit && !obj.IsEdit)
            {
                itm.IsApproval = false;
            }
            itm.Name = obj.Name.Trim();
            itm.Note = obj.Note;
            itm.Code = obj.Code;
            itm.ApprovalBy = obj.Approval != null ? obj.Approval.ID : obj.EmployeeInfo.ID;
            itm.Compendia = obj.Compendia;
            itm.DispatchSecurityID = obj.SecurityLevelID;
            itm.DispatchUrgencyID = obj.UrgencyId;
            itm.DispatchMethodID = obj.MethodID;
            itm.FormHard = obj.FormH;
            itm.FormSoft = obj.FormS;
            itm.Date = obj.Date;
            itm.DispatchCategoryID = obj.CategoryID;
            itm.DestinationAddress = obj.DestinationAddress.Trim();
            itm.IsEdit = obj.IsEdit;
            itm.Note = obj.Note;
            itm.AlowNotAproval = obj.AlowNotApproval;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            dispatchGoDA.Update(itm);
            dispatchGoDA.Save();
            new FileSV().Upload<DispatchGoAttachmentFile>(obj.AttachmentFiles, itm.ID);
        }
        public void UpdateApprove(DispatchGoItem obj)
        {
            var itm = dispatchGoDA.GetById(obj.ID);
            itm.Date = obj.Date;
            itm.IsApproval = true;
            itm.IsEdit = obj.IsEdit;
            itm.ApprovalNote = obj.ApprovalNote;
            itm.ApprovalAt = DateTime.Now;
            itm.IsAccept = obj.IsAccept;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            dispatchGoDA.Update(itm);
            dispatchGoDA.Save();
        }
        public void UpdateMove(DispatchGoItem obj, List<int> lst, int userId)
        {
            try
            {
                var itm = dispatchGoDA.GetById(obj.ID);
                itm.IsMove = true;
                itm.MoveAt = DateTime.Now;
                itm.IsVerify = false;
                itm.NoteMove = obj.NoteMove;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                dispatchGoDA.Update(itm);
                dispatchGoDA.Save();
                if (obj.StrType.Equals(DispatchObjectType.Department.ToString()))
                    UpdateDepartment(obj, lst, userId);
                else if (obj.StrType.Equals(DispatchObjectType.Employee.ToString()))
                    UpdateEmployee(obj, lst, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateMoveOut(DispatchGoObjectItem obj)
        {
            try
            {
                if (obj.ID > 0)
                {
                    var itmOut = dispatchGoOutSideDA.GetById(obj.ID);
                    if (obj.Type == (int)DispatchObjectType.Department)
                    {
                        itmOut.Telephone = obj.TelCompany;
                        itmOut.CompanyEmail = obj.EmailCompany;
                        itmOut.Type = true;
                    }
                    else
                    {
                        itmOut.Type = false;
                        itmOut.Position = obj.Position;
                    }
                    itmOut.Name = obj.Name != null ? obj.Name.Trim() : "";
                    itmOut.Company = obj.Company != null ? obj.Company.Trim() : "";
                    itmOut.Telephone = obj.TelCompany;
                    itmOut.CompanyEmail = obj.EmailCompany;
                    itmOut.PersonEmail = obj.EmailPerson;
                    itmOut.Phone = obj.TelPerson;
                    itmOut.Address = obj.Address != null ? obj.Address.Trim() : "";
                    itmOut.IsVerify = false;
                    itmOut.Note = obj.NoteMove;
                    itmOut.Date = obj.Date;
                    itmOut.UpdateAt = DateTime.Now;
                    itmOut.UpdateBy = obj.UpdateBy;
                    dispatchGoOutSideDA.Update(itmOut);
                    dispatchGoOutSideDA.Save();
                }
                else
                {
                    var itmOut = new DispatchGoOutSide
                    {
                        DispatchGoID = obj.DispatchGo,
                        Name = obj.Name != null ? obj.Name.Trim() : "",
                        Company = obj.Company != null ? obj.Company.Trim() : "",
                        Telephone = obj.TelCompany,
                        CompanyEmail = obj.EmailCompany,
                        PersonEmail = obj.EmailPerson,
                        Phone = obj.TelPerson,
                        IsVerify = false,
                        Note = obj.NoteMove,
                        Date = obj.Date,
                        Address = obj.Address != null ? obj.Address.Trim() : "",
                        CreateAt = DateTime.Now,
                        CreateBy = obj.UpdateBy
                    };
                    if (obj.Type == (int)DispatchObjectType.Department)
                    {
                        itmOut.Telephone = obj.TelCompany;
                        itmOut.CompanyEmail = obj.EmailCompany;
                        itmOut.Type = true;
                    }
                    else
                    {
                        itmOut.Type = false;
                        itmOut.Position = obj.Position;
                    }
                    dispatchGoOutSideDA.Insert(itmOut);
                }
                dispatchGoOutSideDA.Save();
                var itm = dispatchGoDA.GetById(obj.DispatchGo);
                itm.IsMove = true;
                itm.MoveAt = DateTime.Now;
                itm.IsVerify = false;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                dispatchGoDA.Update(itm);
                dispatchGoDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateDepartment(DispatchGoItem obj, List<int> lstDept, int userId)
        {
            try
            {
                var arIDDeptIn = dispatchGoDepartmentDA.GetQuery(p => p.DispatchGoID == obj.ID).Select(p => p.DepartmentID).ToArray();
                if (arIDDeptIn != null && arIDDeptIn.Count() > 0)
                {
                    foreach (var itm in lstDept)
                    {
                        if (!arIDDeptIn.Contains(itm))
                        {
                            var item = new DispatchGoDepartment
                            {
                                DispatchGoID = obj.ID,
                                DepartmentID = itm,
                                IsVerify = false,
                                SendBy = userId,
                                CreateAt = DateTime.Now,
                                CreateBy = obj.UpdateBy
                            };
                            dispatchGoDepartmentDA.Insert(item);
                        }
                    }
                }
                else
                {
                    foreach (var itm in lstDept)
                    {
                        var item = new DispatchGoDepartment
                        {
                            DispatchGoID = obj.ID,
                            DepartmentID = itm,
                            IsVerify = false,
                            SendBy = userId,
                            CreateAt = DateTime.Now,
                            CreateBy = obj.UpdateBy
                        };
                        dispatchGoDepartmentDA.Insert(item);
                    }
                }
                dispatchGoDepartmentDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateDepartmentVerify(DispatchGoObjectItem obj, int idDept)
        {
            try
            {
                var itm = dispatchGoDepartmentDA.GetById(obj.ID);
                if (itm != null)
                {
                    itm.IsVerify = true;
                    itm.VerifyAt = obj.DateVerify;
                    itm.Note = obj.NoteVerify;
                    itm.UpdateAt = DateTime.Now;
                    itm.UpdateBy = obj.UpdateBy;
                    dispatchGoDepartmentDA.Update(itm);
                    dispatchGoDepartmentDA.Save();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<int> GetEmployeeNotExits(int dispatchGoID, List<int> lstEmployee)
        {
            List<int> LstEmp = new List<int>();
            var arIDEmptIn = dispatchGoEmployeeDA.GetQuery(p => p.DispatchGoID == dispatchGoID).Select(p => p.EmployeeID).ToArray();
            foreach (var itm in lstEmployee)
            {
                if (!arIDEmptIn.Contains(itm))
                {
                    LstEmp.Add(itm);
                }
            }
            return LstEmp;
        }
        public void UpdateEmployee(DispatchGoItem obj, List<int> lstEmployee, int userId)
        {
            try
            {
                var arIDEmptIn = dispatchGoEmployeeDA.GetQuery(p => p.DispatchGoID == obj.ID).Select(p => p.EmployeeID).ToArray();
                if (arIDEmptIn != null && arIDEmptIn.Count() > 0)
                {
                    foreach (var itm in lstEmployee)
                    {
                        if (!arIDEmptIn.Contains(itm))
                        {
                            var item = new DispatchGoEmployee
                            {
                                DispatchGoID = obj.ID,
                                EmployeeID = itm,
                                SendBy = userId,
                                IsVerify = false,
                                CreateAt = DateTime.Now,
                                CreateBy = obj.UpdateBy
                            };
                            dispatchGoEmployeeDA.Insert(item);
                        }
                    }
                }
                else
                {
                    foreach (var itm in lstEmployee)
                    {
                        var item = new DispatchGoEmployee
                        {
                            DispatchGoID = obj.ID,
                            EmployeeID = itm,
                            IsVerify = false,
                            SendBy = userId,
                            CreateAt = DateTime.Now,
                            CreateBy = obj.UpdateBy
                        };
                        dispatchGoEmployeeDA.Insert(item);
                    }
                }
                dispatchGoEmployeeDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateEmployeeVerify(DispatchGoObjectItem obj, int idEmp)
        {
            try
            {
                var itm = dispatchGoEmployeeDA.GetById(obj.ID);
                if (itm != null)
                {
                    itm.IsVerify = true;
                    itm.Note = obj.NoteVerify;
                    itm.VerifyAt = obj.DateVerify;
                    itm.UpdateAt = DateTime.Now;
                    itm.UpdateBy = obj.UpdateBy;
                    dispatchGoEmployeeDA.Update(itm);
                    dispatchGoEmployeeDA.Save();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateVerifyOut(DispatchGoItem obj, string type)
        {
            try
            {
                var itm = dispatchGoOutSideDA.GetById(obj.ID);
                if (itm != null)
                {
                    itm.IsVerify = true;
                    itm.NoteVerify = obj.NoteVerify;
                    itm.UpdateAt = DateTime.Now;
                    itm.UpdateBy = obj.UpdateBy;
                    dispatchGoOutSideDA.Update(itm);
                    dispatchGoEmployeeDA.Save();
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
                var item = dispatchGoDA.GetById(id);
                dispatchGoDA.Delete(item);
                dispatchGoDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertTask(int taskID, int dispatchID, int useID, string note)
        {
            DispatchGoTaskDA dispatchGoTaskDA = new DA.DispatchGoTaskDA();
            var itm = new DispatchGoTask
            {
                TaskID = taskID,
                DispatchGoID = dispatchID,
                CreateAt = DateTime.Now,
                CreateBy = useID,
                Note = note
            };
            dispatchGoTaskDA.Insert(itm);
            dispatchGoTaskDA.Save();
        }
        public void DeleteDepatment(int id)
        {
            try
            {
                var itm = dispatchGoDepartmentDA.GetById(id);
                dispatchGoDepartmentDA.Delete(itm);
                dispatchGoDepartmentDA.Save();
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
                var itm = dispatchGoEmployeeDA.GetById(id);
                dispatchGoEmployeeDA.Delete(itm);
                dispatchGoEmployeeDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteMoveOut(int id)
        {
            try
            {
                var itm = dispatchGoOutSideDA.GetById(id);
                dispatchGoOutSideDA.Delete(itm);
                dispatchGoOutSideDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CheckNotIsUse(int id)
        {
            DocumentDA doc = new DocumentDA();
            var obj = doc.GetQuery(p => p.DocumentSecurityID == id);
            if (obj.Count() > 0)
                return false;
            ProfileDA profileDa = new ProfileDA();
            var obj1 = profileDa.GetQuery(p => p.ProfileSecurityID == id);
            if (obj1.Count() > 0)
                return false;
            return true;
        }
        public void DeleteRange(List<object> ids)
        {
            throw new NotImplementedException();
        }
        public bool CheckNotExitNumberTo(string code, int id = 0)
        {
            try
            {
                code = code.ToLower();
                if (id > 0)
                {
                    var objOld = dispatchGoDA.GetById(id);
                    if (objOld != null && objOld.Code.Trim().ToLower().Equals(code))
                        return true;
                }
                var itm = dispatchGoDA.GetQuery(p => p.Code.ToLower().Equals(code));
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
        private DispatchGoItem convertToDispatchGoItem(DispatchGo i, HumanEmployeeViewItem employee = null, List<int> userLogonDepts = null, int userID = 0)
        {
            var dbo = dispatchGoDA.Repository;
            var obj = new DispatchGoItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                DestinationAddress = i.DestinationAddress,
                EmployeeCreate = i.CreateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(t => t.ID == i.CreateBy).HumanEmployee.ID : 0,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : string.Empty,
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : string.Empty,
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : string.Empty,
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : string.Empty,
                MethodID = i.DispatchMethodID,
                Method = i.DispatchMethod != null ? i.DispatchMethod.Name : string.Empty,
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                Approval = new HumanEmployeeViewItem()
                {
                    ID = i.ApprovalBy != null ? (int)i.ApprovalBy : 0,
                    Name = i.ApprovalBy.HasValue ?
                    dbo.HumanEmployees.FirstOrDefault(m => m.ID == i.ApprovalBy) != null ?
                    dbo.HumanEmployees.FirstOrDefault(m => m.ID == i.ApprovalBy).Name :
                    string.Empty :
                    string.Empty,
                    Role = i.ApprovalBy.HasValue ?
                    dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == i.ApprovalBy).Select(t => t.HumanRole) != null ?
                    dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == i.ApprovalBy).Select(t => t.HumanRole.Name).FirstOrDefault() :
                    string.Empty :
                    string.Empty,
                    Department = i.ApprovalBy.HasValue ?
                    dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == i.ApprovalBy).Select(t => t.HumanRole) != null ?
                    (dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == i.ApprovalBy).Select(t => t.HumanRole.HumanDepartment) != null ?
                    dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == i.ApprovalBy).Select(t => t.HumanRole.HumanDepartment.Name).FirstOrDefault() : string.Empty) :
                    string.Empty : string.Empty,
                },
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                IsVerify = i.IsVerify,
                IsEdit = i.IsEdit,
                ApprovalBy = i.ApprovalBy,
                IsAccept = i.IsAccept,
                ApprovalNote = i.ApprovalNote,
                ApprovalAt = i.ApprovalAt,
                IsSend = i.IsMove,
                Type = i.Type,
                Note = i.Note,
                IsApproval = i.IsApproval,
                MoveAt = i.MoveAt,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                Create = employSV.GetEmployeeView(i.CreateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(t => t.ID == i.CreateBy).HumanEmployee.ID : 0),
                UpdateBy = i.UpdateBy,
                AlowNotApproval = i.AlowNotAproval,
                AttachmentFileIDs = dbo.DispatchGoAttachmentFiles.Where(t => t.DispatchGoID == i.ID).Select(x => x.AttachmentFileID).ToList()
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                var objEm = employSV.GetEmployeeView(i.ApprovalBy);
                obj.EmployeeInfo = objEm;
                obj.StorageFormID = getDocIssForm(obj.FormH, obj.FormS);
                bool flagApprove = true;
                obj.FlagVerify = checkIsVerfyByUserLogon(i, userLogonDepts, userID);
                if (employee != null)
                    obj.UserReceive = employee;
                if (i.DispatchGoDepartments != null && i.DispatchGoDepartments.Count() > 0)
                {
                    obj.IsVerify = userLogonDepts != null && i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).IsVerify : false;
                    obj.NoteVerify = userLogonDepts != null && i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).Note : string.Empty;
                    if (userLogonDepts != null)
                    {
                        foreach (var dep in i.DispatchGoDepartments)
                        {
                            if (userLogonDepts.Contains(dep.DepartmentID) && !dep.IsVerify)
                            {
                                obj.FlagVerify = false;
                                break;
                            }
                        }
                    }
                }
                if (i.DispatchGoEmployees != null && i.DispatchGoEmployees.Count() > 0)
                {
                    if (userLogonDepts != null)
                    {
                        foreach (var dep in i.DispatchGoEmployees)
                        {
                            if (userLogonDepts.Contains(dep.EmployeeID) && !dep.IsVerify)
                            {
                                obj.FlagVerify = false;
                                break;
                            }
                        }
                    }
                    obj.IsVerify = employee != null && i.DispatchGoEmployees.FirstOrDefault(t => t.DispatchGoID == obj.ID && t.EmployeeID == employee.ID) != null ? i.DispatchGoEmployees.FirstOrDefault(t => t.DispatchGoID == obj.ID && t.EmployeeID == employee.ID).IsVerify : false;
                    obj.NoteVerify = employee != null && i.DispatchGoEmployees.FirstOrDefault(t => t.DispatchGoID == obj.ID && t.EmployeeID == employee.ID) != null ? i.DispatchGoEmployees.FirstOrDefault(t => t.DispatchGoID == obj.ID && t.EmployeeID == employee.ID).Note : string.Empty;
                }
                obj.Status = getStatus(obj.IsSend, (bool)obj.IsVerify, obj.IsApproval, obj.IsAccept, obj.IsEdit, ref flagApprove);
                obj.FlagApprove = flagApprove;
            }
            return obj;
        }

        private string getDocIssForm(bool isH, bool isS)
        {
            if (isH && isS)
                return StorageForm.SoftCopy.ToString() + "," + StorageForm.HardCopy.ToString();
            else if (isS)
                return StorageForm.SoftCopy.ToString();
            else if (isH)
                return StorageForm.HardCopy.ToString();
            return "";
        }
        private string getStatus(bool isMove, bool isConfirm, bool isAprove, bool isAccept, bool isEdit, ref bool flagAprove)
        {
            flagAprove = true;
            if (!isAprove && !isEdit)
            {
                flagAprove = false;
                return "<span style=\"color:#8e210b\">Chờ duyệt</span>";
            }
            else if (isAprove && isAccept && !isMove)
                return "Duyệt";
            else if (isAprove && !isAccept && !isEdit && !isMove)
                return "Không Duyệt";
            else if (isAprove && !isAccept && isEdit && !isMove)
            {
                flagAprove = false;
                return "Sửa đổi";
            }
            else if (isMove && !isConfirm)
            {
                return "Đã chuyển";
            }
            else if (isMove && isConfirm)
                return "Đã xác nhận";
            return "Mới";
        }
        public void UpdateVerify(DispatchGoObjectItem obj, int userId, bool isEmployee)
        {
            try
            {
                var itm = dispatchGoDA.GetById(obj.DispatchGo);
                itm.IsVerify = true;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = userId;
                dispatchGoDA.Update(itm);
                dispatchGoDA.Save();
                if (!isEmployee)
                {
                    obj.DateVerify = DateTime.Now;
                    UpdateDepartmentVerify(obj, obj.ObjectID);
                }
                else
                {
                    obj.DateVerify = DateTime.Now;
                    UpdateEmployeeVerify(obj, obj.ObjectID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateVerifyOut(DispatchGoObjectItem obj)
        {
            try
            {
                var itmOut = dispatchGoOutSideDA.GetById(obj.ID);
                itmOut.IsVerify = true;
                itmOut.NoteVerify = obj.NoteVerify;
                itmOut.Note = obj.NoteMove;
                itmOut.DateVerify = DateTime.Now;
                itmOut.UpdateAt = DateTime.Now;
                itmOut.UpdateBy = obj.UpdateBy;
                dispatchGoOutSideDA.Update(itmOut);
                dispatchGoOutSideDA.Save();
                var itm = dispatchGoDA.GetById(obj.DispatchGo);
                itm.IsVerify = true;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                dispatchGoDA.Update(itm);
                dispatchGoDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool checkIsVerfyByUserLogon(DispatchGo i, List<int> userLogonDepts = null, int userID = 0)
        {
            bool flagVerify = true;
            if (i.DispatchGoOutSides != null && i.DispatchGoOutSides.Count() > 0)
            {
                foreach (var dep in i.DispatchGoOutSides)
                {
                    if (!dep.IsVerify)
                    {
                        flagVerify = false;
                        break;
                    }
                }
            }
            if (i.DispatchGoDepartments != null && i.DispatchGoDepartments.Count() > 0 && userLogonDepts != null && userLogonDepts.Count() > 0)
            {
                foreach (var dep in i.DispatchGoDepartments)
                {
                    if (userLogonDepts.Contains((int)dep.DepartmentID) && !dep.IsVerify)
                    {
                        flagVerify = false;
                        break;
                    }
                }
            }
            if (i.DispatchGoEmployees != null && i.DispatchGoEmployees.Count() > 0 && userID > 0)
            {
                foreach (var us in i.DispatchGoEmployees)
                {
                    if (us.EmployeeID == userID && !us.IsVerify)
                    {
                        flagVerify = false;
                        break;
                    }
                }
            }
            return flagVerify;
        }
        public List<TaskItem> GetAllTaskByID(ModelFilter filter, int dispatchGoID)
        {
            var dbo = dispatchGoDA.Repository;
            List<TaskItem> lst = new List<TaskItem>();
            var task1s = dbo.Tasks.Join(dbo.DispatchGoTasks, t => t.ID, dt => dt.TaskID, (t, dt) => new TaskItem
            {
                ID = t.ID,
                Name = t.Name,
                IDRef = dt.DispatchGo.ID,//Lấy tạm gt
                CategoryID = t.TaskCategoryID,
                CategoryName = t.TaskCategory.Name,
                Content = t.Content,
                IsComplete = t.IsComplete,
                CreateBy = t.CreateBy,
                LevelColor = t.TaskLevel.Color,
                IsEdit = t.IsEdit,
                IsNew = t.IsNew,
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
                PerformBy = t.HumanEmployeeID,
                Rate = t.Rate
            })
            .Where(T => T.IDRef == dispatchGoID)
            .Filter(filter)
            .ToList();
            return task1s;
        }
        public List<TaskItem> GetTreeTask(int taskId, int dispatchGoID)
        {
            var dbo = dispatchGoDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = dbo.Tasks.Join(dbo.DispatchGoTasks.Where(x => x.DispatchGoID == dispatchGoID), t => t.ID, dt => dt.TaskID, (t, dt) => new
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
                result = dbo.Tasks.Join(dbo.DispatchGoTasks.Where(x => x.DispatchGoID == dispatchGoID), t => t.ID, dt => dt.TaskID, (t, dt) => new
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
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            dispatchGoDA.DeleteRange(ids);
            dispatchGoDA.Save();
        }
        /// <summary>
        /// Dùng cho thong kê
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DispatchGoItem> GetTotalDispatchGoFromDepartment(ModelFilter filter, int departmentId, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {

            var dbo = dispatchGoDA.Repository;
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var lstItm = dbo.DispatchGoDepartments
                    .Where(t => t.DepartmentID == departmentId)
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Distinct()
                    .Filter(filter)
                    .ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItemFromStatistical(itm, new List<int> { departmentId }, 0));
                }
            }
            return lst;
        }
        public List<DispatchGoItem> GetTotalDispatchGoFromParentDepartment(ModelFilter filter, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            var dbo = dispatchGoDA.Repository;
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var lstItm = dbo.DispatchGoDepartments
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                     .Distinct()
                    .Filter(filter)
                    .ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItemFromStatisticalParent(itm, 0));
                }
            }
            return lst;
        }
        public List<DispatchGoItem> GetTotalDispatchGoVerifyFromDepartment(ModelFilter filter, int departmentId, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            var dbo = dispatchGoDA.Repository;
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var lstItm = dbo.DispatchGoDepartments
                    .Where(t => t.DepartmentID == departmentId)
                    .Where(t => t.IsVerify)
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Distinct()
                    .Filter(filter)
                    .ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItemFromStatistical(itm, new List<int> { departmentId }, 0));
                }
            }
            return lst;
        }
        public List<DispatchGoItem> GetTotalDispatchGoVerifyFromParentDepartment(ModelFilter filter, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            var dbo = dispatchGoDA.Repository;
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var lstItm = dbo.DispatchGoDepartments
                    .Where(t => t.IsVerify)
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                     .Distinct()
                    .Filter(filter)
                    .ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItemFromStatistical(itm, null, 0));
                }
            }
            return lst;
        }
        public List<DispatchGoItem> GetTotalDispatchGoFromPerson(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = dispatchGoDA.Repository;
            HumanEmployeeViewItem employee = new HumanEmployeeViewItem()
            {
                ID = employeeId,
                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId) != null ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId).Name : string.Empty,
                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.Name : string.Empty,
                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.HumanDepartment.Name : string.Empty,
            };
            List<int> userLogonDepts = dbo.HumanEmployees.Where(t => t.ID == employeeId).SelectMany(t => t.HumanOrganizations.Select(x => x.HumanRole.HumanDepartment.ID).ToList()).ToList();
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var lstItm = dbo.DispatchGoEmployees
                    .Where(t => t.EmployeeID == employeeId)
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Filter(filter)
                    .ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItem(itm, employee, userLogonDepts, 0));
                }
            }
            return lst;
        }
        public List<DispatchGoItem> GetTotalDispatchGoVerifyFromPerson(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = dispatchGoDA.Repository;
            HumanEmployeeViewItem employee = new HumanEmployeeViewItem()
            {
                ID = employeeId,
                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId) != null ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId).Name : string.Empty,
                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.Name : string.Empty,
                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.HumanDepartment.Name : string.Empty,
            };
            List<int> userLogonDepts = dbo.HumanEmployees.Where(t => t.ID == employeeId).SelectMany(t => t.HumanOrganizations.Select(x => x.HumanRole.HumanDepartment.ID).ToList()).ToList();
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var lstItm = dbo.DispatchGoEmployees
                    .Where(t => t.EmployeeID == employeeId)
                    .Where(t => t.IsVerify)
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Filter(filter)
                    .ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItem(itm, employee, userLogonDepts, 0));
                }
            }
            return lst;
        }
        public List<DispatchGoItem> GetTotalDispatchGoFromEmployeeFromPerson(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {

            var dbo = dispatchGoDA.Repository;
            HumanEmployeeViewItem employee = new HumanEmployeeViewItem()
            {
                ID = employeeId,
                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId) != null ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == employeeId).Name : string.Empty,
                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.Name : string.Empty,
                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == employeeId).HumanRole.HumanDepartment.Name : string.Empty,
            };
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var lstItm = dbo.DispatchGoes
                    .Where(t => !t.IsEdit && t.ApprovalBy == employeeId && t.IsAccept && t.IsMove)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Filter(filter)
                    .ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItem(itm, employee, null, 0));
                }
            }
            return lst;
        }
        /// <summary>
        /// Dùng cho xem chi tiết của thống kê
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDeptIDs"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<DispatchGoObjectItem> GetByIDForStatistical(int id)
        {
            try
            {
                List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
                var dbo = dispatchGoDA.Repository;
                var itemTmp = dbo.DispatchGoes.Find(id);
                lst = itemTmp.DispatchGoDepartments
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p => new DispatchGoObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.DepartmentID,
                                DispatchGo = p.DispatchGoID,
                                Company = dbo.HumanDepartments.FirstOrDefault(t => t.ID == p.DepartmentID).Name,
                                Name = p.IsVerify ? dbo.HumanUsers.FirstOrDefault(t => t.ID == p.CreateBy).HumanEmployee.Name : string.Empty,
                                NoteMove = p.Note,
                                NoteVerify = p.Note,
                                DateVerify = p.VerifyAt,
                                IsVerify = p.IsVerify,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Department
                            })
                            .ToList();
                var arIDAtt = itemTmp.DispatchGoEmployees
                            .Where(p => p.DispatchGo.ID == id)
                            .Select(p => new DispatchGoObjectItem
                            {
                                ID = p.ID,
                                ObjectID = p.EmployeeID,
                                DispatchGo = p.DispatchGoID,
                                Name = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).Name,
                                Position = dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault() == null ?
                                       "" : dbo.HumanEmployees.FirstOrDefault(t => t.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                NoteMove = p.Note,
                                IsVerify = p.IsVerify,
                                DateVerify = p.VerifyAt,
                                UpdateAt = p.UpdateAt,
                                UpdateBy = p.UpdateBy,
                                CreateAt = p.CreateAt,
                                CreateBy = p.CreateBy,
                                Type = (int)DispatchObjectType.Employee,
                                NoteVerify = p.Note
                            })
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
        private DispatchGoItem convertToDispatchGoItemFromStatistical(DispatchGo i, List<int> userLogonDepts = null, int userID = 0)
        {
            var dbo = dispatchGoDA.Repository;
            var obj = new DispatchGoItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                DestinationAddress = i.DestinationAddress,
                EmployeeCreate = i.CreateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(t => t.ID == i.CreateBy).HumanEmployee.ID : 0,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : string.Empty,
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : string.Empty,
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : string.Empty,
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : string.Empty,
                MethodID = i.DispatchMethodID,
                Method = i.DispatchMethod != null ? i.DispatchMethod.Name : string.Empty,
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                Approval = new HumanEmployeeViewItem()
                {
                    ID = i.ApprovalBy != null ? (int)i.ApprovalBy : 0,
                    Name = i.ApprovalBy.HasValue ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == i.ApprovalBy) != null ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == i.ApprovalBy).Name : string.Empty : string.Empty,
                    Role = i.ApprovalBy.HasValue ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole.Name : string.Empty : string.Empty,
                    Department = i.ApprovalBy.HasValue ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole.HumanDepartment.Name : string.Empty : string.Empty,
                },
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                IsVerify = i.IsVerify,
                IsEdit = i.IsEdit,
                ApprovalBy = i.ApprovalBy,
                IsAccept = i.IsAccept,
                ApprovalNote = i.ApprovalNote,
                ApprovalAt = i.ApprovalAt,
                IsSend = i.IsMove,
                Note = i.Note,
                IsApproval = i.IsApproval,
                MoveAt = i.MoveAt,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy,
                AlowNotApproval = i.AlowNotAproval
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                var objEm = employSV.GetEmployeeView(i.ApprovalBy);
                obj.EmployeeInfo = objEm;
                obj.StorageFormID = getDocIssForm(obj.FormH, obj.FormS);
                bool flagApprove = true;
                obj.FlagVerify = checkIsVerfyByUserLogon(i, userLogonDepts, userID);
                if (i.DispatchGoDepartments != null && i.DispatchGoDepartments.Count() > 0)
                {
                    obj.IsVerify = userLogonDepts != null && i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).IsVerify : true;
                    obj.NoteVerify = userLogonDepts != null && i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)) != null ? i.DispatchGoDepartments.FirstOrDefault(t => t.DispatchGoID == obj.ID && userLogonDepts.Contains(t.DepartmentID)).Note : string.Empty;
                    if (userLogonDepts != null)
                    {
                        foreach (var dep in i.DispatchGoDepartments)
                        {
                            if (userLogonDepts.Contains(dep.DepartmentID) && !dep.IsVerify)
                            {
                                obj.FlagVerify = false;
                                break;
                            }
                        }
                    }
                }
                obj.Status = getStatus(obj.IsSend, (bool)obj.IsVerify, obj.IsApproval, obj.IsAccept, obj.IsEdit, ref flagApprove);
            }
            return obj;
        }
        private DispatchGoItem convertToDispatchGoItemFromStatisticalParent(DispatchGo i, int userID = 0)
        {
            var dbo = dispatchGoDA.Repository;
            var obj = new DispatchGoItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                Compendia = i.Compendia,
                DestinationAddress = i.DestinationAddress,
                EmployeeCreate = i.CreateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(t => t.ID == i.CreateBy).HumanEmployee.ID : 0,
                SecurityLevelID = i.DispatchSecurityID,
                SecurityColor = i.DispatchSecurity != null ? i.DispatchSecurity.Color : string.Empty,
                Security = i.DispatchSecurity != null ? i.DispatchSecurity.Name : string.Empty,
                UrgencyId = i.DispatchUrgencyID,
                UrgencyColor = i.DispatchUrgency != null ? i.DispatchUrgency.Color : string.Empty,
                Urgency = i.DispatchUrgency != null ? i.DispatchUrgency.Name : string.Empty,
                MethodID = i.DispatchMethodID,
                Method = i.DispatchMethod != null ? i.DispatchMethod.Name : string.Empty,
                FormH = i.FormHard,
                FormS = i.FormSoft,
                Date = i.Date,
                Approval = new HumanEmployeeViewItem()
                {
                    ID = i.ApprovalBy != null ? (int)i.ApprovalBy : 0,
                    Name = i.ApprovalBy.HasValue ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == i.ApprovalBy) != null ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == i.ApprovalBy).Name : string.Empty : string.Empty,
                    Role = i.ApprovalBy.HasValue ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole.Name : string.Empty : string.Empty,
                    Department = i.ApprovalBy.HasValue ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole != null ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole.HumanDepartment.Name : string.Empty : string.Empty,
                },
                CategoryID = i.DispatchCategoryID,
                Category = i.DispatchCategory != null ? i.DispatchCategory.Name : "",
                IsVerify = i.IsVerify,
                IsEdit = i.IsEdit,
                ApprovalBy = i.ApprovalBy,
                IsAccept = i.IsAccept,
                ApprovalNote = i.ApprovalNote,
                ApprovalAt = i.ApprovalAt,
                IsSend = i.IsMove,
                Note = i.Note,
                IsApproval = i.IsApproval,
                MoveAt = i.MoveAt,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy,
                AlowNotApproval = i.AlowNotAproval
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                var objEm = employSV.GetEmployeeView(i.ApprovalBy);
                obj.EmployeeInfo = objEm;
                obj.StorageFormID = getDocIssForm(obj.FormH, obj.FormS);
                bool flagApprove = true;
                if (i.DispatchGoDepartments != null && i.DispatchGoDepartments.Count() > 0)
                {
                    obj.IsVerify = i.DispatchGoDepartments.Where(t => t.DispatchGoID == obj.ID) != null ? i.DispatchGoDepartments.Where(t => t.DispatchGoID == obj.ID && t.IsVerify) != null ? true : false : false;
                }
                obj.Status = getStatus(obj.IsSend, (bool)obj.IsVerify, obj.IsApproval, obj.IsAccept, obj.IsEdit, ref flagApprove);
            }
            return obj;
        }

        public List<DispatchGoItem> GetCC(ModelFilter filter, int employeeId, int userId = 0, int focusId = 0)
        {
            List<DispatchGoItem> lst = new List<DispatchGoItem>();
            var query = dispatchGoDA.GetQuery(p => p.DispatchGoCCs.Any(t => t.HumanEmployeeID == employeeId));
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var lstItm = query.Filter(filter).ToList();
            if (lstItm.Count > 0)
            {
                foreach (var itm in lstItm)
                {
                    lst.Add(convertToDispatchGoItem(itm, null, null, userId));
                }
            }
            return lst;
        }
        public List<HumanEmployeeItem> GetEmployeeCC(ModelFilter filter, int dispatchGoId)
        {
            var dbo = dispatchGoDA.Repository;
            var data = dbo.DispatchGoCCs.Where(t => t.DispatchGoID == dispatchGoId)
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
        public void AddEmployeeCC(int dispatchGoId, int employeeId, int userId)
        {
            var dbo = dispatchGoDA.Repository;
            var obj = dbo.DispatchGoCCs.Where(t => t.DispatchGoID == dispatchGoId && t.HumanEmployeeID == employeeId).FirstOrDefault();
            if (obj == null && employeeId != 0)
            {
                var cc = new DispatchGoCC();
                cc.HumanEmployeeID = employeeId;
                cc.DispatchGoID = dispatchGoId;
                cc.CreateAt = DateTime.Now;
                cc.CreateBy = userId;
                dbo.DispatchGoCCs.Add(cc);
                dbo.SaveChanges();
            }
        }
        public void DeleteCC(string stringId)
        {
            var dbo = dispatchGoDA.Repository;
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            if (ids.Count > 0)
                foreach (var item in ids)
                {
                    dbo.DispatchGoCCs.Remove(dbo.DispatchGoCCs.Where(t => t.ID == (int)item).FirstOrDefault());
                    dbo.SaveChanges();
                }

        }

    }
}
