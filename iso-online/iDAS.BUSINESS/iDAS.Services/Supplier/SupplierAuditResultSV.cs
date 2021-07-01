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
    public class SupplierAuditResultSV
    {
        SupplierAuditResultDA da = new SupplierAuditResultDA();
        public void Insert(SupplierAuditResultItem data,int userID)
        {
            var temp = da.GetById(data.ID);
            if(temp!=null)//Cập nhật điểm
            {
                temp.Point = data.Point;
                temp.Factory = data.Factory;
                temp.TotalPoint = data.TotalPoint;
                temp.UpdateAt = DateTime.Now;
                temp.UpdateBy = userID;
                da.Update(temp);
            }else//Thêm mới
            {
                SupplierAuditResult obj = new SupplierAuditResult
                {
                    SupplierAuditID = data.SupplierAuditID,
                    QualityCriteria = data.QualityCriteria,
                    Point = data.Point,
                    Factory = data.Factory,
                    TotalPoint = data.TotalPoint,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    CreateBy = userID
                };
                da.Insert(obj);
            }
            da.Save();
        }


        public List<SupplierAuditResultItem> getBySuppAuditID(int supplierAuditID)
        {
            var rs = da.GetQuery(i => i.SupplierAuditID == supplierAuditID).Select(item => new SupplierAuditResultItem
                {
                    ID= item.ID,
                    SupplierAuditID= item.SupplierAuditID,
                    QualityCriteria = item.QualityCriteria,
                    Point = item.Point,
                    Factory = item.Factory,
                    TotalPoint = item.TotalPoint,
                    QualityCriteria1 = new QualityCriteriaItem()
                    {
                        CategoryName = item.QualityCriteria1.QualityCriteriaCategory.Name,
                        Name = item.QualityCriteria1.Name,
                        MaxPoint = item.QualityCriteria1.MaxPoint,
                        MinPoint = item.QualityCriteria1.MinPoint,
                        Factory = item.QualityCriteria1.Factory
                    }
                }).ToList();
            return rs;
        }
        public List<SupplierAuditResultItem> getBySuppAuditIDCategoryID(int supplierAuditID,int cateid)
        {
            var rs = da.GetQuery(i => i.SupplierAuditID == supplierAuditID && i.QualityCriteria1.QualityCriteriaCategoryID==cateid).Select(item => new SupplierAuditResultItem
            {
                ID = item.ID,
                SupplierAuditID = item.SupplierAuditID,
                QualityCriteria = item.QualityCriteria,
                Point = item.Point,
                Factory = item.Factory,
                TotalPoint = item.TotalPoint,
                QualityCriteria1 = new QualityCriteriaItem()
                {
                    CategoryName = item.QualityCriteria1.QualityCriteriaCategory.Name,
                    Name = item.QualityCriteria1.Name
                }
            }).ToList();
            return rs;
        }
        public void Update(SupplierAuditResultItem data, int p)
        {
            var obj = da.GetById(data.ID);
            if(obj!=null)
            {
                obj.SupplierAuditID = data.SupplierAuditID;
                obj.QualityCriteria = data.QualityCriteria;
                obj.Point = data.Point;
                obj.Factory = data.Factory;
                obj.TotalPoint = data.TotalPoint;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = p;
            }
            da.Update(obj);
            da.Save();
        }

        public void DeletebysupplierAuditID(int p)
        {
            throw new NotImplementedException();
        }
    }
}
