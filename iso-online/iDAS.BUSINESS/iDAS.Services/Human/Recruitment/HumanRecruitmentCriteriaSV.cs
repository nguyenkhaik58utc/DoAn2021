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
    public class HumanRecruitmentCriteriaSV
    {
        private HumanRecruitmentCriteriaDA RecruitmentCriteriaDA = new HumanRecruitmentCriteriaDA();
        /// <summary>
        /// Lấy danh sách tiêu chí tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentCriteriaItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = RecruitmentCriteriaDA.Repository;
            var rc = RecruitmentCriteriaDA.GetQuery(i => i.IsDelete == false)
                        .Select(item => new HumanRecruitmentCriteriaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Content = item.Content,
                            MaxPoint = item.MaxPoint,
                            MinPoint = item.MinPoint,
                            Factor = item.Factor,
                            Role = new HumanRoleViewItem()
                            {
                                ID = item.HumanRoleID,
                                Name = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRoleID).Name
                            },
                            //RoleID = item.RoleID,
                            //RoleName = dbo.HumanRole.FirstOrDefault(i => i.ID == item.RoleID).Name,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return rc;
        }
        public List<HumanRecruitmentCriteriaItem> GetByDepartment(int page, int pageSize, out int totalCount, int departmentID)
        {
            var dbo = RecruitmentCriteriaDA.Repository;
            var rc = RecruitmentCriteriaDA.GetQuery(i =>!i.IsDelete && (i.HumanRole.HumanDepartment.ID == departmentID||departmentID ==0))
                        .Select(item => new HumanRecruitmentCriteriaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Content = item.Content,
                            MaxPoint = item.MaxPoint,
                            MinPoint = item.MinPoint,
                            Factor = item.Factor,
                            Role = new HumanRoleViewItem()
                            {
                                ID = item.HumanRoleID,
                                Name = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRoleID).Name,
                            },
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return rc;
        }
        /// <summary>
        /// Lấy danh sách tiêu chí tuyển dụng đã kích hoạt
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentCriteriaItem> GetAllActiveCriteria(int page, int pageSize, out int totalCount)
        {
            var dbo = RecruitmentCriteriaDA.Repository;
            var rc = dbo.HumanRecruitmentCriterias.Where(i => i.IsDelete == false && i.IsActive == true)
                        .Select(item => new HumanRecruitmentCriteriaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Content = item.Content,
                            MaxPoint = item.MaxPoint,
                            MinPoint = item.MinPoint,
                            Factor = item.Factor,
                            Role = new HumanRoleViewItem()
                            {
                                ID = item.HumanRoleID,
                                Name = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRoleID).Name,
                            },
                            //RoleID = item.RoleID,
                            //RoleName = dbo.HumanRole.FirstOrDefault(i => i.ID == item.RoleID).Name,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return rc;
        }
        /// <summary>
        /// Lấy danh sách tiêu chí tuyển dụng theo chức danh là những tiêu chí đã được kích hoạt
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="RoleID">Mã chức danh</param>
        /// <returns></returns>
        public List<HumanRecruitmentCriteriaItem> GetByRole(int page, int pageSize, out int totalCount, int RoleID)
        {
            var dbo = RecruitmentCriteriaDA.Repository;
            var rc = RecruitmentCriteriaDA.GetQuery(i => i.IsDelete == false && i.IsActive == true && i.HumanRoleID == RoleID)
                        .Select(item => new HumanRecruitmentCriteriaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Content = item.Content,
                            MaxPoint = item.MaxPoint,
                            MinPoint = item.MinPoint,
                            Factor = item.Factor,
                            Role = new HumanRoleViewItem()
                            {
                                ID = item.HumanRoleID,
                                Name = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRoleID).Name,
                                Department = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRoleID).HumanDepartment.Name,
                            },
                            //RoleID = item.RoleID,
                            //RoleName = dbo.HumanRole.FirstOrDefault(i => i.ID == item.RoleID).Name,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return rc;
        }
        /// <summary>
        /// Lấy tiêu chí tuyển dụng theo ID
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentCriteriaItem GetById(int Id)
        {
            var dbo = RecruitmentCriteriaDA.Repository;
            var rc = dbo.HumanRecruitmentCriterias.Where(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentCriteriaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Content = item.Content,
                            MaxPoint = item.MaxPoint,
                            MinPoint = item.MinPoint,
                            Factor = item.Factor,
                            Role = new HumanRoleViewItem()
                            {
                                ID = item.HumanRoleID,
                                Name = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRoleID).Name,
                                Department = dbo.HumanRoles.FirstOrDefault(i => i.ID == item.HumanRoleID).HumanDepartment.Name,
                            },
                            //RoleID = item.RoleID,
                            //RoleName = dbo.HumanRole.FirstOrDefault(i => i.ID == item.RoleID).Name,
                            //DepartmentName = dbo.HumanRole.FirstOrDefault(i => i.ID == item.RoleID).HumanDepartment.Name,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return rc;
        }

        /// <summary>
        /// Cập nhật tiêu chí tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentCriteriaItem item, int userID)
        {
            var rc = RecruitmentCriteriaDA.GetById(item.ID);
            rc.Name = item.Name;
            rc.Content = item.Content;
            rc.MaxPoint = item.MaxPoint;
            rc.MinPoint = item.MinPoint;
            rc.Factor = item.Factor;
            rc.HumanRoleID = item.Role.ID;
            //rc.RoleID = item.RoleID;
            rc.IsActive = item.IsActive;
            rc.IsDelete = item.IsDelete;
            rc.UpdateAt = DateTime.Now;
            rc.UpdateBy = userID;
            RecruitmentCriteriaDA.Save();
        }
        /// <summary>
        /// Thêm mới tiêu chí tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentCriteriaItem item, int userID)
        {
            var rc = new HumanRecruitmentCriteria()
            {
                Name = item.Name,
                Content = item.Content,
                MaxPoint = item.MaxPoint,
                MinPoint = item.MinPoint,
                Factor = item.Factor,
                HumanRoleID = item.Role.ID,
                //RoleID = item.RoleID,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RecruitmentCriteriaDA.Insert(rc);
            RecruitmentCriteriaDA.Save();
        }
        /// <summary>
        /// Kích hoạt tiêu chí
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <param name="IsActive">Trạng thái kích hoạt</param>
        public void Active(int id, int userID, bool IsActive)
        {
            var rc = RecruitmentCriteriaDA.GetById(id);
            rc.IsActive = IsActive;
            rc.UpdateAt = DateTime.Now;
            rc.UpdateBy = userID;
            RecruitmentCriteriaDA.Save();
        }
        /// <summary>
        /// Xóa tiêu chí tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var dbo = RecruitmentCriteriaDA.Repository;
            var criteria = dbo.HumanRecruitmentCriterias.FirstOrDefault(i => i.ID == id);
            var requirement = dbo.HumanRecruitmentRequirements.FirstOrDefault(i => i.HumanRoleID == criteria.HumanRoleID);
            if (requirement != null)
            {
                return false;
            }
            else
            {
                dbo.HumanRecruitmentCriterias.Remove(criteria);
                RecruitmentCriteriaDA.Save();
                return true;
            }
        }
        /// <summary>
        /// Xóa tạm tiêu chí tuyển dụng khi tiêu chí tuyển dụng đang được sử dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserID"></param>
        public void IsDelete(int id, int userID)
        {
            var rc = RecruitmentCriteriaDA.GetById(id);
            rc.IsDelete = true;
            rc.UpdateAt = DateTime.Now;
            rc.UpdateBy = userID;
            RecruitmentCriteriaDA.Save();
        }
    }
}
