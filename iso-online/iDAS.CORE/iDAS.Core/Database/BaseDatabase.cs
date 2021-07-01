using System.Data.Entity;
using System.Web;

namespace iDAS.Core
{
    /// <summary>
    /// Base Database
    /// </summary>
    public class BaseDatabase
    {
        private static Database getFileConfig()
        {
            var database = HttpContext.Current.Application["Database"] as Database;
            if (database == null)
            {
                var path = HttpContext.Current.Server.MapPath(BaseConfig.ConfigFilePath);
                if (System.IO.File.Exists(path))
                {
                    var file = System.IO.File.OpenText(path);
                    var content = file.ReadToEnd();
                    var result = iDAS.Core.Encryptor.Decode(content).Split('/');
                    database = new Database()
                    {
                        DataSource = result[0],
                        DataName = result[1],
                        UserId = result[2],
                        Password = result[3],
                    };

                    // HungNM. The temporary fixing.
                    //database = new Database()
                    //{
                    //    DataSource = "171.244.139.20\\MSSQLSERVER2012,50161",
                    //    DataName = "iDAS.CENTER.NEW",
                    //    UserId = "sa",
                    //    Password = "idas@2015",
                    //};
                    // End.

                    HttpContext.Current.Application["Database"] = database;
                    file.Close();
                }
            }
            return database;
        }
        private static void setFileConfig(Database database)
        {
            HttpContext.Current.Application["Database"] = database;
            var path = HttpContext.Current.Server.MapPath(BaseConfig.ConfigFilePath);
            if (database != null)
            {
                var file = System.IO.File.CreateText(path);
                var content = database.DataSource + '/' + database.DataName + '/' + database.UserId + '/' + database.Password;
                file.Write(Encryptor.Encode(content));
                file.Close();
            }
            else
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        internal static Database DatabaseCenter
        {
            get
            {
                return getFileConfig();
            }
            set
            {
                setFileConfig(value);
            }
        }
        internal static Database DatabaseByCode
        {
            get
            {
                return HttpContext.Current.Session["Database"] as Database;
            }
            set
            {
                HttpContext.Current.Session["Database"] = value;
            }
        }

        internal static string ConnectionStringCenter
        {
            get
            {
                var connectionString = GetConnectionString(DatabaseCenter);
                return connectionString;
            }
        }
        internal static string ConnectionStringByCode
        {
            get
            {
                var connectionString = GetConnectionString(DatabaseByCode);
                return connectionString;
            }
        }

        private static string _ConnectionString = "metadata=res://*/;provider=System.Data.SqlClient; provider connection string=\"Server={0};Database={1};user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework\"";

        public static string GetConnectionString(Database database)
        {
            var connectionString = string.Format(_ConnectionString, database.DataSource, database.DataName, database.UserId, database.Password);
            return connectionString;
        }

        public static Database GetDatabaseCenter()
        {
            return DatabaseCenter;
        }

        public static Database GetDatabaseByCode()
        {
            return DatabaseByCode;
        }

        public static void SetDatabaseByCode(Database database)
        {
            DatabaseByCode = database;
        }

        public static void CreateDatabase(string connectionString)
        {
            var context = BaseDbContext.Instance.GetDbContext(connectionString);
            if (!context.Database.Exists()) context.Database.Create();
        }

        public static void CreateDatabase(Database database)
        {
            var context = BaseDbContext.Instance.GetDbContext(database);
            if (!context.Database.Exists()) context.Database.Create();
        }

        public static void DeleteDatabase(string connectionString)
        {
            var context = BaseDbContext.Instance.GetDbContext(connectionString);
            if (context.Database.Exists()) context.Database.Delete();
        }

        public static void DeleteDatabase(Database database)
        {
            var context = BaseDbContext.Instance.GetDbContext(database);
            if (context.Database.Exists()) context.Database.Delete();
        }

        public static bool CheckDatabaseExist(string connectionString)
        {
            DbContext db = new DbContext(connectionString);
            return db.Database.Exists();
        }

        public static bool CheckDatabaseExist(Database database)
        {
            var connectionString = GetConnectionString(database);
            return CheckDatabaseExist(connectionString);
        }
    }
}
