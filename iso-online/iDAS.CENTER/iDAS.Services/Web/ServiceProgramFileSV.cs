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
    public class ServiceProgramFileSV : BaseService
    {
        ServiceProgramFileDA serviceProgramFileDA = new ServiceProgramFileDA();

        public List<ServiceProgramFileItem> GetServiceProgramFiles(int page, int pageSize, out int total, int serviceProgramId)
        {
            var lst = serviceProgramFileDA.GetQuery()
                            .Where(i => i.WebServiceItemID == serviceProgramId)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ServiceProgramFileItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Extension = i.File.Extension.Replace(".",""),
                                Size = i.File.Size,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                UpdateAt = i.UpdateAt,
                            })
                            .OrderByDescending(i => i.CreateAt)
                            .Page(page, pageSize, out total)
                            .ToList();
            return lst;
        }
        public ServiceProgramFileItem GetServiceProgramFile(int id)
        {
            var item = serviceProgramFileDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ServiceProgramFileItem()
                            {
                                File = new AttachmentFileItem() { ID = i.FileID, Name = i.File.Name },
                                ID = i.ID,
                                Name = i.Name,
                                Extension = i.File.Extension.Replace(".",""),
                                Size = i.File.Size,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return item;
        }
        public void Insert(ServiceProgramFileItem item)
        {
            var service = new WebServiceItemFile()
            {
                WebServiceItemID = item.ServiceProgramID,
                Name = item.Name,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            var fileId = new FileSV().Upload(item.File).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                service.FileID = fileId;
            }
            serviceProgramFileDA.Insert(service);
            serviceProgramFileDA.Save();
        }
        public void Update(ServiceProgramFileItem item)
        {
            var service = serviceProgramFileDA.GetById(item.ID);
            service.Name = item.Name;
            service.IsActive = item.IsActive;
            service.UpdateAt = DateTime.Now;
            service.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.File).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                service.FileID = fileId;
            }
            serviceProgramFileDA.Save();
        }
        public void Delete(int id)
        {
            var item = serviceProgramFileDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            serviceProgramFileDA.Save();
        }
    }
}
