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
    public class HumanProfileWorkTrialResultSV
    {
        private HumanProfileWorkTrialResultDA pwtDA = new HumanProfileWorkTrialResultDA();
        public void InsertListCri(string strCateIds, int humanProfileWorkTrialID, int userID)
        {
            var dbo = pwtDA.Repository; 
            var ids = strCateIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var lst = dbo.QualityCriterias.Where(x => ids.Contains(x.QualityCriteriaCategoryID.Value)).ToList();
            foreach(var data in lst)
            {
                var item = new HumanProfileWorkTrialResult()
                {
                    HumanProfileWorkTrialID = humanProfileWorkTrialID,
                    QualityCriteriaID = data.ID,
                    CreateAt = DateTime.Now,
                    CreateBy = userID
                };
                pwtDA.Insert(item);
            }            
            pwtDA.Save();
        }
    }
}
