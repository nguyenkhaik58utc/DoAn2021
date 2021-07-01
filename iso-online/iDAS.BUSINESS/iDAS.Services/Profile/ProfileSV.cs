using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class ProfileService
    {
        private ProfileDA profileDA = new ProfileDA();
        private ProfileBorrowDA proBorrowDA = new ProfileBorrowDA();
        private ProfileCancelListDA profileCancelDA = new ProfileCancelListDA();
        private HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        private HumanUserSV userSV = new HumanUserSV();
        public List<ProfileItem> GetAll(ModelFilter filter)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Select(i => i)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetAllByCateID(int page, int pageSize, out int totalCount, int cateId)
        {
            List<ProfileItem> lst = new List<ProfileItem>();
            List<Profile> services = new List<Profile>();
            services = profileDA.GetQuery(p => p.ProfileCategoryID == cateId)
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            if (services != null && services.Count() > 0)
            {
                foreach (var itm in services)
                {
                    var obj = convertToItem(itm);
                    lst.Add(obj);
                }
            }
            return lst;
        }
        public List<ProfileItem> GetAllByCateID(int cateId)
        {
            List<ProfileItem> lst = new List<ProfileItem>();
            List<Profile> services = new List<Profile>();
            services = profileDA.GetQuery(p => p.ProfileCategoryID == cateId)
                        .OrderByDescending(item => item.Name)
                        .ToList();
            if (services != null && services.Count() > 0)
            {
                foreach (var itm in services)
                {
                    var obj = convertToItem(itm);
                    lst.Add(obj);
                }
            }
            return lst;
        }
        public object GetAllByDeptID(int page, int pageSize, out int totalCount, int Cate, int depId)
        {
            var arIDBorrow = proBorrowDA.GetQuery(p => p.IsReturn == false).Select(p => p.ProfileID).ToArray();
            var arIDCancel = profileCancelDA.GetQuery().Select(p => p.ProfileID).ToArray();
            List<ProfileItem> lst = new List<ProfileItem>();
            List<Profile> services = new List<Profile>();
            if (arIDBorrow != null && arIDBorrow.Count() > 0)
            {
                services = profileDA.GetQuery(p => p.ProfileCategory.DepartmentID == depId)
                    .Where(p => !arIDBorrow.Contains(p.ID) && p.FormH)
                    .Where(p => p.IsUse)
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            }
            else
            {
                services = profileDA.GetQuery(p => p.ProfileCategory.DepartmentID == depId)
                    .Where(p => p.FormH)
                    .Where(p => p.IsUse)
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            }
            if (services != null && services.Count() > 0)
            {
                foreach (var itm in services)
                {
                    var obj = convertToItem(itm, Cate);
                    if (arIDCancel != null && arIDCancel.Count() > 0)
                    {
                        if (!arIDCancel.Contains(itm.ID))
                        {
                            lst.Add(obj);
                        }
                    }
                    else
                        lst.Add(obj);
                }
            }
            return lst;
        }
        public object GetAllCancel(int page, int pageSize, out int totalCount, int depID = 0, int id = 0)
        {
            var arIDBorrow = proBorrowDA.GetQuery(p => p.IsReturn == false).Select(p => p.ProfileID).ToArray();
            var arIDCancel = profileCancelDA.GetQuery().Select(p => p.ProfileID).ToArray();
            List<ProfileItem> lst = new List<ProfileItem>();
            List<Profile> services = new List<Profile>();
            if (arIDBorrow != null && arIDBorrow.Count() > 0)
            {
                services = profileDA.GetQuery(p => p.ProfileCategory.DepartmentID == depID)
                    .Where(p => !arIDBorrow.Contains(p.ID))
                    .Where(p => p.NotUseAt.HasValue)
                    .Where(p => p.StoreTime.HasValue)
                    .Where(p => !p.IsUse)
                    .OrderByDescending(item => item.Name)
                   // .Page(page, pageSize, out totalCount)
                    .ToList();
            }
            else
            {
                services = profileDA.GetQuery(p => p.ProfileCategory.DepartmentID == depID)
                    .Where(p => !p.IsUse)
                    .Where(p => p.NotUseAt.HasValue)
                    .Where(p => p.StoreTime.HasValue)
                    .Where(p => !p.IsUse)
                    .OrderByDescending(item => item.Name)
                   // .Page(page, pageSize, out totalCount)
                    .ToList();
            }
            if (services != null && services.Count() > 0)
            {
                foreach (var itm in services)
                {

                    var obj = convertToItem(itm);
                    if (arIDCancel != null && arIDCancel.Count() > 0)
                    {
                        if (!arIDCancel.Contains(itm.ID) && itm.StoreTime!=0 && DateTime.Now >= itm.NotUseAt.Value.AddMonths(System.Convert.ToInt32(itm.StoreTime.ToString())))
                        {
                            lst.Add(obj);
                        }
                    }
                    else if (itm.StoreTime!=0 && DateTime.Now >= itm.NotUseAt.Value.AddMonths(System.Convert.ToInt32(itm.StoreTime.ToString())))
                        lst.Add(obj);
                }
            }
            return lst.Page(page, pageSize, out totalCount);
        }
        public ProfileItem GetByID(int Id)
        {
            var dbo = profileDA.Repository;
            var item = profileDA.GetQuery(p => p.ID == Id)
                    .Select(i => new ProfileItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Code = i.Code,
                        ReceivedDepartmentID = i.ProfileCategory.DepartmentID,
                        CategoryID = i.ProfileCategoryID,
                        Category = i.ProfileCategory.Name,
                        SecurityID = i.ProfileSecurityID,
                        Security = i.ProfileSecurity.Name,
                        Note = i.Note,
                        IsUse = i.IsUse,
                        ReceivedAt = i.ReceivedAt,
                        NotUseAt = i.NotUseAt,
                        FormH = i.FormH,
                        FormS = i.FormS,
                        Color = i.ProfileSecurity.Color,
                        StorageRoomPosition = i.StoreRoomPosition,
                        StoragePartTime = i.StoreTime,
                        StorageRoomTime = i.StoreRoomTime,
                        ReceivedBy = i.EmployeeID,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                    })
                    .FirstOrDefault();
            if (item != null)
            {
                item.EmployeeInfo = sysUserSV.GetEmployeeView(item.ReceivedBy);
                if (item.EmployeeInfo != null)
                {
                    item.ReceivedName = item.EmployeeInfo.Name;
                    item.Position = iDAS.Utilities.Common.GetData.GetPositionInfo(item.EmployeeInfo.Department, item.EmployeeInfo.Role);
                }
                item.CreateByName = userSV.GetNameByUserID(item.CreateBy);
                item.UpdateByName = userSV.GetNameByUserID(item.UpdateBy);
                item.StorageFormID = getDocIssForm(item.FormH, item.FormS);
                item.AttachmentFile = new AttachmentFileItem()
                {
                    Files = dbo.ProfileAttachmentFiles.Where(i => i.ProfileID == Id)
                        .Select(i => i.AttachmentFileID).ToList()
                };
            }
            return item;
        }
        public int Insert(ProfileItem obj)
        {
            try
            {
                var itm = convertToProfile(obj);
                profileDA.Insert(itm);
                profileDA.Save();
                new FileSV().Upload<ProfileAttachmentFile>(obj.AttachmentFile, itm.ID);
                return itm.ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private ProfileItem convertToItem(Profile item, int cateBorrID = 0)
        {
            var dbo = profileDA.Repository;
            var obj = new ProfileItem()
            {
                ID = item.ID,
                Code = item.Code.Trim(),
                Name = item.Name.Trim(),
                CategoryID = item.ProfileCategoryID,
                NotUseAt = item.NotUseAt,
                CategoryBorowID = cateBorrID,
                Category = item.ProfileCategory.Name,
                ReceivedBy = item.EmployeeID,
                ReceivedAt = item.ReceivedAt,
                SecurityID = item.ProfileSecurityID,
                Security = item.ProfileSecurity.Name,
                Color = item.ProfileSecurity.Color,
                FlagBorrow = item.FormH ? true : false,
                StoragePartTime = item.StoreTime,
                StoreRoomTime = item.StoreRoomTime,
                StorageRoomPosition = item.StoreRoomPosition,
                IsDelete = item.IsDelete,
                Note = item.Note,
                FormH = item.FormH,
                FormS = item.FormS,
                CreateAt = item.CreateAt,
                UpdateAt = item.UpdateAt,
                CreateBy = item.CreateBy,
                UpdateBy = item.UpdateBy,
                AttachmentFileIDs = dbo.ProfileAttachmentFiles.Where(t => t.ProfileID == item.ID).Select(x => x.AttachmentFileID).ToList()
            };
            if (obj != null)
            {
                obj.EmployeeInfo = sysUserSV.GetEmployeeView(obj.ReceivedBy);
                obj.ReceivedName = sysUserSV.GetNameByID(obj.ReceivedBy);
                int code = 0;
                obj.Status = getStatus(obj.ID, ref code);
                obj.StatusCode = code;
            }
            return obj;
        }
        private Profile convertToProfile(ProfileItem obj)
        {
            try
            {
                var itm = new Profile
                {
                    Name = obj.Name.Trim(),
                    Code = obj.Code.Trim(),
                    ProfileCategoryID = obj.CategoryID,
                    FormH = (obj.StorageFormInt == (int)StorageForm.HardCopy || obj.StorageFormInt == (int)StorageForm.Both) ? true : false,
                    FormS = (obj.StorageFormInt == (int)StorageForm.SoftCopy || obj.StorageFormInt == (int)StorageForm.Both) ? true : false,
                    ProfileSecurityID = obj.SecurityID,
                    IsDelete = false,
                    EmployeeID = obj.EmployeeInfo.ID,
                    ReceivedAt = obj.ReceivedAt,
                    IsUse = obj.IsUse,
                    NotUseAt = obj.NotUseAt,
                    StoreTime = obj.StoragePartTime,
                    StoreRoomTime = obj.StorageRoomTime,
                    StoreRoomPosition = obj.StorageRoomPosition,
                    Note = obj.Note,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy

                };
                return itm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(ProfileItem obj)
        {
            try
            {
                var item = profileDA.GetById(obj.ID);
                item.Name = obj.Name.Trim();
                item.Code = obj.Code.Trim();
                item.ProfileCategoryID = obj.CategoryID;
                item.FormH = (obj.StorageFormInt == (int)StorageForm.HardCopy || obj.StorageFormInt == (int)StorageForm.Both) ? true : false;
                item.FormS = (obj.StorageFormInt == (int)StorageForm.SoftCopy || obj.StorageFormInt == (int)StorageForm.Both) ? true : false;
                item.ProfileSecurityID = obj.SecurityID;
                item.EmployeeID = obj.EmployeeInfo.ID;
                item.ReceivedAt = obj.ReceivedAt;
                item.StoreTime = obj.StoragePartTime;
                item.IsUse = obj.IsUse;
                item.NotUseAt = obj.NotUseAt;
                item.StoreRoomTime = obj.StorageRoomTime;
                item.StoreRoomPosition = obj.StorageRoomPosition;
                item.Note = obj.Note;
                item.UpdateAt = DateTime.Now;
                item.UpdateBy = obj.CreateBy;
                new FileSV().Upload<ProfileAttachmentFile>(obj.AttachmentFile, obj.ID);
                profileDA.Update(item);
                profileDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string getStatus(int id, ref int code)
        {
            code = (int)ProfileStatus.New;
            string status = "<span style=\"color:#045fb8\">Mới</span>";
            var itm = proBorrowDA.GetQuery(p => p.ProfileID == id).OrderByDescending(p => p.CreateAt);
            if (itm.Count() > 0)
            {
                bool isReturn = itm.FirstOrDefault().IsReturn;
                if (!isReturn)
                {
                    if (itm.FirstOrDefault().LimitAt < DateTime.Now)
                    {
                        code = (int)ProfileStatus.OverPaid;
                        status = "<span style=\"color:#8e210b\">Mượn quá hạn</span>";
                    }
                    else
                    {
                        code = (int)ProfileStatus.Borrow;
                        status = "<span style=\"color:#293955\">Đang mượn</span>";
                    }
                }
                else
                {
                    code = (int)ProfileStatus.Paid;
                    status = "Đã trả";
                }
            }
            var profile = profileDA.GetById(id);
            if (!profile.IsUse)
            {
                status = "<span style=\"color:#ccc\">Không sử dụng</span>";
            }
            var cancel = profileCancelDA.GetQuery(p => p.ProfileID == id);
            if (cancel.Count() > 0)
            {
                if (cancel.FirstOrDefault().ProfileCancel.IsCanceled)
                {
                    code = (int)ProfileStatus.Destroy;
                    status = "<span style=\"color:red\">Đã hủy</span>";
                }
                else
                {
                    code = (int)ProfileStatus.PreDestroy;
                    status = "<span style=\"color:blue\">Chờ hủy</span>";
                }
            }
            return status;
        }
        private string getDocIssForm(bool isH, bool isS)
        {
            if (isH && isS)
                return StorageForm.SoftCopy.ToString() + "," + StorageForm.HardCopy.ToString();
            else if (isS)
                return StorageForm.SoftCopy.ToString();
            else if (isH)
                return StorageForm.HardCopy.ToString();
            return "";
        }
        public ProfileItem GetByCode(string code)
        {
            code = code.Trim().ToLower();
            var services = profileDA.GetQuery(p => p.Code.Trim().ToLower().Equals(code))
                     .Select(i => new ProfileItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                         Code = i.Code,

                         Note = i.Note,
                         FormH = i.FormH,
                         FormS = i.FormS,
                         //Sercurity= 

                         CreateAt = i.CreateAt,
                         UpdateAt = i.UpdateAt
                     }).FirstOrDefault();
            return services;
        }
        public string GetNameByID(int id)
        {
            var item = profileDA.GetById(id);
            if (item != null)
                return item.Name;
            return string.Empty;
        }
        #region Dùng cho thống kê
        /// <summary>
        /// Danh mục
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="securityId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<ProfileItem> GetUseFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategoryID == cateId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategoryID == cateId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowOverTimeFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategoryID == cateId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetNotUseFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                   .Where(t => t.ProfileCategoryID == cateId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetCancelFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategoryID == cateId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetRequestCancelFromList(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategoryID == cateId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        /// <summary>
        /// Mức độ mật
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<ProfileItem> GetUseFromSecurityLevel(ModelFilter filter, int securityId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileSecurityID == securityId)
                     .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Select(i => i)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowFromSecurityLevel(ModelFilter filter, int securityId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileSecurityID == securityId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowOverTimeFromSecurityLevel(ModelFilter filter, int securityId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileSecurityID == securityId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetNotUseFromSecurityLevel(ModelFilter filter, int securityId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                   .Where(t => t.ProfileSecurityID == securityId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetCancelFromSecurityLevel(ModelFilter filter, int securityId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileSecurityID == securityId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                    .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                    || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                    || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                    || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                    || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                    )
                                    .Count() > 0
                                )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetRequestCancelFromSecurityLevel(ModelFilter filter, int securityId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileSecurityID == securityId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        /// <summary>
        /// Dùng cho phòng ban
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="securityId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<ProfileItem> GetUseFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategory.DepartmentID == departmentId)
                     .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Select(i => i)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategory.DepartmentID == departmentId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowOverTimeFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategory.DepartmentID == departmentId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetNotUseFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                   .Where(t => t.ProfileCategory.DepartmentID == departmentId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetCancelFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategory.DepartmentID == departmentId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;

        }
        public List<ProfileItem> GetRequestCancelFromDepartment(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.ProfileCategory.DepartmentID == departmentId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        /// <summary>
        /// Cho parent
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<ProfileItem> GetUseFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                      .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Select(i => i)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowOverTimeFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetNotUseFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetCancelFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetRequestCancelFromParentDepartment(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        /// <summary>
        /// Thong ke theo nguoi luu tru
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="securityId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<ProfileItem> GetUseFromReceived(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.EmployeeID == employeeId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowFromReceived(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.EmployeeID == employeeId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetBorrowOverTimeFromReceived(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.EmployeeID == employeeId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt) || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt) || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt) || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetNotUseFromReceived(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.EmployeeID == employeeId)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetCancelFromReceived(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.EmployeeID == employeeId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        public List<ProfileItem> GetRequestCancelFromReceived(ModelFilter filter, int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<ProfileItem> lstItem = new List<ProfileItem>();
            var lst = profileDA.GetQuery()
                    .Where(t => t.EmployeeID == employeeId)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Filter(filter)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var itm in lst)
                {
                    var obj = convertToItem(itm);
                    lstItem.Add(obj);
                }
                return lstItem;
            }
            return lstItem;
        }
        #endregion;
    }
}