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
    public class DepartmentRegulatorySV
    {
        private DepartmentRegulatoryDA DepartmentRegulatoryDA = new DepartmentRegulatoryDA();
        public List<CenterISOStandardItem> GetISO()
        {
            var ISODA = new CenterISOStandardDA();
            return ISODA.GetQuery(i => i.IsActive).Select(item => new CenterISOStandardItem { ID = item.ID, Name = item.Name }).ToList();
        }
        public List<DepartmentRegulatoryItem> GetAll(ModelFilter filter)
        {
            var dbo = DepartmentRegulatoryDA.Repository;
            var DepartmentRegulatory = DepartmentRegulatoryDA.GetQuery()
                        .Select(item => new DepartmentRegulatoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            Content = item.Content,
                            IsUse = item.IsUse,
                            IsApplyAll = item.IsApplyAll,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return DepartmentRegulatory;
        }
        public string GetContent(int Id)
        {
            var content = DepartmentRegulatoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => item.Content)
                        .First();
            return content;
        }
        public DepartmentRegulatoryItem GetById(int id)
        {
            var dbo = DepartmentRegulatoryDA.Repository;
            var departmentRegulatory = DepartmentRegulatoryDA.GetQuery(i => i.ID == id)
                        .Select(item => new DepartmentRegulatoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ISOID = item.ISOID,
                            Description = item.Description,
                            Content = item.Content,
                            DepartmentApply = new HumanDepartmentViewItem()
                            {
                                strIDs = item.ListApplyDepartment,
                            },
                            IsApplyAll = item.IsApplyAll,
                            Scope = item.Scope,
                            Time = item.Time,
                            IsUse = item.IsUse,
                        })
                        .First();
            if (departmentRegulatory.DepartmentApply.IDs.Count > 0)
            {
                if (departmentRegulatory.DepartmentApply.IDs.Count == 1)
                {
                    departmentRegulatory.DepartmentApply.Name = DepartmentRegulatoryDA.Repository
                        .HumanDepartments.Where(i => i.ID == departmentRegulatory.DepartmentApply.IDs.FirstOrDefault())
                        .Select(i => i.Name).FirstOrDefault();
                }
                else
                {
                    departmentRegulatory.DepartmentApply.Name = "Có " + departmentRegulatory.DepartmentApply.IDs.Count.ToString() + " phòng ban";
                }
            }
            departmentRegulatory.AttachmentFiles = new AttachmentFileItem()
            {
                Files = DepartmentRegulatoryDA.Repository.DepartmentRegulatoryAttachments.Where(i => i.DepartmentRegulatoryID == id)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            return departmentRegulatory;
        }
        public bool GetIsUse(int id)
        {
            return DepartmentRegulatoryDA.GetQuery(i => i.ID == id).Select(i => i.IsUse).FirstOrDefault();
        }
        public void Update(DepartmentRegulatoryItem item, int userID)
        {
            var departmentRegulatory = DepartmentRegulatoryDA.GetById(item.ID);
            departmentRegulatory.ID = item.ID;
            departmentRegulatory.Name = item.Name;
            departmentRegulatory.ISOID = item.ISOID;
            departmentRegulatory.Description = item.Description;
            departmentRegulatory.Content = item.Content;
            departmentRegulatory.IsApplyAll = item.IsApplyAll;
            if (item.IsApplyAll)
            {
                departmentRegulatory.ListApplyDepartment = string.Empty;
            }
            else
            {
                departmentRegulatory.ListApplyDepartment = item.DepartmentApply.strIDs;
            }
            departmentRegulatory.Scope = item.Scope;
            departmentRegulatory.Time = item.Time;
            departmentRegulatory.IsUse = item.IsUse;
            departmentRegulatory.UpdateAt = DateTime.Now;
            departmentRegulatory.UpdateBy = userID;
            DepartmentRegulatoryDA.Save();
            new FileSV().Upload<DepartmentRegulatoryAttachment>(item.AttachmentFiles, departmentRegulatory.ID);
        }
        public int Insert(DepartmentRegulatoryItem item, int userID)
        {
            var departmentRegulatory = new DepartmentRegulatory()
            {
                Name = item.Name,
                Description = item.Description,
                ISOID = item.ISOID,
                Content = item.Content,
                IsApplyAll = item.IsApplyAll,
                Scope = item.Scope,
                Time = item.Time,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            if (item.IsApplyAll)
            {
                departmentRegulatory.ListApplyDepartment = string.Empty;
            }
            else
            {
                departmentRegulatory.ListApplyDepartment = item.DepartmentApply.strIDs;
            }
            DepartmentRegulatoryDA.Insert(departmentRegulatory);
            DepartmentRegulatoryDA.Save();
            if (item.AttachmentFiles != null)
            {
                new FileSV().Upload<DepartmentRegulatoryAttachment>(item.AttachmentFiles, departmentRegulatory.ID);
            }
            return departmentRegulatory.ID;
        }
        public bool Delete(int id)
        {
            var Regulatory = DepartmentRegulatoryDA.GetById(id);
            bool isDelete = false;
            if (Regulatory.Time > DateTime.Now || Regulatory.Time == null)
                isDelete = true;
            else
                if (!Regulatory.IsUse) isDelete = true;
            if (isDelete)
            {
                new FileSV().Delete<DepartmentRegulatoryAttachment>(Regulatory.ID);
                DepartmentRegulatoryDA.Delete(id);
                DepartmentRegulatoryDA.Save();
                return true;
            }
            return false;
        }
        public void InsertDataTemplate(int centerRequirmentId, int userID)
        {
            CenterDepartmentRequirmentItem data =
                new CenterDepartmentRequirmentSV().GetById(centerRequirmentId);
            var item = new DepartmentRegulatoryItem()
            {
                Name = data.Name,
                Description = data.Description,
                ISOID = data.ISOStandardID,
                Content = data.Content,
                Scope = data.Scope,
                IsUse = true,
                IsApplyAll = true,
            };
            Insert(item, userID);
        }
    }
}
