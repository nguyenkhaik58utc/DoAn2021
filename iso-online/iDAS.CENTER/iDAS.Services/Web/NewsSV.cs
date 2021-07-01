using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Utilities;

namespace iDAS.Services
{
    public class NewsSV : BaseService
    {
        NewsDA newsDA = new NewsDA();

        public List<NewsCategoryItem> GetNewsCategories()
        {
            var dbo = newsDA.Repository;
            var newsCategories = dbo.WebNewsCategories
                                .Where(i => i.IsActive)
                                .Where(i => !i.IsDelete)
                                .OrderBy(i => i.Position)
                                .Select(i => new NewsCategoryItem()
                                {
                                    ID = i.ID,
                                    Name = i.Name,
                                    Description = i.Description,
                                    IsActive = i.IsActive,
                                    CreateAt = i.CreateAt,
                                    UpdateAt = i.UpdateAt,
                                })
                                .OrderBy(i => i.CreateAt)
                                .ToList();
            return newsCategories;
        }
        public List<NewsItem> GetNews(int page, int pageSize, out int total,int newsCategoryId)
        {
            var news = newsDA.GetQuery()
                        .Where(i => i.WebNewsCategoryID == newsCategoryId)        
                        .Where(i => !i.IsDelete)
                        .Select(i => new NewsItem()
                        {
                            ID = i.ID,
                            Title = i.Title,
                            Description = i.Description,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return news;
        }
        public NewsItem GetNew(int id)
        {
            var newsItem = newsDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new NewsItem()
                            {
                                NewsCategoryID = i.WebNewsCategoryID,
                                Image = new AttachmentFileItem() { ID = i.FileID },
                                ID = i.ID,
                                Title = i.Title,
                                Description = i.Description,
                                Html = i.Html,
                                Tags = i.Tags,
                                Author = i.Author,
                                TotalView = i.TotalView,
                                IsFirst = i.IsFirst,
                                IsImportal = i.IsImportal,
                                IsActive = i.IsActive,
                                RefreshAt = i.RefreshAt,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return newsItem;
        }
        public void Insert(NewsItem item)
        {
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            var news = new WebNew()
            {
                WebNewsCategoryID = item.NewsCategoryID,
                Title = item.Title.Trim(),
                Description = item.Description,
                Html = item.Html,
                Tags = item.Tags,
                Author = item.Author,
                IsFirst = item.IsFirst,
                IsImportal = item.IsImportal,
                IsActive = item.IsActive,
                IsDelete = false,
                RefreshAt = item.RefreshAt,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            if (fileId != Guid.Empty)
            {
                news.FileID = fileId;
            }
            newsDA.Insert(news);
            newsDA.Save();
        }
        public void Update(NewsItem item)
        {
            var news = newsDA.GetById(item.ID);
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            news.WebNewsCategoryID = item.NewsCategoryID;
            news.Title = item.Title;
            news.Description = item.Description;
            news.Html = item.Html;
            news.Tags = item.Tags;
            news.Author = item.Author;
            news.IsFirst = item.IsFirst;
            news.IsImportal = item.IsImportal;
            news.IsActive = item.IsActive;
            news.RefreshAt = item.RefreshAt;
            news.UpdateAt = DateTime.Now;
            if (fileId != Guid.Empty)
            {
                news.FileID = fileId;
            }
            news.UpdateBy = User.ID;
            newsDA.Save();
        }
        public void Delete(int id)
        {
            var item = newsDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            newsDA.Save();
        }
        public bool CheckNameExist(string title)
        {
            var check = newsDA.GetQuery()
                        .Where(i => i.Title.ToUpper().Equals(title.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string title, int id)
        {
            var check = newsDA.GetQuery()
                        .Where(i => i.ID != id)
                        .Where(i => i.Title.ToUpper().Equals(title.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
