using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DAL;
using System.Reflection;
using iDAS.Items;

namespace iDAS.Services
{
    [BaseSystem]
    public class SystemService
    {
        public void UpdateSystemAuto(int userID)
        {
            var system = new BaseSystem();

            var modules = system.GetAutoModules<SystemModuleItem>();
            var functions = system.GetAutoFunctions<SystemFunctionItem>(modules);
            var actions = system.GetAutoActions<SystemActionItem>(functions);

            InsertModule(modules, userID);
            InsertFunction(functions, userID);
            InsertAction(actions, userID);
        }

        //public void RestoreSystemAuto(int userID)
        //{
        //    var modules = ManagerSystem.Instance.GetAutoModules<SystemModuleItem>();
        //    var functions = ManagerSystem.Instance.GetAutoFunctions<SystemFunctionItem>(modules);
        //    var actions = ManagerSystem.Instance.GetAutoActions<SystemActionItem>(functions);

        //    UpdateModule(modules, userID);
        //    UpdateFunction(functions, userID);
        //    UpdateAction(actions, userID);
        //}

        public void InsertModule(List<SystemModuleItem> modules, int userID)
        {
            var service = new SystemModuleService();
            //service.SetStatusModules((byte)SystemCommon.Status.Obsolete);

            foreach (var item in modules)
            {
                var module = service.Get(item.Code);
                if (module != null)
                {
                    //module.Status = (byte)SystemCommon.Status.Using;
                    service.Update(module, userID);
                }
                else
                {
                    //item.Status = (byte)SystemCommon.Status.New;
                    service.Insert(item, userID);
                }
            }
        }

        public void InsertFunction(List<SystemFunctionItem> functions, int userID)
        {
            var service = new SystemFunctionService();
            //service.SetStatusFunctions((byte)SystemCommon.Status.Obsolete);

            foreach (var item in functions)
            {
                var function = service.Get(item.ModuleCode, item.Code);
                if (function != null)
                {
                    //function.Status = (byte)SystemCommon.Status.Using;
                    service.Update(function, userID);
                }
                else
                {
                    //item.Status = (byte)SystemCommon.Status.New;
                    service.Insert(item, userID);
                }
            }
        }

        public void InsertAction(List<SystemActionItem> actions, int userID)
        {
            var service = new SystemActionService();
            //service.SetStatusActions((byte)SystemCommon.Status.Obsolete);

            foreach (var item in actions)
            {
                var action = service.Get(item.ModuleCode, item.FunctionCode, item.Code);
                if (action != null)
                {
                    //action.Status = (byte)SystemCommon.Status.Using;
                    service.Update(action, userID);
                }
                else
                {
                    //item.Status = (byte)SystemCommon.Status.New;
                    service.Insert(item, userID);
                }
            }
        }

        public void UpdateModule(List<SystemModuleItem> modules, int userID)
        {
            var service = new SystemModuleService();
            //service.SetStatusModules((byte)SystemCommon.Status.Obsolete);

            foreach (var item in modules)
            {
                var module = service.Get(item.Code);
                if (module != null)
                {
                    module.Name = item.Name;
                    module.IconName = item.IconName;
                    module.Description = item.Description;
                    module.IsActive = item.IsActive;
                    module.IsShow = item.IsShow;
                    module.Position = item.Position;
                    //module.Status = (byte)SystemCommon.Status.Using;

                    service.Update(module, userID);
                }
            }
        }

        public void UpdateFunction(List<SystemFunctionItem> functions, int userID)
        {
            var service = new SystemFunctionService();
            //service.SetStatusFunctions((byte)SystemCommon.Status.Obsolete);

            foreach (var item in functions)
            {
                var function = service.Get(item.ModuleCode, item.Code);
                if (function != null)
                {
                    function.Name = item.Name;
                    function.IconName = item.IconName;
                    function.Description = item.Description;
                    function.IsActive = item.IsActive;
                    function.IsShow = item.IsShow;
                    //function.Status = (byte)SystemCommon.Status.Using;
                    function.ParentCode = item.ParentCode;
                    function.Position = item.Position;

                    service.Update(function, userID);
                }
            }
        }

        public void UpdateAction(List<SystemActionItem> actions, int userID)
        {
            var service = new SystemActionService();
            //service.SetStatusActions((byte)SystemCommon.Status.Obsolete);

            foreach (var item in actions)
            {
                var action = service.Get(item.ModuleCode, item.FunctionCode, item.Code);
                if (action != null)
                {
                    action.Name = item.Name;
                    action.IconName = item.IconName;
                    action.Description = item.Description;
                    action.IsActive = item.IsActive;
                    action.IsShow = item.IsShow;
                    action.Url = item.Url;
                    action.Position = item.Position;
                    action.Permission = item.Permission;
                    //action.Status = (byte)SystemCommon.Status.Using;

                    service.Update(action, userID);
                }
            }
        }
    }
}
