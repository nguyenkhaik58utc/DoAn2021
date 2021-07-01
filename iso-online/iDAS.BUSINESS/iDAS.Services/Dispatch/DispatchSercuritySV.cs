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
    public class DispatchSercuritySV
    {
        DispatchSecurityDA dispatchSercurityDA = new DispatchSecurityDA();
        HumanUserSV userSV = new HumanUserSV();
        public List<DispatchSecurityItem> GetAll(int page, int pageSize, out int totalCount)
        {
            try
            {
                var services = dispatchSercurityDA.GetQuery()
                            .Select(i => new DispatchSecurityItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Color = i.Color,
                                Note = i.Note,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt
                            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out totalCount)
                            .ToList();

                return services;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DispatchSecurityItem> GetAllIsActive()
        {
            try
            {
                var services = dispatchSercurityDA.GetQuery(t => t.IsActive)
                          .Select(i => new DispatchSecurityItem()
                          {
                              ID = i.ID,
                              Name = i.Name,
                              Color = i.Color,
                          })
                          .OrderByDescending(item => item.Name)
                          .ToList();

                return services;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DispatchSecurityItem> GetAll()
        {
            try
            {
                var services = dispatchSercurityDA.GetQuery()
                          .Select(i => new DispatchSecurityItem()
                          {
                              ID = i.ID,
                              Name = i.Name,
                              Color = i.Color,
                          })
                          .OrderByDescending(item => item.Name)
                          .ToList();

                return services;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Insert(DispatchSecurityItem obj)
        {
            try
            {
                var itm = new DispatchSecurity
                {
                    Name = obj.Name,
                    Color = obj.Color,
                    Note = obj.Note,
                    IsActive = obj.IsActive,

                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy
                };


                dispatchSercurityDA.Insert(itm);
                dispatchSercurityDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(DispatchSecurityItem obj)
        {
            try
            {
                var itm = dispatchSercurityDA.GetById(obj.ID);

                itm.Name = obj.Name;
                itm.Color = obj.Color;
                itm.Note = obj.Note;
                itm.IsActive = obj.IsActive;


                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;

                dispatchSercurityDA.Update(itm);
                dispatchSercurityDA.Save();
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
                var item = dispatchSercurityDA.GetById(id);
                dispatchSercurityDA.Delete(item);
                dispatchSercurityDA.Save();
            }
            catch (Exception)
            {

                throw;
            }

        }



        public DispatchSecurityItem GetByName(string name)
        {
            try
            {
                name = name.Trim().ToLower();
                var obj = dispatchSercurityDA.GetQuery(p => p.Name.Trim().ToLower().Equals(name))
                      .FirstOrDefault();
                if (obj != null)
                    return convertToDispatchSecurityItem(obj);

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DispatchSecurityItem GetByID(int id)
        {
            try
            {
                var obj = dispatchSercurityDA.GetById(id);
                if (obj != null)
                    return convertToDispatchSecurityItem(obj);
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool CheckNotIsUse(int id)
        {
            try
            {
                var dbo = dispatchSercurityDA.Repository;

                var obj = dbo.DispatchGoes.Where(p => p.DispatchSecurityID == id);
                if (obj.Count() > 0)
                    return false;


                var obj1 = dbo.DispatchToes.Where(p => p.DispatchSecurityID == id);
                if (obj1.Count() > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }



        private DispatchSecurityItem convertToDispatchSecurityItem(DispatchSecurity i)
        {
            try
            {
                var obj = new DispatchSecurityItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    Color = i.Color,
                    Note = i.Note,
                    IsActive = i.IsActive,
                    CreateAt = i.CreateAt,
                    CreateBy = i.CreateBy,
                    UpdateAt = i.UpdateAt,
                    UpdateBy = i.UpdateBy
                };
                if (obj != null)
                {
                    obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                    obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);
                }
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
