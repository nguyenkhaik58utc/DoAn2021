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

namespace iDAS.Web.Areas.User.Controllers
{
    public partial class OrganizationController : BaseController
    {
        private OrganizationSV organizationService = new OrganizationSV();
        private readonly string storeOrganizationsID = "StoreOrganizations";
        private RoleSV roleService = new RoleSV();

        public ActionResult LoadOrganizations(int? userId = null, int? groupId = null)
        {
            if (groupId == null) return null;
            var roles = roleService.GetAllByGroupID(groupId.Value);
            if (userId != null)
            {
                foreach (var role in roles)
                {
                    var organization = organizationService.GetOrganization(userId.Value, role.ID);
                    role.IsSelected = organization != null;
                }
            }
            return this.Store(roles);
        }
        public ActionResult UpdateOrganization(string data, int userId)
        {
            var role = Ext.Net.JSON.Deserialize<RoleItem>(data);
            try
            {
                var organization = organizationService.GetOrganization(userId, role.ID);
                if (organization == null && role.IsSelected)
                {
                    organization = new OrganizationItem();
                    organization.RoleID = role.ID;
                    organization.UserID = userId;
                    organizationService.Insert(organization, User.ID);
                }
                if (organization != null && !role.IsSelected)
                {
                    organizationService.Delete(organization.ID);
                }
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeOrganizationsID).Reload();
            }
            return this.Direct();
        }

        /// <summary>
        /// Thanhnv 08/12/2014
        /// load store Tổ chức chức danh
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult LoadPosition(int? userId = null)
        {
            if (userId == null) return null;
            var data = organizationService.GetAllPositionAndGroupByUserID(userId);
            return this.Store(data);
        }
    }
}
