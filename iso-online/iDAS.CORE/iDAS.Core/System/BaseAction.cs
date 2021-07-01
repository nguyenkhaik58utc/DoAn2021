using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace iDAS.Core
{
    /// <summary>
    /// Action System
    /// </summary>
    public class BaseAction
    {
        /// <summary>
        /// Check Action System is exist or not
        /// </summary>
        private bool checkIsActionExist<TClass>(IEnumerable<TClass> actions, string moduleCode, string functionCode, string actionCode) where TClass : IAction
        {
            var check = actions
                        .Where(i => i.ModuleCode == moduleCode)
                        .Where(i => i.FunctionCode == functionCode)
                        .Where(i => i.Code == actionCode)
                        .Count() > 0;
            return check;
        }
        /// <summary>
        ///  Get all Actions of System
        /// </summary>
        public List<TClass> GetActions<TClass>(IEnumerable<IFunction> functions) where TClass : IAction
        {
            //init new list actions
            List<TClass> actions = new List<TClass>();

            foreach (var function in functions)
            {
                //get code of module
                var mCode = function.ModuleCode;

                //get code of function
                var fCode = function.Code;

                //get all actions
                var types = ReflectionHelper.GetMethods<ActionResult>(function.Type);

                foreach (var type in types)
                {
                    //get info of action
                    var info = type.GetCustomAttribute<BaseSystemAttribute>();

                    //if action not right of system then go to other action
                    if (info == null) continue;

                    //get code of action
                    var aCode = type.Name;

                    //if action is exist then go to other action
                    if (checkIsActionExist(actions, mCode, fCode, aCode)) continue;

                    //init new action
                    TClass action = (TClass)ReflectionHelper.CreateInstance(typeof(TClass));

                    //set properties value for new action
                    action.Code = aCode ?? string.Empty;
                    action.Name = !string.IsNullOrEmpty(info.Name) ? info.Name : aCode;
                    action.ModuleCode = mCode;
                    action.FunctionCode = fCode;
                    action.IsActive = info.IsActive;

                    //add action to list actions
                    actions.Add(action);
                }
            }
            return actions;
        }
    }
}
