using iDAS.Base;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
  public  class ProductDevelopmentRequirementPlanDocumentSV
    {
      private ProductDevelopmentRequirementPlanDocumentDA productDevelopmentRequirementPlanDocumentDA = new ProductDevelopmentRequirementPlanDocumentDA();
      public void Insert(int productDevelopmentRequirementPlanId, int documentId, int userId)
      {
          ProductDevelopmentRequirementPlanDocument obj = new ProductDevelopmentRequirementPlanDocument();
          obj.DocumentID = documentId;
          obj.ProductDevelopmentRequirementPlanID = productDevelopmentRequirementPlanId;
          obj.CreateAt = DateTime.Now;
          obj.CreateBy = userId;
          productDevelopmentRequirementPlanDocumentDA.Insert(obj);
          productDevelopmentRequirementPlanDocumentDA.Save();

      }
      public bool CheckDocumentProductionIsApproved(int productDevelopmentRequirementPlanId)
      {
          var dbo = productDevelopmentRequirementPlanDocumentDA.Repository;
          var lstDocument = productDevelopmentRequirementPlanDocumentDA.GetQuery(t=>t.ProductDevelopmentRequirementPlanID==productDevelopmentRequirementPlanId).Select(t=>t.DocumentID).ToArray();
          var data = dbo.Documents.Where(t => lstDocument.Contains(t.ID) && t.IsAccept && t.IsPublic && !t.IsObsolete).ToList();
          return data.Count() > 0 ? true : false;
      }
    }
}
