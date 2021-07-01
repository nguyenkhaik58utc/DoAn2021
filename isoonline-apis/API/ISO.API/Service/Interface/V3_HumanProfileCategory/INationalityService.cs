using ISO.API.Models.Nationality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục quốc tịch
    /// </summary>
    public interface INationalityService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        NationalityDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<NationalityDTO> GetAll();

        /// <summary>
        /// Lấy toàn bộ danh sách có phân trang
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<NationalityDTO> GetPaging(out int totalRows, int pageSize, int pageNumber);

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(NationalityDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(NationalityDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
