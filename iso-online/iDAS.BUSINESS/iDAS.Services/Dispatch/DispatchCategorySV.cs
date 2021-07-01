using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Items;
using iDAS.DA;
using iDAS.Core;

namespace iDAS.Services
{
    public class DispatchCategorySV
    {
        private DispatchCategoryDA dispatchCategoryDA = new DispatchCategoryDA();
        private HumanEmployeeSV sysUserSV = new HumanEmployeeSV();
        private HumanUserSV userSV = new HumanUserSV();
        public List<DispatchCategoryItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var lst = dispatchCategoryDA.GetQuery(t => !t.IsDelete)
                    .Select(i => new DispatchCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        IsUse = i.IsUse,
                        Note = i.Note,
                        CreateBy = i.CreateBy,
                        UpdateBy = i.UpdateBy,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (var obj in lst)
                {
                    obj.CreateByName = userSV.GetNameByUserID(obj.CreateBy);
                    obj.UpdateByName = userSV.GetNameByUserID(obj.UpdateBy);
                }
            }
            return lst;
        }
        public List<DispatchCategoryItem> GetAll()
        {
            var obj = dispatchCategoryDA.GetQuery(t => !t.IsDelete)
                     .Select(i => new DispatchCategoryItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                     })
                     .OrderByDescending(item => item.Name)
                     .ToList();
            return obj;
        }
        public List<DispatchCategoryItem> GetAllIsActive()
        {
            var obj = dispatchCategoryDA.GetQuery(t => !t.IsDelete && t.IsUse)
                     .Select(i => new DispatchCategoryItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                     })
                     .OrderByDescending(item => item.Name)
                     .ToList();
            return obj;
        }
        public DispatchCategoryItem GetByID(int id)
        {
            var i = dispatchCategoryDA.GetById(id);
            var obj = new DispatchCategoryItem()
            {
                ID = i.ID,
                Name = i.Name,
                Note = i.Note,
                IsUse = i.IsUse,
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
        public void Insert(DispatchCategoryItem obj)
        {
            try
            {
                var itm = new DispatchCategory
                {
                    IsDelete = false,
                    IsUse = obj.IsUse,
                    Name = obj.Name,
                    Note = obj.Note,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy

                };
                dispatchCategoryDA.Insert(itm);
                dispatchCategoryDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(DispatchCategoryItem obj)
        {
            try
            {
                var itm = dispatchCategoryDA.GetById(obj.ID);
                itm.IsUse = obj.IsUse;
                itm.Name = obj.Name;
                itm.Note = obj.Note;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                dispatchCategoryDA.Update(itm);
                dispatchCategoryDA.Save();
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
                var item = dispatchCategoryDA.GetById(id);
                item.IsDelete = true;
                dispatchCategoryDA.Update(item);
                dispatchCategoryDA.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DispatchCategoryItem GetByName(string name)
        {
            try
            {
                name = name.Trim().ToLower();
                var obj = dispatchCategoryDA.GetQuery(p => p.Name.Trim().ToLower().Equals(name) && !p.IsDelete)
                     .Select(i => new DispatchCategoryItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                         Note = i.Note,
                         CreateBy = i.CreateBy,
                         UpdateBy = i.UpdateBy,
                         CreateAt = i.CreateAt,
                         UpdateAt = i.UpdateAt
                     }).FirstOrDefault();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CheckNotIsUse(int id)
        {
            var dbo = dispatchCategoryDA.Repository;
            var obj1 = dbo.DispatchGoes.Where(p => p.DispatchCategoryID == id);
            if (obj1.Count() > 0)
                return false;
            var obj2 = dbo.DispatchToes.Where(p => p.DispatchCategoryID == id);
            if (obj2.Count() > 0)
                return false;
            return true;
        }
    }
}
