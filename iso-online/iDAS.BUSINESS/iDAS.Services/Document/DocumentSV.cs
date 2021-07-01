using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Utilities;
using iDAS.DA;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class DocumentSV
    {
        private DocumentDA documentDA = new DocumentDA();
        private DocumentRequirementDA documentReqDA = new DocumentRequirementDA();
        private HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        private HumanUserSV userSV = new HumanUserSV();
     //   private DepartmentSV depSv = new DepartmentSV();
        public List<DocumentItem> GetAll(ModelFilter filter)
        {
            var services = documentDA.GetQuery()
                    .Select(i => new DocumentItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        ParentID = i.ParentID,
                        Code = i.Code,
                        Version = i.Version,
                        IssuedDate = i.PublicDate,
                        IssuedTime = i.PublicNumber,
                        Note = i.Note,
                        FormH = i.FormH,
                        FormS = i.FormS,
                        Security = i.DocumentSecurity.Name,
                        Color = i.DocumentSecurity.Color,
                        EffectiveDate = i.PublicDate,
                        IsEdit = i.IsEdit,
                        Type = i.Type ? "Bên ngoài" : "Nội bộ",
                        TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                        IsApproval = i.IsApproval,
                        IsDeleted = i.IsDelete,
                        IsAccept = i.IsAccept,
                        IsPublic = i.IsPublic,
                        IsObs = i.IsObsolete,
                        HasRequest = false,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        ApprovalBy = i.ApprovalBy,
                        FlagModified = false
                    })
                    .Filter(filter)
                    .OrderByDescending(item => item.CreateAt)
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetAllTypeInByDepartmentID(ModelFilter filter, int departmentID, int focusId)
        {
            var dbo = documentDA.Repository;
           // var lstDepartmentID = depSv.GetDepartmentIDsByParentID(departmentID);
            IQueryable<iDAS.Base.Document> query = dbo.Documents
                .Where(p => (p.DepartmentID.Value==departmentID || p.DocumentCategory.DepartmentID == departmentID) && p.Type == false);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var services = query                    
                    .Select(i => new DocumentItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        ParentID = i.ParentID,
                        Code = i.Code,
                        Version = i.Version,
                        IssuedDate = i.PublicDate,
                        IssuedTime = i.PublicNumber,
                        Note = i.Note,
                        FormH = i.FormH,
                        FormS = i.FormS,
                        Security = i.DocumentSecurity.Name,
                        Color = i.DocumentSecurity.Color,
                        EffectiveDate = i.PublicDate,
                        IsEdit = i.IsEdit,
                        Type = i.Type ? "Bên ngoài" : "Nội bộ",
                        TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                        IsApproval = i.IsApproval,
                        IsDeleted = i.IsDelete,
                        IsAccept = i.IsAccept,
                        IsPublic = i.IsPublic,
                        IsObs = i.IsObsolete,
                        HasRequest = false,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        ApprovalBy = i.ApprovalBy,
                        FlagModified = false,
                        AttachmentFileIDs = dbo.DocumentAttachmentFiles.Where(t=>t.DocumentID==i.ID).Select(x=>x.AttachmentFileID).ToList()
                    })
                    .Filter(filter)
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            if (focusId != 0)
            {
                services = services.OrderBy(i => i.ID == focusId ? false : true).ToList();
            }
            return services;
        }
        public List<DocumentItem> GetAllTypeOutByDepartmentID(ModelFilter filter, int departmentID, int focusId = 0)
        {
            var dbo = documentDA.Repository;
          //  var lstDepartmentID = depSv.GetDepartmentIDsByParentID(departmentID);
            var query = documentDA
                .GetQuery(p =>  (p.DepartmentID.Value ==departmentID || p.DocumentCategory.DepartmentID==departmentID) && p.Type == true)
                .Filter(filter);
            if (focusId != 0)
            {
                query = query.Where(i => i.ID == focusId).Filter(filter);
            }
            var services = query
                    .Select(i => new DocumentItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        ParentID = i.ParentID,
                        Code = i.Code,
                        Version = i.Version,
                        IssuedDate = i.PublicDate,
                        IssuedTime = i.PublicNumber,
                        Note = i.Note,
                        FormH = i.FormH,
                        FormS = i.FormS,
                        Security = i.DocumentSecurity.Name,
                        Color = i.DocumentSecurity.Color,
                        EffectiveDate = i.PublicDate,
                        IsEdit = i.IsEdit,
                        Type = i.Type ? "Bên ngoài" : "Nội bộ",
                        TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                        IsApproval = i.IsApproval,
                        IsDeleted = i.IsDelete,
                        IsAccept = i.IsAccept,
                        IsPublic = i.IsPublic,
                        IsObs = i.IsObsolete,
                        HasRequest = false,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        ApprovalBy = i.ApprovalBy,
                        FlagModified = false,
                        AttachmentFileIDs = dbo.DocumentAttachmentFiles.Where(t => t.DocumentID == i.ID).Select(x => x.AttachmentFileID).ToList()
                    })
                    .Filter(filter)
                    .OrderByDescending(item => item.CreateAt)
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            if (focusId != 0)
            {
                services = services.Where(i => i.ID == focusId).ToList();
            }
            return services;
        }
        public List<DocumentItem> GetAllByCateID(int page, int pageSize, out int totalCount, int cateId)
        {
            var services = documentDA.GetQuery(p => p.DocumentCategoryID == cateId && (p.IsPublic || p.IsObsolete))
                     .Select(i => new DocumentItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                         Code = i.Code,
                         Version = i.Version,
                         IssuedDate = i.PublicDate,
                         IssuedTime = i.PublicNumber,
                         Note = i.Note,
                         FormH = i.FormH,
                         FormS = i.FormS,
                         Security = i.DocumentSecurity.Name,
                         Color = i.DocumentSecurity.Color,
                         EffectiveDate = i.PublicDate,
                         IsEdit = i.IsEdit,
                         Type = i.Type ? "Bên ngoài" : "Nội bộ",
                         TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                         IsApproval = i.IsApproval,
                         IsDeleted = i.IsDelete,
                         IsAccept = i.IsAccept,
                         IsPublic = i.IsPublic,
                         IsObs = i.IsObsolete,
                         CreateAt = i.CreateAt,
                         UpdateAt = i.UpdateAt
                     })
                     .OrderBy(item => item.Name)
                      .OrderBy(item => item.IsObs)
                     .Page(page, pageSize, out totalCount)
                     .ToList();
            if (services != null && services.Count() > 0)
            {
                foreach (var item in services)
                {
                    int code = 0;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, 0, true);
                    item.StatusCode = code;
                }
            }
            return services;
        }
        public List<DocumentItem> GetAllByCateID(int cateId)
        {
            try
            {
                var lst = documentDA.GetQuery(p => p.DocumentCategoryID == cateId && (p.IsPublic || p.IsObsolete))
                             .Select(i => new DocumentItem()
                             {
                                 ID = i.ID,
                                 Name = i.Name,
                                 Code = i.Code,
                                 Version = i.Version,
                                 IssuedDate = i.PublicDate,
                                 IssuedTime = i.PublicNumber,
                                 Note = i.Note,
                                 FormH = i.FormH,
                                 FormS = i.FormS,
                                 Security = i.DocumentSecurity.Name,
                                 Color = i.DocumentSecurity.Color,
                                 EffectiveDate = i.PublicDate,
                                 IsEdit = i.IsEdit,
                                 Type = i.Type ? "Bên ngoài" : "Nội bộ",
                                 TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                                 IsApproval = i.IsApproval,
                                 IsDeleted = i.IsDelete,
                                 IsAccept = i.IsAccept,
                                 IsPublic = i.IsPublic,
                                 IsObs = i.IsObsolete,
                                 CreateAt = i.CreateAt,
                                 UpdateAt = i.UpdateAt
                             })
                             .ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public object GetAllIssued()
        {
            var services = documentDA.GetQuery(p => p.IsPublic && !p.IsObsolete)
                     .Select(i => new DocumentItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                         Code = i.Code,
                     })
                     .OrderByDescending(item => item.Name)
                     .ToList();
            return services;
        }
        public DocumentItem GetByID(int Id)
        {
            var dbo = documentDA.Repository;
            var item = documentDA.GetQuery(p => p.ID == Id)
                    .Select(i => new DocumentItem()
                    {
                        ID = i.ID,
                        ParentID = i.ParentID,
                        Name = i.Name,
                        Code = i.Code,
                        Version = i.Version,
                        Department = new HumanDepartmentViewItem()
                        {
                            ID = i.DepartmentID == null ? 0 : (int)i.DepartmentID,
                            Name = dbo.HumanDepartments.Where(d => d.ID == i.DepartmentID).Select(d => d.Name).FirstOrDefault()
                        },
                        DepartmentOfCategory = dbo.HumanDepartments.FirstOrDefault(d => d.ID == i.DocumentCategory.DepartmentID).Name,
                        IssuedDate = i.PublicDate,
                        IssuedTime = i.PublicNumber,
                        CategoryID = i.DocumentCategoryID,
                        Category = i.DocumentCategory.Name,
                        Type = i.Type ? "Bên ngoài" : "Nội bộ",
                        TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                        Note = i.Note,
                        EmployeesCreateID = dbo.HumanUsers.FirstOrDefault(t => t.ID == i.CreateBy).HumanEmployee.ID,
                        NoteIssues = i.NoteIssued,
                        Content = i.Content,
                        FormH = i.FormH,
                        FormS = i.FormS,
                        SecurityID = i.DocumentSecurityID,
                        Security = i.DocumentSecurity.Name,
                        Color = i.DocumentSecurity.Color,
                        IsEdit = i.IsEdit,
                        IsApproval = i.IsApproval,
                        IsDeleted = i.IsDelete,
                        IsAccept = i.IsAccept,
                        IsPublic = i.IsPublic,
                        IsObs = i.IsObsolete,
                        ApprovalBy = i.ApprovalBy,
                        ApprovalAt = i.ApprovalAt,
                        ApproveNote = i.ApprovalNote,
                        EffectiveDate = i.PublicDate,
                        AllUserAccess = i.AllUserAccess,
                        FolderID = i.FolderID,
                        QuickDownload = i.QuickDownload,
                        DayDownloadLimited = i.DayDownloadLimited
                    })
                    .FirstOrDefault();
            if (item != null)
            {
                int code = 0;
                item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID);
                item.StatusCode = code;
                item.StatusApprove = getStatusApprove(item.IsApproval, item.IsAccept);
                var employeee = sysUserSV.GetEmployeeView(item.ApprovalBy);
                item.Approval = employeee;
                item.EmployeeInfo = employeee;
                if (item.EmployeeInfo != null)
                {
                    item.Approver = item.EmployeeInfo.Name;
                    item.Position = getEmployeeInfo(item.EmployeeInfo);
                }
                item.CreateByName = userSV.GetNameByUserID(item.CreateBy);
                item.UpdateByName = userSV.GetNameByUserID(item.UpdateBy);
                item.StorageFormID = getDocIssForm(item.FormH, item.FormS);
                if (item.ParentID > 0)
                {
                    var old = documentDA.GetById(item.ParentID);
                    item.DocumentParent = old.Name;
                }
                item.AttachmentFile = new AttachmentFileItem()
                {
                    Files = dbo.DocumentAttachmentFiles.Where(i => i.DocumentID == Id)
                        .Select(i => i.AttachmentFileID).ToList()
                };
                
            }
            return item;
        }
        public DocumentItem GetDetailByID(int Id)
        {
            var dbo = documentDA.Repository;
            var item = documentDA.GetQuery(p => p.ID == Id)
                    .Select(i => new DocumentItem()
                    {
                        ID = i.ID,
                        ParentID = i.ParentID,
                        Name = i.Name,
                        Code = i.Code,
                        Version = i.Version,
                        Department = new HumanDepartmentViewItem()
                        {
                            ID = i.DepartmentID == null ? 0 : (int)i.DepartmentID,
                            Name = dbo.HumanDepartments.Where(d => d.ID == i.DepartmentID).Select(d => d.Name).FirstOrDefault()
                        },
                        IssuedDate = i.PublicDate,
                        IssuedTime = i.PublicNumber,
                        CategoryID = i.DocumentCategoryID,
                        Category = i.DocumentCategory.Name,
                        DepartmentOfCategory = dbo.HumanDepartments.Where(d => d.ID == i.DocumentCategory.DepartmentID).Select(d => d.Name).FirstOrDefault(),
                        Type = i.Type ? "Bên ngoài" : "Nội bộ",
                        TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                        Note = i.Note,
                        NoteIssues = i.NoteIssued,
                        Content = i.Content,
                        FormH = i.FormH,
                        FormS = i.FormS,
                        SecurityID = i.DocumentSecurityID,
                        EmployeesCreateID = dbo.HumanUsers.FirstOrDefault(t => t.ID == i.CreateBy).HumanEmployee.ID,
                        Security = i.DocumentSecurity.Name,
                        Color = i.DocumentSecurity.Color,
                        IsEdit = i.IsEdit,
                        IsApproval = i.IsApproval,
                        Approval = new HumanEmployeeViewItem()
                        {
                            ID = i.ApprovalBy != null ? (int)i.ApprovalBy : 0,
                            Name = i.ApprovalBy.HasValue ? dbo.HumanEmployees.FirstOrDefault(m => m.ID == i.ApprovalBy).Name : string.Empty,
                            Role = i.ApprovalBy.HasValue ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole.Name : string.Empty,
                            Department = i.ApprovalBy.HasValue ? dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == i.ApprovalBy).HumanRole.HumanDepartment.Name : string.Empty,
                        },
                        IsDeleted = i.IsDelete,
                        IsAccept = i.IsAccept,
                        IsPublic = i.IsPublic,
                        IsObs = i.IsObsolete,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy,
                        ApprovalBy = i.ApprovalBy,
                        ApprovalAt = i.ApprovalAt,
                        ApproveNote = i.ApprovalNote,
                        EffectiveDate = i.PublicDate,
                        FolderID = i.FolderID,
                        AllUserAccess = i.AllUserAccess,
                        QuickDownload = i.QuickDownload,
                        DayDownloadLimited = i.DayDownloadLimited
                    })
                    .FirstOrDefault();
            item.AttachmentFile = new AttachmentFileItem()
            {
                Files = dbo.DocumentAttachmentFiles.Where(i => i.DocumentID == Id)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            if (item != null)
            {
                int code = 0;
                item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID);
                item.StatusCode = code;
                item.StatusApprove = getStatusApprove(item.IsApproval, item.IsAccept);
                item.CreateByName = userSV.GetNameByUserID(item.CreateBy);
                item.UpdateByName = userSV.GetNameByUserID(item.UpdateBy);
                item.StorageFormID = getDocIssForm(item.FormH, item.FormS);
                if (item.ParentID > 0)
                {
                    var old = documentDA.GetById(item.ParentID);
                    item.DocumentParent = old.Name;
                }
            }
            return item;
        }
        public DocumentItem GetObjApproveByID(int Id)
        {
            var obj = GetByID(Id);
            if (obj != null)
            {
                var dbo = documentDA.Repository;
                obj.AttachmentFile = new AttachmentFileItem()
                {
                    Files = dbo.DocumentAttachmentFiles.Where(i => i.DocumentID == Id)
                        .Select(i => i.AttachmentFileID).ToList()
                };
            }
            return obj;
        }
        public List<DocumentItem> GetByParentID(int Id)
        {
            var lst = documentDA.GetQuery(p => p.ParentID == Id)
                    .Select(i => new DocumentItem()
                    {
                        ID = i.ID,
                        ParentID = i.ParentID,
                        Name = i.Name,
                        Code = i.Code,
                        Version = i.Version,
                    }).ToList();
            return lst;
        }
        public int Insert(DocumentItem obj, int employeeId = 0)
        {
            try
            {
                var itm = new Document
                {
                    Name = obj.Name.Trim(),
                    Code = obj.Code.Trim(),
                    Version = obj.Version.Trim(),
                    Type = obj.TypeID == (int)DocumentType.In ? false : true,
                    FormH = obj.FormH,
                    FormS = obj.FormS,
                    DocumentSecurityID = obj.SecurityID,
                    DocumentCategoryID = obj.CategoryID,
                    IsEdit = obj.IsEdit,
                    IsApproval = false,
                    IsAccept = false,
                    IsPublic = false,
                    IsDelete = false,
                    ApprovalBy = obj.Approval.ID != 0 ? obj.Approval.ID : employeeId,
                    PublicDate = obj.EffectiveDate,
                    PublicNumber = obj.IssuedTime,
                    DepartmentID = obj.Department.ID,
                    Note = obj.Note,
                    Content = obj.Content,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy,
                    AllUserAccess = obj.AllUserAccess,
                    FolderID = obj.FolderID,
                    QuickDownload = obj.QuickDownload,
                    DayDownloadLimited = obj.DayDownloadLimited
                };
                if (obj.ParentID > 0) itm.ParentID = obj.ParentID;
                documentDA.Insert(itm);
                documentDA.Save();
                new FileSV().Upload<DocumentAttachmentFile>(obj.AttachmentFile, itm.ID);
                return itm.ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(DocumentItem obj, int employeeId = 0)
        {
            var dbo = documentDA.Repository;
            var itm = documentDA.GetById(obj.ID);
            itm.Name = obj.Name.Trim();
            itm.PublicDate = obj.EffectiveDate;
            itm.Code = obj.Code.Trim();
            itm.Version = obj.Version.Trim();
            itm.Type = obj.TypeID == (int)DocumentType.In ? false : true;
            itm.FormH = obj.FormH;
            itm.FormS = obj.FormS;
            itm.DocumentSecurityID = obj.SecurityID;
            itm.DocumentCategoryID = obj.CategoryID;
            itm.IsEdit = obj.IsEdit;
            if (!itm.IsEdit && itm.IsApproval && !itm.IsAccept)
                itm.IsApproval = false;
            itm.Content = obj.Content;
            itm.ApprovalBy = obj.Approval.ID != 0 ? obj.Approval.ID : employeeId;
            itm.Note = obj.Note;
            itm.DepartmentID = obj.Department.ID;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            itm.AllUserAccess = obj.AllUserAccess;
            itm.FolderID = obj.FolderID;
            itm.QuickDownload = obj.QuickDownload;
            itm.DayDownloadLimited = obj.DayDownloadLimited;

            documentDA.Update(itm);
            documentDA.Save();
            new FileSV().Upload<DocumentAttachmentFile>(obj.AttachmentFile, obj.ID);

        }
        public void UpdateApprove(DocumentItem obj)
        {
            var itm = documentDA.GetById(obj.ID);
            if (!obj.IsAccept)
                itm.IsEdit = obj.IsEdit;
            else
            {
                itm.IsEdit = false;
            }
            itm.IsAccept = obj.IsAccept;
            itm.IsApproval = true;
            itm.ApprovalNote = obj.ApprovalNote;
            itm.ApprovalAt = obj.ApprovalAt;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            documentDA.Update(itm);
            documentDA.Save();
        }
        public void UpdateIssued(DocumentItem obj)
        {
            try
            {
                var itm = documentDA.GetById(obj.ID);
                if (obj.NoteIssues != null) obj.NoteIssues = obj.NoteIssues.Trim();
                itm.NoteIssued = obj.NoteIssues;
                if (obj.StatusApprove.Equals(iDAS.Utilities.Common.DocumentStatus.Obsolete.ToString()))
                {
                    itm.IsObsolete = true;
                    itm.DateObsolete = DateTime.Now;
                }
                else if (obj.StatusApprove.Equals(iDAS.Utilities.Common.DocumentStatus.Issued.ToString()))
                {
                    if (!itm.IsPublic)
                    {
                        itm.IsPublic = true;
                        if (itm.ParentID > 0)
                        {
                            var itmOld = documentDA.GetById(itm.ParentID);
                            itmOld.IsObsolete = true;
                            itmOld.DateObsolete = DateTime.Now;
                            itmOld.UpdateAt = DateTime.Now;
                            itmOld.UpdateBy = obj.UpdateBy;
                            documentDA.Update(itmOld);
                        }
                    }
                    itm.IsObsolete = false;
                    itm.PublicDate = obj.IssuedDate;
                }
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                documentDA.Update(itm);
                documentDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateIssue(DocumentItem obj)
        {
            var itm = documentDA.GetById(obj.ID);
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            documentDA.Update(itm);
            if (itm.ParentID > 0)
            {
                var itmOld = documentDA.GetById(itm.ParentID);
                itmOld.IsObsolete = true;
                itmOld.UpdateAt = DateTime.Now;
                itmOld.UpdateBy = obj.UpdateBy;
                documentDA.Update(itmOld);
            }
            documentDA.Save();
        }
        public void UpdateObs(DocumentItem obj)
        {
            var itm = documentDA.GetById(obj.ID);
            itm.IsDelete = obj.IsDeleted;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            documentDA.Update(itm);
            documentDA.Save();
        }
        public DocumentItem GetByCodeVerson(string code)
        {
            code = code.Trim().ToLower();
            var services = documentDA.GetQuery(p => p.Code.Trim().ToLower().Equals(code))
                     .Select(i => new DocumentItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                         Code = i.Code,
                         Version = i.Version,
                         IssuedDate = i.PublicDate,
                         IssuedTime = i.PublicNumber,
                         Note = i.Note,
                         FormH = i.FormH,
                         FormS = i.FormS,
                         CreateAt = i.CreateAt,
                         UpdateAt = i.UpdateAt
                     }).FirstOrDefault();
            return services;
        }
        public string getStatus(bool isEdit, bool isApp, bool isSucc, bool isPublic, bool isObs, ref int code, int? parentID = 0, bool color = false, bool ckReq = false)
        {
            if (isEdit && !isApp && parentID > 0)
            {
                code = (int)Common.DocumentStatus.New;
                return "<span style=\"color:#045fb8\">Tạo mới</span>";
            }
            else if (isEdit && !isApp)
            {
                code = (int)Common.DocumentStatus.New;
                return "<span style=\"color:#045fb8\">Tạo mới</span>";
            }
            else if (!isEdit && !isApp)
            {
                code = (int)Common.DocumentStatus.PreApprove;
                return "<span style=\"color:#8e210b\">Chờ duyệt</span>";
            }
            else if (isApp && !isSucc && !isEdit)
            {
                code = (int)Common.DocumentStatus.ApproveFail;
                return "<span style=\"color:#41519f\">Không duyệt</span>";
            }
            else if (isApp && !isSucc && isEdit)
            {
                code = (int)Common.DocumentStatus.Modified;
                return "<span style=\"color:red\">Sửa đổi</span>";
            }
            else if (isApp && isSucc && !isPublic)
            {
                code = (int)Common.DocumentStatus.Approve;
                return "<span style=\"color:blue\">Duyệt</span>";
            }
            else if (isPublic && !isObs)
            {
                code = (int)Common.DocumentStatus.Issued;
                if (color)
                    return iDAS.Utilities.Common.StatusColor((int)Utilities.Common.DocumentStatus.Issued);
                if (isPublic && !isObs && !color)
                    return "<span style=\"color:blue\">Ban hành </span>";
            }
            else if (isObs && color)
            {
                code = (int)Common.DocumentStatus.Obsolete;
                return iDAS.Utilities.Common.StatusColor((int)Utilities.Common.DocumentStatus.Obsolete);
            }
            else if (isObs && !color)
            {
                code = (int)Common.DocumentStatus.Obsolete;
                return "<span style=\"color:red\">Lỗi thời</span>";
            }
            return "";
        }
        public string getStatusApprove(bool isApp, bool isSucc)
        {
            if (isApp && !isSucc)
                return "Duyệt không đạt";
            else if (isApp && isSucc)
            {
                return "Duyệt đạt";
            }
            return "";
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
            var dbo = documentDA.Repository;
            var item = documentDA.GetById(id);
            var att = dbo.DocumentAttachmentFiles.Where(t => t.DocumentID == id).Select(t => t.AttachmentFileID).ToArray();
            if (att.Count() > 0)
            {
                dbo.AttachmentFiles.RemoveRange(dbo.AttachmentFiles.Where(x => att.Contains(x.ID)));
            }
            dbo.DocumentAttachmentFiles.RemoveRange(dbo.DocumentAttachmentFiles.Where(x => x.DocumentID == id));
            documentDA.Save();
            documentDA.Delete(item);
            documentDA.Save();
        }


        /// <summary>
        /// Dùng cho phòng ban
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="securityId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DocumentItem> GetTotalDocumentFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                .Where(t => t.PublicDate.HasValue)
                 .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                .Where(t => !t.IsDelete)
                .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentIssuedFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                    .Where(t => t.DepartmentID == departmentId)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.IsDelete)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentObsoleteFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                        .Where(t => t.DepartmentID == departmentId)
                        .Where(t => t.PublicDate.HasValue)
                           .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.IsDelete)
                        .Where(t => t.IsObsolete)
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => !t.Type)
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInIssuedFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                 .Where(t => t.PublicDate.HasValue)
                   .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                .Where(t => !t.IsDelete)
                .Where(t => !t.Type)
                .Where(t => t.IsPublic && !t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInObsoleteFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                   .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                     .Where(t => !t.Type)
                        .Where(t => t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                    .Where(t => t.Type)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutIssuedFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                  .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                   .Where(t => t.Type)
                        .Where(t => t.IsPublic && !t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutObsoleteFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                     .Where(t => t.Type)
                        .Where(t => t.IsObsolete)
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberDistributionFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.DepartmentID == departmentId)
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberOfDistributionFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.DepartmentID == departmentId)
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                .Where(t => t.DepartmentID == departmentId)
                .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberOfThuhoiFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.DepartmentID == departmentId)
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                    .Where(t => t.DateObsolote.HasValue)
                    .Where(t => t.NumberObsolete.HasValue && t.NumberObsolete.Value != 0)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                  .Where(t => t.DepartmentID == departmentId)
                 .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        /// <summary>
        /// Cho parent
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DocumentItem> GetTotalDocumentFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                  .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })

                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentIssuedFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentObsoleteFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                       .Where(t => t.IsObsolete)
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                            .Where(t => !t.Type)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInIssuedFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                  .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                   .Where(t => !t.Type)
                .Where(t => t.IsPublic && !t.IsObsolete)
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInObsoleteFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                             .Where(t => !t.Type)
                        .Where(t => t.IsObsolete)
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                         .Where(t => t.Type)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutIssuedFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                      .Where(t => t.Type)
                        .Where(t => t.IsPublic && !t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutObsoleteFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                       .Where(t => t.Type)
                        .Where(t => t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberDistributionFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                     .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                     .Select(t => t.DepartmentID)
                     .Distinct()
                     .ToArray();
            var services = documentDA.GetQuery()
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberOfDistributionFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                  .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberOfThuhoiFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                     .Where(t => t.DateObsolote.HasValue)
                    .Where(t => t.NumberObsolete.HasValue && t.NumberObsolete.Value != 0)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                 .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        /// <summary>
        /// View document by cate
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DocumentItem> GetTotalDocumentFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                  .Where(t => t.DocumentCategoryID == cateId)
                  .Where(t => t.PublicDate.HasValue)
                  .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete) 
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentIssuedFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                  .Where(t => t.DocumentCategoryID == cateId)
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => t.IsPublic && !t.IsObsolete)
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentObsoleteFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                        .Where(t => t.DocumentCategoryID == cateId)
                        .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.IsDelete)
                        .Where(t => t.IsObsolete)
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
                  .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => !t.Type)
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInIssuedFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
            .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                .Where(t => !t.IsDelete)
                .Where(t => !t.Type)
                .Where(t => t.IsPublic && !t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentInObsoleteFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
                 .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                     .Where(t => !t.Type)
                        .Where(t => t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
                  .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                    .Where(t => t.Type)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutIssuedFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                   .Where(t => t.Type)
                        .Where(t => t.IsPublic && !t.IsObsolete)
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetDocumentOutObsoleteFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
                  .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                     .Where(t => t.Type)
                        .Where(t => t.IsObsolete)
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberDistributionFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.DocumentCategoryID == cateId)
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
              .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                 .Select(i => new DocumentItem()
                 {
                     ID = i.ID,
                     Name = i.Name,
                     ParentID = i.ParentID,
                     Code = i.Code,
                     Version = i.Version,
                     IssuedDate = i.PublicDate,
                     IssuedTime = i.PublicNumber,
                     Note = i.Note,
                     FormH = i.FormH,
                     FormS = i.FormS,
                     Security = i.DocumentSecurity.Name,
                     Color = i.DocumentSecurity.Color,
                     EffectiveDate = i.PublicDate,
                     IsEdit = i.IsEdit,
                     Type = i.Type ? "Bên ngoài" : "Nội bộ",
                     TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                     IsApproval = i.IsApproval,
                     IsDeleted = i.IsDelete,
                     IsAccept = i.IsAccept,
                     IsPublic = i.IsPublic,
                     IsObs = i.IsObsolete,
                     HasRequest = false,
                     CreateAt = i.CreateAt,
                     UpdateAt = i.UpdateAt,
                     ApprovalBy = i.ApprovalBy,
                     FlagModified = false
                 })
                 .Filter(filter)
                 .OrderByDescending(item => item.CreateAt)
                 .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberOfDistributionFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.DocumentCategoryID == cateId)
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                .Where(t => t.DocumentCategoryID == cateId)
                   .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                   .Select(i => new DocumentItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       ParentID = i.ParentID,
                       Code = i.Code,
                       Version = i.Version,
                       IssuedDate = i.PublicDate,
                       IssuedTime = i.PublicNumber,
                       Note = i.Note,
                       FormH = i.FormH,
                       FormS = i.FormS,
                       Security = i.DocumentSecurity.Name,
                       Color = i.DocumentSecurity.Color,
                       EffectiveDate = i.PublicDate,
                       IsEdit = i.IsEdit,
                       Type = i.Type ? "Bên ngoài" : "Nội bộ",
                       TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                       IsApproval = i.IsApproval,
                       IsDeleted = i.IsDelete,
                       IsAccept = i.IsAccept,
                       IsPublic = i.IsPublic,
                       IsObs = i.IsObsolete,
                       HasRequest = false,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt,
                       ApprovalBy = i.ApprovalBy,
                       FlagModified = false
                   })
                   .Filter(filter)
                   .OrderByDescending(item => item.CreateAt)
                   .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
        public List<DocumentItem> GetNumberOfThuhoiFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = documentDA.Repository;
            var documentIds = dbo.DocumentDistributions
                    .Where(t => t.Document.DocumentCategoryID == cateId)
                    .Where(t => t.Document.CreateAt >= fromDate && t.Document.CreateAt <= toDate)
                    .Where(t => t.DateObsolote.HasValue)
                    .Where(t => t.NumberObsolete.HasValue && t.NumberObsolete.Value != 0)
                    .Select(t => t.DocumentID)
                    .Distinct()
                    .ToArray();
            var services = documentDA.GetQuery()
                  .Where(t => t.DocumentCategoryID == cateId)
                  .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                  .Where(t => !t.IsDelete)
                  .Where(t => documentIds.Contains(t.ID))
                  .Select(i => new DocumentItem()
                  {
                      ID = i.ID,
                      Name = i.Name,
                      ParentID = i.ParentID,
                      Code = i.Code,
                      Version = i.Version,
                      IssuedDate = i.PublicDate,
                      IssuedTime = i.PublicNumber,
                      Note = i.Note,
                      FormH = i.FormH,
                      FormS = i.FormS,
                      Security = i.DocumentSecurity.Name,
                      Color = i.DocumentSecurity.Color,
                      EffectiveDate = i.PublicDate,
                      IsEdit = i.IsEdit,
                      Type = i.Type ? "Bên ngoài" : "Nội bộ",
                      TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                      IsApproval = i.IsApproval,
                      IsDeleted = i.IsDelete,
                      IsAccept = i.IsAccept,
                      IsPublic = i.IsPublic,
                      IsObs = i.IsObsolete,
                      HasRequest = false,
                      CreateAt = i.CreateAt,
                      UpdateAt = i.UpdateAt,
                      ApprovalBy = i.ApprovalBy,
                      FlagModified = false
                  })
                  .Filter(filter)
                  .OrderByDescending(item => item.CreateAt)
                  .ToList();
            if (services != null && services.Count() > 0)
            {
                var arReq = documentReqDA.GetQuery(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();
                foreach (var item in services)
                {
                    int code = 0;
                    if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                        item.HasRequest = true;
                    item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                    item.StatusCode = code;
                    if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                        item.FlagModified = true;
                }
            }
            return services;
        }
    }
}
