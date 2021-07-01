using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Services
{
   public class AuditResultNCSV
    {
       private QualityNCSV NCService = new QualityNCSV();
       private QualityNCDA NCDA = new QualityNCDA();

     
       public void Insert(QualityNCItem NCItem, int auditResultID)
       {
           var dbo = NCDA.Repository;
           int id = NCService.InsertReturnID(NCItem);
           var auditResult = dbo.AuditResults.FirstOrDefault(i => i.ID == auditResultID);
           auditResult.QualityNCID = id;
           dbo.SaveChanges();
       }
       public List<QualityNCItem> GetAll(int page, int pageSize, out int totalCount, int auditID)
       {
           var dbo = NCDA.Repository;
           var targets = dbo.AuditResults
               .Where(i => i.ID == auditID)
               .Join(dbo.QualityNCs, c => c.QualityNCID, NC => NC.ID, (c, item) => new QualityNCItem()
               {
                   ID = item.ID,
                   IsObs = item.IsObs,
                   IsMaximum = item.IsMaximum,
                   IsMedium = item.IsMedium,
                   IsComplete = item.IsComplete,
                   CreateAt = item.CreateAt
               })
               .OrderByDescending(item => item.CreateAt)
               .Page(page, pageSize, out totalCount)
               .ToList();
           return targets;
       }
       
    }
}
