using ISO.API.Models.HumanProfileCertificate;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Service.Interface;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using ISO.API.Models.ProfileHumanPermission;
using System;

namespace ISO.API.Service.Implement
{
    public class HumanProfileCertificateService : IHumanProfileCertificateService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3HumanProfileCertificate_Delete", new { ID = ID });
            return result;
        }

        public List<HumanProfileCertificateExcel> GetAllByEmployeeID(int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            List<HumanProfileCertificateExcel> lst = exec.GetByField<HumanProfileCertificateExcel>("sp_V3HumanProfileCertificate_GetAllByEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanProfileCertificateResponse> GetByEmployeeID(out int totalRows, int pageSize, int pageNumber, int employeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", employeeID, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanProfileCertificateResponse> lst = exec.GetPaging<HumanProfileCertificateResponse>(out totalRows, "sp_V3HumanProfileCertificate_GetByEmployeeID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return lst;
        }

        public HumanProfileCertificateDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileCertificateDTO> lst = exec.GetByField<HumanProfileCertificateDTO>("sp_V3HumanProfileCertificate_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public HumanProfileCertificateResponse GetDetailByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<HumanProfileCertificateResponse> lst = exec.GetByField<HumanProfileCertificateResponse>("sp_V3HumanProfileCertificate_GetDetailByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<HumanProfileCertificateExcel> getListCertificateByEmpID(string lstEmployeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var dynamicParameters = new DynamicParameters();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(Int32));
            List<int> lstID = JsonConvert.DeserializeObject<List<int>>(lstEmployeeID);
            foreach (var item in lstID)
            {
                dt.Rows.Add(item);
            }
            dynamicParameters.Add("@lstID", dt.AsTableValuedParameter("[dbo].[EmployeeIDList]"));
            List<HumanProfileCertificateExcel> result = exec.GetByField<HumanProfileCertificateExcel>("sp_V3HumanCertificate_GetByEmpID", dynamicParameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Insert(HumanProfileCertificateDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("CertificateTypeID", entity.CertificateTypeID);
            param.Add("CertificateLevelID", entity.CertificateLevelID);
            param.Add("PlaceOfTraining", entity.PlaceOfTraining);
            param.Add("DateIssuance", entity.DateIssuance);
            param.Add("DateExpiration", entity.DateExpiration);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt); param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileCertificate_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(HumanProfileCertificateDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Name", entity.Name);
            param.Add("CertificateTypeID", entity.CertificateTypeID);
            param.Add("CertificateLevelID", entity.CertificateLevelID);
            param.Add("PlaceOfTraining", entity.PlaceOfTraining);
            param.Add("DateIssuance", entity.DateIssuance);
            param.Add("DateExpiration", entity.DateExpiration);
            param.Add("IsDeleted", entity.IsDeleted);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3HumanProfileCertificate_Update", param);
            return result;
        }
    }
}
