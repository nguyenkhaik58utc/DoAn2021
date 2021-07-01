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
    public class QualityNCCategorySV
    {
        //private NCCategoryDA NCCategoryDA = new NCCategoryDA();
        //public List<NCCategoryItem> GetAll(int page, int pageSize, out int totalCount)
        //{
        //    var dbo = NCCategoryDA.Repository;
        //    var source = dbo.QualityNCCategory
        //        .Select(item => new NCCategoryItem()
        //        {
        //            ID = item.ID,
        //            Name = item.Name,
        //            CheckBy = item.CheckBy == null ? null : new EmployeeViewItem()
        //            {
        //                Name = dbo.HumanEmployee.FirstOrDefault(m => m.ID == item.CheckBy).Name,
        //            },
        //            CreateBy = item.CreateBy,
        //        })
        //        .OrderByDescending(i => i.CreateBy)
        //        .Page(page, pageSize, out totalCount)
        //        .ToList();

        //    return source;
        //}
        //public NCCategoryItem GetByID(int ID)
        //{
        //    var dbo = NCCategoryDA.Repository;
        //    var result = dbo.QualityNCCategory.Where(i => i.ID == ID)
        //        .Select(item => new NCCategoryItem()
        //        {
        //            ID = item.ID,
        //            Name = item.Name,
        //            CheckBy = item.CheckBy == null ? null : new EmployeeViewItem()
        //            {
        //                ID = (int)item.CheckBy,
        //                Name = dbo.HumanEmployee.FirstOrDefault(m => m.ID == item.CheckBy).Name,
        //                Avatar = dbo.HumanEmployee.FirstOrDefault(m => m.ID == item.CheckBy).Avatar,
        //                Role = dbo.HumanOrganization.FirstOrDefault(m => m.EmployeeID == item.CheckBy).HumanRole.Name,
        //                Department = dbo.HumanOrganization.FirstOrDefault(m => m.EmployeeID == item.CheckBy).HumanRole.HumanDepartment.Name,
        //            },
        //            CreateBy = item.CreateBy,
        //        }).First();
        //    return result;
        //}
        //public void Insert(QualityNCCategoryItem     item)
        //{
        //    var incorrect = new QualityNCCategory()
        //    {
        //        Name = item.Name,
        //        CheckBy = item.CheckBy.ID,
        //        CreateBy = item.CreateBy,
        //        CreateAt = DateTime.Now,
        //    };
        //    NCCategoryDA.Insert(incorrect);
        //    NCCategoryDA.Save();
        //}
        //public void Update(NCCategoryItem item)
        //{
        //    var nc = NCCategoryDA.GetById(item.ID);
        //    nc.Name = item.Name;
        //    nc.CheckBy = item.CheckBy.ID;
        //    nc.UpdateAt = DateTime.Now;
        //    nc.UpdateBy = nc.UpdateBy;
        //    NCCategoryDA.Save();
        //}
        //public void Delete(int id)
        //{
        //    NCCategoryDA.Delete(id);
        //    NCCategoryDA.Save();
        //}
    }
}
