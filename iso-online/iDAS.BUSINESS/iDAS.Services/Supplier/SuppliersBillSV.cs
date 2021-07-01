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
    public class SuppliersBillSV
    {
        private SuppliersBillDA billOrderDA = new SuppliersBillDA();
        public List<SuppliersBillItem> GetAllbyOrderID(int id)
        {
            var suppBills = billOrderDA.GetQuery(i => i.SuppliersOrderID == id).Select(item => new SuppliersBillItem()
                {
                    ID = item.ID,
                    PayedMoney = item.PayedMoney,
                    PayDate = item.PayDate,
                    BillType = item.BillType,
                    SuppliersOrder = new SuppliersOrderItem()
                    {
                        SupplierName = item.SuppliersOrder.Supplier.Name,
                        CODE = item.SuppliersOrder.CODE,
                        Amount = item.SuppliersOrder.Amount,
                        SuppliersOrderDetails = item.SuppliersOrder.SuppliersOrderDetails.Select(x=>new SuppliersOrderDetailItem()
                        {
                            ReciveQuantity = x.ReciveQuantity,
                            Price = x.Price
                        }).ToList()
                    }
                }).ToList();
            return suppBills;
        }

        public void Insert(SuppliersBillItem data, int p)
        {
            SuppliersBill obj = new SuppliersBill();
            if(data !=null)
            {
                obj.PayedMoney = data.PayedMoney;
                obj.PayDate = data.PayDate;
                obj.SuppliersOrderID = data.SuppliersOrderID;
                obj.Note = data.Note;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = p;
                billOrderDA.Insert(obj);
                billOrderDA.Save();
            }

        }

        public void Update(SuppliersBillItem data, int p)
        {
            SuppliersBill obj = billOrderDA.GetById(data.ID);
            if (data != null)
            {
                obj.PayedMoney = data.PayedMoney;
                obj.PayDate = data.PayDate;
                obj.Note = data.Note;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = p;
                billOrderDA.Update(obj);
                billOrderDA.Save();
            }
        }

        public void Delete(int id)
        {
            billOrderDA.Delete(id);
            billOrderDA.Save();
        }

        public object GetById(int id)
        {
            SuppliersBillItem obj = new SuppliersBillItem();
            var data = billOrderDA.Get(t => t.ID == id).FirstOrDefault();
            if (data != null)
            {
                obj.ID = data.ID;
                obj.PayedMoney = data.PayedMoney;
                obj.PayDate = data.PayDate;
                obj.SuppliersOrderID = data.SuppliersOrderID;
                obj.Note = data.Note;                
            }
            return obj;
        }
    }
}
