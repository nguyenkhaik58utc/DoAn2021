using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;

namespace iDAS.Services
{
    public class ProfileBorrowCategoryService
    {
        ProfileBorrowCategoryDA profileBorrowCateDA = new ProfileBorrowCategoryDA(); 
        ProfileBorrowDA profileBorrowDA = new ProfileBorrowDA();
        HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        HumanUserSV userSV = new HumanUserSV();

        public ProfileCategoryItem GetByID(int id)
        {
            var dbo = profileBorrowCateDA.Repository;
            var i = profileBorrowCateDA.GetById(id);
            var obj = new ProfileCategoryItem()
            {
                ID = i.ID,
                Name = i.Name,
                DepartmentID = i.DepartmentID,
                Department = new HumanDepartmentViewItem()
                {
                    ID = i.DepartmentID,
                    Name = dbo.HumanDepartments.Where(d => d.ID == i.DepartmentID).Select(d => d.Name).FirstOrDefault()
                },
                Note = i.Note,
                CreateAt = i.CreateAt,
                UpdateAt = i.UpdateAt,
                CreateBy = i.CreateBy,
                UpdateBy = i.UpdateBy,
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
            }
            return obj;
            
        }
        public List<ProfileCategoryItem> GetAll(ModelFilter filter, int groupId)
        {
            var services = profileBorrowCateDA.GetQuery(p => p.DepartmentID == groupId && p.IsDelete== false)
                    .Select(i => new ProfileCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        DepartmentID = i.DepartmentID,
                        //Department= i.HumanDepartment.Name,
                        Note = i.Note,
                        //IsDelete = i.IsDelete,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy= i.CreateBy,
                        UpdateBy= i.UpdateBy
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Filter(filter)
                    .ToList();
            if(services != null && services.Count() >0)
            {
                foreach(var itm in services)
                {
                    itm.CreateByName = userSV.GetNameByUserID(itm.CreateBy);
                    itm.UpdateByName = userSV.GetNameByUserID(itm.UpdateBy);
                }
            }
            return services;
        }
        public List<ProfileCategoryItem> GetAll()
        {
            var services = profileBorrowCateDA.GetQuery()
                    .Select(i => new ProfileCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        DepartmentID = i.DepartmentID,
                        //Department = i.HumanDepartment.Name,
                        Note = i.Note,
                        //IsDelete = i.IsDelete,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt
                    })
                    .OrderByDescending(item => item.Name)
                    .ToList();


            return services;
        }

        public List<ProfileCategoryItem> GetAllByCateGroup(int page, int pageSize, out int totalCount, int cateID, int groupId)
        {
            var services = profileBorrowCateDA.GetQuery(p => p.DepartmentID == groupId)
                    .Select(i => new ProfileCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        DepartmentID = i.DepartmentID,
                        //Department = i.HumanDepartment.Name,
                        Note = i.Note,

                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                foreach (var itm in services)
                {

                    itm.CreateByName = userSV.GetNameByUserID(itm.CreateBy);
                    itm.UpdateByName = userSV.GetNameByUserID(itm.UpdateBy);

                }
            }

            return services;
        }

        public List<ProfileCategoryItem> GetAllDeleteByGroup(int page, int pageSize, out int totalCount, int groupId)
        {
            var services = profileBorrowCateDA.GetQuery(p => p.DepartmentID == groupId && p.IsDelete)
                    .Select(i => new ProfileCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        DepartmentID = i.DepartmentID,
                        //Department = i.HumanDepartment.Name,
                        Note = i.Note,

                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                foreach (var itm in services)
                {

                    itm.CreateByName = userSV.GetNameByUserID(itm.CreateBy);
                    itm.UpdateByName = userSV.GetNameByUserID(itm.UpdateBy);

                }
            }

            return services;
        }


        public void Insert(ProfileCategoryItem obj)
        {
           try  {
                var itm = new ProfileBorrowCategory
                   {                      
                       Name = obj.Name.Trim(),
                       DepartmentID = obj.Department.ID,
                       Note = obj.Note,
                       IsDelete= false,
                       CreateAt = DateTime.Now,
                       CreateBy = obj.CreateBy

                   };
                profileBorrowCateDA.Insert(itm);
                profileBorrowCateDA.Save();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Update(ProfileCategoryItem obj)
        {
            try
            {
                var itm = profileBorrowCateDA.GetById(obj.ID);
                itm.Name = obj.Name.Trim();
                itm.DepartmentID = obj.Department.ID;
                itm.Note = obj.Note;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                profileBorrowCateDA.Update(itm);
                profileBorrowCateDA.Save();
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        public void UpdateDelete(int id, int updateBy)
        {
            try
            {
                var itm = profileBorrowCateDA.GetById(id);
                itm.IsDelete =false;               
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = updateBy;
                profileBorrowCateDA.Update(itm);
                profileBorrowCateDA.Save();
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void Delete(int id)
        {
            try
            {
                var itm = profileBorrowCateDA.GetById(id);
                if( itm != null)
                {

                    var lst = profileBorrowDA.GetQuery(p => p.ProfileBorrowCategoryID == id).ToList();
                    if (lst != null && lst.Count() > 0)
                    {
                        itm.IsDelete = true;
                        itm.UpdateAt = DateTime.Now;
                        profileBorrowCateDA.Update(itm);
                    }else
                        profileBorrowCateDA.Delete(itm);
                    profileBorrowCateDA.Save();
                }
               
             
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProfileCategoryItem> GetAllBorrowCate(int depID)
        {
            var lst = profileBorrowCateDA.GetQuery(p => p.DepartmentID == depID && !p.IsDelete)
             .Select(i => new ProfileCategoryItem()
             {
                 ID = i.ID,
                 Name = i.Name,
                 DepartmentID = i.DepartmentID,
                 //Department = i.HumanDepartment.Name,
                 Note = i.Note,

                 CreateAt = i.CreateAt,
                 UpdateAt = i.UpdateAt
             }).ToList();
            return lst;
        }

        public object GetByName(string name)
        {
            name= name.Trim().ToLower();
            var lst = profileBorrowCateDA.GetQuery(p => p.Name.Trim().ToLower().Equals(name));
            if (lst.Count() > 0)
                return lst.FirstOrDefault();

            return null;
        }
    }
}
