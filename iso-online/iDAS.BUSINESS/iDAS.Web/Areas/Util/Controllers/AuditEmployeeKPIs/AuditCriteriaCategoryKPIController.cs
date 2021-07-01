using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Util.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Bộ tiêu chí đánh giá mẫu", Icon = "PageGear", IsActive = true, IsShow = true, Position = 1, Parent = GroupAuditKPIController.Code)]
    public class AuditCriteriaCategoryKPIController : BaseController
    {
        //
        // GET: /Human/AuditCriteriaKPI/
        private HumanCriteriaCategorySV humanCategoryCriteriaSV = new HumanCriteriaCategorySV();
        private HumanCriteriaSV humanCriteriaSV = new HumanCriteriaSV();
        private HumanCriteriaPointSV humanAnswerSV = new HumanCriteriaPointSV();
        private PermissionSV permissionSV = new PermissionSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDataPoint(StoreRequestParameters parameters, int criteriaId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditGradationCriteriaPointItem> lstData;
            lstData = humanAnswerSV.GetByCateId(filter, criteriaId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditCriteriaCategoryItem> lstData;
            lstData = humanCategoryCriteriaSV.GetData(filter);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataCoppy(StoreRequestParameters parameters, int cateId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditCriteriaCategoryItem> lstData;
            lstData = humanCategoryCriteriaSV.GetDataCoppy(filter, cateId);
            return this.Store(lstData, filter.PageTotal);
        }
        #region Thêm mới bộ tiêu chí
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới")]
        public ActionResult FormAdd()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create" };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Insert(HumanAuditCriteriaCategoryItem objNew)
        {
            try
            {
                objNew.CreateBy = User.ID;
                if (humanCategoryCriteriaSV.Insert(objNew))
                {
                    X.GetCmp<FormPanel>("frCategoryCriteria").Reset();
                    X.GetCmp<Store>("stCategoryCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên bộ tiêu chí đánh giá KPIs đã tồn tại!"
                    });
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            return this.Direct();
        }
        #endregion
        #region Cập nhật bộ tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        public ActionResult ShowUpdate(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = humanCategoryCriteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Update(HumanAuditCriteriaCategoryItem objEdit)
        {
            try
            {
                if (humanCategoryCriteriaSV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frCategoryCriteria").Reset();
                    X.GetCmp<Store>("stCategoryCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        #endregion
        #region Xem chi tiết bộ tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult ShowDetail(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = humanCategoryCriteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Xóa bộ tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (humanCategoryCriteriaSV.Delete(id))
                {
                    X.GetCmp<Store>("stCategoryCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    return this.Direct();
                }
            }
            catch { }
            X.Msg.Show(new MessageBoxConfig
            {
                Buttons = MessageBox.Button.OK,
                Icon = MessageBox.Icon.ERROR,
                Title = "Thông báo",
                Message = "Bộ tiêu chí đánh giá đã được sử dụng không được xóa!"
            });
            return this.Direct("Error");
        }
        #endregion
        public ActionResult GetCriteria(StoreRequestParameters parameters, int cateId = 0)
        {
            List<HumanAuditCriteriaItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = humanCriteriaSV.GetByCateId(filter, cateId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetCriteriaByListID(StoreRequestParameters parameters, string stringId)
        {
            List<HumanAuditCriteriaItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = humanCriteriaSV.GetByListCateId(filter, stringId);
            return this.Store(lstData, filter.PageTotal);
        }
        #region Thêm mới tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới tiêu chí")]
        public ActionResult FormAddCriteria(int cateId)
        {
            try
            {
                var data = new HumanAuditCriteriaItem()
                {
                    HumanAuditCriteriaCategoryID = cateId,
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CreateCriteria", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertCriteria(HumanAuditCriteriaItem objNew, string strPoint)
        {
            try
            {
                List<HumanAuditGradationCriteriaPointItem> lstPointItem = new List<HumanAuditGradationCriteriaPointItem>();
                lstPointItem = Ext.Net.JSON.Deserialize<HumanAuditGradationCriteriaPointItem[]>(strPoint).ToList();
                if (lstPointItem.Count > 0)
                {
                    if (lstPointItem.FirstOrDefault(x => string.IsNullOrEmpty(x.Name.Trim())) != null)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên điểm đánh giá không được để trống!"
                        });
                        return this.Direct(false);
                    }
                    else if(lstPointItem.Select(t=>t.Name.Trim().ToUpper()).Distinct().Count()<lstPointItem.Count())
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên điểm đánh giá trùng vui lòng kiểm tra lại!"
                        });
                        return this.Direct(false);
                    }
                }
                if (humanCriteriaSV.Insert(objNew, User.ID, lstPointItem) != 0)
                {
                    X.GetCmp<FormPanel>("frCriteria").Reset();
                    X.GetCmp<Store>("stCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct(true);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên tiêu chí đánh giá đã tồn tại!"
                    });
                    return this.Direct(false);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError);
                return this.Direct(false);
            }

        }
        #endregion
        #region Cập nhật tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa tiêu chí")]
        public ActionResult ShowUpdateCriteria(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "EditCriteria", Model = humanCriteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult ShowRole(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "AddAuditCriteriaRole", Model = humanCriteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult LoadDataPermission(string code, int departmentId, int CritId, bool isModule = false, bool isGroup = false, bool isFunction = false)
        {
            var data = permissionSV.GetAuditCri_Factory(code, departmentId, isModule, isGroup, isFunction, CritId);
            return this.Store(data);
        }
        public ActionResult UpdateCriteria(HumanAuditCriteriaItem objEdit, string strPoint)
        {
            try
            {
                List<HumanAuditGradationCriteriaPointItem> lstPointItem = new List<HumanAuditGradationCriteriaPointItem>();
                lstPointItem = Ext.Net.JSON.Deserialize<HumanAuditGradationCriteriaPointItem[]>(strPoint).ToList();
                if (lstPointItem.Count > 0)
                {
                    if (lstPointItem.FirstOrDefault(x => string.IsNullOrEmpty(x.Name.Trim())) != null)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên điểm đánh giá không được để trống!"
                        });
                        return this.Direct(false);
                    }
                }
                if (humanCriteriaSV.Update(objEdit, User.ID, lstPointItem))
                {
                    X.GetCmp<FormPanel>("frCriteria").Reset();
                    X.GetCmp<Store>("stCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct(true);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                    return this.Direct(false);
                }

            }
            catch
            {
                return this.Direct(false);
            }
        }
        #endregion
        #region Xem chi tiết tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết tiêu chí")]
        public ActionResult ShowDetailCriteria(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailCriteria", Model = humanCriteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Xóa bộ tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa tiêu chí")]
        public ActionResult DeleteCriteria(int id)
        {
            try
            {
                humanCriteriaSV.Delete(id);
                X.GetCmp<Store>("stCriteria").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Tiêu chí đánh giá đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        #endregion
        #region Xóa điểm đánh giá
        public ActionResult DeletePoint(string stringId)
        {
            try
            {
                humanAnswerSV.Delete(stringId);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }
        #endregion
        public DirectResult HandleChanges(int roleId, int critId, int factory)
        {
            //new HumanCriteriaFactorySV().UpdateFactory(roleId, critId, factory);
            //X.GetCmp<Store>("StorePermissions").Reload();
            return this.Direct();
        }
        public ActionResult ShowCoppy(int id = 0)
        {
            try
            {
                ViewData["CateID"] = id;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CoppyData", ViewData = ViewData };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult CoppyData(int id = 0, int cateId = 0)
        {
            try
            {
                humanCriteriaSV.CoppyData(id, cateId, User.ID);
                X.GetCmp<Store>("stCriteria").Reload();
                X.GetCmp<Window>("winCoppyDataCriteria").Close();
            }
            catch (Exception ex)
            {
                
                 X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Đã xảy ra lỗi trong quá trình coppy dữ liệu!"
                    });
            }
            return this.Direct();
        }
    }
}
