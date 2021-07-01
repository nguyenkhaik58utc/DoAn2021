using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.DocumentFolder;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class DocumentFolderService : IDocumentFolderService
    {
        public int Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int result = exec.ExcuteScalar("sp_V3DocumentFolder_Delete", new { ID = ID });
            return result;
        }

        public List<DocumentFolderDTO> GetAll(int DocumentOut)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("DocumentOut", DocumentOut);
            List<DocumentFolderDTO> lst = exec.GetByField<DocumentFolderDTO>("sp_V3DocumentFolder_GetAll", param, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public DocumentFolderDTO GetByID(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<DocumentFolderDTO> lst = exec.GetByField<DocumentFolderDTO>("sp_V3DocumentFolder_GetByID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public DocumentFolderDTO GetByName(string Name,bool DocumentOut,int ParentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { Name = Name , DocumentOut = DocumentOut, ParentID = ParentID };

            List<DocumentFolderDTO> lst = exec.GetByField<DocumentFolderDTO>("sp_V3DocumentFolder_GetByName", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public int Insert(DocumentFolderDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("Name", entity.Name);
            param.Add("Note", entity.Note);
            param.Add("ParentID", entity.ParentID);
            param.Add("CreateBy", entity.CreateBy);
            param.Add("IsDelete", entity.IsDelete);
            param.Add("DocumentOut", entity.DocumentOut);
            param.Add("CreateAt", entity.CreateAt);
            param.Add("ReturnID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3DocumentFoder_InsUpd", param, "ReturnID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public int Update(DocumentFolderDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("Name", entity.Name);
            param.Add("Note", entity.Note);
            param.Add("DocumentOut", entity.DocumentOut);
            param.Add("ParentID", entity.ParentID);
            param.Add("IsDelete", entity.IsDelete);
            param.Add("UpdateBy", entity.UpdateBy);
            param.Add("UpdateAt", entity.UpdateAt); 
            param.Add("ReturnID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3DocumentFoder_InsUpd", param, "ReturnID", commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
