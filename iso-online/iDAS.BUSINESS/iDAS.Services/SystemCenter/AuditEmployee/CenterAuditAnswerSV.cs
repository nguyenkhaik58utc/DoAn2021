using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class CenterAuditAnswerSV
    {
        private CenterAuditAnswerDA criteriaDA = new CenterAuditAnswerDA();
        public List<CenterAuditAnswerItem> GetByCateId(ModelFilter filter, int questionId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterAuditAnswers.Where(t => t.CenterAuditQuestionID == questionId)
                          .Select(item => new CenterAuditAnswerItem()
                          {
                              ID = item.ID,
                              Answer = item.Answer,
                              CenterAuditQuestionID = item.CenterAuditQuestionID,
                              IsTrue = item.IsTrue,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
    }
}
