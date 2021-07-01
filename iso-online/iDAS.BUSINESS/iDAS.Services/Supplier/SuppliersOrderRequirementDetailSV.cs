using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.Services;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class SuppliersOrderRequirementDetailSV
    {
        private SuppliersOrderRequirementDetailDA suppOrderDetailDA = new SuppliersOrderRequirementDetailDA();
        public void Insert(SuppliersOrderRequirementDetailItem odt, int OrderID)
        {
            SuppliersOrderRequirementDetail SOD = new SuppliersOrderRequirementDetail()
            {
                StocksProductID = odt.StockProduct.ID,
                SuppliersOrderRequirementID = OrderID,
                Quantity = odt.Quantity,
                Note = odt.Note,
                Status = 1 
            };
            suppOrderDetailDA.Insert(SOD);
            suppOrderDetailDA.Save();
        }

        public void DeleteByIdSuppOrderID(int ids)
        {
            var dbo = suppOrderDetailDA.Repository;
            List<SuppliersOrderRequirementDetail> lstObj = dbo.SuppliersOrderRequirementDetails.Where(i => i.SuppliersOrderRequirementID == ids).ToList();
            suppOrderDetailDA.DeleteRange(lstObj);
            suppOrderDetailDA.Save();
        }

        public List<SuppliersOrderRequirementDetailItem> GetListProductByStringId(string[] strwhere, int rs)
        {
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            var dbo = suppOrderDetailDA.Repository;
            List<SuppliersOrderRequirementDetailItem> lst = new List<SuppliersOrderRequirementDetailItem>();
            var lstProducts = dbo.StockProducts.Where(t => output.Contains((int)t.ID)).ToList();
            if (lstProducts != null && lstProducts.Count > 0)
            {
                int n = 1;
                foreach (var item in lstProducts)
                {
                    lst.Add(new SuppliersOrderRequirementDetailItem
                    {
                        ID = n + rs,
                        StocksProductID = item.ID,
                        Quantity = 0,
                        StockProduct = new StockProductItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Retail_Price = item.Retail_Price,
                            Unit_Name = dbo.StockUnits.FirstOrDefault(i => i.ID == item.Unit_ID).Name,
                            Code = item.Code
                        },
                        Note = ""
                    });
                    n++;
                }
            }
            return lst;
        }

        public List<SuppliersOrderRequirementDetailItem> GetOrderDetailByID(int page, int pageSize, out int totalCount, string[] strwhere)
        {
            var dbo = suppOrderDetailDA.Repository;
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            //Chỉ lấy những sản phẩm chưa được lưu vào đơn hàng nào
            var lst = dbo.SuppliersOrderRequirementDetails.Where(i =>output.Contains(i.SuppliersOrderRequirementID.Value) && i.Status==1).ToList();
            List<SuppliersOrderRequirementDetailItem> rs = new List<SuppliersOrderRequirementDetailItem>();
            foreach(SuppliersOrderRequirementDetail item in lst)
            {
                rs.Add(new SuppliersOrderRequirementDetailItem
                {
                    ID = item.ID,
                    StocksProductID = item.ID,
                    Quantity = item.Quantity,
                    StockProduct = new StockProductItem
                    {
                        ID = item.ID,
                        Name = item.StockProduct.Name,
                        Unit_Name = dbo.StockUnits.FirstOrDefault(i => i.ID == item.StockProduct.Unit_ID).Name,
                        Code = item.StockProduct.Code,
                        Group_Name = item.StockProduct.StockProductGroup.Name
                    },
                    Note = item.Note,
                    SuppliersOrderRequirement = new SuppliersOrderRequirementItem
                    {
                        Name = item.SuppliersOrderRequirement.Name
                    }
                });
            }
            return rs.OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
        }

        public void UpdateStatus(int OrderRequiredDetailID,int status)
        {
            var obj = suppOrderDetailDA.GetById(OrderRequiredDetailID);
            if(obj!=null)
            {
                obj.Status = status;
                suppOrderDetailDA.Update(obj);
                suppOrderDetailDA.Save();
            }
            
        }
    }
}
