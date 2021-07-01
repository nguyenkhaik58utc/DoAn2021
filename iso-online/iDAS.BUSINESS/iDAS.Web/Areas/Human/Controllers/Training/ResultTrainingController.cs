using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Kết quả", Icon = "ApplicationSideList", IsActive = true, IsShow = true, Position = 5, IsGroup = false, Parent = GroupTrainingController.Code)]
    public class ResultTrainingController : BaseController
    {
        private HumanProfileTrainingSV profileTrainingSV = new HumanProfileTrainingSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id)
        {
           var obj = profileTrainingSV.GetPractioner(id);
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateResult", Model = obj };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateFormJoin(int id)
        {
            var obj = profileTrainingSV.GetPractioner(id);
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateJoin", Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Update(HumanTrainingPractionersItem objEdit)
        {

            try
            {
                profileTrainingSV.UpdatePractioner(objEdit, User.ID);
                return this.Direct();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertToProfile(int id)
        {

            try
            {
                var obj = profileTrainingSV.GetPractioner(id);
                HumanProfileTrainingItem item = new HumanProfileTrainingItem();
                item.Certificate = obj.Certificate;
                item.Content = obj.Content;
                item.EmployeeID = obj.EmployeesID;
                item.EndDate = obj.EndDate;
                item.StartDate = obj.StartDate;
                item.Result = obj.RankID == 1 ? "Giỏi" : obj.RankID == 2 ? "Khá" : obj.RankID == 3 ? "Trung bình" : obj.RankID == 4 ? "Yếu" : obj.RankID == 5 ? "Kém" : string.Empty;
                item.Name = obj.Content;
                item.Form = obj.FormName;
                item.Reviews = obj.CommentOfTeacher;           
                profileTrainingSV.Insert(item, User.ID);
                profileTrainingSV.UpdateIsInProile(id);
                X.GetCmp<Store>("StorePractioners").Reload();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateJoin(HumanTrainingPractionersItem objEdit)
        {
            try
            {
                profileTrainingSV.UpdateJoin(objEdit, User.ID);
                return this.Direct();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

    }
}
