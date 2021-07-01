using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;


namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Danh sách đề nghị", IsActive = true, IsShow = false, Position = 2, Parent = GroupTrainingController.Code)]
    public class TrainingRequirementListController : BaseController
    {
        private HumanTrainingRequirementsListSV trainingRequirementListSV = new HumanTrainingRequirementsListSV();

        #region Thông tin chung

        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int id)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Index", Model = new HumanTrainingRequirementItem() { ID = id } };
        }

        public ActionResult LoadRequirementList(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            return this.Store(trainingRequirementListSV.GetByRequirement(parameters.Page, parameters.Limit, out totalCount, id), totalCount);
        }

        #endregion

        #region Thêm
        [BaseSystem(Name = "Thêm mới")]
        public ActionResult AddForm(int id)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Add",
                Model = new HumanTrainingRequirementListItem() { RequirementID = id }
            };
        }
        public ActionResult Insert(HumanTrainingRequirementListItem updateData)
        {
            try
            {
                trainingRequirementListSV.Insert(updateData, User.ID);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            finally
            {

                X.GetCmp<Window>("winRequirementListAdd").Close();
                X.GetCmp<Store>("StoreRequirementList").Reload();
            }
            return this.Direct();
        }
        public ActionResult InsertMultiple(string employeeIds,int id)
        {
            try
            {
                trainingRequirementListSV.InsertMulti(employeeIds, id, User.ID);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            finally
            {
                X.GetCmp<Store>("StoreRequirementList").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Sửa
        [BaseSystem(Name = "Sửa")]
        public ActionResult UpdateForm(int ID = 0, string ActionForm = "")
        {
            var data = new HumanTrainingRequirementListItem();
            if (ID != 0)
            {
                data = trainingRequirementListSV.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };

        }

        public ActionResult Update(HumanTrainingRequirementListItem updateData)
        {
            try
            {
                if (updateData.ID != 0)
                {
                    trainingRequirementListSV.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winRequirementListUpdate").Close();
                X.GetCmp<Store>("StoreRequirementList").Reload();

            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteRequirement(int id)
        {
            try
            {

                trainingRequirementListSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirementList").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int ID)
        {
            var data = trainingRequirementListSV.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}
