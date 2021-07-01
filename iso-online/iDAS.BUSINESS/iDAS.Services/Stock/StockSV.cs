using System;
using System.Collections.Generic;
using iDAS.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;


namespace iDAS.Services
{
    public class StockSV
    {
        private StockDA listStockDA = new StockDA();
        public StockItem GetById(int Id)
        {
            var result = listStockDA.GetQuery(i => i.ID == Id)
                        .Select(item => new StockItem()
                    {
                        ID = item.ID,
                        ImageMap = item.ImageMap,
                        Name = item.Name,
                        Address = item.Address,
                        Active = item.Active,
                        Code = item.Code,
                        Contact = item.Contact,
                        Description = item.Description,
                        Email = item.Email,
                        Fax = item.Fax,
                        Manager = item.Manager,
                        Mobi = item.Mobi,
                        Telephone = item.Telephone,
                        CreateAt = DateTime.Now,
                        CreateBy = item.CreateBy
                    }).First();
            return result;
        }
        public List<ComboboxItem> Combobox(string query)
        {
            var ls = listStockDA.GetQuery(t => (t.Name.Contains(query) || string.IsNullOrEmpty(query)) && t.Active).Select(item => new ComboboxItem()
            {
                ID = item.ID,
                Name = item.Name,
                Address = item.Address
            }).ToList();
            return ls;
        }
        public string GetNameStockById(int id)
        {
            var rs = listStockDA.GetById(id);
            if (rs != null)
                return rs.Name;
            return "";
        }
        public string GetMaxCode()
        {
            var lstTmp = listStockDA.GetQuery().OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Code;
            return "";
        }
        public int GetIdByName(string name)
        {
            var rs = listStockDA.GetQuery().Where(t => t.Name.Equals(name.Trim())).FirstOrDefault();
            if (rs != null)
                return rs.ID;
            return 0;
        }
        public List<StockItem> GetAll(ModelFilter filter)
        {
            var data = listStockDA.GetQuery()
                        .Filter(filter)
                        .Select(item => new StockItem
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Active = item.Active,
                            Address = item.Address,
                            Contact = item.Contact,
                            CreateAt = item.CreateAt,
                            Description = item.Description,
                            Email = item.Email,
                            Fax = item.Fax,
                            Manager = item.Manager,
                            ImageMap = item.ImageMap,
                            Mobi = item.Mobi,
                            Name = item.Name,
                            Telephone = item.Telephone,
                        })
                        .OrderByDescending(t => t.CreateAt)
                        .ToList();
            return data;
        }
        public List<StockItem> GetAll()
        {
            var data = listStockDA.GetQuery(t => t.Active == true)
                .Select(item => new StockItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name
                    })
                .ToList();
            return data;
        }
        public void Insert(StockItem item)
        {
            Base.Stock lsStock = new Base.Stock()
            {
                ImageMap = item.ImageMap,
                Name = item.Name,
                Address = item.Address,
                Code = item.Code,
                Contact = item.Contact,
                Description = item.Description,
                Email = item.Email,
                Fax = item.Fax,
                Manager = item.Manager,
                Mobi = item.Mobi,
                Active = item.Active,
                Telephone = item.Telephone,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            listStockDA.Insert(lsStock);
            listStockDA.Save();
        }
        public void Update(StockItem item)
        {
            Base.Stock lsStock = listStockDA.GetById(item.ID);
            lsStock.ImageMap = item.ImageMap != null ? item.ImageMap : lsStock.ImageMap;
            lsStock.Name = item.Name;
            lsStock.Address = item.Address;
            lsStock.Code = item.Code;
            lsStock.Contact = item.Contact;
            lsStock.Description = item.Description;
            lsStock.Email = item.Email;
            lsStock.Fax = item.Fax;
            lsStock.Manager = item.Manager;
            lsStock.Mobi = item.Mobi;
            lsStock.Active = item.Active;
            lsStock.Telephone = item.Telephone;
            lsStock.UpdateAt = DateTime.Now;
            lsStock.UpdateBy = item.UpdateBy;
            listStockDA.Update(lsStock);
            listStockDA.Save();
        }
        public bool CheckNameExits(string name)
        {
            var data = listStockDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
            if (data != null)
            {
                return true;
            }
            return false;
        }
        public bool CheckCodeExits(string code)
        {
            var data = listStockDA.GetQuery(t => t.Code.ToUpper() == code.ToUpper()).FirstOrDefault();
            if (data != null)
            {
                return true;
            }
            return false;
        }
        public void DeleteRange(string strIds)
        {
            var ids = strIds.Split(',').Select(n => (object)int.Parse(n)).ToList();
            listStockDA.DeleteRange(ids);
            listStockDA.Save();
        }
    }
}
