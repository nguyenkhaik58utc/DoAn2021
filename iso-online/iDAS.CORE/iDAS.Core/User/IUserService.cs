using System.Collections.Generic;

namespace iDAS.Core
{
    /// <summary>
    /// Interface User Service
    /// </summary>
    public interface IUserService
    {
        UserPrincipal GetUserPrincipal(IUserLogin user);
        Database GetDatabaseByCode(string code);
        bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds);
        bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds, int departmentId);
    }
}
