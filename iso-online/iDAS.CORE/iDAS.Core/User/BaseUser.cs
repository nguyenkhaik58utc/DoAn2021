using System.Collections.Generic;

namespace iDAS.Core
{
    /// <summary>
    /// Base User
    /// </summary>
    public class BaseUser
    {
        private static IUserService _UserService
        {
            get
            {
                return ReflectionHelper.CreateInstance<IUserService>(BaseConfig.UserService);
            }
        }

        /// <summary>
        /// Get user principal by user login
        /// </summary>
        /// <param name="user">user login</param>
        /// <returns>UserPrincipal</returns>
        public static UserPrincipal GetUserPrincipal(IUserLogin user)
        {
            return _UserService.GetUserPrincipal(user);
        }

        /// <summary>
        /// Set Database by Code of System
        /// </summary>
        /// <param name="code">string</param>
        /// <returns>Database</returns>
        public static void SetDatabaseByCode(string code)
        {
            Database database = null;
            if (string.IsNullOrEmpty(code))
            {
                database = BaseDatabase.DatabaseCenter;
            }
            else
            {
                database = _UserService.GetDatabaseByCode(code);
            }
            BaseDatabase.DatabaseByCode = database;
        }

        /// <summary>
        /// Check permission of user principal
        /// </summary>
        /// <param name="moduleCode">module code</param>
        /// <param name="functionCode">function code</param>
        /// <param name="roleIds">roles</param>
        /// <returns>list action code</returns>
        public static bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds)
        {
            bool check = false;
            check = _UserService.CheckPermissions(moduleCode, functionCode, actionCode, roleIds);
            return check;
        }

        public static bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds, int departmentId)
        {
            return _UserService.CheckPermissions(moduleCode, functionCode, actionCode, roleIds, departmentId);
        }

    }
}
