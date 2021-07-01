using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net; 
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Web.API.DepartmentV3;
using iDAS.Web.Areas.Department.Models.TitleMenuRoleV3;
using iDAS.Web.API;
using iDAS.Web.Areas.Human.Models;

namespace iDAS.Web.Controllers
{
    [DirectController]
    [SystemAuthorize(CheckAuthorize = false)]
    public class HomeController : BaseController
    {
        // HungNM. Get MenuTreeRole for the current user.
        private string baseUrl = Ultilities.APIServer;
        private TitleMenuRoleV3API departmentTitleAPI = new TitleMenuRoleV3API();
        // End.

        private HumanUserSV humanUserSV = new HumanUserSV();
        private ShiftHistoryAPI shiftHistoryAPI = new ShiftHistoryAPI();

        [OutputCache(Duration = 0, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Migration()
        {
            var result = BaseDbContext.Instance.MigrationDbContextThrowException<iDAS.Base.iDASBusinessEntities>(BaseDatabase.GetDatabaseByCode());
            return this.Direct(result);
        }
        public StoreResult GetData()
        {
            return new StoreResult(ChartModel.GetRevenuaForYear(DateTime.Now.Year, User.EmployeeID));
        }
        public ActionResult Dashboard()
        {
            ViewBag.EmployeeID = User.EmployeeID;
            // thông tin trực ca
            var shiftInfo = shiftHistoryAPI.GetByUserID(User.ID).Result;
            string shirtText = "";
            DateTime shiftTime = DateTime.Now;
            if (shiftInfo != null && shiftInfo.ID > 0)
            {
                shirtText = "Bạn đang trong ca trực";
                if(shiftInfo.ProblemEventTotal > 0)
                    Ultility.ShowMessageBox(message: string.Format("Có {0} sự cố đang chờ xử lý", shiftInfo.ProblemEventTotal), icon: Ext.Net.MessageBox.Icon.WARNING);
            }    
            else
            {
                shiftInfo = new ShiftHistoryDTO();
                shiftInfo.ShiftTime = DateTime.Now;
            }

            ViewData["ShiftText"] = shirtText;
            ViewData["ShiftTime"] = shiftTime;

            return PartialView("Dashboard", shiftInfo);
        }
        public ActionResult LoadHeaderSystem()
        {
            string notifyNewTotal = new BusinessNotifySV().GetNotifyNewTotal(User.EmployeeID);
            ViewData["NotifyNewTotal"] = notifyNewTotal;
            return PartialView();
        }

        public ActionResult LoadMenuSystem()
        {
            var modules = new BusinessModuleSV().GetModulesMenu();
            return PartialView(modules);
        }

        // HungNM. Add a clone view load MenuSystem.
        public ActionResult LoadMenuSystemV3()
        {
            try
            {
                // Get userID of current user.
                //int userID = User.EmployeeID;
                var modules = departmentTitleAPI.GetBusinessModuleByUserID(User.EmployeeID);
                return PartialView(modules);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LoadMenuV3(string moduleCode)
        {
            try
            {
                List<v3OrgMenuDTO> functions = new List<v3OrgMenuDTO>();
                functions = departmentTitleAPI.GetBusinessFunctionByUserIdAndModuleCode(User.EmployeeID, moduleCode);
                var nodes = new NodeCollection(false);

                foreach (var function in functions)
                {
                    if (!string.IsNullOrEmpty(function.ParentCode)) break;
                    nodes.Add(getNodeV3(functions, function));
                }
                return this.Content(nodes.ToJson());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private Node getNodeV3(List<v3OrgMenuDTO> functions, v3OrgMenuDTO function)
        {
            Node node = createNodeV3(function);
            var childrens = getChildrensV3(functions, function);
            foreach (var children in childrens)
            {
                node.Children.Add(getNodeV3(functions, children));
            }
            return node;
        }

        private Node createNodeV3(v3OrgMenuDTO function)
        {
            Node node = new Node()
            {
                NodeID = function.ModuleCode + function.Code,
                Text = function.Name,
                Icon = Ultility.ConvertToIcon(function.Icon),
                HrefTarget = function.IsGroup == false ? function.Url : string.Empty,
                Leaf = function.IsGroup == false,
            };
            return node;
        }

        private List<v3OrgMenuDTO> getChildrensV3(List<v3OrgMenuDTO> functions, v3OrgMenuDTO function)
        {
            var childrens = (from f in functions where f.ParentCode == function.Code select f).ToList();
            return childrens;
        }
        // End. HungNM.

        public ActionResult LoadMenu(string moduleCode)
        {
            var nodes = new NodeCollection(false);
            var functions = new BusinessFunctionSV().GetFunctionsMenu(moduleCode);
            foreach (var function in functions)
            {
                if (!string.IsNullOrEmpty(function.ParentCode)) break;
                nodes.Add(getNode(functions, function));
            }
            return this.Content(nodes.ToJson());
        }

        private Node getNode(List<BusinessMenuFunctionItem> functions, BusinessMenuFunctionItem function)
        {
            Node node = createNode(function);
            var childrens = getChildrens(functions, function);
            foreach (var children in childrens)
            {
                node.Children.Add(getNode(functions, children));
            }
            return node;
        }
        private Node createNode(BusinessMenuFunctionItem function)
        {
            Node node = new Node()
            {
                NodeID = function.ModuleCode + function.Code,
                Text = function.Name,
                Icon = Ultility.ConvertToIcon(function.Icon),
                HrefTarget = function.IsGroup == false ? function.Url : string.Empty,
                Leaf = function.IsGroup == false,
            };
            return node;
        }
        private List<BusinessMenuFunctionItem> getChildrens(List<BusinessMenuFunctionItem> functions, BusinessMenuFunctionItem function)
        {
            var childrens = (from f in functions where f.ParentCode == function.Code select f).ToList();
            return childrens;
        }

        public ActionResult ShowChangePassword()
        {
            HumanUserItem obj = new HumanUserItem();
            obj.Account = User.Account;
            obj.ID = User.ID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChangePassword", Model = obj };
        }
        public ActionResult SaveChangePass(HumanUserItem obj)
        {
            if (Encryptor.Encrypt(obj.Password) == humanUserSV.GetById(User.ID).Password)
            {
                humanUserSV.ChangePassword(obj, User.ID);
                return this.Direct(true);
            }
            else
            {
                Ultility.ShowMessageBox(message: "Mật khẩu cũ không đúng vui lòng nhập lại!", icon: Ext.Net.MessageBox.Icon.WARNING);
                return this.Direct(false);
            }
           
        }

        public ActionResult Insert(ShiftHistoryDTO obj, bool val)
        {
            try
            {
                obj.UserID = User.ID;

                //if (ModelState.IsValid)
                {
                    shiftHistoryAPI.Insert(obj, User.ID);
                    //X.GetCmp<Panel>("pnlDashboard").Reload();
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
        public ActionResult UpdateShift(ShiftHistoryDTO obj)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    obj.UserID = User.ID;
                    shiftHistoryAPI.Update(obj);
                    //X.GetCmp<Panel>("pnlDashboard").Reload();
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

        public ActionResult HandOverShift(ShiftHistoryDTO objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objEdit.UserID = User.ID;
                    objEdit.ToUserID = objEdit.UserTo.ID;
                    shiftHistoryAPI.HandOverShift(objEdit);
                    //X.GetCmp<Store>("stTitleNew").Reload();
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
    }
}
