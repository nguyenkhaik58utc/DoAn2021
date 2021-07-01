using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class BusinessFunctionSV
    {
        private BusinessFunctionDA functionDA;
        public BusinessFunctionSV() {
            functionDA = new BusinessFunctionDA();
        }
        public BusinessFunctionSV(Database database) {
            functionDA = new BusinessFunctionDA(database);
        }
        public List<BusinessFunctionItem> GetAllForPermisstion(int page, int pageSize, out int totalCount, string moduleCode)
        {
            var functions = functionDA.GetQuery()
                        .Where(f => f.ModuleCode == moduleCode && f.IsActive == true && !string.IsNullOrEmpty(f.Name))
                        .Select(item => new BusinessFunctionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            Code = item.Code,
                            Name = item.Name,
                            ParentCode = item.ParentCode,
                            Icon = item.Icon,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            Position = item.Position,
                            Description = item.Description,
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
        public List<BusinessFunctionItem> GetAll(int page, int pageSize, out int totalCount, string moduleCode)
        {
            var functions = functionDA.GetQuery()
                        .Where(f => f.ModuleCode == moduleCode)
                        .Select(item => new BusinessFunctionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            Code = item.Code,
                            Name = item.Name,
                            ParentCode = item.ParentCode,
                            Icon = item.Icon,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            Position = item.Position,
                            Description = item.Description,
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
        public List<BusinessFunctionItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var functions = functionDA.GetQuery()
                        .Select(item => new BusinessFunctionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            Code = item.Code,
                            Name = item.Name,
                            ParentCode = item.ParentCode,
                            Icon = item.Icon,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            Position = item.Position,
                            Description = item.Description,
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
        public List<BusinessMenuFunctionItem> GetFunctionsPermission(string moduleCode)
        {
            var dbo = functionDA.Repository;
            var functions = functionDA.GetQuery()
                            .Where(f => f.IsActive)                         
                            .Where(f => !f.IsDelete)
                            .Where(f => !f.IsGroup)
                            .Where(f => f.ModuleCode == moduleCode)
                            .OrderBy(f => f.ParentCode)
                            .ThenBy(f => f.Position)
                            .ThenByDescending(f => f.IsGroup)
                            .ToList();
            List<BusinessMenuFunctionItem> lst = new List<BusinessMenuFunctionItem>();
            foreach (var item in functions)
            {
                lst.Add(new BusinessMenuFunctionItem()
                            {
                                ModuleCode = item.ModuleCode,
                                ParentCode = item.ParentCode,
                                IDFunction = item.ID,
                                IDModule = dbo.BusinessModules.FirstOrDefault(t=>t.Code==item.ModuleCode).ID,
                                IsGroup = item.IsGroup,
                                Url = item.Url,
                                Code = item.Code,
                                Name = item.Name,
                                Icon = item.Icon,
                                IsShow = item.IsShow,
                            });
            }
            return lst;
        }
        public BusinessFunctionItem GetByID(int id)
        {
            var function = functionDA.GetQuery(t=>t.ID==id)
                            .Select(item => new BusinessFunctionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                ParentCode = item.ParentCode,
                                Code = item.Code,
                                Name = item.Name,
                                Icon = item.Icon,
                                IsActive = item.IsActive,
                                IsShow = item.IsShow,
                                Position = item.Position,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return function;
        }
        public BusinessFunctionItem GetNotifyFunction(string moduleCode, string code, string systemCode)
        {
            var function = functionDA.GetQuery(t => t.ModuleCode.Equals(moduleCode) && t.Code.Equals(code) && t.SystemCode.Equals(systemCode))
                            .Select(item => new BusinessFunctionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                ParentCode = item.ParentCode,
                                Code = item.Code,
                                Url=item.Url,
                                Name = item.Name,
                                Icon = item.Icon,
                                IsActive = item.IsActive,
                                IsShow = item.IsShow,
                                Position = item.Position,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return function;
        }
        public BusinessFunctionItem Get(string moduleCode, string functionCode)
        {
            var function = functionDA.GetQuery()
                            .Where(item => item.Code == functionCode && item.ModuleCode == moduleCode)
                            .Select(item => new BusinessFunctionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                ParentCode = item.ParentCode,
                                Code = item.Code,
                                Name = item.Name,
                                Icon = item.Icon,
                                IsActive = item.IsActive,
                                IsShow = item.IsShow,
                                Position = item.Position,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return function;
        }
        #region Core system
        public void Update(List<CenterFunction> centerFunctions)
        {
            //step1: get all functions on system 
            var listFunctionDB = getAllFromDB();

            //step2: update old function
            foreach (var functionDB in listFunctionDB)
            {
                var functionItem = getCenterFunction(centerFunctions, functionDB.Code, functionDB.ModuleCode);
                if (functionItem != null)
                {
                    update(functionItem, functionDB);
                }
                else {
                    functionDB.IsDelete = true;
                }
            }

            //step3: insert new function
            foreach (var function in centerFunctions)
            {
                insert(function);
            }

            functionDA.Save();
        }
        private List<BusinessFunction> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var functions = functionDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return functions;
        }
        private CenterFunction getCenterFunction(List<CenterFunction> functions, string functioncode, string moduleCode)
        {
            foreach (var function in functions)
            {
                if (function.Code == functioncode && function.ModuleCode == moduleCode)
                {
                    functions.Remove(function);
                    return function;
                }
            }
            return null;
        }
        private void insert(CenterFunction item)
        {
            var function = new BusinessFunction()
            {
                SystemCode = item.SystemCode,
                ModuleCode = item.ModuleCode,
                ParentCode = item.ParentCode,
                Code = item.Code,
                Name = item.Name,
                IsActive = item.IsActive,
                IsShow = item.IsShow,
                IsDelete = item.IsDelete,
                Position = item.Position,
                Url = item.Url,
                Icon = item.Icon,
                IsGroup = item.IsGroup,
                CreateAt = item.CreateAt,
                CreateBy = item.CreateBy,
            };
            functionDA.Insert(function);
        }
        private void update(CenterFunction source, BusinessFunction target)
        {
            target.Name = source.Name;
            target.ParentCode = source.ParentCode;
            target.IsShow = source.IsShow;
            target.IsDelete = source.IsDelete;
            target.IsGroup = source.IsGroup;
            target.Url = source.Url;
            target.Position = source.Position;
            target.Icon = source.Icon;
            target.UpdateAt = source.UpdateAt;
            target.UpdateBy = source.UpdateBy;
        }
        public List<BusinessMenuFunctionItem> GetFunctionsMenu(string moduleCode)
        {
            var systemCode = BaseSystem.SystemCode;
            var functions = functionDA.GetQuery()
                            .Where(f => f.SystemCode == systemCode)
                            .Where(f => f.IsActive)
                            .Where(f => f.IsShow)
                            .Where(f => !f.IsDelete)
                            .Where(f => f.ModuleCode == moduleCode)
                            .OrderBy(f => f.ParentCode)
                            .ThenBy(f => f.Position)
                            .ThenByDescending(f => f.IsGroup)
                            .Select(f => new BusinessMenuFunctionItem()
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
        #endregion
    }
}
