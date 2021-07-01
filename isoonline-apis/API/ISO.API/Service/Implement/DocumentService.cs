using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISO.API.Service
{
    public class DocumentService : IDocumentService
    {
        public DocumentListRespDTO GetData(bool type, int folderID, int pageIndex, int pageSize, int userID, int employeeID)
        {
            DocumentListRespDTO resp = new DocumentListRespDTO();
            DynamicParameters param = new DynamicParameters();
            param.Add("UserID", userID, dbType: DbType.Int32);
            param.Add("EmployeeID", employeeID, dbType: DbType.Int32);
            param.Add("Type", type, dbType: DbType.Boolean);
            param.Add("FolderID", folderID, dbType: DbType.Int32);
            param.Add("PageIndex", pageIndex, dbType: DbType.Int32);
            param.Add("PageSize", pageSize, dbType: DbType.Int32);
            param.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            ExecuteQuery exec = new ExecuteQuery();
            resp.lstDocument = exec.GetPaging<DocumentList>(out int totalRows, "sp_Document_GetByFolderID", param, "Total", commandType: CommandType.StoredProcedure);
            resp.total = totalRows;
            resp.pageIndex = pageIndex;
            resp.pageSize = pageSize;
            return resp;
        }

        public int SavePublicPermission(int documentId, bool isPublic)
        {
            ExecuteQuery exec = new ExecuteQuery();

            var dynamicParameters = new DynamicParameters();
            //tao type table [dbo].[EmployeeIDList] trong sql server
            dynamicParameters.Add("DocumentID", documentId, dbType: DbType.Int32);
            dynamicParameters.Add("IsPublic", isPublic, dbType: DbType.Boolean);
            var result = exec.ExcuteScalar("sp_V3DocumentPublic_Save", dynamicParameters);
            return result;
        }

        public bool CheckQuickDownload(Guid fileID )
        {
            ExecuteQuery exec = new ExecuteQuery();

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@fileID", fileID, dbType: DbType.Guid);
            var result = exec.GetByField<DocumentDownload>("sp_Document_CheckDownloadFile", dynamicParameters);
            if (result != null && result.Count > 0)
                return true;
            return false;
        }
    }
}
