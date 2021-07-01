using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;

namespace iDAS.Services
{
    public class HumanResultAnswerSV
    {
        private HumanResultAnswerDA humanResultAnswerDA = new HumanResultAnswerDA();
        private int[] GetByHumanResultQuestionID(int humanResultQuestionID)
        {
            try
            {
                var modules = humanResultAnswerDA.GetQuery()
                                    .Where(a => a.HumanResultQuestionID == humanResultQuestionID)
                                    .Select(a => a.HumanAnswerID).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool SetIsSelected(int humanResultQuestionID, int humanAnswerID)
        {
            if (GetByHumanResultQuestionID(humanResultQuestionID).Contains(humanAnswerID))
            {
                return true;
            }
            return false;
        }
        public List<HumanResultAnswerItem> GetByCateId(int questionId, int humanResultQuestionID)
        {
            var dbo = humanResultAnswerDA.Repository;
            List<HumanResultAnswerItem> lst = new List<HumanResultAnswerItem>();
            var criterias = dbo.HumanAnswers
                               .Where(t => t.HumanQuestionID == questionId)                               
                               .ToList();
            if (criterias.Count > 0)
            {
                foreach (var item in criterias)
                {
                    lst.Add(new HumanResultAnswerItem
                   {
                       HumanResultQuestionID = humanResultQuestionID,
                       Answer = item.Answer,
                       CreateAt = item.CreateAt,
                       CreateBy = item.CreateBy,
                       HumanAnswerID = item.ID,
                       ID = item.ID,
                       IsSelected = SetIsSelected(humanResultQuestionID, item.ID)
                   });
                }
            }
            return lst;
        }
    }
}
