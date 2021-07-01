using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.ProfileCuriculmDeparmentTitle;
using ISO.API.Models.ProfileHumanPermission;
using ISO.API.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class ProfileHumanPermissionService : IProfileHumanPermissionService
    {

        public List<CheckProfileHumanPermissionResponse> CheckProfielHumanPermission(CheckProfileHumanPermissionRequest entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new
            {
                HumanEmployeeID = entity.HumanEmployeeID
            };

            List<CheckProfileHumanPermissionResponse> listProfilePermission = exec.GetByField<CheckProfileHumanPermissionResponse>("sp_V3ProfileHumanPermission_Check", param, commandType: CommandType.StoredProcedure);
            return listProfilePermission;
        }

        public List<ProfilePermissionResponse> GetAllByDepartmentTitle(int DepartmentTitleFromID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new
            {
                DepartmentTitleFromID = DepartmentTitleFromID,
            };

            List<ProfilePermissionResponse> listProfilePermission = exec.GetByField<ProfilePermissionResponse>("sp_V3ProfileHumanPermission_GetAll", param, commandType: CommandType.StoredProcedure);
            return listProfilePermission;
        }

        /// <summary>
        /// Lưu danh sách quyền đã thay đổi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int SaveProfilePermission(SaveProfilePermissionRequest request)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var lst = JsonConvert.DeserializeObject<List<ProfilePermissionResponse>>(request.SaveList);
            //Tạo bảng cấu trúc bảng ProfileHumanSaveModel map với type table trong Database
            DataTable dtAdd = new DataTable();
            dtAdd.Columns.Add("ID",typeof(Int32));
            dtAdd.Columns.Add("DepartmentTitleFromID", typeof(Int32));
            dtAdd.Columns.Add("DepartmentTitleToID", typeof(Int32));
            dtAdd.Columns.Add("IsView", typeof(bool));
            dtAdd.Columns.Add("IsUpdate", typeof(bool));

            //DataTable dtUpdate = dtAdd;
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("ID", typeof(Int32));
            dtUpdate.Columns.Add("DepartmentTitleFromID", typeof(Int32));
            dtUpdate.Columns.Add("DepartmentTitleToID", typeof(Int32));
            dtUpdate.Columns.Add("IsView", typeof(bool));
            dtUpdate.Columns.Add("IsUpdate", typeof(bool));

            //Gán dữ liệu vào bảng
            for (int i = 0; i < lst.Count; i++)
            {
                //Nếu ID=0 chưa có bản ghi trên hệ thống => thêm vào lsstAdd
                if (lst[i].ID == 0)
                {

                    dtAdd.Rows.Add(lst[i].ID, lst[i].DepartmentTitleFromID, lst[i].DepartmentTitleToID, lst[i].IsView, lst[i].IsUpdate);
                }
                else
                {
                    dtUpdate.Rows.Add(lst[i].ID, lst[i].DepartmentTitleFromID, lst[i].DepartmentTitleToID, lst[i].IsView, lst[i].IsUpdate);
                }
            }

            var dynamicParameters = new DynamicParameters();
            //tao type table [dbo].[ProfileHumanSaveModel] trong sql server
            dynamicParameters.Add("@lstAdd", dtAdd.AsTableValuedParameter("[dbo].[ProfileHumanSaveModel]"));
            dynamicParameters.Add("@lstUpdate", dtUpdate.AsTableValuedParameter("[dbo].[ProfileHumanSaveModel]"));
            dynamicParameters.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3ProfileHumanPermission_Save", dynamicParameters, "Total", commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
