using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Areas.Department.Models;
using iDAS.Web.Areas.Department.Models.TitleMenuRoleV3;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using iDAS.Web.API.DepartmentV3;
using Ext.Net.Utilities;
using iDAS.Web.API.Human;
using iDAS.Web.Areas.Human.Model;
using ISO.API.Models;
using System.Web.Script.Serialization;
using iDAS.Items;

namespace iDAS.Web.Areas.Human.Controllers.Profile
{
    [BaseSystem(Name = "Phân quyền hồ sơ lý lịch", IsActive = true, IsShow = true, Position = 2, Icon = "HouseKey")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class V3_ProfilePermissionController : BaseController
    {
        private DepartmentSV departmentSV = new DepartmentSV();
        private TitleMenuRoleV3API departmentTitleAPI = new TitleMenuRoleV3API();
        private V3_ProfilePermissionAPI profilePermissionAPI = new V3_ProfilePermissionAPI();

        [SystemAuthorize(CheckAuthorize = true, FunctionCode = "ProfileHuman", ActionCode = "Permission")]

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lay danh sach chuc danh theo phong ban
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ActionResult LoadDepartmentTitleList(int departmentId)
        {
            try
            {
                var data = new List<object>();
                List<DepartmentTitleDTO> tmpRsl = new List<DepartmentTitleDTO>();

                tmpRsl = departmentTitleAPI.GetAllTitlesByDepartmentID(departmentId);

                foreach (var item in tmpRsl)
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("id", item.ID);
                    dic.Add("titleID", item.TitleID);
                    dic.Add("titleName", item.TitleName);
                    dic.Add("departmentID", item.DepartmentID);
                    data.Add(dic);
                }
                List<V3_ProfilePermissionResponse> a = new List<V3_ProfilePermissionResponse>();
               
                return this.Store(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load form 
        /// </summary>
        /// <param name="DepartmentTitleFromID"></param>
        /// <returns></returns>
        public ActionResult FormPermission(int DepartmentTitleFromID)
        {
            ViewData["DepartmentTitleFromID"] = DepartmentTitleFromID.ToString();
            return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
        }

        /// <summary>
        /// Lay danh sach da phan quyen theo chuc danh 
        /// </summary>
        /// <param name="DepartmentTitleFromID"></param>
        /// <returns></returns>
        public ActionResult GetAllPermission(int DepartmentTitleFromID)
        {
            try
            {
                // Lay ra quyen menu tu chuc danh va phong ban
                ResponseModel<List<V3_ProfilePermissionResponse>> tmpRsl = new ResponseModel<List<V3_ProfilePermissionResponse>>();
                tmpRsl = profilePermissionAPI.GetAllPermission(DepartmentTitleFromID);
                //tmpRsl = profilePermissionAPI.GetListPermission(88, 1);
                var data = tmpRsl.Data;
               // data= SortDepartmentTitleList(tmpRsl.Data);
                List<V3_ProfilePermissionResponse> newList = new List<V3_ProfilePermissionResponse>();
                data = SortDepartmentTitleList(0, tmpRsl.Data, newList, "");
                return this.Store(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xu ly sap xep danh sach chuc danh
        /// </summary>
        /// <param name="oldList">danh sach truoc khi xu ly</param>
        /// <returns></returns>
        private List<V3_ProfilePermissionResponse> SortDepartmentTitleList(int ParentDepartmentID, List<V3_ProfilePermissionResponse> oldList, List<V3_ProfilePermissionResponse> newList, string stringPostion)
        {
            string stringPostionTmp = "";
            var departments1 = departmentSV.GetDepartments(ParentDepartmentID, false, false, false, false, "");
            //lap danh sach phong ban
            for (int i = 0; i < departments1.Count; i++)
            {
                //gan bien vi tri
                stringPostionTmp = stringPostion + i.ToString() + ".";
                for (int j = 0; j < oldList.Count; j++)
                {
                    if (oldList[j].DepartmentID == departments1[i].ID)
                    {
                        //them vao list moi
                        oldList[j].StringPosition = stringPostionTmp;
                        newList.Add(oldList[j]);
                        //xoa khoi list cu
                        oldList.Remove(oldList[j]);
                        if (j != 0)
                        {
                            j = j - 1;
                        }
                    }
                }
                SortDepartmentTitleList(departments1[i].ID, oldList, newList, stringPostionTmp);
            }
            //tra ve ds moi
            return newList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepartmentTitleFromID"></param>
        /// <param name="dataMenuRole"></param>
        /// <returns></returns>
        public ActionResult SaveProfilePermission(int DepartmentTitleFromID, string dataProfileHumanPermission)
        {
            bool success = true;
            if (dataProfileHumanPermission != null && dataProfileHumanPermission.Length > 2)
            {
                var dataUpdated = JSON.Deserialize<Dictionary<string, object>>(dataProfileHumanPermission ?? string.Empty)["Updated"];
                var lstUpdated = ((Newtonsoft.Json.Linq.JArray)dataUpdated);
                //Lấy danh sách quyền đã thay đổi
                List<V3_ProfilePermissionResponse> lstUpdateRespon = lstUpdated.ToObject<List<V3_ProfilePermissionResponse>>();
                //Chuyển lstUpdateRespon về lstSaveRequest
                List<V3_ProfilePermissionSaveDTO> lstSaveRequest = new List<V3_ProfilePermissionSaveDTO>();
                V3_ProfilePermissionSaveDTO tmp;
                foreach (var item in lstUpdateRespon)
                {
                    tmp = new V3_ProfilePermissionSaveDTO();
                    tmp.ID = item.ID;
                    tmp.DepartmentTitleFromID = DepartmentTitleFromID;
                    tmp.DepartmentTitleToID = item.DepartmentTitleID;
                    tmp.IsView = item.IsView;
                    tmp.IsUpdate = item.IsUpdate;
                    lstSaveRequest.Add(tmp);
                }

                V3_ProfilePermissionSaveRequest model = new V3_ProfilePermissionSaveRequest();
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string output = jss.Serialize(lstSaveRequest);
                model.SaveList = output;
                profilePermissionAPI.SaveProfilePermission(model);
                X.GetCmp<Store>("stTitle").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            return this.Direct(success);
        }
    }
}
