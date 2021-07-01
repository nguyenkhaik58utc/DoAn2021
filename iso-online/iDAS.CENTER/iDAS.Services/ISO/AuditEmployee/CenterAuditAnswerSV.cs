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
        public bool Insert(CenterAuditAnswerItem obj)
        {
            if (criteriaDA.GetQuery(i => i.Answer.Trim().ToUpper() == obj.Answer.Trim().ToUpper() && i.CenterAuditQuestionID == obj.CenterAuditQuestionID).FirstOrDefault() != null)
            {
                return false;
            }
            var itm = new CenterAuditAnswer
            {
                Answer = obj.Answer,
                IsTrue = obj.IsTrue,
                CenterAuditQuestionID = obj.CenterAuditQuestionID,
                CreateAt = DateTime.Now,
                CreateBy = obj.CreateBy
            };
            criteriaDA.Insert(itm);
            criteriaDA.Save();
            return true;
        }
        public bool Update(CenterAuditAnswerItem obj)
        {
            var itm = criteriaDA.GetById(obj.ID);
            if (itm.Answer != obj.Answer && criteriaDA.GetQuery(i => i.Answer.Trim().ToUpper() == obj.Answer && i.CenterAuditQuestionID == obj.CenterAuditQuestionID).FirstOrDefault() != null)
            {
                return false;
            }
            itm.Answer = obj.Answer;
            itm.IsTrue = obj.IsTrue;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            criteriaDA.Update(itm);
            criteriaDA.Save();
            return true;
        }
        public CenterAuditAnswerItem GetByID(int id)
        {
            var obj = criteriaDA.GetQuery(i => i.ID == id)
                .Select(profileSecurity => new CenterAuditAnswerItem()
                {
                    ID = profileSecurity.ID,
                    Answer = profileSecurity.Answer,
                    IsTrue = profileSecurity.IsTrue,
                    CreateAt = profileSecurity.CreateAt,
                    CreateBy = profileSecurity.CreateBy,
                    UpdateAt = profileSecurity.UpdateAt,
                    UpdateBy = profileSecurity.UpdateBy
                }).FirstOrDefault();
            return obj;
        }
        public bool Delete(int id)
        {
            var item = criteriaDA.GetById(id);
            criteriaDA.Delete(item);
            criteriaDA.Save();
            return true;
        }
    }
}
