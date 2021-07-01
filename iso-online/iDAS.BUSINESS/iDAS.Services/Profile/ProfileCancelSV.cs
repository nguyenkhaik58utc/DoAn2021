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

    public class ProfileCancelService
    {
        ProfileCancelDA profilesCancelDA = new ProfileCancelDA();//TT Bien ban Huy
        ProfileDA profilesDA = new ProfileDA();//TT Bien ban Huy
        ProfileCancelEmployeeDA profileCancelEmployeeDA = new ProfileCancelEmployeeDA();//TT Bien ban Huy
        ProfileCancelListDA profileCancelListDA = new ProfileCancelListDA();//TT Bien ban Huy
        ProfileBorrowCategoryDA profileBorrowCateDA = new ProfileBorrowCategoryDA();
        HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        HumanUserSV userSV = new HumanUserSV();

        public ProfileCancelItem GetByID(int id)
        {
            var i = profilesCancelDA.GetById(id);
            var obj = new ProfileCancelItem()
            {
                ID = i.ID,
                Name = i.Name,
                CancelAt = i.Date,
                Note = i.Note,
                MethodID = i.ProfileCancelMethodID,
                CancelMethod = i.ProfileCancelMethod.Name,
                CreateAt = i.CreateAt,
                UpdateAt = i.UpdateAt,
                CreateBy = i.CreateBy,
                UpdateBy = i.UpdateBy,
                Status = i.IsCanceled == true ? "Đã hủy" : "Chờ hủy"
            };

            if (obj != null)
            {
                //obj.PublicName = sysUserSV.GetNameByID(i.PublicBy);
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);

            }
            return obj;

        }
        //Lay thông tin cua từng Hồ sơ bị hủy trong bảng ProfileCancelList
        public ProfileCancelItem GetProfileByID(int id)
        {
            var i = profileCancelListDA.GetById(id);
            var obj = new ProfileCancelItem()
            {
                ID = i.ID,
                Name = i.Profile.Name,
                CancelAt = i.ProfileCancel.Date,
                MethodID = i.ProfileCancel.ProfileCancelMethodID,
                Reason = i.Reason,
                TotalPage = i.PageTotal,
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

        public ProfileCancelItem GetCancelByCancelIDProfileID(int id, int proID)
        {
            try
            {
                var i = profileCancelListDA.GetQuery(p => p.ProfileCancelID == id && p.ProfileID == proID).FirstOrDefault();
                if (i != null)
                {
                    var obj = new ProfileCancelItem
                    {
                        ID = i.ID,
                        ProfileID = i.ProfileID,
                        CancelID = i.ProfileCancelID,
                        CancelAt = i.ProfileCancel.Date,
                        TotalPage = i.PageTotal,
                        Reason = i.Reason,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        StrStorageTime = "N/A",
                        ProfileNotUseAt = i.Profile.NotUseAt
                    };
                    if (obj.ProfileNotUseAt != null && obj.ProfileNotUseAt > DateTime.MinValue)
                    {
                        TimeSpan dt = DateTime.Now - (DateTime)obj.ProfileNotUseAt;
                        obj.StorageTime = dt.TotalDays;
                        if (dt.TotalDays > 0)
                            obj.StrStorageTime = dt.TotalDays.ToString();
                    }
                    return obj;
                }
                else
                {
                    var objpro = profilesDA.GetById(proID);
                    if (objpro.CreateAt != null && objpro.CreateAt > DateTime.MinValue)
                    {
                        TimeSpan dt = DateTime.Now - (DateTime)objpro.NotUseAt;
                        var obj = new ProfileCancelItem { StorageTime = dt.TotalDays, StrStorageTime = "N/A", };
                        if (dt.TotalDays > 0)
                            obj.StrStorageTime = Math.Round(dt.TotalDays).ToString();
                        return obj;
                    }

                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<ProfileCancelItem> GetAll(ModelFilter filter)
        {
            List<ProfileCancelItem> lstObj = new List<ProfileCancelItem>();
            var lst = profilesCancelDA.GetQuery()
                    .OrderByDescending(item => item.CreateAt)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                ProfileCancelItem obj = new ProfileCancelItem();
                foreach (var i in lst)
                {
                    obj = convertToProfileCancel(i);
                    lstObj.Add(obj);
                }
            }
            return lstObj;
        }
        public List<ProfileItem> GetAllCancel(int page, int pageSize, out int totalCount)
        {
            ProfileBorrowDA profileBorrowDA = new DA.ProfileBorrowDA();
            List<ProfileItem> lst;
            var arID = profileBorrowDA.GetQuery(p => p.IsReturn == false).Select(p => p.ID).ToArray();//Lay HS Chua Tra

            if (arID != null && arID.Count() > 0)
            {
                lst = profilesDA.GetQuery(p => !arID.Contains(p.ID))
                   .Select(i => new ProfileItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       Note = i.Note,
                       CreateAt = i.CreateAt,
                       UpdateAt = i.UpdateAt
                   })
                   .OrderByDescending(item => item.Name)
                   .Page(page, pageSize, out totalCount)
                   .ToList();


            }
            else
            {
                lst = profilesDA.GetQuery()
                    .Select(i => new ProfileItem()
                    {
                        ID = i.ID,
                        Name = i.Name,

                        Note = i.Note,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();

            }

            return lst;
        }
        public List<ProfileCancelItem> GetAllByCancelID(int page, int pageSize, out int totalCount, int cancelID)
        {
            var lst = profileCancelListDA.GetQuery(p => p.ProfileCancelID == cancelID)
                    .Select(i => new ProfileCancelItem()
                    {
                        ID = i.ID,
                        Code = i.Profile.Code,
                        Name = i.Profile.Name,
                        CancelAt = i.ProfileCancel.Date,
                        TotalPage = i.PageTotal,
                        Reason = i.Reason,
                        CancelID = i.ProfileCancelID,
                        ProfileID = i.ProfileID,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var i in lst)
                {
                    i.CreateByName = userSV.GetNameByUserID(i.CreateBy);
                    i.UpdateByName = userSV.GetNameByUserID(i.UpdateBy);
                    if (i.UpdateAt == null)
                        i.UpdateAt = i.CreateAt;
                    if (i.UpdateByName.Equals(""))
                        i.UpdateByName = i.CreateByName;

                }
            }
            return lst;
        }

        public List<ProfileCancel> GetAllByMethodID(int methodID)
        {
            var lst = profilesCancelDA.GetQuery(p => p.ProfileCancelMethodID == methodID)
                    .ToList();
            return lst;
        }

        public void GetEmployeeByID(ref List<HumanEmployeeItem> lstUser, int cancelID)
        {

            var arEmployee = profileCancelEmployeeDA.GetQuery(p => p.ProfileCancelID == cancelID)
                .Select(p => p.EmployeeID).ToArray();
            foreach (var item in lstUser)
            {

                if (arEmployee.Contains(item.ID))
                    item.IsCheck = true;
                else
                    item.IsCheck = false;
            }
        }

        public List<HumanEmployeeItem> GetEmployeeByCancelID(int page, int pageSize, out int totalCount, int cancelID)
        {
            try
            {
                var dbo = profileCancelEmployeeDA.Repository;
                var arEmployee = profileCancelEmployeeDA.GetQuery(p => p.ProfileCancelID == cancelID).Select(p => new HumanEmployeeItem
                {
                    ID = dbo.HumanEmployees.FirstOrDefault(i => i.ID == p.EmployeeID).ID,
                    Name = dbo.HumanEmployees.FirstOrDefault(i => i.ID == p.EmployeeID).Name,
                    FileID = dbo.HumanEmployees.FirstOrDefault(i => i.ID == p.EmployeeID).AttachmentFileID,
                    FileName = dbo.HumanEmployees.FirstOrDefault(i => i.ID == p.EmployeeID).AttachmentFile.Name,
                    Role = dbo.HumanEmployees.FirstOrDefault(i => i.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.Name,
                    DepartmentName = dbo.HumanEmployees.FirstOrDefault(i => i.ID == p.EmployeeID).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,

                })
                .OrderByDescending(item => item.Name)
                .Page(page, pageSize, out totalCount)
                .ToList();
                return arEmployee;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public int Insert(ProfileCancelItem obj)
        {
            try
            {
                var itm = new ProfileCancel
                {
                    Name = obj.Name.Trim(),
                    ProfileCancelMethodID = obj.MethodID,
                    Note = obj.Note,
                    Date = obj.CancelAt,
                    IsCanceled = false,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy

                };
                profilesCancelDA.Insert(itm);
                profilesCancelDA.Save();
                return itm.ID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(ProfileCancelItem obj)
        {
            try
            {
                var itm = profilesCancelDA.GetById(obj.ID);
                itm.Name = obj.Name.Trim();
                itm.ProfileCancelMethodID = obj.MethodID;
                itm.Note = obj.Note;
                itm.Date = obj.CancelAt;
                // itm.IsCanceled = obj.IsCancel;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                profilesCancelDA.Update(itm);
                profilesCancelDA.Save();
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
                var itm = profilesCancelDA.GetById(id);

                profilesCancelDA.Delete(itm);
                profilesCancelDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteProfileCancel(int id)
        {
            try
            {
                var itm = profileCancelListDA.GetById(id);

                profileCancelListDA.Delete(itm);
                profileCancelListDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteEmployee(int cancelid, int employID)
        {
            try
            {
                var itm = profileCancelEmployeeDA.GetQuery(p => p.ProfileCancelID == cancelid && p.EmployeeID == employID).FirstOrDefault();

                profileCancelEmployeeDA.Delete(itm);
                profileCancelEmployeeDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public List<CategoryItem> GetAllBorrowCate(int depID)
        //{
        //    var lst = profileBorrowCateDA.GetQuery(p => p.DepartmentID == depID)
        //     .Select(i => new CategoryItem()
        //     {
        //         ID = i.ID,
        //         Name = i.Name,
        //         DepartmentID = i.DepartmentID,
        //         Department = i.HumanDepartment.Name,
        //         Note = i.Note,

        //         CreateAt = i.CreateAt,
        //         UpdateAt = i.UpdateAt
        //     }).ToList();
        //    return lst;
        //}

        public void InsertEmployees(int cancelID, int[] arID, int userID)
        {
            try
            {
                if (arID.Count() > 0 && cancelID > 0)
                {
                    ProfileCancelEmployee itm;

                    foreach (var i in arID)
                    {

                        itm = new ProfileCancelEmployee
                           {
                               ProfileCancelID = cancelID,
                               EmployeeID = i,


                               CreateAt = DateTime.Now,
                               CreateBy = userID

                           };
                        profileCancelEmployeeDA.Insert(itm);
                    }
                    profileCancelEmployeeDA.Save();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void InsertEmployee(int cancelID, int employeeID, int userID)
        {
            try
            {
                if (employeeID > 0 && cancelID > 0)
                {
                    ProfileCancelEmployee itm = new ProfileCancelEmployee
                        {
                            ProfileCancelID = cancelID,
                            EmployeeID = employeeID,

                            CreateAt = DateTime.Now,
                            CreateBy = userID

                        };
                    profileCancelEmployeeDA.Insert(itm);

                    profileCancelEmployeeDA.Save();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        public void InsertProfileCancel(ProfileCancelItem obj, int userID)
        {
            try
            {
                if (obj.CancelID > 0 && obj.ProfileID > 0)
                {
                    ProfileCancelProfile itm = new ProfileCancelProfile
                        {
                            ProfileCancelID = obj.CancelID,
                            ProfileID = obj.ProfileID,
                            PageTotal = obj.TotalPage,
                            Reason = obj.Reason,
                            CreateAt = DateTime.Now,
                            CreateBy = userID
                        };
                    profileCancelListDA.Insert(itm);
                    profileCancelListDA.Save();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateProfileCancel(ProfileCancelItem obj, int userID)
        {
            try
            {
                var itm = profileCancelListDA.GetById(obj.ID);
                itm.PageTotal = obj.TotalPage;
                itm.Reason = obj.Reason;

                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = userID;

                profileCancelListDA.Update(itm);

                profileCancelListDA.Save();

            }
            catch (Exception)
            {

                throw;
            }
        }


        public void UpdateVerifyCancel(int id, int userID)
        {
            try
            {
                var itm = profilesCancelDA.GetById(id);
                itm.IsCanceled = true;


                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = userID;

                profilesCancelDA.Update(itm);
                profilesCancelDA.Save();

            }
            catch (Exception)
            {

                throw;
            }
        }



        public ProfileCancelItem convertToProfileCancel(ProfileCancel i)
        {
            ProfileCancelItem obj = new ProfileCancelItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        CancelAt = i.Date,
                        MethodID = i.ProfileCancelMethodID,
                        CancelMethod = i.ProfileCancelMethod.Name,
                        Note = i.Note,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy,
                        IsCancel = i.IsCanceled,
                        Status = i.IsCanceled == true ? "<span style=\"color:red\">Đã hủy </span>" : "Chờ hủy"
                    };
            obj.FlagCancel = true;
            if (!i.IsCanceled && i.ProfileCancelProfiles.Count() > 0 && i.ProfileCancelEmployees.Count() > 0)
                obj.FlagCancel = false;

            obj.CreateByName = userSV.GetNameByUserID(i.CreateBy);
            obj.UpdateByName = userSV.GetNameByUserID(i.UpdateBy);

            return obj;

        }
        public ProfileCancelItem GetByName(string name)
        {
            try
            {
                name = name.Trim().ToLower();
                var i = profilesCancelDA.GetQuery(p => p.Name.Trim().ToLower().Equals(name)).FirstOrDefault();
                if (i != null)
                {
                    var obj = new ProfileCancelItem
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Note = i.Note,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt
                    };
                    return obj;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
