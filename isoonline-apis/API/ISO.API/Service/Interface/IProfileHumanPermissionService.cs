using ISO.API.Models.ProfileCuriculmDeparmentTitle;
using ISO.API.Models.ProfileHumanPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IProfileHumanPermissionService
    {

        /// <summary>
        /// Lấy quyền thao tác với từng phòng ban theo chức danh
        /// </summary>
        /// <returns></returns>
        ///List<ProfilePermissionResponse> GetListPermissionByDepartmentID(GetListPermissionRequest entity);

        /// <summary>
        /// Lấy toàn bộ quyền thao tác theo chức danh
        /// </summary>
        /// <returns></returns>
        List<ProfilePermissionResponse> GetAllByDepartmentTitle(int DepartmentTitleFromID);

        /// <summary>
        /// Lấy danh sách quyền theo HumanEmployeeID
        /// </summary>
        /// <returns></returns>
        List<CheckProfileHumanPermissionResponse> CheckProfielHumanPermission(CheckProfileHumanPermissionRequest entity);

        /// <summary>
        /// Thêm quyền thao tác hồ sơ lý lịch
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       /// int InsertUpdate(ProfileHumanPermissionDTO entity);

        /// <summary>
        /// Lưu quyền thao tác hồ sơ lý lịch
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int SaveProfilePermission(SaveProfilePermissionRequest request);
    }
}
