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
    public class BusinessModuleSV
    {
        private BusinessModuleDA moduleDA;

        public BusinessModuleSV() {
            moduleDA = new BusinessModuleDA();
        }
        public BusinessModuleSV(Database database) {
            moduleDA = new BusinessModuleDA(database);
        }

        public List<BusinessModuleItem> GetAll()
        {
            var modules = moduleDA.GetQuery()
                        .Select(item => new BusinessModuleItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            Icon = item.Icon,
                            //Status = item.Status,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return modules;
        }
        public List<BusinessModuleItem> GetToCombobox()
        {
            var modules = moduleDA.GetQuery()
                        .Select(item => new BusinessModuleItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            Icon = item.Icon,
                            //Status = item.Status,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .ToList();
            return modules;
        }
        public List<BusinessModuleItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var modules = moduleDA.GetQuery()
                        .Select(item => new BusinessModuleItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            Icon = item.Icon,
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
            return modules;
        }

        public BusinessModuleItem Get(string code)
        {
            var module = moduleDA.GetQuery().Where(m => m.Code == code)
                            .Select(item => new BusinessModuleItem()
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                IsActive = item.IsActive,
                                IsShow = item.IsShow,
                                Icon = item.Icon,
                                Position = item.Position,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return module;
        }

        #region Core system
        public void Update(List<CenterModule> centerModules)
        {
            //step1: get all modules from database
            var listModuleDB = getAllFromDB();

            //step2: update old module
            foreach (var moduleDB in listModuleDB)
            {
                var moduleItem = getCenterModule(centerModules, moduleDB.Code);
                if (moduleItem != null)
                {
                    update(moduleItem, moduleDB);
                }
                else {
                    moduleDB.IsDelete = true;
                }
            }

            //step3: insert new module
            foreach (var module in centerModules)
            {
                insert(module);
            }

            moduleDA.Save();
        }
        private List<BusinessModule> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var modules = moduleDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return modules;
        }
        private CenterModule getCenterModule(List<CenterModule> modules, string code)
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
        private void insert(CenterModule item)
        {
            var module = new BusinessModule()
            {
                SystemCode = item.SystemCode,
                Code = item.Code,
                Name = item.Name,
                IsActive = item.IsActive,
                IsShow = item.IsShow,
                IsDelete = item.IsDelete,
                Position = item.Position,
                Icon = item.Icon,
                CreateAt = item.CreateAt,
                CreateBy = item.CreateBy,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
            };
            moduleDA.Insert(module);
        }
        private void update(CenterModule source, BusinessModule target)
        {
            target.Name = source.Name;
            target.IsShow = source.IsShow;
            target.IsDelete = source.IsDelete;
            target.Position = source.Position;
            target.Icon = source.Icon;
            target.CreateAt = source.CreateAt;
            target.CreateBy = source.CreateBy;
            target.UpdateAt = source.UpdateAt;
            target.UpdateBy = source.UpdateBy;
        }
        public List<BusinessMenuModuleItem> GetModulesMenu()
        {
            var systemCode = BaseSystem.SystemCode;
            var modules = moduleDA.GetQuery()
                            .Where(m => m.SystemCode == systemCode)
                            .Where(m => m.IsActive)
                            .Where(m => m.IsShow)
                            .Where(m => !m.IsDelete)
                            .OrderBy(m => m.Position)
                            .Select(m => new BusinessMenuModuleItem()
                            {
                                Code = m.Code,
                                Name = m.Name,
                                Icon = m.Icon,
                            })
                            .ToList();
            return modules;
        }
        #endregion
    }
}
