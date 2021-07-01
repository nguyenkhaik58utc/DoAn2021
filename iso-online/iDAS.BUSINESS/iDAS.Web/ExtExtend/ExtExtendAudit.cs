using Ext.Net;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace iDAS.Web.ExtExtend
{
    public class ButtonAudit : Button.Builder
    {
        public string StoreUrl { get; set; }
        public Parameter StoreParam { get; set; }
        public string SubmitUrl { get; set; }
        public virtual ButtonAudit UrlStore(string url)
        {
            StoreUrl = url;
            return this;
        }
        public virtual ButtonAudit ParamStore(Parameter param)
        {
            StoreParam = param;
            return this;
        }
        public virtual ButtonAudit UrlSubmit(string url)
        {
            SubmitUrl = url;
            return this;
        }
    }

    public static class ExtExtendAudit
    {
        public static ButtonAudit ButtonAudit(this BuilderFactory builder)
        {
            var button = new ButtonAudit();
            button.Text("Đánh giá");
            button.Icon(Icon.AwardStarGold1);
            return button;
        }

        public static ButtonAudit AddWindowAudit(this ButtonAudit button)
        {
            var windowID = "Window" + Guid.NewGuid().ToString("N");
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlHelper.Action("List", "Audit", new { area = "Audit", urlSubmit = button.SubmitUrl, urlStore = button.StoreUrl, Name = button.StoreParam.Name, Value  = button.StoreParam.Value, Mode = button.StoreParam.Mode });
            var window = Html.X().Window().ID(windowID).Hidden(true)
                            .Title("Danh sách kết quả đánh giá")
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
