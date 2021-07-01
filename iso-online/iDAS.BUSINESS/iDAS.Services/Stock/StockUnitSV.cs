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
    public class StockUnitSV
    {
        private StockUnitDA unitDA = new StockUnitDA();
        public bool CheckNameExits(string name)
        {
            var rs = (unitDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper() && !t.IsDelete)).FirstOrDefault();
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
            var rs = unitDA.GetById(id);
            return rs.Name;
        }
        public StockUnitItem GetById(int Id)
        {
            var result = unitDA.GetQuery(i => i.ID == Id)
                        .Select(item => new StockUnitItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            Active = item.Active,
                            CreateAt = DateTime.Now,
                        }).First();
            return result;
        }
        public List<StockProductGroupItem> GetAll(int page, int pageSize, out int total)
        {
            try
            {
                List<StockProductGroupItem> lst = new List<StockProductGroupItem>();
                var lstProductGroup = unitDA.GetQuery(t=>!t.IsDelete).OrderBy(t => t.Name).Page(page, pageSize, out total).ToList();
                if (lstProductGroup != null && lstProductGroup.Count > 0)
                {
                    foreach (var item in lstProductGroup)
                    {
                        lst.Add(new StockProductGroupItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            IsUse = item.Active == true ? true : false,
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<StockProductGroupItem> GetGroupSupplier(int page, int pageSize, out int total)
        {
            try
            {
                List<StockProductGroupItem> lst = new List<StockProductGroupItem>();
                var lstProductGroup = unitDA.GetQuery().OrderBy(t => t.Name).Page(page, pageSize, out total).ToList();
                if (lstProductGroup != null && lstProductGroup.Count > 0)
                {
                    foreach (var item in lstProductGroup)
                    {
                        lst.Add(new StockProductGroupItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            //IsUse = (bool)item.IsUse
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<StockProductGroupItem> GetListAll()
        {
            try
            {
                List<StockProductGroupItem> lst = new List<StockProductGroupItem>();
                var lstProductGroup = unitDA.GetQuery().ToList();
                if (lstProductGroup != null && lstProductGroup.Count > 0)
                {
                    foreach (var item in lstProductGroup)
                    {
                        lst.Add(new StockProductGroupItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<StockProductGroupItem> GetListAllIsUse()
        {
            try
            {
                List<StockProductGroupItem> lst = new List<StockProductGroupItem>();
                var lstProductGroup = unitDA.GetQuery(t=>t.Active && !t.IsDelete).ToList();
                if (lstProductGroup != null && lstProductGroup.Count > 0)
                {
                    foreach (var item in lstProductGroup)
                    {
                        lst.Add(new StockProductGroupItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Insert(string name, string description, bool isUse, int userId)
        {
            StockUnit unit = new StockUnit()
            {
                Name = name,
                Description = description,
                Active = isUse,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };
            unitDA.Insert(unit);
            unitDA.Save();
        }
        public void Update(StockUnitItem item)
        {
            StockUnit unit = unitDA.GetById(item.ID);
            unit.Name = item.Name;
            unit.Description = item.Description;
            unit.Active = item.Active;
            unit.UpdateAt = DateTime.Now;
            unit.UpdateBy = item.UpdateBy;
            unitDA.Update(unit);
            unitDA.Save();
        }
        public void DeleteRange(string strIds)
        {
            var ids = strIds.Split(',').Select(n => (object)int.Parse(n)).ToList();
            foreach (var item in ids)
            {
                var obj = unitDA.GetById(item);
                obj.IsDelete = true;
                unitDA.Update(obj);
            }
            unitDA.Save();
        }
    }
}
