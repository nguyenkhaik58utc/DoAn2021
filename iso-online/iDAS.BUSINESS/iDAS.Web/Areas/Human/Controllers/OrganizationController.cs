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
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.API.Problem;
using iDAS.Web.Areas.Department.Models;
using Newtonsoft.Json.Linq;

namespace iDAS.Web.Areas.Human.Controllers
{
    public partial class OrganizationController : BaseController
    {
        private HumanOrganizationSV organizationService = new HumanOrganizationSV();
        private readonly string storeOrganizationsID = "StoreOrganizations";
        private RoleSV roleSV = new RoleSV();
        DepartmentTitleAPI titleAPI = new DepartmentTitleAPI();
        DepartmentTitleUserAPI TitleUserAPI = new DepartmentTitleUserAPI();
        public ActionResult LoadOrganizations(int userId, int groupId)
        {
            var roles = roleSV.GetRoleSelect(groupId, userId);
            foreach (var role in roles)
            {
                var organization = organizationService.GetOrganization(userId, role.ID);
                role.IsSelected = organization != null;
            }
            return this.Store(roles);
        }

        public ActionResult LoadOrganizationsV3(int userId, int groupId)
        {
            var roles = titleAPI.GetByDepartment(groupId);
            List<DepartmentTitleV3DTO> results = new List<DepartmentTitleV3DTO>();
            foreach (var role in roles)
            {
                results.Add(new DepartmentTitleV3DTO(role.ID,role.DepartmentID,role.DepartmentName,false,role.TitleID,role.TitleName));
            }
            var organizations = TitleUserAPI.GetByDepAndEmp(groupId, userId).Result;
            foreach (var result in results)
            {
                foreach (var organization in organizations)
                {
                    if (result.ID == organization.DepartmentTitleID)
                        result.IsSelect = true;
                }
            }
            return this.Store(results);
        }

        public ActionResult UpdateOrganization(string data, int userId)
        {
            var role = Ext.Net.JSON.Deserialize<HumanRoleItem>(data);
            try
            {
                var organization = organizationService.GetOrganization(userId, role.ID);
                if (organization == null && role.IsSelected)
                {
                    organization = new HumanOrganizationItem();
                    organization.RoleID = role.ID;
                    organization.UserID = userId;
                    organizationService.Insert(organization, User.ID);
                }
                if (organization != null && !role.IsSelected)
                {
                    organizationService.Delete(organization.ID);
                }
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeOrganizationsID).Reload();
            }
            return this.Direct();
        }

        public ActionResult UpdateOrganizationV3(string data, int userId)
        {
            try
            {
                dynamic json = JObject.Parse(data);
                int departmentTitleId = json.ID;
                string TitleName = json.TitleName;
                bool IsSelect = json.IsSelect;
                if (IsSelect)
                {
                    TitleUserAPI.Insert(departmentTitleId, userId);
                }
                else
                {
                    TitleUserAPI.Delete(departmentTitleId,userId);
                }
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeOrganizationsID).Reload();
            }
            return this.Direct();
        }

        public ActionResult ChangeDepartment(string strEmployessId, int roleId, int departmentId)
        {
            try
            {
                organizationService.DeleteByDepartmentIdAndEmployeesId(strEmployessId, roleId, departmentId);
                organizationService.InsertByEmployeesIdAndRoleId(strEmployessId, roleId, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Window>("winDepartmetChange").Close();
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
        }
        /// <summary>
        /// Thanhnv 09/12/2014
        /// load store Tổ chức chức danh
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult LoadPosition(int? userId = null)
        {
            if (userId == null) return null;
            var data = organizationService.GetAllRoleAndDepartmentByEmployeeID(userId);
            return this.Store(data);
        }
    }
}
