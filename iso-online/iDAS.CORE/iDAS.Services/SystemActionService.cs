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
    public class SystemActionService
    {
        SystemActionDA actionDA = new SystemActionDA();

        public void SetStatusActions(byte status)
        {
            var actions = actionDA.GetQuery().ToList();
            foreach (var action in actions)
            {
                //action.Status = status;
            }
            actionDA.Save();
        }

        public List<ActionMenuItem> GetActionsMenu(FunctionMenuItem function)
        {
            var actions = actionDA.GetQuery()
                            //.Where(a => a.IsActive && a.IsShow && a.ModuleCode==function.ModuleCode && a.FunctionCode == function.Code)
                            //.OrderBy(a => a.Position)
                            .Select(a => new ActionMenuItem()
                            {
                                ModuleCode = a.ModuleCode,
                                FunctionCode = a.FunctionCode,
                                Code = a.Code,
                                Name = a.Name,
                                //Url = a.Url,
                                //Icon = a.Icon,
                            }).ToList();
            return actions;
        }

        public string GetName(int actionId)
        {
            var name = actionDA.GetQuery()
                        .Where(a => (a.ID == actionId))
                        .Select(a => a.Name).FirstOrDefault();
            return name;
        }

        public int GetActionId(string moduleCode, string functionCode, string actionCode) {
            var actionId = actionDA.GetQuery()
                            .Where(a => a.ModuleCode == moduleCode && a.FunctionCode == functionCode && a.Code == actionCode && a.IsActive)
                            .Select(a => a.ID).FirstOrDefault();
            return actionId;
        }

        public List<SystemActionItem> GetActions(string moduleCode, string functionCode) {
            var actions = actionDA.GetQuery()
                            .Where(a => a.ModuleCode == moduleCode && a.FunctionCode == functionCode && a.IsActive)
                            .Select(a => new SystemActionItem() { 
                                ID = a.ID,
                                Code = a.Code,
                            }).ToList();
            return actions;
        }

        public List<SystemActionItem> GetAll()
        {
            var actions = actionDA.GetQuery()
                        .Select(item => new SystemActionItem()
                        {
                            ModuleCode = item.ModuleCode,
                            ModuleName = actionDA.SystemContext.SystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            FunctionCode = item.FunctionCode,
                            FunctionName = actionDA.SystemContext.SystemFunctions.Where(m => m.Code == item.FunctionCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            //Url = item.Url,
                            //IconName = item.Icon,
                            IsActive = item.IsActive,
                            //IsShow = item.IsShow,
                            //Status = item.Status.Value,
                            //Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .ToList();
            return actions;
        }

        public List<SystemActionItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var actions = actionDA.GetQuery()
                        .Select(item => new SystemActionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            ModuleName = actionDA.SystemContext.SystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            FunctionCode = item.FunctionCode,
                            FunctionName = actionDA.SystemContext.SystemFunctions.Where(m => m.Code == item.FunctionCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            //Url = item.Url,
                            //IconName = item.Icon,
                            IsActive = item.IsActive,
                            //IsShow = item.IsShow,
                            //Status = item.Status.Value,
                            //Position = item.Position.Value,
                            //Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return actions;
        }

        public List<SystemActionItem> GetAll(int page, int pageSize, out int totalCount, string moduleCode)
        {
            var actions = actionDA.GetQuery()
                        .Where(a=> a.ModuleCode==moduleCode)
                        .Select(item => new SystemActionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            ModuleName = actionDA.SystemContext.SystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            FunctionCode = item.FunctionCode,
                            FunctionName = actionDA.SystemContext.SystemFunctions.Where(m => m.Code == item.FunctionCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            //Url = item.Url,
                            //IconName = item.Icon,
                            IsActive = item.IsActive,
                            //IsShow = item.IsShow,
                            //Status = item.Status.Value,
                            //Position = item.Position.Value,
                            //Description = item.Description,
                            //Permission =  !string.IsNullOrEmpty(item.Permission)? item.Permission:item.Name,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderBy(item => item.FunctionCode)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return actions;
        }

        public SystemActionItem Get(string moduleCode, string functionCode, string actionCode)  {
            var action = actionDA.GetQuery()
                            .Where(item => item.Code == actionCode && item.FunctionCode == functionCode && item.ModuleCode == moduleCode)
                            .Select(item => new SystemActionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                FunctionCode = item.FunctionCode,
                                Code = item.Code,
                                Name = item.Name,
                                //Url = item.Url,
                                //IconName = item.Icon,
                                IsActive = item.IsActive,
                                //IsShow = item.IsShow,
                                //Status = item.Status.Value,
                                //Position = item.Position.Value,
                                //Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return action;
        }

        public void Insert(SystemActionItem item, int userID)
        {
            var action = new SystemAction()
            {
                Code = item.Code,
                Name = item.Name,
                ModuleCode = item.ModuleCode,
                FunctionCode = item.FunctionCode,
                //Icon = item.IconName,
                //Description = item.Description,
                //Url = item.Url,
                IsDelete = false,
                IsActive = item.IsActive,
                //IsShow = item.IsShow,
                //Status = item.Status,
                //Position = item.Position,
                //Permission = item.Permission,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            actionDA.Insert(action);
            actionDA.Save();
        }

        public void Update(SystemActionItem item, int userID)
        {
            var action = actionDA.GetById(item.ID);
            action.Name = item.Name;
            //action.Icon = string.IsNullOrEmpty(item.IconName) ? action.Icon : item.IconName;
            action.IsActive = item.IsActive;
            //action.IsShow = item.IsShow;
            //action.Url = item.Url;
            //action.Status = item.Status;
            //action.Position = item.Position;
            //a/ction.Description = item.Description;
            //action.Permission = item.Permission;
            action.UpdateAt = DateTime.Now;
            action.UpdateBy = userID;
            actionDA.Save();
        }

        public void Delete(int id)
        {
            var action = actionDA.GetById(id);
            //if (action.Status != (byte)SystemCommon.Status.Obsolete)
            //{
            //    throw new Exception();
            //}

            var service = new SystemPermissionService();
            service.DeleteByActionId(id);

            actionDA.Delete(id);
            actionDA.Save();
        }

        public void DeleteAll()
        {
            actionDA.DeleteAll();
            actionDA.Save();
        }
    }
}
