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
    public class HumanRecruitmentPlanTaskSV
    {
        private HumanRecruitmentPlanTaskDA RecruitmentPlanTaskDA = new HumanRecruitmentPlanTaskDA();
        /// <summary>
        /// Lấy tất cả các giai đoạn tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanTaskItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var rpt = RecruitmentPlanTaskDA.GetQuery()
                        .Select(item => new HumanRecruitmentPlanTaskItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            TaskID = item.TaskID,
                            Cost = item.Cost,
                            //IsInterview = item.IsInterview,
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
        /// Lấy các công việc của kế hoạch tuyển dụng theo mã kế hoạch
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanRecruitmentPlanTaskItem> GetByPlanID(int page, int pageSize, out int totalCount, int? PlanID)
        {
            var task = RecruitmentPlanTaskDA.GetQuery(p => p.HumanRecruitmentPlanID == PlanID)
                        .Select(item => new HumanRecruitmentPlanTaskItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            TaskID = item.TaskID,
                            Cost = item.Cost,
                            //IsInterview = item.IsInterview,
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
        /// Phân loại công việc là phỏng vấn hay không của kế hoạch tuyển dụng theo mã kế hoạch
        /// || Author: Thanhnv || CreateDate: 27/12/2014  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        /// 
        public List<HumanRecruitmentPlanTaskItem> GetTreeTask(int taskId, int? PlanID)
        {
            var dbo = RecruitmentPlanTaskDA.Repository;
            var result = new List<HumanRecruitmentPlanTaskItem>();
            if (taskId == 0)
            {
                result = dbo.Tasks.Join(dbo.HumanRecruitmentTasks, t => t.ID, dt => dt.TaskID, (t, dt) => new
               HumanRecruitmentPlanTaskItem
                {
                    PlanID = dt.HumanRecruitmentPlanID,
                    Cost = dt.Cost,
                    ID = t.ID,
                    Name = t.Name,
                    ParentID = t.ParentID,
                    Content = t.Content,
                    IsComplete = t.IsComplete,
                    CategoryName = t.TaskCategory.Name,
                    IsEdit = t.IsEdit,
                    IsNew = t.IsNew,
                    IsPrivate = t.IsPrivate,
                    Rate = t.Rate,
                    CreateBy = t.CreateBy,
                    LevelColor = t.TaskLevel.Color,
                    StartTime = t.StartTime,
                    UpdateAt = t.UpdateAt,
                    UpdateBy = t.UpdateBy,
                    CreateAt = t.CreateAt,
                    LevelID = t.TaskLevelID,
                    EndTime = t.EndTime,
                    IsPause = t.IsPause,
                    IsPass = t.IsPass,
                    LevelName = t.TaskLevel.Name,
                    Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null
                })
                .Where(p => p.PlanID == PlanID)
                .OrderByDescending(p=>p.CreateAt)
                .Distinct()
                .ToList();
                var lis = result.Select(t => t.ID).ToArray();
                List<HumanRecruitmentPlanTaskItem> taskremove = new List<HumanRecruitmentPlanTaskItem>();
                foreach (var item in result)
                {
                    if (!lis.Contains(item.ParentID))
                    {
                        taskremove.Add(item);
                    }
                }
                result = taskremove.OrderByDescending(t=>t.CreateAt.Value).ToList();
            }
            else
            {
                result = dbo.Tasks.Where(t => t.ParentID == taskId).Join(dbo.HumanRecruitmentTasks, t => t.ID, dt => dt.TaskID, (t, dt) => new
              HumanRecruitmentPlanTaskItem
                {
                    PlanID = dt.HumanRecruitmentPlanID,
                    Cost = dt.Cost,
                    ID = t.ID,
                    Name = t.Name,
                    Content = t.Content,
                    ParentID= t.ParentID,
                    IsComplete = t.IsComplete,
                    CategoryName = t.TaskCategory.Name,
                    IsEdit = t.IsEdit,
                    IsNew = t.IsNew,
                    IsPrivate = t.IsPrivate,
                    Rate = t.Rate,
                    CreateBy = t.CreateBy,
                    LevelColor = t.TaskLevel.Color,
                    StartTime = t.StartTime,
                    UpdateAt = t.UpdateAt,
                    UpdateBy = t.UpdateBy,
                    CreateAt = t.CreateAt,
                    LevelID = t.TaskLevelID,
                    EndTime = t.EndTime,
                    IsPause = t.IsPause,
                    IsPass = t.IsPass,
                    LevelName = t.TaskLevel.Name,
                    Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null
                })
           .Distinct()
           .OrderByDescending(p => p.CreateAt)
           .ToList();
            }
            return result;
        }
        public List<HumanRecruitmentPlanTaskItem> GetTaskByPlanID(int page, int pageSize, out int totalCount, int? PlanID)
        {
            var dbo = RecruitmentPlanTaskDA.Repository;
            var task = dbo.Tasks.Join(dbo.HumanRecruitmentTasks, t => t.ID, dt => dt.TaskID, (t, dt) => new
                HumanRecruitmentPlanTaskItem
                {
                    //ID = dt.ID,
                    PlanID = dt.HumanRecruitmentPlanID,
                    Cost = dt.Cost,
                    //IsInterview = dt.IsInterview,
                    ID = t.ID,
                    Name = t.Name,
                    Content = t.Content,
                    IsComplete = t.IsComplete,
                    CategoryName = t.TaskCategory.Name,
                    IsEdit = t.IsEdit,
                    IsNew = t.IsNew,
                    IsPrivate = t.IsPrivate,
                    Rate = t.Rate,
                    CreateBy = t.CreateBy,
                    LevelColor = t.TaskLevel.Color,
                    //IsFinish = t.IsFinish,
                    StartTime = t.StartTime,
                    UpdateAt = t.UpdateAt,
                    UpdateBy = t.UpdateBy,
                    // ChangeName = t.ChangeBy.HasValue ? dbo.HumanEmployee.FirstOrDefault(e => e.ID == t.ChangeBy).Name : null,
                    //ApprovalName = t.ApprovalBy.HasValue ? dbo.HumanEmployee.FirstOrDefault(e => e.ID == t.ApprovalBy).Name : null,
                    //CheckName = t.CheckBy.HasValue ? dbo.HumanEmployee.FirstOrDefault(e => e.ID == t.CheckBy).Name : null,
                    // AssessName = t.AuditBy.HasValue ? dbo.HumanEmployee.FirstOrDefault(e => e.ID == t.AuditBy).Name : null,
                    // FinishByName = t.FinishBy.HasValue ? dbo.HumanEmployee.FirstOrDefault(e => e.ID == t.FinishBy).Name : null,
                    CreateAt = t.CreateAt,
                    //CreateByName = t.CreateBy != null ? dbo.HumanUser.FirstOrDefault(u => u.ID == t.CreateBy).HumanEmployee.Name : null,
                    LevelID = t.TaskLevelID,
                    EndTime = t.EndTime,
                    IsPause = t.IsPause,
                    IsPass = t.IsPass,
                    LevelName = t.TaskLevel.Name,
                    //PerformBy = t.PerformBy,
                    //PerformEmployeesName = t.PerformBy != null ? dbo.HumanEmployee.FirstOrDefault(u => u.ID == t.PerformBy).Name : null,
                    //Content = t.Content,

                })
            .Where(p => p.PlanID == PlanID)
            .OrderByDescending(t => t.Name).Page(page, pageSize, out totalCount)
            .ToList();
            return task;
        }
        /// <summary>
        /// Lấy công việc kế hoạch theo mã công việc công việc
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanRecruitmentPlanTaskItem GetById(int Id)
        {
            var rpt = RecruitmentPlanTaskDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanRecruitmentPlanTaskItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            TaskID = item.TaskID,
                            Cost = item.Cost,
                            //IsInterview = item.IsInterview,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .First();
            return rpt;
        }
        /// <summary>
        /// Lấy công việc của kế hoạch theo mã công việc
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public HumanRecruitmentPlanTaskItem GetByTaskId(int TaskId)
        {
            var rpt = RecruitmentPlanTaskDA.GetQuery(i => i.TaskID == TaskId)
                        .Select(item => new HumanRecruitmentPlanTaskItem()
                        {
                            ID = item.ID,
                            PlanID = item.HumanRecruitmentPlanID,
                            TaskID = item.TaskID,
                            Cost = item.Cost,
                            //IsInterview = item.IsInterview,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .FirstOrDefault();
            return rpt;
        }
        /// <summary>
        /// Cập nhật công việc của kế hoạch
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanRecruitmentPlanTaskItem item, int userID)
        {
            var dbo = RecruitmentPlanTaskDA.Repository;
            var task = dbo.HumanRecruitmentTasks.FirstOrDefault(i => i.ID == item.ID);
            task.HumanRecruitmentPlanID = item.PlanID;
            task.TaskID = item.TaskID;
            task.Cost = item.Cost;
            //task.IsInterview = false;
            task.UpdateAt = DateTime.Now;
            task.UpdateBy = userID;
            dbo.SaveChanges();
        }
        /// <summary>
        /// Cập nhật công việc của kế hoạch
        /// || Author: Thanhnv || CreateDate: 23/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void UpdateTask(HumanRecruitmentPlanTaskItem item, int userID)
        {
            var dbo = RecruitmentPlanTaskDA.Repository;
            var task = dbo.HumanRecruitmentTasks.FirstOrDefault(i => i.TaskID == item.TaskID);
            task.HumanRecruitmentPlanID = item.PlanID;
            task.Cost = item.Cost;
            //task.IsInterview = false;
            task.UpdateAt = DateTime.Now;
            task.UpdateBy = userID;
            dbo.SaveChanges();
        }
        /// <summary>
        /// Lưu thông tin kế hoạch cho công việc
        /// || Author: Thanhnv || CreateDate: 27/12/2014
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PlanID"></param>
        public void InsertPlanID(int Id, int PlanID)
        {
            var task = RecruitmentPlanTaskDA.GetById(Id);
            task.HumanRecruitmentPlanID = PlanID;
            RecruitmentPlanTaskDA.Save();
        }
        /// <summary>
        /// Thêm mới công việc trong kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanRecruitmentPlanTaskItem item, int userID)
        {
            var dbo = RecruitmentPlanTaskDA.Repository;
            var task = new HumanRecruitmentTask()
            {
                HumanRecruitmentPlanID = item.PlanID,
                TaskID = item.TaskID,
                Cost = item.Cost,
                //IsInterview = false,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            dbo.HumanRecruitmentTasks.Add(task);
            dbo.SaveChanges();
        }
        /// <summary>
        /// Xóa công việc trong kế hoạch tuyển dụng
        /// || Author: Thanhnv || CreateDate: 27/12/2014 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RecruitmentPlanTaskDA.Delete(id);
            RecruitmentPlanTaskDA.Save();
        }
        /// <summary>
        /// Xóa thông tin giai đoạn của nhiều bản ghi
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteRange(int[] ids)
        {
            var dbo = RecruitmentPlanTaskDA.Repository;
            var deleteItems = dbo.HumanRecruitmentTasks.Where(i => ids.Contains(i.ID));
            dbo.HumanRecruitmentTasks.RemoveRange(deleteItems);
            dbo.SaveChanges();
        }
    }
}
