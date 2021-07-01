using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileContract;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service hồ sơ hợp đồng lao động
    /// </summary>
    public interface IHumanProfileContractService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileContractDTO GetByID(int ID);

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileContractResponse GetDetailByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileContractResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID);
        
        /// <summary>
        /// Lấy toàn bộ hồ sơ hợp đồng theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileContractResponse> GetAllByEmployeeID(int employeeID);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(HumanProfileContractDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(HumanProfileContractDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
