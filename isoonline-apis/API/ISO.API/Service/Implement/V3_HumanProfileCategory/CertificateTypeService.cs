using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.CertificateType;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class CertificateTypeService : ICertificateTypeService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3MstCertificateType_Delete", new { ID = ID });
            return result;
        }

        public List<CertificateTypeDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<CertificateTypeDTO> lst = exec.GetByField<CertificateTypeDTO>("sp_V3MstCertificateType_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public CertificateTypeDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<CertificateTypeDTO> lst = exec.GetByField<CertificateTypeDTO>("sp_V3MstCertificateType_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(CertificateTypeDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("CreatedBy", entity.CreatedBy);
            param.Add("CreatedAt", entity.CreatedAt);
            param.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3MstCertificateType_Insert", param, "ID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(CertificateTypeDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("UpdatedBy", entity.UpdatedBy);
            param.Add("UpdatedAt", entity.UpdatedAt);

            var result = exec.ExcuteScalar("sp_V3MstCertificateType_Update", param);
            return result;
        }
    }
}
