using iDAS.DA.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;

namespace iDAS.Services.Web
{
  public  class NewsCategoryServicesSV
    {
      private NewsCategoryServicesDA newsCategoryServicesDA = new NewsCategoryServicesDA();
        #region Insert, Update, Delete    
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        /// CuongPC           26/8/2014              tạo mới
        public bool Insert(int cateId, int serviceId)
        {
            try
            {
                WebNewsCategoryService Category = new WebNewsCategoryService();
                Category.WebNewsCategoryID = cateId;
                Category.ServiceID = serviceId;
                newsCategoryServicesDA.Insert(Category);
                newsCategoryServicesDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }        
        /// <summary>
        /// xóa 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CuongPC           26/8/2014            tạo mới
        public bool Delete(int cateId, int serviceId)
        {
            try
            {
                var obj = newsCategoryServicesDA.GetQuery(t => t.WebNewsCategoryID == cateId && t.ServiceID == serviceId).FirstOrDefault();
                newsCategoryServicesDA.Delete(obj);
                newsCategoryServicesDA.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
