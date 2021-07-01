using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class StockProductGroupSV
    {
        private StockProductGroupDA product_GroupDA = new StockProductGroupDA();
        public List<ComboboxItem> Combobox(string query)
        {
            var ls = product_GroupDA.GetQuery(t => (t.Name.Contains(query) || string.IsNullOrEmpty(query))&& t.Active).Select(item => new ComboboxItem()
            {
                ID = item.ID,
                Name = item.Name,
            }).ToList();
            return ls;
        }
        public bool CheckNameExits(string name)
        {
            var rs = (product_GroupDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper())).FirstOrDefault();
            if (rs != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetNameGroupById(int id)
        {

            var rs = product_GroupDA.GetById(id);
            if (rs != null)
                return rs.Name;
            return string.Empty;
        }
        public StockProductGroupItem GetById(int Id)
        {
            var result = product_GroupDA.GetQuery(i => i.ID == Id)
                        .Select(item => new StockProductGroupItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            IsUse = (bool)item.Active,
                            CreateAt = DateTime.Now,
                            CreateBy = item.CreateBy
                        }).First();
            return result;
        }
        public List<StockProductGroupItem> GetAll(int page, int pageSize, out int total)
        {
            var lstProductGroup = product_GroupDA.GetQuery()
                .Select(item => new StockProductGroupItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Description = item.Description,
                        IsUse = (bool)item.Active
                    })
                .OrderBy(t => t.Name)
                .Page(page, pageSize, out total).ToList();
            return lstProductGroup;

        }
        public List<StockProductGroupItem> GetListAll()
        {
            var lstProductGroup = product_GroupDA.GetQuery()
                .Select(item => new StockProductGroupItem
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    IsUse = (bool)item.Active
                })
                .OrderBy(t => t.Name)
                .ToList();
            return lstProductGroup;
        }
        public void Insert(StockProductGroupItem item)
        {
            StockProductGroup lsStock = new StockProductGroup()
            {
                ID = item.ID,
                Name = item.Name,
                Description = item.Description,
                Active = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            product_GroupDA.Insert(lsStock);
            product_GroupDA.Save();
        }
        public void Update(StockProductGroupItem item)
        {
            StockProductGroup productgroup = product_GroupDA.GetById(item.ID);
            productgroup.ID = item.ID;
            productgroup.Name = item.Name;
            productgroup.Description = item.Description;
            productgroup.Active = item.IsUse;
            productgroup.UpdateAt = DateTime.Now;
            productgroup.UpdateBy = item.UpdateBy;
            product_GroupDA.Update(productgroup);
            product_GroupDA.Save();
        }
        public void DeleteRange(string strIds)
        {
            var ids = strIds.Split(',').Select(n => (object)int.Parse(n)).ToList();
            product_GroupDA.DeleteRange(ids);
            product_GroupDA.Save();
        }
    }
}
