﻿using System;
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
    public class DepartmentSV
    {
        DepartmentDA groupDA = new DepartmentDA();

        public List<DepartmentItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var groups = groupDA.GetQuery()
                        .Select(item => new DepartmentItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            Description = item.Description,
                            ParentID = item.ParentID,
                            CreateAt = item.CreateAt,
                           // CreateBy = groupDA.SystemContext.SystemUser.Where(u=>u.ID== item.CreateBy).FirstOrDefault().FullName,
                            UpdateAt = item.UpdateAt,
                           // UpdateBy = groupDA.SystemContext.SystemUser.Where(u=>u.ID== item.UpdateBy).FirstOrDefault().FullName,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return groups;
        }

        public void Update(DepartmentItem item, int userID)
        {
            var group = groupDA.GetById(item.ID);
            group.Name = item.Name;
            group.ParentID = item.ParentID;
            group.IsActive = item.IsActive;
            group.Description = item.Description;
            group.UpdateAt = DateTime.Now;
            group.UpdateBy = userID;
            groupDA.Save();
        }

        public void Insert(DepartmentItem item, int userID)
        {
            var group = new SystemDepartment()
            {
                ParentID = item.ParentID,
                Name = item.Name,
                IsActive = item.IsActive,
                Description = item.Description,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                UpdateAt = DateTime.Now,
                UpdateBy = userID,
            };
            groupDA.Insert(group);
            groupDA.Save();
        }
        public void Insert(DepartmentItem item, int userID, out int id)
        {
            var group = new SystemDepartment()
            {
                ParentID = item.ParentID,
                Name = item.Name,
                IsActive = item.IsActive,
                Description = item.Description,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                UpdateAt = DateTime.Now,
                UpdateBy = userID,
            };
            groupDA.Insert(group);
            groupDA.Save();
            id = group.ID;
        }

        public void Delete(int id)
        {
            var groupIds = new List<object>();
            groupIds.Add(id);
            GetGroupsByParentID(id, ref groupIds);

            groupDA.DeleteRange(groupIds);
            groupDA.Save();
        }

        public DepartmentItem GetByID(int id)
        {
            var dbo = groupDA.Repository;
            var item = groupDA.GetById(id);
            var group = new DepartmentItem();
            group.ID = item.ID;
            group.ParentID = item.ParentID;
            group.Name = item.Name;
            group.IsActive = item.IsActive;
            group.Description = item.Description;
            group.CreateAt = item.CreateAt;          
            group.CreateByName = dbo.SystemUsers.Where(u => u.ID == item.CreateBy).Select(i=>i.Name).FirstOrDefault();
            group.UpdateAt = item.UpdateAt;
            group.UpdateByName = dbo.SystemUsers.Where(u => u.ID == item.UpdateBy).Select(i => i.Name).FirstOrDefault();
            return group;
        }

        public void GetGroupsByParentID(int id, ref List<object> groupIds)
        {
            var groups = GetGroupsByParentID(id);
            if (groups.Count <= 0) return;
            foreach (var group in groups)
            {
                groupIds.Add(group.ID);
                GetGroupsByParentID(group.ID, ref groupIds);
            }
        }

        public List<DepartmentItem> GetGroupsByParentID(int parentId)
        {
            var groups = groupDA.GetQuery()
                        .Where(g => g.ParentID == parentId)
                        .Select(item => new DepartmentItem()
                        {
                            ID = item.ID,
                            ParentID = item.ParentID,
                            Name = item.Name,
                            Description = item.Description,
                            IsActive = item.IsActive,
                        }).ToList();
            return groups;
        }

        public List<int> GetParentGroupIDs(List<int> groupIds) {
            var parentIds = groupDA.GetQuery().Where(g => groupIds.Contains(g.ID)).Select(g => g.ParentID).ToList();
            return parentIds;
        }
    }
}
