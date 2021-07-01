using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.Ages;

namespace ISO.API.Service.Interface
{

    /// <summary>
    /// Service danh mục độ tuổi
    /// </summary>
    public interface IAgeService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        AgeDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<AgeDTO> GetAll();

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(AgeDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(AgeDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
