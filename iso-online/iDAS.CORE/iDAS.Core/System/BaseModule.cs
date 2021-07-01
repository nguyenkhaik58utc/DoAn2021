using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace iDAS.Core
{
    /// <summary>
    /// Module System
    /// </summary>
    public class BaseModule
    {
        /// <summary>
        /// Get all Module of System
        /// </summary>
        public List<TClass> GetModules<TClass>(Assembly assembly) where TClass : IModule
        {
            //init new list modules
            List<TClass> modules = new List<TClass>();

            //get all modules
            IEnumerable<Type> types = ReflectionHelper.GetTypes<AreaRegistration>(assembly);

            foreach (var type in types)
            {
                //get info of module
                var info = type.GetCustomAttribute<BaseSystemAttribute>();

                //if module not right of system then go to other module
                if (info == null) continue;

                //get code of module
                var code = ReflectionHelper.CreateInstance<AreaRegistration>(type).AreaName;

                //init new module
                TClass module = (TClass)Activator.CreateInstance(typeof(TClass));

                //set properties value for new module
                module.Code = code ?? string.Empty;
                module.Name = !string.IsNullOrEmpty(info.Name) ? info.Name : code;
                module.IsActive = info.IsActive;
                module.IsShow = info.IsShow;
                module.Position = info.Position;
                module.Icon = info.Icon;

                //add module to list modules
                modules.Add(module);
            }

            return modules;
        }
    }
}
