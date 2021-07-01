using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class HumanTrainingPlanDetailSV
    {
        private HumanTrainingPlanDetailDA planDetailDA = new HumanTrainingPlanDetailDA();
        public HumanTrainingPlanDetailItem GetByID(int id)
        {
            var data = planDetailDA.GetById(id);
            var obj = new HumanTrainingPlanDetailItem();
            if (data != null)
            {
                obj.ID = data.ID;
                obj.IsCommit = data.IsCommit;
                obj.Note = data.Note;
                obj.Content = data.Content;
                obj.IsCommit = data.IsCommit;
                obj.IsCancel = (bool)data.IsCancel;
                obj.ReasonCancel = data.ReasonCancel;
                obj.Address = data.Address;
                obj.Number = data.Number;
                obj.ExpectedCost = data.ExpectedCost;
                obj.FromDate = data.FromDate;
                obj.CommitContent = data.CommitContent;
                obj.TrainingTypeID = data.Type;
                obj.ToDate = data.ToDate;
                obj.Reason = data.Reason;
                obj.PlanID = data.HumanTrainingPlanID;


            }
            return obj;

        }
        public void Insert(HumanTrainingPlanDetailItem objNew, int userId)
        {
            var obj = new HumanTrainingPlanDetail();
            obj.HumanTrainingPlanID = objNew.PlanID;
            obj.CommitContent = objNew.CommitContent;
            obj.Content = objNew.Content;
            obj.ExpectedCost = objNew.ExpectedCost;
            obj.FromDate = objNew.FromDate;
            obj.ToDate = objNew.ToDate;
            obj.Address = objNew.Address;
            obj.IsCancel = false;
            obj.IsCommit = objNew.IsCommit != null ? objNew.IsCommit : false;
            obj.Note = objNew.Note;
            obj.Number = objNew.Number;
            obj.Reason = objNew.Reason;
            obj.Type = objNew.TrainingTypeID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            planDetailDA.Insert(obj);
            planDetailDA.Save();
        }
        public void UpdateCancel(HumanTrainingPlanDetailItem objEdit, int userId)
        {
            var obj = planDetailDA.GetById(objEdit.ID);
            obj.IsCancel = objEdit.IsCancel;
            obj.ReasonCancel = objEdit.ReasonCancel;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            planDetailDA.Update(obj);
            planDetailDA.Save();
        }
        public void Update(HumanTrainingPlanDetailItem objEdit, int userId)
        {
            var obj = planDetailDA.GetById(objEdit.ID);
            obj.HumanTrainingPlanID = objEdit.PlanID;
            obj.CommitContent = objEdit.CommitContent;
            obj.Content = objEdit.Content;
            obj.ExpectedCost = objEdit.ExpectedCost;
            obj.FromDate = objEdit.FromDate;
            obj.IsCancel = false;
            obj.Address = objEdit.Address;
            obj.ToDate = objEdit.ToDate;
            obj.IsCommit = objEdit.IsCommit!=null?objEdit.IsCommit:false;
            obj.Note = objEdit.Note;
            obj.Number = objEdit.Number;
            obj.Reason = objEdit.Reason;
            obj.Type = objEdit.TrainingTypeID;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            planDetailDA.Update(obj);
            planDetailDA.Save();
        }
        public void Delete(int id)
        {
            var obj = planDetailDA.GetById(id);
            planDetailDA.Delete(obj);
            planDetailDA.Save();
        }
        public List<HumanTrainingPlanDetailItem> GetAll(int page, int pageSize, out int total, int planId)
        {
            List<HumanTrainingPlanDetailItem> lst = new List<HumanTrainingPlanDetailItem>();
            var data = planDetailDA.GetQuery(t => t.HumanTrainingPlanID == planId).OrderBy(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new HumanTrainingPlanDetailItem
                    {
                        ID = item.ID,
                        Content = item.Content,
                        Number = item.Number,
                        CommitContent = item.CommitContent,
                        ExpectedCost = item.ExpectedCost,
                        FromDate = item.FromDate,
                        IsCommit = item.IsCommit,
                        Note = item.Note,
                        IsCancel = (bool)item.IsCancel,
                        PlanID = item.HumanTrainingPlanID,
                        Reason = item.Reason,
                        ToDate = item.ToDate,
                        TrainingTypeID = item.Type,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy
                    });
                }
            }
            return lst;
        }
          public List<HumanTrainingPlanDetailItem> GetAllForResultTraining(int page, int pageSize, out int total, int planId)
        {
            List<HumanTrainingPlanDetailItem> lst = new List<HumanTrainingPlanDetailItem>();
            var data = planDetailDA.GetQuery(t => t.HumanTrainingPlanID == planId && !(bool)t.IsCancel).OrderBy(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new HumanTrainingPlanDetailItem
                    {
                        ID = item.ID,
                        Content = item.Content,
                        Number = item.Number,
                        CommitContent = item.CommitContent,
                        ExpectedCost = item.ExpectedCost,
                        FromDate = item.FromDate,
                        IsCommit = item.IsCommit,
                        Note = item.Note,
                        IsCancel = (bool)item.IsCancel,
                        PlanID = item.HumanTrainingPlanID,
                        Reason = item.Reason,
                        ToDate = item.ToDate,
                        TrainingTypeID = item.Type,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy
                    });
                }
            }
            return lst;
        }

          public List<HumanTrainingPlanDetailItem> GetAllStatistiacl(int page, int pageSize, out int total, int planId)
          {
              List<HumanTrainingPlanDetailItem> lst = new List<HumanTrainingPlanDetailItem>();
              var data = planDetailDA.GetQuery(t => t.HumanTrainingPlanID == planId).OrderBy(t => t.CreateAt).Page(page, pageSize, out total).ToList();
              if (data != null && data.Count > 0)
              {
                  foreach (var item in data)
                  {
                      lst.Add(new HumanTrainingPlanDetailItem
                      {
                          ID = item.ID,
                          Content = item.Content,
                          Number = item.Number,
                          CommitContent = item.CommitContent,
                          ExpectedCost = item.ExpectedCost,
                          FromDate = item.FromDate,
                          IsCommit = item.IsCommit,
                          Note = item.Note,
                          IsCancel = (bool)item.IsCancel,
                          PlanID = item.HumanTrainingPlanID,
                          Reason = item.Reason,
                          ToDate = item.ToDate,
                          TrainingTypeID = item.Type,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          NumberRegister = item.HumanTrainingPractioners.Count(),
                          NumberReal = item.HumanTrainingPractioners.Count(x => x.IsJoin),
                          ToTalGood = item.HumanTrainingPractioners.Count(x=>x.Rank==1),
                          TotalCreat = item.HumanTrainingPractioners.Count(x=>x.Rank==2),
                          TotalAvg = item.HumanTrainingPractioners.Count(x=>x.Rank==3),
                          TotalBad = item.HumanTrainingPractioners.Count(x=>x.Rank==4),
                          TotalBest = item.HumanTrainingPractioners.Count(x=>x.Rank==5)
                      });
                  }
              }
              return lst;
          }
    }

}
