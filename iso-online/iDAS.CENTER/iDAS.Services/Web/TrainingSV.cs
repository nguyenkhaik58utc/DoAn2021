using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class TrainingSV
    {
        TrainingDA trainingDA = new TrainingDA();
        public List<TrainingItem> GetAll(int page, int pageSize, out int totalCount)
        {
            try
            {
                var lst = trainingDA.GetQuery()
                                 .OrderBy(a => a.Name)
                                 .Select(a => new TrainingItem()
                                 {
                                     ID = a.ID,
                                     Name = a.Name,
                                     Contents = a.Contents,
                                     Description = a.Description,
                                     FileID = a.FileID,
                                     IsShow = a.IsShow,
                                     Order = a.Order,
                                     Time = a.Time,
                                     CreateAt = a.CreateAt,
                                     UpdateAt = a.UpdateAt
                                 })
                                 .OrderByDescending(item => item.CreateAt)
                                 .Page(page, pageSize, out totalCount)
                                 .ToList();
                return lst;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TrainingItem GetByID(int id)
        {
            try
            {
                var a = trainingDA.GetQuery(i => i.ID == id)
                 .First();
                if (a != null)
                {
                    var result = new TrainingItem()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Contents = a.Contents,
                        Description = a.Description,
                        FileID = a.FileID,
                        IsShow = a.IsShow,
                        Order = a.Order,

                        Time = a.Time,
                        CreateAt = a.CreateAt,
                        UpdateAt = a.UpdateAt,
                        UpdateBy = a.UpdateBy
                    };
                    if (a.FileID != null)
                    {
                        result.File = new AttachmentFileItem()
                        {
                            Files = new List<Guid>() { (Guid)a.FileID },
                        };
                    }
                    return result;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int Insert(TrainingItem item)
        {
            try
            {
                var iso = new WebTraining()
                {
                    Name = item.Name,
                    Contents = item.Contents,
                    Description = item.Description,
                    IsShow = item.IsShow,
                    Time = item.Time,
                    Order = item.Order,
                    CreateAt = DateTime.Now,
                    CreateBy = item.CreateBy
                };
                if (item.File != null && item.File.FileAttachments != null)
                {
                    var fileIds = new FileSV().InsertRange(item.File.FileAttachments, item.CreateBy.Value);
                    foreach (var fileID in fileIds)
                    {
                        iso.FileID = fileID;
                    }
                }   
                trainingDA.Insert(iso);
                trainingDA.Save();
                return iso.ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(TrainingItem item)
        {
            try
            {
                var updateData = trainingDA.GetById(item.ID);
                if (item.File != null && item.File.FileRemoves.Count > 0)
                {
                    new FileSV().DeleteRange(item.File.FileRemoves);
                    updateData.FileID = null;
                }
                if (item.File != null && item.File.FileAttachments.Count > 0)
                {
                    if (updateData.FileID == null)
                    {
                        if (item.File.FileAttachments != null)
                        {
                            var fileID = new FileSV().InsertRange(item.File.FileAttachments, item.UpdateBy.Value).FirstOrDefault();
                            if (fileID != Guid.Empty)
                                updateData.FileID = fileID;
                        }
                    }
                    else
                    {
                        var file = item.File.FileAttachments.First();
                        if (file != null)
                        {
                            var fileItem = new FileItem()
                            {
                                ID = (Guid)updateData.FileID,
                                Extension = string.IsNullOrWhiteSpace(file.FileName) ?
                                  "" : file.FileName.Substring(file.FileName.IndexOf('.') + 1),
                                Size = file.ContentLength,
                                Type = file.ContentType,
                                Data = iDAS.Utilities.CustomConvert.ToByteArray(file.InputStream),
                            };
                            new FileSV().Update(fileItem, item.UpdateBy.Value);
                        }
                    }
                }
                updateData.Name = item.Name;
                updateData.Contents = item.Contents;
                updateData.Description = item.Description;
                updateData.IsShow = item.IsShow;
                updateData.Time = item.Time;
                updateData.Order = item.Order;
                updateData.FileID = updateData.FileID;
                updateData.UpdateBy = (int)item.UpdateBy;
                trainingDA.Update(updateData);
                trainingDA.Save();
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
                var iso = trainingDA.GetById(id);
                trainingDA.Delete(id);
                trainingDA.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
