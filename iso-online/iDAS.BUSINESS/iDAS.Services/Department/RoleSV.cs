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
    public class RoleSV : BaseService
    {
        private HumanRoleDA roleDA = new HumanRoleDA();
        private HumanDepartmentDA departmentDA = new HumanDepartmentDA();
        
        public List<HumanRoleItem> GetRoles(int departmentID)
        {
            var roles = roleDA.GetQuery()
                        .Where(i => i.HumanDepartmentID == departmentID)
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Select(item => new HumanRoleItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            DepartmentName = item.HumanDepartment.Name,
                            IsActive = item.IsActive,
                            CreateAt = item.CreateAt,
                            Position = item.Position
                        })
                        .OrderBy(i => i.Position)
                        .ToList();
            return roles;
        }
        public List<HumanRoleItem> GetRoleSelect(int groupId, int employeeId)
        {
            bool IsDepartmentRoot = roleDA.Repository.HumanDepartments.Where(i => i.ID == groupId && i.ParentID == 0).FirstOrDefault() != null;
            var roles = roleDA.GetQuery()
                        .Where(r => (IsDepartmentRoot || r.HumanDepartmentID == groupId) && !r.IsDelete && r.IsActive)
                        .Select(item => new HumanRoleItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            DepartmentName = item.HumanDepartment.Name,
                            IsSelected = item.HumanOrganizations.Where(i => i.HumanEmployeeID == employeeId).FirstOrDefault() != null,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return roles;
        }
        
        
        public List<int> GetIDByDepartmentId(int departmentId)
        {
            var roles = roleDA.GetQuery()
                       .Where(r => r.HumanDepartmentID == departmentId).Select(t => t.ID).ToList<int>();
            return roles;
        }
        public void ChangeDepartmentId(string stringRoleId, int departmentId)
        {
            var roleIds = stringRoleId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            if (roleIds.Count > 0)
            {
                foreach (var item in roleIds)
                {
                    var role = roleDA.GetQuery(t => t.ID == (int)item && t.IsDelete == false).FirstOrDefault();
                    if (role != null)
                    {
                        role.Name = role.HumanDepartment.Name + "_" + role.Name;
                        role.HumanDepartmentID = departmentId;

                    }

                }

            }
            roleDA.Save();
        }
        public void ChangeDepartmentId(int id, int departmentIdNew)
        {
            var role = roleDA.GetQuery(t => t.HumanDepartmentID == id).ToList();
            if (role != null)
            {
                foreach (var item in role)
                {
                    item.HumanDepartmentID = departmentIdNew;
                    roleDA.Update(item);
                }
            }
            roleDA.Save();
        }
        
        public List<HumanRoleViewItem> GetByIds(int page, int pageSize, out int totalCount, int[] ids)
        {
            var roles = roleDA.GetQuery(i => ids.Contains(i.ID))
                        .Select(item => new HumanRoleViewItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Department = item.HumanDepartment.Name
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return roles;
        }
        public List<HumanRoleItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var roles = roleDA.GetQuery()
                        .Select(item => new HumanRoleItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
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
        public HumanRoleItem GetByID(int id)
        {
            var dbo = roleDA.Repository;
            var item = roleDA.GetById(id);
            var role = new HumanRoleItem();
            role.ID = item.ID;
            role.DepartmentID = item.HumanDepartmentID;
            role.Authorize = item.Authorize;
            role.Resposibility = item.Resposibility;
            role.Name = item.Name;
            role.ReportTo = item.ReportTo;
            role.ReplaceBy = item.ReplaceBy;
            role.Competence = item.Competence;
            role.CreateByName = item.CreateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name : null;
            role.UpdateByName = item.UpdateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy).HumanEmployee.Name : null;
            role.IsActive = item.IsActive;
            role.CreateAt = item.CreateAt;
            role.UpdateAt = item.UpdateAt;
            return role;
        }

        ///Review 1 -- 7/2015
        public List<int> GetDepartmentIDs(List<int> roleIds)
        {
            var departmentIds = roleDA.GetQuery()
                            .Where(i => roleIds.Contains(i.ID))
                            .Select(i => i.HumanDepartmentID).Distinct().ToList();
            return departmentIds;
        }
        public List<HumanRoleItem> GetRoles(int departmentID, bool isDelete)
        {
            var roles = roleDA.GetQuery()
                        .Where(i => i.HumanDepartmentID == departmentID)
                        .Where(i => i.IsDelete == isDelete)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(item => new HumanRoleItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            DepartmentName = item.HumanDepartment.Name,
                            IsDelete = item.IsDelete,
                            IsActive = item.IsActive,
                            UpdateAt = item.UpdateAt,
                        })
                        .ToList();
            return roles;
        }
        public HumanRoleItem GetRole(int RoleID)
        {
            var dbo = roleDA.Repository;
            var item = roleDA.GetById(RoleID);
            var role = new HumanRoleItem();
            role.ID = item.ID;
            role.DepartmentID = item.HumanDepartmentID;
            role.Authorize = item.Authorize;
            role.Resposibility = item.Resposibility;
            role.Name = item.Name;
            role.ReportTo = item.ReportTo;
            role.ReplaceBy = item.ReplaceBy;
            role.Competence = item.Competence;
            role.CreateByName = item.CreateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name : null;
            role.UpdateByName = item.UpdateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy).HumanEmployee.Name : null;
            role.IsActive = item.IsActive;
            role.Note = item.Note;
            role.CreateAt = item.CreateAt;
            role.UpdateAt = item.UpdateAt;
            return role;
        }
        public void Insert(HumanRoleItem item)
        {
            var role = new HumanRole()
            {
                HumanDepartmentID = item.DepartmentID,
                Name = item.Name.Trim(),
                Authorize = item.Authorize,
                Resposibility = item.Resposibility,
                Competence = item.Competence,
                ReportTo = item.ReportTo,
                ReplaceBy = item.ReplaceBy,
                IsActive = item.IsActive,
                IsDelete = false,
                IsDestroy = false,
                Note = item.Note,
                Position = 0,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            roleDA.Insert(role);
            roleDA.Save();
        }
        public void Update(HumanRoleItem item)
        {
            var role = roleDA.GetById(item.ID);
            role.Name = item.Name;
            role.Resposibility = item.Resposibility;
            role.Authorize = item.Authorize;
            role.Competence = item.Competence;
            role.ReportTo = item.ReportTo;
            role.ReplaceBy = item.ReplaceBy;
            role.IsActive = item.IsActive;
            role.Note = item.Note;
            role.UpdateAt = DateTime.Now;
            role.UpdateBy = User.ID;
            roleDA.Save();
        }
        public void Delete(int roleID)
        {
            var item = roleDA.GetById(roleID);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            roleDA.Save();
        }
        public void Restore(int roleID)
        {
            var item = roleDA.GetById(roleID);
            item.IsDelete = false;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            roleDA.Save();
        }
        public void Destroy(int roleID)
        {
            var item = roleDA.GetById(roleID);
            item.IsDestroy = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            roleDA.Save();
        }
        public void Move(int roleID, int departmentID)
        {
            var role = roleDA.GetById(roleID);
            role.HumanDepartmentID = departmentID;
            roleDA.Save();
        }
        public bool Sort(int roleID, int departmentID, bool sort) {
            var change = false;
            var roles = roleDA.GetQuery()
                                .Where(i => i.HumanDepartmentID == departmentID)
                                .Where(i => !i.IsDelete)
                                .Where(i => !i.IsDestroy)
                                .OrderBy(i => i.Position).ToList();
            var count = roles.Count();
            for (var i = 0; i < count; i++)
            {
                int pos = i + 1;
                roles[i].Position = pos;
                if (roles[i].ID == roleID)
                {

                    if (sort && i > 0)
                    {
                        roles[i].Position = pos - 1;
                        roles[i - 1].Position = pos;
                        change = true;
                    }
                    if (!sort && i < count - 1)
                    {
                        roles[i].Position = pos + 1;
                        roles[i + 1].Position = pos;
                        i = pos;
                        change = true;
                    }
                }
            }
            roleDA.Save();
            return change;
        }
        public bool CheckNameExist(string name, int departmentID)
        {
            var check = roleDA.GetQuery()
                        .Where(i => i.HumanDepartmentID == departmentID)
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDestroy)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string name, int departmentID, int roleID)
        {
            var check = roleDA.GetQuery()
                        .Where(i => i.HumanDepartmentID == departmentID)
                        .Where(i => i.ID != roleID)
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDestroy)
                        .Count() > 0;
            return check;
        }
    }
}
