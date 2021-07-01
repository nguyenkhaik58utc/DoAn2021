using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize =false)]
    [BaseSystem(Name = "Quản lý khảo sát", Icon = "PageEdit", IsActive = true, IsShow = true, Position = 1, Parent = GroupCommonController.Code)]
    public class CustomerSurveyController : BaseController
    {
        private CustomerSurveySV CustomerSurveyService = new CustomerSurveySV();
        private CustomerSurveyObjectSV CustomerSurveyObjectService = new CustomerSurveyObjectSV();
        private CustomerSurveyQuestionSV CustomerSurveyQuestionService = new CustomerSurveyQuestionSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy danh sách 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerSurvey(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            return this.Store(CustomerSurveyService.GetAll(filter), filter.PageTotal);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0)
        {
            var data = new CustomerSurveyItem();
            data.ActionForm = Form.Add;
            if (ID != 0)
            {
                data = CustomerSurveyService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerSurveyItem data)
        {
            try
            {
                data.IsPause = data.StatusValue == CustomerStatus.Pause ? true : false;
                data.IsFinish = data.StatusValue == CustomerStatus.Finish ? true : false;
                if (data.ID != 0)
                {
                    CustomerSurveyService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerSurveyService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winUpdateSurvey").Close();
                X.GetCmp<Store>("StoreCustomerSurvey").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerSurveyService.Delete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerSurvey").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerSurveyItem();
            if (ID != 0)
            {
                data = CustomerSurveyService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Đối tượng khảo sát
        [BaseSystem(Name = "Thiết lập đối tượng khảo sát")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ObjectForm(int ID = 0)
        {
            var data = new CustomerSurveyItem();
            data.ID = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Object", Model = data };
        }
        /// <summary>
        /// Lấy danh sách đối tượng khách hàng được chăm sóc
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerSurveyObject(StoreRequestParameters parameters, int surveyID = 0)
        {
            int totalCount;
            var result = CustomerSurveyObjectService.GetAllSurveyObject(parameters.Page, parameters.Limit, out totalCount, surveyID);
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateCustomerSurveyObject(string data)
        {
            var CustomerSurveyObjectItem = Ext.Net.JSON.Deserialize<CustomerSurveyObjectItem>(data);
            try
            {
                if (CustomerSurveyObjectItem.ID == null && CustomerSurveyObjectItem.IsSelect == true)
                {
                    CustomerSurveyObjectService.Insert(CustomerSurveyObjectItem, User.ID);

                }
                if (CustomerSurveyObjectItem.ID != null && CustomerSurveyObjectItem.IsSelect == false)
                {
                    CustomerSurveyObjectService.Delete((int)CustomerSurveyObjectItem.ID);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            X.GetCmp<Store>("storeCustomerSurveyObject").Reload();
            return this.Direct();
        }
        #endregion

        #region Câu hỏi khảo sát
        [BaseSystem(Name = "Thiết lập câu hỏi khảo sát")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult QuestionForm(int ID = 0)
        {
            var data = new CustomerSurveyItem();
            data.ID = ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Question", Model = data };
        }
        /// <summary>
        /// Danh sách câu hỏi khảo sát
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public ActionResult LoadQuestion(string node, int surveyID = 0)
        {
            NodeCollection nodes = new NodeCollection();
            int parentId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var groups = CustomerSurveyQuestionService.GetByParentAndSurvey(parentId, surveyID);
            foreach (var group in groups)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = group.ID.ToString();
                nodeItem.Text = group.Name;
                nodeItem.Leaf = group.Leaf;
                nodeItem.Icon = Icon.Folder;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = group.ID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "SurveyID", Value = group.SurveyID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = group.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = group.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsCategory", Value = JSON.Serialize(group.IsCategory), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsUse", Value = JSON.Serialize(group.IsUse), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Store(nodes);
        }

        public ActionResult LoadAnswer(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            return this.Store(CustomerSurveyQuestionService.GetAnswer(parameters.Page, parameters.Limit, out totalCount, id), totalCount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="parentID"></param>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        public ActionResult QuestionUpdateForm(int ID = 0, int parentID = 0, int surveyID = 0)
        {
            var data = new CustomerSurveyQuestionItem();
            if (ID != 0)
            {
                data = CustomerSurveyQuestionService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
                data.ParentID = parentID;
                data.SurveyID = surveyID;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "QuestionUpdate", Model = data };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult QuestionUpdateItem(CustomerSurveyQuestionItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerSurveyQuestionService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerSurveyQuestionService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winSurveyQuestion").Close();
            }
            return this.Direct();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult UpdateAnswer(string data)
        {
            try
            {
                CustomerSurveyQuestionItem updateData = new CustomerSurveyQuestionItem();
                updateData = Ext.Net.JSON.Deserialize<CustomerSurveyQuestionItem>(data);
                updateData.UpdateBy = User.ID;
                CustomerSurveyQuestionService.UpdateAnswer(updateData);
                X.GetCmp<Store>("StoreAnswer").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult DeleteQuestion(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerSurveyQuestionService.Delete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreTreeQuestion").GetById(ID).Destroy();
            }
            return this.Direct();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="parentID"></param>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        public ActionResult QuestionDetailForm(int ID = 0, int parentID = 0, int surveyID = 0)
        {
            var data = new CustomerSurveyQuestionItem();
            if (ID != 0)
            {
                data = CustomerSurveyQuestionService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "QuestionDetail", Model = data };
        }

        public ActionResult AddAnswerForm(int parentId = 0, int surveyID = 0)
        {
            var data = new CustomerSurveyQuestionItem()
            {
                ParentID = parentId,
                IsCategory = false,
                IsUse = true,
                SurveyID = surveyID
            };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Answer", Model = data };
        }

        public ActionResult UpdateAnswerItem(CustomerSurveyQuestionItem data)
        {
            try
            {
                CustomerSurveyQuestionService.Insert(data, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winSurveyAnswer").Close();
            }
            return this.Direct();
        }
        #endregion

        #region Gửi mail cho khách hàng
        [BaseSystem(Name = "Gửi Email khảo sát")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult SendMailSurvey(int id)
        {
            try
            {
                string request = Request.Url.Authority;
                CustomerSurveyService.SendMailSurvey(id, 10, request);
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.Direct();
        }
        #endregion
    }
}
