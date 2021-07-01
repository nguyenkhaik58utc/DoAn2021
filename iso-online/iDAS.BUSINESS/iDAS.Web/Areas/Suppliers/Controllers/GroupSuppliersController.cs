using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh sách nhóm nhà cung cấp", Icon = "PagePortraitShot", IsActive = true, IsShow = false, Position = 5)]
    public class GroupSuppliersController : BaseController
    {
        //
        // GET: /Suppliers/GroupSuppliers/
        private SuppliersGroupSV SuppGroupSV = new SuppliersGroupSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = SuppGroupSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Thêm", IsActive = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult InsertForm(string parentID = "1")
        {
            int pID = Int16.Parse(parentID);
            var data = new SuppliersGroupItem();
            data.ParentID = pID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [BaseSystem(Name = "Sửa", IsActive = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = SuppGroupSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(SuppliersGroupItem data)
        {
            bool success = false;
            try
            {   
                    if (data.ID == 0)
                    {
                        if (SuppGroupSV.checkNameExist(data.ParentID, data.Name))
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Thông báo",
                                Message = "Tên nhóm NCC đã tồn tại!"
                            });
                        }
                        else
                        {                                                  
                            SuppGroupSV.Insert(data, User.ID);
                            Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                            X.GetCmp<Window>("winGroupSupp").Close();
                            success = true;
                        }
                    }
                    else
                    {
                        if (SuppGroupSV.checkNameExist(data.ParentID, data.Name,data.ID))
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Thông báo",
                                Message = "Tên nhóm NCC đã tồn tại!"
                            });
                        }
                        else
                        {
                            SuppGroupSV.Update(data, User.ID);
                            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                            X.GetCmp<Window>("winGroupSupp").Close();
                            success = true;
                        }
                    }
                
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                //X.GetCmp<TreePanel>("treeGroupSupplierID").GetRootNode().Reload();
                //X.GetCmp<Store>("StoreGroupCustomer").Reload();
            }
            return this.Direct(success);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ///
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(AllowAnonymous = false, CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                int ids = int.Parse(stringId);
                SuppGroupSV.Delete(ids);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();                
                X.GetCmp<Store>("StoreGroupCustomer").Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Xóa bản ghi không thành công!"
                });
                return this.Direct("Error");
            }
        }
        #region Dùng chung
        public ActionResult MultipleDetail(string ids)
        {
            var result = new List<SuppliersGroupItem>();
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var intIds = ids.Split(',').Select(i => System.Convert.ToInt32(i)).ToArray();
                result = SuppGroupSV.GetByIds(intIds);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "MultipleDetail", Model = result };
        }
        public ActionResult MultipleSelect(string ids = "")
        {
            ViewData["Selected"] = ids;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "MultipleSelect", ViewData = ViewData };
        }
        
        public ActionResult List()
        {
            ViewData["Administrator"] = User.Administrator;
            SuppGroupSV.checkRoot();
            return PartialView();
        }
        // Du lieu khach hang dung chung
        public ActionResult LoadTreeData(string node)
        {
            NodeCollection nodes = new NodeCollection();
            int nodeId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var lsDataNodes = SuppGroupSV.GetTreeData(nodeId);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Text = dataNode.Name;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;                
                nodeItem.Leaf = dataNode.Leaf;
                if (!dataNode.Leaf) nodeItem.Expanded = true;
                else nodeItem.Expanded = false;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                //nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsParent", Value = dataNode.Leaf.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }
        #endregion
    }
}
