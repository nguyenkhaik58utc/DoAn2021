using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DAL;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    [BaseSystem]
    public class SystemModuleService
    {
        SystemModuleDA moduleDA = new SystemModuleDA();

        public void SetStatusModules(byte status) {
            var modules = moduleDA.GetQuery().ToList();
            foreach (var module in modules) {
                //module.Status = status;
            }
            moduleDA.Save();
        }

        public List<SystemModuleItem> GetAll()
        {
            var modules = moduleDA.GetQuery()
                        .Select(item => new SystemModuleItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            IconName = item.Icon,
                            //Status = item.Status,
                            //Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return modules;
        }

        public List<ModuleMenuItem> GetModulesMenu()
        {
            var modules = moduleDA.GetQuery()
                            .Where(m=>m.IsActive)
                            .Where(m=>m.IsShow)
                            .Where(m=>!m.IsDelete)
                            .OrderBy(m => m.Position)
                            .Select(m => new ModuleMenuItem()
                            {
                                Code = m.Code,
                                Name = m.Name,
                                Icon = m.Icon,
                            })
                            .ToList();
            return modules;
        }

        public List<SystemModuleItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var modules = moduleDA.GetQuery()
                        .Select(item => new SystemModuleItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsShow = item.IsShow,
                            IconName = item.Icon,
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
            return modules;
        }

        public SystemModuleItem Get(string moduleCode)
        {
            var module = moduleDA.GetQuery().Where(m => m.Code == moduleCode)
                            .Select(item => new SystemModuleItem()
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                IsActive = item.IsActive,
                                IsShow = item.IsShow,
                                IconName = item.Icon,
                                //Status = item.Status.Value,
                                Position = item.Position.Value,
                                //Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return module;
        }

        public void Insert(IModule item, int userID)
        {
            var module = new SystemModule()
                {
                    Code = item.Code,
                    Name = item.Name,
                    //Icon = item.IconName,
                    //Description = item.Description,
                    IsActive = item.IsActive,
                    IsShow = item.IsShow,
                    IsDelete  =false,
                    //Status = item.Status,
                    Position = item.Position,
                    CreateAt = DateTime.Now,
                    CreateBy = userID,
                };
            moduleDA.Insert(module);
            moduleDA.Save();
        }

        public void Update(SystemModuleItem item,int userID){
            var module = moduleDA.GetById(item.ID);
            module.Name = item.Name;
            module.Icon = string.IsNullOrEmpty(item.IconName) ? module.Icon : item.IconName;
            //module.Description = item.Description;
            module.IsActive = item.IsActive;
            //module.Status = item.Status;
            module.Position = item.Position;
            module.UpdateAt = DateTime.Now;
            module.UpdateBy = userID;
            moduleDA.Save();
        }

        public void Delete(int id) {
            var module = moduleDA.GetById(id);
            //if (module.Status != (byte)SystemCommon.Status.Obsolete) {
            //    throw new Exception(); 
            //}
            moduleDA.Delete(id);
            moduleDA.Save();
        }

        public void DeleteAll() {
            moduleDA.DeleteAll();
            moduleDA.Save();
        }
    }
}
