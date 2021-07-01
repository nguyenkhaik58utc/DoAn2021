using System;
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
    public class ProfileSecuritySV
    {
        ProfileSecurityDA profileSercurityDA = new ProfileSecurityDA();
        public List<ProfileSecurityItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var services = profileSercurityDA.GetQuery()
                    .Select(i => new ProfileSecurityItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Color = i.Color,
                        Note = i.Note,
                        IsActive = i.IsActive
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();

            return services;
        }
        public List<ProfileSecurityItem> GetAll()
        {
            var services = profileSercurityDA.GetQuery(p => p.IsActive)
                    .Select(i => new ProfileSecurityItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Color = i.Color,
                    })
                    .OrderByDescending(item => item.Name)
                    .ToList();
            return services;
        }
        public bool Insert(ProfileSecurityItem obj)
        {
            if(profileSercurityDA.GetQuery(i=>i.Name.Trim().ToUpper()==obj.Name.Trim().ToUpper()).FirstOrDefault()!=null)
            {
                return false;
            }
            var itm = new ProfileSecurity
            {
                Name = obj.Name,
                Color = obj.Color,
                Note = obj.Note,
                IsActive = obj.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = obj.CreateBy
            };
            profileSercurityDA.Insert(itm);
            profileSercurityDA.Save();
            return true;
        }
        public bool Update(ProfileSecurityItem obj)
        {
            var itm = profileSercurityDA.GetById(obj.ID);
            if (itm.Name != obj.Name && profileSercurityDA.GetQuery(i=>i.Name.Trim().ToUpper()==obj.Name).FirstOrDefault() !=null)
            {
                return false;
            }
            itm.Name = obj.Name;
            itm.Color = obj.Color;
            itm.Note = obj.Note;
            itm.IsActive = obj.IsActive;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            profileSercurityDA.Update(itm);
            profileSercurityDA.Save();
            return true;
        }
        public bool Delete(int id)
        {
            ProfileDA profileDA = new ProfileDA();
            var profileUsing = profileDA.GetQuery(p => p.ProfileSecurityID == id);
            if (profileUsing.Count() > 0)
                return false;
            var item = profileSercurityDA.GetById(id);
            profileSercurityDA.Delete(item);
            profileSercurityDA.Save();
            return true;
        }
        public ProfileSecurityItem GetByID(int id)
        {
            var obj = profileSercurityDA.GetById(id);
            if (obj != null)
                return ConvertToProfileSecurityItem(obj);
            return null;
        }
        private ProfileSecurityItem ConvertToProfileSecurityItem(ProfileSecurity profileSecurity)
        {
            var obj = new ProfileSecurityItem()
            {
                ID = profileSecurity.ID,
                Name = profileSecurity.Name,
                Color = profileSecurity.Color,
                Note = profileSecurity.Note,
                IsActive = profileSecurity.IsActive,
                CreateAt = profileSecurity.CreateAt,
                CreateBy = profileSecurity.CreateBy,
                UpdateAt = profileSecurity.UpdateAt,
                UpdateBy = profileSecurity.UpdateBy
            };
            if (obj != null)
            {
                HumanUserSV userSV = new HumanUserSV();
                obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);
            }
            return obj;
        }
    }
}
