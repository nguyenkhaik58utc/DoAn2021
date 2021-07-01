using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
 public   class CalendarCategorySV
    {
     private CalendarCategoryDA calendarCategoryDA = new CalendarCategoryDA();
     public List<CalendarCategoryItem> GetTreeCateCalendar(int cateId, int departmentId)
     {
         var dbo = calendarCategoryDA.Repository;
         var result = new List<CalendarCategoryItem>();
         if (cateId == 0)
         {
             result = calendarCategoryDA
                   .GetQuery()
                   .Where(t => t.HumanDepartmentID == departmentId)                 
                   .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                   .Select(item => new CalendarCategoryItem()
                   {
                       ID = item.ID,
                       ParentID = item.ParentID,                     
                       Name = item.Name,                       
                       CreateAt = item.CreateAt,
                       CreateBy = item.CreateBy,
                       UpdateAt = item.UpdateAt,
                       UpdateBy = item.UpdateBy,
                       Leaf = dbo.CalendarCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                   })
                    .OrderByDescending(p => p.CreateAt)
                    .Distinct()
                   .ToList();
             var lis = result.Select(t => t.ID).ToArray();
             List<CalendarCategoryItem> taskremove = new List<CalendarCategoryItem>();
             foreach (var item in result)
             {
                 if (!lis.Contains(item.ParentID))
                 {
                     taskremove.Add(item);
                 }
             }
             result = taskremove;
         }
         else
         {
             result = calendarCategoryDA
                        .GetQuery()
                        .Where(t => t.ParentID == cateId)                   
                        .Distinct()
                        .Select(item => new CalendarCategoryItem()
                        {
                            ID = item.ID,
                            ParentID = item.ParentID,
                            Name = item.Name,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Leaf = dbo.CalendarCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                        })
                          .Distinct()
                        .OrderByDescending(p => p.CreateAt)
                        .ToList();
         }
         return result;
     }
     public CalendarCategory Insert(string parentId, string name, int departmentId, int userId)
     {
         var obj = new CalendarCategory();
         obj.ParentID = int.Parse(parentId);
         obj.Name = name;
         obj.HumanDepartmentID = departmentId;
         obj.CreateAt = DateTime.Now;
         obj.CreateBy = userId;
         calendarCategoryDA.Insert(obj);
         calendarCategoryDA.Save();
         return obj;
     }
     public CalendarCategory Update(string id, string newValue, int departmentId, int userId)
     {
         var obj = calendarCategoryDA.GetById(int.Parse(id));       
         obj.Name = newValue;
         obj.HumanDepartmentID = departmentId;
         obj.UpdateAt = DateTime.Now;
         obj.UpdateBy = userId;
         calendarCategoryDA.Update(obj);
         calendarCategoryDA.Save();
         return obj;
     }
     public void Move(string[] ids, string targetId, int departmentId, int userId)
     {
         foreach (var item in ids)
         {
             var obj = calendarCategoryDA.GetById(int.Parse(item.ToString().Trim('[', '\\', '\"', ']')));
             obj.ParentID = int.Parse(targetId);
             obj.UpdateAt = DateTime.Now;
             obj.UpdateBy = userId;
             obj.HumanDepartmentID = departmentId;
             calendarCategoryDA.Update(obj);
             calendarCategoryDA.Save();
             
         }
       
     }
     public CalendarCategory Update(int parentId, string name, int userId)
     {
         var obj = new CalendarCategory();
         obj.ParentID = parentId;
         obj.Name = name;
         obj.CreateAt = DateTime.Now;
         obj.CreateBy = userId;
         calendarCategoryDA.Insert(obj);
         calendarCategoryDA.Save();
         return obj;
     }
     public void Delete(string id)
     {
         var obj = calendarCategoryDA.GetById(int.Parse(id));
         calendarCategoryDA.Delete(obj);
         calendarCategoryDA.Save();
         
     }
    }
}
