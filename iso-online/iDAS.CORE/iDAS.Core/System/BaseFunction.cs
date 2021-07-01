using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iDAS.Core
{
    /// <summary>
    /// Function System
    /// </summary>
    public class BaseFunction
    {
        /// <summary>
        /// Check Module System by module code
        /// </summary>
        private bool checkIsModuleSystem(IEnumerable<IModule> modules, string moduleCode)
        {
            var check = modules.Where(i => i.Code == moduleCode).Count() > 0;
            return check;
        }

        /// <summary>
        /// Get all Functions of System
        /// </summary>
        public List<TClass> GetFunctions<TClass>(Assembly assembly, IEnumerable<IModule> modules) where TClass : IFunction
        {
            //init new list functions
            List<TClass> functions = new List<TClass>();

            //get all functions
            IEnumerable<Type> types = ReflectionHelper.GetTypes<BaseController>(assembly);

            foreach (var type in types)
            {
                //get info of function
                var info = type.GetCustomAttribute<BaseSystemAttribute>();

                //if function not right of system then go to other function
                if (info == null) continue;

                //get instance controller of type function
                var controller = ReflectionHelper.CreateInstance<BaseController>(type);

                //get code of module
                var mCode = controller.GetAreaNameController();

                //get code of function
                var fCode = controller.GetNameController();

                //if module of function not right of system then go to other function
                if (!checkIsModuleSystem(modules, mCode)) continue;

                //init new function
                TClass function = (TClass)ReflectionHelper.CreateInstance(typeof(TClass));

                //set properties value for new function
                function.Code = fCode ?? string.Empty;
                function.Name = !string.IsNullOrEmpty(info.Name) ? info.Name : fCode;
                function.ModuleCode = mCode;
                function.ParentCode = info.Parent;
                function.IsActive = info.IsActive;
                function.IsShow = info.IsShow;
                function.Position = info.Position;
                function.IsGroup = info.IsGroup;
                function.Url = !info.IsGroup ? controller.GetActionUrlController(info.StartAction) : string.Empty;
                function.Type = type;
                function.Icon = info.Icon;

                //add function to list functions
                functions.Add(function);
            }

            return functions;
        }
    }
}
