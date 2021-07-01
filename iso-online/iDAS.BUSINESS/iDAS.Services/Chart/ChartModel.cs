using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.DA;
using System.Globalization;
using iDAS.Items;

namespace iDAS.Services
{
    public class ChartModel
    {
        public string Name
        {
            get;
            set;
        }
        public double Data1
        {
            get;
            set;
        }
        public double Data2
        {
            get;
            set;
        }
        public double Data3
        {
            get;
            set;
        }
        public double Data4
        {
            get;
            set;
        }
        public double Data5
        {
            get;
            set;
        }
        public double Data6
        {
            get;
            set;
        }
        public double Data7
        {
            get;
            set;
        }
        public double Data8
        {
            get;
            set;
        }
        public double Data9
        {
            get;
            set;
        }
        #region Thống kê cho module quản lý công việc
        #region Thống kê công việc Dashboard
        public class TotalTaskInfo
        {
            public string TimeFix { get; set; }//thời gian tổng hợp dữ liệu
            public int TotalValue { get; set; }//Tổng công việc
            public int DoingValue { get; set; }//Tổng công việc đang thực hiện
            public int FinishValue { get; set; }//Tổng công việc đã hoàn thành
            public int PauseValue { get; set; }//Tổng công việc tạm dừng
            public int OverTimeValue { get; set; }//Tổng công việc quá hạn
            public int FinishOverTimeValue { get; set; }//tổng công việc hoàn thành quá hạn
        }
        public static List<TotalTaskInfo> GetRevenuaForYear(int year, int employeeId)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            List<TotalTaskInfo> lst = new List<TotalTaskInfo>();
            for (int i = 1; i <= 12; i++)
            {
                lst.Add(new TotalTaskInfo
                {
                    TimeFix = "Tháng " + i,
                    TotalValue = dbo.Tasks
                                    .Where(t => t.HumanEmployeeID == employeeId)
                                    .Where(t => !t.IsNew)
                                    .Where(t => ((t.StartTime.Year < year || t.StartTime.Month <= i) && t.EndTime.Year == year) && (t.EndTime.Month >= i && t.EndTime.Year == year))
                                    .Count(),
                    DoingValue = dbo.Tasks
                                    .Where(t => t.HumanEmployeeID == employeeId)
                                    .Where(t => !t.IsNew)
                                    .Where(t => ((t.StartTime.Year < year || t.StartTime.Month <= i) && t.EndTime.Year == year) && (t.EndTime.Month >= i && t.EndTime.Year == year))
                                    .Where(t => !t.IsComplete && !t.IsPause)
                                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                                    .Count(),
                    FinishValue = dbo.Tasks
                                    .Where(t => t.HumanEmployeeID == employeeId)
                                    .Where(t => !t.IsNew)
                                    .Where(t => ((t.StartTime.Year < year || t.StartTime.Month <= i) && t.EndTime.Year == year) && (t.EndTime.Month >= i && t.EndTime.Year == year))
                                    .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                                    .Where(t => t.EndTime >= t.CompleteTime)
                                    .Count(),
                    OverTimeValue = dbo.Tasks
                                    .Where(t => t.HumanEmployeeID == employeeId)
                                    .Where(t => !t.IsNew)
                                    .Where(t => ((t.StartTime.Year < year || t.StartTime.Month <= i) && t.EndTime.Year == year) && (t.EndTime.Month >= i && t.EndTime.Year == year))
                                    .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                                    .Where(t => t.EndTime <= DateTime.Now)
                                    .Count(),
                    PauseValue = dbo.Tasks
                                    .Where(t => t.HumanEmployeeID == employeeId)
                                    .Where(t => !t.IsNew)
                                    .Where(t => ((t.StartTime.Year < year || t.StartTime.Month <= i) && t.EndTime.Year == year) && (t.EndTime.Month >= i && t.EndTime.Year == year))
                                    .Where(t => t.IsPause)
                                    .Count(),
                    FinishOverTimeValue = dbo.Tasks
                                    .Where(t => t.HumanEmployeeID == employeeId)
                                    .Where(t => !t.IsNew)
                                    .Where(t => ((t.StartTime.Year < year || t.StartTime.Month <= i) && t.EndTime.Year == year) && (t.EndTime.Month >= i && t.EndTime.Year == year))
                                    .Where(t => t.IsComplete && t.Rate == 100)
                                    .Where(t => t.EndTime < t.CompleteTime)
                                    .Count()
                });
            }
            return lst;
        }
        #endregion
        #region Thống kê công việc theo nhóm công việc
        public class CateStatisticalInfo
        {
            public int CateID { get; set; }
            public string CateName { get; set; }
            public System.DateTime? CreateAt { get; set; }
            public int TotalTask { get; set; }//Tổng công việc
            public int TotalDoing { get; set; }//Tổng công việc đang thực hiện
            public int TotalFinish { get; set; }//Tổng công việc đã hoàn thành
            public int TotalPause { get; set; }//Tổng công việc tạm dừng
            public int TotalOverTime { get; set; }//Tổng công việc quá hạn
            public int TotalFinishOverTime { get; set; }//tổng công việc hoàn thành quá hạn
            public int TotalAuditNot { get; set; }//Tổng công việc chưa đánh giá
            public int TotalAuditGood { get; set; }//Tổng công việc đánh giá đạt
            public int TotalAuditFail { get; set; }//tổng công việc đánh giá không đạt
        }
        public static List<CateStatisticalInfo> GetCateStatisticalTask(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            DateTime dateTotal = toDate.AddDays(1);
            var dbo = taskDA.Repository;
            var results = dbo.TaskCategories
                        .Where(i => i.IsActive && !i.IsDelete)
                        .Select(item => new CateStatisticalInfo()
                        {
                            CateID = item.ID,
                            CateName = item.Name,
                            CreateAt = item.CreateAt
                        })
                        .Distinct()
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            foreach (var cate in results)
            {
                cate.TotalTask = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Count();
                cate.TotalDoing = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                    .Count();
                cate.TotalFinish = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                    .Where(t => t.EndTime >= t.CompleteTime)
                    .Count();
                cate.TotalOverTime = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                    .Where(t => t.EndTime <= DateTime.Now)
                    .Count();
                cate.TotalPause = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                    .Count();
                cate.TotalFinishOverTime = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                    .Count();
                cate.TotalAuditNot = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    .Count();
                cate.TotalAuditGood = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => t.Audit.IsPass)
                    .Count();
                cate.TotalAuditFail = dbo.Tasks
                    .Where(t => t.TaskCategoryID == cate.CateID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => !t.Audit.IsPass)
                    .Count();

            }
            return results;
        }
        public static CateStatisticalInfo GetByCateID(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var results = dbo.TaskCategories
                .Where(i => i.ID == id)
                .Where(i => i.IsActive && !i.IsDelete)
                .Select(item => new CateStatisticalInfo()
                {
                    CateID = item.ID,
                    CateName = item.Name,
                    CreateAt = item.CreateAt
                })
                .FirstOrDefault();
            if (results != null)
            {
                results.TotalTask = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Count();
                results.TotalDoing = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                    .Count();
                results.TotalFinish = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                    .Where(t => t.EndTime >= t.CompleteTime)
                    .Count();
                results.TotalOverTime = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                    .Where(t => t.EndTime <= DateTime.Now)
                    .Count();
                results.TotalPause = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                    .Count();
                results.TotalFinishOverTime = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                    .Count();
                results.TotalAuditNot = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    .Count();
                results.TotalAuditGood = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => t.Audit.IsPass)
                    .Count();
                results.TotalAuditFail = dbo.Tasks
                    .Where(t => t.TaskCategoryID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => !t.Audit.IsPass)
                    .Count();
            }
            return results;
        }
        public static List<ChartModel> GenerateDataCate(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByCateID(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành(" + Math.Round((obj.TotalFinish != 0 ? Math.Floor(Math.Max((double)obj.TotalFinish * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalFinish != 0 ? Math.Floor(Math.Max((double)obj.TotalFinish * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Đang thực hiện(" + Math.Round((obj.TotalDoing != 0 ? Math.Floor(Math.Max((double)obj.TotalDoing * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalDoing != 0 ? Math.Floor(Math.Max((double)obj.TotalDoing * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành quá hạn(" + Math.Round((obj.TotalFinishOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalFinishOverTime * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalFinishOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalFinishOverTime * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Quá hạn(" + Math.Round((obj.TotalOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalOverTime * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalOverTime * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Tạm dừng(" + Math.Round((obj.TotalPause != 0 ? Math.Floor(Math.Max((double)obj.TotalPause * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalPause != 0 ? Math.Floor(Math.Max((double)obj.TotalPause * 100, obj.TotalTask)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataCateColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByCateID(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành",
                    Data1 = obj.TotalFinish
                });
                data.Add(new ChartModel()
                {
                    Name = "Đang thực hiện",
                    Data1 = obj.TotalDoing
                });
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành quá hạn",
                    Data1 = obj.TotalFinishOverTime
                });
                data.Add(new ChartModel()
                {
                    Name = "Quá hạn",
                    Data1 = obj.TotalOverTime
                });
                data.Add(new ChartModel()
                {
                    Name = "Tạm dừng",
                    Data1 = obj.TotalPause
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataAuditCate(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByCateID(id, fromDate, toDate);
            if (obj != null)
            {
                int floor = obj.TotalAuditGood + obj.TotalAuditFail;
                data.Add(new ChartModel()
                {
                    Name = "Đạt(" + Math.Round((obj.TotalAuditGood != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditGood * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalAuditGood != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditGood * 100, floor)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Không đạt(" + Math.Round((obj.TotalAuditFail != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditFail * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalAuditFail != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditFail * 100, floor)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataAuditCateColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByCateID(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Đạt",
                    Data1 = obj.TotalAuditGood
                });
                data.Add(new ChartModel()
                {
                    Name = "Không đạt",
                    Data1 = obj.TotalAuditFail
                });
            }
            return data;
        }
        #endregion
        #region Thống kê công việc theo phòng ban
        public class DepartmentStatisticalInfo
        {
            public int DepartmentID { get; set; }//Tổng công việc
            public string DepartmentName { get; set; }
            public bool Leaf { get; set; }
            public int ParentID { get; set; }
            public int TotalTask { get; set; }//Tổng công việc
            public int TotalDoing { get; set; }//Tổng công việc đang thực hiện
            public int TotalFinish { get; set; }//Tổng công việc đã hoàn thành
            public int TotalPause { get; set; }//Tổng công việc tạm dừng
            public int TotalOverTime { get; set; }//Tổng công việc quá hạn
            public int TotalFinishOverTime { get; set; }//tổng công việc hoàn thành quá hạn
            public int TotalAuditNot { get; set; }//Tổng công việc chưa đánh giá
            public int TotalAuditGood { get; set; }//Tổng công việc đánh giá đạt
            public int TotalAuditFail { get; set; }//tổng công việc đánh giá không đạt
        }
        public List<DepartmentStatisticalInfo> GetDepartmentStatisticalTask(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var results = dbo.HumanDepartments
                        .Where(i => (i.ParentID != null && i.ParentID == id))
                        .Where(i => i.IsActive == true || i.IsCancel || i.IsMerge || i.IsDelete)
                        .Where(i => i.IsCancel == false)
                        .Where(i => i.IsMerge == false)
                        .Where(i => i.IsDelete == false)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(item => new DepartmentStatisticalInfo()
                        {
                            DepartmentID = item.ID,
                            ParentID = item.ParentID,
                            DepartmentName = item.Name,
                            Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                        }).ToList();
            foreach (var department in results)
            {
                if (department.ParentID != 0)
                {
                    department.TotalTask = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Count();
                    department.TotalDoing = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && !t.IsPause)
                        .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                        .Count();
                    department.TotalFinish = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                        .Where(t => t.EndTime >= t.CompleteTime)
                        .Count();
                    department.TotalOverTime = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                        .Count();
                    department.TotalPause = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsPause)
                        .Count();
                    department.TotalFinishOverTime = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100)
                        .Where(t => t.EndTime < t.CompleteTime)
                        .Count();
                    department.TotalAuditNot = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.AuditID.HasValue)
                       .Count();
                    department.TotalAuditGood = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => t.Audit.IsPass)
                        .Count();
                    department.TotalAuditFail = dbo.Tasks
                        .Where(t => t.HumanDepartmentID == department.DepartmentID)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => !t.Audit.IsPass)
                        .Count();
                }
                else
                {
                    department.TotalTask = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Count();
                    department.TotalDoing = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && !t.IsPause)
                        .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                        .Count();
                    department.TotalFinish = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                        .Where(t => t.EndTime >= t.CompleteTime)
                        .Count();
                    department.TotalOverTime = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                        .Count();
                    department.TotalPause = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsPause)
                        .Count();
                    department.TotalFinishOverTime = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100)
                        .Where(t => t.EndTime < t.CompleteTime)
                        .Count();
                    department.TotalAuditNot = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.AuditID.HasValue)
                        .Count();
                    department.TotalAuditGood = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => t.Audit.IsPass)
                        .Count();
                    department.TotalAuditFail = dbo.Tasks
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => !t.Audit.IsPass)
                        .Count();
                }
            }
            return results;
        }
        public static DepartmentStatisticalInfo GetByDepartmnet(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var results = dbo.HumanDepartments
                        .Where(i => i.ID == id)
                        .Select(item => new DepartmentStatisticalInfo()
                        {
                            DepartmentID = item.ID,
                            ParentID = item.ParentID,
                            DepartmentName = item.Name,
                            Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                        }).FirstOrDefault();
            if (results.ParentID != 0)
            {
                results.TotalTask = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Count();
                results.TotalDoing = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                    .Count();
                results.TotalFinish = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                    .Where(t => t.EndTime >= t.CompleteTime)
                    .Count();
                results.TotalOverTime = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                    .Where(t => t.EndTime <= DateTime.Now)
                    .Count();
                results.TotalPause = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                    .Count();
                results.TotalFinishOverTime = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                    .Count();
                results.TotalAuditNot = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    .Count();
                results.TotalAuditGood = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => t.Audit.IsPass)
                    .Count();
                results.TotalAuditFail = dbo.Tasks
                    .Where(t => t.HumanDepartmentID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => !t.Audit.IsPass)
                    .Count();
            }
            else
            {
                results.TotalTask = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Count();
                results.TotalDoing = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                    .Count();
                results.TotalFinish = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                    .Where(t => t.EndTime >= t.CompleteTime)
                    .Count();
                results.TotalOverTime = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                    .Where(t => t.EndTime <= DateTime.Now)
                    .Count();
                results.TotalPause = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                    .Count();
                results.TotalFinishOverTime = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                    .Count();
                results.TotalAuditNot = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    .Count();
                results.TotalAuditGood = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => t.Audit.IsPass)
                    .Count();
                results.TotalAuditFail = dbo.Tasks
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => !t.Audit.IsPass)
                    .Count();
            }
            return results;
        }
        public static List<ChartModel> GenerateDataColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByDepartmnet(id, fromDate, toDate);
            data.Add(new ChartModel()
            {
                Name = "Hoàn thành",
                Data1 = obj.TotalFinish
            });
            data.Add(new ChartModel()
            {
                Name = "Đang thực hiện",
                Data1 = obj.TotalDoing
            });
            data.Add(new ChartModel()
            {
                Name = "Hoàn thành quá hạn",
                Data1 = obj.TotalFinishOverTime
            });
            data.Add(new ChartModel()
            {
                Name = "Quá hạn",
                Data1 = obj.TotalOverTime
            });
            data.Add(new ChartModel()
            {
                Name = "Tạm dừng",
                Data1 = obj.TotalPause
            });
            return data;
        }
        public static List<ChartModel> GenerateData(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByDepartmnet(id, fromDate, toDate);
            data.Add(new ChartModel()
            {
                Name = "Hoàn thành(" + Math.Round((obj.TotalFinish != 0 ? Math.Floor(Math.Max((double)obj.TotalFinish * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                Data1 = obj.TotalFinish != 0 ? Math.Floor(Math.Max((double)obj.TotalFinish * 100, obj.TotalTask)) : 0
            });
            data.Add(new ChartModel()
            {
                Name = "Đang thực hiện(" + Math.Round((obj.TotalDoing != 0 ? Math.Floor(Math.Max((double)obj.TotalDoing * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                Data1 = obj.TotalDoing != 0 ? Math.Floor(Math.Max((double)obj.TotalDoing * 100, obj.TotalTask)) : 0
            });
            data.Add(new ChartModel()
            {
                Name = "Hoàn thành quá hạn(" + Math.Round((obj.TotalFinishOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalFinishOverTime * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                Data1 = obj.TotalFinishOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalFinishOverTime * 100, obj.TotalTask)) : 0
            });
            data.Add(new ChartModel()
            {
                Name = "Quá hạn(" + Math.Round((obj.TotalOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalOverTime * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                Data1 = obj.TotalOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalOverTime * 100, obj.TotalTask)) : 0
            });
            data.Add(new ChartModel()
            {
                Name = "Tạm dừng(" + Math.Round((obj.TotalPause != 0 ? Math.Floor(Math.Max((double)obj.TotalPause * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                Data1 = obj.TotalPause != 0 ? Math.Floor(Math.Max((double)obj.TotalPause * 100, obj.TotalTask)) : 0
            });
            return data;
        }
        public static List<ChartModel> GenerateDataAuditColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByDepartmnet(id, fromDate, toDate);
            data.Add(new ChartModel()
            {
                Name = "Đạt",
                Data1 = obj.TotalAuditGood
            });
            data.Add(new ChartModel()
            {
                Name = "Không đạt",
                Data1 = obj.TotalAuditFail
            });
            return data;
        }
        public static List<ChartModel> GenerateAuditData(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByDepartmnet(id, fromDate, toDate);
            int floor = obj.TotalAuditFail + obj.TotalAuditGood;
            data.Add(new ChartModel()
            {
                Name = "Đạt(" + Math.Round((obj.TotalAuditGood != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditGood * 100, floor)) : 0) / floor) + "%)",
                Data1 = obj.TotalAuditGood != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditGood * 100, floor)) : 0
            });
            data.Add(new ChartModel()
            {
                Name = "Không đạt(" + Math.Round((obj.TotalAuditFail != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditFail * 100, floor)) : 0) / floor) + "%)",
                Data1 = obj.TotalAuditFail != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditFail * 100, floor)) : 0
            });
            return data;
        }
        #endregion
        #region Thống kê công việc theo cá nhân
        public class EmployeeStatisticalInfo
        {
            public string _AvatarUrl = "/Generic/File/LoadAvatarFile?employeeId={0}";
            public string _AvatarUrlDefault = "/Content/images/underfind.jpg";
            public int EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public System.DateTime? CreateAt { get; set; }
            public string Avatar
            {
                get
                {
                    if (EmployeeID == 0)
                        return _AvatarUrlDefault;
                    return string.Format(_AvatarUrl, EmployeeID);
                }
                set
                {
                    _AvatarUrl = value;
                }
            }
            public int TotalTask { get; set; }//Tổng công việc
            public int TotalDoing { get; set; }//Tổng công việc đang thực hiện
            public int TotalFinish { get; set; }//Tổng công việc đã hoàn thành
            public int TotalPause { get; set; }//Tổng công việc tạm dừng
            public int TotalOverTime { get; set; }//Tổng công việc quá hạn
            public int TotalFinishOverTime { get; set; }//tổng công việc hoàn thành quá hạn
            public int TotalAuditNot { get; set; }//Tổng công việc chưa đánh giá
            public int TotalAuditGood { get; set; }//Tổng công việc đánh giá đạt
            public int TotalAuditFail { get; set; }//tổng công việc đánh giá không đạt
        }
        public static List<EmployeeStatisticalInfo> GetEmployeeStatisticalTask(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            DateTime dateTotal = toDate.AddDays(1);
            var dbo = taskDA.Repository;
            var results = dbo.HumanRoles.Where(i => i.HumanDepartmentID == departmentId)
                    .SelectMany(i => i.HumanOrganizations)
                    .Select(i => i.HumanEmployee).Distinct()
                    .Select(item => new EmployeeStatisticalInfo()
                    {
                        EmployeeID = item.ID,
                        EmployeeName = item.Name,
                        CreateAt = item.CreateAt
                    })
                    .Distinct()
                    .OrderByDescending(item => item.CreateAt)
                    .Filter(filter)
                    .ToList();
            foreach (var employee in results)
            {
                employee.TotalTask = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Count();
                employee.TotalDoing = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                    .Count();
                employee.TotalFinish = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                    .Where(t => t.EndTime >= t.CompleteTime)
                    .Count();
                employee.TotalOverTime = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                    .Where(t => t.EndTime <= DateTime.Now)
                    .Count();
                employee.TotalPause = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                    .Count();
                employee.TotalFinishOverTime = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                    .Count();
                employee.TotalAuditNot = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    .Count();
                employee.TotalAuditGood = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                     .Where(t => t.Audit.IsPass)
                    .Count();
                employee.TotalAuditFail = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == employee.EmployeeID)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => !t.Audit.IsPass)
                    .Count();
            }
            return results;
        }
        public static EmployeeStatisticalInfo GetByEmployee(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var results = dbo.HumanEmployees
                        .Where(i => i.ID == id)
                        .Select(item => new EmployeeStatisticalInfo()
                        {
                            EmployeeID = item.ID,
                            EmployeeName = item.Name,
                            CreateAt = item.CreateAt
                        }).FirstOrDefault();
            if (results != null)
            {
                results.TotalTask = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Count();
                results.TotalDoing = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                    .Count();
                results.TotalFinish = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                    .Where(t => t.EndTime >= t.CompleteTime)
                    .Count();
                results.TotalOverTime = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                    .Where(t => t.EndTime <= DateTime.Now)
                    .Count();
                results.TotalPause = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                    .Count();
                results.TotalFinishOverTime = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                    .Count();
                results.TotalAuditNot = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    .Count();
                results.TotalAuditGood = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => t.Audit.IsPass)
                    .Count();
                results.TotalAuditFail = dbo.Tasks
                    .Where(t => t.HumanEmployeeID == id)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                    .Where(t => !t.Audit.IsPass)
                    .Count();
            }
            return results;
        }
        public static List<ChartModel> GenerateDataEmployee(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByEmployee(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành(" + Math.Round((obj.TotalFinish != 0 ? Math.Floor(Math.Max((double)obj.TotalFinish * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalFinish != 0 ? Math.Floor(Math.Max((double)obj.TotalFinish * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Đang thực hiện(" + Math.Round((obj.TotalDoing != 0 ? Math.Floor(Math.Max((double)obj.TotalDoing * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalDoing != 0 ? Math.Floor(Math.Max((double)obj.TotalDoing * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành quá hạn(" + Math.Round((obj.TotalFinishOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalFinishOverTime * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalFinishOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalFinishOverTime * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Quá hạn(" + Math.Round((obj.TotalOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalOverTime * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalOverTime * 100, obj.TotalTask)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Tạm dừng(" + Math.Round((obj.TotalPause != 0 ? Math.Floor(Math.Max((double)obj.TotalPause * 100, obj.TotalTask)) : 0) / obj.TotalTask) + "%)",
                    Data1 = obj.TotalPause != 0 ? Math.Floor(Math.Max((double)obj.TotalPause * 100, obj.TotalTask)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataEmployeeColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByEmployee(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành",
                    Data1 = obj.TotalFinish
                });
                data.Add(new ChartModel()
                {
                    Name = "Đang thực hiện",
                    Data1 = obj.TotalDoing
                });
                data.Add(new ChartModel()
                {
                    Name = "Hoàn thành quá hạn",
                    Data1 = obj.TotalFinishOverTime
                });
                data.Add(new ChartModel()
                {
                    Name = "Quá hạn",
                    Data1 = obj.TotalOverTime
                });
                data.Add(new ChartModel()
                {
                    Name = "Tạm dừng",
                    Data1 = obj.TotalPause
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataAuditEmployee(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByEmployee(id, fromDate, toDate);
            if (obj != null)
            {
                int floor = obj.TotalAuditFail + obj.TotalAuditGood;
                data.Add(new ChartModel()
                {
                    Name = "Đạt(" + Math.Round((obj.TotalAuditGood != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditGood * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalAuditGood != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditGood * 100, floor)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Không đạt(" + Math.Round((obj.TotalAuditFail != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditFail * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalAuditFail != 0 ? Math.Floor(Math.Max((double)obj.TotalAuditFail * 100, floor)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataAuditEmployeeColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetByEmployee(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Đạt",
                    Data1 = obj.TotalAuditGood
                });
                data.Add(new ChartModel()
                {
                    Name = "Không đạt",
                    Data1 = obj.TotalAuditFail
                });
            }
            return data;
        }
        #endregion
        #endregion
        #region Thống kê cho module quản lý hồ sơ
        #region Thống kê hồ sơ theo danh mục
        public class ListProfileStatisticalInfo
        {
            public int CateID { get; set; }
            public string CateName { get; set; }
            public System.DateTime? CreateAt { get; set; }
            public int TotalProfileUse { get; set; }//Tổng hồ sơ sử dụng
            public int TotalProfileBorrow { get; set; }//Tổng hồ sơ đang mượn
            public int TotalProfileBorrowOverTime { get; set; }//Tổng hồ sơ mượn quá hạn
            public int TotalProfileNotUse { get; set; }//Tổng hồ sơ hết hạn
            public int TotalProfileCancel { get; set; }//tổng hồ sơ đã hủy
            public int TotalProfileRequestCancel { get; set; }//Tổng hồ sơ chờ hủy
            public int TotalPageCancel { get; set; }//Tổng số trang hồ sơ hủy
        }
        public static List<ListProfileStatisticalInfo> StatisticalCateProfile(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            ProfileDA profileDA = new ProfileDA();
            var dbo = profileDA.Repository;
            var results = dbo.ProfileCategories
                        .Where(i => !i.IsDelete)
                        .Where(i => i.DepartmentID == departmentId)
                        .Select(item => new ListProfileStatisticalInfo()
                        {
                            CateID = item.ID,
                            CateName = item.Name,
                            CreateAt = item.CreateAt
                        })
                        .Distinct()
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            foreach (var cate in results)
            {
                cate.TotalProfileUse = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Count();
                cate.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileNotUse = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                        || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                        || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                        || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                        || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                        ).Count() > 0)
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileCancel = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                        .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalPageCancel = dbo.Profiles
                     .Where(t => t.ProfileCategoryID == cate.CateID)
                     .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                    .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                        || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                        || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                        || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                        || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                        )
                                        .Count() > 0
                                    )
                        )
                        .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return results;
        }
        public static List<ListProfileStatisticalInfo> StatisticalProfileByDepartment(int departmentId, DateTime fromDate, DateTime toDate)
        {
            ProfileDA profileDA = new ProfileDA();
            var dbo = profileDA.Repository;
            var results = dbo.ProfileCategories
                        .Where(i => !i.IsDelete)
                        .Where(i => i.DepartmentID == departmentId)
                        .Select(item => new ListProfileStatisticalInfo()
                        {
                            CateID = item.ID,
                            CateName = item.Name,
                            CreateAt = item.CreateAt
                        })
                        .Distinct()
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            foreach (var cate in results)
            {
                cate.TotalProfileUse = dbo.Profiles
                   .Where(t => t.ProfileCategoryID == cate.CateID)
                   .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                       || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                       || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                       || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                       || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                       || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                       )
                   .Where(t => t.IsUse)
                   .Count();
                cate.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileNotUse = dbo.Profiles
                      .Where(t => t.ProfileCategoryID == cate.CateID)
                      .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                          || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                          || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                          || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                          || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                          || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                          || (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                          || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                          || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                          || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                          || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                          ).Count() > 0)
                          )
                      .Where(t => !t.IsUse)
                      .Count();
                cate.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileCancel = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                        .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalPageCancel = dbo.Profiles
                     .Where(t => t.ProfileCategoryID == cate.CateID)
                     .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                    .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                        || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                        || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                        || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                        || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                        )
                                        .Count() > 0
                                    )

                        )
                       .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return results;

        }
        public static List<ChartModel> GenerateDataCateProfileByDepartment(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalProfileByDepartment(id, fromDate, toDate);
            var floor = obj.Select(t => t.TotalProfileUse).Sum();
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.CateName + "(" + Math.Round((item.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)item.TotalProfileUse * 100, floor)) : 0) / floor) + "%)",
                    Data1 = item.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)item.TotalProfileUse * 100, floor)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataCateProfileByDepartmentColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalProfileByDepartment(id, fromDate, toDate);
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.CateName,
                    Data1 = item.TotalProfileUse,
                    Data2 = item.TotalProfileBorrow,
                    Data3 = item.TotalProfileBorrowOverTime,
                    Data4 = item.TotalProfileNotUse,
                    Data5 = item.TotalProfileRequestCancel,
                    Data6 = item.TotalProfileCancel
                });
            }
            return data;
        }
        public static ListProfileStatisticalInfo StatisticalProfileByCateID(int cateId, DateTime fromDate, DateTime toDate)
        {
            ProfileDA profileDA = new ProfileDA();
            var dbo = profileDA.Repository;
            var cate = dbo.ProfileCategories
                        .Where(i => !i.IsDelete)
                        .Where(i => i.ID == cateId)
                        .Select(item => new ListProfileStatisticalInfo()
                        {
                            CateID = item.ID,
                            CateName = item.Name,
                            CreateAt = item.CreateAt
                        })
                        .FirstOrDefault();
            if (cate != null)
            {
                cate.TotalProfileUse = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Count();
                cate.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileNotUse = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                             .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                              || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                              || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                              || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                              || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                              ).Count() > 0)
                              )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileCancel = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalPageCancel = dbo.Profiles
                    .Where(t => t.ProfileCategoryID == cate.CateID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                      .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return cate;
        }
        public static List<ChartModel> GenerateDataCateList(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalProfileByCateID(id, fromDate, toDate);
            if (obj != null)
            {
                int floor = obj.TotalProfileNotUse + obj.TotalProfileUse;
                data.Add(new ChartModel()
                {
                    Name = "Đang mượn(" + Math.Round((obj.TotalProfileBorrow != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileBorrow * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalProfileBorrow != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileBorrow * 100, floor)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Mượn quá hạn(" + Math.Round((obj.TotalProfileBorrowOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileBorrowOverTime * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalProfileBorrowOverTime != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileBorrowOverTime * 100, floor)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Chờ hủy(" + Math.Round((obj.TotalProfileRequestCancel != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileRequestCancel * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalProfileRequestCancel != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileRequestCancel * 100, floor)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Đã hủy(" + Math.Round((obj.TotalProfileNotUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileNotUse * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalProfileCancel != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileCancel * 100, floor)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataCateListColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalProfileByCateID(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Đang mượn",
                    Data1 = obj.TotalProfileBorrow
                });
                data.Add(new ChartModel()
                {
                    Name = "Mượn quá hạn",
                    Data1 = obj.TotalProfileBorrowOverTime
                });
                data.Add(new ChartModel()
                {
                    Name = "Chờ hủy",
                    Data1 = obj.TotalProfileRequestCancel
                });
                data.Add(new ChartModel()
                {
                    Name = "Đã hủy",
                    Data1 = obj.TotalProfileCancel
                });
            }
            return data;
        }
        #endregion
        #region Thống kê theo hồ sơ theo phòng ban
        public class DepartmentStatisticalProfileInfo
        {
            public int DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public bool Leaf { get; set; }
            public int ParentID { get; set; }
            public int TotalProfileUse { get; set; }//Tổng hồ sơ sử dụng
            public int TotalProfileBorrow { get; set; }//Tổng hồ sơ đang mượn
            public int TotalProfileBorrowOverTime { get; set; }//Tổng hồ sơ mượn quá hạn
            public int TotalProfileNotUse { get; set; }//Tổng hồ sơ hết hạn
            public int TotalProfileCancel { get; set; }//tổng hồ sơ đã hủy
            public int TotalProfileRequestCancel { get; set; }//Tổng hồ sơ chờ hủy
            public int TotalPageCancel { get; set; }//Tổng số trang hồ sơ hủy
        }
        public List<DepartmentStatisticalProfileInfo> GetDepartmentStatisticalProfile(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var results = dbo.HumanDepartments
                        .Where(i => (i.ParentID != null && i.ParentID == id))
                        .Where(i => i.IsActive == true || i.IsCancel || i.IsMerge || i.IsDelete)
                        .Where(i => i.IsCancel == false)
                        .Where(i => i.IsMerge == false)
                        .Where(i => i.IsDelete == false)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(item => new DepartmentStatisticalProfileInfo()
                        {
                            DepartmentID = item.ID,
                            ParentID = item.ParentID,
                            DepartmentName = item.Name,
                            Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                        }).ToList();
            foreach (var department in results)
            {
                if (department.ParentID != 0)
                {
                    department.TotalProfileUse = dbo.Profiles
                         .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                         .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                             || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                             || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                             || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                             || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                             || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                             )
                         .Where(t => t.IsUse)
                         .Count();
                    department.TotalProfileBorrow = dbo.Profiles
                        .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                        .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                            )
                        .Where(t => t.IsUse)
                        .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                        .Count();
                    department.TotalProfileBorrowOverTime = dbo.Profiles
                        .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                        .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                            )
                        .Where(t => t.IsUse)
                        .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                        .Count();
                    department.TotalProfileNotUse = dbo.Profiles
                        .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                        .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                            || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                          )
                        .Where(t => !t.IsUse)
                        .Count();
                    department.TotalProfileRequestCancel = dbo.Profiles
                        .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                        .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                            && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                      .Where(t => !t.IsUse)
                      .Count();
                    department.TotalProfileCancel = dbo.Profiles
                        .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                        .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                             && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                    department.TotalPageCancel = dbo.Profiles
                   .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                      .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
                }
                else
                {
                    department.TotalProfileUse = dbo.Profiles
                         .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                             || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                             || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                             || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                             || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                             || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                             )
                         .Where(t => t.IsUse)
                         .Count();
                    department.TotalProfileBorrow = dbo.Profiles
                        .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                            )
                        .Where(t => t.IsUse)
                        .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                        .Count();
                    department.TotalProfileBorrowOverTime = dbo.Profiles
                        .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                            )
                        .Where(t => t.IsUse)
                        .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                        .Count();
                    department.TotalProfileNotUse = dbo.Profiles
                        .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                             || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                        .Where(t => !t.IsUse)
                        .Count();
                    department.TotalProfileRequestCancel = dbo.Profiles
                        .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                            && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                        .Where(t => !t.IsUse)
                        .Count();
                    department.TotalProfileCancel = dbo.Profiles
                        .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                            && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                    .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                        || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                        || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                        || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                        || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                        )
                                        .Count() > 0
                                    )

                        )
                        .Where(t => !t.IsUse)
                        .Count();
                    department.TotalPageCancel = dbo.Profiles
                        .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                            || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                            || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                            || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                            && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                    .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                        || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                        || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                        || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                        || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                        )
                                        .Count() > 0
                                    )

                        )
                        .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                        .ToList()
                        .Sum();
                }
            }
            return results;
        }
        public static DepartmentStatisticalProfileInfo GetProfileByDepartment(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var department = dbo.HumanDepartments
                        .Where(i => i.ID == id)
                        .Select(item => new DepartmentStatisticalProfileInfo()
                        {
                            DepartmentID = item.ID,
                            ParentID = item.ParentID,
                            DepartmentName = item.Name,
                            Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                        }).FirstOrDefault();
            if (department.ParentID != 0)
            {
                department.TotalProfileUse = dbo.Profiles
                     .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                     .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                         || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                         || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                         || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                         || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                         || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                         )
                     .Where(t => t.IsUse)
                     .Count();
                department.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                department.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                department.TotalProfileNotUse = dbo.Profiles
                    .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                          || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                department.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                         && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                department.TotalProfileCancel = dbo.Profiles
                    .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                department.TotalPageCancel = dbo.Profiles
                    .Where(t => t.ProfileCategory.DepartmentID == department.DepartmentID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                         && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            else
            {
                department.TotalProfileUse = dbo.Profiles
                     .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                         || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                         || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                         || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                         || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                         || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                         )
                     .Where(t => t.IsUse)
                     .Count();
                department.TotalProfileBorrow = dbo.Profiles
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                department.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                department.TotalProfileNotUse = dbo.Profiles
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                department.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                department.TotalProfileCancel = dbo.Profiles
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                department.TotalPageCancel = dbo.Profiles
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return department;
        }
        public static List<ChartModel> GenerateDataProfileColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetProfileByDepartment(id, fromDate, toDate);
            data.Add(new ChartModel()
            {
                Name = "Đang mượn",
                Data1 = obj.TotalProfileBorrow
            });
            data.Add(new ChartModel()
            {
                Name = "Mượn quá hạn",
                Data1 = obj.TotalProfileBorrowOverTime
            });
            data.Add(new ChartModel()
            {
                Name = "Hồ sơ đã hủy",
                Data1 = obj.TotalProfileCancel
            });
            data.Add(new ChartModel()
            {
                Name = "Hồ sơ chờ hủy",
                Data1 = obj.TotalProfileRequestCancel
            });
            return data;
        }
        public static List<ChartModel> GenerateDataProfile(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetProfileByDepartment(id, fromDate, toDate);
            int floor = obj.TotalProfileNotUse + obj.TotalProfileUse;
            data.Add(new ChartModel()
            {
                Name = "Tổng số hồ sơ sử dụng(" + Math.Round((obj.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileUse * 100, floor)) : 0) / floor) + "%)",
                Data1 = obj.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileUse * 100, floor)) : 0
            });
            data.Add(new ChartModel()
            {
                Name = "Tổng số hồ sơ không sử dụng(" + Math.Round((obj.TotalProfileNotUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileNotUse * 100, floor)) : 0) / floor) + "%)",
                Data1 = obj.TotalProfileNotUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileNotUse * 100, floor)) : 0
            });
            return data;
        }
        #endregion
        #region Thống kê hồ sơ theo mức độ bảo mật
        public class SecurityLevelProfileStatisticalInfo
        {
            public int SecurityID { get; set; }
            public string LevelName { get; set; }
            public System.DateTime? CreateAt { get; set; }
            public int TotalProfileUse { get; set; }//Tổng hồ sơ sử dụng
            public int TotalProfileBorrow { get; set; }//Tổng hồ sơ đang mượn
            public int TotalProfileBorrowOverTime { get; set; }//Tổng hồ sơ mượn quá hạn
            public int TotalProfileNotUse { get; set; }//Tổng hồ sơ hết hạn
            public int TotalProfileCancel { get; set; }//tổng hồ sơ đã hủy
            public int TotalProfileRequestCancel { get; set; }//Tổng hồ sơ chờ hủy
            public int TotalPageCancel { get; set; }//Tổng số trang hồ sơ hủy
        }
        public static List<SecurityLevelProfileStatisticalInfo> GetSecurityLevelStatisticalProfile(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            ProfileDA profileDA = new ProfileDA();
            var dbo = profileDA.Repository;
            var results = dbo.ProfileSecurities
                        .Where(i => !i.IsDelete)
                        .Select(item => new SecurityLevelProfileStatisticalInfo()
                        {
                            SecurityID = item.ID,
                            LevelName = item.Name,
                            CreateAt = item.CreateAt
                        })
                        .Distinct()
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            foreach (var cate in results)
            {
                cate.TotalProfileUse = dbo.Profiles
                    .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Count();
                cate.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileNotUse = dbo.Profiles
                     .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                             .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                              || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                              || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                              || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                              || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                              ).Count() > 0)
                              )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileCancel = dbo.Profiles
                     .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                       && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalPageCancel = dbo.Profiles
                     .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                      .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return results;
        }
        public static List<SecurityLevelProfileStatisticalInfo> StatisticalProfile(DateTime fromDate, DateTime toDate)
        {
            ProfileDA profileDA = new ProfileDA();
            var dbo = profileDA.Repository;
            var results = dbo.ProfileSecurities
                    .Where(i => !i.IsDelete)
                    .Select(item => new SecurityLevelProfileStatisticalInfo()
                    {
                        SecurityID = item.ID,
                        LevelName = item.Name,
                        CreateAt = item.CreateAt
                    })
                    .Distinct()
                    .OrderByDescending(item => item.CreateAt)
                    .ToList();
            foreach (var cate in results)
            {
                cate.TotalProfileUse = dbo.Profiles
                   .Where(t => t.ProfileSecurityID == cate.SecurityID)
                   .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                       || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                       || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                       || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                       || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                       || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                       )
                   .Where(t => t.IsUse)
                   .Count();
                cate.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                cate.TotalProfileNotUse = dbo.Profiles
                     .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                             .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                              || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                              || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                              || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                              || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                              ).Count() > 0)
                              )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileRequestCancel = dbo.Profiles
                      .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalProfileCancel = dbo.Profiles
                    .Where(t => t.ProfileSecurityID == cate.SecurityID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                cate.TotalPageCancel = dbo.Profiles
                     .Where(t => t.ProfileSecurityID == cate.SecurityID)
                     .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                      .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return results;
        }
        public static List<ChartModel> GenerateDataSecurityLevelProfile(DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalProfile(fromDate, toDate);
            var floor = obj.Select(t => t.TotalProfileUse).Sum();
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.LevelName + "(" + Math.Round((item.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)item.TotalProfileUse * 100, floor)) : 0) / floor) + "%)",
                    Data1 = item.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)item.TotalProfileUse * 100, floor)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataSecurityLevelProfileColumn(DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalProfile(fromDate, toDate);
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.LevelName,
                    Data1 = item.TotalProfileUse
                });
            }
            return data;
        }
        #endregion
        #region Thống kê theo hồ sơ theo người lưu trữ
        public class ReceivedStatisticalProfileInfo
        {
            public string _AvatarUrl = "/Generic/File/LoadAvatarFile?employeeId={0}";
            public string _AvatarUrlDefault = "/Content/images/underfind.jpg";
            public int EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public System.DateTime? CreateAt { get; set; }
            public string Avatar
            {
                get
                {
                    if (EmployeeID == 0)
                        return _AvatarUrlDefault;
                    return string.Format(_AvatarUrl, EmployeeID);
                }
                set
                {
                    _AvatarUrl = value;
                }
            }
            public int TotalProfileUse { get; set; }//Tổng hồ sơ sử dụng
            public int TotalProfileBorrow { get; set; }//Tổng hồ sơ đang mượn
            public int TotalProfileBorrowOverTime { get; set; }//Tổng hồ sơ mượn quá hạn
            public int TotalProfileNotUse { get; set; }//Tổng hồ sơ hết hạn
            public int TotalProfileCancel { get; set; }//tổng hồ sơ đã hủy
            public int TotalProfileRequestCancel { get; set; }//Tổng hồ sơ chờ hủy
            public int TotalPageCancel { get; set; }//Tổng số trang hồ sơ hủy
        }
        public static List<ReceivedStatisticalProfileInfo> GetReceivedStatisticalProfile(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var results = dbo.HumanRoles.Where(i => i.HumanDepartmentID == departmentId)
                 .SelectMany(i => i.HumanOrganizations)
                 .Select(i => i.HumanEmployee).Distinct()
                 .Select(item => new ReceivedStatisticalProfileInfo()
                 {
                     EmployeeID = item.ID,
                     EmployeeName = item.Name,
                     CreateAt = item.CreateAt
                 })
                 .Distinct()
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            foreach (var employee in results)
            {
                employee.TotalProfileUse = dbo.Profiles
                   .Where(t => t.EmployeeID == employee.EmployeeID)
                   .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                       || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                       || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                       || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                       || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                       || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                       )
                   .Where(t => t.IsUse)
                   .Count();
                employee.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                employee.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                employee.TotalProfileNotUse = dbo.Profiles
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                             .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                              || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                              || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                              || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                              || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                              ).Count() > 0)
                              )
                    .Where(t => !t.IsUse)
                    .Count();
                employee.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                       && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                employee.TotalProfileCancel = dbo.Profiles
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Where(t => !t.IsUse)
                    .Count();
                employee.TotalPageCancel = dbo.Profiles
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                                || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                                || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                                || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                                || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                                )
                                                .Count() > 0
                                            )
                            )
                    .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return results;
        }
        public static ReceivedStatisticalProfileInfo GetProfileByEmployee(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var employee = dbo.HumanEmployees
                        .Where(i => i.ID == id)
                        .Select(item => new ReceivedStatisticalProfileInfo()
                        {
                            EmployeeID = item.ID,
                            EmployeeName = item.Name,
                            CreateAt = item.CreateAt
                        }).FirstOrDefault();
            if (employee != null)
            {
                employee.TotalProfileUse = dbo.Profiles
                     .Where(t => t.EmployeeID == id)
                     .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                         || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                         || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                         || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                         || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                         || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                         )
                     .Where(t => t.IsUse)
                     .Count();
                employee.TotalProfileBorrow = dbo.Profiles
                    .Where(t => t.EmployeeID == id)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt >= DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                employee.TotalProfileBorrowOverTime = dbo.Profiles
                    .Where(t => t.EmployeeID == id)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                        )
                    .Where(t => t.IsUse)
                    .Where(t => t.ProfileBorrows.Where(x => x.LimitAt < DateTime.Now).Where(x => !x.IsReturn).Count() > 0)
                    .Count();
                employee.TotalProfileNotUse = dbo.Profiles
                    .Where(t => t.EmployeeID == id)
                    .Where(t => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate)
                          || (t.NotUseAt.HasValue && t.ProfileCancelProfiles
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                employee.TotalProfileRequestCancel = dbo.Profiles
                    .Where(t => t.EmployeeID == id)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                         && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => !x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                employee.TotalProfileCancel = dbo.Profiles
                    .Where(t => t.EmployeeID == id)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Where(t => !t.IsUse)
                    .Count();
                employee.TotalPageCancel = dbo.Profiles
                    .Where(t => t.EmployeeID == id)
                    .Where(t => ((t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate <= t.NotUseAt && toDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.NotUseAt && toDate >= t.NotUseAt && fromDate >= t.ReceivedAt)
                        || (t.NotUseAt.HasValue && fromDate >= t.ReceivedAt && toDate <= t.NotUseAt)
                        || (t.NotUseAt.HasValue && fromDate <= t.ReceivedAt && toDate >= t.NotUseAt)
                        || (!t.NotUseAt.HasValue && t.ReceivedAt <= fromDate))
                        && (t.NotUseAt.HasValue && t.ProfileCancelProfiles.Where(x => x.ProfileCancel.IsCanceled)
                                            .Where(x => (t.ReceivedAt >= fromDate && t.ReceivedAt <= toDate)
                                            || (fromDate <= t.ReceivedAt && toDate <= x.ProfileCancel.Date && toDate >= t.ReceivedAt)
                                            || (fromDate <= x.ProfileCancel.Date && toDate >= x.ProfileCancel.Date && fromDate >= t.ReceivedAt)
                                            || (fromDate >= t.ReceivedAt && toDate <= x.ProfileCancel.Date)
                                            || (fromDate <= t.ReceivedAt && toDate >= x.ProfileCancel.Date)
                                            )
                                            .Count() > 0
                                        )
                        )
                    .Select(t => t.ProfileCancelProfiles.Count > 0 ? t.ProfileCancelProfiles.ToList().Select(x => x.PageTotal).Sum() : 0)
                    .ToList()
                    .Sum();
            }
            return employee;
        }
        public static List<ChartModel> GenerateDataReceivedProfileColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetProfileByEmployee(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Đang mượn",
                    Data1 = obj.TotalProfileBorrow
                });
                data.Add(new ChartModel()
                {
                    Name = "Mượn quá hạn",
                    Data1 = obj.TotalProfileBorrowOverTime
                });
                data.Add(new ChartModel()
                {
                    Name = "Hồ sơ đã hủy",
                    Data1 = obj.TotalProfileCancel
                });
                data.Add(new ChartModel()
                {
                    Name = "Hồ sơ chờ hủy",
                    Data1 = obj.TotalProfileRequestCancel
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataReceivedProfile(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetProfileByEmployee(id, fromDate, toDate);
            if (obj != null)
            {
                int floor = obj.TotalProfileNotUse + obj.TotalProfileUse;
                data.Add(new ChartModel()
                {
                    Name = "Tổng số hồ sơ sử dụng(" + Math.Round((obj.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileUse * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalProfileUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileUse * 100, floor)) : 0
                });
                data.Add(new ChartModel()
                {
                    Name = "Tổng số hồ sơ không sử dụng(" + Math.Round((obj.TotalProfileNotUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileNotUse * 100, floor)) : 0) / floor) + "%)",
                    Data1 = obj.TotalProfileNotUse != 0 ? Math.Floor(Math.Max((double)obj.TotalProfileNotUse * 100, floor)) : 0
                });
            }
            return data;
        }
        #endregion
        #endregion
        #region Thống kê cho module quản lý tài liệu
        #region Thống kê tài liệu theo phòng ban
        public class DepartmentStatisticalDocumnetInfo
        {
            public int DepartmentID { get; set; }//Tổng công việc
            public string DepartmentName { get; set; }
            public bool Leaf { get; set; }
            public int ParentID { get; set; }
            public int TotalDocument { get; set; }//Tổng tài liệu
            public int TotalDocumentIssued { get; set; }//Tổng tài liệu ban hành
            public int TotalDocumentObsolete { get; set; }//Tổng tài liệu lỗi thời
            public int TotalDocumentIn { get; set; }//Tổng tài liệu nội bộ
            public int TotalDocumentInIssued { get; set; }//Tổng tài liệu nội bộ được ban hành
            public int TotalDocumentInObsolete { get; set; }//Tổng tài liệu nội bộ lỗi thời
            public int TotalDocumentOut { get; set; }//Tổng tài liệu bên ngoài
            public int TotalDocumentOutIssued { get; set; }//Tổng tài liệu bên ngoài ban hành
            public int TotalDocumentOutObsolete { get; set; }//Tổng tài liệu bên ngoài lỗi thời
            public int NumberOfDistribution { get; set; }//Số tài liệu được phân phối
            public int NumberDistribution { get; set; }//Số tài liệu đã phân phối
            public int NumberOfThuhoi { get; set; }//Số tài liệu thu hồi
        }
        public static DepartmentStatisticalDocumnetInfo GetDocumentByDepartment(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var department = dbo.HumanDepartments
                    .Where(i => i.ID == id)
                    .Select(item => new DepartmentStatisticalDocumnetInfo()
                    {
                        DepartmentID = item.ID,
                        ParentID = item.ParentID,
                        DepartmentName = item.Name,
                        Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                    }).FirstOrDefault();
            if (department.ParentID != 0)
            {
                department.TotalDocument = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.IsDelete)
                    .Count();
                department.TotalDocumentIssued = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                department.TotalDocumentObsolete = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsObsolete)
                    .Count();
                department.TotalDocumentIn = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Count();
                department.TotalDocumentInIssued = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                department.TotalDocumentInObsolete = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                department.TotalDocumentOut = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Count();
                department.TotalDocumentOutIssued = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                department.TotalDocumentOutObsolete = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                department.NumberDistribution = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.ToList().Select(x => x.Number).Sum())
                    .Sum();
                department.NumberOfDistribution = dbo.Documents
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.Where(x => x.DepartmentID == department.DepartmentID).ToList().Select(x => x.Number).Sum())
                    .Sum();
                department.NumberOfThuhoi = dbo.Documents
                    .Where(t => t.DepartmentID == department.DepartmentID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.Where(x => x.DateObsolote.HasValue).Where(x => x.NumberObsolete.HasValue).ToList().Select(x => x.NumberObsolete.Value).Sum())
                    .Sum();
            }
            else
            {
                department.TotalDocument = dbo.Documents
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.IsDelete)
                    .Count();
                department.TotalDocumentIssued = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                department.TotalDocumentObsolete = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsObsolete)
                    .Count();
                department.TotalDocumentIn = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Count();
                department.TotalDocumentInIssued = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                department.TotalDocumentInObsolete = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                department.TotalDocumentOut = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .ToList()
                    .Count();
                department.TotalDocumentOutIssued = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                department.TotalDocumentOutObsolete = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                department.NumberDistribution = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.ToList().Select(x => x.Number).Sum())
                    .Sum();
                department.NumberOfDistribution = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.Where(x => x.DepartmentID == department.DepartmentID).ToList().Select(x => x.Number).Sum())
                    .Sum();
                department.NumberOfThuhoi = dbo.Documents
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                     .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.Where(x => x.DateObsolote.HasValue).Where(x => x.NumberObsolete.HasValue).ToList().Select(x => x.NumberObsolete.Value).Sum())
                    .Sum();
            }
            return department;
        }
        public List<DepartmentStatisticalDocumnetInfo> GetDepartmentStatisticalDocument(int id, DateTime fromDate, DateTime toDate)
        {
            DocumentDA documentDA = new DocumentDA();
            var dbo = documentDA.Repository;
            var results = dbo.HumanDepartments
                        .Where(i => (i.ParentID != null && i.ParentID == id))
                        .Where(i => i.IsActive == true || i.IsCancel || i.IsMerge || i.IsDelete)
                        .Where(i => i.IsCancel == false)
                        .Where(i => i.IsMerge == false)
                        .Where(i => i.IsDelete == false)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(item => new DepartmentStatisticalDocumnetInfo()
                        {
                            DepartmentID = item.ID,
                            ParentID = item.ParentID,
                            DepartmentName = item.Name,
                            Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                        }).ToList();
            foreach (var department in results)
            {
                if (department.ParentID != 0)
                {
                    department.TotalDocument = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.IsDelete)
                        .Count();
                    department.TotalDocumentIssued = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.IsPublic && !t.IsObsolete)
                        .Count();
                    department.TotalDocumentObsolete = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.IsObsolete)
                        .Count();
                    department.TotalDocumentIn = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.Type)
                        .Count();
                    department.TotalDocumentInIssued = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.Type)
                        .Where(t => t.IsPublic && !t.IsObsolete)
                        .Count();
                    department.TotalDocumentInObsolete = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.Type)
                        .Where(t => t.IsObsolete)
                        .Count();
                    department.TotalDocumentOut = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.Type)
                        .Count();
                    department.TotalDocumentOutIssued = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.Type)
                        .Where(t => t.IsPublic && !t.IsObsolete)
                        .Count();
                    department.TotalDocumentOutObsolete = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.Type)
                        .Where(t => t.IsObsolete)
                        .Count();
                    department.NumberDistribution = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .ToList()
                        .Select(t => t.DocumentDistributions.ToList().Select(x => x.Number).Sum())
                        .Sum();
                    department.NumberOfDistribution = dbo.Documents
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .ToList()
                        .Select(t => t.DocumentDistributions.Where(x => x.DepartmentID == department.DepartmentID).ToList().Select(x => x.Number).Sum())
                        .Sum();
                    department.NumberOfThuhoi = dbo.Documents
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.PublicDate.HasValue)
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .ToList()
                        .Select(t => t.DocumentDistributions.Where(x => x.DateObsolote.HasValue).Where(x => x.NumberObsolete.HasValue).ToList().Select(x => x.NumberObsolete.Value).Sum())
                        .Sum();
                }
                else
                {
                    department.TotalDocument = dbo.Documents
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.IsDelete)
                        .Count();
                    department.TotalDocumentIssued = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.IsPublic && !t.IsObsolete)
                        .Count();
                    department.TotalDocumentObsolete = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.IsObsolete)
                        .Count();
                    department.TotalDocumentIn = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.Type)
                        .Count();
                    department.TotalDocumentInIssued = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.Type)
                        .Where(t => t.IsPublic && !t.IsObsolete)
                        .Count();
                    department.TotalDocumentInObsolete = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => !t.Type)
                        .Where(t => t.IsObsolete)
                        .Count();
                    department.TotalDocumentOut = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.Type)
                        .ToList()
                        .Count();
                    department.TotalDocumentOutIssued = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.Type)
                        .Where(t => t.IsPublic && !t.IsObsolete)
                        .Count();
                    department.TotalDocumentOutObsolete = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .Where(t => t.Type)
                        .Where(t => t.IsObsolete)
                        .Count();
                    department.NumberDistribution = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .ToList()
                        .Select(t => t.DocumentDistributions.ToList().Select(x => x.Number).Sum())
                        .Sum();
                    department.NumberOfDistribution = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .ToList()
                        .Select(t => t.DocumentDistributions.Where(x => x.DepartmentID == department.DepartmentID).ToList().Select(x => x.Number).Sum())
                        .Sum();
                    department.NumberOfThuhoi = dbo.Documents
                        .Where(t => !t.IsDelete)
                        .Where(t => t.PublicDate.HasValue)
                         .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                        .ToList()
                        .Select(t => t.DocumentDistributions.Where(x => x.DateObsolote.HasValue).Where(x => x.NumberObsolete.HasValue).ToList().Select(x => x.NumberObsolete.Value).Sum())
                        .Sum();
                }
            }
            return results;
        }
        public static List<ChartModel> GenerateDataDocumentColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetDocumentByDepartment(id, fromDate, toDate);
            data.Add(new ChartModel()
            {
                Name = "Tài liệu nội bộ",
                Data1 = obj.TotalDocumentIn,
                Data2 = obj.TotalDocumentInIssued,
                Data3 = obj.TotalDocumentInObsolete,
            });
            data.Add(new ChartModel()
            {
                Name = "Tài liệu bên ngoài",
                Data1 = obj.TotalDocumentOut,
                Data2 = obj.TotalDocumentOutIssued,
                Data3 = obj.TotalDocumentOutObsolete
            });
            return data;
        }
        #endregion
        #region Thống kê tài liệu theo danh mục
        public class ListStatisticalDocumnetInfo
        {
            public int CateID { get; set; }
            public string CateName { get; set; }
            public System.DateTime? CreateAt { get; set; }
            public int TotalDocument { get; set; }//Tổng tài liệu
            public int TotalDocumentIssued { get; set; }//Tổng tài liệu ban hành
            public int TotalDocumentObsolete { get; set; }//Tổng tài liệu lỗi thời
            public int TotalDocumentIn { get; set; }//Tổng tài liệu nội bộ
            public int TotalDocumentInIssued { get; set; }//Tổng tài liệu nội bộ được ban hành
            public int TotalDocumentInObsolete { get; set; }//Tổng tài liệu nội bộ lỗi thời
            public int TotalDocumentOut { get; set; }//Tổng tài liệu bên ngoài
            public int TotalDocumentOutIssued { get; set; }//Tổng tài liệu bên ngoài ban hành
            public int TotalDocumentOutObsolete { get; set; }//Tổng tài liệu bên ngoài lỗi thời
            public int NumberDistribution { get; set; }//Số tài liệu đã phân phối
            public int NumberOfThuhoi { get; set; }//Số tài liệu thu hồi
        }
        public static List<ListStatisticalDocumnetInfo> StatisticalCateDocument(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            DocumentDA documentDA = new DocumentDA();
            var dbo = documentDA.Repository;
            var results = dbo.DocumentCategories
                    .Where(i => !i.IsDelete)
                    .Where(i => i.DepartmentID == departmentId)
                    .Select(item => new ListStatisticalDocumnetInfo()
                    {
                        CateID = item.ID,
                        CateName = item.Name,
                        CreateAt = item.CreateAt
                    })
                    .Distinct()
                    .OrderByDescending(item => item.CreateAt)
                    .Filter(filter)
                    .ToList();
            foreach (var cate in results)
            {
                cate.TotalDocument = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.IsDelete)
                    .Count();
                cate.TotalDocumentIssued = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentObsolete = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.TotalDocumentIn = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Count();
                cate.TotalDocumentInIssued = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentInObsolete = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.TotalDocumentOut = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Count();
                cate.TotalDocumentOutIssued = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentOutObsolete = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.NumberDistribution = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.ToList().Select(x => x.Number).Sum())
                    .Sum();
                cate.NumberOfThuhoi = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.Where(x => x.DateObsolote.HasValue).Where(x => x.NumberObsolete.HasValue).ToList().Select(x => x.NumberObsolete.Value).Sum())
                    .Sum();
            }
            return results;
        }
        public static List<ListStatisticalDocumnetInfo> StatisticalDocumentByDepartment(int departmentId, DateTime fromDate, DateTime toDate)
        {
            DocumentDA documentDA = new DocumentDA();
            var dbo = documentDA.Repository;
            var results = dbo.DocumentCategories
                    .Where(i => !i.IsDelete)
                    .Where(i => i.DepartmentID == departmentId)
                    .Select(item => new ListStatisticalDocumnetInfo()
                    {
                        CateID = item.ID,
                        CateName = item.Name,
                        CreateAt = item.CreateAt
                    })
                    .Distinct()
                    .OrderByDescending(item => item.CreateAt)
                    .ToList();
            foreach (var cate in results)
            {
                cate.TotalDocument = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.IsDelete)
                    .ToList()
                    .Count();
                cate.TotalDocumentIssued = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentObsolete = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.TotalDocumentIn = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Count();
                cate.TotalDocumentInIssued = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentInObsolete = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.TotalDocumentOut = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Count();
                cate.TotalDocumentOutIssued = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentOutObsolete = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.NumberDistribution = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.ToList().Select(x => x.Number).Sum())
                    .Sum();
                cate.NumberOfThuhoi = dbo.Documents
                    .Where(t => t.DocumentCategoryID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                       .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.Where(x => x.DateObsolote.HasValue).Where(x => x.NumberObsolete.HasValue).ToList().Select(x => x.NumberObsolete.Value).Sum())
                    .Sum();
            }
            return results;

        }
        public static List<ChartModel> GenerateDataCateDocumentByDepartment(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalDocumentByDepartment(id, fromDate, toDate);
            var floor = obj.Select(t => t.TotalDocument).Sum();
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.CateName + "(" + Math.Round((item.TotalDocument != 0 ? Math.Floor(Math.Max((double)item.TotalDocument * 100, floor)) : 0) / floor) + "%)",
                    Data1 = item.TotalDocument != 0 ? Math.Floor(Math.Max((double)item.TotalDocument * 100, floor)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataCateDocumentByDepartmentColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalDocumentByDepartment(id, fromDate, toDate);
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.CateName,
                    Data1 = item.TotalDocument,
                    Data2 = item.TotalDocumentIssued,
                    Data3 = item.TotalDocumentObsolete,
                    Data4 = item.TotalDocumentIn,
                    Data5 = item.TotalDocumentInIssued,
                    Data6 = item.TotalDocumentInObsolete,
                    Data7 = item.TotalDocumentOut,
                    Data8 = item.TotalDocumentOutIssued,
                    Data9 = item.TotalDocumentOutObsolete
                });

            }
            return data;
        }
        public static ListStatisticalDocumnetInfo StatisticalDocumentByCateID(int cateId, DateTime fromDate, DateTime toDate)
        {
            ProfileDA profileDA = new ProfileDA();
            var dbo = profileDA.Repository;
            var cate = dbo.DocumentCategories
                .Where(i => !i.IsDelete)
                .Where(i => i.ID == cateId)
                .Select(item => new ListStatisticalDocumnetInfo()
                {
                    CateID = item.ID,
                    CateName = item.Name,
                    CreateAt = item.CreateAt
                })
                .FirstOrDefault();
            if (cate != null)
            {
                cate.TotalDocument = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.IsDelete)
                    .Count();
                cate.TotalDocumentIssued = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentObsolete = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.TotalDocumentIn = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Count();
                cate.TotalDocumentInIssued = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentInObsolete = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => !t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.TotalDocumentOut = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Count();
                cate.TotalDocumentOutIssued = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsPublic && !t.IsObsolete)
                    .Count();
                cate.TotalDocumentOutObsolete = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .Where(t => t.Type)
                    .Where(t => t.IsObsolete)
                    .Count();
                cate.NumberDistribution = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.ToList().Select(x => x.Number).Sum())
                    .Sum();
                cate.NumberOfThuhoi = dbo.Documents
                    .Where(t => t.DocumentCategory.ID == cate.CateID)
                    .Where(t => !t.IsDelete)
                    .Where(t => t.PublicDate.HasValue)
                    .Where(t => (t.PublicDate >= fromDate && t.PublicDate <= toDate) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate <= t.DateObsolete && toDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate <= t.DateObsolete && toDate >= t.DateObsolete && fromDate >= t.PublicDate) || (t.DateObsolete.HasValue && fromDate >= t.PublicDate && toDate <= t.DateObsolete) || (t.DateObsolete.HasValue && fromDate <= t.PublicDate && toDate >= t.DateObsolete) || (!t.DateObsolete.HasValue && t.PublicDate <= fromDate))
                    .ToList()
                    .Select(t => t.DocumentDistributions.Where(x => x.DateObsolote.HasValue).Where(x => x.NumberObsolete.HasValue).ToList().Select(x => x.NumberObsolete.Value).Sum())
                    .Sum();
            }
            return cate;
        }
        public static List<ChartModel> GenerateDataCateDocumentListColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalDocumentByCateID(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Tài liệu nội bộ",
                    Data1 = obj.TotalDocumentIn,
                    Data2 = obj.TotalDocumentInIssued,
                    Data3 = obj.TotalDocumentInObsolete,
                });
                data.Add(new ChartModel()
                {
                    Name = "Tài liệu bên ngoài",
                    Data1 = obj.TotalDocumentOut,
                    Data2 = obj.TotalDocumentOutIssued,
                    Data3 = obj.TotalDocumentOutObsolete
                });
            }
            return data;
        }
        #endregion
        #endregion
        #region Thống kê cho module quản lý cung cấp dịch vụ
        #region Thống kê lệnh cung cấp dịch vụ theo dịch vụ
        public class CommandStatisticalServiceInfo
        {
            public int ServiceID { get; set; }//Tổng công việc
            public string ServiceName { get; set; }
            public int TotalCommand { get; set; }//Tổng lệnh
            public int TotalCommandApproval { get; set; }//Tổng lệnh được thực hiện
            public int TotalCommandNotApproval { get; set; }//Tổng lệnh không được thực hiện
            public int TotalCommandDoing { get; set; }//Tổng lệnh đang thực hiện
            public int TotalCommandCancel { get; set; }//Tổng lệnh hủy
            public int TotalCommandPause { get; set; }//Tổng lệnh đang tam dung
            public int TotalCommandFinished { get; set; }//Tổng lệnh đã hoàn thành
        }
        public static List<CommandStatisticalServiceInfo> StatisticalCommandService(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            ServiceDA serviceDA = new ServiceDA();
            var dbo = serviceDA.Repository;
            var serviceStatisticals = dbo.Services
                       .Where(t => !t.IsDelete)
                       .Where(t => t.IsUse)
                       .OrderByDescending(t => t.CreateAt)
                       .Select(item => new CommandStatisticalServiceInfo()
                       {
                           ServiceID = item.ID,
                           ServiceName = item.Name
                       })
                       .Filter(filter)
                       .ToList();
            foreach (var serviceStatistical in serviceStatisticals)
            {
                serviceStatistical.TotalCommand = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandApproval = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandNotApproval = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandDoing = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => !t.CustomerContract.IsFinish)
                    .Where(t => !t.CustomerContract.IsPause)
                    .Where(t => !t.CustomerContract.IsCancel)
                    .Where(t => t.CustomerContract.IsStart)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandPause = dbo.CustomerOrders
                   .Where(t => t.CustomerContractID.HasValue)
                   .Where(t => !t.CustomerContract.IsFinish)
                   .Where(t => t.CustomerContract.IsPause && !t.CustomerContract.IsCancel)
                   .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                   .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                   .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                   .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                   .Count();
                serviceStatistical.TotalCommandCancel = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => t.CustomerContract.IsCancel)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandFinished = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => t.CustomerContract.IsFinish)
                    .Where(t => !t.CustomerContract.IsCancel)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
            }
            return serviceStatisticals;
        }
        public static List<CommandStatisticalServiceInfo> StatisticalCommandServiceForChart(DateTime fromDate, DateTime toDate)
        {
            DocumentDA documentDA = new DocumentDA();
            var dbo = documentDA.Repository;
            var serviceStatisticals = dbo.Services
                       .Where(t => !t.IsDelete)
                       .Where(t => t.IsUse)
                       .Select(item => new CommandStatisticalServiceInfo()
                       {
                           ServiceID = item.ID,
                           ServiceName = item.Name
                       })
                       .ToList();
            foreach (var serviceStatistical in serviceStatisticals)
            {
                serviceStatistical.TotalCommand = dbo.CustomerOrders
                   .Where(t => t.CustomerContractID.HasValue)
                   .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                   .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                   .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                   .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                   .Count();
                serviceStatistical.TotalCommandApproval = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandNotApproval = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandDoing = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => !t.CustomerContract.IsFinish)
                    .Where(t => !t.CustomerContract.IsPause)
                    .Where(t => t.CustomerContract.IsStart)
                    .Where(t => !t.CustomerContract.IsCancel)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandPause = dbo.CustomerOrders
                   .Where(t => t.CustomerContractID.HasValue)
                   .Where(t => t.CustomerContract.IsPause)
                   .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                   .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                   .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                   .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                   .Count();
                serviceStatistical.TotalCommandCancel = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => t.CustomerContract.IsCancel)
                    .Where(t => !t.CustomerContract.IsFinish)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
                serviceStatistical.TotalCommandFinished = dbo.CustomerOrders
                    .Where(t => t.CustomerContractID.HasValue)
                    .Where(t => t.CustomerContract.IsFinish)
                    .Where(t => !t.CustomerContract.IsCancel)
                    .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                    .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                    .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                    .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                    .Count();
            }
            return serviceStatisticals;

        }
        public static CommandStatisticalServiceInfo StatisticalCommandByServiceID(int id, DateTime fromDate, DateTime toDate)
        {
            ServiceDA serviceDA = new ServiceDA();
            var dbo = serviceDA.Repository;
            var serviceStatistical = dbo.Services
                        .Where(i => i.ID == id)
                        .Select(item => new CommandStatisticalServiceInfo()
                        {
                            ServiceID = item.ID,
                            ServiceName = item.Name
                        }).FirstOrDefault();
            serviceStatistical.TotalCommand = dbo.CustomerOrders
                   .Where(t => t.CustomerContractID.HasValue)
                   .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                   .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                   .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                   .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                   .Count();
            serviceStatistical.TotalCommandApproval = dbo.CustomerOrders
                .Where(t => t.CustomerContractID.HasValue)
                .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                .Count();
            serviceStatistical.TotalCommandNotApproval = dbo.CustomerOrders
                .Where(t => t.CustomerContractID.HasValue)
                .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                .Count();
            serviceStatistical.TotalCommandDoing = dbo.CustomerOrders
                .Where(t => t.CustomerContractID.HasValue)
                .Where(t => !t.CustomerContract.IsFinish)
                .Where(t => !t.CustomerContract.IsPause)
                .Where(t => t.CustomerContract.IsStart)
                .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                .Count();
            serviceStatistical.TotalCommandPause = dbo.CustomerOrders
               .Where(t => t.CustomerContractID.HasValue)
               .Where(t => t.CustomerContract.IsPause)
               .Where(t => !t.CustomerContract.IsFinish)
               .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
               .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
               .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
               .Where(t => t.ServiceID == serviceStatistical.ServiceID)
               .Count();
            serviceStatistical.TotalCommandCancel = dbo.CustomerOrders
                .Where(t => t.CustomerContractID.HasValue)
                .Where(t => t.CustomerContract.IsCancel)
                .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                .Count();
            serviceStatistical.TotalCommandFinished = dbo.CustomerOrders
                .Where(t => t.CustomerContractID.HasValue)
                .Where(t => t.CustomerContract.IsFinish)
                .Where(t => !t.CustomerContract.IsCancel)
                .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                .Where(t => t.ServiceID == serviceStatistical.ServiceID)
                .Count();
            return serviceStatistical;
        }
        public static List<ChartModel> GenerateDataCommandService(DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalCommandServiceForChart(fromDate, toDate);
            var floor = obj.Select(t => t.TotalCommandFinished).Sum();
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.ServiceName + "(" + Math.Round((item.TotalCommandFinished != 0 ? Math.Floor(Math.Max((double)item.TotalCommandFinished * 100, floor)) : 0) / floor) + "%)",
                    Data1 = item.TotalCommandFinished != 0 ? Math.Floor(Math.Max((double)item.TotalCommandFinished * 100, floor)) : 0
                });
            }
            return data;
        }
        public static List<ChartModel> GenerateDataCommandServiceColumn(DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = StatisticalCommandServiceForChart(fromDate, toDate);
            foreach (var item in obj)
            {
                data.Add(new ChartModel()
                {
                    Name = item.ServiceName,
                    Data1 = item.TotalCommand,
                    Data2 = item.TotalCommandApproval,
                    Data3 = item.TotalCommandNotApproval,
                    Data4 = item.TotalCommandDoing,
                    Data5 = item.TotalCommandPause,
                    Data6 = item.TotalCommandCancel,
                    Data7 = item.TotalCommandFinished
                });
            }
            return data;
        }
        #endregion
        #endregion
        #region Thống kê cho module quản lý công văn
        #region Thống kê công văn theo phòng ban
        public class DepartmentStatisticalDispatchInfo
        {
            public int DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public bool Leaf { get; set; }
            public int ParentID { get; set; }
            public int TotalDispatchTo { get; set; }//Tổng công văn đến
            public int TotalDispatchToVerify { get; set; }//Tổng công văn đến đã xác nhận
            public int TotalDispatchGo { get; set; }//Tổng công văn đi
            public int TotalDispatchGoVerify { get; set; }//Tổng công văn đi đã xác nhận
        }
        public List<DepartmentStatisticalDispatchInfo> GetDepartmentStatisticalDispatch(int id, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            DispatchGoDA dispatchGoDA = new DispatchGoDA();
            var dbo = dispatchGoDA.Repository;
            var results = dbo.HumanDepartments
                            .Where(i => (i.ParentID != null && i.ParentID == id))
                            .Where(i => i.IsActive == true || i.IsCancel || i.IsMerge || i.IsDelete)
                            .Where(i => i.IsCancel == false)
                            .Where(i => i.IsMerge == false)
                            .Where(i => i.IsDelete == false)
                            .Where(i => !i.IsDestroy)
                            .OrderBy(i => i.Position)
                            .Select(item => new DepartmentStatisticalDispatchInfo()
                            {
                                DepartmentID = item.ID,
                                ParentID = item.ParentID,
                                DepartmentName = item.Name,
                                Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                            })
                            .ToList();
            foreach (var department in results)
            {
                if (department.ParentID != 0)
                {
                    department.TotalDispatchTo = dbo.DispatchToDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                    department.TotalDispatchToVerify = dbo.DispatchToDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                    department.TotalDispatchGo = dbo.DispatchGoDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                    department.TotalDispatchGoVerify = dbo.DispatchGoDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.IsVerify)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                }
                else
                {
                    department.TotalDispatchTo = dbo.DispatchToDepartments
                         .ToList()
                         .Select(t => t.DispatchTo)
                         .Where(t => t.Date >= fromDate && t.Date <= toDate)
                         .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                         .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                         .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                         .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                         .Count();
                    department.TotalDispatchToVerify = dbo.DispatchToDepartments
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                        .Count();
                    department.TotalDispatchGo = dbo.DispatchGoDepartments
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                        .Count();
                    department.TotalDispatchGoVerify = dbo.DispatchGoDepartments
                        .Where(t => t.IsVerify)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                        .Count();
                }
            }
            return results;
        }
        public static DepartmentStatisticalDispatchInfo GetDispatchByDepartment(int id, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            DispatchGoDA dispatchGoDA = new DispatchGoDA();
            var dbo = dispatchGoDA.Repository;
            var department = dbo.HumanDepartments
                        .Where(i => i.ID == id)
                        .Select(item => new DepartmentStatisticalDispatchInfo()
                        {
                            DepartmentID = item.ID,
                            ParentID = item.ParentID,
                            DepartmentName = item.Name,
                            Leaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID)
                        }).FirstOrDefault();
            if (department != null)
            {
                if (department.ParentID != 0)
                {

                    department.TotalDispatchTo = dbo.DispatchToDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                    department.TotalDispatchToVerify = dbo.DispatchToDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                    department.TotalDispatchGo = dbo.DispatchGoDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                    department.TotalDispatchGoVerify = dbo.DispatchGoDepartments
                        .Where(t => t.DepartmentID == department.DepartmentID)
                        .Where(t => t.IsVerify)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                        .Count();
                }
                else
                {
                    department.TotalDispatchTo = dbo.DispatchToDepartments
                         .ToList()
                         .Select(t => t.DispatchTo)
                         .Where(t => t.Date >= fromDate && t.Date <= toDate)
                         .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                         .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                         .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                         .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                          .Distinct()
                         .Count();
                    department.TotalDispatchToVerify = dbo.DispatchToDepartments
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                        .Count();
                    department.TotalDispatchGo = dbo.DispatchGoDepartments
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                        .Count();
                    department.TotalDispatchGoVerify = dbo.DispatchGoDepartments
                        .Where(t => t.IsVerify)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Where(t => securityLevelId == 0 || t.DispatchSecurityID == securityLevelId)
                        .Where(t => urgencyId == 0 || t.DispatchUrgencyID == urgencyId)
                        .Where(t => categoryId == 0 || t.DispatchCategoryID == categoryId)
                        .Where(t => methodId == 0 || t.DispatchMethodID == methodId)
                         .Distinct()
                        .Count();
                }
            }
            return department;
        }
        public static List<ChartModel> GenerateDataDispatchColumn(int id, int securityLevelId, int urgencyId, int categoryId, int methodId, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetDispatchByDepartment(id, securityLevelId, urgencyId, categoryId, methodId, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Công văn đến từ bên ngoài",
                    Data1 = obj.TotalDispatchTo,
                    Data2 = obj.TotalDispatchToVerify
                });
                data.Add(new ChartModel()
                {
                    Name = "Công văn đến trong nội bộ",
                    Data1 = obj.TotalDispatchGo,
                    Data2 = obj.TotalDispatchGoVerify
                });
            }
            return data;
        }

        #endregion
        #region Thống kê công văn theo cá nhân
        public class EmployeeStatisticalDispatchInfo
        {
            public string _AvatarUrl = "/Generic/File/LoadAvatarFile?employeeId={0}";
            public string _AvatarUrlDefault = "/Content/images/underfind.jpg";
            public int EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public System.DateTime? CreateAt { get; set; }
            public string Avatar
            {
                get
                {
                    if (EmployeeID == 0)
                        return _AvatarUrlDefault;
                    return string.Format(_AvatarUrl, EmployeeID);
                }
                set
                {
                    _AvatarUrl = value;
                }
            }
            public int TotalDispatchTo { get; set; }//Tổng công văn đến
            public int TotalDispatchToVerify { get; set; }//Tổng công văn đến đã xử lý
            public int TotalDispatchGo { get; set; }//Tổng công văn đi
            public int TotalDispatchGoVerify { get; set; }//Tổng công văn đi đã xử lý
            public int TotalDispatchGoFromEmployee { get; set; }//Tổng công văn đi của cá nhân
        }
        public static List<EmployeeStatisticalDispatchInfo> GetEmployeeStatisticalDispatch(ModelFilter filter, int departmentId, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            DateTime dateTotal = toDate.AddDays(1);
            var dbo = taskDA.Repository;
            var results = dbo.HumanRoles.Where(i => i.HumanDepartmentID == departmentId)
                        .SelectMany(i => i.HumanOrganizations)
                        .Select(i => i.HumanEmployee)
                        .Distinct()
                        .Select(item => new EmployeeStatisticalDispatchInfo()
                        {
                            EmployeeID = item.ID,
                            EmployeeName = item.Name,
                            CreateAt = item.CreateAt
                        })
                        .Distinct()
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            foreach (var employee in results)
            {
                employee.TotalDispatchTo = dbo.DispatchToEmployees
                        .Where(t => t.EmployeeID == employee.EmployeeID)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Count();
                employee.TotalDispatchToVerify = dbo.DispatchToEmployees
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => t.IsVerify)
                    .ToList()
                    .Select(t => t.DispatchTo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Count();
                employee.TotalDispatchGo = dbo.DispatchGoEmployees
                      .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Count();
                employee.TotalDispatchGoVerify = dbo.DispatchGoEmployees
                    .Where(t => t.EmployeeID == employee.EmployeeID)
                    .Where(t => t.IsVerify)
                    .Select(t => t.DispatchGo)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Count();
                employee.TotalDispatchGoFromEmployee = dbo.DispatchGoes
                    .Where(t => !t.IsEdit && t.ApprovalBy == employee.EmployeeID && t.IsAccept && t.IsMove)
                    .Where(t => t.Date >= fromDate && t.Date <= toDate)
                    .Count();
            }
            return results;
        }
        public static EmployeeStatisticalDispatchInfo GetDispatchByEmployee(int id, DateTime fromDate, DateTime toDate)
        {
            TaskDA taskDA = new TaskDA();
            var dbo = taskDA.Repository;
            var result = dbo.HumanEmployees
                        .Where(i => i.ID == id)
                        .Select(item => new EmployeeStatisticalDispatchInfo()
                        {
                            EmployeeID = item.ID,
                            EmployeeName = item.Name,
                            CreateAt = item.CreateAt
                        }).FirstOrDefault();
            if (result != null)
            {
                result.TotalDispatchTo = dbo.DispatchToEmployees
                        .Where(t => t.EmployeeID == result.EmployeeID)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Count();
                result.TotalDispatchToVerify = dbo.DispatchToEmployees
                        .Where(t => t.EmployeeID == result.EmployeeID)
                        .Where(t => t.IsVerify)
                        .ToList()
                        .Select(t => t.DispatchTo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Count();
                result.TotalDispatchGo = dbo.DispatchGoEmployees
                        .Where(t => t.EmployeeID == result.EmployeeID)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Count();
                result.TotalDispatchGoVerify = dbo.DispatchGoEmployees
                        .Where(t => t.EmployeeID == result.EmployeeID)
                        .Where(t => t.IsVerify)
                        .Select(t => t.DispatchGo)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Count();
                result.TotalDispatchGoFromEmployee = dbo.DispatchGoes
                          .Where(t => !t.IsEdit && t.ApprovalBy == result.EmployeeID && t.IsAccept && t.IsMove)
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .Count();
            }
            return result;

        }
        public static List<ChartModel> GenerateDataDispatchEmployeeColumn(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetDispatchByEmployee(id, fromDate, toDate);
            if (obj != null)
            {
                data.Add(new ChartModel()
                {
                    Name = "Công văn đến từ bên ngoài",
                    Data1 = obj.TotalDispatchTo,
                    Data2 = obj.TotalDispatchToVerify
                });
                data.Add(new ChartModel()
                {
                    Name = "Công văn đến trong nội bộ",
                    Data1 = obj.TotalDispatchGo,
                    Data2 = obj.TotalDispatchGoVerify
                });
            }
            return data;
        }

        #endregion
        #endregion
        #region Thống kê cho module quản lý phát triển sản phẩm mới
        #region Thống kê yêu cầu phát triển sản phẩm mới
        public class ProductDevelopmentStatisticalInfo
        {
            public int TotalRequest { get; set; }//Tổng yêu cầu
            public int TotalRequestPerfomed { get; set; }//Tổng yêu cầu đã thực hiện
            public int TotalRequestPending { get; set; }//Tổng yêu cầu chưa thực hiện
        }
        public static ProductDevelopmentStatisticalInfo GetProductDevelopmentStatistical(DateTime fromDate, DateTime toDate)
        {
            ProductDevelopmentRequirementDA productDevelopmentRequirementDA = new ProductDevelopmentRequirementDA();
            var dbo = productDevelopmentRequirementDA.Repository;
            ProductDevelopmentStatisticalInfo obj = new ProductDevelopmentStatisticalInfo();
            obj.TotalRequest = dbo.ProductDevelopmentRequirements
                .Where(t => t.CreateAt >= fromDate && t.CreateAt <= toDate)
                .Count();
            obj.TotalRequestPending = dbo.ProductDevelopmentRequirements
                .Where(t => t.CreateAt >= fromDate && t.CreateAt <= toDate)
                .Where(t => !t.IsWork)
                .Count();
            obj.TotalRequestPerfomed = dbo.ProductDevelopmentRequirements
                .Where(t => t.CreateAt >= fromDate && t.CreateAt <= toDate)
                .Where(t => t.IsWork)
                .Count();
            return obj;

        }
        public static List<ChartModel> GenerateDataProductDevelopment(DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetProductDevelopmentStatistical(fromDate, toDate);
            data.Add(new ChartModel()
            {
                Name = "Yêu cầu phát triển sản phẩm",
                Data1 = obj.TotalRequest,
                Data2 = obj.TotalRequestPending,
                Data3 = obj.TotalRequestPerfomed
            });
            return data;
        }
        public static List<ChartModel> GenerateDataProductDevelopmentPie(DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = GetProductDevelopmentStatistical(fromDate, toDate);
            data.Add(new ChartModel()
            {
                Name = "Tổng yêu cầu chưa thực hiện" + "(" + Math.Round((obj.TotalRequestPending != 0 ? Math.Floor(Math.Max((double)obj.TotalRequestPending * 100, obj.TotalRequest)) : 0) / obj.TotalRequest) + "%)",
                Data1 = obj.TotalRequestPending != 0 ? Math.Floor(Math.Max((double)obj.TotalRequestPending * 100, obj.TotalRequest)) : 0
            });
            data.Add(new ChartModel()
            {
                Name = "Tổng yêu cầu đã thực hiện" + "(" + Math.Round((obj.TotalRequestPerfomed != 0 ? Math.Floor(Math.Max((double)obj.TotalRequestPerfomed * 100, obj.TotalRequest)) : 0) / obj.TotalRequest) + "%)",
                Data1 = obj.TotalRequestPerfomed != 0 ? Math.Floor(Math.Max((double)obj.TotalRequestPerfomed * 100, obj.TotalRequest)) : 0
            });
            return data;
        }
        #endregion
        #endregion
    }
}