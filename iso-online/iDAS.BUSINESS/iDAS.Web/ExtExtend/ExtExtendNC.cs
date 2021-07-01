using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace iDAS.Web.ExtExtend
{
    public class ButtonNC : Button.Builder
    {
        public string StoreUrl { get; set; }
        public Parameter StoreParam { get; set; }
        public string SubmitUrl { get; set; }
        public virtual ButtonNC UrlStore(string url)
        {
            StoreUrl = url;
            return this;
        }
        public virtual ButtonNC ParamStore(Parameter param)
        {
            StoreParam = param;
            return this;
        }
        public virtual ButtonNC UrlSubmit(string url)
        {
            SubmitUrl = url;
            return this;
        }
    }

    public static class ExtExtendNC
    {
        public static ButtonNC ButtonNC(this BuilderFactory builder)
        {
            var button = new ButtonNC();
            button.Text("Điểm không phù hợp");
            button.Icon(Icon.ApplicationOsx);
            return button;
        }

        public static ButtonNC AddWindowNC(this ButtonNC button)
        {
            var windowID = "Window" + Guid.NewGuid().ToString("N");
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlHelper.Action("List", "NC", new { area = "Quality", urlSubmit = button.SubmitUrl, urlStore = button.StoreUrl, Name = button.StoreParam.Name, Value = button.StoreParam.Value, Mode = button.StoreParam.Mode });
            var window = Html.X().Window().ID(windowID).Hidden(true)
                            .Title("Danh sách điểm không phù hợp")
                            .StyleSpec("-webkit-border-radius:0")
                            .Icon(Icon.ApplicationSideList)
                            .Height(1)
                            .Maximized(true)
                            .Constrain(true)
                            .Layout(LayoutType.Fit)
                            .Modal(true)
                            .Resizable(false)
                            .Margin(0)
                            .BodyPadding(0)
                            .Border(true)
                            .Loader(
                                Html.X().ComponentLoader().Url(url)
                                .AutoLoad(false)
                                .LoadMask(lm => { lm.ShowMask = true; lm.Msg = "Đang tải dữ liệu ..."; })
                                .Mode(LoadMode.Script)
                                .Params(new { containerId = windowID })
                                .Listeners(ls => ls.Load.Handler = "App." + windowID + ".down('gridpanel').getStore().load();")
                            )
                            .Listeners(ls => ls.Show.Handler = "this.getLoader().load();");
            button.Bin(window);
            button.Listeners(ls => ls.Click.Handler = "App." + windowID + ".show()");
            return button;
        }
    }
}
