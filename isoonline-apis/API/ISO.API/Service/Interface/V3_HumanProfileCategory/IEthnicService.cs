using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.Ethnic;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục dân tộc
    /// </summary>
    public interface IEthnicService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        EthnicDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo quốc tịch
        /// </summary>
        /// <returns></returns>
        List<EthnicDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(EthnicDTO entity);

        /// <summary>
        /// cập nhật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(EthnicDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}