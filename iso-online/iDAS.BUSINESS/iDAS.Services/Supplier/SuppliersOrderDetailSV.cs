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
    public class SuppliersOrderDetailSV
    {
        private SuppliersOrderDetailDA suppOrderDetailDA = new SuppliersOrderDetailDA();

        public List<SuppliersOrderDetailItem> GetListProductByStringId(string[] strwhere, int rs)
        {
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            var dbo = suppOrderDetailDA.Repository;
            List<SuppliersOrderDetailItem> lst = new List<SuppliersOrderDetailItem>();
            var lstProducts = dbo.SuppliersOrderRequirementDetails.Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (var item in lstProducts)
                {
                    lst.Add(new SuppliersOrderDetailItem
                    {
                        ID = item.ID,
                        StocksProductID = item.StocksProductID,
                        Quantity = item.Quantity,
                        StockProduct = new StockProductItem
                        {
                            Name = item.StockProduct.Name,
                            Unit_Name = dbo.StockUnits.FirstOrDefault(i => i.ID == item.StockProduct.Unit_ID).Name,
                            Code = item.StockProduct.Code
                        },
                        Note = item.Note,
                        SuppliersOrderRequirementDetailID = item.ID
                    });

                }
            }
            return lst;
        }

        public SuppliersOrderDetailItem getByID(int ID)
        {
            var item = suppOrderDetailDA.GetById(ID);
            var dbo = suppOrderDetailDA.Repository;
            return new SuppliersOrderDetailItem()
                {
                    ID =ID,
                    StocksProductID = item.StockProduct.ID,
                    Quantity = 0,
                    StockProduct = new StockProductItem
                    {
                        ID = item.StockProduct.ID,
                        Name = item.StockProduct.Name,
                        Retail_Price = item.StockProduct.Retail_Price,
                        Unit_Name = dbo.StockUnits.FirstOrDefault(i => i.ID == item.StockProduct.Unit_ID).Name,
                        Code = item.StockProduct.Code
                    }
                };
        }

        public void Insert(SuppliersOrderDetailItem odt,int OrderID)
        {
            SuppliersOrderDetail SOD = new SuppliersOrderDetail()
            {                
                StocksProductID = odt.StocksProductID,
                SuppliersOrderID = OrderID,
                Quantity = odt.Quantity,
                Note = odt.Note,
                SuppliersOrderRequirementDetailID =odt.SuppliersOrderRequirementDetailID
            };
            suppOrderDetailDA.Insert(SOD);
            suppOrderDetailDA.Save();
        }
        public void DeleteByIdSuppOrderID(int ids)
        {
            var dbo = suppOrderDetailDA.Repository;
            List<SuppliersOrderDetail> lstObj =dbo.SuppliersOrderDetails.Where(i => i.SuppliersOrderID == ids).ToList();
             //Cập nhật lại OrderRequired
            foreach(SuppliersOrderDetail odt in lstObj)
            {
                new SuppliersOrderRequirementDetailSV().UpdateStatus(odt.SuppliersOrderRequirementDetailID.Value, 1);
            }            
            suppOrderDetailDA.DeleteRange(lstObj);
            suppOrderDetailDA.Save();
        }

        public void Update(SuppliersOrderDetailItem odt)
        {
            var item = suppOrderDetailDA.GetById(odt.ID);
            if(item!= null)
                item.Price = odt.Price;
            suppOrderDetailDA.Update(item);
            suppOrderDetailDA.Save();

        }

        public void UpdateRecive(SuppliersOrderDetailItem odt)
        {
            var item = suppOrderDetailDA.GetById(odt.ID);
            if (item != null)
            {
                item.ReciveQuantity = odt.ReciveQuantity;
                item.ReciveQuality = odt.ReciveQuality;
                item.Note = odt.Note;
            }
            new SuppliersOrderRequirementDetailSV().UpdateStatus(item.SuppliersOrderRequirementDetailID.Value, 4);
            suppOrderDetailDA.Update(item);
            suppOrderDetailDA.Save();
        }
    }
}
