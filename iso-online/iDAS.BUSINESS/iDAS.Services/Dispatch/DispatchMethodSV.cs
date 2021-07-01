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
    public class DispatchMethodService
    {
        DispatchMethodDA methodDA = new DispatchMethodDA();
        HumanUserSV userSV = new HumanUserSV();
        public List<DispatchMethodItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var services = methodDA.GetQuery()
                    .Select(i => new DispatchMethodItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Note = i.Note,
                        IsActive = i.IsActive,
                        CreateAt = i.CreateAt,
                        CreateBy = i.CreateBy,
                        UpdateAt = i.UpdateAt,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();

            return services;
        }


        public List<DispatchMethodItem> GetAll()
        {
            var services = methodDA.GetQuery()
                    .Select(i => new DispatchMethodItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Note = i.Note
                    })
                    .OrderByDescending(item => item.Name)
                    .ToList();

            return services;
        }
        public List<DispatchMethodItem> GetAllIsActive()
        {
            var services = methodDA.GetQuery(t => t.IsActive)
                    .Select(i => new DispatchMethodItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Note = i.Note
                    })
                    .OrderByDescending(item => item.Name)
                    .ToList();

            return services;
        }
        public void Insert(DispatchMethodItem obj)
        {
            try
            {
                var itm = new DispatchMethod
                {
                    Name = obj.Name,
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

        public void Update(DispatchMethodItem obj)
        {
            var itm = methodDA.GetById(obj.ID);

            itm.Name = obj.Name;

            itm.Note = obj.Note;
            itm.IsActive = obj.IsActive;


            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;

            methodDA.Update(itm);
            methodDA.Save();
        }
        public void Delete(int id)
        {
            try
            {
                var item = methodDA.GetById(id);
                methodDA.Delete(item);
                methodDA.Save();
            }
            catch (Exception)
            {

                throw;
            }

        }



        public DispatchMethodItem GetByName(string name)
        {
            name = name.Trim().ToLower();
            var obj = methodDA.GetQuery(p => p.Name.Trim().ToLower().Equals(name))
                  .FirstOrDefault();
            if (obj != null)
                return convertToDispatchMethodItem(obj);

            return null;
        }

        public DispatchMethodItem GetByID(int id)
        {
            var obj = methodDA.GetById(id);
            if (obj != null)
                return convertToDispatchMethodItem(obj);

            return null;
        }


        public bool CheckNotIsUse(int id)
        {
            var dbo = methodDA.Repository;
            var obj = dbo.DispatchToes.Where(p => p.DispatchMethodID == id);
            if (obj.Count() > 0)
                return false;

            var obj1 = dbo.DispatchGoes.Where(p => p.DispatchMethodID == id);
            if (obj1.Count() > 0)
                return false;

            return true;
        }
        private DispatchMethodItem convertToDispatchMethodItem(DispatchMethod i)
        {
            var dbo = methodDA.Repository;
            var obj = new DispatchMethodItem()
            {
                ID = i.ID,
                Name = i.Name,
                Note = i.Note,
                IsActive = i.IsActive,
                CreateAt = i.CreateAt,
                CreateBy = i.CreateBy,
                UpdateAt = i.UpdateAt,
                UpdateBy = i.UpdateBy
            };
            if (obj != null)
            {
                obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
            }
            return obj;
        }
    }
}
