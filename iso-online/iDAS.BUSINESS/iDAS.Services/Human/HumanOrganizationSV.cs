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
    public class HumanOrganizationSV
    {
        HumanOrganizationDA organizationDA = new HumanOrganizationDA();
        RoleSV roleService = new RoleSV();

        public HumanOrganizationItem GetOrganization(int userId, int roleId)
        {
            var organization = organizationDA.GetQuery().Where(o => o.HumanEmployeeID == userId && o.HumanRoleID == roleId).FirstOrDefault();
            if (organization != null)
            {
                return new HumanOrganizationItem()
                {
                    ID = organization.ID,
                    RoleID = organization.HumanRoleID,
                    UserID = organization.HumanEmployeeID,
                    CreateAt = organization.CreateAt,
                    CreateBy = organization.CreateBy,
                };
            }
            return null;
        }
        public void DeleteByDepartmentIdAndEmployeesId(string strEmployessId, int roleId, int departmentId)
        {
            List<int> lstroleofdepartment = roleService.GetIDByDepartmentId(departmentId);
            var employessIds = strEmployessId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            if (employessIds.Count > 0)
            {
                foreach (var item in employessIds)
                {
                    var lst = organizationDA.GetQuery(t => t.HumanEmployeeID == (int)item && lstroleofdepartment.Contains(t.HumanRoleID)).ToList();
                    if (lst.Count > 0)
                    {
                        organizationDA.DeleteRange(lst);
                    }
                }
            }
            organizationDA.Save();
        }
        public void InsertByEmployeesIdAndRoleId(string strEmployessId, int roleId, int userId)
        {
            var employessIds = strEmployessId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            List<HumanOrganization> lst = new List<HumanOrganization>();
            if (employessIds.Count > 0)
            {
                foreach (var item in employessIds)
                {
                    lst.Add(new HumanOrganization
                    {
                        HumanEmployeeID = (int)item,
                        HumanRoleID = roleId,
                        CreateAt = DateTime.Now,
                        CreateBy = userId

                    });
                }
                organizationDA.InsertRange(lst);
            }
            organizationDA.Save();
        }
        /// <summary>
        /// Lấy quyền và tổ chức theo Id người dùng
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<HumanGroupPositionItem> GetAllRoleAndDepartmentByEmployeeID(int? employeeId)
        {
            try
            {
                var result = organizationDA.GetQuery()
                    .Where(o => o.HumanEmployeeID == employeeId)
                    .Where(o => o.HumanRole.IsActive)
                    .Where(o => !o.HumanRole.IsDelete)
                    .Where(o => !o.HumanRole.IsDestroy)
                    .Select(item =>
                    new HumanGroupPositionItem()
                    {
                        ID = employeeId,
                        Name = item.HumanRole.Name,
                        GroupName = item.HumanRole.HumanDepartment.Name

                    }
                    ).ToList();
                return result;
            }
            catch
            {
                return null;
            }

        }
        //review 1 - 8/2015
        public List<int> GetRoleIds(int? employeeID)
        {
            var roleIds = organizationDA.GetQuery()
                            .Where(i => i.HumanEmployeeID == employeeID)
                            .Where(i => i.HumanRole.IsActive)
                            .Where(i => !i.HumanRole.IsDelete)
                            .Where(i => !i.HumanRole.IsDestroy)
                            .Select(i => i.HumanRoleID)
                            .ToList();
            return roleIds;
        }
        //=============================================================
        public void Insert(HumanOrganizationItem item, int userID)
        {
            var organization = new HumanOrganization()
            {
                ID = item.ID,
                HumanRoleID = item.RoleID,
                HumanEmployeeID = item.UserID,
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
