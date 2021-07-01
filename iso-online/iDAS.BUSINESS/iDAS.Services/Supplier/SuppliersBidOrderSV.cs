using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using System.Security.Principal;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;

namespace iDAS.Services
{
    public class SuppliersBidOrderSV
    {
        SuppliersBidOrderDA suppBidOrderDA = new SuppliersBidOrderDA();
        public SuppliersBidOrderItem GetById(int id)
        {
            var dbo = suppBidOrderDA.Repository;
            SuppliersBidOrderItem obj = new SuppliersBidOrderItem();
            
            var data = suppBidOrderDA.Get(t => t.ID == id).FirstOrDefault();
            if (data != null)
            {
                obj.ID = data.ID;
                obj.StartDate = data.StartDate;
                obj.EndDate = data.EndDate;
                obj.ReceiveDate = data.ReceiveDate;
                obj.Status = data.Status;
                obj.SupplierID = data.SupplierID;
                obj.SuppliersOrderID = data.SuppliersOrderID;
                obj.Contents = data.Contents;
                obj.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.SupplierBidToAttachmentFiles.Where(z => z.SupplierBidOrderID == obj.ID)
                        .Select(d => d.AttachmentFileID).ToList()
                };
            }
            return obj;
        }
        public List<SuppliersBidOrderItem> GetAllbySuppliersOrderID(int orderID)
        {

            var data = suppBidOrderDA.GetQuery(item=>item.SuppliersOrderID == orderID).Select(item => new SuppliersBidOrderItem()
            {
                ID = item.ID,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                ReceiveDate = item.ReceiveDate,
                Status = item.Status,
                SupplierID = item.SupplierID,
                SuppliersOrderID = item.SuppliersOrderID,
                Supplier = new SupplierItem()
                {                    
                    Name = item.Supplier.Name,
                    Code =item.Supplier.Code,
                    Email = item.Supplier.Email
                },
                Contents = item.Contents
            }).OrderByDescending(item => item.ID)                        
                        .ToList();            
            return data;
        }

        public void Insert(SuppliersBidOrderItem data, int p)
        {
            SuppliersBidOrder obj = new SuppliersBidOrder();
            if(data != null)
            {
                obj.SupplierID = data.SupplierID;
                obj.SuppliersOrderID = data.SuppliersOrderID;
                obj.StartDate = data.StartDate;
                obj.EndDate = data.EndDate;
                obj.Contents = data.Contents;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = p;
                obj.Status = false;
            }
            //new FileSV().Upload<SupplierBidToAttachmentFile>(data.AttachmentFiles, data.ID);
            suppBidOrderDA.Insert(obj);
            suppBidOrderDA.Save();
        }
        public void Update(SuppliersBidOrderItem data, int p)
        {
            SuppliersBidOrder obj = suppBidOrderDA.GetById(data.ID);
            if (data != null)
            {
                obj.SupplierID = data.SupplierID;
                obj.SuppliersOrderID = data.SuppliersOrderID;
                obj.StartDate = data.StartDate;
                obj.EndDate = data.EndDate;
                obj.Contents = data.Contents;
                obj.ReceiveDate = data.ReceiveDate;
                obj.Status = data.Status;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = p;
            }
            //new FileSV().Upload<SupplierBidToAttachmentFile>(data.AttachmentFiles, data.ID);
            suppBidOrderDA.Update(obj);
            suppBidOrderDA.Save();
        }
        public void UpdateStatus(SuppliersBidOrderItem data, int p)
        {
            SuppliersBidOrder obj = suppBidOrderDA.GetById(data.ID);
            if (data != null)
            {
                obj.Status = data.Status;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = p;
            }
            suppBidOrderDA.Update(obj);
            suppBidOrderDA.Save();
        }
        public void Delete(int id)
        {
            suppBidOrderDA.Delete(id);
            suppBidOrderDA.Save();
        }
        public void DeleteByIdSuppOrderID(int ids)
        {
            var dbo = suppBidOrderDA.Repository;
            List<SuppliersBidOrder> lstObj = dbo.SuppliersBidOrders.Where(i => i.SuppliersOrderID == ids).ToList();
            suppBidOrderDA.DeleteRange(lstObj);
            suppBidOrderDA.Save();
        }
        public List<int> GetAllByOrderId(int orderID)
        {
            var data = suppBidOrderDA.GetQuery(item => item.SuppliersOrderID == orderID).Select(item => item.SupplierID).ToList();
            return data;
        }
    }
}
