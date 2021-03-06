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


namespace iDAS.Web.Areas.ISO.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Bộ câu hỏi đánh giá", Icon = "Help", IsActive = true, IsShow = true, Position = 8)]
    public class CategoryQuestionController : BaseController
    {
        //
        // GET: /CenterAudit/CenterAuditCategoryQuestion/
        private CenterAuditCategoryQuestionSV CenterAuditCategoryQuestionSV = new CenterAuditCategoryQuestionSV();
        private CenterAuditQuestionSV CenterAuditQuestionSV = new CenterAuditQuestionSV();
        private CenterAuditAnswerSV CenterAuditAnswerSV = new CenterAuditAnswerSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public StoreResult GetData(string node, int naceCodeID)
        {
            var nodes = new NodeCollection();
            var lsDataNode = CenterAuditCategoryQuestionSV.GetTreeData(node, naceCodeID);
            foreach (var dataNode in lsDataNode)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Expanded = false;
                nodeItem.Text = dataNode.Name.ToUpper();
                nodeItem.Icon = Icon.Folder;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = dataNode.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = JSON.Serialize(dataNode.ID), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsUse", Value = JSON.Serialize(dataNode.IsUse), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Store(nodes);
        }
        #region Thêm mới bộ câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới")]
        public ActionResult FormAdd(int parentId, int nacodeId)
        {
            try
            {
                var data = new CenterAuditCategoryQuestionItem()
                {
                    ParentID = parentId,
                    ISONaceCodeID = nacodeId
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Insert(CenterAuditCategoryQuestionItem objNew)
        {
            try
            {
                objNew.CreateBy = User.ID;
                if (CenterAuditCategoryQuestionSV.Insert(objNew))
                {
                    X.GetCmp<FormPanel>("frCategoryQuestion").Reset();
                    X.GetCmp<Store>("stCategoryQuestion").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên bộ câu hỏi đánh giá đã tồn tại!"
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
        #region Cập nhật bộ câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        public ActionResult ShowUpdate(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = CenterAuditCategoryQuestionSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Update(CenterAuditCategoryQuestionItem objEdit)
        {
            try
            {
                if (CenterAuditCategoryQuestionSV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frCategoryQuestion").Reset();
                    X.GetCmp<Store>("stCategoryQuestion").Reload();
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
        #region Xem chi tiết bộ câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult ShowDetail(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = CenterAuditCategoryQuestionSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Xóa bộ câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (CenterAuditCategoryQuestionSV.Delete(id))
                {
                    X.GetCmp<Store>("stCategoryQuestion").Reload();
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
                Message = "Bộ câu hỏi đánh giá đã được sử dụng không được xóa!"
            });
            return this.Direct("Error");
        }
        #endregion
        public ActionResult GetQuestion(StoreRequestParameters parameters, int cateId = 0)
        {
            List<CenterAuditQuestionItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = CenterAuditQuestionSV.GetByCateId(filter, cateId);
            return this.Store(lstData, filter.PageTotal);
        }
        #region Thêm mới câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới câu hỏi")]
        public ActionResult FormAddQuestion(int cateId)
        {
            try
            {
                var data = new CenterAuditQuestionItem()
                {
                    CenterAuditCategoryQuestionID = cateId,
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CreateQuestion", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertQuestion(CenterAuditQuestionItem objNew)
        {
            try
            {
                objNew.CreateBy = User.ID;
                if (CenterAuditQuestionSV.Insert(objNew, User.ID) != 0)
                {
                    X.GetCmp<FormPanel>("frQuestion").Reset();
                    X.GetCmp<Store>("stQuestion").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên câu hỏi đánh giá đã tồn tại!"
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
        #region Cập nhật câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa câu hỏi")]
        public ActionResult ShowUpdateQuestion(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "EditQuestion", Model = CenterAuditQuestionSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateQuestion(CenterAuditQuestionItem objEdit)
        {
            try
            {
                if (CenterAuditQuestionSV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frQuestion").Reset();
                    X.GetCmp<Store>("stQuestion").Reload();
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
        #region Xem chi tiết câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết câu hỏi")]
        public ActionResult ShowDetailQuestion(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailQuestion", Model = CenterAuditQuestionSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Xóa bộ câu hỏi đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa câu hỏi")]
        public ActionResult DeleteQuestion(int id)
        {
            try
            {
                CenterAuditQuestionSV.Delete(id);
                X.GetCmp<Store>("stQuestion").Reload();
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
                    Message = "Câu hỏi đánh giá đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        #endregion
        public ActionResult LoadDataAnswer(StoreRequestParameters parameters, int questionId = 0)
        {
            List<CenterAuditAnswerItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = CenterAuditAnswerSV.GetByCateId(filter, questionId);
            return this.Store(lstData, filter.PageTotal);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách câu trả lời")]
        public ActionResult ShowAnswer(int questionId = 0)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Answers", ViewData = new ViewDataDictionary { { "QuestionID", questionId } } };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm trả lời")]
        public ActionResult InsertAnswer(string data, int questionId = 0)
        {
            try
            {
                var objItem = Ext.Net.JSON.Deserialize<CenterAuditAnswerItem>(data);
                objItem.CenterAuditQuestionID = questionId;
                objItem.CreateBy = User.ID;
                if (CenterAuditAnswerSV.Insert(objItem))
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("stAnswer").Reload();
                }
                else
                {
                    Ultility.ShowMessageBox(RequestMessage.Notify, RequestMessage.NameExist, MessageBox.Icon.ERROR, MessageBox.Button.OK);
                    X.GetCmp<Store>("stAnswer").Reload();
                }
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa câu trả lời")]
        public ActionResult UpdateAnswer(string data)
        {
            try
            {
                var objItem = Ext.Net.JSON.Deserialize<CenterAuditAnswerItem>(data);
                objItem.UpdateBy = User.ID;
                if (CenterAuditAnswerSV.Update(objItem))
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>("stAnswer").Reload();
                }
                else
                {
                    Ultility.ShowMessageBox(RequestMessage.Notify, RequestMessage.NameExist, MessageBox.Icon.ERROR, MessageBox.Button.OK);
                    X.GetCmp<Store>("stAnswer").Reload();
                }
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa câu trả lời")]
        public ActionResult DeleteAnswer(int id)
        {
            try
            {
                if (CenterAuditAnswerSV.Delete(id))
                {
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("stAnswer").Reload();
                }
                else
                {
                    Ultility.ShowMessageBox(RequestMessage.Notify, RequestMessage.DataUsing, MessageBox.Icon.ERROR, MessageBox.Button.OK);
                }
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết câu trả lời")]
        public ActionResult ShowFrmDetailAnswer(int id = 0)
        {
            if (id > 0)
            {
                var obj = CenterAuditAnswerSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return null;
        }
    }
}
