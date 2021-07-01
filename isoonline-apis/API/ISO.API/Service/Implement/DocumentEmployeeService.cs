using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class DocumentEmployeeService : IDocumentEmployeeService
    {
        public List<HumanEmployeeResponse> GetListByDocumentID(int documentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new
            {
                DocumentID = documentID,
            };

            List<HumanEmployeeResponse> listProfilePermission = exec.GetByField<HumanEmployeeResponse>("sp_V3DocumentEmployee_GetByDocumentID", param, commandType: CommandType.StoredProcedure);
            return listProfilePermission;
        }

        public bool SavePermission(int documentID, List<DocumentEmployeeSaveDTO> lstEmployee)
        {
            ExecuteQuery exec = new ExecuteQuery();
            //Tạo bảng cấu trúc bảng ProfileHumanSaveModel map với type table trong Database
            DataTable dtAdd = new DataTable();
            dtAdd.Columns.Add("ID", typeof(Int32));
            dtAdd.Columns.Add("CheckedShow", typeof(bool));
            dtAdd.Columns.Add("CheckedEdit", typeof(bool));
            dtAdd.Columns.Add("CheckedDelete", typeof(bool));

            foreach (var item in lstEmployee)
            {
                dtAdd.Rows.Add(item.EmployeeID,item.CheckedShow,item.CheckedEdit,false);
            }
            var dynamicParameters = new DynamicParameters();
            //tao type table [dbo].[EmployeeIDList] trong sql server
            dynamicParameters.Add("lstEmployeeID", dtAdd.AsTableValuedParameter("[dbo].[EmployeeIDList1]"));
            dynamicParameters.Add("DocumentID", documentID);
            dynamicParameters.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = exec.ExcuteReturnValue("sp_V3DocumentEmployee_Save", dynamicParameters, "Total", commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}
