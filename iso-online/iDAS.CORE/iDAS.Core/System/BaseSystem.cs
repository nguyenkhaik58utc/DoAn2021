using System.Collections.Generic;

namespace iDAS.Core
{
    /// <summary>
    /// Base System
    /// </summary>
    public class BaseSystem
    {
        public static string SystemCode
        {
            get
            {
                return BaseConfig.SystemCode;
            }
        }

        /// <summary>
        /// Get auto all modules of system
        /// </summary>
        public List<TClass> GetAutoModules<TClass>() where TClass : IModule
        {
            var manager = new BaseModule();
            return manager.GetModules<TClass>(BaseConfig.System);
        }

        /// <summary>
        /// Get auto all functions of system
        /// </summary>
        public List<TClass> GetAutoFunctions<TClass>(IEnumerable<IModule> modules) where TClass : IFunction
        {
            var manager = new BaseFunction();
            return manager.GetFunctions<TClass>(BaseConfig.System, modules);
        }

        /// <summary>
        /// Get auto all actions of system
        /// </summary>
        public List<TClass> GetAutoActions<TClass>(IEnumerable<IFunction> functions) where TClass : IAction
        {
            var manager = new BaseAction();
            return manager.GetActions<TClass>(functions);
        }
    }
}
