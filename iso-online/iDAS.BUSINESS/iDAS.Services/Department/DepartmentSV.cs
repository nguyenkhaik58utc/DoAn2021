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
    public class DepartmentSV : BaseService
    {
        private HumanDepartmentDA departmentDA = new HumanDepartmentDA();
        private HumanRoleDA roleDA = new HumanRoleDA();
        private IEnumerable<HumanDepartment> getChilds(IEnumerable<HumanDepartment> e, int id)
        {
            var list1 = e.Where(a => a.ParentID == id);
            var list2 = e.Where(a => a.ID == id);
            list2.Concat(list1);
            return list2.Concat(list1.SelectMany(a => getChilds(e, a.ID)));
        }
        public IEnumerable<HumanDepartment> GetListDepartmentByParentID(int parentID)
        {
            var dbo = departmentDA.Repository;
            var result = getChilds(dbo.HumanDepartments, parentID);
            return result;
        }
        public List<int> GetDepartmentIDsByParentID(int parentID)
        {
            var dbo = departmentDA.Repository;
            var result = getChilds(dbo.HumanDepartments, parentID);
            return result.Select(t=>t.ID).ToList();
        }
        public bool CheckParent(int id, int idcheck)
        {
            var groupIds = new List<object>();
            GetGroupsByParentID(idcheck, ref groupIds);
            if (groupIds.Count > 0)
            {
                if (groupIds.Contains(id))
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }
        public List<int> getListDepartmentID(List<int> lstRoleID)
        {
            List<int> rs = new List<int>();
            var dbo = departmentDA.Repository;
            foreach (int roleID in lstRoleID)
            {
                var obj = dbo.HumanRoles.FirstOrDefault(i=>i.ID==roleID);
                if (obj != null)
                    rs.Add(obj.HumanDepartmentID);
            }

            return rs;
        }
        public List<HumanDepartmentItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = departmentDA.Repository;
            var groups = departmentDA.GetQuery()
                        .Select(item => new HumanDepartmentItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            ParentID = item.ParentID,
                            CreateAt = item.CreateAt,
                            //CreateBy = dbo.HumanUser.FirstOrDefault(u=>u.ID == item.CreateBy).HumanEmployee.FirstOrDefault().Name,
                            UpdateAt = item.UpdateAt,
                            //UpdateBy = dbo.HumanUser.FirstOrDefault(u => u.ID == item.UpdateBy).HumanEmployee.FirstOrDefault().Name,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return groups;
        }
        public List<HumanDepartmentItem> GetForCombobox()
        {

            var groups = departmentDA.GetQuery(d => d.IsActive == true & d.IsDelete == false && d.IsCancel == false)
                        .Select(item => new HumanDepartmentItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return groups;
        }
        /// <summary>
        /// UpdateBy: CuongPC
        /// UpdateAt:10/10/2014
        /// </summary>
        /// <param name="id">id của tổ chức</param>
        /// <returns></returns>
        public HumanDepartmentItem GetByID(int id)
        {
            var dbo = departmentDA.Repository;
            var item = departmentDA.GetById(id);
            var depart = new HumanDepartmentItem();
            if (item != null)
            {
                depart.ID = item.ID;
                depart.Authorize = item.Authorize;
                depart.Resposibility = item.Resposibility;
                depart.ParentID = item.ParentID;
                depart.Name = item.Name;
                depart.Code = item.Code;
                depart.ParentID = item.ParentID;
                depart.Email = item.Email;
                depart.Address = item.Address;
                depart.EstablishedDate = item.EstablishedDate;
                depart.Fax = item.Fax;
                depart.NameE = item.NameE;
                depart.Phone = item.Phone;
                depart.IsCancel = item.IsCancel;
                depart.IsDelete = item.IsDelete;
                depart.CreateAt = item.CreateAt;
                depart.Tax = item.Tax;
                depart.IsMerge = item.IsMerge;
                depart.History = item.History;
                depart.Website = item.Website;
                depart.IsActive = item.IsActive;
                depart.Position = item.Position;
                depart.CreateByName = item.CreateBy.HasValue ? (dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name : string.Empty) : String.Empty;
                depart.UpdateAt = item.UpdateAt;
                depart.UpdateByName = item.UpdateBy.HasValue ? (dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy) != null ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy).HumanEmployee.Name : string.Empty) : String.Empty;
            }
            return depart;
        }
        public void Insert(HumanDepartmentItem item, int userID)
        {
            var depart = new HumanDepartment()
            {
                ParentID = item.ParentID,
                Name = item.Name,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                UpdateAt = DateTime.Now,
                UpdateBy = userID,
            };
            departmentDA.Insert(depart);
            departmentDA.Save();
        }
        public void Insert(HumanDepartmentItem item, int userID, out int id)
        {
            var depart = new HumanDepartment()
            {
                ParentID = item.ParentID,
                Name = item.Name,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                UpdateAt = DateTime.Now,
                UpdateBy = userID,
            };
            departmentDA.Insert(depart);
            departmentDA.Save();
            id = depart.ID;
        }
        public void Update(HumanDepartmentItem item, int userID)
        {
            var depart = departmentDA.GetById(item.ID);
            depart.Name = item.Name;
            depart.Resposibility = item.Resposibility;
            depart.Authorize = item.Authorize;
            depart.Address = item.Address;
            depart.Code = item.Code;
            depart.Email = item.Email;
            depart.EstablishedDate = item.EstablishedDate;
            depart.Fax = item.Fax;
            depart.IsCancel = item.IsCancel != null ? item.IsCancel : depart.IsCancel;
            depart.IsDelete = item.IsDelete != null ? item.IsDelete : depart.IsDelete;
            depart.NameE = item.NameE;
            depart.Phone = item.Phone;
            depart.Tax = item.Tax;
            depart.Website = item.Website;
            depart.ParentID = item.ParentID != null ? item.ParentID : depart.ParentID;
            depart.IsActive = item.IsActive != null ? item.IsActive : depart.IsActive;
            depart.UpdateAt = DateTime.Now;
            depart.UpdateBy = userID;
            departmentDA.Save();
        }
        //public void Delete(int id)
        //{
        //    var Ids = new List<object>();
        //    Ids.Add(id);
        //    GetGroupsByParentID(id, ref Ids);
        //    departmentDA.DeleteRange(Ids);
        //    departmentDA.Save();
        //}
        public void SetCancelTrue(int id)
        {
            try
            {
                var groupIds = new List<object>();
                groupIds.Add(id);
                GetGroupsByParentID(id, ref groupIds);
                if (groupIds.Count > 0)
                {
                    foreach (var item in groupIds)
                    {
                        var objs = departmentDA.GetById(Convert.ToInt32(item));
                        objs.IsCancel = true;
                        objs.IsActive = false;
                        departmentDA.Update(objs);
                        var roles = roleDA.GetQuery(t => t.HumanDepartmentID == (int)item).ToList();
                        if (roles.Count > 0)
                        {
                            foreach (var i in roles)
                            {
                                i.ID = i.ID;
                                i.IsDelete = true;
                                i.IsActive = false;
                                roleDA.Update(i);
                            }
                        }
                    }
                }
                roleDA.Save();
                departmentDA.Save();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // HungNM. Add full role for Giám đốc. 20201015.
        public bool isDirectorRole(List<int> HumanRolesID)
        {
            try
            {
                bool isDirector = false;
                foreach (int item in HumanRolesID)
                {
                    var roles = roleDA.GetQuery(t => t.ID == item).ToList().FirstOrDefault();
                    if ((roles.Name + "").Trim().ToLower() == "Giám đốc".Trim().ToLower())
                    {
                        isDirector = true;
                        break;
                    }    
                }
                return isDirector;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // End.

        public void SetDeletedTrue(int id)
        {
            try
            {
                var groupIds = new List<object>();
                GetGroupsByParentID(id, ref groupIds);
                groupIds.Add(id);
                if (groupIds.Count > 0)
                {
                    foreach (var item in groupIds)
                    {
                        var objs = departmentDA.GetById(Convert.ToInt32(item));
                        objs.IsDelete = true;
                        objs.IsActive = false;
                        departmentDA.Update(objs);
                        var roles = roleDA.GetQuery(t => t.HumanDepartmentID == (int)item).ToList();
                        if (roles.Count > 0)
                        {
                            foreach (var i in roles)
                            {
                                i.ID = i.ID;
                                i.IsDelete = true;
                                i.IsActive = false;
                                roleDA.Update(i);
                            }
                        }
                    }
                }
                roleDA.Save();
                departmentDA.Save();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //public void Restore(int id)
        //{
        //    try
        //    {
        //        var groupIds = new List<object>();
        //        GetGroupsByParentID(id, ref groupIds);
        //        groupIds.Add(id);
        //        if (groupIds.Count > 0)
        //        {
        //            foreach (var item in groupIds)
        //            {
        //                var objs = departmentDA.GetById(Convert.ToInt32(item));
        //                objs.IsDelete = false;
        //                objs.IsCancel = false;
        //                objs.IsActive = true;
        //                departmentDA.Update(objs);
        //                var roles = roleDA.GetQuery(t => t.HumanDepartmentID == (int)item).ToList();
        //                if (roles.Count > 0)
        //                {
        //                    foreach (var i in roles)
        //                    {
        //                        i.ID = i.ID;
        //                        i.IsDelete = false;
        //                        i.IsActive = true;
        //                        roleDA.Update(i);
        //                    }
        //                }
        //            }
        //        }
        //        roleDA.Save();
        //        departmentDA.Save();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
        private void ChangeParentId(int parentId, int parentIdNew)
        {
            try
            {
                List<HumanDepartment> lsgroup = departmentDA.GetQuery(t => t.ParentID == parentId).ToList();
                foreach (var item in lsgroup)
                {
                    item.ParentID = parentIdNew;
                }
                departmentDA.Save();
            }
            catch (Exception ex)
            {

                throw;
            }
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
        public List<HumanDepartmentItem> GetGroupsByParentID(int parentId)
        {
            var groups = departmentDA.GetQuery(t => t.ParentID == parentId)
                           .Select(item => new HumanDepartmentItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               Name = item.Name,
                               IsDelete = item.IsDelete,
                               IsActive = item.IsActive,
                               IsCancel = item.IsCancel
                           }).ToList();

            return groups;
        }
        public int GetChildrenCount(int parentId, bool viewCancel = false, bool viewDeleted = false)
        {
            var count = departmentDA.GetQuery(t => t.ParentID == parentId)
                            .Where(i => viewCancel || !viewCancel && !i.IsCancel)
                            .Where(i => viewDeleted || !viewDeleted && !i.IsDelete)
                           .Count();

            return count;
        }

        //public List<DepartmentTreeItem> GetDepartments(int departmentID, bool viewCancel = false, bool viewDeleted = false)
        //{
        //    var dbo = departmentDA.Repository;
        //    var data = dbo.HumanDepartments
        //                .Where(i => i.ParentID == departmentID)
        //                .Where(i => viewCancel || !viewCancel && !i.IsCancel)
        //                .Where(i => viewDeleted || !viewDeleted && !i.IsDeleted)
        //                .Select(item => new DepartmentTreeItem()
        //                {
        //                    ID = item.ID,
        //                    ParentID = item.ParentID,
        //                    Name = item.Name,
        //                    IsDeleted = item.IsDeleted,
        //                    IsUse = item.IsActive,
        //                    IsCancel = item.IsCancel,
        //                    Leaf = dbo.HumanDepartments.FirstOrDefault(i => i.ParentID == item.ID) == null
        //                }).ToList();
        //    return data;
        //}
        public List<HumanDepartmentItem> GetGroupsByParentID(int parentId, bool viewCancel = false, bool viewDeleted = false)
        {
            var groups = departmentDA.GetQuery(t => t.ParentID == parentId)
                            .Where(i => viewCancel || !viewCancel && !i.IsCancel)
                            .Where(i => viewDeleted || !viewDeleted && !i.IsDelete)
                           .Select(item => new HumanDepartmentItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               Name = item.Name,
                               IsDelete = item.IsDelete,
                               IsActive = item.IsActive,
                               IsCancel = item.IsCancel
                           }).ToList();

            return groups;
        }
        public List<HumanDepartmentItem> GetGroupsByParentID(int parentId, bool chkIsCancel)
        {
            var groups = new List<HumanDepartmentItem>();
            if (chkIsCancel)
            {
                groups = departmentDA.GetQuery(t => t.ParentID == parentId)
                            .Select(item => new HumanDepartmentItem()
                            {
                                ID = item.ID,
                                ParentID = item.ParentID,
                                Name = item.Name,
                                IsDelete = item.IsDelete,
                                IsActive = item.IsActive,
                                IsCancel = item.IsCancel
                            }).ToList();
            }
            else
            {
                groups = departmentDA.GetQuery(t => t.ParentID == parentId && t.IsDelete == false && t.IsCancel == false)
                            .Select(item => new HumanDepartmentItem()
                            {
                                ID = item.ID,
                                ParentID = item.ParentID,
                                Name = item.Name,
                                IsDelete = item.IsDelete,
                                IsActive = item.IsActive,
                                IsCancel = item.IsCancel
                            }).ToList();
            }
            return groups;
        }
        public List<int> GetParentGroupIDs(List<int> groupIds)
        {
            var parentIds = departmentDA.GetQuery().Where(g => groupIds.Contains(g.ID)).Select(g => g.ParentID).ToList();
            return parentIds;
        }
        public List<HumanDepartmentViewItem> GetByIds(int[] ids)
        {
            var dbo = departmentDA.Repository;
            var results = dbo.HumanDepartments.Where(i => ids.Contains(i.ID))
                            .Where(i => !i.IsDestroy)
                            .Select(item => new HumanDepartmentViewItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                            }).ToList();
            return results;
        }
        private void ChangeDepartmentId(int id, int departmentIdNew)
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

        ///Review 1 -- 7/2015
        private void activeDepartments(HumanDepartmentItem item)
        {
            var dbo = departmentDA.Repository;
            var departments = getDepartmentChilds(dbo.HumanDepartments, item.ID);
            foreach (var department in departments)
            {
                department.IsActive = item.IsActive;
                department.IsCancel = false;
                department.IsMerge = false;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = User.ID;
            }
        }
        private void cancelDepartments(HumanDepartmentItem item)
        {
            var dbo = departmentDA.Repository;
            var departments = getDepartmentChilds(dbo.HumanDepartments, item.ID);
            foreach (var department in departments)
            {
                department.IsCancel = item.IsCancel;
                department.IsActive = false;
                department.IsMerge = false;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = User.ID;
            }
        }
        private void mergeDepartments(HumanDepartmentItem item)
        {
            var dbo = departmentDA.Repository;
            var departments = getDepartmentChilds(dbo.HumanDepartments, item.ID);
            foreach (var department in departments)
            {
                department.IsMerge = item.IsMerge;
                department.IsActive = false;
                department.IsCancel = false;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = User.ID;
            }
        }
        private void deleteDepartments(int departmentID)
        {
            var dbo = departmentDA.Repository;
            var departments = getDepartmentChilds(dbo.HumanDepartments, departmentID);
            foreach (var department in departments)
            {
                department.IsDelete = true;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = User.ID;
            }
        }
        private void restoreDepartments(int departmentID)
        {
            var dbo = departmentDA.Repository;
            var departments = getDepartmentChilds(dbo.HumanDepartments, departmentID);
            foreach (var department in departments)
            {
                department.IsDelete = false;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = User.ID;
            }
        }
        private void destroyDepartments(int departmentID)
        {
            var dbo = departmentDA.Repository;
            var departments = getDepartmentChilds(dbo.HumanDepartments, departmentID);
            foreach (var department in departments)
            {
                if (!department.IsDelete) continue;
                department.IsDestroy = true;
                department.UpdateAt = DateTime.Now;
                department.UpdateBy = User.ID;
            }
        }
        private IEnumerable<HumanDepartment> getDepartmentChilds(IEnumerable<HumanDepartment> query, int parentID)
        {
            var list1 = query.Where(i => i.ParentID == parentID).Where(i => !i.IsDestroy);
            var list2 = query.Where(i => i.ID == parentID).Where(i => !i.IsDestroy);
            list2.Concat(list1);
            return list2.Concat(list1.SelectMany(i => getDepartmentChilds(query, i.ID)));
        }
        public List<HumanDepartmentItem> GetDepartments(int departmentID, bool isDeactive, bool isCancel, bool isMerge, bool isDelete, string strDepartmentIDs)
        {
            var departmentIDs = Utilities.Convert.ToListInt(strDepartmentIDs);
            var dbo = departmentDA.Repository;
            var data = dbo.HumanDepartments
                        .Where(i => i.ParentID == departmentID)
                        .Where(i => i.IsActive == true || isDeactive || i.IsCancel || i.IsMerge || i.IsDelete)
                        .Where(i => i.IsCancel == false || isCancel)
                        .Where(i => i.IsMerge == false || isMerge)
                        .Where(i => i.IsDelete == false || isDelete)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(i => new HumanDepartmentItem()
                        {
                            ID = i.ID,
                            ParentID = i.ParentID,
                            Name = i.Name,
                            EstablishedDate = i.EstablishedDate,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                            IsDelete = i.IsDelete,
                            IsActive = i.IsActive,
                            IsCancel = i.IsCancel,
                            IsMerge = i.IsMerge,
                            IsSelected = departmentIDs.Contains(i.ID),
                            IsParent = dbo.HumanDepartments
                                        .Where(a => a.ParentID == i.ID)
                                        .Where(a => a.IsActive == true || isDeactive || i.IsCancel || i.IsMerge || i.IsDelete)
                                        .Where(a => a.IsCancel == false || isCancel)
                                        .Where(a => a.IsMerge == false || isMerge)
                                        .Where(a => a.IsDelete == false || isDelete)
                                        .Where(a => !a.IsDestroy)
                                        .Count() > 0
                        }).ToList();
            return data;
        }
        public HumanDepartmentItem GetDepartment(int departmentID)
        {
            var dbo = departmentDA.Repository;
            var item = departmentDA.GetById(departmentID);
            var department = new HumanDepartmentItem();
            if (item != null)
            {
                department.ID = item.ID;
                department.Authorize = item.Authorize;
                department.Resposibility = item.Resposibility;
                department.ParentID = item.ParentID;
                department.Name = item.Name;
                department.Code = item.Code;
                department.ParentID = item.ParentID;
                department.Email = item.Email;
                department.Address = item.Address;
                department.EstablishedDate = item.EstablishedDate;
                department.Fax = item.Fax;
                department.NameE = item.NameE;
                department.Phone = item.Phone;
                department.IsActive = item.IsActive;
                department.IsCancel = item.IsCancel;
                department.IsMerge = item.IsMerge;
                department.IsDelete = item.IsDelete;
                department.Tax = item.Tax;
                department.History = item.History;
                department.Website = item.Website;
                department.Position = item.Position;
                department.IsParentActive = CheckDepartmentParentActive(item.ParentID);
                department.CreateAt = item.CreateAt;
                department.CreateByName = item.CreateBy.HasValue ? (dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name : string.Empty) : String.Empty;
                department.UpdateAt = item.UpdateAt;
                department.UpdateByName = item.UpdateBy.HasValue ? (dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy) != null ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy).HumanEmployee.Name : string.Empty) : String.Empty;
            }
            return department;
        }
        public void Insert(HumanDepartmentItem item)
        {
            var department = new HumanDepartment()
            {
                ParentID = item.ParentID,
                Name = item.Name,
                Code = item.Code,
                NameE = item.NameE,
                Address = item.Address,
                Email = item.Email,
                Fax = item.Fax,
                Website = item.Website,
                Phone = item.Phone,
                Tax = item.Tax,
                EstablishedDate = item.EstablishedDate,
                Resposibility = item.Resposibility,
                Authorize = item.Authorize,
                Position = 0,
                IsActive = item.IsActive,
                IsCancel = item.IsCancel,
                IsMerge = item.IsMerge,
                IsDestroy = false,
                History = item.History,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            departmentDA.Insert(department);
            departmentDA.Save();
        }
        public void Update(HumanDepartmentItem item)
        {
            var department = departmentDA.GetById(item.ID);
            if (department.IsActive != item.IsActive)
            {
                activeDepartments(item);
            }
            if (department.IsCancel != item.IsCancel)
            {
                cancelDepartments(item);
            }
            if (department.IsMerge != item.IsMerge)
            {
                mergeDepartments(item);
            }
            department.Name = item.Name;
            department.Resposibility = item.Resposibility;
            department.Authorize = item.Authorize;
            department.Address = item.Address;
            department.Code = item.Code;
            department.Email = item.Email;
            department.EstablishedDate = item.EstablishedDate;
            department.Fax = item.Fax;
            department.IsCancel = item.IsCancel;
            department.IsDelete = item.IsDelete;
            department.NameE = item.NameE;
            department.Phone = item.Phone;
            department.Tax = item.Tax;
            department.Website = item.Website;
            department.ParentID = item.ParentID;
            department.IsActive = item.IsActive;
            department.IsMerge = item.IsMerge;
            department.History = item.History;
            department.UpdateAt = DateTime.Now;
            department.UpdateBy = User.ID;

            departmentDA.Save();
        }
        public void Delete(int departmentID)
        {
            deleteDepartments(departmentID);
            departmentDA.Save();
        }
        public void Destroy(int departmentID)
        {
            destroyDepartments(departmentID);
            departmentDA.Save();
        }
        public void Restore(int departmentID)
        {
            restoreDepartments(departmentID);
            departmentDA.Save();
        }
        public bool Sort(int departmentID, int parentID, bool sort, bool isDeactive, bool isCancel, bool isMerge, bool isDelete)
        {
            var change = false;
            var departments = departmentDA.GetQuery()
                                .Where(i => i.ParentID == parentID)
                                .Where(i => i.IsActive == true || isDeactive || i.IsCancel || i.IsMerge || i.IsDelete)
                                .Where(i => i.IsCancel == false || isCancel)
                                .Where(i => i.IsMerge == false || isMerge)
                                .Where(a => a.IsDelete == false || isDelete)
                                .Where(i => !i.IsDestroy)
                                .OrderBy(i => i.Position).ToList();
            var count = departments.Count(); 
            for (var i = 0; i < count; i++ )
            {
                int pos = i + 1;
                departments[i].Position = pos;
                if (departments[i].ID == departmentID)
                {

                    if (sort && i>0)
                    {
                        departments[i].Position = pos - 1;
                        departments[i - 1].Position = pos;
                        change = true;
                    }
                    if (!sort && i < count - 1)
                    {
                        departments[i].Position = pos + 1;
                        departments[i + 1].Position = pos;
                        i = pos;
                        change = true;
                    }
                }
            }
            departmentDA.Save();
            return change;
        }
        public List<int> GetDepartmentIDs(int parentID) {
            var data = departmentDA.GetQuery()
                        .Where(i => i.ParentID == parentID)
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.ParentID)
                        .ThenBy(i => i.Position)
                        .Select(i => i.ID)
                        .ToList();
            return data;
        }
        public bool CheckNameExist(string name, int parentID)
        {
            var check = departmentDA.GetQuery()
                        .Where(i => i.ParentID == parentID)
                        .Where(i => i.Name.ToUpper().Equals(name.Trim().ToUpper()))
                        .Where(i => !i.IsDestroy)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string name, int parentID, int departmentID)
        {
            var check = departmentDA.GetQuery()
                        .Where(i => i.ParentID == parentID)
                        .Where(i => i.ID != departmentID)
                        .Where(i => i.Name.ToUpper().Equals(name.Trim().ToUpper()))
                        .Where(i => !i.IsDestroy)
                        .Count() > 0;
            return check;
        }
        public bool CheckDepartmentParentActive(int parentID)
        {
            var check = departmentDA.GetQuery()
                        .Where(i => i.ID == parentID)
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsCancel)
                        .Where(i => !i.IsMerge)
                        .Where(i => !i.IsDelete)
                        .Where(i => !i.IsDestroy)
                        .Count() > 0;
            return check;
        }
    }
}
