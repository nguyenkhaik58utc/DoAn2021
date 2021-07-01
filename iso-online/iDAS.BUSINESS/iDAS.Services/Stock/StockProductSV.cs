using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class StockProductSV
    {
        private StockProductDA productDA = new StockProductDA();
        private StockUnitDA genericUnitDA = new StockUnitDA();
        public List<ComboboxItem> Combobox(string query)
        {
            var ls = productDA.GetQuery(t => (t.Name.Contains(query) || string.IsNullOrEmpty(query))).Select(item => new ComboboxItem()
            {
                ID = item.ID,
                Name = item.Name
            }).ToList();
            return ls;
        }
        private string GetUnitNameById(int id)
        {
            var rs = genericUnitDA.GetById(id);
            if (rs != null)
                return rs.Name;
            return string.Empty;
        }
        public decimal GetMinStock(int idproduct)
        {
            var obj = productDA.GetQuery(t => t.ID == idproduct).FirstOrDefault();
            if (obj != null)
            {
                return obj.MinStock.ToString().Trim().Equals("") != true ? (decimal)obj.MinStock : 0;
            }
            else
            {
                return 0;
            }
        }
        public decimal GetMaxStock(int idproduct)
        {
            var obj = productDA.GetQuery(t => t.ID == idproduct).FirstOrDefault();
            if (obj != null)
            {
                return obj.MaxStock.ToString().Trim().Equals("") != true ? (decimal)obj.MaxStock : 0;
            }
            else
            {
                return 0;
            }
        }
        public string GetMaxCode()
        {
            var lstTmp = productDA.GetQuery().OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Code;
            return "";
        }
        public int GetUnitIdByProductId(int StockProductID)
        {
            var rs = productDA.GetById(StockProductID);
            if (rs != null)
                return (int)rs.Unit_ID;
            return 0;
        }
        public string GetGroupNameByProductId(int id)
        {
            var rs = productDA.GetById(id);
            return rs.StockProductGroup.Name;
        }
        public string GetNameByProductId(int id)
        {
            var rs = productDA.GetById(id);
            return rs.Name;
        }
        public string GetCodeByProductId(int id)
        {
            var rs = productDA.GetById(id);
            return rs.Code;
        }
        public bool CheckCodeExits(string code)
        {
            var data = productDA.GetQuery().OrderBy(t => t.ID).Where(t => t.Code.ToUpper() == code.ToUpper()).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool CheckCodeEditExits(int id, string code)
        {
            var data = productDA.GetQuery().OrderBy(t => t.ID).Where(t => t.Code.ToUpper() == code.ToUpper() && t.ID != id).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }
        public StockProductItem GetById(int id)
        {
            StockProductItem lst = new StockProductItem();
            var dbo = productDA.Repository;
            var item = productDA.GetQuery(t => t.ID == id).OrderByDescending(t => t.ID).FirstOrDefault();
            if (item != null)
            {

                lst.ID = item.ID;
                lst.Code = item.Code;
                lst.Active = item.Active;
                lst.AverageCost = item.AverageCost;
                lst.Barcode = item.Barcode;
                lst.Batch = item.Batch;
                lst.Type_Name = Common.GetProductTypeName(item.Type_ID.ToString().Equals("") == false ? (int)item.Type_ID : 30);
                lst.Color = item.Color;
                lst.Commission = item.Commission;
                lst.CostMethod = item.CostMethod;
                lst.CreateAt = item.CreateAt;
                lst.CreateBy = item.CreateBy;
                lst.Currency_ID = item.Currency_ID;
                lst.CurrentCost = item.CurrentCost;
                lst.Customer_ID = item.Customer_ID;
                lst.Customer_Name = item.Customer_Name;
                lst.Depth = item.Depth;
                lst.Description = item.Description;
                lst.Discount = item.Discount;
                lst.ExchangeRate = item.ExchangeRate;
                lst.Expiry = item.Expiry;
                lst.ExpiryValue = item.ExpiryValue;
                lst.ExportTax_ID = item.ExportTax_ID;
                lst.Group_ID = item.StockProductGroupID;
                lst.Height = item.Height;
                lst.ImportTax_ID = item.ImportTax_ID;
                lst.LimitOrders = item.LimitOrders;
                lst.LuxuryTax_ID = item.LuxuryTax_ID;
                lst.Group_Name = dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID).Name;
                lst.Manufacturer_ID = item.Manufacturer_ID;
                lst.MaxStock = item.MaxStock;
                lst.MinStock = item.MinStock;
                lst.Model_ID = item.Model_ID;
                lst.Name = item.Name;
                lst.NameEN = item.NameEN;
                lst.Org_Price = item.Org_Price;
                lst.Origin = item.Origin;
                lst.ImageProduct = item.ImageProduct;
                lst.Provider_ID = item.Provider_ID;
                lst.Quantity = item.Quantity;
                lst.Retail_Price = item.Retail_Price;
                lst.Sale_Price = item.Sale_Price;
                lst.Serial = item.Serial;
                lst.Size = item.Size;
                lst.StockID = item.StockID;
                lst.Stock_Name = dbo.Stocks.FirstOrDefault(i => i.ID == item.StockID).Name;
                lst.Thickness = item.Thickness;
                lst.Type_ID = item.Type_ID;
                lst.Unit_ID = item.Unit_ID;
                lst.Unit_Name = GetUnitNameById((int)item.Unit_ID);
                lst.UnitConvert = item.UnitConvert;
                lst.UnitRate = item.UnitRate;
                lst.VAT_ID = item.VAT_ID;
                lst.Warranty = item.Warranty;
                lst.Width = item.Width;
                lst.UpdateAt = item.UpdateAt;
                lst.UpdateBy = item.UpdateBy;
            }

            return lst;
        }
        public List<StockProductItem> GetByMaterialsGroupId(string[] strwhere, string[] strwhere1)
        {
            var dbo = productDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<StockProductItem> lst = new List<StockProductItem>();
            var lstMaterials = productDA.GetQuery().Where(t => output.Contains((int)t.StockProductGroupID)).ToList();
            if (lstMaterials != null && lstMaterials.Count > 0)
            {
                foreach (var item in lstMaterials)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID).Name,
                        Description = item.Description,
                        Active = Boolean.Parse(item.Active.ToString())
                    });
                }
            }
            return lst;
        }
        public List<StockProductItem> GetAll(ModelFilter filter)
        {
            var dbo = productDA.Repository;
            var data = productDA.GetQuery()
                         .OrderByDescending(t => t.ID)
                                .Filter(filter)
                                .ToList();
            List<StockProductItem> lst = new List<StockProductItem>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Active = item.Active,
                        AverageCost = item.AverageCost,
                        Barcode = item.Barcode,
                        Batch = item.Batch,
                        Type_Name = Common.GetProductTypeName(item.Type_ID.ToString().Equals("") == false ? (int)item.Type_ID : 30),
                        Color = item.Color,
                        Commission = item.Commission,
                        CostMethod = item.CostMethod,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        Currency_ID = item.Currency_ID,
                        CurrentCost = item.CurrentCost,
                        Customer_ID = item.Customer_ID,
                        Customer_Name = item.Customer_Name,
                        Depth = item.Depth,
                        Description = item.Description,
                        Discount = item.Discount,
                        ExchangeRate = item.ExchangeRate,
                        Expiry = item.Expiry,
                        ExpiryValue = item.ExpiryValue,
                        ExportTax_ID = item.ExportTax_ID,
                        Group_ID = item.StockProductGroupID,
                        Height = item.Height,
                        ImportTax_ID = item.ImportTax_ID,
                        LimitOrders = item.LimitOrders,
                        LuxuryTax_ID = item.LuxuryTax_ID,
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID) == null ? "" : dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID).Name,
                        Manufacturer_ID = item.Manufacturer_ID,
                        MaxStock = item.MaxStock,
                        MinStock = item.MinStock,
                        Model_ID = item.Model_ID,
                        Name = item.Name,
                        NameEN = item.NameEN,
                        Org_Price = item.Org_Price,
                        Origin = item.Origin,
                        ImageProduct = item.ImageProduct,
                        Provider_ID = item.Provider_ID,
                        Quantity = item.Quantity,
                        Retail_Price = item.Retail_Price,
                        Sale_Price = item.Sale_Price,
                        Serial = item.Serial,
                        Size = item.Size,
                        StockID = item.StockID,
                        Stock_Name = dbo.Stocks.FirstOrDefault(i => i.ID == item.StockID) != null ? dbo.Stocks.FirstOrDefault(i => i.ID == item.StockID).Name : "",
                        Thickness = item.Thickness,
                        Type_ID = item.Type_ID,
                        Unit_ID = item.Unit_ID,
                        Unit_Name = item.Unit_ID.HasValue ? dbo.StockUnits.FirstOrDefault(i => i.ID == item.Unit_ID) != null ? dbo.StockUnits.FirstOrDefault(i => i.ID == item.Unit_ID).Name : string.Empty : string.Empty,
                        UnitConvert = item.UnitConvert,
                        UnitRate = item.UnitRate,
                        VAT_ID = item.VAT_ID,
                        Warranty = item.Warranty,
                        Width = item.Width,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    });
                }
            }

            return lst;
        }
        /// <summary>
        /// Vinh DQ 07/2015
        /// Get List StockProduct using for combobox
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<StockProductItem> GetAllforCombobox()
        {
            var dbo = productDA.Repository;
            var data = productDA.GetQuery()
                                .ToList();
            List<StockProductItem> lst = new List<StockProductItem>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        NameEN = item.NameEN
                    });
                }
            }

            return lst;
        }
        /// <summary>
        /// CuongPC
        /// </summary>
        /// <returns></returns>
        public List<StockProductItem> GetAllforComboboxNotDev()
        {
            var dbo = productDA.Repository;
            var data = productDA.GetQuery(t => t.Type_ID == (int)Common.Product_Type.spcpt)
                                .ToList();
            List<StockProductItem> lst = new List<StockProductItem>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        NameEN = item.NameEN
                    });
                }
            }

            return lst;
        }
        public bool CheckNameExits(string name)
        {
            var data = productDA.GetQuery().OrderBy(t => t.ID).Where(t => t.Name.ToUpper() == name.ToUpper()).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }
        public List<StockProductItem> GetListProductByStringId(string[] strwhere)
        {
            var dbo = productDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<StockProductItem> lst = new List<StockProductItem>();
            var lstProducts = productDA.GetQuery().Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Code = item.Code,
                        Unit_Name = GetUnitNameById((int)item.Unit_ID),
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID).Name,
                        Description = item.Description
                    });
                }
            }
            return lst;
        }
        public List<StockProductItem> GetListProductByType()
        {
            var dbo = productDA.Repository;
            List<StockProductItem> lst = new List<StockProductItem>();
            var lstProducts = productDA.GetQuery(t => t.Type_ID == 2 || t.Type_ID == 5 || t.Type_ID == 6).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (var item in lstProducts)
                {
                    if (dbo.StockProductBuilds.FirstOrDefault(m => m.Build_ID == item.ID) != null)
                    {
                        lst.Add(new StockProductItem
                        {
                            ID = item.ID,
                            Name = item.Name
                        });
                    }
                }
            }
            return lst;
        }
        public List<StockProductItem> GetProductsByGroup_Id(string[] strwhere)
        {
            var dbo = productDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<StockProductItem> lst = new List<StockProductItem>();
            var lstProducts = productDA.GetQuery().Where(t => output.Contains((int)t.StockProductGroupID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Code = item.Code,
                        Unit_Name = GetUnitNameById((int)item.Unit_ID),
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID).Name,
                        Description = item.Description

                    });
                }
            }
            return lst;
        }
        public List<StockProductItem> GetProductsByGroup(ModelFilter filter, string[] strwhere)
        {
            var dbo = productDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            var lstProducts = productDA.GetQuery().Where(t => output.Contains((int)t.StockProductGroupID)).Select(item => new StockProductItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Code = item.Code,
                        Unit_Name = dbo.StockUnits.Where(t=>t.ID==item.Unit_ID).FirstOrDefault()!=null?dbo.StockUnits.Where(t=>t.ID==item.Unit_ID).FirstOrDefault().Name:string.Empty,
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID).Name,
                        Description = item.Description,
                        CreateAt = item.CreateAt
                    })
                    .Filter(filter)
                    .ToList();
            return lstProducts;
        }
        public List<StockProductItem> GetProductsByGroup_IdForStart(string[] strwhere, List<int> idpro)
        {
            var dbo = productDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<StockProductItem> lst = new List<StockProductItem>();
            var lstProducts = productDA.GetQuery().Where(t => output.Contains((int)t.StockProductGroupID) && !idpro.Contains(t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Code = item.Code,
                        Unit_Name = GetUnitNameById((int)item.Unit_ID),
                        Group_Name = dbo.StockProductGroups.FirstOrDefault(i => i.ID == item.StockProductGroupID).Name,
                        Description = item.Description

                    });
                }
            }
            return lst;
        }
        public List<StockProductItem> GetProductsForCreate(int start, int limit, int page, string query, int productCreate_id)
        {
            List<StockProductItem> lst = new List<StockProductItem>();
            var lstProducts = productDA.GetQuery(t => t.ID != productCreate_id && t.Name.Contains(query)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (var item in lstProducts)
                {
                    lst.Add(new StockProductItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Code = item.Code,
                        Unit_Name = GetUnitNameById((int)item.Unit_ID)
                    });
                }
            }
            return lst;
        }
        public void Insert(StockProductItem item)
        {
            StockProduct product = new StockProduct()
            {
                Code = item.Code,
                Active = item.Active,
                AverageCost = item.AverageCost,
                Barcode = item.Barcode,
                Batch = item.Batch,
                Color = item.Color,
                Commission = item.Commission,
                CostMethod = item.CostMethod,
                CreateAt = item.CreateAt,
                CreateBy = item.CreateBy,
                Currency_ID = item.Currency_ID,
                CurrentCost = item.CurrentCost,
                Customer_ID = item.Customer_ID,
                Customer_Name = item.Customer_Name,
                Depth = item.Depth,
                Description = item.Description,
                Discount = item.Discount,
                ExchangeRate = item.ExchangeRate,
                Expiry = item.Expiry,
                ExpiryValue = item.ExpiryValue,
                ExportTax_ID = item.ExportTax_ID,
                StockProductGroupID = item.Group_ID,
                Height = item.Height,
                ImportTax_ID = item.ImportTax_ID,
                LimitOrders = item.LimitOrders,
                LuxuryTax_ID = item.LuxuryTax_ID,
                Manufacturer_ID = item.Manufacturer_ID,
                MaxStock = item.MaxStock,
                MinStock = item.MinStock,
                Model_ID = item.Model_ID,
                Name = item.Name,
                NameEN = item.NameEN,
                Org_Price = item.Org_Price,
                Origin = item.Origin,
                ImageProduct = item.ImageProduct,
                Provider_ID = item.Provider_ID,
                Quantity = item.Quantity,
                Retail_Price = item.Retail_Price,
                Sale_Price = item.Sale_Price,
                Serial = item.Serial,
                Size = item.Size,
                StockID = item.StockID,
                Thickness = item.Thickness,
                Type_ID = item.Type_ID,
                Unit_ID = item.Unit_ID,
                UnitConvert = item.UnitConvert,
                UnitRate = item.UnitRate,
                VAT_ID = item.VAT_ID,
                Warranty = item.Warranty,
                Width = item.Width,
            };
            productDA.Insert(product);
            productDA.Save();
        }
        public void Update(StockProductItem item)
        {
            StockProduct product = productDA.GetById(item.ID);
            product.Code = item.Code;
            product.Active = item.Active;
            product.AverageCost = item.AverageCost;
            product.Barcode = item.Barcode;
            product.Batch = item.Batch;
            product.Color = item.Color;
            product.Commission = item.Commission;
            product.CostMethod = item.CostMethod;
            product.CreateAt = item.CreateAt;
            product.CreateBy = item.CreateBy;
            product.Currency_ID = item.Currency_ID;
            product.CurrentCost = item.CurrentCost;
            product.Customer_ID = item.Customer_ID;
            product.Customer_Name = item.Customer_Name;
            product.Depth = item.Depth;
            product.Description = item.Description;
            product.Discount = item.Discount;
            product.ExchangeRate = item.ExchangeRate;
            product.Expiry = item.Expiry;
            product.ExpiryValue = item.ExpiryValue;
            product.ExportTax_ID = item.ExportTax_ID;
            product.StockProductGroupID = item.Group_ID;
            product.Height = item.Height;
            product.ImportTax_ID = item.ImportTax_ID;
            product.LimitOrders = item.LimitOrders;
            product.LuxuryTax_ID = item.LuxuryTax_ID;
            product.Manufacturer_ID = item.Manufacturer_ID;
            product.MaxStock = item.MaxStock;
            product.MinStock = item.MinStock;
            product.Model_ID = item.Model_ID;
            product.Name = item.Name;
            product.NameEN = item.NameEN;
            product.Org_Price = item.Org_Price;
            product.Origin = item.Origin;
            product.ImageProduct = item.ImageProduct != null ? item.ImageProduct : product.ImageProduct;
            product.Provider_ID = item.Provider_ID;
            product.Quantity = item.Quantity;
            product.Retail_Price = item.Retail_Price;
            product.Sale_Price = item.Sale_Price;
            product.Serial = item.Serial;
            product.Size = item.Size;
            product.StockID = item.StockID;
            product.Thickness = item.Thickness;
            product.Type_ID = item.Type_ID;
            product.Unit_ID = item.Unit_ID;
            product.UnitConvert = item.UnitConvert;
            product.UnitRate = item.UnitRate;
            product.VAT_ID = item.VAT_ID;
            product.Warranty = item.Warranty;
            product.Width = item.Width;
            productDA.Update(product);
            productDA.Save();
        }
        public void DeleteRange(string strIds)
        {
            var ids = strIds.Split(',').Select(n => (object)int.Parse(n)).ToList();
            productDA.DeleteRange(ids);
            productDA.Save();
        }
    }
}
