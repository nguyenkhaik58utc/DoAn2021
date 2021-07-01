using ISO.API.DataAccess;
using ISO.API.Models.ManagementLevel;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class ManagementLevelService : IManagementLevelService
    {
        /// <summary>Thêm mới cấp quản lý</summary>
        /// <remarks></remarks>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        public int AddManagementLevel(object param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int managementLevelDTO = exec.ExcuteScalar("sp_ManagementLevel_Insert", param);
            return managementLevelDTO;
        }

        /// <summary>xóa bỏ cấp quản lý</summary>
        /// <remarks></remarks>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        public int deleteManagementLevel(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int managementLevelDTO = exec.ExcuteScalar("sp_ManagementLevel_Delete", new { ID = id });
            return managementLevelDTO;
        }

        /// <summary>Danh sách cấp quản lý</summary>
        /// <remarks></remarks>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        public List<ManagementLevelDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<ManagementLevelDTO> managementLevelDTO = exec.GetAll<ManagementLevelDTO>("sp_ManagementLevel_GetAll", commandType: CommandType.StoredProcedure);
            return managementLevelDTO;
        }

        /// <summary>Cấp quản lý theo ID</summary>
        /// <remarks></remarks>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        public List<ManagementLevelDTO> GetByID(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<ManagementLevelDTO> managementLevelDTO = exec.GetByField<ManagementLevelDTO>("sp_ManagementLevel_GetByID", new { ID = id }, commandType: CommandType.StoredProcedure);
            return managementLevelDTO;
        }

        /// <summary>cập nhật cấp quản lý</summary>
        /// <remarks></remarks>
        /// <returns></returns>
        /// <response code="200">Success!!!</response>
        /// <response code="400">Some thing wrong!</response>
        /// <response code="401">Unauthorize!</response>
        /// <response code="500">Oops! API is down :(</response>
        public int UpdateManagementLevel(object param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int managementLevelDTO = exec.ExcuteScalar("sp_ManagementLevel_Update", param);
            return managementLevelDTO;
        }
    }
}
