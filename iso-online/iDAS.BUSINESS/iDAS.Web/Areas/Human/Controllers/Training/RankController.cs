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
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thiết lâp hình thức xếp loại", Icon = "TagGreen", IsActive = true, IsShow = true, Position = 1, Parent = GroupTrainingController.Code)]
    public class RankController : BaseController
    {
        //
        // GET: /Human/Rank/
        HumanTrainingPractionerRankSV rankSV = new HumanTrainingPractionerRankSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = rankSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        public ActionResult LoadDataUse(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = rankSV.GetAllUse(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm ")]
        public ActionResult Insert(string name, string des, bool isUse,int id)
        {
            try
            {
                var objItem = convertToTypeItem(name, des, isUse,id);
                string ck = checkValid(objItem);
                if (ck.Equals(""))
                {
                    objItem.CreateBy = User.ID;
                    rankSV.Insert(objItem);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("stRank").Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);

                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa ")]
        public ActionResult Update(int id, string name, string des, bool isUse)
        {
            try
            {
                var objItem = convertToTypeItem(name, des, isUse, id);
                string ck = checkValid(objItem, id);
                if (ck.Equals(""))
                {
                    objItem.UpdateBy = User.ID;
                    rankSV.Update(objItem);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>("stRank").Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa ")]
        public ActionResult Delete(int id)
        {
            try
            {
                string ck = checkValidDelete(id);
                if (ck.Equals(""))
                {
                    rankSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("stRank").Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);

                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }

        private HumanTrainingPractionerRankItem convertToTypeItem(string RankName, string Descripson, bool IsUse = false, int ID = 0)
        {
            var objItem = new HumanTrainingPractionerRankItem
            {
                RankName = RankName.Trim(),
                Descripson = Descripson,
                IsUse = IsUse,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
            };
            if (ID > 0) objItem.ID = ID;
            return objItem;
        }
        private string checkValid(HumanTrainingPractionerRankItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.RankName = objDraft.RankName.Trim();
            if (id > 0)
            {
                var docItem = rankSV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.RankName.Trim().ToUpper().Equals(objDraft.RankName.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = rankSV.GetByName(objDraft.RankName);
                if (doc != null)
                {
                    return "Tên đã tồn tại trên hệ thống. Vui lòng nhập lại!";
                }
            }
            return "";
        }
        private string checkValidDelete(int id)
        {
            if (id > 0)
            {
                var docItem = rankSV.CheckNotIsUse(id);
                if (!docItem)
                {
                    return "Tên đã được sử dụng trong bảng khác trên hệ thống nên không được phép Xóa!";
                }
            }
            return "";
        }
    }
}
