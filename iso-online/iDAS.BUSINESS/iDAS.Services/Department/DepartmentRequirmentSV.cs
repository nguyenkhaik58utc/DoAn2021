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
    public class DepartmentRequirmentSV
    {
        private DepartmentRequirmentDA DepartmentRequirmentDA = new DepartmentRequirmentDA();
        public List<CenterISOStandardItem> GetISO()
        {
            var ISODA = new CenterISOStandardDA();
            return ISODA.GetQuery(i => i.IsActive).Select(item => new CenterISOStandardItem { ID = item.ID, Name = item.Name }).ToList();
        }
        public List<DepartmentRequirmentItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = DepartmentRequirmentDA.Repository;
            var DepartmentRequirment = DepartmentRequirmentDA.GetQuery()
                        .Select(item => new DepartmentRequirmentItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ISOID = item.ISOID,
                            Description = item.Description,
                            Object = item.Object,
                            Scope = item.Scope,
                            Time = item.Time,
                            IsUse = item.IsUse,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return DepartmentRequirment;
        }
        public string GetContent(int Id)
        {
            var content = DepartmentRequirmentDA.GetQuery(i => i.ID == Id)
                        .Select(item =>item.Content)
                        .First();
            return content;
        }
        public DepartmentRequirmentItem GetById(int Id)
        {
            var DepartmentRequirment = DepartmentRequirmentDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentRequirmentItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ISOID = item.ISOID,
                            Description = item.Description,
                            Content = item.Content,
                            Object = item.Object,
                            Scope = item.Scope,
                            Time = item.Time,
                            IsUse = item.IsUse,
                        })
                        .First();
            DepartmentRequirment.AttachmentFiles = new AttachmentFileItem()
            {
                Files = DepartmentRequirmentDA.Repository.DepartmentRequirmentAttachments.Where(i => i.DepartmentRequirmentID == Id)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            return DepartmentRequirment;
        }

        public void Update(DepartmentRequirmentItem item, int userID)
        {
            var DepartmentRequirment = DepartmentRequirmentDA.GetById(item.ID);
            DepartmentRequirment.ID = item.ID;
            DepartmentRequirment.Name = item.Name;
            DepartmentRequirment.ISOID = item.ISOID;
            DepartmentRequirment.Description = item.Description;
            DepartmentRequirment.Content = item.Content;
            DepartmentRequirment.Object = item.Object;
            DepartmentRequirment.Scope = item.Scope;
            DepartmentRequirment.Time = item.Time;
            DepartmentRequirment.IsUse = item.IsUse;
            DepartmentRequirment.UpdateAt = DateTime.Now;
            DepartmentRequirment.UpdateBy = userID;
            new FileSV().Upload<DepartmentRequirmentAttachment>(item.AttachmentFiles, DepartmentRequirment.ID);
            DepartmentRequirmentDA.Save();
        }
      
        public int Insert(DepartmentRequirmentItem item, int userID)
        {
            var DepartmentRequirment = new DepartmentRequirment()
            {
                Name = item.Name,
                Description = item.Description,
                ISOID = item.ISOID,
                Content = item.Content,
                Object = item.Object,
                Scope = item.Scope,
                Time = item.Time,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            DepartmentRequirmentDA.Insert(DepartmentRequirment);
            DepartmentRequirmentDA.Save();
            if (item.AttachmentFiles != null)
            {
                new FileSV().Upload<DepartmentRequirmentAttachment>(item.AttachmentFiles, DepartmentRequirment.ID);
            }
            return DepartmentRequirment.ID;
        }
       
        public bool Delete(int id)
        {
            var Requirment = DepartmentRequirmentDA.GetById(id);
            bool isDelete = false;
            if (Requirment.Time > DateTime.Now|| Requirment.Time == null)
                isDelete = true;
            else
                if (!Requirment.IsUse) isDelete = true;
            if(isDelete)
            {
                new FileSV().Delete<DepartmentRequirmentAttachment>(Requirment.ID);
                DepartmentRequirmentDA.Delete(id);
                DepartmentRequirmentDA.Save();
                return true;
            }
            return false;
        }
        public void InsertDataTemplate(int centerRequirmentId, int userID)
        {
            CenterDepartmentRequirmentItem data =
                new CenterDepartmentRequirmentSV().GetById(centerRequirmentId);
            var item = new DepartmentRequirmentItem()
            {
                Name = data.Name,
                Description = data.Description,
                ISOID = data.ISOStandardID,
                Content = data.Content,
                Object = data.Object,
                Scope = data.Scope,
                IsUse = true
            };
            Insert(item, userID);
        }
    }
}
