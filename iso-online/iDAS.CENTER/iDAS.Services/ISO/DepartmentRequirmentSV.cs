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
        public List<DepartmentRequirmentItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var DepartmentRequirment = DepartmentRequirmentDA.GetQuery()
                        .Select(item => new DepartmentRequirmentItem()
                        {
                            ID = item.ID,
                            CenterDepartmentRequirmentCategoryID = item.CenterDepartmentRequirmentCategoryID,
                            RequirmentCategoryName = item.CenterDepartmentRequirmentCategory.Name,
                            Name = item.Name,
                            ISOStandardID = item.ISOStandardID,
                            Description = item.Description,
                            Object = item.Object,
                            Scope = item.Scope,
                            IsUse = item.IsUse,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return DepartmentRequirment;
        }
        public List<DepartmentRequirmentItem> GetRequirmentIn(int page, int pageSize, out int totalCount)
        {
            var DepartmentRequirment = DepartmentRequirmentDA.GetQuery(i => i.IsInner)
                        .Select(item => new DepartmentRequirmentItem()
                        {
                            ID = item.ID,
                            CenterDepartmentRequirmentCategoryID = item.CenterDepartmentRequirmentCategoryID,
                            RequirmentCategoryName = item.CenterDepartmentRequirmentCategory.Name,
                            Name = item.Name,
                            ISOStandardID = item.ISOStandardID,
                            ISOName = item.ISOStandard.Name,
                            Description = item.Description,
                            Object = item.Object,
                            Scope = item.Scope,
                            IsUse = item.IsUse,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return DepartmentRequirment;
        }
        public List<DepartmentRequirmentItem> GetRequirmentOut(int page, int pageSize, out int totalCount)
        {
            var DepartmentRequirment = DepartmentRequirmentDA.GetQuery(i => !i.IsInner)
                        .Select(item => new DepartmentRequirmentItem()
                        {
                            ID = item.ID,
                            CenterDepartmentRequirmentCategoryID = item.CenterDepartmentRequirmentCategoryID,
                            RequirmentCategoryName = item.CenterDepartmentRequirmentCategory.Name,
                            Name = item.Name,
                            ISOStandardID = item.ISOStandardID,
                            ISOName = item.ISOStandard.Name,
                            Description = item.Description,
                            Object = item.Object,
                            Scope = item.Scope,
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
                        .Select(item => item.Content)
                        .First();
            return content;
        }
        public DepartmentRequirmentItem GetById(int Id)
        {
            var DepartmentRequirment = DepartmentRequirmentDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentRequirmentItem()
                        {
                            ID = item.ID,
                            CenterDepartmentRequirmentCategoryID = item.CenterDepartmentRequirmentCategoryID,
                            Name = item.Name,
                            ISOStandardID = item.ISOStandardID,
                            Description = item.Description,
                            Content = item.Content,
                            Object = item.Object,
                            Scope = item.Scope,
                            IsUse = item.IsUse,
                        })
                        .First();
            DepartmentRequirment.AttachmentFiles = new AttachmentFileItem()
            {
                Files = DepartmentRequirmentDA.Repository.CenterDepartmentRequirmentAttachments.Where(i => i.CenterDepartmentRequirmentID == Id)
                    .Select(i => i.FileID).ToList()
            };
            return DepartmentRequirment;
        }

        public void Update(DepartmentRequirmentItem item, int userID)
        {
            var DepartmentRequirment = DepartmentRequirmentDA.GetById(item.ID);
            DepartmentRequirment.CenterDepartmentRequirmentCategoryID =
                item.CenterDepartmentRequirmentCategoryID;
            DepartmentRequirment.Name = item.Name;
            DepartmentRequirment.ISOStandardID = item.ISOStandardID;
            DepartmentRequirment.Description = item.Description;
            DepartmentRequirment.Content = item.Content;
            DepartmentRequirment.Object = item.Object;
            DepartmentRequirment.Scope = item.Scope;
            DepartmentRequirment.IsUse = item.IsUse;
            DepartmentRequirment.UpdateAt = DateTime.Now;
            DepartmentRequirment.UpdateBy = userID;
            new FileSV().Upload<CenterDepartmentRequirmentAttachment>(item.AttachmentFiles, DepartmentRequirment.ID);
            DepartmentRequirmentDA.Save();
        }

        public int Insert(DepartmentRequirmentItem item, int userID)
        {
            var DepartmentRequirment = new CenterDepartmentRequirment()
            {
                CenterDepartmentRequirmentCategoryID = item.CenterDepartmentRequirmentCategoryID,
                Name = item.Name,
                IsInner = item.IsInner,
                Description = item.Description,
                ISOStandardID = item.ISOStandardID,
                Content = item.Content,
                Object = item.Object,
                Scope = item.Scope,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            DepartmentRequirmentDA.Insert(DepartmentRequirment);
            DepartmentRequirmentDA.Save();
            new FileSV().Upload<CenterDepartmentRequirmentAttachment>(item.AttachmentFiles, DepartmentRequirment.ID);
            return DepartmentRequirment.ID;
        }

        public bool Delete(int id)
        {
            var Requirment = DepartmentRequirmentDA.GetById(id);
            new FileSV().Delete<CenterDepartmentRequirmentAttachment>(Requirment.ID);
            DepartmentRequirmentDA.Delete(id);
            DepartmentRequirmentDA.Save();
            return true;
        }
    }
}
