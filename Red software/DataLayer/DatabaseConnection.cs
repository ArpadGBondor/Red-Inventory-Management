using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Reflection;
using System.Linq.Expressions;
using EntityLayer;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Configuration;

namespace DataLayer
{
    public class DatabaseConnection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string _server;
        private static string _connectionString;
        public static string ConnectionString { get { return _connectionString; } }
        private static string _directory;
        public static string Directory { get { return _directory; } }
        private static string _dbName;
        public static string DbName { get { return _dbName; } }

        /// <summary>
        /// The static constructor sets the default values to the private variables.
        /// </summary>
        static DatabaseConnection()
        {
            _server = "(LocalDB)\\MSSQLLocalDB";
            _directory = "";
            _dbName = "";
            _connectionString = "Data Source=" + _server + ";AttachDbFilename=\"" + Directory + DbName + ".mdf\";Integrated Security=True;";
        }

        /// <summary>
        /// Sets the new connection string if the database file exists. 
        /// Returns false if the database file does not exist.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static bool InitializeConnection(string directory, string dbName)
        {
            bool result = File.Exists(directory + dbName + ".mdf");
            if (result)
            {
                DatabaseConnection._directory = directory;
                DatabaseConnection._dbName = dbName;
                DatabaseConnection._connectionString = "Data Source=" + _server + ";AttachDbFilename=\"" + Directory + DbName + ".mdf\";Integrated Security=True;";
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<InventoryContext, Migrations.Configuration>());
                using (var db = new InventoryContext(_connectionString))
                {
                    result = db.Database.Exists();
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if a database can be reached with the connection string.
        /// </summary>
        /// <returns></returns>
        public static bool Test()
        {
            bool result = false;
            using (var db = new InventoryContext(_connectionString))
            {
                result = db.Database.Exists();
            }
            return result;
        }
        
        /// <summary>
        /// Creates a database file if it does not exists.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static bool CreateDatabase(string directory, string dbName)
        {
            bool result = false;
            string connectionString = "Data Source=" + _server + ";AttachDbFilename=\"" + directory + dbName + ".mdf\";Integrated Security=True;";
            using (var db = new InventoryContext(connectionString))
            {
                try
                {
                    db.Database.CreateIfNotExists();
                    result = true;
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Create database error\nFolder: {0}\nDatabase: {1}",directory,dbName), ex);
                }
            }
            if (result)
                InitializeConnection(directory, dbName);
            return result;
        }

        /// <summary>
        /// Lists every record from a table where the condition returns true
        /// </summary>
        /// <typeparam name="Entity">Table type in the database</typeparam>
        /// <param name="condition">Condition on the records of the table.</param>
        /// <returns></returns>
        public static List<Entity> ListTable<Entity>(Expression<Func<Entity, bool>> condition) where Entity : class
        {
            List<Entity> list = new List<Entity>(); ;
            using (var db = new InventoryContext(_connectionString))
            {
                DbSet<Entity> EntityTable = db.Set<Entity>();
                var query = EntityTable.Where(condition);
                list.AddRange(query);
            }
            return list;
        }

        /// <summary>
        /// Checks if there is at least one record in a database table where the condition returns true.
        /// </summary>
        /// <typeparam name="Entity">Table type in the database</typeparam>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static bool IsExist<Entity>(Expression<Func<Entity, bool>> condition) where Entity : class
        {
            bool exists = false;
            using (var db = new InventoryContext(_connectionString))
            {
                DbSet<Entity> EntityTable = db.Set<Entity>();
                var query = EntityTable.Where(condition);
                exists = (query.Count() > 0);
            }
            return exists;
        }
        /// <summary>
        /// Adds a record to a database table.
        /// </summary>
        /// <typeparam name="Entity">Table type in the database</typeparam>
        /// <param name="record"></param>
        /// <returns></returns>
        public static bool Add<Entity>(Entity record) where Entity : class
        {
            bool result = false;
            using (var db = new InventoryContext(_connectionString))
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        DbSet<Entity> EntityTable = db.Set<Entity>();
                        EntityTable.Add(record);

                        db.SaveChanges();
                        dbTransaction.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        log.Error(string.Format("Cannot add a record to the database."), ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Modifies a record in a database table
        /// </summary>
        /// <typeparam name="Entity">Table type in the database</typeparam>
        /// <param name="record">A record with updated values.</param>
        /// <param name="condition">A condition on the database table, that selects the record. Eg. (p => p.Id == record.Id) </param>
        /// <returns></returns>
        public static bool Modify<Entity>(Entity record, Expression<Func<Entity,bool>> condition) where Entity : class
        {
            bool result = false;
            bool exists = false;
            using (var db = new InventoryContext(_connectionString))
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        DbSet<Entity> EntityTable = db.Set<Entity>();
                        var query = EntityTable.Where(condition);
                        foreach (var rec in query)
                        {
                            exists = true;

                            PropertyInfo[] properties = EntityCloner.GetProperties(typeof(Entity));
                            foreach (PropertyInfo property in properties)
                            {
                                property.SetValue(rec, property.GetValue(record));
                            }
                        }
                        if (exists)
                        {
                            db.SaveChanges();
                            dbTransaction.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        log.Error(string.Format("Cannot modify a record in the database."), ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes every record from a database table where the condition returns true.
        /// </summary>
        /// <typeparam name="Entity">Table type in the database</typeparam>
        /// <param name="condition">A condition on the database table, that selects the records. Eg. (p => p.Id == Id) </param>
        /// <returns></returns>
        public static bool Remove<Entity>(Expression<Func<Entity, bool>> condition) where Entity : class
        {
            bool result = false;
            bool exists = false;
            using (var db = new InventoryContext(_connectionString))
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        DbSet<Entity> EntityTable = db.Set<Entity>();
                        var query = EntityTable.Where(condition);
                        foreach (var rec in query)
                        {
                            exists = true;
                            EntityTable.Remove(rec);
                        }
                        if (exists)
                        {
                            db.SaveChanges();
                            dbTransaction.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        log.Error(string.Format("Cannot remove a record from the database."), ex);
                    }
                }
            }
            return result;
        }
    }
}
