using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.HumanProfileCuriculmViate;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service Hồ sơ lý lịch nhân viên
    /// </summary>
    public interface IHumanProfileCuriculmViateService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileCuriculmViateDTO GetByHumanEmployeeID(int humanEmployeeID);

        /// <summary>
        /// Lấy full chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HumanProfileCuriculmViateReportDTO GetFullByHumanEmployeeID(int humanEmployeeID);

        /// <summary>
        /// Lấy danh sách thông tin chung theo danh sách id nhân viên
        /// </summary>
        /// <param name="lstEmployeeID">danh sach id nhan vien</param>
        /// <returns></returns>
        List<HumanCommonInformationDTO> GetCommonIformationByListEmployeeID(string lstEmployeeID);

        /// <summary>
        /// Lấy danh sách thông tin lý lịch chính trị theo danh sách id nhân viên
        /// </summary>
        /// <param name="lstEmployeeID">danh sach id nhan vien</param>
        /// <returns></returns>
        List<HumanPoliticInformationDTO> GetPoliticIformationByListEmployeeID(string lstEmployeeID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        List<HumanProfileCuriculmViateDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Save(HumanProfileCuriculmViateDTO entity);
    }
}
