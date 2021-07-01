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
    public class ServiceStageSV : BaseService
    {
        ServiceStageDA serviceStageDA = new ServiceStageDA();

        public List<ServiceStageItem> GetServiceStages(int page, int pageSize, out int total, int serviceId)
        {
            var services = serviceStageDA.GetQuery()
                            .Where(i => i.WebServiceID == serviceId)
                            .Where(i => !i.IsDelete)
                            .OrderBy(i => i.Position)
                            .Select(i => new ServiceStageItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Description = i.Description,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                UpdateAt = i.UpdateAt,
                            })
                            .Page(page, pageSize, out total)
                            .ToList();
            return services;
        }
        public ServiceStageItem GetServiceStage(int id)
        {
            var service = serviceStageDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ServiceStageItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Image = new AttachmentFileItem() { ID = i.FileID },
                                Html = i.Html,
                                Description = i.Description,
                                Position = i.Position,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return service;
        }
        public void Insert(ServiceStageItem item)
        {
            var service = new WebServiceStage()
            {
                WebServiceID = item.WebServiceID,
                Name = item.Name.Trim(),
                Description = item.Description,
                Position = item.Position,
                Html = item.Html,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                service.FileID = fileId;
            }
            serviceStageDA.Insert(service);
            serviceStageDA.Save();
        }
        public void Update(ServiceStageItem item)
        {
            var record = serviceStageDA.GetById(item.ID);
            record.Name = item.Name;
            record.Description = item.Description;
            record.Html = item.Html;
            record.Position = item.Position;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            serviceStageDA.Save();
        }
        public void Delete(int id)
        {
            var item = serviceStageDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            serviceStageDA.Save();
        }
        public bool CheckExist(ServiceStageItem item)
        {
            var check = serviceStageDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.WebServiceID == item.WebServiceID)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
