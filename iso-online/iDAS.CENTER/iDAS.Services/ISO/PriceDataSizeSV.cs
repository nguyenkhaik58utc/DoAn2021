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
    public class PriceDataSizeSV
    {
        private PriceDataSizeDA priceDataSizeDA = new PriceDataSizeDA();
        public PriceDataSizeItem GetById(int id)
        {
            var result = priceDataSizeDA.GetQuery(i => i.ID == id)
                .Select(item => new PriceDataSizeItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    Value = item.Value,
                    Price = item.Price
                })
                .First();
            return result;
        }
        #region Insert, Update, Delete
        public List<PriceDataSizeItem> GetAll(int page, int pageSize, out int total)
        {
            try
            {
                List<PriceDataSizeItem> lst = new List<PriceDataSizeItem>();
                var lstType = priceDataSizeDA.GetQuery().OrderByDescending(t => t.ID).Page(page, pageSize, out total).ToList();

                if (lstType != null && lstType.Count > 0)
                {
                    foreach (var item in lstType)
                    {
                        lst.Add(new PriceDataSizeItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            Value = item.Value,
                            Price = item.Price

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
        public List<PriceDataSizeItem> GetAll()
        {
            try
            {
                List<PriceDataSizeItem> lst = new List<PriceDataSizeItem>();
                var lstType = priceDataSizeDA.GetQuery().OrderByDescending(t => t.ID).ToList();
                if (lstType != null && lstType.Count > 0)
                {
                    foreach (var item in lstType)
                    {
                        lst.Add(new PriceDataSizeItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            Value = item.Value,
                            Price = item.Price

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


        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>

        public bool Insert(PriceDataSizeItem objNew)
        {
            try
            {
                PriceDataSize obj = new PriceDataSize
                {
                    Name = objNew.Name,
                    Price   = objNew.Price,
                    Value = objNew.Value,
                    Description = objNew.Description,
                    UpdateBy = objNew.CreateBy,
                    CreateAt= DateTime.Now,
                    CreateBy = objNew.CreateBy,
                };
                obj.UpdateAt = DateTime.Now;
                priceDataSizeDA.Insert(obj);
                priceDataSizeDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// cập nhật
        /// </summary>
        /// <param name="objNew"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        public bool Update(PriceDataSizeItem objNew, int? id)
        {
            try
            {
                var obj = priceDataSizeDA.GetById(id);
                obj.Name = objNew.Name;
                obj.Description = objNew.Description;
                obj.UpdateBy = objNew.UpdateBy;
                obj.UpdateAt = DateTime.Now;
                obj.Price = objNew.Price;
                obj.Value = objNew.Value;
                priceDataSizeDA.Update(obj);
                priceDataSizeDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// xóa 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        public bool Delete(int? id)
        {
            try
            {
                priceDataSizeDA.Delete(id);
                priceDataSizeDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
