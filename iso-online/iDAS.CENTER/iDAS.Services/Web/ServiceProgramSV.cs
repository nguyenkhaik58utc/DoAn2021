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
    public class ServiceProgramSV : BaseService
    {
        ServiceProgramDA serviceProgramDA = new ServiceProgramDA();

        public List<ServiceProgramItem> GetServicePrograms(int page, int pageSize, out int total, int serviceId)
        {
            var servicePrograms = serviceProgramDA.GetQuery()
                            .Where(i => i.WebServiceID == serviceId)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ServiceProgramItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Price = i.Price,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                UpdateAt = i.UpdateAt,
                            })
                            .OrderByDescending(i => i.CreateAt)
                            .Page(page, pageSize, out total)
                            .ToList();
            return servicePrograms;
        }
        public ServiceProgramItem GetServiceProgram(int id)
        {
            var serviceProgram = serviceProgramDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ServiceProgramItem()
                            {
                                Image = new AttachmentFileItem() { ID = i.FileID },
                                ID = i.ID,
                                Name = i.Name,
                                Tags = i.Tags,
                                Html = i.Html,
                                Address = i.Address,
                                Duration = i.Duration,
                                StartAt = i.StartAt,
                                EndAt = i.EndAt,
                                RefreshAt = i.RefreshAt,
                                Note = i.Note,
                                Price = i.Price,
                                IsFirst = i.IsFirst,
                                IsNew = i.IsNew,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return serviceProgram;
        }
        public void Insert(ServiceProgramItem item)
        {
            var serviceProgram = new WebServiceItem()
            {
                WebServiceID = item.ServiceID,
                Name = item.Name.Trim(),
                Tags = item.Tags,
                Address = item.Address,
                StartAt = item.StartAt,
                EndAt = item.EndAt,
                RefreshAt = item.RefreshAt,
                Price = item.Price,
                Note = item.Note,
                Html = item.Html,
                Duration = item.Duration,
                IsFirst = item.IsFirst,
                IsNew = item.IsNew,
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
                serviceProgram.FileID = fileId;
            }
            serviceProgramDA.Insert(serviceProgram);
            serviceProgramDA.Save();
        }
        public void Update(ServiceProgramItem item)
        {
            var serviceProgram = serviceProgramDA.GetById(item.ID);
            serviceProgram.Name = item.Name;
            serviceProgram.Tags = item.Tags;
            serviceProgram.Duration = item.Duration;
            serviceProgram.Address = item.Address;
            serviceProgram.StartAt = item.StartAt;
            serviceProgram.EndAt = item.EndAt;
            serviceProgram.Html = item.Html;
            serviceProgram.Note = item.Note;
            serviceProgram.Price = item.Price;
            serviceProgram.RefreshAt = item.RefreshAt;
            serviceProgram.IsFirst = item.IsFirst;
            serviceProgram.IsNew = item.IsNew;
            serviceProgram.IsActive = item.IsActive;
            serviceProgram.UpdateAt = DateTime.Now;
            serviceProgram.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                serviceProgram.FileID = fileId;
            }
            serviceProgramDA.Save();
        }
        public void Delete(int id)
        {
            var item = serviceProgramDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            serviceProgramDA.Save();
        }
        public bool CheckExist(ServiceProgramItem item)
        {
            var check = serviceProgramDA.GetQuery()
                        .Where(i => i.ID != item.ID||item.ID == 0)
                        .Where(i => i.WebServiceID == item.ServiceID)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
