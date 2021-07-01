using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileRelationship;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service hồ sơ quan hệ gia đình
    /// </summary>
    public interface IHumanProfileRelationshipService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileRelationshipDTO GetByID(int ID);

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileRelationshipResponse GetDetailByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileRelationshipResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID);
        
        /// <summary>
        /// Lấy toàn bộ hồ sơ quan hệ gia đình theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileRelationshipResponse> GetAllByEmployeeID(int employeeID);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(HumanProfileRelationshipDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(HumanProfileRelationshipDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
