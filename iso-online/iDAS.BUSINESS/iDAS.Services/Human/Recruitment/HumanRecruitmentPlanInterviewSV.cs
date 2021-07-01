using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class HumanRecruitmentPlanInterviewSV
    {
        private HumanRecruitmentPlanInterviewDA RecruitmentPlanInterviewDA = new HumanRecruitmentPlanInterviewDA();
        /// <summary>
        /// Lấy tất cả các vòng thi tuyển 
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanInterviewItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var rpt = RecruitmentPlanInterviewDA.GetQuery()
                        .Select(item => new HumanRecruitmentPlanInterviewItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                         .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return rpt;
        }
        /// <summary>
        /// Lấy các vòng thi tuyển của kế hoạch tuyển dụng 
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanInterviewItem> GetByPlan(int page, int pageSize, out int totalCount, int? PlanID)
        {
            var task = RecruitmentPlanInterviewDA.GetQuery(p => p.HumanRecruitmentPlanID == PlanID)
                        .Select(item => new HumanRecruitmentPlanInterviewItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return task;
        }
        /// <summary>
        /// Lấy vòng thi tuyển theo ID
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentPlanInterviewItem GetById(int Id)
        {
            var rpt = RecruitmentPlanInterviewDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentPlanInterviewItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .First();
            return rpt;
        }
        /// <summary>
        /// Cập nhật vòng thi tuyển của kế hoạch
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentPlanInterviewItem item, int userID)
        {
            var planInterview = RecruitmentPlanInterviewDA.GetById(item.ID);
            planInterview.HumanRecruitmentPlanID = item.PlanID;
            planInterview.Name = item.Name;
            planInterview.StartTime = item.StartTime;
            planInterview.EndTime = item.EndTime;
            planInterview.Content = item.Content;
            planInterview.UpdateAt = DateTime.Now;
            planInterview.UpdateBy = userID;
            RecruitmentPlanInterviewDA.Save();
        }
        /// <summary>
        /// Lưu thông tin kế hoạch cho vòng thi tuyển
        /// || Author: Thanhnv || CreateDate: 27/12/2014
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PlanID"></param>
        public void InsertPlanID(int Id, int PlanID)
        {
            var planInterview = RecruitmentPlanInterviewDA.GetById(Id);
            planInterview.HumanRecruitmentPlanID = PlanID;
            RecruitmentPlanInterviewDA.Save();
        }
        /// <summary>
        /// Thêm mới vòng thi tuyển trong kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentPlanInterviewItem item, int userID)
        {
            var planInterview = new HumanRecruitmentPlanInterview()
            {
                HumanRecruitmentPlanID = item.PlanID,
                Name = item.Name,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Content = item.Content,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RecruitmentPlanInterviewDA.Insert(planInterview);
            RecruitmentPlanInterviewDA.Save();
        }
        /// <summary>
        /// Xóa vòng thi tuyển trong kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentPlanInterviewDA.Delete(id);
            RecruitmentPlanInterviewDA.Save();
        }
    }
}
