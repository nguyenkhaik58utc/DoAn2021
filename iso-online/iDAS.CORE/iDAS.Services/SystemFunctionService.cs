using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DAL;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    [BaseSystem]
    public class SystemFunctionService
    {
        SystemFunctionDA functionDA = new SystemFunctionDA();

        public void SetStatusFunctions(byte status)
        {
            var functions = functionDA.GetQuery().ToList();
            foreach (var function in functions)
            {
                //function.Status = status;
            }
            functionDA.Save();
        }

        public List<SystemFunctionItem> GetAll(int page, int pageSize, out int totalCount, string moduleCode)
        {
            var functions = functionDA.GetQuery()
                        .Where(f=>f.ModuleCode==moduleCode)
                        .Select(item => new SystemFunctionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            ModuleName = functionDA.SystemContext.SystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            ParentCode = item.ParentCode,
                            IconName = item.Icon,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            //Status = item.Status.Value,
                            Position = item.Position.Value,
                            //Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return functions;
        }

        public List<SystemFunctionItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var functions = functionDA.GetQuery()
                        .Select(item => new SystemFunctionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            ModuleName = functionDA.SystemContext.SystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            ParentCode = item.ParentCode,
                            IconName = item.Icon,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            //Status = item.Status.Value,
                            Position = item.Position.Value,
                            //Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return functions;
        }

        public List<FunctionMenuItem> GetFunctionsMenu(string moduleCode)
        {
            var functions = functionDA.GetQuery()
                            .Where(f=>f.IsShow)
                            .Where(f=>!f.IsDelete)
                            .Where(f => f.IsActive && f.ModuleCode == moduleCode)
                            .OrderBy(f => f.ParentCode)
                            .ThenBy(f => f.Position)
                            .ThenByDescending(f=>f.IsGroup)
                            .Select(f => new FunctionMenuItem()
                            {
                                ModuleCode = f.ModuleCode,
                                ParentCode = f.ParentCode,
                                IsGroup = f.IsGroup,
                                Url = f.Url,
                                Code = f.Code,
                                Name = f.Name,
                                Icon = f.Icon,
                                IsShow = f.IsShow,
                            }).ToList();
            return functions;
        }

        public SystemFunctionItem Get(string moduleCode, string functionCode) {
            var function = functionDA.GetQuery()
                            .Where(item => item.Code == functionCode && item.ModuleCode == moduleCode)
                            .Select(item => new SystemFunctionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                ParentCode = item.ParentCode,
                                Code = item.Code,
                                Name = item.Name,
                                IconName = item.Icon,
                                IsActive = item.IsActive,
                                IsShow = item.IsShow,
                                //Status = item.Status.Value,
                                Position = item.Position.Value,
                                //Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return function;
        }

        public void Insert(SystemFunctionItem item, int userID)
        {
            var function = new SystemFunction()
            {
                ModuleCode = item.ModuleCode,
                Code = item.Code,
                Name = item.Name,
                Icon = item.IconName,
                //Description = item.Description,
                IsActive = item.IsActive,
                IsShow = item.IsShow,
                IsDelete = false,
                IsGroup = item.IsGroup,
                Url = item.Url,
                //Status = item.Status,
                Position = item.Position,
                ParentCode = item.ParentCode,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            functionDA.Insert(function);
            functionDA.Save();
        }

        public void Update(SystemFunctionItem item, int userID)
        {
            var function = functionDA.GetById(item.ID);
            function.Name = item.Name;
            function.Icon = string.IsNullOrEmpty(item.IconName) ? function.Icon : item.IconName; 
            //function.Description = item.Description;
            function.IsActive = item.IsActive;
            function.IsShow = item.IsShow;
            //function.Status = item.Status;
            function.Position = item.Position;
            function.ParentCode = item.ParentCode;
            function.UpdateAt = DateTime.Now;
            function.UpdateBy = userID;
            functionDA.Save();
        }

        public void Delete(int id)
        {
            var function = functionDA.GetById(id);
            //if (function.Status != (byte)SystemCommon.Status.Obsolete)
            //{
            //    throw new Exception();
            //}

            functionDA.Delete(id);
            functionDA.Save();
        }

        public void DeleteAll() {
            functionDA.DeleteAll();
            functionDA.Save();
        }
    }
}
