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
    public class FaqAnswerSV : BaseService
    {
        FaqAnswerDA faqAnswerDA = new FaqAnswerDA();

        public List<FaqAnswerItem> GetFaqAnswers(int page, int pageSize, out int total, int faqQuestionId)
        {
            var faqAnswers = faqAnswerDA.GetQuery()
                        .Where(i => i.WebFaqQuestionID == faqQuestionId)
                        .Where(i => !i.IsDelete)
                        .Select(i => new FaqAnswerItem()
                        {
                            ID = i.ID,
                            Answer = i.Answer,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return faqAnswers;
        }
        public FaqAnswerItem GetFaqAnswer(int id)
        {
            var faqAnswer = faqAnswerDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new FaqAnswerItem()
                            {
                                FaqQuestionID = i.WebFaqQuestionID,
                                ID = i.ID,
                                Answer = i.Answer,
                                Position = i.Position,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return faqAnswer;
        }
        public void Insert(FaqAnswerItem item)
        {
            var faqAnswer = new WebFaqAnswer()
            {
                WebFaqQuestionID = item.FaqQuestionID,
                Answer = item.Answer.Trim(),
                Position = item.Position,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            faqAnswerDA.Insert(faqAnswer);
            faqAnswerDA.Save();
        }
        public void Update(FaqAnswerItem item)
        {
            var faqAnswer = faqAnswerDA.GetById(item.ID);
            faqAnswer.Answer = item.Answer;
            faqAnswer.IsActive = item.IsActive;
            faqAnswer.Position = item.Position;
            faqAnswer.UpdateAt = DateTime.Now;
            faqAnswer.UpdateBy = User.ID;
            faqAnswerDA.Save();
        }
        public void Delete(int id)
        {
            var item = faqAnswerDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            faqAnswerDA.Save();
        }
        public bool CheckExist(FaqAnswerItem item)
        {
            var check = faqAnswerDA.GetQuery()
                        .Where(i => i.WebFaqQuestionID == item.FaqQuestionID)
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Answer.ToUpper().Equals(item.Answer.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
