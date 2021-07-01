using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;
using iDAS.Utilities;


namespace iDAS.Services
{
    public class ProfileBorrowSV
    {
        ProfileBorrowDA profileBorrowDA = new ProfileBorrowDA();
        HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        HumanUserSV userSV = new HumanUserSV();


        public ProfileBorrowItem GetByID(int id)
        {
            try
            {
                var dbo = profileBorrowDA.Repository;
                var i = profileBorrowDA.GetById(id);
                if (i != null)
                {
                    var obj = new ProfileBorrowItem()
                    {
                        ID = i.ID,
                        Name = i.Profile.Name,
                        BorrowBy = i.BorrowBy,
                        BorrowAt = i.BorrowAt,
                        DepartmentID = i.ProfileBorrowCategory.DepartmentID,
                        CategoryID = i.ProfileBorrowCategoryID,
                        Category = i.ProfileBorrowCategory.Name,
                        Code = i.Profile.Code,
                        IsReturn = i.IsReturn,
                        ReturnAt = i.ReturnAt,
                        LimitAt = i.LimitAt,
                        Note = i.Note,
                       
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy,

                    };

                    if (obj != null)
                    {
                       
                        obj.EmployeeInfo = sysUserSV.GetEmployeeView(i.BorrowBy);
                       
                        if (obj.EmployeeInfo != null)
                        {
                            obj.Borrower = obj.EmployeeInfo.Name;
                            obj.Position = iDAS.Utilities.Common.GetData.GetPositionInfo(obj.EmployeeInfo.Department, obj.EmployeeInfo.Role);
                        }


                        obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                        obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);

                    }
                    return obj;


                }
                return null;
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        //public List<CategoryItem> GetAll(int page, int pageSize, out int totalCount, int groupId)
        //{
        //    var services = profileBorrowDA.GetQuery(p => p.CategoryID == groupId)
        //            .Select(i => new CategoryItem()
        //            {
        //                ID = i.ID,
        //                Name = i.Name,
        //                DepartmentID = i.DepartmentID,
        //                Department = i.HumanDepartment.Name,
        //                Note = i.Note,

        //                CreateAt = i.CreateAt,
        //                UpdateAt = i.UpdateAt,
        //                CreateBy = i.CreateBy,
        //                UpdateBy = i.UpdateBy
        //            })
        //            .OrderByDescending(item => item.Name)
        //            .Page(page, pageSize, out totalCount)
        //            .ToList();
        //    if (services != null && services.Count() > 0)
        //    {
        //        foreach (var itm in services)
        //        {

        //            itm.CreateByName = userSV.GetNameByUserID(itm.CreateBy);
        //            itm.UpdateByName = userSV.GetNameByUserID(itm.UpdateBy);

        //        }


        //    }

        //    return services;
        //}


        //public List<CategoryItem> GetAll()
        //{
        //    var services = profileBorrowDA.GetQuery()
        //            .Select(i => new CategoryItem()
        //            {
        //                ID = i.ID,
        //                Name = i.Name,
        //                DepartmentID = i.DepartmentID,
        //                Department = i.HumanDepartment.Name,
        //                Note = i.Note,
        //                CreateAt = i.CreateAt,
        //                UpdateAt = i.UpdateAt
        //            })
        //            .OrderByDescending(item => item.Name)
        //            .ToList();


        //    return services;
        //}
        public void Insert(ProfileCategoryItem obj)
        {
            try
            {
                var itm = new ProfileBorrow
                {
                    //Name = obj.Name,
                    //DepartmentID = obj.DepartmentID,
                    Note = obj.Note,
                    //IsDelete = false,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy

                };
                profileBorrowDA.Insert(itm);
                profileBorrowDA.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public void Update(CategoryItem obj)
        //{
        //    try
        //    {
        //        var itm = profileBorrowDA.GetById(obj.ID);
        //        itm.Name = obj.Name;
        //        itm.DepartmentID = obj.DepartmentID;
        //        itm.Note = obj.Note;
        //        itm.UpdateAt = DateTime.Now;
        //        itm.UpdateBy = obj.UpdateBy;
        //        profileBorrowDA.Update(itm);
        //        profileBorrowDA.Save();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        //public void Delete(int id)
        //{
        //    try
        //    {
        //        var itm = profileBorrowDA.GetById(id);

        //        profileBorrowDA.Delete(itm);
        //        profileBorrowDA.Save();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public ProfileBorrowItem GetByProfileID(int id, bool isReturn=false)
        {
            try
            {
                var i = profileBorrowDA.GetQuery(p => p.ProfileID== id && p.IsReturn== isReturn).FirstOrDefault();
                if (i != null)
                {
                    var obj = new ProfileBorrowItem()
                    {
                        ID = i.ID,
                        Name = i.Profile.Name,
                        BorrowBy = i.BorrowBy,
                        BorrowAt = i.BorrowAt,
                        DepartmentID = i.ProfileBorrowCategory.DepartmentID,
                        CategoryID = i.ProfileBorrowCategoryID,
                        Category = i.ProfileBorrowCategory.Name,
                        Code = i.Profile.Code,
                        IsReturn = i.IsReturn,
                        ReturnAt = i.ReturnAt,
                        LimitAt = i.LimitAt,

                        Note = i.Note,

                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy,

                    };

                    if (obj != null)
                    {
                        obj.EmployeeInfo = sysUserSV.GetEmployeeView(obj.BorrowBy);
                        obj.Borrower = obj.EmployeeInfo.Name;
                        obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                        obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);

                    }
                    return obj;


                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<ProfileBorrowItem> GetAllBorrow(int page, int pageSize, out int totalCount, int cateID)
        {
            var services = profileBorrowDA.GetQuery(p => p.ProfileBorrowCategoryID == cateID )
                    .Select(i => new ProfileBorrowItem()
                    {
                        ID = i.ID,
                        Code= i.Profile.Code,
                        Security= i.Profile.ProfileSecurity.Name,
                        Color = i.Profile.ProfileSecurity.Color,
                       ReceiveID= i.Profile.EmployeeID,
                        Name = i.Profile.Name,
                        BorrowBy = i.BorrowBy,
                        BorrowAt = i.BorrowAt,
                        Note = i.Note,
                        CategoryID= i.ProfileBorrowCategoryID,
                        IsReturn= i.IsReturn,
                        LimitAt= i.LimitAt,
                        ReturnAt= i.ReturnAt,
                        CreateAt = i.CreateAt,
                        UpdateAt = (i.UpdateAt != null && i.UpdateAt> DateTime.MinValue)? i.UpdateAt: i.CreateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)
                      .OrderByDescending(item => item.UpdateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                foreach (var itm in services)
                {
                    itm.EmployeeInfo = sysUserSV.GetEmployeeView(itm.BorrowBy);
                    itm.Receiver = sysUserSV.GetNameByID(itm.ReceiveID);
                    itm.Borrower = itm.EmployeeInfo !=null? itm.EmployeeInfo.Name : "";
                    itm.CreateName = userSV.GetNameByUserID(itm.CreateBy);
                    itm.UpdateName = userSV.GetNameByUserID(itm.UpdateBy);

                }


            }

            return services;
        }

        public List<ProfileBorrowItem> GetAllByBorrowCate( int cateID)
        {
            var services = profileBorrowDA.GetQuery(p => p.ProfileBorrowCategoryID == cateID)
                    .Select(i => new ProfileBorrowItem()
                    {
                        ID = i.ID,
                        Code = i.Profile.Code,
                        Security = i.Profile.ProfileSecurity.Name,
                        Color = i.Profile.ProfileSecurity.Color,
                        ReceiveID = i.Profile.EmployeeID,
                        Name = i.Profile.Name,
                        BorrowBy = i.BorrowBy,
                        BorrowAt = i.BorrowAt,
                        Note = i.Note,
                        CategoryID = i.ProfileBorrowCategoryID,
                        IsReturn = i.IsReturn,
                        LimitAt = i.LimitAt,
                        ReturnAt = i.ReturnAt,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)
                
                    .ToList();
            

            return services;
        }
        public List<ProfileBorrowItem> GetAllNotReturnByBorrowCate(int cateID)
        {
            var lst = profileBorrowDA.GetQuery(p => p.ProfileBorrowCategoryID == cateID && !p.IsReturn)
                    .Select(i => new ProfileBorrowItem()
                    {
                        ID = i.ID,
                        Code = i.Profile.Code,
                        Security = i.Profile.ProfileSecurity.Name,
                        Color = i.Profile.ProfileSecurity.Color,
                        ReceiveID = i.Profile.EmployeeID,
                        Name = i.Profile.Name,
                        BorrowBy = i.BorrowBy,
                        BorrowAt = i.BorrowAt,
                        Note = i.Note,
                        CategoryID = i.ProfileBorrowCategoryID,
                        IsReturn = i.IsReturn,
                        LimitAt = i.LimitAt,
                        ReturnAt = i.ReturnAt,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)

                    .ToList();


            return lst;
        }
        public void Insert(ProfileBorrowItem obj)
        {
            try
            {
                var itm = new ProfileBorrow
                {
                    ProfileBorrowCategoryID = obj.CategoryID,
                    BorrowBy = obj.EmployeeInfo.ID,
                    BorrowAt = obj.BorrowAt,
                    LimitAt = obj.LimitAt,
                    IsReturn = false,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy,
                    Note = obj.Note,
                    ProfileID = obj.ProfileID

                };

                profileBorrowDA.Insert(itm);
                profileBorrowDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(ProfileBorrowItem obj)
        {
            try
            {
                var itm = profileBorrowDA.GetById(obj.ID);
                itm.ProfileBorrowCategoryID = obj.CategoryID;
                itm.BorrowBy = obj.EmployeeInfo.ID;
                itm.BorrowAt = obj.BorrowAt;
                itm.LimitAt = obj.LimitAt;
                itm.IsReturn = false;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                itm.Note = obj.Note;
                // itm.    ProfileID = obj.ProfileID


                profileBorrowDA.Update(itm);
                profileBorrowDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateBorrow(ProfileBorrowItem obj)
        {
            try
            {
                var itm = profileBorrowDA.GetById(obj.ID);
              //  itm.CategoryID = obj.CategoryID;
                itm.BorrowBy = obj.EmployeeInfo.ID;
                itm.BorrowAt = obj.BorrowAt;
                itm.LimitAt = obj.LimitAt;               
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                itm.Note = obj.Note;
            
                profileBorrowDA.Update(itm);
                profileBorrowDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        public void UpdatePaid(ProfileBorrowItem obj)
        {
            try
            {
                var itm = profileBorrowDA.GetById(obj.ID);
                
                itm.IsReturn = true;
                itm.ReturnAt =obj.ReturnAt;
               
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                itm.Note = obj.Note;

                profileBorrowDA.Update(itm);
                profileBorrowDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
