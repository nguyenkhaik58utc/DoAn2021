using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileTraining;
using ISO.API.Models.ProfileHumanPermission;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service hồ sơ đào tạo
    /// </summary>
    public interface IHumanProfileTrainingService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileTrainingDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileTrainingResponse> GetByEmployeeID(int employeeID);

        /// <summary>
        /// Lấy toàn bộ danh sách có phân trang theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileTrainingResponse> GetPagingByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID);

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <returns></returns>
        HumanProfileTrainingResponse GetDetailByID(int ID);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(HumanProfileTrainingDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(HumanProfileTrainingDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

        List<HumanProfileEducationTrainingExcel> getListTrainingByEmpID(string lstEmployeeID);

    }
}
