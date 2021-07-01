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
    public class OrganizationSV
    {
        OrganizationDA organizationDA = new OrganizationDA();

        public OrganizationItem GetOrganization(int userId, int roleId)
        {
            var organization = organizationDA.GetQuery().Where(o => o.SystemUserID == userId && o.SystemRoleID == roleId).FirstOrDefault();
            if (organization != null)
            {
                return new OrganizationItem()
                {
                    ID = organization.ID,
                    RoleID = organization.SystemRoleID.Value,
                    UserID = organization.SystemUserID.Value,
                    CreateAt = organization.CreateAt,
                    CreateBy = organization.CreateBy,
                };
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public List<RoleItem> GetAllPositionAndGroupByUserID(int? userId)
        {
            try
            {
                var result = organizationDA.GetQuery().Where(o => o.SystemUserID == userId).Select(item =>
                    new RoleItem()
                    {
                        Name = item.SystemRole.Name,
                        GroupName = item.SystemRole.SystemDepartment.Name

                    }
                    ).ToList();
                return result;
            }
            catch
            {
                return null;
            }

        }

        public List<int> GetRoleIds(int userId)
        {
            var roleIds = organizationDA.GetQuery(filter: o => o.SystemUserID == userId).Select(o => o.SystemRoleID.Value).ToList();
            return roleIds;
        }

        public void Insert(OrganizationItem item, int userID)
        {
            var organization = new SystemOrganization()
            {
                ID = item.ID,
                SystemRoleID = item.RoleID,
                SystemUserID = item.UserID,
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
