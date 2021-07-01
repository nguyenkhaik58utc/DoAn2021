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
    public class RoleSV
    {
        RoleDA roleDA = new RoleDA();

        public List<RoleItem> GetAllByGroupID(int groupId)
        {
            var roles = roleDA.GetQuery()
                        .Where(r => r.SystemDepartmentID == groupId)
                        .Select(item => new RoleItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return roles;
        }

        public List<int> GetGroupIds(List<int> roleIds)
        {
            var groupIds = roleDA.GetQuery(filter: r => roleIds.Contains(r.ID)).Select(r => r.SystemDepartmentID.Value).Distinct().ToList();
            return groupIds;
        }

        public List<RoleItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var roles = roleDA.GetQuery()
                        .Select(item => new RoleItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return roles;
        }

        public RoleItem GetByID(int id)
        {
            var dbo = roleDA.Repository;
            var item = roleDA.GetById(id);
            var role = new RoleItem();
            role.ID = item.ID;
            role.GroupID = item.SystemDepartmentID.Value;
            role.Name = item.Name;
            role.CreateByName = item.CreateBy.HasValue ? dbo.SystemUsers.Where(u => u.ID == item.CreateBy).Select(i => i.Name).FirstOrDefault() : null;
            role.UpdateByName = item.UpdateBy.HasValue ? dbo.SystemUsers.Where(u => u.ID == item.UpdateBy).Select(i => i.Name).FirstOrDefault() : null;
            role.IsActive = item.IsActive;
            role.Description = item.Description;
            role.CreateAt = item.CreateAt;
            role.CreateBy = item.CreateBy;
            role.UpdateAt = item.UpdateAt;
            role.UpdateBy = item.UpdateBy;
            return role;
        }

        public void Update(RoleItem item, int userID)
        {
            var role = roleDA.GetById(item.ID);
            role.Name = item.Name;
            role.IsActive = item.IsActive;
            role.Description = item.Description;
            role.UpdateAt = DateTime.Now;
            role.UpdateBy = userID;
            roleDA.Save();
        }

        public void Insert(RoleItem item, int userID)
        {
            var role = new SystemRole()
            {
                SystemDepartmentID = item.GroupID,
                Name = item.Name,
                IsActive = item.IsActive,
                Description = item.Description,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                UpdateAt = DateTime.Now,
                UpdateBy = userID,
            };
            roleDA.Insert(role);
            roleDA.Save();
        }

        public void Delete(int id)
        {
            roleDA.Delete(id);
            roleDA.Save();
        }
    }
}
