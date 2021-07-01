using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.Document;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class DocumentDepartmentService : IDocumentDepartmentService
    {
        public List<DocumentDepartmentDTO> GetListByDocumentID(int documentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new
            {
                DocumentID = documentID,
            };

            List<DocumentDepartmentDTO> listProfilePermission = exec.GetByField<DocumentDepartmentDTO>("sp_V3DocumentDepartment_GetByDocumentID", param, commandType: CommandType.StoredProcedure);
            return listProfilePermission;
        }

        public bool SavePermission(int documentID, List<DocumentDepartmentDTO> lstDepartment)
        {
            ExecuteQuery exec = new ExecuteQuery();
            //Tạo bảng cấu trúc bảng ProfileHumanSaveModel map với type table trong Database
            DataTable dtAdd = new DataTable();
            dtAdd.Columns.Add("DepartmentID", typeof(Int32));
            dtAdd.Columns.Add("IsShow", typeof(bool));
            dtAdd.Columns.Add("IsUpdate", typeof(bool));

            foreach (var item in lstDepartment)
            {
                dtAdd.Rows.Add(item.DepartmentID,item.IsShow,item.IsUpdate);
            }
            var dynamicParameters = new DynamicParameters();
            //tao type table [dbo].[EmployeeIDList] trong sql server
            dynamicParameters.Add("lstDepartmentID", dtAdd.AsTableValuedParameter("[dbo].[DepartmentList]"));
            dynamicParameters.Add("DocumentID", documentID);
            dynamicParameters.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3DocumentDepartment_Save", dynamicParameters, "Total", commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}
