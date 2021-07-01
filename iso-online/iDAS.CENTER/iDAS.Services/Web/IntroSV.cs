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
    public class WebIntroSV
    {
        private WebIntroDA WebIntroDA = new WebIntroDA();
        public List<IntroItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var result = WebIntroDA.GetQuery()
                 .Select(item => new IntroItem()
                 {
                     ID = item.ID,
                     Title = item.Title,
                     Content = item.Content,
                     Order = item.Order,
                     IsShow = item.IsShow,
                     CreateAt = item.CreateAt
                 })
                 .OrderBy(i => i.CreateAt)
                 .Page(page, pageSize, out totalCount)
                 .ToList();
            return result;
        }
        public IntroItem GetById(int id)
        {
            var item = WebIntroDA.GetQuery(i => i.ID == id).FirstOrDefault();
            if (item != null)
            {
              var result=new IntroItem()
                  {
                      ID = item.ID,
                      Title = item.Title,
                      Content = item.Content,
                      Order = item.Order,
                      IsShow = item.IsShow,
                  };
              if (item.FileID != null)
              {
                  result.File = new AttachmentFileItem()
                  {
                      Files = new List<Guid>() { (Guid)item.FileID },
                  };  
              }
              return result;
            }
            return null;
        }
        public bool Insert(IntroItem item)
        {
            
            var itemInsert = new WebIntroduction
            {
                ID = item.ID,
                Title = item.Title,
                Content = item.Content,
                IsShow = item.IsShow,
                Order= item.Order,
                CreateAt = item.CreateAt,
                CreateBy = item.CreateBy,
            };
            //if(item.File.FileAttachments.Count!=0)
            //{
            //    var fileId=new FileSV().InsertRange(item.File.FileAttachments, (int)item.CreateBy).First();
            //    itemInsert.FileID = fileId;
            //}
            WebIntroDA.Insert(itemInsert);
            WebIntroDA.Save();
            return true;
        }
        public bool Update(IntroItem item)
        {
            var updateData = WebIntroDA.GetById(item.ID);
            //if (item.File.FileRemoves.Count > 0)
            //{
            //    new FileSV().DeleteRange(item.File.FileRemoves);
            //    updateData.FileID = null;
            //}
            //if (item.File.FileAttachments.Count > 0)
            //{
            //    if(updateData.FileID == null)
            //    {
            //        var fileID = new FileSV().InsertRange(item.File.FileAttachments, (int)item.UpdateBy).First();
            //        updateData.FileID = fileID;
            //    }
            //    else
            //    {
            //        var file = item.File.FileAttachments.First();
            //        if(file !=null)
            //        {
            //            var fileItem = new FileItem()
            //            {
            //                ID = (Guid)updateData.FileID,
            //                Extension = string.IsNullOrWhiteSpace(file.FileName) ?
            //                   "" : file.FileName.Substring(file.FileName.IndexOf('.') + 1),
            //                Size = file.ContentLength,
            //                Type = file.ContentType,
            //                Data = iDAS.Utilities.CustomConvert.ToByteArray(file.InputStream),
            //            };
            //            new FileSV().Update(fileItem, (int)updateData.UpdateBy);
            //        }
            //    }
            //}
            updateData.Title = item.Title;
            updateData.Content = item.Content;
            updateData.IsShow = item.IsShow;
            updateData.Order = item.Order;
            updateData.UpdateAt = item.UpdateAt;
            updateData.UpdateBy = item.UpdateBy;
            WebIntroDA.Save();
            return true;
        }
        public void Delete(int id)
        {
            WebIntroDA.Delete(id);
            WebIntroDA.Save();
        }
        public bool DeleteByListId(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            WebIntroDA.DeleteRange(ids);
            WebIntroDA.Save();
            return true;
        }
    }
}