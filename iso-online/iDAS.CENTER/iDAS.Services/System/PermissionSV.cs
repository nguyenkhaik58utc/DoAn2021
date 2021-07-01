using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;
namespace iDAS.Services
{
    public class PermissionSV
    {
        PermissionDA permissionDA = new PermissionDA();

        public bool CheckPermission(string moduleCode, string functionCode, string actionCode, List<int> roleIds)
        {
            var dbo = permissionDA.Repository;
            var lstID = dbo.CenterActions.Where(t=>t.Code==actionCode).Select(t=>t.ID).Distinct().ToList();
            var permission = permissionDA.GetQuery()
                                .Where(i => lstID.Contains(i.ActionID))
                                .Where(i => roleIds.Contains(i.SystemRoleID.HasValue?i.SystemRoleID.Value:0))
                                .Count();
            var check = permission > 0;
            return check;
        }
        
        public PermissionItem GetPermission(int roleId, int actionId) {
            var permission = permissionDA.GetQuery(filter: p => p.SystemRoleID == roleId && p.ActionID == actionId).FirstOrDefault();
            if (permission != null)
            {
                return new PermissionItem()
                {
                    ID = permission.ID,
                    RoleID = permission.SystemRoleID.Value,
                    ActionID = permission.ActionID,
                    CreateAt = permission.CreateAt,
                    CreateBy = permission.CreateBy,
                };
            }
            return null;
        }

        public List<int> GetActionIds(List<int> roleIds) {
            var actionIds = permissionDA.GetQuery(filter: p => roleIds.Contains(p.SystemRoleID.Value)).Select(p => p.ActionID).Distinct().ToList();
            return actionIds;
        }

        public void Insert(PermissionItem item, int userID)
        {
            var permission = new SystemPermission()
            {
                ID = item.ID,
                SystemRoleID = item.RoleID,
                ActionID = item.ActionID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            permissionDA.Insert(permission);
            permissionDA.Save();
        }

        public void Delete(int id)
        {
            permissionDA.Delete(id);
            permissionDA.Save();
        }

        public void DeleteByActionId(int actionId) {
            var source = permissionDA.GetQuery().Where(p => p.ActionID == actionId).Select(p => p.ID).ToList();
            var permissionIds = new List<object>();
            foreach (var item in source){
                permissionIds.Add(item);
            }
            permissionDA.DeleteRange(permissionIds);
            permissionDA.Save();
        }
    }
}
