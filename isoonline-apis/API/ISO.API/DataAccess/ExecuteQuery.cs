using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ISO.API.DataAccess
{
    public class ExecuteQuery
    {
        private static GetConnection cs = GetConnection.getDbInstance();
        private IDbConnection connection = cs.GetDBConnection();
        /// <summary>
        /// Thực thi stored trả về list object
        /// </summary>
        /// <typeparam name="T">Loại dữ liệu trả về</typeparam>
        /// <param name="commandText">Tên stored proceduce</param>
        /// <param name="commandType">Loại command</param>
        /// <returns></returns>
        public List<T> GetAll<T>(string commandText, CommandType commandType = CommandType.StoredProcedure)
        {
            List<T> output = new List<T>();
            output = connection.Query<T>(commandText, commandType: commandType).AsList();
            return output;
        }
        /// <summary>
        /// Thực thi stored trả về list object
        /// </summary>
        /// <typeparam name="T">Loại dữ liệu trả về</typeparam>
        /// <param name="commandText">Tên stored proceduce</param>
        /// <param name="param">Param truyền vào</param>
        /// <param name="commandType">Loại command</param>
        /// <returns></returns>
        public List<T> GetByField<T>(string commandText, object param, CommandType commandType = CommandType.StoredProcedure)
        {
            List<T> output = new List<T>();
            output = connection.Query<T>(commandText, param, commandType: commandType).AsList();
            return output;
        }
        public int ExcuteScalar(string commandText, object param)
        {
            var rs = connection.Execute(commandText, param, commandType: CommandType.StoredProcedure);
            return rs;
        }
        /// <summary>
        /// Thực thi stored trả về kết quả
        /// </summary>
        /// <param name="StoreProcedureName">Tên stored</param>
        /// <param name="param">Param truyền vào</param>
        /// <param name="returnValueParamName">Tên param out put trong stored</param>
        /// <param name="commandType">Loại command</param>
        /// <returns></returns>
        public int ExcuteReturnValue(string StoreProcedureName, DynamicParameters param, string returnValueParamName, CommandType commandType = CommandType.StoredProcedure)
        {
            connection.Execute(StoreProcedureName, param, commandType: commandType);
            return param.Get<int>(returnValueParamName);
        }
        /// <summary>
        /// Thực thi stored trả về paging
        /// </summary>
        /// <typeparam name="T">Loại dữ liệu trả về</typeparam>
        /// <param name="totalRows">Tổng số bản ghi trả về</param>
        /// <param name="commandText">Tên stored</param>
        /// <param name="param">Param truyền vào</param>
        /// <param name="returnValueParamName">Tên param out put trong stored</param>
        /// <param name="commandType">Loại command</param>
        /// <returns></returns>
        public List<T> GetPaging<T>(out int totalRows, string commandText, DynamicParameters param, string returnValueParamName, CommandType commandType = CommandType.StoredProcedure)
        {
            List<T> output = new List<T>();
            output = connection.Query<T>(commandText, param, commandType: commandType).ToList();
            totalRows = param.Get<int>(returnValueParamName);
            return output;
        }
        public int Execute(string commandName, object[] param, CommandType commandType = CommandType.StoredProcedure)
        {
            var effect = connection.Execute(commandName, param, commandType: commandType);
            return effect;
        }
        public SqlMapper.GridReader QueryMulti(string commandName, object param, CommandType commandType = CommandType.StoredProcedure)
        {
            var multi = connection.QueryMultiple(commandName, param, commandType: commandType);
            return multi;
        }
    }
}
