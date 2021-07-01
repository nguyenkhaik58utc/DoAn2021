using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;

namespace iDAS.Services
{
    public class CenterFunctionSV
    {
        private CenterFunctionDA functionDA = new CenterFunctionDA();

        #region Core system
        public void Update(List<CenterFunctionItem> listFunctionItem)
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
        private List<CenterFunction> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var functions = functionDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return functions;
        }
        private CenterFunctionItem getFunctionItem(List<CenterFunctionItem> functions, string functioncode, string moduleCode)
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
        private void insert(CenterFunctionItem item)
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
        private void update(CenterFunctionItem source, CenterFunction target)
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
        public List<BusinessMenuFunctionItem> GetFunctionsMenu(string systemCode, string moduleCode)
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

        public List<CenterFunctionItem> getFunctionItem(string functioncode, string moduleCode)
        {
            var functions = functionDA.GetQuery()
                             .Where(f => f.SystemCode == moduleCode && f.ModuleCode == functioncode)
                             .Where(f => !f.IsDelete)
                             .OrderBy(f => f.ParentCode)
                             .ThenBy(f => f.Position)
                             .ThenByDescending(f => f.IsGroup)
                             .Select(f => new CenterFunctionItem()
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
    }
}
