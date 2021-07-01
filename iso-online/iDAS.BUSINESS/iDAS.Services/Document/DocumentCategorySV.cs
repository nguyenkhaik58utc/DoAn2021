using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class DocumentCategorySV
    {
        DocumentCategoryDA documentCategoryDA = new DocumentCategoryDA();
        public List<DocumentCategoryItem> GetAll(int page, int pageSize, out int totalCount, int groupId)
        {
            var dbo = documentCategoryDA.Repository;
            var lst = documentCategoryDA.GetQuery(p => p.DepartmentID == groupId)
                    .Select(i => new DocumentCategoryItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Note = i.Note,
                        CreateAt = i.CreateAt,
                        CreateByName = dbo.HumanUsers.Where(u => u.ID == i.CreateBy).Select(u => u.HumanEmployee.Name).FirstOrDefault(),
                        UpdateByName = dbo.HumanUsers.Where(u => u.ID == i.UpdateBy).Select(u => u.HumanEmployee.Name).FirstOrDefault()
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return lst;
        }
        public List<DocumentCategoryItem> GetAll()
        {
            var result = documentCategoryDA.GetQuery()
                     .Select(i => new DocumentCategoryItem()
                     {
                         ID = i.ID,
                         Name = i.Name,
                     })
                     .OrderByDescending(item => item.Name)
                     .ToList();
            return result;
        }
        public DocumentCategoryItem GetByID(int id)
        {
            var dbo = documentCategoryDA.Repository;
            var item = documentCategoryDA.GetById(id);
            var obj = new DocumentCategoryItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Department = new HumanDepartmentViewItem()
                        {
                            ID = item.DepartmentID,
                            Name = dbo.HumanDepartments.Where(d => d.ID == item.DepartmentID).Select(d => d.Name).FirstOrDefault()
                        },
                        Note = item.Note,
                        CreateAt = item.CreateAt,
                        UpdateAt = item.UpdateAt,
                        CreateByName = dbo.HumanUsers.Where(u => u.ID == item.CreateBy).Select(u => u.HumanEmployee.Name).FirstOrDefault(),
                        UpdateByName = dbo.HumanUsers.Where(u => u.ID == item.UpdateBy).Select(u => u.HumanEmployee.Name).FirstOrDefault(),
                    };
            return obj;
        }
        public DocumentCategory CheckExit(int id,string name, int departmentId)
        {
            var item = documentCategoryDA.GetQuery(t=>t.ID!=id && t.DepartmentID==departmentId && t.Name.ToUpper().Contains(name.ToUpper())).FirstOrDefault();           
            return item;
        }
        public void Insert(DocumentCategoryItem obj)
        {
            var itm = new DocumentCategory
               {
                   Name = obj.Name,
                   DepartmentID = obj.Department.ID,
                   Note = obj.Note,
                   CreateAt = DateTime.Now,
                   CreateBy = obj.CreateBy

               };
            documentCategoryDA.Insert(itm);
            documentCategoryDA.Save();
        }
        public void Update(DocumentCategoryItem obj)
        {
            var itm = documentCategoryDA.GetById(obj.ID);
            itm.Name = obj.Name;
            itm.Note = obj.Note;
            itm.DepartmentID = obj.Department.ID;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            documentCategoryDA.Update(itm);
            documentCategoryDA.Save();
        }
        public bool Delete(int id)
        {
            var item = documentCategoryDA.GetById(id);
            if (documentCategoryDA.Repository.Documents.Where(i => i.DocumentCategoryID == id).FirstOrDefault() != null)
            {
                return false;
            }
            documentCategoryDA.Delete(item);
            documentCategoryDA.Save();
            return true;
        }
        public HumanDepartment GetByNameDepartment(int depId)
        {
            var dbo = documentCategoryDA.Repository;
            var obj = dbo.HumanDepartments.Where(t=>t.ID==depId)
                 .FirstOrDefault();
            return obj;
        }
    }
}
