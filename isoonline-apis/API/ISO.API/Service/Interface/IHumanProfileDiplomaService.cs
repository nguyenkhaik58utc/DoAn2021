using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileDiploma;
using ISO.API.Models.ProfileHumanPermission;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service hồ sơ văn bằng
    /// </summary>
    public interface IHumanProfileDiplomaService
    {
        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileDiplomaDTO GetByID(int ID);

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileDiplomaResponse GetDetailByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên có phân trang
        /// </summary>
        /// <returns></returns>
        List<HumanProfileDiplomaResponse> GetPagingByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileDiplomaResponse> GetByEmployeeID( int employeeID);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(HumanProfileDiplomaDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(HumanProfileDiplomaDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);
        List<HumanProfileDiplomaExcel> getListDiplomaByEmpID(string lstEmployeeID);

    }
}
