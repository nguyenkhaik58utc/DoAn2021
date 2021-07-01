using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.DevelopmentProduct.Controllers
{
    [BaseSystem(Name = "Thực hiện kế hoạch", Icon = "RubyGo", IsActive = true, IsShow = true, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class PerformPlanController : BaseController
    {
        //
        // GET: /DevelopmentProduct/PerformPlan/
        private QualityPlanTaskSV planTaskSV = new QualityPlanTaskSV();
        private DocumentSV documentSV = new DocumentSV();
        private ProductionRequirementSV productionRequirementSV = new ProductionRequirementSV();
        private ProductDevelopmentRequirementPlanDocumentSV productDevelopmentRequirementPlanDocumentSV = new ProductDevelopmentRequirementPlanDocumentSV();
        private ProductDevelopmentRequirementSV productDevelopmentRequirementSV = new ProductDevelopmentRequirementSV();
        private ProductDevelopmentRequirementPlanSV productDevelopmentRequirementPlanSV = new ProductDevelopmentRequirementPlanSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int productDevelopmentRequirementPlanID)
        {
            int totalCount;
            var lst = productDevelopmentRequirementPlanSV.GetDocument(parameters.Page, parameters.Limit, out totalCount, productDevelopmentRequirementPlanID);
            return this.Store(lst, totalCount);
        }
        private Node createNodeTask(TaskItem dataNode, int planId)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            //if (dataNode. != planId)
            //{
            //    nodeItem.Cls = "clsUnView";
            //}
            nodeItem.Text = !dataNode.Leaf ? dataNode.Name.ToUpper() : dataNode.Name;
            nodeItem.Icon = !dataNode.Leaf ? Icon.Folder : Icon.TagBlue;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Rate", Value = JSON.Serialize(dataNode.Rate), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsNew", Value = JSON.Serialize(dataNode.IsNew), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsEdit", Value = JSON.Serialize(dataNode.IsEdit), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsComplete", Value = JSON.Serialize(dataNode.IsComplete), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsPause", Value = JSON.Serialize(dataNode.IsPause), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsPause", Value = JSON.Serialize(dataNode.IsPause), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CategoryName", Value = dataNode.CategoryName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Status", Value = dataNode.Status.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelName", Value = dataNode.LevelName.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "LevelColor", Value = dataNode.LevelColor.ToString(), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "StartTime", Value = dataNode.StartTime.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "EndTime", Value = dataNode.EndTime.ToString("dd/MM/yyyy HH:mm"), Mode = ParameterMode.Value });
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        public ActionResult LoadTasks(string node, int planId = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var taskId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var tasks = planTaskSV.GetTreeTask(taskId, planId);
                foreach (var task in tasks)
                {
                    nodes.Add(createNodeTask(task, planId));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        //public ActionResult LoadTasks(StoreRequestParameters parameters, int planId = 0)
        //{
        //    ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
        //    var lst = planTaskSV.GetAll(filter, planId);
        //    return this.Store(lst, filter.PageTotal);
           
        //}
        public ActionResult GetDataRequirment(StoreRequestParameters parameters)
        {
            List<ProductDevelopmentRequirementItem> lstData;
            int total;
            lstData = productDevelopmentRequirementSV.GetAllPerform(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<ProductDevelopmentRequirementItem>(lstData, total));
        }
        public ActionResult GetDataIsApproval(StoreRequestParameters parameters, int requirmentId = 0)
        {
            List<ProductDevelopmentRequirementPlanItem> lstData;
            int total;
            lstData = productDevelopmentRequirementPlanSV.GetIsApproval(parameters.Page, parameters.Limit, out total, requirmentId);
            return this.Store(new Paging<ProductDevelopmentRequirementPlanItem>(lstData, total));
        }
        [BaseSystem(Name = "Quản lý tài liệu thiết kế sản phẩm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DocumentProduction(int productDevelopmentRequirementPlanID = 0)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DocumentProduction", ViewData = new ViewDataDictionary { { "productDevelopmentRequirementPlanID", productDevelopmentRequirementPlanID }, { "UserLogOn", User.EmployeeID } } };
        }
        public ActionResult Insert(int productDevelopmentRequirementPlanID)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertDocument", ViewData = new ViewDataDictionary { { "productDevelopmentRequirementPlanID", productDevelopmentRequirementPlanID } } };
        }
        public ActionResult Modified(int id, int productDevelopmentRequirementPlanID)
        {
            var obj = documentSV.GetByID(id);
            if (obj.FlagModified == true)
            {
                Ultility.ShowMessageBox("Thông báo", "Tài liệu ban hành được chọn đã có tài liệu sửa đổi!", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                return this.Direct();
            }
            if (obj.IssuedTime != null)
                obj.IssuedTime = (int)obj.IssuedTime + 1;
            obj.IsAccept = false;
            obj.IsEdit = true;
            obj.IsApproval = false;
            obj.ProductDevelopmentRequirementPlanID = productDevelopmentRequirementPlanID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ModifiedDocument", Model = obj };
        }
        private int setDocIssForm(string[] doc, ref DocumentItem obj)
        {
            if (doc != null)
            {
                if (doc.Count() == 2)
                {
                    obj.FormH = true;
                    obj.FormS = true;
                    return (int)StorageForm.Both;
                }
                else
                {
                    if (doc[0].ToString().Equals(StorageForm.SoftCopy.ToString()))
                    {

                        obj.FormS = true;
                        return (int)StorageForm.SoftCopy;
                    }

                    else
                    {
                        obj.FormH = true;
                        return (int)StorageForm.HardCopy;
                    }

                }

            }
            return -1;

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertDocument(DocumentItem obj, [Bind(Prefix = "StorageFormID")]string[] borders, bool IsSendApproval = false)
        {
            try
            {
                setDocIssForm(borders, ref obj);
                obj.CreateBy = User.ID;
                obj.IsEdit = !IsSendApproval;
                int id = insert(obj);
                productDevelopmentRequirementPlanDocumentSV.Insert(obj.ProductDevelopmentRequirementPlanID, id, User.ID);
                if (id > 0)
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("StDocument").Reload();
                    X.GetCmp<Hidden>("hdID").Text = id.ToString();
                    X.GetCmp<Window>("winAddDocument").Close();
                }
            }
            catch (Exception)
            {
                return this.Direct();
            }
            return this.Direct();
        }
        private string checkValid(DocumentItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Code = objDraft.Code.Trim();
            objDraft.Version = objDraft.Version.Trim();
            if (objDraft.ParentID > 0)
                return "";
            if (id > 0)
            {
                var docItem = documentSV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Code.Trim().ToUpper().Equals(objDraft.Code.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = documentSV.GetByCodeVerson(objDraft.Code);
                if (doc != null)
                {
                    return "Mã Tài liệu đã tồn tại trong hệ thống! Vui lòng nhập mã khác.";
                }
            }
            if (CheckValidation.HasSpecialCharacters(objDraft.Code.Trim(), "@#$%^&*()~"))
                return "Mã Tài liệu không được chứa ký tự đặc biệt! Vui lòng nhập mã khác.";

            return "";

        }
        private int insert(DocumentItem objDraft)
        {
            try
            {
                int id = 0;
                string ck = checkValid(objDraft);
                if (ck.Trim().Equals(""))
                {
                    id = documentSV.Insert(objDraft, User.EmployeeID);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = ck

                    });
                }
                return id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        [BaseSystem(Name = "Thêm mới và sửa phiếu yêu cầu chế thử sản phẩm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult RequirementProduction(int id = 0, int productDevelopmentRequirementPlanId = 0)
        {
            if (productDevelopmentRequirementPlanDocumentSV.CheckDocumentProductionIsApproved(productDevelopmentRequirementPlanId))
            {
                var data = new ProductionRequirementItem();
                if (id != 0)
                {
                    data = productionRequirementSV.GetById(id);
                    data.ActionForm = Form.Edit;
                }
                else
                {
                    data.Product = productDevelopmentRequirementPlanSV.GetProductIDByID(productDevelopmentRequirementPlanId);
                    data.ActionForm = Form.Add;
                }
                return new Ext.Net.MVC.PartialViewResult { ViewName = "RequirementProduction", Model = data, ViewData = new ViewDataDictionary { { "productDevelopmentRequirementPlanId", productDevelopmentRequirementPlanId } } };
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Thông báo",
                    Message = "Để yêu cầu sản xuất thử sản phẩm thì kế hoạch phải có tài liệu thiết kế sản phẩm đã được ban hành!"
                });
                return this.Direct();
            }
        }
        public ActionResult InsertRequirement(ProductionRequirementItem data, int productDevelopmentRequirementPlanID)
        {
            if (data.HumanEmployee.ID == 0)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn người yêu cầu thiết kế thử sản phẩm!"
                });
                return this.Direct(false);
            }
            else
            {
                try
                {
                    if (data.ID != 0)
                    {
                        productionRequirementSV.Update(data, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    }
                    else
                    {
                        int idRequirement = productionRequirementSV.InsertFormDevelopmentProduction(data, User.ID);
                        productDevelopmentRequirementPlanSV.UpdateProductionRequirement(productDevelopmentRequirementPlanID, idRequirement, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                }
                catch
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
                finally
                {
                    X.GetCmp<Store>("StorePlanIndex").Reload();
                }
                return this.Direct(true);
            }
        }
    }
}
