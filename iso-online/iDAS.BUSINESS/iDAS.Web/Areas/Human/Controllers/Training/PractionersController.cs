using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Đăng ký", Icon = "ApplicationFormAdd", IsActive = true, IsShow = true, Position = 4, IsGroup = false, Parent = GroupTrainingController.Code)]
    public class PractionersController : BaseController
    {
        private HumanTrainingPractionersSV practionersSevice = new HumanTrainingPractionersSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadDataPlan(StoreRequestParameters parameters)
        {
            List<HumanTrainingPlanItem> lst;
            int total;
            lst = practionersSevice.GetPlanIsAccept(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<HumanTrainingPlanItem>(lst, total));
        }
        public ActionResult GetDataPlanDetail(StoreRequestParameters parameters, int planId = 0)
        {
            List<HumanTrainingPlanDetailItem> lst;
            int total;
            lst = practionersSevice.GetPlanDetail(parameters.Page, parameters.Limit, out total, planId);
            return this.Store(new Paging<HumanTrainingPlanDetailItem>(lst, total));
        }
        public ActionResult GetDataPlanDetailResult(StoreRequestParameters parameters, int planId = 0)
        {
            List<HumanTrainingPlanDetailItem> lst;
            int total;
            lst = practionersSevice.GetAllForResultTraining(parameters.Page, parameters.Limit, out total, planId);
            return this.Store(new Paging<HumanTrainingPlanDetailItem>(lst, total));
        }
        public ActionResult GetData(StoreRequestParameters parameters, int detailId = 0)
        {
            List<HumanTrainingPractionersItem> lst;
            int total;
            lst = practionersSevice.GetAll(parameters.Page, parameters.Limit, out total, detailId);
            return this.Store(new Paging<HumanTrainingPractionersItem>(lst, total));
        }
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AddForm(int detailId = 0)
        {
            HumanTrainingPlanDetailItem detail = practionersSevice.GetPlanDetail(detailId);
            var data = new HumanTrainingPractionersItem();
            data.ContentCommit = detail.CommitContent;
            data.IsCommit = (bool)detail.IsCommit;
            data.DetailID = detail.ID;
            data.Number = detail.Number;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }
        public ActionResult InsertListObject(string stringId, int detailId = 0)
        {
            try
            {
                practionersSevice.InsertObject(stringId, User.ID, detailId);
                X.GetCmp<Store>("StorePractioners").Reload();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
        /// <summary>
        /// Hàm insert dữ liệu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Đăng ký tham gia khóa đào tạo", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        [ValidateInput(false)]
        public ActionResult Insert(HumanTrainingPractionersItem objNew)
        {
            try
            {
                if (objNew.EmployeesRegister.ID == 0)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO,
                        Title = "Thông báo",
                        Message = "Bạn phải chọn ứng viên tham gia khóa đào tạo!"
                    });
                    return this.Direct(false);
                }
                else if (!objNew.IsAcceptCommit)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO,
                        Title = "Thông báo",
                        Message = "Để tham gia đợt đào tạo bạn phải đồng ý với cam kết trên!"
                    });
                    return this.Direct(false);
                }
                else if (practionersSevice.CheckInvalidEmployees(objNew.EmployeesRegister.ID, objNew.DetailID))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO,
                        Title = "Thông báo",
                        Message = "Học viên đã đăng ký đợt đào tạo vui lòng chọn học viên khác!"
                    });
                    return this.Direct(false);
                }
                else if (practionersSevice.CheckOverNumber(objNew.DetailID, objNew.Number))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO,
                        Title = "Thông báo",
                        Message = "Số lượng đăng ký đã hết vui lòng chờ đợt đào tạo lần sau!"
                    });
                    return this.Direct(false);
                }
                else
                {
                    practionersSevice.Insert(objNew, User.ID, User.EmployeeID);
                    X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!").Show();
                    return this.Direct(true);
                }
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
        }

        #region Xóa
        /// <summary>
        /// Xóa học viên đăng ký đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeletePractioners(int id)
        {
            try
            {
                practionersSevice.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePractioners").Reload();
            }
            return this.Direct();
        }
        #endregion
    }
}
