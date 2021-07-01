using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.Items;
using iDAS.DA;
namespace iDAS.Services
{
    public class PageSV
    {
        PageDA pageDA = new PageDA();
        public List<PageItem> GetAll(int page, int pageSize, out int total)
        {
            return pageDA.GetQuery(i => !i.IsDelete)
                         .Select(
                            item => new PageItem
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Code = item.Code,
                                CreateAt = item.CreateAt
                            }
                         )
                         .OrderBy(i => i.CreateAt)
                         .Page(page, pageSize, out total)
                         .ToList();
        }
        public PageItem GetById(int id)
        {
            return pageDA.GetQuery(i => i.ID == id)
                         .Select(
                            item => new PageItem
                                    {
                                        ID = item.ID,
                                        Name = item.Name,
                                        Code = item.Code,
                                        Description = item.Description,
                                        IsDelete = item.IsDelete,
                                        IsUse = item.IsUse
                                    }
                         ).First();
        }
        public bool Insert(PageItem item, int createBy)
        {
            pageDA.Insert(new WebPage
            {
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                IsDelete = item.IsDelete,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = createBy
            });
            pageDA.Save();
            return true;
        }
        public bool Update(PageItem PageItem, int updateBy)
        {
            var item = pageDA.GetById(PageItem.ID);
            item.Description = PageItem.Description;
            item.IsDelete = PageItem.IsDelete;
            item.IsUse = PageItem.IsUse;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = updateBy;
            pageDA.Update(item);
            pageDA.Save();
            return true;
        }
        public bool Delete(int Id)
        {
            pageDA.Delete(Id);
            pageDA.Save();
            return true;
        }
        public bool DeleteByListId(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            pageDA.DeleteRange(ids);
            pageDA.Save();
            return true;
        }
    }
}
