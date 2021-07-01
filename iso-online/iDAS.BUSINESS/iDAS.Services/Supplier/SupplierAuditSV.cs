using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Utilities;

namespace iDAS.Services
{
    public class SupplierAuditSV
    {
        private SupplierAuditDA suppAuditDA = new SupplierAuditDA();
        public List<SupplierAuditItem> GetAllByPlanId(int page, int pageSize, out int total, int planID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierAuditPlanID == planID).Select(item => new SupplierAuditItem
            {
                ID = item.ID,
                Supplier = new SupplierItem
                {
                    Name = item.Supplier.Name
                },
                SupplierID = item.SupplierID,
                SupplierAuditPlanID = item.SupplierAuditPlanID,
                SupplierAuditPlan = new SupplierAuditPlanItem
                {
                    Name = item.SupplierAuditPlan.QualityPlan.Name,
                    StartTime = item.SupplierAuditPlan.QualityPlan.StartTime,
                    EndTime = item.SupplierAuditPlan.QualityPlan.EndTime
                },
                IsPass = item.IsPass
            })
            .OrderByDescending(item => item.ID)
            .Page(page, pageSize, out total)
            .ToList();
            return data;
        }

        public List<SupplierAuditItem> GetSummaryByPalnID(int page, int pageSize, out int total, int planID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierAuditPlanID == planID).Select(item => new SupplierAuditItem
            {
                ID = item.ID,
                Supplier = new SupplierItem
                {
                    Name = item.Supplier.Name,
                    Commodity = item.Supplier.Commodity,
                    SuppliersOrders = item.Supplier.SuppliersOrders.Select(x => new SuppliersOrderItem()
                    {
                        ID = x.ID
                    }).ToList()
                },
                SupplierID = item.SupplierID,
                SupplierAuditPlanID = item.SupplierAuditPlanID,
                SupplierAuditResults = item.SupplierAuditResults.Select(x => new SupplierAuditResultItem()
                {
                    TotalPoint = x.TotalPoint
                }).ToList()
            })
            .OrderByDescending(item => item.ID)
            .Page(page, pageSize, out total)
            .ToList();
            return data;
        }
        public List<SupplierAuditItem> GetAllBySuppID(int page, int pageSize, out int total, int SuppID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierID == SuppID).Select(item => new SupplierAuditItem
            {
                ID = item.ID,
                Supplier = new SupplierItem
                {
                    Name = item.Supplier.Name
                },
                SupplierID = item.SupplierID,
                SupplierAuditPlanID = item.SupplierAuditPlanID,
                SupplierAuditPlan = new SupplierAuditPlanItem
                {
                    Name = item.SupplierAuditPlan.QualityPlan.Name,
                    StartTime = item.SupplierAuditPlan.QualityPlan.StartTime,
                    EndTime = item.SupplierAuditPlan.QualityPlan.EndTime
                }
            })
            .OrderByDescending(item => item.ID)
            .Page(page, pageSize, out total)
            .ToList();
            return data;
        }
        public List<int> GetAllByPlanId(int planID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierAuditPlanID == planID).Select(item => item.SupplierID).ToList();
            return data;
        }
        public void UpdateIsPass(int id, bool isPass)
        {
            var obj = suppAuditDA.GetById(id);
            if (obj != null)
            {
                obj.IsPass = isPass;
            }
            suppAuditDA.Update(obj);
            suppAuditDA.Save();
        }
        public void Insert(SupplierAuditItem supplierAuditItem)
        {
            SupplierAudit suppAudit = new SupplierAudit
            {
                SupplierID = supplierAuditItem.SupplierID,
                SupplierAuditPlanID = supplierAuditItem.SupplierAuditPlanID
            };
            suppAuditDA.Insert(suppAudit);
            suppAuditDA.Save();
        }

        public void Delete(int id)
        {
            suppAuditDA.Delete(id);
            suppAuditDA.Save();
        }



        public List<SupplierAuditItem> GetAllByPlanIdChecked(int page, int pageSize, out int total, int planID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierAuditPlanID == planID)
                .Where(x => x.SupplierAuditResults.Count() > 0 || x.IsPass == true)
                .Select(item => new SupplierAuditItem
            {
                ID = item.ID,
                Supplier = new SupplierItem
                {
                    Name = item.Supplier.Name
                },
                SupplierID = item.SupplierID,
                SupplierAuditPlanID = item.SupplierAuditPlanID,
                SupplierAuditPlan = new SupplierAuditPlanItem
                {
                    Name = item.SupplierAuditPlan.QualityPlan.Name,
                    StartTime = item.SupplierAuditPlan.QualityPlan.StartTime,
                    EndTime = item.SupplierAuditPlan.QualityPlan.EndTime
                },
                IsPass = item.IsPass
            })
            .OrderByDescending(item => item.ID)
            .Page(page, pageSize, out total)
            .ToList();
            return data;
        }
        public List<SupplierAuditItem> GetAllByPlanIdNotChecked(int page, int pageSize, out int total, int planID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierAuditPlanID == planID)
                .Where(x => x.SupplierAuditResults.Count() == 0 || x.IsPass != true)
                .Select(item => new SupplierAuditItem
                {
                    ID = item.ID,
                    Supplier = new SupplierItem
                    {
                        Name = item.Supplier.Name
                    },
                    SupplierID = item.SupplierID,
                    SupplierAuditPlanID = item.SupplierAuditPlanID,
                    SupplierAuditPlan = new SupplierAuditPlanItem
                    {
                        Name = item.SupplierAuditPlan.QualityPlan.Name,
                        StartTime = item.SupplierAuditPlan.QualityPlan.StartTime,
                        EndTime = item.SupplierAuditPlan.QualityPlan.EndTime
                    },
                    IsPass = item.IsPass
                })
            .OrderByDescending(item => item.ID)
            .Page(page, pageSize, out total)
            .ToList();
            return data;
        }

        public List<SupplierAuditItem> GetAllByPlanIdPass(int page, int pageSize, out int total, int planID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierAuditPlanID == planID)
                 .Where(x => x.IsPass.HasValue && x.IsPass.Value == true)
                 .Select(item => new SupplierAuditItem
                 {
                     ID = item.ID,
                     Supplier = new SupplierItem
                     {
                         Name = item.Supplier.Name
                     },
                     SupplierID = item.SupplierID,
                     SupplierAuditPlanID = item.SupplierAuditPlanID,
                     SupplierAuditPlan = new SupplierAuditPlanItem
                     {
                         Name = item.SupplierAuditPlan.QualityPlan.Name,
                         StartTime = item.SupplierAuditPlan.QualityPlan.StartTime,
                         EndTime = item.SupplierAuditPlan.QualityPlan.EndTime
                     },
                     IsPass = item.IsPass
                 })
             .OrderByDescending(item => item.ID)
             .Page(page, pageSize, out total)
             .ToList();
            return data;
        }

        public List<SupplierAuditItem> GetAllByPlanIdNotPass(int page, int pageSize, out int total, int planID)
        {
            var data = suppAuditDA.GetQuery(t => t.SupplierAuditPlanID == planID)
                 .Where(x => !(x.IsPass.HasValue && x.IsPass.Value == true))
                 .Select(item => new SupplierAuditItem
                 {
                     ID = item.ID,
                     Supplier = new SupplierItem
                     {
                         Name = item.Supplier.Name
                     },
                     SupplierID = item.SupplierID,
                     SupplierAuditPlanID = item.SupplierAuditPlanID,
                     SupplierAuditPlan = new SupplierAuditPlanItem
                     {
                         Name = item.SupplierAuditPlan.QualityPlan.Name,
                         StartTime = item.SupplierAuditPlan.QualityPlan.StartTime,
                         EndTime = item.SupplierAuditPlan.QualityPlan.EndTime
                     },
                     IsPass = item.IsPass
                 })
             .OrderByDescending(item => item.ID)
             .Page(page, pageSize, out total)
             .ToList();
            return data;
        }
    }
}
