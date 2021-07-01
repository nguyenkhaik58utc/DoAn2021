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
    public class TopicSV
    {
        TopicDA TopicDA = new TopicDA();
        public List<TopicItem> GetAll(int page, int pageSize, out int total)
        {
            return TopicDA.GetQuery(i => !i.IsDelete)
                         .Select(
                            item => new TopicItem
                            {
                                ID = item.ID,
                                Title = item.Title,
                                Content = item.Content.Length < 100 ? item.Content : (item.Content.Substring(0, 100) + "..."),
                                CreateAt = item.CreateAt
                            }
                         )
                         .OrderBy(i => i.CreateAt)
                         .Page(page, pageSize, out total)
                         .ToList();
        }
        public TopicItem GetById(int id)
        {
            return TopicDA.GetQuery(i => i.ID == id)
                         .Select(
                            item => new TopicItem
                                    {
                                        ID = item.ID,
                                        Title = item.Title,
                                        Content = item.Content,
                                        IsDelete = item.IsDelete,
                                        IsUse = item.IsUse
                                    }
                         ).First();
        }
        public bool Insert(TopicItem item, int createBy)
        {
            TopicDA.Insert(new WebTopic
            {
                Title = item.Title,
                Content = item.Content,
                IsDelete = item.IsDelete,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = createBy
            });
            TopicDA.Save();
            return true;
        }
        public bool Update(TopicItem TopicItem, int updateBy)
        {
            var item = TopicDA.GetById(TopicItem.ID);
            item.Title = TopicItem.Title;
            item.Content = TopicItem.Content;
            item.IsDelete = TopicItem.IsDelete;
            item.IsUse = TopicItem.IsUse;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = updateBy;
            TopicDA.Update(item);
            TopicDA.Save();
            return true;
        }
        public bool Delete(int Id)
        {
            TopicDA.Delete(Id);
            TopicDA.Save();
            return true;
        }
        public bool DeleteByListId(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            TopicDA.DeleteRange(ids);
            TopicDA.Save();
            return true;
        }
    }
}
