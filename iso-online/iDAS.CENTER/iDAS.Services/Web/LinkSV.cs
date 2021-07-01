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
    public class LinkSV
    {
        private LinkDA LinkDA = new LinkDA();
        public List<LinkItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var result = LinkDA.GetQuery(i => !i.IsDelete && i.IsUse)
                 .Select(item => new LinkItem()
                 {
                     ID = item.ID,
                     Text = item.Text,
                     Link = item.Link,
                     CategoryID = item.WebLinkCategoryID.Value,
                     CategoryName = item.WebLinkCategory.Name,
                     IsUse = item.IsUse,
                     Position = item.Position
                 })
                 .OrderBy(i => i.Position)
                 .Page(page, pageSize, out totalCount)
                 .ToList();
            return result;
        }
        public List<LinkItem> GetByCategoryID(int id)
        {
            var result = LinkDA.GetQuery(i => !i.IsDelete && i.IsUse && i.WebLinkCategoryID == id)
                .Select(item => new LinkItem()
                {
                    ID = item.ID,
                    Text = item.Text,
                    Link = item.Link,
                    Position = item.Position
                })
                .OrderBy(i => i.Position)
                .ToList();
            return result;
        }
        public LinkItem GetById(int id)
        {
            var result = LinkDA.GetQuery(i => i.ID == id)
                  .Select(item => new LinkItem()
                  {
                      ID = item.ID,
                      Text = item.Text,
                      Link = item.Link,
                      CategoryID = item.WebLinkCategoryID.Value,
                      CategoryName = item.WebLinkCategory.Name,
                      IsUse = item.IsUse,
                      Position = item.Position
                  }).First();
            return result;
        }
        public bool Insert(LinkItem item, int p)
        {
            LinkDA.Insert(new WebLink
            {
                ID = item.ID,
                Text = item.Text,
                Link = item.Link,
                WebLinkCategoryID = item.CategoryID,
                IsUse = item.IsUse,
                Position = item.Position
            });
            LinkDA.Save();
            return true;
        }
        public bool Update(LinkItem item, int p)
        {
            var updateData = LinkDA.GetById(item.ID);
            updateData.Text = item.Text;
            updateData.Link = item.Link;
            updateData.WebLinkCategoryID = item.CategoryID;
            updateData.IsUse = item.IsUse;
            updateData.Position = item.Position;
            LinkDA.Save();
            return true;
        }
        public void Delete(int id)
        {
            LinkDA.Delete(id);
            LinkDA.Save();
        }
        public bool DeleteByListId(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            LinkDA.DeleteRange(ids);
            LinkDA.Save();
            return true;
        }
    }
}