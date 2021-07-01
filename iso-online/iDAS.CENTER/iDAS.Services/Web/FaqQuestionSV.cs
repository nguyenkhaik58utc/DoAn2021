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
    public class FaqQuestionSV : BaseService
    {
        FaqQuestionDA faqQuestionDA = new FaqQuestionDA();

        public List<FaqCategoryItem> GetFaqCategories()
        {
            var dbo = faqQuestionDA.Repository;
            var faqs = dbo.WebFaqCategories
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Position)
                        .Select(i => new FaqCategoryItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Description = i.Description,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .ToList();
            return faqs;
        }
        public List<FaqQuestionItem> GetFaqQuestions(int page, int pageSize, out int total, int faqCategoryId)
        {
            var faqs = faqQuestionDA.GetQuery()
                        .Where(i => i.WebFaqCategoryID == faqCategoryId)
                        .Where(i => !i.IsDelete)
                        .Select(i => new FaqQuestionItem()
                        {
                            ID = i.ID,
                            Question = i.Question,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return faqs;
        }
        public FaqQuestionItem GetFaqQuestion(int id)
        {
            var faqItem = faqQuestionDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new FaqQuestionItem()
                            {
                                FaqCategoryID = i.WebFaqCategoryID,
                                ID = i.ID,
                                Question = i.Question,
                                Position = i.Position,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return faqItem;
        }
        public void Insert(FaqQuestionItem item)
        {
            var faqQuestion = new WebFaqQuestion()
            {
                WebFaqCategoryID = item.FaqCategoryID,
                Question = item.Question.Trim(),
                Position = item.Position,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            faqQuestionDA.Insert(faqQuestion);
            faqQuestionDA.Save();
        }
        public void Update(FaqQuestionItem item)
        {
            var faqQuestion = faqQuestionDA.GetById(item.ID);
            faqQuestion.WebFaqCategoryID = item.FaqCategoryID;
            faqQuestion.Question = item.Question;
            faqQuestion.Position = item.Position;
            faqQuestion.IsActive = item.IsActive;
            faqQuestion.UpdateAt = DateTime.Now;
            faqQuestion.UpdateBy = User.ID;
            faqQuestionDA.Save();
        }
        public void Delete(int id)
        {
            var item = faqQuestionDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            faqQuestionDA.Save();
        }
        public bool CheckExist(FaqQuestionItem item)
        {
            var check = faqQuestionDA.GetQuery()
                        .Where(i => i.WebFaqCategoryID == item.FaqCategoryID)
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Question.ToUpper().Equals(item.Question.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
