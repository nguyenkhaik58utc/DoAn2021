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
    public class BusinessActionSV
    {
        private BusinessActionDA actionDA;
        public BusinessActionSV() {
            actionDA = new BusinessActionDA();
        }
        public BusinessActionSV(Database database) {
            actionDA = new BusinessActionDA(database);
        }

        public List<BusinessActionItem> GetAll(int page, int pageSize, out int totalCount, string moduleCode, string functionCode)
        {
            var actions = actionDA.GetQuery()
                        .Where(a => a.ModuleCode == moduleCode && a.FunctionCode == functionCode )
                        .Select(item => new BusinessActionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            //  ModuleName = actionDA.SystemContext.MnSystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            FunctionCode = item.FunctionCode,
                            //FunctionName = actionDA.Repository.IDasFunctions.Where(m => m.Code == item.FunctionCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,

                            //Url = item.Url,
                            //IconName = item.Icon,
                            IsActive = item.IsActive,
                            //IsShow = item.IsShow,
                            //Status = item.Status.Value,
                            //Position = item.Position.Value,
                            Description = item.Description,
                            //Permission = !string.IsNullOrEmpty(item.Permission) ? item.Permission : item.Name,
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

        public List<BusinessActionItem> GetActions(string moduleCode, string functionCode)
        {
            var actions = actionDA.GetQuery()
                            .Where(a => a.ModuleCode == moduleCode && a.FunctionCode == functionCode && a.IsActive)
                            .Select(a => new BusinessActionItem()
                            { 
                                ID = a.ID,
                                Code = a.Code,
                            }).ToList();
            return actions;
        }

        public List<BusinessActionItem> GetAll()
        {
            var actions = actionDA.GetQuery()
                        .Select(item => new BusinessActionItem()
                        {
                            ModuleCode = item.ModuleCode,
                          //  ModuleName = actionDA.SystemContext.MnSystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            FunctionCode = item.FunctionCode,
                           // FunctionName = actionDA.SystemContext.MnSystemFunctions.Where(m => m.Code == item.FunctionCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            //Url = item.Url,
                            //IconName = item.Icon,
                            IsActive = item.IsActive,
                            //IsShow = item.IsShow,
                            //Status = item.Status.Value,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .ToList();
            return actions;
        }

        public List<BusinessActionItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var actions = actionDA.GetQuery()
                        .Select(item => new BusinessActionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                            //ModuleName = actionDA.SystemContext.MnSystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            FunctionCode = item.FunctionCode,
                           // FunctionName = actionDA.SystemContext.MnSystemFunctions.Where(m => m.Code == item.FunctionCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            //Url = item.Url,
                            //IconName = item.Icon,
                            IsActive = item.IsActive,
                           // IsShow = item.IsShow,
                            //Status = item.Status.Value,
                           // Position = item.Position.Value,
                            Description = item.Description,
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

        public List<BusinessActionItem> GetAll(int page, int pageSize, out int totalCount, string moduleCode)
        {
            var actions = actionDA.GetQuery()
                        .Where(a=> a.ModuleCode==moduleCode)
                        .Select(item => new BusinessActionItem()
                        {
                            ID = item.ID,
                            ModuleCode = item.ModuleCode,
                          //  ModuleName = actionDA.SystemContext.MnSystemModules.Where(m => m.Code == item.ModuleCode).FirstOrDefault().Name,
                            FunctionCode = item.FunctionCode,
                           // FunctionName = actionDA.SystemContext.MnSystemFunctions.Where(m => m.Code == item.FunctionCode).FirstOrDefault().Name,
                            Code = item.Code,
                            Name = item.Name,
                            //Url = item.Url,
                            //IconName = item.Icon,
                            IsActive = item.IsActive,
                            //IsShow = item.IsShow,
                            ///Status = item.Status.Value,
                            //Position = item.Position.Value,
                            Description = item.Description,
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
        public List<BusinessActionItem> GetByFuctionCode(string functionCode)
        {
            var action = actionDA.GetQuery()
                            .Where(item => item.FunctionCode == functionCode)
                            .Select(item => new BusinessActionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                FunctionCode = item.FunctionCode,
                                Code = item.Code,
                                Name = item.Name,
                                IsActive = item.IsActive,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).ToList();
            return action;
        }
        public List<BusinessActionItem> GetByFuctionID(int functionId)
        {
            var dbo = actionDA.Repository;
            var action = actionDA.GetQuery()
                            .Where(item => dbo.BusinessFunctions.FirstOrDefault(t => t.Code.Equals(item.FunctionCode)).ID == functionId)
                            .Select(item => new BusinessActionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                FunctionCode = item.FunctionCode,
                                Code = item.Code,
                                Name = item.Name,
                                IsActive = item.IsActive,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).ToList();
            return action;
        }
        public List<BusinessActionItem> GetByListFuctionID(List<int> functionId)
        {
            var dbo = actionDA.Repository;
            var action = actionDA.GetQuery()
                            .Where(item => functionId.Contains(dbo.BusinessFunctions.FirstOrDefault(t => t.Code.Equals(item.FunctionCode)).ID))
                            .Select(item => new BusinessActionItem()
                            {
                                ID = item.ID,
                                ModuleCode = item.ModuleCode,
                                FunctionCode = item.FunctionCode,
                                Code = item.Code,
                                Name = item.Name,
                                IsActive = item.IsActive,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).ToList();
            return action;
        }
        public BusinessActionItem Get(string moduleCode, string functionCode, string actionCode)
        {
            var action = actionDA.GetQuery()
                            .Where(item => item.Code == actionCode && item.FunctionCode == functionCode && item.ModuleCode == moduleCode)
                            .Select(item => new BusinessActionItem()
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
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                CreateBy = item.CreateBy,
                                UpdateAt = item.UpdateAt,
                                UpdateBy = item.UpdateBy,
                            }).FirstOrDefault();
            return action;
        }

        #region Core system
        public void Update(List<CenterAction> centerActions)
        {
            //step1: get all actions on system 
            var listActionDB = getAllFromDB();

            //step2: update old action
            foreach (var actionDB in listActionDB)
            {
                var actionItem = getActionItem(centerActions, actionDB.Code, actionDB.ModuleCode, actionDB.FunctionCode);
                if (actionItem != null)
                {
                    update(actionItem, actionDB);
                }
                else {
                    actionDB.IsDelete = true;
                }
            }

            //step3: insert new action
            foreach (var action in centerActions)
            {
                insert(action);
            }

            actionDA.Save();
        }
        private List<BusinessAction> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var actions = actionDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return actions;
        }
        private CenterAction getActionItem(List<CenterAction> actions, string actionCode, string moduleCode, string functionCode)
        {
            foreach (var action in actions)
            {
                if (action.Code == actionCode && action.ModuleCode == moduleCode && action.FunctionCode == functionCode)
                {
                    actions.Remove(action);
                    return action;
                }
            }
            return null;
        }
        private void insert(CenterAction item)
        {
            var action = new BusinessAction()
            {
                SystemCode = item.SystemCode,
                ModuleCode = item.ModuleCode,
                FunctionCode = item.FunctionCode,
                Code = item.Code,
                Name = item.Name,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                Description = item.Description,
                CreateAt = item.CreateAt,
                CreateBy = item.CreateBy,
            };
            actionDA.Insert(action);
        }
        private void update(CenterAction source, BusinessAction target)
        {
            target.Name = source.Name;
            target.IsActive = source.IsActive;
            target.IsDelete = source.IsDelete;
            target.Description = source.Description;
            target.UpdateAt = source.UpdateAt;
            target.UpdateBy = source.UpdateBy;
        }
        #endregion
    }
}
