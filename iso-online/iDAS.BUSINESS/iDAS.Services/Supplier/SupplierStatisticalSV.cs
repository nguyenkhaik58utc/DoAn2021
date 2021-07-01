using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class SupplierStatisticalSV
    {
        SupplierDA suppDA = new SupplierDA();
        public List<SupplierStatisticalItem> getDataStatistical(int id, DateTime fromDate, DateTime toDate)
        {
            var dbo = suppDA.Repository;
            var results = dbo.SuppliersGroups
                           .Where(i => (i.ParentID != null && i.ParentID == id))
                            .Select(item => new SupplierStatisticalItem()
                            {
                                CateID = item.ID,
                                ParentID = item.ParentID,
                                Name = item.Name,
                                Leaf = !dbo.SuppliersGroups.Any(i => i.ParentID == item.ID)
                            }).ToList();
            foreach(var i in results)
            {
                var lstChild = new SuppliersGroupSV().getChilds(dbo.SuppliersGroups, i.CateID).Select(t => t.ID).ToList();
                if (!lstChild.Contains(i.CateID)) lstChild.Add(i.CateID);
                var lstOrder = dbo.SuppliersOrders.Where(item => item.Status > 7 && lstChild.Contains(item.Supplier.SuppliersGroupID))
                        .Where(item=>item.OrderDate.HasValue)
                        .Where(item=>item.OrderDate>=fromDate && item.OrderDate<=toDate)
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CODE = item.CODE,
                            OrderDate = item.OrderDate,
                            ShipDate = item.ShipDate,
                            ReciepDate = item.ReciepDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name,
                            SupplierName = item.Supplier.Name,
                            Amount = item.Amount,
                            Discount = item.Discount,
                            Payment = item.Payment,
                            SuppliersBills = item.SuppliersBills.Select(t => new SuppliersBillItem()
                            {
                                PayDate = t.PayDate,
                                PayedMoney = t.PayedMoney
                            }).ToList(),
                            SuppliersOrderDetails = item.SuppliersOrderDetails.Select(t => new SuppliersOrderDetailItem()
                            {
                                ReciveQuantity = t.ReciveQuantity,
                                Price = t.Price
                            }).ToList()
                        })
                        .ToList();

                i.Data1 = dbo.Suppliers.Where(x => lstChild.Contains(x.SuppliersGroupID)).Count();
                i.Data2 = dbo.SuppliersOrders.Where(x => lstChild.Contains(x.Supplier.SuppliersGroupID))
                        .Where(item => item.OrderDate.HasValue)
                        .Where(item => item.OrderDate >= fromDate && item.OrderDate <= toDate)
                        .Select(x => x.SupplierID).Distinct().Count();
                i.Data3 = lstOrder.Count();
                i.TotalAmount = lstOrder.Sum(x => x.Amount.Value);
                i.TotalPayed = lstOrder.Sum(x => x.Payed);
                i.TotalOwe = lstOrder.Sum(x => x.Owe);
                i.TotalRecive = lstOrder.Sum(x => x.AmountRecive);
            }
            return results;
        }
    }
}
