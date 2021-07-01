using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;
using iDAS.Core;
namespace iDAS.Services
{
    public class LinkCategorySV
    {
        private LinkCategoryDA LinkCategoryDA = new LinkCategoryDA();
        public object GetAll()
        {
            var result = LinkCategoryDA.GetQuery(i => !i.IsDelete)
                .Select(item => new LinkCategoryItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    IsHorizon = item.IsHorizon,
                    Position = item.Position,
                    IsUse = item.IsUse,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt
                })
                .OrderByDescending(item => item.Name)
                .ToList();
            return result;
        }
        public List<LinkCategoryItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var result = LinkCategoryDA.GetQuery(i => !i.IsDelete)
                .Select(item => new LinkCategoryItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    IsHorizon = item.IsHorizon,
                    Position = item.Position,
                    IsUse = item.IsUse,
                    IsDelete = item.IsDelete,
                    CreateAt = item.CreateAt
                })
                .OrderByDescending(item => item.CreateAt)
                .Page(page, pageSize, out totalCount)
                .ToList();
            return result;
        }
        public List<LinkCategoryItem> GetUse()
        {
            var result = LinkCategoryDA.GetQuery(i => i.IsUse && !i.IsDelete)
                .Select(item => new LinkCategoryItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    IsHorizon = item.IsHorizon,
                    IsUse = item.IsUse,
                    Position = item.Position
                })
                .OrderBy(i => i.Position)
                .ToList();
            return result;
        }
        public List<LinkCategoryItem> GetByVertical()
        {
            var result = LinkCategoryDA.GetQuery(i => !i.IsDelete && i.IsUse && !i.IsHorizon)
                .Select(item => new LinkCategoryItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    IsUse = item.IsUse,
                    Position = item.Position,
                })
                .OrderBy(i => i.Position)
                .ToList();
            return result;
        }
        public List<LinkCategoryItem> GetByHorizon()
        {
            var result = LinkCategoryDA.GetQuery(i => !i.IsDelete && i.IsUse && i.IsHorizon)
                .Select(item => new LinkCategoryItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    IsUse = item.IsUse,
                    Position = item.Position,
                })
                .OrderBy(i => i.Position)
                .ToList();
            return result;
        }
        public LinkCategoryItem GetById(int id)
        {
            var result = LinkCategoryDA.GetQuery(i => i.ID == id)
                .Select(item => new LinkCategoryItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Position = item.Position,
                    IsHorizon = item.IsHorizon,
                    IsUse = item.IsUse,
                    CreateAt = item.CreateAt,
                })
                .First();
            return result;
        }
        public void Insert(LinkCategoryItem obj)
        {
            LinkCategoryDA.Insert(new WebLinkCategory()
                {
                    Name = obj.Name,
                    Position = obj.Position,
                    IsDelete = obj.IsDelete,
                    IsHorizon = obj.IsHorizon,
                    IsUse = obj.IsUse,
                    CreateAt = obj.CreateAt,
                    CreateBy = obj.CreateBy,
                });
            LinkCategoryDA.Save();
        }
        public void Update(LinkCategoryItem obj)
        {
            var objUpdate = LinkCategoryDA.GetById(obj.ID);
            objUpdate.Name = obj.Name;
            objUpdate.Position = obj.Position;
            objUpdate.IsDelete = obj.IsDelete;
            objUpdate.IsHorizon = obj.IsHorizon;
            objUpdate.IsUse = obj.IsUse;
            objUpdate.UpdateAt = obj.UpdateAt;
            objUpdate.UpdateBy = obj.UpdateBy;
            LinkCategoryDA.Save();
        }
        public void Delete(int id)
        {
            LinkCategoryDA.Delete(id);
            LinkCategoryDA.Save();
        }
    }
}
