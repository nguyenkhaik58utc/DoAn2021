using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class BannerItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string TargetLink { get; set; }
        public int PageID { get; set; }
        public Nullable<int> Order { get; set; }
        public bool IsShow { get; set; }
        public string IsUseName { get; set; }
        public Nullable<int> LanguageID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public string tTitle { get; set; }
        public string t2Title { get; set; }
        public string dTitle { get; set; }
        public string duTitle { get; set; }
        public Nullable<int> widthTitle { get; set; }
        public Nullable<int> topTitle { get; set; }
        public Nullable<int> leftTitle { get; set; }
        public Nullable<int> paddingleftTitle { get; set; }
        public Nullable<int> paddingrightTitle { get; set; }
        public Nullable<int> paddingtopTitle { get; set; }
        public Nullable<int> paddingbottomTitle { get; set; }
        public string textalignTitle { get; set; }
        public Nullable<int> lineheightTitle { get; set; }
        public Nullable<int> fontsizeTitle { get; set; }
        public string colorTitle { get; set; }
        public string backgroundTitle { get; set; }
        public string backgroundcolorTitle { get; set; }
        public string textshadow { get; set; }

        public string tDes { get; set; }
        public string t2Des { get; set; }
        public string dDes { get; set; }
        public string duDes { get; set; }
        public Nullable<int> widthDes { get; set; }
        public Nullable<int> topDes { get; set; }
        public Nullable<int> leftDes { get; set; }
        public Nullable<int> paddingleftDes { get; set; }
        public Nullable<int> paddingrightDes { get; set; }
        public Nullable<int> paddingtopDes { get; set; }
        public Nullable<int> paddingbottomDes { get; set; }
        public string textalignDes { get; set; }
        public Nullable<int> lineheightDes { get; set; }
        public Nullable<int> fontsizeDes { get; set; }
        public string colorDes { get; set; }
        public string backgroundDes { get; set; }
        public string backgroundcolorDes { get; set; }
    }
}
