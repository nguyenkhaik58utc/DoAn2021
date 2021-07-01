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
    public class DepartmentPolicySV
    {
        private DepartmentPolicyDA DepartmentPolicyDA = new DepartmentPolicyDA();
        public List<CenterISOStandardItem> GetISO()
        {
            var ISODA = new CenterISOStandardDA();
            return ISODA.GetQuery(i => i.IsActive).Select(item => new CenterISOStandardItem { ID = item.ID, Name = item.Name }).ToList();
        }
        public List<DepartmentPolicyItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = DepartmentPolicyDA.Repository;
            var DepartmentPolicy = DepartmentPolicyDA.GetQuery()
                        .Select(item => new DepartmentPolicyItem()
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
            return DepartmentPolicy;
        }
        public string GetContent(int Id)
        {
            var content = DepartmentPolicyDA.GetQuery(i => i.ID == Id)
                        .Select(item => item.Content)
                        .First();
            return content;
        }
        public DepartmentPolicyItem GetById(int Id)
        {
            var DepartmentPolicy = DepartmentPolicyDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentPolicyItem()
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
            DepartmentPolicy.AttachmentFiles = new AttachmentFileItem()
            {
                Files = DepartmentPolicyDA.Repository.DepartmentPolicyAttachments.Where(i => i.DepartmentPolicyID == Id)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            return DepartmentPolicy;
        }

        public void Update(DepartmentPolicyItem item, int userID)
        {
            var DepartmentPolicy = DepartmentPolicyDA.GetById(item.ID);
            DepartmentPolicy.ID = item.ID;
            DepartmentPolicy.Name = item.Name;
            DepartmentPolicy.ISOID = item.ISOID;
            DepartmentPolicy.Description = item.Description;
            DepartmentPolicy.Content = item.Content;
            DepartmentPolicy.Object = item.Object;
            DepartmentPolicy.Scope = item.Scope;
            DepartmentPolicy.Time = item.Time;
            DepartmentPolicy.IsUse = item.IsUse;
            DepartmentPolicy.UpdateAt = DateTime.Now;
            DepartmentPolicy.UpdateBy = userID;
            new FileSV().Upload<DepartmentPolicyAttachment>(item.AttachmentFiles, DepartmentPolicy.ID);
            DepartmentPolicyDA.Save();
        }

        public int Insert(DepartmentPolicyItem item, int userID)
        {
            var DepartmentPolicy = new DepartmentPolicy()
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
            DepartmentPolicyDA.Insert(DepartmentPolicy);
            DepartmentPolicyDA.Save();
            if (item.AttachmentFiles!=null)
            {
                new FileSV().Upload<DepartmentPolicyAttachment>(item.AttachmentFiles, DepartmentPolicy.ID);
            }
            return DepartmentPolicy.ID;
        }

        public bool Delete(int id)
        {
            var policy = DepartmentPolicyDA.GetById(id);
            bool isDelete = false;
            if (policy.Time > DateTime.Now || policy.Time == null)
                isDelete = true;
            else
                if (!policy.IsUse) isDelete = true;
            if (isDelete)
            {
                new FileSV().Delete<DepartmentPolicyAttachment>(policy.ID);
                DepartmentPolicyDA.Delete(id);
                DepartmentPolicyDA.Save();
                return true;
            }
            return false;
        }
        public void InsertDataTemplate(int centerRequirmentId, int userID)
        {
            CenterDepartmentRequirmentItem data =
                new CenterDepartmentRequirmentSV().GetById(centerRequirmentId);
            var item = new DepartmentPolicyItem()
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
