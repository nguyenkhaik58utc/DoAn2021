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
    public class ModuleSV
    {
        ModuleDA moduleDA = new ModuleDA();

        #region Core system
        public void Update(List<ModuleItem> listModuleItem)
        {
            //step1: get all modules idas on current system web
            var listModuleDB = getAllFromDB();

            //step2: update old module
            foreach (var moduleDB in listModuleDB)
            {
                var moduleItem = getModuleItem(listModuleItem, moduleDB.Code);
                if (moduleItem != null)
                {
                    update(moduleItem, moduleDB);
                }
                else
                {
                    moduleDB.IsDelete = true;
                }
            }

            //step3: insert new module
            foreach (var module in listModuleItem)
            {
                insert(module);
            }

            moduleDA.Save();
        }
        public void Cancel(List<ModuleItem> listModuleItem)
        {
            //step1: get all modules idas on current system web
            var listModuleDB = getAllFromDB();
            //step2: cancel old module
            foreach (var moduleDB in listModuleDB)
            {
                var moduleItem = getModuleItem(listModuleItem, moduleDB.Code);
                if (moduleItem != null)
                {
                    moduleDB.IsDelete = true;
                }
            }
            moduleDA.Save();
        }
        private List<CenterModule> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var modules = moduleDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return modules;
        }
        private ModuleItem getModuleItem(List<ModuleItem> modules, string code)
        {
            foreach (var module in modules)
            {
                if (module.Code == code)
                {
                    modules.Remove(module);
                    return module;
                }
            }
            return null;
        }
        private void insert(ModuleItem item)
        {
            var module = new CenterModule()
            {
                SystemCode = BaseSystem.SystemCode,
                Code = item.Code,
                Name = item.Name,
                IsShow = item.IsShow,
                IsActive = true,
                IsDelete = false,
                Position = item.Position,
                Icon = item.Icon,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            moduleDA.Insert(module);
        }
        private void update(ModuleItem source, CenterModule target)
        {
            target.Name = source.Name;
            target.IsShow = source.IsShow;
            target.IsDelete = false;
            target.Position = source.Position;
            target.Icon = source.Icon;
            target.UpdateAt = DateTime.Now;
            target.UpdateBy = source.UpdateBy;
        }
        public List<MenuModuleItem> GetModulesMenu(string systemCode)
        {
            var modules = moduleDA.GetQuery()
                            .Where(m => m.SystemCode == systemCode)
                            .Where(m => m.IsActive)
                            .Where(m => m.IsShow)
                            .Where(m => !m.IsDelete)
                            .OrderBy(m => m.Position)
                            .Select(m => new MenuModuleItem()
                            {
                                Code = m.Code,
                                Name = m.Name,
                                Icon = m.Icon,
                            })
                            .ToList();
            return modules;
        }
        #endregion

        public void Getmodule()
        {

        }

        //public void Update(List<ModuleItem> listModuleItem)
        //{
        //    //step1: get all modules idas on current system web
        //    var listModuleDB = getAll();
        //    //step2: update old module
        //    foreach (var moduleDB in listModuleDB)
        //    {
        //        var moduleItem = getModuleItem(listModuleItem, moduleDB.Code);
        //        if (moduleItem != null)
        //        {
        //            update(moduleItem, moduleDB);
        //        }
        //    }
        //    moduleDA.Save();
        //}
        public List<ModuleItem> GetAll(string systemCode)
        {
            systemCode = systemCode.Trim();
            var modules = moduleDA.GetQuery()
                        .Where(p => p.SystemCode.Trim().ToUpper().Equals(systemCode.ToUpper()))
                //.Where(m => m.IsActive && m.IsShow && !m.IsDelete)
                        .Select(item => new ModuleItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            IsDeleted = item.IsDelete,
                            IconName = item.Icon,
                            IconUrl = null,
                            Position = item.Position,
                            Description = item.Description,
                        })
                        .OrderBy(item => item.Position)
                        .ToList();
            return modules;
        }
        public void Update(ModuleItem item, int userID)
        {
            var module = moduleDA.GetById(item.ID);
            module.Name = item.Name;
            module.Icon = string.IsNullOrEmpty(item.IconName) ? module.Icon : item.IconName;
            module.Description = item.Description;
            module.IsDelete = item.IsDeleted;
            module.IsActive = item.IsActive;
            module.Position = item.Position;
            module.UpdateAt = DateTime.Now;
            module.UpdateBy = userID;
            moduleDA.Save();
        }
        public List<ModuleSelectedItem> GetModuleSelected(int isoId, string systemCode, int page, int limit, out int totalCount)
        {
            systemCode = systemCode.Trim();
            var dbo = moduleDA.Repository;
            var modules = dbo.CenterModules
                        .Where(p => p.SystemCode.Trim().ToUpper().Equals(systemCode.ToUpper()))
                        .Select(item => new ModuleSelectedItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            IsDeleted = item.IsDelete,
                            IconName = item.Icon,
                            IconUrl = null,
                            Position = (int)item.Position,
                            Description = item.Description,
                            IsSelected = dbo.ISOStandardModules.FirstOrDefault(i => i.ISOStandardID == isoId && i.CenterModuleID == item.ID) != null ? true : false
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, limit, out totalCount)
                        .ToList();
            return modules;
        }

        public ModuleItem GetByID(int ID)
        {
            return moduleDA.GetQuery(i => i.ID == ID).Select(item => new ModuleItem()
            {
                ID = item.ID,
                Name = item.Name,
                Position = item.Position,
                Description = item.Description,
                IsDeleted = item.IsDelete,
                IsShow = item.IsShow,
                IsActive = item.IsActive
            }).FirstOrDefault();
        }
    }
}
