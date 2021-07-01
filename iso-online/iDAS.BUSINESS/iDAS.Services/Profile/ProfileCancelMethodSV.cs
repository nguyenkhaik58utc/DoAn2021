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
    public class ProfileCancelMethodService
    {
        ProfileCancelMethodDA methodDA = new ProfileCancelMethodDA();
        ProfileCancelDA profileCancelDA = new ProfileCancelDA();
        HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        HumanUserSV userSV = new HumanUserSV();

        public ProfileCategoryItem GetByID(int id)
        {
            try
            {
                var itm = methodDA.GetById(id);
                var obj = new ProfileCategoryItem
                {
                    ID = itm.ID,
                    Name = itm.Name,
                    Note = itm.Note,
                    IsActive = itm.IsActive,
                    CreateAt = itm.CreateAt,
                    CreateBy = itm.CreateBy,
                    UpdateAt = itm.UpdateAt,
                    UpdateBy = itm.UpdateBy

                };
                if (obj != null)
                {
                    obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                    obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);

                }
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProfileCategoryItem> GetAll(int page, int pageSize, out int totalCount)
        {
            try
            {
                var services = methodDA.GetQuery()
                            .Select(i => new ProfileCategoryItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Note = i.Note,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                UpdateAt = i.UpdateAt,
                                CreateBy = i.CreateBy,
                                UpdateBy = i.UpdateBy
                            })
                            .OrderByDescending(item => item.CreateAt)
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
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProfileCategoryItem> GetAll()
        {
            try
            {
                var services = methodDA.GetQuery(p => p.IsActive == true)
                            .Select(i => new ProfileCategoryItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Note = i.Note,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                UpdateAt = i.UpdateAt,
                                CreateBy = i.CreateBy,
                                UpdateBy = i.UpdateBy
                            })
                            .OrderByDescending(item => item.Name)
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
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckProfileCancelByMethodID(int id)
        {
            try
            {
                var itm = profileCancelDA.GetQuery(p => p.ProfileCancelMethodID == id && !p.IsCanceled);
                if (itm.Count() > 0)
                    return false;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Insert(ProfileCategoryItem obj)
        {
            try
            {
                var itm = new ProfileCancelMethod
                {
                    Name = obj.Name.Trim(),
                    Note = obj.Note,
                    IsActive = obj.IsActive,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy
                };
                methodDA.Insert(itm);
                methodDA.Save();
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
                var itm = methodDA.GetById(obj.ID);

                itm.Name = obj.Name.Trim();
                itm.IsActive = obj.IsActive;
                itm.Note = obj.Note;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                methodDA.Update(itm);
                methodDA.Save();
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
                var itm = methodDA.GetById(id);
                methodDA.Delete(itm);
                methodDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProfileCategoryItem GetByName(string name)
        {
            name = name.Trim().ToLower();
            var obj = methodDA.GetQuery(p => p.Name.Trim().ToLower().Equals(name)).Select(
                i => new ProfileCategoryItem
                {

                    ID = i.ID,
                    Name = i.Name,
                    Note = i.Note,
                    IsActive = i.IsActive,
                }).FirstOrDefault();
            return obj;
        }
    }
}
