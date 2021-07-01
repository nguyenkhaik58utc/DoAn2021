using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Services
{
    public class ActionSV
    {
        ActionDA ActionDA = new ActionDA();

        #region Core system
        public void Update(List<ActionItem> listActionItem)
        {
            //step1: get all actions on system 
            var listActionDB = getAllFromDB();

            //step2: update old action
            foreach (var actionDB in listActionDB)
            {
                var actionItem = getActionItem(listActionItem, actionDB.Code, actionDB.ModuleCode, actionDB.FunctionCode);
                if (actionItem != null)
                {
                    update(actionItem, actionDB);
                }
                else
                {
                    actionDB.IsDelete = true;
                }
            }

            //step3: insert new action
            foreach (var action in listActionItem)
            {
                insert(action);
            }

            ActionDA.Save();
        }
        public void Cancel(List<ActionItem> listActionItem)
        {
            //step1: get all actions on system 
            var listActionDB = getAllFromDB();
            //step2: cacel action
            foreach (var actionDB in listActionDB)
            {
                var functionItem = getActionItem(listActionItem, actionDB.Code, actionDB.ModuleCode, actionDB.FunctionCode);
                if (functionItem != null)
                {
                    actionDB.IsDelete = true;
                }
            }
            ActionDA.Save();
        }
        private List<CenterAction> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var actions = ActionDA.GetQuery()
                .Where(m => m.SystemCode == systemCode)
                .ToList();
            return actions;
        }
        private ActionItem getActionItem(List<ActionItem> actions, string actionCode, string moduleCode, string functionCode)
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
        private void insert(ActionItem item)
        {
            var action = new CenterAction()
            {
                SystemCode = BaseSystem.SystemCode,
                ModuleCode = item.ModuleCode,
                FunctionCode = item.FunctionCode,
                Code = item.Code,
                Name = item.Name,
                IsActive = true,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            ActionDA.Insert(action);
        }
        private void update(ActionItem source, CenterAction target)
        {
            target.Name = source.Name;
            target.IsDelete = false;
            target.UpdateAt = DateTime.Now;
            target.UpdateBy = source.UpdateBy;
        }
        #endregion
       
        public List<ActionItem> GetAll(int Page, int Limit, out int totalCount, string moduleCode, string functionCode)
        {
            var results = ActionDA.GetQuery(a => a.ModuleCode == moduleCode && a.FunctionCode == functionCode && a.IsActive && !a.IsDelete)
                            .Select(a => new ActionItem()
                            {
                                ID = a.ID,
                                Code = a.Code,
                                Name = a.Name,
                            })
                            .OrderBy(i=>i.ID)
                            .Page(Page, Limit,out totalCount)
                            .ToList();
            return results;
        }
        public List<ActionItem> GetActions(string moduleCode, string functionCode)
        {
            var actions = ActionDA.GetQuery()
                            .Where(a => a.ModuleCode == moduleCode && a.FunctionCode == functionCode && a.IsActive)
                            .Select(a => new ActionItem()
                            {
                                ID = a.ID,
                                Code = a.Code,
                            }).ToList();
            return actions;
        }
    }
}
