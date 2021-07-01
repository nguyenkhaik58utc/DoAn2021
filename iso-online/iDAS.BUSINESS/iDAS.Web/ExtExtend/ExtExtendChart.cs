using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class ChartContainer : Chart.Builder
    {
        public NumericAxis.Builder YControl { get; set; }
        public CategoryAxis.Builder XControl { get; set; }
        public AjaxProxy.Builder Proxy { get; set; }
        public Store.Builder StoreControl { get; set; }
        public Model.Builder Model { get; set; }
        public ChartContainer()
        {
        }
        public virtual ChartContainer XAxis(string title = "", string value = "")
        {
            XControl.Title(title);
            XControl.Fields(value);
            return this;
        }
        public virtual ChartContainer YAxis(string title = "", string value = "")
        {
            YControl.Title(title);
            YControl.Fields(value);
            return this;
        }
        public ChartContainer Url(string value = "")
        {
            Proxy.Url(value);
            return this;
        }
        public ChartContainer StoreField(params string[] fieldNames)
        {
            Model.Fields(fieldNames);
            return this;
        }
        public ChartContainer Parameters(Action<StoreParameterCollection> action)
        {
            StoreControl.Parameters(action);
            return this;
        }
        public ChartContainer AutoLoad(bool value = true)
        {
            StoreControl.AutoLoad(value);
            return this;
        }
    }
    public class ChartPieContainer : Chart.Builder
    {
        public PieSeries.Builder PieSeries { get; set; }
        public SeriesLabel.Builder SeriesLabel { get; set; }
        public AjaxProxy.Builder Proxy { get; set; }
        public Store.Builder StoreControl { get; set; }
        public Model.Builder Model { get; set; }
        public ChartPieContainer()
        {
        }
        public ChartPieContainer Field(string angleField, string[] displayName)
        {
            PieSeries.AngleField(angleField)
                    .ShowInLegend(true)
                    .Donut(0)
                    .Highlight(true)
                    .HighlightSegmentMargin(20)
                    .Label(SeriesLabel.Display(SeriesLabelDisplay.InsideStart)
                                        .Field(displayName)
                                        .Contrast(true)
                                        .Font("12px Helvetica,sans-serif")
                                        .Orientation(Orientation.Horizontal)
                            );
            return this;
        }
        public ChartPieContainer Url(string value = "")
        {
            Proxy.Url(value);
            return this;
        }
        public ChartPieContainer StoreField(params string[] fieldNames)
        {
            Model.Fields(fieldNames);
            return this;
        }
        public ChartPieContainer Parameters(Action<StoreParameterCollection> action)
        {
            StoreControl.Parameters(action);
            return this;
        }
        public ChartPieContainer AutoLoad(bool value = true)
        {
            StoreControl.AutoLoad(value);
            return this;
        }
        public ChartPieContainer StoreControlID(string storeId = "")
        {
            StoreControl.ID(storeId);
            return this;
        }
    }
    public static class ExtExtendChart
    {
        public static ChartContainer CustomChar<TModel>(this BuilderFactory<TModel> X, bool isUseStore = true)
        {
            var chart = new ChartContainer();
            var YAxis = X.NumericAxis().Grid(true).Minimum(0)
                            .Label(X.AxisLabel().Renderer(r => r.Handler = "return Ext.util.Format.number(value, '0,0');"));
            var XAxis = X.CategoryAxis().Position(Position.Bottom);
            chart.XControl = XAxis;
            chart.YControl = YAxis;
            chart.StoreControl = X.Store().RemotePaging(true);
            chart.Proxy = X.AjaxProxy().Reader(X.JsonReader().Root("data")).IDParam("ID");
            chart.Model = X.Model();
            chart.Shadow(true).StandardTheme(StandardChartTheme.Base).Animate(true)
                .LegendConfig(X.ChartLegend().Position(LegendPosition.Top).LabelFont("11px Tahoma").BoxStroke("#084594"))
                .Axes(chart.XControl, chart.YControl);
            if (isUseStore)
            {
                chart.Store(chart.StoreControl.Model(chart.Model).Proxy(chart.Proxy));
            }
            return chart;
        }
        public static ChartPieContainer CustomCharPie<TModel>(this BuilderFactory<TModel> X, bool isUseStore = true)
        {
            var chart = new ChartPieContainer();
            chart.PieSeries = X.PieSeries().ShowInLegend(true)
                                .Donut(0)
                                .Highlight(true)
                                .HighlightSegmentMargin(20);
            chart.SeriesLabel = X.SeriesLabel()
                                .Display(SeriesLabelDisplay.InsideStart)
                                .Contrast(true)
                                .Font("12px Helvetica,sans-serif")
                                .Orientation(Orientation.Horizontal);
            chart.StoreControl = X.Store().RemotePaging(true);
            chart.Proxy = X.AjaxProxy().Reader(X.JsonReader().Root("data")).IDParam("ID");
            chart.Model = X.Model();
            chart.Animate(true).Shadow(true).InsetPadding(20).Theme("Base:gradients").StandardTheme(StandardChartTheme.Base)
                .LegendConfig(Html.X().ChartLegend().Position(LegendPosition.Right)).StyleSpec("background:#fff;")
                .Series(chart.PieSeries.Label(chart.SeriesLabel));
            if (isUseStore)
            {
                chart.Store(chart.StoreControl.Model(chart.Model).Proxy(chart.Proxy));
            }
            return chart;
        }
        public static ColumnSeries.Builder CustomColumnSeries(this BuilderFactory X, string xfield, string[] yfield)
        {
            return X.ColumnSeries()
                        .Axis(Position.Bottom)
                        .Highlight(true)
                        .XField(xfield)
                        .YField(yfield);
        }
        public static LineSeries.Builder CustomLineSeries(this BuilderFactory X, string xfield, string yfield, string color = "", SpriteType Type = SpriteType.Circle)
        {
            var line = X.LineSeries()
                    .Axis(Position.Left).Smooth(3).Fill(true)
                    .XField(xfield)
                    .YField(yfield)
                    .HighlightConfig(new SpriteAttributes { Size = 7, Radius = 7 })
                    .MarkerConfig(new SpriteAttributes { Type = Type, Size = 4, Radius = 4, StrokeWidth = 0 });
            if (!String.IsNullOrEmpty(color))
                line.MarkerConfig(new SpriteAttributes { Fill = color });
            return line;
        }
    }
}