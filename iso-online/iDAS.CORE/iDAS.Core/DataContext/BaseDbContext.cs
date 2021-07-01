using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace iDAS.Core
{
    /// <summary>
    /// Base DbContext
    /// </summary>
    public class BaseDbContext
    {
        private BaseDbContext() { }

        private static BaseDbContext _Instance;
        public static BaseDbContext Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BaseDbContext();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// Get DbContext server
        /// </summary>
        public TDbContext GetDbContextCenter<TDbContext>() where TDbContext : DbContext
        {
            if (BaseDatabase.DatabaseCenter == null) return null;
            return GetDbContext<TDbContext>(BaseDatabase.ConnectionStringCenter);
        }

        /// <summary>
        /// Get DbContext client's code
        /// </summary>
        public TDbContext GetDbContextByCode<TDbContext>() where TDbContext : DbContext
        {
            if (BaseDatabase.DatabaseByCode == null) return null;
            return GetDbContext<TDbContext>(BaseDatabase.ConnectionStringByCode);
        }

        /// <summary>
        /// Get DbContext 
        /// </summary>
        public TDbContext GetDbContext<TDbContext>(Database database) where TDbContext : DbContext
        {
            var connectionString = BaseDatabase.GetConnectionString(database);
            var context = GetDbContext<TDbContext>(connectionString);
            return context;
        }

        /// <summary>
        /// Get DbContext can migration enabled
        /// </summary>
        public TDbContext GetDbContext<TDbContext>(string connectionString) where TDbContext : DbContext
        {
            var context = Activator.CreateInstance(typeof(TDbContext), connectionString);
            return context as TDbContext;
        }

        public bool MigrationDbContext<TDbContext>(Database database) where TDbContext : DbContext
        {
            var connectionString = BaseDatabase.GetConnectionString(database);
            var success = MigrationDbContext<TDbContext>(connectionString);
            return success;
        }

        public bool MigrationDbContext<TDbContext>(string connectionString) where TDbContext : DbContext
        {
            var success = true;
            try
            {
                connectionString = connectionString.Replace("metadata=res://*/;provider=System.Data.SqlClient; provider connection string=", "").Trim('"');
                var connectionInfo = new DbConnectionInfo(connectionString, "System.Data.SqlClient");
                DbContextInfo contextInfo = new DbContextInfo(typeof(TDbContext), connectionInfo);
                var migrationConfig = new DbMigrationsConfiguration<TDbContext>();
                migrationConfig.AutomaticMigrationDataLossAllowed = true;
                migrationConfig.AutomaticMigrationsEnabled = true;
                migrationConfig.TargetDatabase = connectionInfo;
                var migrator = new DbMigrator(migrationConfig);
                migrator.Update();
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public string MigrationDbContextThrowException<TDbContext>(Database database) where TDbContext : DbContext
        {
            var connectionString = BaseDatabase.GetConnectionString(database);
            var message = MigrationDbContextThrowException<TDbContext>(connectionString);
            return message;
        }

        public string MigrationDbContextThrowException<TDbContext>(string connectionString) where TDbContext : DbContext
        {
            var message = "Succeess!";
            try
            {
                connectionString = connectionString.Replace("metadata=res://*/;provider=System.Data.SqlClient; provider connection string=", "").Trim('"');
                var connectionInfo = new DbConnectionInfo(connectionString, "System.Data.SqlClient");
                DbContextInfo contextInfo = new DbContextInfo(typeof(TDbContext), connectionInfo);
                var migrationConfig = new DbMigrationsConfiguration<TDbContext>();
                migrationConfig.AutomaticMigrationDataLossAllowed = true;
                migrationConfig.AutomaticMigrationsEnabled = true;
                migrationConfig.TargetDatabase = connectionInfo;
                var migrator = new DbMigrator(migrationConfig);
                migrator.Update();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        public DbContext GetDbContext(Database database)
        {
            var connectionString = BaseDatabase.GetConnectionString(database);
            var context = GetDbContext(connectionString);
            return context;
        }

        public DbContext GetDbContext(string connectionString)
        {
            var context = Activator.CreateInstance(typeof(DbContext), connectionString);
            return context as DbContext;
        }
    }
}
