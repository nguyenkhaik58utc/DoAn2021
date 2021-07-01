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
    public class SystemOrganizationService
    {
        SystemOrganizationDA organizationDA = new SystemOrganizationDA();

        public SystemOrganizationItem GetOrganization(int userId, int roleId){
            var organization = organizationDA.GetQuery().Where(o => o.UserID == userId && o.RoleID == roleId).FirstOrDefault();
            if (organization != null) {
                return new SystemOrganizationItem()
                {
                    ID = organization.ID,
                    RoleID = organization.RoleID,
                    UserID = organization.UserID,
                    CreateAt = organization.CreateAt,
                    CreateBy = organization.CreateBy,
                };
            }
            return null;
        }

        public List<int> GetRoleIds(int userId) {
            var roleIds = organizationDA.GetQuery(filter: o => o.UserID == userId).Select(o => o.RoleID).ToList();
            return roleIds;
        }

        public void Insert(SystemOrganizationItem item, int userID)
        {
            var organization = new SystemOrganization()
            {   
                ID  = item.ID,
                RoleID = item.RoleID,
                UserID = item.UserID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            organizationDA.Insert(organization);
            organizationDA.Save();
        }

        public void Delete(int id)
        {
            organizationDA.Delete(id);
            organizationDA.Save();
        }
    }
}
