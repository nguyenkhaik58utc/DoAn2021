using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileReward;
using ISO.API.Models.ProfileHumanPermission;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service hồ sơ khen thưởng
    /// </summary>
    public interface IHumanProfileRewardService
    {
        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileRewardDTO GetByID(int ID);

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileRewardResponse GetDetailByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileRewardResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID);

        /// <summary>
        /// Lấy toàn bộ hồ sơ khen thưởng theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileRewardExcel> GetAllByEmployeeID(int employeeID);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(HumanProfileRewardDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(HumanProfileRewardDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

        List<HumanProfileRewardExcel> getListRewardByEmpID(string lstEmployeeID);

    }
}
