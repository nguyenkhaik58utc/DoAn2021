using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;

namespace ISO.API.DataAccess
{
    public class GetConnection
    {
        private static GetConnection dbInstance;
        private static readonly string conStr = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build().GetSection("ConnectionString").Value;
        private static readonly SqlConnection conn = new SqlConnection(conStr);

        private GetConnection()
        {
        }

        public static GetConnection getDbInstance()
        {
            if (dbInstance == null)
            {
                dbInstance = new GetConnection();
            }
            return dbInstance;
        }

        public SqlConnection GetDBConnection()
        {
            try
            {
                if(conn.State == ConnectionState.Closed)
                    conn.Open();
            }
            catch (SqlException e)
            {
               
            }
           
            return conn;
        }
    }
}
