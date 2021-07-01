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
    public class SystemRoleService
    {
        SystemRoleDA roleDA = new SystemRoleDA();

        public List<SystemRoleItem> GetAllByGroupID(int groupId)
        {
            var roles = roleDA.GetQuery()
                        .Where(r => r.GroupID == groupId)
                        .Select(item => new SystemRoleItem()
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
            var groupIds = roleDA.GetQuery(filter: r => roleIds.Contains(r.ID)).Select(r => r.GroupID).Distinct().ToList();
            return groupIds;
        }

        public List<SystemRoleItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var roles = roleDA.GetQuery()
                        .Select(item => new SystemRoleItem()
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

        public SystemRoleItem GetByID(int id)
        {
            var item = roleDA.GetById(id);
            var role = new SystemRoleItem();
            role.ID = item.ID;
            role.GroupID = item.GroupID;
            role.Name = item.Name;
            role.IsActive = item.IsActive;
            role.Description = item.Description;
            role.CreateAt = item.CreateAt;
            role.CreateBy = item.CreateBy;
            role.UpdateAt = item.UpdateAt;
            role.UpdateBy = item.UpdateBy;
            return role;
        }

        public void Update(SystemRoleItem item, int userID)
        {
            var role = roleDA.GetById(item.ID);
            role.Name = item.Name;
            role.IsActive = item.IsActive;
            role.Description = item.Description;
            role.UpdateAt = DateTime.Now;
            role.UpdateBy = userID;
            roleDA.Save();
        }

        public void Insert(SystemRoleItem item, int userID)
        {
            var role = new SystemRole()
            {
                GroupID = item.GroupID,
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
