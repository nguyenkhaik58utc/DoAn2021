using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iDAS.Core
{
    /// <summary>
    /// Reflection Help
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// Get Attribute of any Type
        /// </summary>
        private TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return (TAttribute)type.GetCustomAttribute(typeof(TAttribute));
        }

        /// <summary>
        /// Get Assembly is calling this function
        /// </summary>
        public static Assembly GetAssembly()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return assembly;
        }

        /// <summary>
        /// Get all methods public of any class
        /// </summary>
        public static IEnumerable<MethodInfo> GetMethods<TClass>(Type type) where TClass : class
        {
            var methods = (from m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                           where m.ReturnType == typeof(TClass)
                           select m).Distinct();
            return methods;
        }

        /// <summary>
        /// Get all types is subclass of any class in Assembly
        /// </summary>
        public static IEnumerable<Type> GetTypes<TClass>(Assembly assembly) where TClass : class
        {
            var types = from type in assembly.GetExportedTypes()
                        where type.IsSubclassOf(typeof(TClass))
                        select type;
            return types;
        }

        /// <summary>
        /// Create Instance Object for generic Type
        /// </summary>
        public static TClass CreateInstance<TClass>(Type type) where TClass : class
        {
            return Activator.CreateInstance(type) as TClass;
        }

        /// <summary>
        /// Create Instance Object for generic Type
        /// </summary>
        public static TClass CreateInstance<TClass>() where TClass : class
        {
            return Activator.CreateInstance(typeof(TClass)) as TClass;
        }

        /// <summary>
        /// Create Instance Object 
        /// </summary>
        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
