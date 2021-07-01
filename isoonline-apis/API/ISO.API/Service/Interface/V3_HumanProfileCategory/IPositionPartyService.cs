using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.PositionParty;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục chức vụ đảng
    /// </summary>
    public interface IPositionPartyService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        PositionPartyDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo quốc tịch
        /// </summary>
        /// <returns></returns>
        List<PositionPartyDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(PositionPartyDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(PositionPartyDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
