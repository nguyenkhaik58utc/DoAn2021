using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items.Problem;
using iDAS.Utilities;
using iDAS.Web.API.Problem;
using System;
using System.Web.Mvc;
using iDAS.Web.Areas.Department.Models.TitleMenuRoleV3;

namespace iDAS.Web.Areas.Problem.Controllers
{
    public class ProblemTypeController : Controller
    {
        ProblemGroupAPI groupAPI = new ProblemGroupAPI();
        ProblemTypeAPI api = new ProblemTypeAPI();
        // GET: Problem/ProblemType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            try
            {
                int total = 0;
                var resp = api.GetProblemTypeAPI(parameters.Page, parameters.Limit).Result;

                var data = resp.lstProblemType;
                foreach(ProblemTypeItem item in data)
                {
                    if (item.IsDefault)
                        item.textDefault = "Mặc định";
                    else
                        item.textDefault = "";
                }
                total = resp.total;
                return this.Store(data, total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // lấy danh sách Loại sự cố theo nhóm sự cố
        public ActionResult LoadData2(StoreRequestParameters parameters,int ID)
        {
            try
            {
                int total = 0;
                var resp = api.GetProblemTypeAPI(parameters.Page, parameters.Limit).Result;
                var data = resp.lstProblemType;
                var lstTyleById = api.GetListTypeById(ID).Result;
                
                foreach (ProblemTypeItem item in data)
                {
                    if (item.IsDefault)
                        item.textDefault = "Mặc định";
                    else
                        item.textDefault = "";
                    for (int i = 0; i < lstTyleById.lstProblemType.Count; i++)
                    {
                        if(item.ID == lstTyleById.lstProblemType[i].ID)
                        {
                            item.IsSelect = true;
                            break;
                        }
                    }
                }
                total = resp.total;
                return this.Store(data, total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Create
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Create()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(ProblemTypeItem problemTypeItem)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    api.Create(problemTypeItem);
                    Ultility.ShowMessageRequest(RequestStatus.CreateSuccess);
                    success = true;
                }
                else
                {
                    Ultility.ShowMessageRequest(RequestStatus.ValidError);
                    success = false;
                }
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Update
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Update(int ProblemTypeId)
        {
            try
            {
                var resp = api.GetProblemTypeById(ProblemTypeId).Result;
                var data = resp.lstProblemType[0];
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(ProblemTypeItem problemTypeItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Update(problemTypeItem);
                    X.GetCmp<Store>("StoreProblemType").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
        #endregion

        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int ProblemTypeId = 0)
        {
            try
            {
                var delete = api.delete(ProblemTypeId);
                if (delete.Result != 0)
                    Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
                else
                {
                    Ultility.ShowMessageBox("Không thành công", "Loại sự cố đang được sử dụng", MessageBox.Icon.ERROR);
                }
                return this.Direct();
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.DeleteError);
            }
            return this.Direct();
        }
        #endregion


        #region Defalt
        [BaseSystem(Name = "Mặc định")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Default()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Default(int ProblemTypeId = 0)
        {
            try
            {
                var delete = api.Default(ProblemTypeId);
                Ultility.ShowMessageRequest(RequestStatus.UpdateSuccess);
                return this.Direct();
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.UpdateSuccess);
            }
            return this.Direct();
        }
        #endregion

        #region Detail
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Detail(int ProblemTypeId)
        {
            try
            {
                var resp = api.GetProblemTypeById(ProblemTypeId).Result;
                var data = resp.lstProblemType[0];
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion

    }
}