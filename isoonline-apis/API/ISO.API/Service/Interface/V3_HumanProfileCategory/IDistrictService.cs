using ISO.API.Models;
using ISO.API.Models.District;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục quận huyện
    /// </summary>
    public interface IDistrictService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        DistrictDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo tinh thành
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<DistrictDTO> GetByCity(int cityID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        List<DistrictDTO> GetAll();

        /// <summary>
        /// Lấy toàn bộ danh sách có phân trang
        /// </summary>
        /// <returns></returns>
        List<DistrictDTO> GetPaging(out int totalRows, int pageSize, int pageNumber);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(DistrictDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(DistrictDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
