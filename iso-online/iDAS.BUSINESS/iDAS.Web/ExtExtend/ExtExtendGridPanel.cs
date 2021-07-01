using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class GridPanelContainer : GridPanel.Builder
    {
        public AjaxProxy.Builder Proxy { get; set; }
        public Store.Builder StoreControl { get; set; }
        public Model.Builder Model { get; set; }
        public GridPanelContainer()
        {

        }
        public GridPanelContainer Url(string value = "")
        {
            Proxy.Url(value);
            return this;
        }
        public GridPanelContainer StoreField(params string[] fieldNames)
        {
            Model.Fields(fieldNames);
            return this;
        }
        public GridPanelContainer Parameters(Action<StoreParameterCollection> action)
        {
            StoreControl.Parameters(action);
            return this;
        }
        public GridPanelContainer AutoLoad(bool value = true)
        {
            StoreControl.AutoLoad(value);
            return this;
        }
    }
    public static class ExtExtendGridPanel
    {

        public static GridPanelContainer CustomGridPanel<TModel>(this BuilderFactory<TModel> X)
        {
            var grid = new GridPanelContainer();
            grid.StoreControl = X.Store().PageSize(20).RemotePaging(true);
            grid.Proxy = X.AjaxProxy().Reader(X.JsonReader().Root("data")).IDParam("ID");
            grid.Model = X.Model();
            grid.Store(grid.StoreControl.Model(grid.Model).Proxy(grid.Proxy))
                .ColumnLines(true)
                .BottomBar(X.CustomPagingToolbar());
            return grid;
        }
    }
}