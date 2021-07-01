using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;

namespace iDAS.Services
{
    public class StockProductBuildSV
    {
        private StockProductBuildDA product_BuildDA  = new StockProductBuildDA();
        private StockProductSV productService = new StockProductSV();
        public List<StockProductBuildItem> GetAll(int page, int pageSize, out int total, int build_Id)
        {
            try
            {
                List<StockProductBuildItem> lst = new List<StockProductBuildItem>();
                var lstProduct_Build = product_BuildDA.GetQuery().OrderBy(t => t.StockProductID).Where(t => t.Build_ID == build_Id).Page(page, pageSize, out total).ToList();
                if (lstProduct_Build != null && lstProduct_Build.Count > 0)
                {
                    foreach (var item in lstProduct_Build)
                    {
                        lst.Add(new StockProductBuildItem
                        {
                            ID = item.ID,
                            Code = item.StockProduct.Code,
                            Amount = item.Amount,
                            Build_ID = item.Build_ID,
                            Price = item.Price,
                            StockProductID = item.StockProductID,
                            Group_Name = item.StockProduct.StockProductGroup.Name,
                            Product_Name = item.StockProduct.Name,
                            Unit = item.UnitName,
                            Quantity = item.Quantity
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
        public bool GetRowByProductID(int productId)
        {
            var data = product_BuildDA.GetQuery(t => t.Build_ID == productId).Select(t => t.ID).ToList();
            if (data.Count > 0)
                return true;
            return false;
        }
        public List<StockProductBuildItem> GetAll(Nullable<int> build_Id)
        {
            try
            {
                List<StockProductBuildItem> lst = new List<StockProductBuildItem>();
                var lstProduct_Build = product_BuildDA.GetQuery().OrderBy(t => t.StockProductID).Where(t => t.Build_ID == build_Id).ToList();
                if (lstProduct_Build != null && lstProduct_Build.Count > 0)
                {
                    foreach (var item in lstProduct_Build)
                    {
                        lst.Add(new StockProductBuildItem
                        {
                            ID = item.ID,
                            Code = item.StockProduct.Code,
                            Amount = item.Amount,
                            Build_ID = item.Build_ID,
                            Price = item.Price,
                            StockProductID = item.StockProductID,
                            Group_Name = item.StockProduct.StockProductGroup.Name,
                            Product_Name = item.StockProduct.Name,
                            Unit = item.UnitName,
                            Quantity = item.Quantity
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
        public StockProductBuildItem GetById(int Id)
        {
            var dbo = product_BuildDA.Repository;
            var result = product_BuildDA.GetQuery(i => i.ID == Id)
                        .Select(item => new StockProductBuildItem()
                        {
                            ID = item.ID,
                            Code = item.StockProduct.Code,
                            Amount = item.Amount,
                            Build_ID = item.Build_ID,
                            Price = item.Price,
                            StockProductID = item.StockProductID,
                            Group_Name = dbo.StockProductGroups.FirstOrDefault(i=>i.ID == item.StockProduct.StockProductGroupID).Name,
                            Product_Name = item.StockProduct.Name,
                            Unit = item.UnitName,
                            Quantity = item.Quantity
                        }).First();
            return result;
        }
        public void Insert(StockProductBuildItem item)
        {
            StockProductBuild stockProduct = new StockProductBuild()
            {
                Amount = item.Amount,
                Build_ID = item.Build_ID,
                Price = item.Price,
                StockProductID = item.StockProductID,
                Quantity = item.Quantity,
                UnitName = item.Unit,
            };
            product_BuildDA.Insert(stockProduct);
            product_BuildDA.Save();
        }
        public void Update(StockProductBuildItem item)
        {
            StockProductBuild stockProduct = product_BuildDA.GetById(item.ID);
            stockProduct.Amount = item.Amount;
            stockProduct.Build_ID = item.Build_ID;
            stockProduct.Price = item.Price;
            stockProduct.StockProductID = item.StockProductID;
            stockProduct.Quantity = item.Quantity;
            stockProduct.UnitName = item.Unit;
            product_BuildDA.Update(stockProduct);
            product_BuildDA.Save();
        }
        public void DeleteRange(string strIds)
        {
            var ids = strIds.Split(',').Select(n => (object)int.Parse(n)).ToList();
            product_BuildDA.DeleteRange(ids);
            product_BuildDA.Save();
        }
    }
}
