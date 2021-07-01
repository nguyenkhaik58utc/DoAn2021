using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using zkemkeeper;

namespace ISO.API.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TimekeeperService : ITimekeeperService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimekeeperListRespDTO GetData()
        {
            TimekeeperListRespDTO resp = new TimekeeperListRespDTO();
           
            ExecuteQuery exec = new ExecuteQuery();
            resp.lstTimekeeper = exec.GetAll<TimekeeperDTO>("sp_Timekeeper_select",  commandType: CommandType.StoredProcedure);
          
            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TimekeeperDTO GetByID(int ID)
        {

            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };

            List<TimekeeperDTO> lst = exec.GetByField<TimekeeperDTO>("sp_Timekeeper_select", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timekeeper"></param>
        /// <returns></returns>
        public int Insert(TimekeeperDTO timekeeper)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new DynamicParameters();
            p.Add("Name", timekeeper.Name, DbType.String);
            p.Add("IP", timekeeper.IP, DbType.String);
            p.Add("Status", timekeeper.Status, DbType.Int32);
            p.Add("Post", timekeeper.Post, DbType.Int32);
            p.Add("Serial", timekeeper.Serial, dbType: DbType.String);

            var rs = exec.ExcuteScalar("sp_Timekeeper_Insert", p);
            return rs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timekeeper"></param>
        /// <returns></returns>
        public bool Update(TimekeeperDTO timekeeper)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var p = new
            {
                ID = timekeeper.ID,
                Name = timekeeper.Name,
                IP = timekeeper.IP,
                Status = timekeeper.Status,
                Post = timekeeper.Post,
                Serial = timekeeper.Serial
            };

            var rs = exec.ExcuteScalar("sp_Timekeeper_Update", p);
            return rs > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Delete(int ID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { ID = ID };
            int rs = exec.ExcuteScalar("sp_Timekeeper_Delete", param);
            return rs > 0;
        }

        /// <summary>
        /// input chấm công
        /// </summary>
        public bool InputTimekeeping(int TimkeeperID, string IP, int Post, out string Error)
        {
            try
            {
                var data = Getlogdata(TimkeeperID, IP, Post);
                if (data.Rows.Count > 0)
                {
                    // kiểm tra dữ liệu và thêm dữ liệu vào bảng Timekeepinghistory
                    SummaryTimekeepinghistory(data);
                }
                //thêm dữ liệu vào bảng timekeeping
                SummaryTimekeeping(DateTime.Now);
                Error = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }

        }

        // Lấy dữ liệu từ máy chấm công 
        private DataTable Getlogdata(int TimekeeperID, string IP, int Port)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("TimekeeperID", typeof(int));
                dt.Columns.Add("Code", typeof(int));
                dt.Columns.Add("DateTimekeeping", typeof(DateTime));

                string dwEnrollNumber1 = "";
                int dwVerifyMode;
                int dwInOutMode;
                int dwYear;
                int dwMonth;
                int dwDay;
                int dwHour;
                int dwMinute;
                int dwSecond;
                int dwWorkCode = 0;
                var MCC = new CZKEM();
                if (MCC.Connect_Net(IP, Port))
                {
                    MCC.ReadAllGLogData(TimekeeperID);
                    while (MCC.SSR_GetGeneralLogData(TimekeeperID, out dwEnrollNumber1, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
                    {
                        DateTime date = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                        
                        dt.Rows.Add(TimekeeperID, dwEnrollNumber1, date);
                    }
                }
                return dt;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Lỗi đọc logdata: {0}", e.Message));
            }
        }

        /// <summary>
        /// kiểm tra và thêm dữ liệu vào TimekeepingHistory
        /// </summary>
        /// <param name="data"></param>
        protected static void SummaryTimekeepinghistory(DataTable data)
        {
            //Sql_ADO.idasAdoService.ExecuteNoquery("sp_Timekeepinghistory_create_check", parameter: new { TimekeepinghistoryTable = data });
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { TimekeepinghistoryTable = data };
            int rs = exec.ExcuteScalar("sp_Timekeepinghistory_create_check", param);
        }

        /// <summary>
        /// thêm dữ liệu vào trong bảng timekeeping
        /// </summary>
        /// <param name="date"></param>
        protected static void SummaryTimekeeping(DateTime date)
        {
            //Sql_ADO.idasAdoService.ExecuteNoquery("sp_Timekeeping_Input", parameter: new { date = date });
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { date = date };
            int rs = exec.ExcuteScalar("sp_Timekeeping_Input", param);
        }

    }
}
