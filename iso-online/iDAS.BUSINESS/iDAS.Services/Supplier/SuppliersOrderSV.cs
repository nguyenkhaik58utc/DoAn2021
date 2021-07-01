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
    public class SuppliersOrderSV
    {
        private SuppliersOrderDA suppOrderDA = new SuppliersOrderDA();
        public List<SuppliersOrderItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrders
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).Name,
                            },                            
                            //IsApproval = item.IsApproval.HasValue?item.IsApproval.Value:false,
                            //ApprovalAt = item.ApprovalAt,
                            //ApprovalBy = item.ApprovalBy,
                            //ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,                            
                            Name = item.Name
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }

        public SuppliersOrderItem GetById(int id)
        {
            SuppliersOrderItem obj = new SuppliersOrderItem();
            var dbo = suppOrderDA.Repository;
            var data = suppOrderDA.Get(t => t.ID == id).FirstOrDefault();
            if (data != null)
            {
                obj.ID = data.ID;
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.SuppliersOrderDetails = data.SuppliersOrderDetails.Select(item => new SuppliersOrderDetailItem()
                     {
                         ID=item.ID,
                         Quantity = item.Quantity,
                         Price = item.Price,
                         Note = item.Note,
                         StocksProductID = item.StocksProductID,
                         StockProduct = new StockProductItem
                         {
                             ID = item.StockProduct.ID,
                             Name = item.StockProduct.Name,
                             Unit_Name = dbo.StockUnits.FirstOrDefault(i => i.ID == item.StockProduct.Unit_ID).Name,
                             Code = item.StockProduct.Code
                         },
                         ReciveQuality = item.ReciveQuality,
                         ReciveQuantity = item.ReciveQuantity,
                         SuppliersOrderRequirementDetailID = item.SuppliersOrderRequirementDetailID
                     }).ToList();
                obj.CODE = data.CODE;
                //obj.Approval = new iDAS.Items.HumanEmployeeViewItem()
                //            {
                //                ID = data.ApprovalBy != null ? (int)data.ApprovalBy : 0,
                //                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == data.ApprovalBy).Name,
                //                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == data.ApprovalBy).HumanRole.Name,
                //                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == data.ApprovalBy).HumanRole.HumanDepartment.Name,
                //            };
                //obj.ApprovalBy = data.ApprovalBy;
                //obj.IsEdit = data.IsEdit.HasValue?data.IsEdit.Value:true;
                //obj.IsApproval = data.IsApproval.HasValue ? data.IsApproval.Value : true;
                //obj.IsAccept = data.IsAccept.HasValue ? data.IsAccept.Value : true;
                obj.Payment = data.Payment;
                obj.ReciepPlace = data.ReciepPlace;
                obj.ReciepDate = data.ReciepDate;
                obj.ShipDate = data.ShipDate;
                obj.OrderDate = data.OrderDate;
                obj.Status = data.Status;
                obj.Amount = data.Amount;
                obj.ShipType = data.ShipType;
                obj.Discount = data.Discount;
                obj.CreateAt = data.CreateAt;
                obj.CreateBy = data.CreateBy;
                obj.UpdateAt = data.UpdateAt;
                obj.UpdateBy = data.UpdateBy;
                obj.SupplierID = data.SupplierID;
                
                obj.SupplierName = data.SupplierID.HasValue?data.Supplier.Name:"";
            }
            return obj;
        }

        public int Insert(SuppliersOrderItem data)
        {
            SuppliersOrder obj = new SuppliersOrder();
            if (data != null)
            {
                obj.CODE = data.CODE;
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.ReciepPlace = data.ReciepPlace;
                obj.Payment = data.Payment;
                obj.ShipType = data.ShipType;
                obj.Status = data.Status;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = data.CreateBy;
            }
            suppOrderDA.Insert(obj);
            suppOrderDA.Save();
            return obj.ID;
        }

        public void Update(SuppliersOrderItem data)
        {
            if(data!= null)
            {
                SuppliersOrder obj = suppOrderDA.GetById(data.ID);
                obj.CODE = data.CODE;
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.ReciepPlace = data.ReciepPlace;
                obj.Payment = data.Payment;
                obj.ShipType = data.ShipType;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = data.UpdateBy;
                //obj.ApprovalBy = data.Approval.ID;
                //obj.IsEdit = data.IsEdit;
                //obj.IsApproval = data.IsApproval;
               
                suppOrderDA.Update(obj);
                suppOrderDA.Save();
            }            
        }
        public void UpdateOrder(SuppliersOrderItem data)
        {
            if (data != null)
            {
                SuppliersOrder obj = suppOrderDA.GetById(data.ID);
                obj.Payment = data.Payment;
                obj.ReciepPlace = data.ReciepPlace;
                obj.ReciepDate = data.ReciepDate;
                obj.ShipDate = data.ShipDate;
                obj.OrderDate = data.OrderDate;
                obj.Status = data.Status;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = data.UpdateBy;
                obj.Discount = data.Discount;
                obj.Amount = data.Amount;

                suppOrderDA.Update(obj);
                suppOrderDA.Save();
            }
        }
        //public void Approved(SuppliersOrderItem data)
        //{
        //    var dbo = suppOrderDA.Repository;
        //    var pl = suppOrderDA.GetById(data.ID);
        //    pl.IsApproval = true;
        //    pl.IsEdit = data.IsEdit;
        //    pl.ApprovalAt = data.ApprovalAt;
        //    pl.IsAccept = data.IsAccept;
        //    pl.Note = data.ApprovalNote;
        //    if (data.IsAccept == true)
        //        pl.Status = 4;
        //    else if (data.IsEdit)
        //        pl.Status = 2;
        //    else
        //        pl.Status = 5;
        //    suppOrderDA.Update(pl);
        //    suppOrderDA.Save();
        //}
        public void Delete(int ids)
        {
            //Xóa orderdetail trước
            new SuppliersOrderDetailSV().DeleteByIdSuppOrderID(ids);
            //Xóa oderBid
            new SuppliersBidOrderSV().DeleteByIdSuppOrderID(ids);
            suppOrderDA.Delete(ids);
            suppOrderDA.Save();
        }
        
        public string GetMaxCode()
        {
            var lstTmp = suppOrderDA.GetQuery(t => t.CODE.Contains("HD")).OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.CODE;
            return string.Empty;
        }

        public void UpdateStatus(int status,int OrderID)
        {
            SuppliersOrder obj = suppOrderDA.GetById(OrderID);
            if(obj.Status<status)
            {
                obj.Status = status;
                suppOrderDA.Update(obj);
                suppOrderDA.Save();
            }
            
        }


        public List<SuppliersOrderItem> GetOrder(int page, int pageSize, out int totalCount)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrders.Where(i=> i.Status>=6 || i.Status==4)
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).Name,
                            },
                            //IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            //ApprovalAt = item.ApprovalAt,
                            //ApprovalBy = item.ApprovalBy,
                            //ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            CODE = item.CODE,
                            OrderDate = item.OrderDate,
                            ShipDate = item.ShipDate,
                            ReciepDate = item.ReciepDate,
                            Name = item.Name
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<SuppliersOrderItem> GetOrderRequirement(int page, int pageSize, out int totalCount)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrders.Where(i=>i.Status<6)
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanUsers.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).HumanEmployee.Name,
                            },
                            CODE = item.CODE,
                            OrderDate = item.OrderDate,
                            ShipDate = item.ShipDate,
                            ReciepDate = item.ReciepDate,
                            //IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            //ApprovalAt = item.ApprovalAt,
                            //ApprovalBy = item.ApprovalBy,
                            //ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        
        public List<SuppliersOrderItem> GetOrderList(int page, int pageSize, out int totalCount)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrders.Where(i=>i.Status>6)
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).Name,
                            },
                            CODE = item.CODE,
                            OrderDate = item.OrderDate,
                            ShipDate = item.ShipDate,
                            ReciepDate = item.ReciepDate,
                            //IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            //ApprovalAt = item.ApprovalAt,
                            //ApprovalBy = item.ApprovalBy,
                            //ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name,
                            SupplierName = item.Supplier.Name
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<SuppliersOrderItem> GetOrderListBySuppID(int page, int pageSize, out int totalCount,int SuppID)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrders.Where(i => i.Status ==9 && i.SupplierID==SuppID)
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).Name,
                            },
                            CODE = item.CODE,
                            OrderDate = item.OrderDate,
                            ShipDate = item.ShipDate,
                            ReciepDate = item.ReciepDate,
                            //IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            //ApprovalAt = item.ApprovalAt,
                            //ApprovalBy = item.ApprovalBy,
                            //ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name,
                            SupplierName = item.Supplier.Name
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<SuppliersOrderItem> GetOrderListBySuppID(int supplierid, string query)
        {
            var dbo = suppOrderDA.Repository;
            var orders = dbo.SuppliersOrders
                        .Where(i => i.Status == 9 && i.SupplierID == supplierid)
                        .Where(i => string.IsNullOrEmpty(query) || i.CODE.Contains(query) || i.Name.ToUpper().Contains(query.ToUpper()))
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CODE = item.CODE,
                            Name = item.Name
                        })
                        .ToList();
            return orders;
        }
        public List<SuppliersOrderItem> GetOrderBill(int page, int pageSize, out int totalCount)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrders.Where(i => i.Status > 7)
                        .Select(item => new SuppliersOrderItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanEmployees.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).Name,
                            },
                            CODE = item.CODE,
                            OrderDate = item.OrderDate,
                            ShipDate = item.ShipDate,
                            ReciepDate = item.ReciepDate,
                            //IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            //ApprovalAt = item.ApprovalAt,
                            //ApprovalBy = item.ApprovalBy,
                            //ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name,
                            SupplierName = item.Supplier.Name,
                            Amount = item.Amount,
                            Discount=item.Discount,
                            Payment = item.Payment,
                            SuppliersBills = item.SuppliersBills.Select(i => new SuppliersBillItem()
                            {
                                PayDate = i.PayDate,
                                PayedMoney = i.PayedMoney
                            }).ToList(),
                            SuppliersOrderDetails = item.SuppliersOrderDetails.Select(i => new SuppliersOrderDetailItem()
                            {
                                ReciveQuantity = i.ReciveQuantity,
                                Price = i.Price
                            }).ToList()
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public void UpdateSupplier(int suppID,int orderID)
        {
            SuppliersOrder obj = suppOrderDA.GetById(orderID);
            obj.SupplierID = suppID;
            obj.Status = 7;
            suppOrderDA.Update(obj);
            suppOrderDA.Save();
            
        }
        public string GetCodeById(int orderID)
        {
            var obj = suppOrderDA.GetQuery(t => t.ID == orderID).FirstOrDefault();
            if (obj != null)
                return obj.CODE;
            return string.Empty;

        }

        public void UpdateOrderRecive(SuppliersOrderItem data)
        {
            SuppliersOrder obj = suppOrderDA.GetById(data.ID);
            obj.ReciepDate = data.ReciepDate;
            obj.Status = data.Status;
            suppOrderDA.Update(obj);
            suppOrderDA.Save();
        }

        public List<SuppliersOrderItem> GetOrderListBySuppCate(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate)
        {
            var dbo = suppOrderDA.Repository;
            var lstChild = new SuppliersGroupSV().getChilds(dbo.SuppliersGroups, cateId).Select(t => t.ID).ToList();
            var lstOrder = dbo.SuppliersOrders.Where(item => item.Status > 7 && lstChild.Contains(item.Supplier.SuppliersGroupID))
                        .Where(item => item.OrderDate.HasValue)
                        .Where(item => item.OrderDate >= searchFromDate && item.OrderDate <= searchToDate)
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
            return lstOrder;
        }
    }
}
