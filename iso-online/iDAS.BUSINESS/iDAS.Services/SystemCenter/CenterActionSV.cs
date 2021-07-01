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
    public class CenterActionSV
    {
        private CenterActionDA actionDA = new CenterActionDA();

        public List<CenterActionItem> GetActions(string moduleCode, string functionCode)
        {
            var actions = actionDA.GetQuery()
                            .Where(a => a.ModuleCode == moduleCode && a.FunctionCode == functionCode && a.IsActive)
                            .Select(a => new CenterActionItem()
                            {
                                ID = a.ID,
                                Code = a.Code,
                            }).ToList();
            return actions;
        }

        #region Core system
        public void Update(List<CenterActionItem> listActionItem)
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

            actionDA.Save();
        }
        private List<CenterAction> getAllFromDB()
        {
            var systemCode = BaseSystem.SystemCode;
            var actions = actionDA.GetQuery()
                .Where(i => i.SystemCode == systemCode)
                .ToList();
            return actions;
        }
        private CenterActionItem getActionItem(List<CenterActionItem> actions, string actionCode, string moduleCode, string functionCode)
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
        private void insert(CenterActionItem item)
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
            actionDA.Insert(action);
        }
        private void update(CenterActionItem source, CenterAction target)
        {
            target.Name = source.Name;
            target.IsDelete = false;
            target.UpdateAt = DateTime.Now;
            target.UpdateBy = source.UpdateBy;
        }
        #endregion
    }
}
