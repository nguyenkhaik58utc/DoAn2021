using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class ProfileCategoryService
    {
        ProfileCategoryDA profileCateDA = new ProfileCategoryDA();
        ProfileBorrowCategoryDA profileBorrowCateDA = new ProfileBorrowCategoryDA();
        HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        HumanUserSV userSV = new HumanUserSV();

        public ProfileCategoryItem GetByID(int id)
        {
            var dbo = profileCateDA.Repository;
            var i = profileCateDA.GetById(id);
            var obj = new ProfileCategoryItem()
            {
                ID = i.ID,
                Name = i.Name,
               
                DepartmentID = i.DepartmentID,
                Department = new HumanDepartmentViewItem()
                {
                    ID = i.DepartmentID,
                    Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == i.DepartmentID).Name
                },
                Note = i.Note,
                CreateAt = i.CreateAt,
                UpdateAt = i.UpdateAt,
                CreateBy = i.CreateBy,
                UpdateBy = i.UpdateBy,
            };

            if (obj != null)
            {
                //obj.PublicName = sysUserSV.GetNameByID(i.PublicBy);
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);

            }
            return obj;
            
        }
        public List<ProfileCategoryItem> GetAll(int page, int pageSize, out int totalCount, int groupId)
        {
            var services = profileCateDA.GetQuery(p => p.DepartmentID == groupId)
                    .Select(i => new ProfileCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        DepartmentID = i.DepartmentID,
                        //Department= i.HumanDepartment.Name,
                        Note = i.Note,

                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy= i.CreateBy,
                        UpdateBy= i.UpdateBy
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
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
            var services = profileCateDA.GetQuery()
                    .Select(i => new ProfileCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        DepartmentID = i.DepartmentID,
                        //Department = i.HumanDepartment.Name,
                        Note = i.Note,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt
                    })
                    .OrderByDescending(item => item.Name)
                    .ToList();


            return services;
        }

        public List<ProfileCategoryItem> GetProfileByCate(int id)
        {
            var services = profileCateDA.GetQuery()
                    .Select(i => new ProfileCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        DepartmentID = i.DepartmentID,
                        //Department = i.HumanDepartment.Name,
                        Note = i.Note,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt
                    })
                    .OrderByDescending(item => item.Name)
                    .ToList();


            return services;
        }

        public void Insert(ProfileCategoryItem obj)
        {
           try  {
                var itm = new ProfileCategory
                   {                      
                       Name = obj.Name.Trim(),
                       DepartmentID = obj.Department.ID,
                       Note = obj.Note,
                       IsDelete= false,
                       CreateAt = DateTime.Now,
                       CreateBy = obj.CreateBy

                   };
                profileCateDA.Insert(itm);
                profileCateDA.Save();
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
                var itm = profileCateDA.GetById(obj.ID);
                itm.Name = obj.Name.Trim();
                itm.DepartmentID = obj.Department.ID;
                itm.Note = obj.Note;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                profileCateDA.Update(itm);
                profileCateDA.Save();
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
                var itm = profileCateDA.GetById(id);
              
                profileCateDA.Delete(itm);
                profileCateDA.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }



        public ProfileCategoryItem GetByNameDepartment(string name, int d)
        {
            try
            {
                name = name.Trim().ToLower();
                var obj = profileCateDA.GetQuery(p => p.Name.Trim().ToLower().Equals(name) && p.DepartmentID == d)
                    .Select(i => new ProfileCategoryItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                DepartmentID = i.DepartmentID,
                                //Department = i.HumanDepartment.Name,
                                Note = i.Note,

                            }).FirstOrDefault();
                return obj;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
