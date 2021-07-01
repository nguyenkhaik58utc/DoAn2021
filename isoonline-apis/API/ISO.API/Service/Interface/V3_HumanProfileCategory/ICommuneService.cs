using ISO.API.Models.Commune;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục phường/xã
    /// </summary>
    public interface ICommuneService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        CommuneDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo quận huyện
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<CommuneDTO> GetByDistrict(int districtID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<CommuneDTO> GetAll();

        /// <summary>
        /// Lấy toàn bộ danh sách có phân trang
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<CommuneDTO> GetPaging(out int totalRows, int pageSize, int pageNumber);

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(CommuneDTO entity);

        /// <summary>
        /// cập nhật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(CommuneDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
