using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.API.Human;
using iDAS.Web.Areas.Department.Models.Title;
using iDAS.Web.Areas.Human.Model;
using iDAS.Web.Areas.Timekeeping.Models;
using System;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Timekeeping.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Đăng ký làm bên ngoài", IsActive = true, Icon = "GroupLink", IsShow = true, Position = 1)]
    public class WorkOutController : BaseController
    {
        private WorkOutAPI workOutAPI = new WorkOutAPI();
        private V3_HumanEmployeeAPI humanEmployeeAPI = new V3_HumanEmployeeAPI();

        #region View

        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }

        public Ext.Net.MVC.PartialViewResult IndexAll(string containerId)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                RenderMode = RenderMode.AddTo,
                ContainerId = containerId,
                ViewData = ViewData,
                ViewName = "IndexAll",
                WrapByScriptTag = false
                //check view
            };
        }

        public ActionResult Form(int id = 0)
        {
            WorkOutDTO model;
            if (id == 0)
            {
                ViewData.Add("Operation", "Create");
                model = new WorkOutDTO();
                model.EndTimeHour = "00";
                model.EndTimeMinitu = "00";
                model.StartTimeHour = "00";
                model.StartTimeMinitu = "00";
            }
            else
            {
                ViewData.Add("Operation", "Update");
                model = workOutAPI.GetById(id).Result.Data;
                model.ApprovedBy = model.ApprovedBy;
                model.Status = model.Status;
                model.EndTimeHour = model.EndTime.Hour < 10 ? "0" + model.EndTime.Hour.ToString() : model.EndTime.Hour.ToString();
                model.EndTimeMinitu = model.EndTime.Minute < 10 ? "0" + model.EndTime.Minute.ToString() : model.EndTime.Minute.ToString();
                model.StartTimeHour = model.StartTime.Hour < 10 ? "0" + model.StartTime.Hour.ToString() : model.StartTime.Hour.ToString();
                model.StartTimeMinitu = model.StartTime.Minute < 10 ? "0" + model.StartTime.Minute.ToString() : model.StartTime.Minute.ToString();
                V3_HumanEmployeeResponse item = humanEmployeeAPI.GetById(model.ApprovedBy).Result.Data;
                string strChucDanh = "";
                if (item.ListDepartmentTitle.Count > 0)
                {
                    foreach (DepartmentTitleResponse item1 in item.ListDepartmentTitle)
                    {
                        strChucDanh += item1.TitleName + " -- " + item1.DepartmentName + "</br>";
                    }
                }
                model.Perform = new Items.HumanEmployeeViewItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Role = strChucDanh,
                };
            }
            ViewData.Add("CurrentUserId", User.ID);
            return new Ext.Net.MVC.PartialViewResult { Model = model, ViewData = ViewData };
        }

        public ActionResult Detail(int id)
        {
            WorkOutDTO model;

            model = workOutAPI.GetById(id).Result.Data;
            model.ApprovedBy = model.ApprovedBy;
            model.Status = model.Status;
            model.EndTimeHour = model.EndTime.Hour < 10 ? "0" + model.EndTime.Hour.ToString() : model.EndTime.Hour.ToString();
            model.EndTimeMinitu = model.EndTime.Minute < 10 ? "0" + model.EndTime.Minute.ToString() : model.EndTime.Minute.ToString();
            model.StartTimeHour = model.StartTime.Hour < 10 ? "0" + model.StartTime.Hour.ToString() : model.StartTime.Hour.ToString();
            model.StartTimeMinitu = model.StartTime.Minute < 10 ? "0" + model.StartTime.Minute.ToString() : model.StartTime.Minute.ToString();
            V3_HumanEmployeeResponse item = humanEmployeeAPI.GetById(model.ApprovedBy).Result.Data;
            string strChucDanh = "";
            if (item.ListDepartmentTitle.Count > 0)
            {
                foreach (DepartmentTitleResponse item1 in item.ListDepartmentTitle)
                {
                    strChucDanh += item1.TitleName + " -- " + item1.DepartmentName + "</br>";
                }
            }
            model.Perform = new Items.HumanEmployeeViewItem()
            {
                ID = item.ID,
                Name = item.Name,
                Role = strChucDanh,
            };
            return new Ext.Net.MVC.PartialViewResult { Model = model, ViewData = ViewData };
        }

        #endregion View

        [ValidateInput(false)]
        public ActionResult GetData(StoreRequestParameters param, int Status = -1, string curMonth = default(string))
        {
            DateTime Month = DateTime.MinValue;
            if (!string.IsNullOrEmpty(curMonth))
            {
                Month = DateTime.Parse(curMonth);
            }
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            GetWorkOutByMonthRequest request = new GetWorkOutByMonthRequest()
            {
                EmployeeId = User.EmployeeID,
                PageNumber = param.Page,
                PageSize = param.Limit,
                Month = Month.Month,
                Year = Month.Year,
                Status = Status
            };
            var userId = User.ID;
            var dt = workOutAPI.GetByMonth(request);
            int totalRows = 0;
            totalRows = dt.Result.TotalRows.HasValue ? dt.Result.TotalRows.Value : 0;
            if (totalRows > 0) return this.Store(dt.Result.Data, totalRows);
            else return this.Store(null, 0);
        }

        public ActionResult GetDataForAllUsers(StoreRequestParameters param, int Status = -1, int month = 0, int year = 0)
        {
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            GetWorkOutByMonthRequest request = new GetWorkOutByMonthRequest()
            {
                PageNumber = param.Page,
                PageSize = param.Limit,
                Month = month,
                Year = year,
                Status = Status
            };
            var dt = workOutAPI.GetByMonth(request);
            int totalRows = 0;
            totalRows = dt.Result.TotalRows.HasValue ? dt.Result.TotalRows.Value : 0;

            if (totalRows > 0) return this.Store(dt, totalRows);
            else return this.Store(null, 0);
        }

        public ActionResult Create(WorkOutDTO item)
        {
            var result = false;
            var employeeId = User.EmployeeID;
            item.StartTime = new DateTime(item.StartTime.Year, item.StartTime.Month, item.StartTime.Day, Int32.Parse(item.StartTimeHour), Int32.Parse(item.StartTimeMinitu), 0);
            item.EndTime = new DateTime(item.EndTime.Year, item.EndTime.Month, item.EndTime.Day, Int32.Parse(item.EndTimeHour), Int32.Parse(item.EndTimeMinitu), 0);
            if (ModelState.IsValid)
            {
                try
                {
                    InsertWorkOutRequest request = new InsertWorkOutRequest()
                    {
                        EmployeeId = employeeId,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Address = item.Address,
                        Reason = item.Reason,
                        ApprovedBy = item.Perform.ID
                    };

                    var rs = workOutAPI.Insert(request);

                    if (!string.IsNullOrEmpty(rs))
                    {
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        //NotifyController notify = new NotifyController();
                        //notify.Notify(this.ModuleCode, "Có một đăng ký làm bên ngoài yêu cầu phê duyệt", User.Name, item.Perform.ID, User,"Human/WorkOut/", "DocumentID:" + id.ToString());
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateError);
                }
            }

            return this.Direct(result: result);
        }

        public ActionResult Update(WorkOutDTO item)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                try
                {
                    item.StartTime = new DateTime(item.StartTime.Year, item.StartTime.Month, item.StartTime.Day, Int32.Parse(item.StartTimeHour), Int32.Parse(item.StartTimeMinitu), 0);
                    item.EndTime = new DateTime(item.EndTime.Year, item.EndTime.Month, item.EndTime.Day, Int32.Parse(item.EndTimeHour), Int32.Parse(item.EndTimeMinitu), 0);
                    item.ApprovedBy = item.Perform.ID;
                    UpdateWorkOutRequest request = new UpdateWorkOutRequest()
                    {
                        Id = item.Id,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Address = item.Address,
                        Reason = item.Reason,
                        ApprovedBy = item.ApprovedBy
                    };
                    var rs = workOutAPI.Update(request);
                    if (!string.IsNullOrEmpty(rs))
                    {
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError);
                }
            }
            return this.Direct(result: result);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                try
                {
                    int rs = workOutAPI.Delete(id).Result.Data;
                    result = true;
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess, error: false);
                }
                catch (Exception)
                {
                    Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                }
            }
            return this.Direct(result: result);
        }

        //[HttpPost]
        //public ActionResult Approve(WorkOutDTO item)
        //{
        //    var result = false;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SolutionEntities"].ConnectionString))
        //            {
        //                if (connection.State != ConnectionState.Open)
        //                    connection.Open();

        //                using (var cmd = connection.CreateCommand())
        //                {
        //                    cmd.CommandText = "sp_EmployeeWorkOut_Approve";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.CommandTimeout = 200;
        //                    cmd.Parameters.AddWithValue("@id", item.Id);
        //                    cmd.Parameters.AddWithValue("@Status", item.Status);

        //                    cmd.ExecuteNonQuery();
        //                }
        //            }
        //            UpdateApproveState(item);
        //            result = true;

        //            //Send notify email
        //            var trueItem = EmployeeWorkOutService.GetById(item.Id);//lost many info, unknown reason
        //            var ApprovedBy = EmployeeService.GetById((Guid)trueItem.ApprovedBy);
        //            var SentBy = EmployeeService.GetById(trueItem.EmployeeId);
        //            var ApprovedByTitle = DepartmentTitleService.GetById(SentBy.Id);
        //            var targetEmail = SentBy.Email ?? "";
        //            var startDate = trueItem.StartTime.ToString("dd/MM/yyyy HH:mm");
        //            var endDate = trueItem.EndTime.ToString("dd/MM/yyyy HH:mm");
        //            if (!string.IsNullOrEmpty(targetEmail.Trim()))
        //            {
        //                var subject = item.Status == 1 ? Resource.AcceptWorkOutMailSubject : Resource.RejectWorkOutMailSubject;
        //                var body = string.Format(Resource.AcceptRejectWorkOutMailBody, SentBy.Name ?? "", startDate, endDate, trueItem.Reason, item.Status == 1 ? "approved" : "rejected", ApprovedBy.Name ?? "");
        //                body += string.Format(Resource.footer, ApprovedBy.Name ?? "", ApprovedByTitle.Name ?? "", ApprovedBy.Email ?? "");
        //                UserService.SaveMailQueue(targetEmail, subject, body, "", "");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            this.ShowNotify(Common.Resource.SystemFailure);
        //        }
        //    }
        //    return this.Direct(result: result);
        //}

        //public bool FormTracking(Guid id, out FormApproveStatusBO tracking)
        //{
        //    var stateList = FormApproveStatusService.GetByFormId(id);
        //    if (stateList.ToList().Count > 0)
        //    {
        //        tracking = stateList.OrderByDescending(u => u.CreateAt).First();
        //        return true;
        //    }
        //    tracking = new FormApproveStatusBO();
        //    return false;
        //}

        //public int GetCurrentStatus(Guid id)
        //{
        //    var tracking = FormTracking(id, out FormApproveStatusBO currentStatus);
        //    if (tracking && currentStatus.isFinal) return currentStatus.Status;
        //    else return 0;
        //}

        //public ActionResult UpdateApproveState(EmployeeWorkOutBO item, string forwardId = "")
        //{
        //    try
        //    {
        //        var tableName = "EmployeeWorkOuts";
        //        //cập nhật trạng thái cho bảng theo dõi trước
        //        var tracking = FormTracking(item.Id, out FormApproveStatusBO currentStatus);
        //        if (tracking)
        //        {
        //            currentStatus.Status = item.Status;
        //            currentStatus.SentBy = item.ApprovedBy;
        //            //nếu đồng ý hoặc từ chối đơn và ko chuyển cho ai khác thì đóng theo dõi
        //            if (item.Status != 0 && forwardId == "")
        //            {
        //                currentStatus.isFinal = true;
        //                FormApproveStatusService.Update(currentStatus);
        //                return this.Direct(true);
        //            }

        //            FormApproveStatusService.Update(currentStatus);
        //        }
        //        //thêm 1 record theo dõi mới
        //        var state = new FormApproveStatusBO()
        //        {
        //            Id = Guid.NewGuid(),
        //            FormId = item.Id,
        //            TableName = tableName,
        //            isFinal = (item.Status != 0 && forwardId == ""),
        //            Status = item.Status,
        //            SentBy = item.ApprovedBy ?? Guid.Empty,
        //            ApprovedBy = !string.IsNullOrEmpty(forwardId) && forwardId != Guid.Empty.ToString() ? new Guid(forwardId) : item.ApprovedBy,
        //            ApprovedAt = item.Status != 0 ? DateTime.Now : (DateTime?)null,
        //            Reason = item.Reason,
        //            CreateBy = item.EmployeeId,
        //            CreateAt = DateTime.Now
        //        };

        //        FormApproveStatusService.Insert(state);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return this.Direct(true);
        //}
    }
}