using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.Items;
using iDAS.DA;
namespace iDAS.Services
{
    public class TutorialSV
    {
        TutorialDA TutorialDA = new TutorialDA();
        public List<TutorialItem> GetAll(int page, int pageSize, out int total)
        {
            return TutorialDA.GetQuery(i => !i.IsDelete)
                         .Select(
                            item => new TutorialItem
                            {
                                ID = item.ID,
                                FileID = item.FileID,
                                SystemCode = item.SystemCode,
                                FunctionCode = item.FunctionCode,
                                ModuleCode = item.ModuleCode,
                                IsActive = item.IsActive,
                                IsDelete = item.IsDelete,
                                CreateAt = item.CreateAt
                            }
                         )
                         .OrderBy(i => i.CreateAt)
                         .Page(page, pageSize, out total)
                         .ToList();
        }
        public List<TutorialItem> GetBySystemCode(int page, int pageSize, out int total, string SystemCode)
        {
            var dbo = TutorialDA.Repository;
            return dbo.Tutorials.Where(i => !i.IsDelete && i.SystemCode == SystemCode)
                         .Select(
                            item => new TutorialItem
                            {
                                ID = item.ID,
                                SystemCode = item.SystemCode,
                                ModuleCode = item.ModuleCode,
                                ModuleName = dbo.CenterModules.Where(i => i.SystemCode == SystemCode && i.Code == item.ModuleCode).Select(i => i.Name).FirstOrDefault(),
                                FunctionCode = item.FunctionCode,
                                FunctionName = dbo.CenterFunctions.Where(i => i.SystemCode == SystemCode && i.ModuleCode == item.ModuleCode && i.Code == item.FunctionCode).Select(i => i.Name).FirstOrDefault(),
                                FileID = item.FileID,
                                FileName = item.FileID == null ? "Chưa có file" : item.File.Name,
                                IsActive = item.IsActive,
                                IsDelete = item.IsDelete,
                                CreateAt = item.CreateAt
                            }
                         )
                         .OrderBy(i => i.CreateAt)
                         .Page(page, pageSize, out total)
                         .ToList();
        }
        public TutorialItem GetById(int id)
        {
            var dbo = TutorialDA.Repository;
            return dbo.Tutorials.Where(i => i.ID == id)
                         .Select(
                            item => new TutorialItem
                                    {
                                        ID = item.ID,
                                        FileID = item.FileID,
                                        SystemCode = item.SystemCode,
                                        ModuleCode = item.ModuleCode,
                                        ModuleName = dbo.CenterModules.Where(i => i.SystemCode == item.SystemCode && i.Code == item.ModuleCode)
                                            .Select(i => i.Name).FirstOrDefault(),
                                        FunctionCode = item.FunctionCode,
                                        FunctionName = dbo.CenterFunctions.Where(i => i.SystemCode == item.SystemCode && i.ModuleCode == item.ModuleCode && i.Code == item.FunctionCode)
                                            .Select(i => i.Name).FirstOrDefault(),
                                        FileName = item.FileID == null ? "Chưa có file" : item.File.Name,
                                        IsDelete = item.IsDelete,
                                        IsActive = item.IsActive
                                    }
                         ).FirstOrDefault();
        }
        public bool Insert(TutorialItem item, int createBy)
        {
            var tutorial=new Tutorial
            {
                SystemCode = item.SystemCode,
                FunctionCode = item.FunctionCode,
                ModuleCode = item.ModuleCode,
                IsDelete = item.IsDelete,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = createBy
            };
            if (item.AttachFile != null)
            {
                var fileId = new FileSV().Insert(item.AttachFile, createBy);
                tutorial.FileID = fileId;
            }
            TutorialDA.Insert(tutorial);
            TutorialDA.Save();
            return true;
        }
        public bool Update(TutorialItem item, int updateBy)
        {
            var dbo = TutorialDA.Repository;
            var tutorial = dbo.Tutorials.Where(i=>i.ID==item.ID).FirstOrDefault();
            if (item.AttachFile != null)
            {
                if (tutorial.FileID == null)
                {
                    item.AttachFile.ID = Guid.NewGuid();
                    var file = new File()
                    {
                        ID = item.AttachFile.ID,
                        Name = item.AttachFile.Name,
                        Extension = item.AttachFile.Extension,
                        Size = item.AttachFile.Size,
                        Type = item.AttachFile.Type,
                        Data = item.AttachFile.Data,
                        IsDeleted = false,
                        CreateAt = DateTime.Now,
                        CreateBy = updateBy
                    };
                    tutorial.File = file;
                    tutorial.FileID = item.AttachFile.ID;
                }
                else
                {
                    item.AttachFile.ID = (Guid)tutorial.FileID;
                    var file = dbo.Files.Where(i => i.ID == item.AttachFile.ID).FirstOrDefault();
                    file.ID = item.AttachFile.ID;
                    file.Name = item.AttachFile.Name;
                    file.Extension = item.AttachFile.Extension;
                    file.Size = item.AttachFile.Size;
                    file.Type = item.AttachFile.Type;
                    file.Data = item.AttachFile.Data;
                    file.UpdateAt = DateTime.Now;
                    file.UpdateBy = updateBy;
                }
            }
            tutorial.SystemCode = item.SystemCode;
            tutorial.FunctionCode = item.FunctionCode;
            tutorial.ModuleCode = item.ModuleCode;
            tutorial.IsDelete = item.IsDelete;
            tutorial.IsActive = item.IsActive;
            tutorial.UpdateAt = DateTime.Now;
            tutorial.UpdateBy = updateBy;
            dbo.SaveChanges();
            return true;
        }
        public bool Delete(int Id)
        {
            TutorialDA.Delete(Id);
            TutorialDA.Save();
            return true;
        }
        public bool DeleteByListId(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            TutorialDA.DeleteRange(ids);
            TutorialDA.Save();
            return true;
        }

        public List<SystemItem> GetSystem()
        {
            SystemSV systemSV = new SystemSV();
            return systemSV.GetAll();
        }
        public List<ModuleItem> GetModule(string systemCode)
        {
            ModuleDA ModuleDA = new ModuleDA();
            return ModuleDA.GetQuery(i => i.SystemCode == systemCode && !i.IsDelete).Select(item => new ModuleItem() { Code = item.Code, Name = item.Name }).ToList();
        }
        public List<FunctionItem> GetFunction(string systemCode, string moduleCode)
        {
            FunctionDA functionDA = new FunctionDA();
            return functionDA.GetQuery(i => i.SystemCode == systemCode && i.ModuleCode == moduleCode && !i.IsDelete).Select(item => new FunctionItem() { Code = item.Code, Name = item.Name }).ToList();
        }
    }
}
