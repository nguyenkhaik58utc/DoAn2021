using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DAL;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class SystemPermissionService
    {
        SystemPermissionDA permissionDA = new SystemPermissionDA();

        public SystemPermissionItem GetPermission(int roleId, int actionId) {
            var permission = permissionDA.GetQuery(filter: p => p.RoleID == roleId && p.ActionID == actionId).FirstOrDefault();
            if (permission != null)
            {
                return new SystemPermissionItem()
                {
                    ID = permission.ID,
                    RoleID = permission.RoleID,
                    ActionID = permission.ActionID,
                    CreateAt = permission.CreateAt,
                    CreateBy = permission.CreateBy,
                };
            }
            return null;
        }

        public List<int> GetActionIds(List<int> roleIds) {
            var actionIds = permissionDA.GetQuery(filter: p => roleIds.Contains(p.RoleID)).Select(p => p.ActionID).Distinct().ToList();
            return actionIds;
        }

        public void Insert(SystemPermissionItem item, int userID)
        {
            var permission = new SystemPermission()
            {
                ID = item.ID,
                RoleID = item.RoleID,
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
