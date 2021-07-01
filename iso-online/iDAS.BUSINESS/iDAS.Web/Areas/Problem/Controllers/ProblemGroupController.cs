using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items.Problem;
using iDAS.Utilities;
using iDAS.Web.API.Problem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Problem.Controllers
{
    public class ProblemGroupController : Controller
    {
        ProblemTypeAPI typeApi = new ProblemTypeAPI();
        ProblemGroupAPI api = new ProblemGroupAPI();
        // GET: Problem/ProblemEmergency
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            try
            {
                var resp = api.GetData().Data;
                return this.Store(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LoadDataNotId(StoreRequestParameters parameters, int ID)
        {
            try
            {
                List<int> lstId = new List<int>();
                var resp = api.GetData().Data;
                for (int i = 0; i < resp.Count; i++)
                {
                    if (resp[i].ParentID == ID || resp[i].ID == ID)
                        lstId.Add(i);
                }
                for (int i = lstId.Count - 1; i >= 0; i--)
                {
                    resp.RemoveAt(lstId[i]);
                }
                return this.Store(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetDataTree(StoreRequestParameters parameters, string node)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var ID = node == "root" ? 0 : System.Convert.ToInt32(node);
                var groups = api.GetByParent(ID).Result;
                var resp = api.GetData().Data;
                foreach (var group in groups)
                {
                    int check = 0;
                    foreach (var res in resp)
                    {
                        if (group.ID == res.ParentID)
                        {
                            check = 1;
                            break;
                        }
                        else check = 0;
                    }
                    if (check == 1)
                        group.IsParent = true;
                    else group.IsParent = false;
                    nodes.Add(createNodeDepartment(group));
                }
                return this.Content(nodes.ToJson());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Node createNodeDepartment(ProblemGroupItem problemGroupItem)
        {
            Node node = new Node();
            var css = "DepartmentActive";
            node.NodeID = problemGroupItem.ID.ToString();
            node.Text = problemGroupItem.ProblemGroupName.ToUpper();
            node.Cls = css;
            node.Icon = problemGroupItem.ParentID == 0 ? Icon.House : Icon.Door;
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = problemGroupItem.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "UpdateAt", Value = problemGroupItem.UpdateAtView, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "EstablishedDate", Value = problemGroupItem.EstablishedDateView, Mode = ParameterMode.Value });
            node.Leaf = !problemGroupItem.IsParent;
            return node;
        }

        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult ShowFrmDetail(int ID)
        {
            var obj = api.GetByID(ID).Result;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }


        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
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
        public ActionResult Create(ProblemGroupItem obj, string[] dataMenuRole)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    List<ProblemTypeItem> ProblemTypeInfo = new List<ProblemTypeItem>();
                    for(int i = 0; i < dataMenuRole.Length; i++)
                    {
                        ProblemTypeItem problemTypeItem = new ProblemTypeItem();
                        problemTypeItem.ID = Int32.Parse(dataMenuRole[i].Trim());
                        ProblemTypeInfo.Add(problemTypeItem);
                    }
                    obj.lstType = ProblemTypeInfo;
                    api.Insert(obj);
                    X.GetCmp<Store>("StoreProblemGroupTree").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();

        }


        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Update(int ID)
        {
            try
            {
                var obj = api.GetByID(ID).Result;
                return new Ext.Net.MVC.PartialViewResult() { Model = obj };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [HttpPost]
        public ActionResult Update(ProblemGroupItem objEdit, string[] dataMenuRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<ProblemTypeItem> ProblemTypeInfo = new List<ProblemTypeItem>();
                    for (int i = 0; i < dataMenuRole.Length; i++)
                    {
                        ProblemTypeItem problemTypeItem = new ProblemTypeItem();
                        string[] lstType = dataMenuRole[i].Trim().Split(',');
                        problemTypeItem.ID = Int32.Parse(lstType[0]);
                        if (lstType[1].Equals("true"))
                        {
                            problemTypeItem.IsSelect = true;
                        }
                        ProblemTypeInfo.Add(problemTypeItem);
                    }
                    objEdit.lstType = ProblemTypeInfo;
                    api.Update(objEdit);
                    X.GetCmp<Store>("StoreProblemGroupTree").Reload();
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

        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = false)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            try
            {
                var delete = api.Delete(ID);
                X.GetCmp<Store>("StoreProblemGroupTree").Reload();
                if (delete.Result != 0)
                    Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
                else
                {
                    Ultility.ShowMessageBox("Không thành công", "Nhóm đang được sử dụng", MessageBox.Icon.ERROR);
                }
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }
        }
    }
}