using ISO.API.Models.ShiftHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IShiftHistoryService
    {
        /// <summary>
        /// Lấy chi tiết ca trực theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        ShiftHistoryDTO GetByID(int ID);
        /// <summary>
        /// Lấy chi tiết ca trực của người dùng
        /// </summary>
        /// <param name="UserID">ID người dùng</param>
        /// <returns></returns>
        ShiftHistoryDTO GetByUserID(int UserID);
        /// <summary>
        /// Lấy danh sách người trực theo phòng ban
        /// </summary>
        /// <param name="DepartmentID">DepartmentID người dùng</param>
        /// <returns></returns>
        string GetByDepartmentID(int DepartmentID);
        /// <summary>
        /// Thêm mới ca trực
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(ShiftHistoryReqModel entity);
        /// <summary>
        /// Cập nhật ca trực
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(ShiftHistoryReqModel entity);

        /// <summary>
        /// Bàn giao ca trực
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool HandOverShift(ShiftHistoryReqModel entity);
        int CountByUserID(int userID);
    }
}
