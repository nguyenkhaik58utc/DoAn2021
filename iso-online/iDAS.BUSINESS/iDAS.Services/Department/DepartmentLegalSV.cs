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
    public class DepartmentLegalSV
    {
        private DepartmentLegalDA DepartmentLegalDA = new DepartmentLegalDA();
        public List<CenterISOStandardItem> GetISO()
        {
            var ISODA = new CenterISOStandardDA();
            return ISODA.GetQuery(i => i.IsActive).Select(item => new CenterISOStandardItem { ID = item.ID, Name = item.Name }).ToList();
        }
        public List<DepartmentLegalItem> GetAll(ModelFilter filter)
        {
            var dbo = DepartmentLegalDA.Repository;
            var DepartmentLegal = DepartmentLegalDA.GetQuery()
                        .Select(item => new DepartmentLegalItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            Content = item.Content,
                            IsApplyAll = item.IsApplyAll,
                            IsUse = item.IsUse,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return DepartmentLegal;
        }
        public string GetContent(int Id)
        {
            var content = DepartmentLegalDA.GetQuery(i => i.ID == Id)
                        .Select(item => item.Content)
                        .First();
            return content;
        }
        public DepartmentLegalItem GetById(int Id)
        {
            var departmentLegal = DepartmentLegalDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentLegalItem()
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
                            IsUse = item.IsUse,
                        })
                        .First();
            if (departmentLegal.DepartmentApply.IDs.Count > 0)
            {
                if (departmentLegal.DepartmentApply.IDs.Count == 1)
                {
                    departmentLegal.DepartmentApply.Name = DepartmentLegalDA.Repository
                        .HumanDepartments.Where(i => i.ID == departmentLegal.DepartmentApply.IDs.FirstOrDefault())
                        .Select(i => i.Name).FirstOrDefault();
                }
                else
                {
                    departmentLegal.DepartmentApply.Name = "Có " + departmentLegal.DepartmentApply.IDs.Count.ToString() + " phòng ban";
                }
            }
            departmentLegal.AttachmentFiles = new AttachmentFileItem()
            {
                Files = DepartmentLegalDA.Repository.DepartmentLegalAttachments.Where(i => i.DepartmentLegalID == Id)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            return departmentLegal;
        }
        public bool GetIsUse(int id)
        {
            return DepartmentLegalDA.GetQuery(i => i.ID == id).Select(i => i.IsUse).FirstOrDefault();
        }
        public void Update(DepartmentLegalItem item, int userID)
        {
            var departmentLegal = DepartmentLegalDA.GetById(item.ID);
            departmentLegal.ID = item.ID;
            departmentLegal.ISOID = item.ISOID;
            departmentLegal.Name = item.Name;
            departmentLegal.Description = item.Description;
            departmentLegal.Content = item.Content;
            departmentLegal.IsApplyAll = item.IsApplyAll;
            if (item.IsApplyAll)
            {
                departmentLegal.ListApplyDepartment = string.Empty;
            }
            else
            {
                departmentLegal.ListApplyDepartment = item.DepartmentApply.strIDs;
            }
            departmentLegal.IsUse = item.IsUse;
            departmentLegal.UpdateAt = DateTime.Now;
            departmentLegal.UpdateBy = userID;
            DepartmentLegalDA.Save();
            new FileSV().Upload<DepartmentLegalAttachment>(item.AttachmentFiles, departmentLegal.ID);
        }

        public int Insert(DepartmentLegalItem item, int userID)
        {
            var DepartmentLegal = new DepartmentLegal()
            {
                Name = item.Name,
                ISOID = item.ISOID,
                Description = item.Description,
                Content = item.Content,
                IsApplyAll = item.IsApplyAll,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            if (item.IsApplyAll)
            {
                DepartmentLegal.ListApplyDepartment = string.Empty;
            }
            else
            {
                DepartmentLegal.ListApplyDepartment = item.DepartmentApply.strIDs;
            }
            DepartmentLegalDA.Insert(DepartmentLegal);
            DepartmentLegalDA.Save();
            if (item.AttachmentFiles != null)
            {
                new FileSV().Upload<DepartmentLegalAttachment>(item.AttachmentFiles, DepartmentLegal.ID);
            }
            return DepartmentLegal.ID;
        }

        public bool Delete(int id)
        {
            var Legal = DepartmentLegalDA.GetById(id);
            if (!Legal.IsUse)
            {
                new FileSV().Delete<DepartmentLegalAttachment>(Legal.ID);
                DepartmentLegalDA.Delete(id);
                DepartmentLegalDA.Save();
                return true;
            }
            return false;
        }
        public void InsertDataTemplate(int centerRequirmentId, int userID)
        {
            CenterDepartmentRequirmentItem data =
                new CenterDepartmentRequirmentSV().GetById(centerRequirmentId);
            var item = new DepartmentLegalItem()
            {
                Name = data.Name,
                Description = data.Description,
                ISOID = data.ISOStandardID,
                Content = data.Content,
                IsUse = true,
                IsApplyAll = true
            };
            Insert(item, userID);
        }

    }
}
