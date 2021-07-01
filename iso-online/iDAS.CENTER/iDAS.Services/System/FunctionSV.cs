using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class FunctionSV
    {
        FunctionDA functionDA = new FunctionDA();

        #region Core system
        public void Update(List<FunctionItem> listFunctionItem)
        {
            //step1: get all functions on system 
            var listFunctionDB = getAllFromDB();

            //step2: update old function
            foreach (var functionDB in listFunctionDB)
            {
                var functionItem = getFunctionItem(listFunctionItem, functionDB.Code, functionDB.ModuleCode);
                if (functionItem != null)
                {
                    update(functionItem, functionDB);
                }
                else
                {
                    functionDB.IsDelete = true;
                }
            }

            //step3: insert new function
            foreach (var function in listFunctionItem)
            {
                insert(function);
            }

            functionDA.Save();
        }
        public void Cancel(List<FunctionItem> listFunctionItem)
        {
            //step1: get all functions on system 
            var listFunctionDB = getAllFromDB();
            //step2: cacel function
            foreach (var functionDB in listFunctionDB)
            {
                var functionItem = getFunctionItem(listFunctionItem, functionDB.Code, functionDB.ModuleCode);
                if (functionItem != null)
                {
                    functionDB.IsDelete = true;
                }
            }
            functionDA.Save();
        }
        private List<CenterFunction> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var functions = functionDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return functions;
        }
        private FunctionItem getFunctionItem(List<FunctionItem> functions, string functioncode, string moduleCode)
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
        public List<FunctionItem> getFunctionItem(string functioncode, string moduleCode)
        {
            var functions = functionDA.GetQuery()
                             .Where(f => f.SystemCode == moduleCode && f.ModuleCode == functioncode)
                             .Where(f => !f.IsDelete)
                             .OrderBy(f => f.ParentCode)
                             .ThenBy(f => f.Position)
                             .ThenByDescending(f => f.IsGroup)
                             .Select(f => new FunctionItem()
                             {
                                 ID = f.ID,
                                 ModuleCode = f.ModuleCode,
                                 ParentCode = f.ParentCode,
                                 IsGroup = f.IsGroup,
                                 Url = f.Url,
                                 Code = f.Code,
                                 Name = f.Name,
                                 Icon = f.Icon,
                                 IsShow = f.IsShow,
                             })
                             .ToList();
            return functions;
        }
        private void insert(FunctionItem item)
        {
            var function = new CenterFunction()
            {
                SystemCode = BaseSystem.SystemCode,
                ModuleCode = item.ModuleCode,
                ParentCode = item.ParentCode,
                Code = item.Code,
                Name = item.Name,
                IsShow = item.IsShow,
                IsActive = true,
                IsDelete = false,
                Position = item.Position,
                Url = item.Url,
                Icon = item.Icon,
                IsGroup = item.IsGroup,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            functionDA.Insert(function);
        }
        private void update(FunctionItem source, CenterFunction target)
        {
            target.Name = source.Name;
            target.ParentCode = source.ParentCode;
            target.IsShow = source.IsShow;
            target.IsDelete = false;
            target.IsGroup = source.IsGroup;
            target.Url = source.Url;
            target.Position = source.Position;
            target.Icon = source.Icon;
            target.UpdateAt = DateTime.Now;
            target.UpdateBy = source.UpdateBy;
        }
        public List<MenuFunctionItem> GetFunctionsMenu(string systemCode,string moduleCode)
        {
            var functions = functionDA.GetQuery()
                            .Where(f => f.SystemCode == systemCode)
                            .Where(f => f.IsActive)
                            .Where(f => f.IsShow)
                            .Where(f => !f.IsDelete)
                            .Where(f => f.ModuleCode == moduleCode)
                            .OrderBy(f => f.ParentCode)
                            .ThenBy(f => f.Position)
                            .ThenByDescending(f => f.IsGroup)
                            .Select(f => new MenuFunctionItem()
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

        //public void Update(List<FunctionItem> listFunctionItem)
        //{
        //    //step1: get all functions on system 
        //    var listFunctionDB = getAll();

        //    //step2: update old function
        //    foreach (var functionDB in listFunctionDB)
        //    {
        //        var functionItem = getFunctionItem(listFunctionItem, functionDB.Code, functionDB.ModuleCode);
        //        if (functionItem != null)
        //        {
        //            update(functionItem, functionDB);
        //        }
        //    }
        //    functionDA.Save();
        //}
        public FunctionItem GetByID(int ID)
        {
            var dbo = functionDA.Repository;
            return dbo.CenterFunctions.Where(i => i.ID == ID)
                            .Select(item => new FunctionItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                                SystemCode = item.SystemCode,
                                ModuleName = dbo.CenterModules.Where(i=>i.SystemCode == item.SystemCode&& i.Code == item.ModuleCode).Select(i=>i.Name).FirstOrDefault(),
                                Position = item.Position,
                                Description = item.Description,
                                IsActive = item.IsActive,
                                IsShow = item.IsShow,
                                IsDeleted = item.IsDelete,
                            }
                            ).FirstOrDefault();
        }
    }
}
