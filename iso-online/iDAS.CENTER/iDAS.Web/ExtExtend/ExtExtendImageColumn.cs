using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public static class ExtExtendImageColumn
    {
        public static Column.Builder ImageColumn<TModel>(this BuilderFactory<TModel> X)
        {
            var script = @"<script> var rendererImg = function(value){ var urlImg = '/File/LoadImage?fileId='+value;"+
                           "var tpl = \"<img src='\"+ urlImg + \"' style='width:100%; height:45px;'/>\"; return tpl;};</script>";
            var column = X.Column().Text("Ảnh").Renderer("rendererImg").Align(Alignment.Center)
                        .HtmlBin(c => script)
                        .Width(80).StyleSpec("font-weight: bold; text-align: center;");
            return column;
        }
    }
}