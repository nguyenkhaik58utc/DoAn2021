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
    public class HumanAnswerSV
    {
        private HumanAnswerDA criteriaDA = new HumanAnswerDA();
        public List<HumanAnswerItem> GetByCateId(ModelFilter filter, int questionId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.HumanAnswers.Where(t => t.HumanQuestionID == questionId)
                          .Select(item => new HumanAnswerItem()
                          {
                              ID = item.ID,
                              Answer = item.Answer,
                              HumanQuestionID = item.HumanQuestionID,
                              IsTrue = item.IsTrue,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public bool Insert(HumanAnswerItem obj)
        {
            if (criteriaDA.GetQuery(i => i.Answer.Trim().ToUpper() == obj.Answer.Trim().ToUpper() && i.HumanQuestionID==obj.HumanQuestionID).FirstOrDefault() != null)
            {
                return false;
            }
            var itm = new HumanAnswer
            {
                Answer = obj.Answer,
                IsTrue = obj.IsTrue,
                HumanQuestionID = obj.HumanQuestionID,
                CreateAt = DateTime.Now,
                CreateBy = obj.CreateBy
            };
            criteriaDA.Insert(itm);
            criteriaDA.Save();
            return true;
        }
        public bool Update(HumanAnswerItem obj)
        {
            var itm = criteriaDA.GetById(obj.ID);
            if (itm.Answer != obj.Answer && criteriaDA.GetQuery(i => i.Answer.Trim().ToUpper() == obj.Answer && i.HumanQuestionID == obj.HumanQuestionID).FirstOrDefault() != null)
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
        public bool Delete(int id)
        {
            ProfileDA profileDA = new ProfileDA();
            var profileUsing = profileDA.GetQuery(p => p.ProfileSecurityID == id);
            if (profileUsing.Count() > 0)
                return false;
            var item = criteriaDA.GetById(id);
            criteriaDA.Delete(item);
            criteriaDA.Save();
            return true;
        }
        public HumanAnswerItem GetByID(int id)
        {
            var obj = criteriaDA.GetById(id);
            if (obj != null)
                return ConvertToHumanAnswerItem(obj);
            return null;
        }
        private HumanAnswerItem ConvertToHumanAnswerItem(HumanAnswer profileSecurity)
        {
            var obj = new HumanAnswerItem()
            {
                ID = profileSecurity.ID,
                Answer = profileSecurity.Answer,
                IsTrue = profileSecurity.IsTrue,
                CreateAt = profileSecurity.CreateAt,
                CreateBy = profileSecurity.CreateBy,
                UpdateAt = profileSecurity.UpdateAt,
                UpdateBy = profileSecurity.UpdateBy
            };
            if (obj != null)
            {
                HumanUserSV userSV = new HumanUserSV();
                obj.CreateAnswer = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateAnswer = userSV.GetNameByUserID(obj.UpdateBy);
            }
            return obj;
        }
    }
}
