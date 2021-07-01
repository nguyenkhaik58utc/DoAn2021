using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iDAS.Core
{
    /// <summary>
    /// Controller Extensions
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Get url action of controller
        /// </summary>
        public static string GetActionUrlController(this Controller controller, string action = "Index")
        {
            RouteValueDictionary values = new RouteValueDictionary();
            values["area"] = controller.GetAreaNameController();
            values["controller"] = controller.GetNameController();
            values["action"] = action;

            var context = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());
            VirtualPathData vpd = RouteTable.Routes.GetVirtualPathForArea(context, values);
            return (vpd == null) ? null : vpd.VirtualPath;
        }

        /// <summary>
        /// Get area name of controller
        /// </summary>
        public static string GetAreaNameController(this Controller controller)
        {
            var source = controller.GetType().FullName.Split('.').ToList();
            var index = source.IndexOf("Areas");
            return index > 0 ? source[index + 1] : string.Empty;
        }

        /// <summary>
        /// Get name of controller
        /// </summary>
        public static string GetNameController(this Controller controller)
        {
            return controller.GetType().Name.Replace("Controller", string.Empty);
        }
    }
}
