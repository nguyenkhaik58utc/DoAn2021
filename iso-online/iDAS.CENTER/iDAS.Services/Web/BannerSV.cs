
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace iDAS.Services
{
    //
    // GET: /AdvertiseSV
    // Author: CuongPC
    // CreatedDate: 4/9/2014
    // 
    public class BannerSV
    {
        private BannerDA advertiseDA = new BannerDA();
        #region Insert, Update, Delete
        public List<BannerItem> GetAll(int page, int pageSize, out int total)
        {
            try
            {
                List<BannerItem> lst = new List<BannerItem>();
                var lstType = advertiseDA.GetQuery().OrderByDescending(t => t.ID).Page(page, pageSize, out total).ToList();

                if (lstType != null && lstType.Count > 0)
                {
                    foreach (var item in lstType)
                    {
                        lst.Add(new BannerItem
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Description = item.Description,
                            IsShow = item.IsShow,
                            ImageUrl = item.ImageUrl,
                            IsUseName = GetData.StatusIsUse(item.IsShow),
                            Order = item.Order,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<BannerItem> GetAllSlide(int page, int pageSize, out int total)
        {
            try
            {
                List<BannerItem> lst = new List<BannerItem>();
                var lstType = advertiseDA.GetQuery().OrderByDescending(t => t.ID).Page(page, pageSize, out total).ToList();

                if (lstType != null && lstType.Count > 0)
                {
                    foreach (var item in lstType)
                    {
                        lst.Add(new BannerItem
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Description = item.Description,
                            IsShow = item.IsShow,
                            ImageUrl = item.ImageUrl,
                            IsUseName = GetData.StatusIsUse(item.IsShow),
                            Order = item.Order,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        /// CuongPC           4/9/2014              tạo mới
        public bool Insert(WebBanner Ads)
        {
            try
            {
                if (Ads.IsShow != true)
                    Ads.IsShow = false;
                Ads.CreateAt = DateTime.Now;
                advertiseDA.Insert(Ads);
                advertiseDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// cập nhật
        /// </summary>
        /// <param name="update"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  CuongPC
        public bool Update(WebBanner update)
        {
            try
            {
                var objNews = advertiseDA.GetById(update.ID);
                objNews.IsShow = update.IsShow;
                objNews.Title = update.Title;
                objNews.Description = update.Description;
                objNews.ImageUrl = update.ImageUrl;
                objNews.ImageUrlSmall = update.ImageUrlSmall;
                objNews.TargetLink = update.TargetLink;
                objNews.Order = update.Order;
                objNews.UpdateAt = DateTime.Now;
                advertiseDA.Update(objNews);
                advertiseDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Thông tin chung
        /// </summary>
        /// <param name="update"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  CuongPC           4/9/2014            tạo mới
        public BannerItem GetSimpleByID(int id)
        {
            try
            {
                BannerItem objAds = new BannerItem();
                WebBanner detail = advertiseDA.GetQuery(m => m.ID == id && m.IsShow == true).FirstOrDefault();
                objAds.ID = detail.ID;
                objAds.Title = detail.Title;
                objAds.Description = detail.Description;
                objAds.TargetLink = detail.TargetLink;
                objAds.IsShow = detail.IsShow;
                objAds.ImageUrl = detail.ImageUrl;
                objAds.CreateAt = detail.CreateAt;
                objAds.UpdateAt = detail.UpdateAt;
                
               
                return objAds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// xóa 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CuongPC           4/9/2014            tạo mới
        public bool Delete(int? id)
        {
            try
            {
                advertiseDA.Delete(id);
                advertiseDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        public List<BannerItem> GetDataCustomer()
        {
            try
            {
                List<BannerItem> lst = new List<BannerItem>();
                var lstType = advertiseDA.GetQuery(m =>m.IsShow == true).OrderBy(t => t.Order).ToList();

                if (lstType != null && lstType.Count > 0)
                {
                    foreach (var item in lstType)
                    {
                        lst.Add(new BannerItem
                        {
                            ID = item.ID,
                            Title = item.Title,
                            ImageUrl = item.ImageUrl,
                            TargetLink = item.TargetLink,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<BannerItem> GetDataSlide()
        {
            try
            {
                List<BannerItem> lst = new List<BannerItem>();
                //var lstType = advertiseDA.GetQuery(m => m.Type == 2 && m.IsShow == true).OrderBy(t => t.NumberNo).ToList();
                var lstType = advertiseDA.GetQuery().ToList();
                if (lstType != null && lstType.Count > 0)
                {
                    foreach (var item in lstType)
                    {
                        lst.Add(new BannerItem
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Description = item.Description,
                            ImageUrl = item.ImageUrl,
                            TargetLink = item.TargetLink,
                            //CreateAt = item.CreateAt,
                            //CreateBy = item.CreateBy,
                            //UpdateAt = item.UpdateAt,
                            //UpdateBy = item.UpdateBy,
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
