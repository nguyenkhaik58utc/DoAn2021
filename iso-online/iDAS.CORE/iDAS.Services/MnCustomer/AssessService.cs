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
    public class AssessService
    {
        AssessDA assessDA = new AssessDA();

        public List<SurveyItem> GetSurveys(int parentId, int careId)
        {
            var dbo = assessDA.SystemContext;
            var surveys = dbo.MnCustomerSurveys
                       .Where(item => item.ParentID == parentId)
                       .Select(item => new SurveyItem()
                       {
                           ID = item.ID,
                           ParentID = item.ParentID,
                           Title = item.Title,
                           Point = item.Point,
                           IsActive = item.IsActive,
                           IsSubject = item.IsSubject,
                           IsSelected = item.MnCustomerAssesses.Count(i => i.CareID == careId) > 0,
                       }).ToList();
            return surveys;
        }

        public List<AssessItem> GetAll(int careId)
        {
            var assesses = assessDA.GetQuery(filter: s => s.CareID == careId)
                        .Select(s => new AssessItem()
                        {
                            ID = s.ID,
                            CareID = s.CareID,
                            SurveyID = s.SurveyID,
                            CreateAt = s.CreateAt,
                            CreateBy = s.CreateBy,
                        }).ToList();
            return assesses;
        }

        public AssessItem Get(int careId, int surveyId)
        {
            var assess = assessDA.GetQuery(filter: s => s.CareID == careId && s.SurveyID == surveyId)
                .Select(item=> new AssessItem()
                {
                    ID = item.ID,
                    CareID = item.CareID,
                    SurveyID = item.SurveyID,
                    CreateAt = item.CreateAt,
                    CreateBy = item.CreateBy,
                })
                .FirstOrDefault();
            return assess;
        }

        public void Insert(AssessItem item)
        {
            var assess = new MnCustomerAssess()
            {
                CareID = item.CareID,
                SurveyID = item.SurveyID,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now,
            };
            assessDA.Insert(assess);
            assessDA.Save();
        }

        public void Delete(int id)
        {
            assessDA.Delete(id);
            assessDA.Save();
        }
    }
}
