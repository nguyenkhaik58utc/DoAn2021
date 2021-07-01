using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using ISO.API.Models.HumanEmployee;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class HumanEmployeeService : IHumanEmployeeService
    {
        private readonly IDepartmentTitleService _departmentTitleService;

        public HumanEmployeeService(IDepartmentTitleService departmentTitleService)
        {
            _departmentTitleService = departmentTitleService;
        }

        public List<HumanEmployeeDTO> GetByDepartment(int departmentID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { DepartmentID = departmentID };
            List<HumanEmployeeDTO> humanEmployeeDTOs = exec.GetByField<HumanEmployeeDTO>("sp_GetHumanInDepartment", param, commandType: CommandType.StoredProcedure);
            return humanEmployeeDTOs;
        }

        public List<HumanEmployeeResponse> GetListALL(out int totalRows, int pageSize, int pageNumber)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@PageSize", pageSize, DbType.Int32);
            param.Add("@PageNumber", pageNumber, DbType.Int32);
            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanEmployeeResponse> humanEmployeeDTOs = exec.GetPaging<HumanEmployeeResponse>(out totalRows, "[sp_V3HumanEmployee_GetAll]", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return humanEmployeeDTOs;
        }

        public List<HumanEmployeeResponse> GetListByDepartmentID(out int totalRows, HumanEmployeeSearchRequest req)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@DepartmentID", req.departmentID, DbType.Int32);
            param.Add("@PageSize", req.pageSize, DbType.Int32);
            param.Add("@PageNumber", req.pageNumber, DbType.Int32);

            if (!string.IsNullOrWhiteSpace(req.name))
                param.Add("@name", req.name, DbType.String);
            if (req.birthDayFrom != null)
                param.Add("@birthDayFrom", req.birthDayFrom, DbType.Date);
            if (req.birthDayTo != null)
                param.Add("@birthDayTo", req.birthDayTo, DbType.Date);
            if (!string.IsNullOrWhiteSpace(req.native))
                param.Add("@native", req.native, DbType.String);
            if (req.religion > 0)
                param.Add("@religion", req.religion, DbType.Int32);
            if (req.ethnic > 0)
                param.Add("@ethnic", req.ethnic, DbType.Int32);

            if (!string.IsNullOrWhiteSpace(req.placeOfWork))
                param.Add("@placeOfWork", req.placeOfWork, DbType.String);
            if (!string.IsNullOrWhiteSpace(req.position))
                param.Add("@position", req.position, DbType.String);
            if (!string.IsNullOrWhiteSpace(req.departmentName))
                param.Add("@departmentName", req.departmentName, DbType.String);

            if (!string.IsNullOrWhiteSpace(req.eduName))
                param.Add("@eduName", req.eduName, DbType.String);
            if (req.educationType > 0)
                param.Add("@educationType", req.educationType, DbType.Int32);
            if (req.educationResult > 0)
                param.Add("@educationResult", req.educationResult, DbType.Int32);

            if (!string.IsNullOrWhiteSpace(req.diplomaName))
                param.Add("@diplomaName", req.diplomaName, DbType.String);
            if (req.major > 0)
                param.Add("@major", req.major, DbType.Int32);
            if (req.diplomaEucationType > 0)
                param.Add("@diplomaEucationType", req.diplomaEucationType, DbType.Int32);
            if (req.diplomaEducationOrg > 0)
                param.Add("@diplomaEducationOrg", req.diplomaEducationOrg, DbType.Int32);
            if (req.educationLevel > 0)
                param.Add("@educationLevel", req.educationLevel, DbType.Int32);
            if (req.certificateLevel > 0)
                param.Add("@certificateLevel", req.certificateLevel, DbType.Int32);

            if (!string.IsNullOrWhiteSpace(req.certificateName))
                param.Add("@certificateName", req.certificateName, DbType.String);
            if (req.level > 0)
                param.Add("@level", req.level, DbType.Int32);
            if (req.certificatetype > 0)
                param.Add("@certificatetype", req.certificatetype, DbType.Int32);

            if (!string.IsNullOrWhiteSpace(req.numberOfDecision))
                param.Add("@numberOfDecision", req.numberOfDecision, DbType.String);
            if (!string.IsNullOrWhiteSpace(req.reason))
                param.Add("@reason", req.reason, DbType.String);
            if (req.form > 0)
                param.Add("@form", req.form, DbType.String);

            //@isLeave
            if (req.isLeave != null)
                param.Add("@isLeave", req.isLeave, DbType.Boolean);

            param.Add("@TotalRows", DbType.Int32, direction: ParameterDirection.Output);
            List<HumanEmployeeResponse> humanEmployeeDTOs = exec.GetPaging<HumanEmployeeResponse>(out totalRows, "sp_V3HumanEmployee_GetByDepartmentID", param, "TotalRows", commandType: CommandType.StoredProcedure);
            return humanEmployeeDTOs;
        }

        public List<HumanEmployeeResponse> GetByListEmployeeId(string lstID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var lst = JsonConvert.DeserializeObject<List<int>>(lstID);
            //Tạo bảng cấu trúc bảng ProfileHumanSaveModel map với type table trong Database
            DataTable dtAdd = new DataTable();
            dtAdd.Columns.Add("ID", typeof(Int32));
            //Gán dữ liệu vào bảng
            for (int i = 0; i < lst.Count; i++)
            {
                dtAdd.Rows.Add(lst[i]);
            }
            var dynamicParameters = new DynamicParameters();
            //tao type table [dbo].[ProfileHumanSaveModel] trong sql server
            dynamicParameters.Add("@lstEmployeeId", dtAdd.AsTableValuedParameter("[dbo].[EmployeeIDList]"));
            List<HumanEmployeeResponse> humanEmployeeDTOs = exec.GetByField<HumanEmployeeResponse>("sp_V3HumanEmployee_GetByListEmployeeId", dynamicParameters, commandType: CommandType.StoredProcedure);
            return humanEmployeeDTOs;
        }

        public List<HumanEmployeeResponse> GetByDocument(int documentId)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@DocumentId", documentId, DbType.Int32);
            List<HumanEmployeeResponse> humanEmployeeDTOs = exec.GetByField<HumanEmployeeResponse>("sp_V3HumanEmployee_GetByDocument", param, commandType: CommandType.StoredProcedure);
            return humanEmployeeDTOs;
        }

        public HumanEmployeeResponse GetById(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", id, DbType.Int32);
            HumanEmployeeResponse item = exec.GetByField<HumanEmployeeResponse>("sp_V3HumanEmployee_GetById", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            item.ListDepartmentTitle = _departmentTitleService.GetListByEmployeeID(id);
            return item;
        }
    }
}