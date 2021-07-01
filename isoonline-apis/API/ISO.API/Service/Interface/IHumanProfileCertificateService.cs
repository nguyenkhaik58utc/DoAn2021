using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileCertificate;
using ISO.API.Models.ProfileHumanPermission;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service hồ sơ chứng chỉ
    /// </summary>
    public interface IHumanProfileCertificateService
    {
        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileCertificateDTO GetByID(int ID);

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileCertificateResponse GetDetailByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách theo nhân viên
        /// </summary>
        /// <returns></returns>
        List<HumanProfileCertificateResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID);
        List<HumanProfileCertificateExcel> GetAllByEmployeeID(int employeeID);

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(HumanProfileCertificateDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(HumanProfileCertificateDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

        List<HumanProfileCertificateExcel> getListCertificateByEmpID(string lstEmployeeID);

    }
}
