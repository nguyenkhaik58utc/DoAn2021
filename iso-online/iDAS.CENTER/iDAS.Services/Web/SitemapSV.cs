using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class SitemapSV:BaseService
    {
        private SitemapDA sitemapDA = new SitemapDA();
        private IEnumerable<WebSitemap> getSitemapChilds(IEnumerable<WebSitemap> query, int parentID)
        {
            var list1 = query.Where(i => i.ParentID == parentID).Where(i => !i.IsDelete);
            var list2 = query.Where(i => i.ID == parentID).Where(i => !i.IsDelete);
            list2.Concat(list1);
            return list2.Concat(list1.SelectMany(i => getSitemapChilds(query, i.ID)));
        }
        public List<SitemapItem> GetSitemaps(int parentId)
        {
            var dbo = sitemapDA.Repository;
            var lst = sitemapDA.GetQuery()
                            .Where(i => i.ParentID == parentId)
                            .Where(i => !i.IsDelete)
                            .OrderBy(i => i.Position)
                            .Select(item => new SitemapItem()
                            {
                                ID = item.ID,
                                Text = item.Text,
                                Tooltip = item.Tooltip,
                                IsPageDynamic = item.IsPageDynamic,
                                IsMenuTop = item.IsMenuTop,
                                IsMenuRight = item.IsMenuRight,
                                IsActive = item.IsActive,
                                IsDelete = item.IsDelete,
                                IsParent = dbo.WebSitemaps.Where(i => i.ParentID == item.ID).Count() > 0,
                            })
                            .ToList();
            return lst;
        }
        public SitemapItem GetSitemap(int id)
        {
            var item = sitemapDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new SitemapItem()
                        {
                            ID = i.ID,
                            ParentID = i.ParentID,
                            Text = i.Text,
                            Tooltip = i.Tooltip,
                            Image = new AttachmentFileItem(){ ID = i.FileID }, 
                            Url = i.Url,
                            IsPageDynamic = i.IsPageDynamic,
                            Html = i.Html,
                            Layout = i.Layout,
                            Script = i.Script,
                            IsMenuRight = i.IsMenuRight,
                            IsMenuTop = i.IsMenuTop,
                            Position = i.Position,
                            IsActive = i.IsActive,
                            IsDelete = i.IsDelete,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(SitemapItem item)
        {
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            var record = new WebSitemap()
            {
                ParentID = item.ParentID,
                Text = item.Text,
                Tooltip = item.Tooltip,
                Url = item.Url,
                Html = item.Html,
                Script = item.Script,
                IsMenuRight = item.IsMenuRight,
                IsActive = item.IsActive,
                IsMenuTop = item.IsMenuTop,
                IsPageDynamic = item.IsPageDynamic,
                Position = item.Position,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
            };
            if (fileId != Guid.Empty){
                record.FileID = fileId;
            }
            sitemapDA.Insert(record);
            sitemapDA.Save();
            item.ID = record.ID;
        }
        public void Update(SitemapItem item)
        {
            var Sitemap = sitemapDA.GetById(item.ID);
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            Sitemap.Text = item.Text;
            Sitemap.Tooltip = item.Tooltip;
            Sitemap.Html = item.Html;
            Sitemap.Layout = item.Layout;
            Sitemap.Url = item.Url;
            Sitemap.Script = item.Script;
            Sitemap.IsMenuRight = item.IsMenuRight;
            Sitemap.IsMenuTop = item.IsMenuTop;
            Sitemap.IsPageDynamic = item.IsPageDynamic;
            Sitemap.IsActive = item.IsActive;
            Sitemap.Position = item.Position;
            Sitemap.IsDelete = item.IsDelete;
            Sitemap.CreateAt = item.CreateAt;
            Sitemap.CreateBy = item.CreateBy;
            Sitemap.UpdateAt = DateTime.Now;
            Sitemap.UpdateBy = User.ID;
            if (fileId != Guid.Empty)
            {
                Sitemap.FileID = fileId;
            }
            sitemapDA.Update(Sitemap);
            sitemapDA.Save();
        }
        public void Delete(int id)
        {
            var dbo = sitemapDA.Repository;
            var sitemaps = getSitemapChilds(dbo.WebSitemaps, id);
            foreach (var sitemap in sitemaps)
            {
                sitemap.IsDelete = true;
                sitemap.UpdateAt = DateTime.Now;
                sitemap.UpdateBy = User.ID;
            }
            sitemapDA.Save();
        }
    }
}
