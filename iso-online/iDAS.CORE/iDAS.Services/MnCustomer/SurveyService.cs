using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class SurveyService
    {
        SurveyDA surveyDA = new SurveyDA();

        //public List<SystemGroupItem> GetAll(int page, int pageSize, out int totalCount)
        //{
        //    var groups = surveyDA.GetQuery()
        //                .Select(item => new SystemGroupItem()
        //                {
        //                    ID = item.ID,
        //                    Name = item.Name,
        //                    IsActive = item.IsActive,
        //                    Description = item.Description,
        //                    ParentID = item.ParentID,
        //                    CreateAt = item.CreateAt,
        //                    CreateBy = surveyDA.SystemContext.SystemUsers.Where(u => u.ID == item.CreateBy).FirstOrDefault().FullName,
        //                    UpdateAt = item.UpdateAt,
        //                    UpdateBy = surveyDA.SystemContext.SystemUsers.Where(u => u.ID == item.UpdateBy).FirstOrDefault().FullName,
        //                })
        //                .OrderByDescending(item => item.CreateAt)
        //                .Page(page, pageSize, out totalCount)
        //                .ToList();
        //    return groups;
        //}

        public void Update(SurveyItem item)
        {
            var survey = surveyDA.GetById(item.ID);
            survey.Title = item.Title;
            survey.Point = item.Point;
            survey.ParentID = item.ParentID;
            survey.IsActive = item.IsActive;
            survey.UpdateAt = DateTime.Now;
            survey.UpdateBy = item.UpdateBy;
            surveyDA.Save();
        }

        //public void Insert(SurveyItem item, int userID)
        //{
        //    var survey = new MnCustomerSurvey()
        //    {
        //        ParentID = item.ParentID,
        //        Title = item.Title,
        //        IsActive = item.IsActive,
        //        CreateAt = DateTime.Now,
        //        CreateBy = userID,
        //        UpdateAt = DateTime.Now,
        //        UpdateBy = userID,
        //    };
        //    surveyDA.Insert(survey);
        //    surveyDA.Save();
        //}

        public void Insert(SurveyItem item)
        {
            var survey = new MnCustomerSurvey()
            {
                ParentID = item.ParentID,
                Title = item.Title,
                IsActive = item.IsActive,
                IsSubject = item.IsSubject,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            surveyDA.Insert(survey);
            surveyDA.Save();
            item.ID = survey.ID;
        }

        public void Delete(int id)
        {
            var groupIds = new List<object>();
            groupIds.Add(id);
            GetSurveysByParentID(id, ref groupIds);

            surveyDA.DeleteRange(groupIds);
            surveyDA.Save();
        }

        public SurveyItem GetByID(int id)
        {
            var item = surveyDA.GetById(id);
            var survey = new SurveyItem();
            survey.ID = item.ID;
            survey.ParentID = item.ParentID;
            survey.Title = item.Title;
            survey.IsActive = item.IsActive;
            survey.IsSubject = item.IsSubject;
            survey.CreateAt = item.CreateAt;
            survey.CreateBy = item.CreateBy;
            survey.UpdateAt = item.UpdateAt;
            survey.UpdateBy = item.UpdateBy;
            return survey;
        }

        public void GetSurveysByParentID(int id, ref List<object> surveyIds)
        {
            var surveys = GetSurveysByParentID(id);
            if (surveys.Count <= 0) return;
            foreach (var survey in surveys)
            {
                surveyIds.Add(survey.ID);
                GetSurveysByParentID(survey.ID, ref surveyIds);
            }
        }

        public List<SurveyItem> GetSurveysByParentID(int parentId)
        {
            var surveys = surveyDA.GetQuery()
                        .Where(g => g.ParentID == parentId)
                        .Select(item => new SurveyItem()
                        {
                            ID = item.ID,
                            ParentID = item.ParentID,
                            Title = item.Title,
                            Point = item.Point,
                            IsActive = item.IsActive,
                            IsSubject = item.IsSubject,
                        }).ToList();
            return surveys;
        }

        //public List<int> GetParentGroupIDs(List<int> groupIds)
        //{
        //    var parentIds = surveyDA.GetQuery().Where(g => groupIds.Contains(g.ID)).Select(g => g.ParentID).ToList();
        //    return parentIds;
        //}
    }
}
