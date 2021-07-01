using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileDiscipline;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service hồ sơ kỉ luật
    /// </summary>
    public interface IHumanProfileDisciplineService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileDisciplineResponse GetDetailByID(int ID);

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileDisciplineDTO GetByID(int ID);
        
        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileDisciplineResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID);
        
        /// <summary>
        /// Lấy toàn bộ hồ sơ kỷ luật theo id nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileDisciplineResponse> GetAllByEmployeeID(int employeeID);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(HumanProfileDisciplineDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(HumanProfileDisciplineDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
