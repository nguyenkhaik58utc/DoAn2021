using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class CenterModuleSV
    {
        private CenterModuleDA moduleDA = new CenterModuleDA();

        #region Core system
        public void Update(List<CenterModuleItem> listModuleItem)
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
        private List<CenterModule> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var modules = moduleDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return modules;
        }
        private CenterModuleItem getModuleItem(List<CenterModuleItem> modules, string code)
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
        private void insert(CenterModuleItem item)
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
        private void update(CenterModuleItem source, CenterModule target)
        {
            target.Name = source.Name;
            target.IsShow = source.IsShow;
            target.IsDelete = false;
            target.Position = source.Position;
            target.Icon = source.Icon;
            target.UpdateAt = DateTime.Now;
            target.UpdateBy = source.UpdateBy;
        }
        public List<BusinessMenuModuleItem> GetModulesMenu(string systemCode)
        {
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

        public List<CenterModuleItem> GetModulesByISOID(int isoId)
        {
            var dbo = moduleDA.Repository;
            List<int> lstModuleID = new List<int>();
            lstModuleID = dbo.ISOStandardModules.Where(t => t.ISOStandardID == isoId && t.CenterModule.SystemCode.Equals("BUSINESS")).Select(t => t.CenterModuleID.Value).ToList();
            var modules = moduleDA.GetQuery()
                            .Where(m => lstModuleID.Contains(m.ID) && m.SystemCode.Equals("BUSINESS"))
                            .Where(m => m.IsActive)
                            .Where(m => m.IsShow)
                            .Where(m => !m.IsDelete)
                            .OrderBy(m => m.Position)
                            .Select(m => new CenterModuleItem()
                            {
                                Code = m.Code,
                                Name = m.Name,
                                Icon = m.Icon,
                            })
                            .ToList();
            return modules;
        }
        public List<CenterModuleItem> GetAll(string systemCode)
        {
            systemCode = systemCode.Trim();
            var dbo = moduleDA.Repository;
            var modules = moduleDA.GetQuery()
                        .Select(item => new CenterModuleItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name
                        })
                        .ToList();
            return modules;
        }
    }
}
