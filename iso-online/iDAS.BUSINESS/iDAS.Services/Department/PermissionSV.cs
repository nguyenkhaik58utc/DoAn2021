using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    /// <summary>
    /// Permission Service - 07/2015
    /// </summary>
    public class PermissionSV : BaseService
    {
        HumanPermissionDA permissionDA = new HumanPermissionDA();

        private IEnumerable<BusinessFunction> getChildFunctions(IEnumerable<BusinessFunction> query, string code)
        {
            var listGet = query.Where(i => i.ParentCode == code);
            var listFirst = query.Where(i => i.Code == code);
            return listFirst.Concat(listGet.SelectMany(i => getChildFunctions(query, i.Code)));
        }
        private IEnumerable<BusinessFunction> getParentFunctions(IEnumerable<BusinessFunction> query, string code)
        {
            var listGet = query.Where(i => i.Code == code);
            return listGet.Concat(listGet.SelectMany(i => getParentFunctions(query, i.ParentCode)));
        }
        public bool CheckPermission(string moduleCode, string functionCode, string actionCode, int roleId)
        {
            var check = permissionDA.GetQuery()
                        .Where(i => i.ModuleCode == moduleCode)
                        .Where(i => i.FunctionCode == functionCode)
                        .Where(i => i.ActionCode == actionCode)
                        .Where(i => i.HumanRoleID == roleId)
                        .Count() > 0;
            return check;
        }
        public bool CheckPermission(string moduleCode, string functionCode, string actionCode, List<int> roleIds)
        {
            var check = permissionDA.GetQuery()
                        .Where(i => i.ModuleCode == moduleCode)
                        .Where(i => i.FunctionCode == functionCode)
                        .Where(i => i.ActionCode == actionCode)
                        .Where(i => roleIds.Contains(i.HumanRoleID))
                        .Count() > 0;
            return check;
        }
        public bool CheckPermission(string moduleCode, string functionCode, string actionCode, List<int> roleIds, int departmentId)
        {
            var check = permissionDA.GetQuery()
                        .Where(i => i.ModuleCode == moduleCode)
                        .Where(i => i.FunctionCode == functionCode)
                        .Where(i => i.ActionCode == actionCode)
                        .Where(i => roleIds.Contains(i.HumanRoleID) && i.HumanRole.HumanDepartment.ID == departmentId)
                        .Count() > 0;
            return check;
        }
        public List<string> GetActionCodes(string moduleCode,string functionCode, List<int> roleIds)
        {
            var actionCodes = permissionDA.GetQuery()
                                .Where(i => i.ModuleCode == moduleCode)
                                .Where(i => i.FunctionCode == functionCode)
                                .Where(i => roleIds.Contains(i.HumanRoleID))
                                .Select(i => i.ActionCode)
                                .Distinct()
                                .ToList();
            return actionCodes;
        }
        public List<BusinessModuleItem> GetPermissionModules(int roleID) {
            var dbo = permissionDA.Repository;
            var systemCode = BaseSystem.SystemCode;
            var modulesCodes = dbo.HumanPermissions
                            .Where(i => i.HumanRoleID == roleID)
                            .Select(i => i.ModuleCode)
                            .Distinct();
            var modules = dbo.BusinessModules
                            .Where(m => m.SystemCode == systemCode)
                            .Where(m => m.IsActive)
                            .Where(m => !m.IsDelete)
                            .OrderBy(m => m.Position)
                            .Select(m => new BusinessModuleItem()
                            {
                                Code = m.Code,
                                Name = m.Name,
                                Icon = m.Icon,
                                IsPermission = modulesCodes.Contains(m.Code),
                            })
                            .ToList();
            return modules;
        }
        public List<BusinessFunctionItem> GetPermissionFunctions(string code, int roleID, bool isGroup = false)
        {
            var dbo = permissionDA.Repository;
            var systemCode = BaseSystem.SystemCode;
            var moduleCode = code;
            var parentCode = string.Empty;
            if (isGroup) {
                var arrCode = code.Split(',');
                moduleCode = arrCode[0];
                parentCode = arrCode.Count() > 1 ? arrCode[1] : string.Empty;
            }
            var functionCodes = dbo.HumanPermissions
                                .Where(i => i.HumanRoleID == roleID)
                                .Where(i => i.ModuleCode == moduleCode)
                                .Select(i => i.FunctionCode)
                                .Distinct();
            var groupCodes = new List<string>();
            foreach (var functionCode in functionCodes)
            {
                groupCodes.AddRange(getParentFunctions(dbo.BusinessFunctions, functionCode)
                                    .Where(i => i.SystemCode == systemCode)
                                    .Where(i => i.ModuleCode == moduleCode)
                                    .Where(i => i.IsActive)
                                    .Where(i => !i.IsDelete)
                                    .Where(i => i.IsGroup)
                                    .Where(i => !groupCodes.Contains(i.Code))
                                    .Select(i => i.Code));
            }
            var functions = dbo.BusinessFunctions
                            .Where(i => i.SystemCode == systemCode)
                            .Where(i => i.ModuleCode == moduleCode)
                            .Where(i => i.ParentCode == parentCode)
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDelete)
                            .OrderBy(i => i.ParentCode)
                            .ThenBy(i => i.Position)
                            .ThenByDescending(i => i.IsGroup)
                            .Select(i => new BusinessFunctionItem()
                            {
                                ModuleCode = i.ModuleCode,
                                ParentCode = i.ParentCode,
                                IsGroup = i.IsGroup,
                                Code = i.Code,
                                Name = i.Name,
                                Icon = i.Icon,
                                IsPermission = functionCodes.Contains(i.Code) || groupCodes.Contains(i.Code),
                            })
                            .ToList();
            return functions;
        }
        public List<BusinessActionItem> GetPermissionActions(string code, int roleID) {
            var dbo = permissionDA.Repository;
            var systemCode = BaseSystem.SystemCode;
            var arrCode = code.Split(',');
            var moduleCode = arrCode[0];
            var functionCode = arrCode[1];
            var actionCodes = permissionDA.GetQuery()
                            .Where(i => i.ModuleCode == moduleCode)
                            .Where(i => i.FunctionCode == functionCode)
                            .Where(i => i.HumanRoleID == roleID)
                            .Select(i => i.ActionCode);
            var actions = dbo.BusinessActions
                            .Where(i => i.SystemCode == systemCode)
                            .Where(i => i.ModuleCode == moduleCode)
                            .Where(i => i.FunctionCode == functionCode)
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDelete)
                            .Select(i => new BusinessActionItem()
                            {
                                ID = i.ID,
                                Code = i.Code,
                                Name = i.Name,
                                IsPermission = actionCodes.Contains(i.Code),
                            })
                            .ToList();
            return actions;
        }
        public void SetPermissionModule(string code, int roleID, bool isPermission)
        {
            var moduleCode = code;
            var dbo = permissionDA.Repository;
            var functionCodes = dbo.BusinessFunctions
                                .Where(i => i.ModuleCode == moduleCode)
                                .Where(i => i.IsActive)
                                .Where(i => !i.IsDelete)
                                .Where(i => !i.IsGroup)
                                .Select(i => i.Code)
                                .ToList();
            foreach (var functionCode in functionCodes)
            {
                var newCode = moduleCode + "," + functionCode;
                SetPermissionFunction(newCode, roleID, isPermission);
            }
        }
        public void SetPermissionGroup(string code, int roleID, bool isPermission)
        {
            var dbo = permissionDA.Repository;
            var systemCode = BaseSystem.SystemCode;
            var arrCode = code.Split(',');
            var moduleCode = arrCode[0];
            var functionCode = arrCode[1];
            var functions = getChildFunctions(dbo.BusinessFunctions, functionCode)
                            .Where(i => i.ModuleCode == moduleCode)
                            .Where(i => i.SystemCode == systemCode)
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDelete)
                            .Where(i => !i.IsGroup)
                            .Select(i => i.Code)
                            .ToList();
            foreach (var function in functions)
            {
                code = moduleCode + ',' + function;
                SetPermissionFunction(code, roleID, isPermission);
            }
        }
        public void SetPermissionFunction(string code,int roleID, bool isPermission) {
            var arrCode = code.Split(',');
            var moduleCode = arrCode[0];
            var functionCode = arrCode[1];
            if (isPermission)
            {
                var dbo = permissionDA.Repository;
                var actionCodes = dbo.BusinessActions
                              .Where(i => i.ModuleCode == moduleCode)
                              .Where(i => i.FunctionCode == functionCode)
                              .Where(i => !i.IsDelete)
                              .Where(i => i.IsActive)
                              .Select(i => i.Code)
                              .ToList();
                foreach (var actionCode in actionCodes) {
                    var permission = new HumanPermission()
                    {
                        ModuleCode = moduleCode,
                        FunctionCode = functionCode,
                        ActionCode = actionCode,
                        HumanRoleID = roleID,
                    };
                    permissionDA.Insert(permission);
                }
                permissionDA.Save();
            }
            else {
                var permissions = permissionDA.GetQuery()
                                    .Where(i => i.ModuleCode == moduleCode)
                                    .Where(i => i.FunctionCode == functionCode)
                                    .Where(i => i.HumanRoleID == roleID)
                                    .ToList();
                permissionDA.DeleteRange(permissions);
                permissionDA.Save();
            }          
        }
        public void SetPermissionAction(string code, int roleID, string actionCode, bool isPermission) {
            var arrCode = code.Split(',');
            var moduleCode = arrCode[0];
            var functionCode = arrCode[1];
            if (isPermission)
            {
                var permission = new HumanPermission()
                {
                    ModuleCode = moduleCode,
                    FunctionCode = functionCode,
                    ActionCode = actionCode,
                    HumanRoleID = roleID,
                };
                permissionDA.Insert(permission);
                permissionDA.Save();
            }
            else {
                var permission = permissionDA.GetQuery()
                                    .Where(i => i.ModuleCode == moduleCode)
                                    .Where(i => i.FunctionCode == functionCode)
                                    .Where(i => i.ActionCode == actionCode)
                                    .Where(i => i.HumanRoleID == roleID)
                                    .FirstOrDefault();
                permissionDA.Delete(permission);
                permissionDA.Save();
            }
        }
        public void SetPermissionAll(int roleID, bool isPermission)
        { 
            var dbo = permissionDA.Repository;
            var moduleCodes = dbo.BusinessModules
                                .Where(i => i.IsActive)
                                .Where(i => !i.IsDelete)
                                .Select(i => i.Code)
                                .ToList();
            foreach(var moduleCode in moduleCodes){
                SetPermissionModule(moduleCode, roleID, isPermission);
            }
        }
        public void SetPermissionRole(string code, int roleID, bool isPermission)
        {
            var systemCode = BaseSystem.SystemCode;
            var arrCode = code.Split('/');
            var moduleCode = arrCode[0];
            var functionCode = arrCode[1];
            var actionCode = arrCode[2];
            if (isPermission)
            {
                var permission = new HumanPermission()
                {
                    ModuleCode = moduleCode,
                    FunctionCode = functionCode,
                    ActionCode = actionCode,
                    HumanRoleID = roleID,
                    CreateAt = DateTime.Now,
                    CreateBy = User.ID,
                };
                permissionDA.Insert(permission);
                permissionDA.Save();
            }
            else
            {
                var permission = permissionDA.GetQuery()
                                    .Where(i => i.ModuleCode == moduleCode)
                                    .Where(i => i.FunctionCode == functionCode)
                                    .Where(i => i.ActionCode == actionCode)
                                    .Where(i => i.HumanRoleID == roleID)
                                    .FirstOrDefault();
                permissionDA.Delete(permission);
                permissionDA.Save();
            }
        }
        public void SetPermissionRole(string code, int roleID, bool isPermission, bool isModule, bool isGroup, bool isFunction) {
            if (isModule) {
                SetPermissionModule(code, roleID, isPermission);
            }
            else
            {
                if (isGroup)
                {
                    SetPermissionGroup(code, roleID, isPermission);
                }
                else
                {
                    SetPermissionFunction(code, roleID, isPermission);
                }
            }
        }
        public List<object> GetPermissions(string code, int departmentID, bool isFunction)
        {
            var data = new List<object>();
            if (isFunction)
            {
                var dbo = permissionDA.Repository;
                var departmentIDs = new DepartmentSV().GetDepartmentIDs(departmentID);
                departmentIDs.Insert(0, departmentID);
                var actions = GetActions(code, isFunction);
                var arrCode = code.Split(',');
                var moduleCode = arrCode[0];
                var functionCode = arrCode[1];
                foreach (var item in departmentIDs)
                {
                    var roles = new RoleSV().GetRoles(item);
                    foreach (var role in roles)
                    {
                        var dic = new Dictionary<string, object>();
                        dic.Add("DepartmentName", role.DepartmentName.ToUpper());
                        dic.Add("RoleID", role.ID);
                        dic.Add("RoleName", role.Name);

                        for (var i = 0; i < actions.Count; i++)
                        {
                            var actionCode = actions[i].Code;
                            var permission = CheckPermission(moduleCode, functionCode, actionCode, role.ID);
                            dic.Add(actions[i].Code, permission);
                        }
                        data.Add(dic);
                    }
                }
            }
            return data;
        }
        public List<object> GetPermissions(string code, int departmentID, bool isModule, bool isGroup, bool isFunction)
        {
            var data = new List<object>();
            var dbo = permissionDA.Repository;
            var departmentIDs = new DepartmentSV().GetDepartmentIDs(departmentID);
            departmentIDs.Insert(0, departmentID);

            var arrCode = code.Split(',');
            var moduleCode = arrCode[0];
            var functions = GetFunctions(code, isModule, isGroup, isFunction);
            foreach (var item in departmentIDs)
            {
                var roles = new RoleSV().GetRoles(item);
                foreach (var role in roles)
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("RoleID", role.ID);
                    dic.Add("RoleName", role.Name);
                    dic.Add("DepartmentName", role.DepartmentName.ToUpper());
                    foreach (var functionCode in functions)
                    {
                        var actions = GetActions(code, functionCode);
                        for (var i = 0; i < actions.Count; i++)
                        {
                            var actionCode = actions[i].Code;
                            var permission = CheckPermission(moduleCode, functionCode, actionCode, role.ID);
                            var dataIndex = moduleCode + "/" + functionCode + "/" + actionCode;
                            dic.Add(dataIndex, permission);
                        }
                    }
                    data.Add(dic);
                }
            }
            return data;
        }
        

        public List<BusinessActionItem> GetActions(string code, bool isFunction)
        {
            var data = new List<BusinessActionItem>();
            if (isFunction)
            {
                var dbo = permissionDA.Repository;
                var systemCode = BaseSystem.SystemCode;
                var arrCode = code.Split(',');
                var moduleCode = arrCode[0];
                var functionCode = arrCode[1];
                data = dbo.BusinessActions
                        .Where(i => i.SystemCode == systemCode)
                        .Where(i => i.ModuleCode == moduleCode)
                        .Where(i => i.FunctionCode == functionCode)
                        .Where(i => !i.IsDelete)
                        .Select(i => new BusinessActionItem
                        {
                            Code = i.Code,
                            Name = i.Name,
                            ModuleName = dbo.BusinessModules.Where(a=>a.Code == i.ModuleCode).FirstOrDefault().Name,
                            FunctionName = dbo.BusinessFunctions.Where(a=>a.Code == i.FunctionCode && a.ModuleCode == i.ModuleCode).FirstOrDefault().Name
                        })
                        .ToList();
            }
            return data;
        }
        public List<BusinessActionItem> GetActions(string code, string functionCode)
        {
            var data = new List<BusinessActionItem>();
            var dbo = permissionDA.Repository;
            var systemCode = BaseSystem.SystemCode;
            var arrCode = code.Split(',');
            var moduleCode = arrCode[0];
            data = dbo.BusinessActions
                    .Where(i => i.SystemCode == systemCode)
                    .Where(i => i.ModuleCode == moduleCode)
                    .Where(i => i.FunctionCode == functionCode)
                    .Where(i => i.IsActive)
                    .Where(i => !i.IsDelete)
                    .Select(i => new BusinessActionItem
                    {
                        Code = i.Code,
                        Name = i.Name,
                        ModuleName = dbo.BusinessModules.Where(a => a.Code == i.ModuleCode).FirstOrDefault().Name,
                        FunctionName = dbo.BusinessFunctions.Where(a => a.Code == i.FunctionCode && a.ModuleCode == i.ModuleCode).FirstOrDefault().Name
                    })
                    .ToList();
            return data;
        }
        public List<string> GetFunctions(string code, bool isModule, bool isGroup, bool isFunction)
        {
            var dbo = permissionDA.Repository;
            var systemCode = BaseSystem.SystemCode;
            var functionCodes = new List<string>();
            if (isModule)
            {
                var moduleCode = code;
                functionCodes = dbo.BusinessFunctions
                                .Where(i => i.SystemCode == systemCode)
                                .Where(i => i.ModuleCode == moduleCode)
                                .Where(i => i.IsActive)
                                .Where(i => !i.IsDelete)
                                .Select(i => i.Code)
                                .ToList();
            }
            if (isGroup)
            {
                var arrCode = code.Split(',');
                var moduleCode = arrCode[0];
                var parentCode = arrCode[1];
                functionCodes = dbo.BusinessFunctions
                                .Where(i => i.SystemCode == systemCode)
                                .Where(i => i.ModuleCode == moduleCode)
                                .Where(i => i.ParentCode == parentCode)
                                .Where(i => i.IsActive)
                                .Where(i => !i.IsDelete)
                                .Select(i => i.Code)
                                .ToList();
            }
            if (isFunction)
            {
                var arrCode = code.Split(',');
                var moduleCode = arrCode[0];
                var functionCode = arrCode[1];
                functionCodes.Add(functionCode);
            }
            return functionCodes;
        }


        public object GetAuditCri_Factory(string code, int departmentId, bool isModule, bool isGroup, bool isFunction,int CritId)
        {
            var data = new List<object>();
            var dbo = permissionDA.Repository;
            var departmentIDs = new DepartmentSV().GetDepartmentIDs(departmentId);
            departmentIDs.Insert(0, departmentId);

            var arrCode = code.Split(',');
            var moduleCode = arrCode[0];
            var functions = GetFunctions(code, isModule, isGroup, isFunction);
            foreach (var item in departmentIDs)
            {
                var roles = new RoleSV().GetRoles(item);
                foreach (var role in roles)
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("RoleID", role.ID);
                    dic.Add("RoleName", role.Name);
                    dic.Add("DepartmentName", role.DepartmentName.ToUpper());
                    //dic.Add("AuditCriteRialFactiory", dbo.HumanAuditCriteriaFactories.FirstOrDefault(x=>x.HumanRoleID == role.ID && x.HumanAuditCriteriaID == CritId)!=null?dbo.HumanAuditCriteriaFactories.FirstOrDefault(x=>x.HumanRoleID == role.ID).Factory:1);

                    foreach (var functionCode in functions)
                    {
                        var actions = GetActions(code, functionCode);
                        for (var i = 0; i < actions.Count; i++)
                        {
                            var actionCode = actions[i].Code;
                            var permission = CheckPermission(moduleCode, functionCode, actionCode, role.ID);
                            var dataIndex = moduleCode + "/" + functionCode + "/" + actionCode;
                            dic.Add(dataIndex, permission);
                        }
                    }
                    data.Add(dic);
                }
            }
            return data;
        }
    }
}
