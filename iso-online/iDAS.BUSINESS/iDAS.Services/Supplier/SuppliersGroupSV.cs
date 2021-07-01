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
    public class SuppliersGroupSV
    {
        private SuppliersGroupDA SuppGroupDA = new SuppliersGroupDA();

        public List<SuppliersGroupItem> GetByIds(int[] intIds)
        {
            throw new NotImplementedException();
        }

        public List<SuppliersGroupItem> GetTreeData(int nodeId)
        {
            var dbo = SuppGroupDA.Repository;
            
                var groupCustomer = SuppGroupDA.GetQuery(i => (i.ParentID != null && i.ParentID == nodeId))
                     .Select(item => new SuppliersGroupItem()
                     {
                         ID = item.ID,
                         ParentID = item.ParentID,
                         Name = item.Name,
                         Note = item.Note,
                         Leaf = dbo.SuppliersGroups.Where(i=>i.ParentID == item.ID).ToList().Count==0
                         
                     })
                     .ToList();
                return groupCustomer;
            
        }
        public void checkRoot()
        {
            if (!SuppGroupDA.GetQuery().Any())
            {
                SuppliersGroup obj = new SuppliersGroup();
                obj.Name = "Nhóm nhà cung cấp";
                obj.ParentID = 0;
                obj.Active = true;
                obj.CreateAt = DateTime.Now;
                SuppGroupDA.Insert(obj);
                SuppGroupDA.Save();
            }
        }
        public void Delete(int ids)
        {
            SuppGroupDA.Delete(ids);
            SuppGroupDA.Save();
        }

        public SuppliersGroupItem GetById(int id)
        {
            SuppliersGroupItem obj = new SuppliersGroupItem();
            var data = SuppGroupDA.Get(t => t.ID == id).FirstOrDefault();
            if (data != null)
            {
                obj.ID = data.ID;
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.ParentID = data.ParentID;
            }
            return obj;
        }

        public void Insert(SuppliersGroupItem data, int p)
        {
            SuppliersGroup obj = new SuppliersGroup();
            if (data != null)
            {                
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.ParentID = data.ParentID;
                obj.Active = true;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = p;
            }
            SuppGroupDA.Insert(obj);
            SuppGroupDA.Save();
        }
        public bool checkNameExist(int parentID,string name)
        {
            var obj = SuppGroupDA.GetQuery(i => i.ParentID == parentID && i.Name.ToString().ToUpper() == name.ToString().ToUpper()).FirstOrDefault();
            if (obj != null) 
                return true;
            else
                return false;            
        }
        public bool checkNameExist(int parentID, string name,int id)
        {
            var obj = SuppGroupDA.GetQuery(i => i.ParentID == parentID && i.Name.ToString().ToUpper() == name.ToString().ToUpper()).FirstOrDefault();
            var obj1 = SuppGroupDA.GetById(id);
            if (obj != null && obj1.Name!=name)
                return true;
            else
                return false;
        }
        public void Update(SuppliersGroupItem data, int p)
        {
            SuppliersGroup obj = SuppGroupDA.GetById(data.ID);
            if (data != null)
            {                
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.ParentID = data.ParentID;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = p;
            }
            SuppGroupDA.Update(obj);
            SuppGroupDA.Save();
        }
        public IEnumerable<SuppliersGroup> getChilds(IEnumerable<SuppliersGroup> e, int? id)
        {
            var SuppliersGroups = e.Where(a => a.ParentID == id);
            var SuppliersGroupFirst = e.Where(a => a.ID == id);
            SuppliersGroupFirst.Concat(SuppliersGroups);
            return SuppliersGroupFirst.Concat(SuppliersGroups.SelectMany(a => getChilds(e, a.ID)));
        }
        public List<ComboboxItem> getCombobox()
        {
            List<ComboboxItem> lst = SuppGroupDA.GetQuery().Select(item => new ComboboxItem
            {
                ID = item.ID,                
                Name = item.Name
            }).ToList();
            return lst;
        }
    }
}
