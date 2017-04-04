using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using EntityLayer;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace DataLayer
{
    public class MyDataContext : DataContext
    {

        private static MappingSource mappingSource = new AttributeMappingSource();

        public MyDataContext(string connection) 
               :base(connection, mappingSource)
        {

        }

        public MyDataContext(System.Data.IDbConnection connection)  
               :base(connection, mappingSource)
        {

        }

        public Table<UserEntity> Users { get { return this.GetTable<UserEntity>(); } }
        public Table<PartnerEntity> Partners { get { return this.GetTable<PartnerEntity>(); } }
        public Table<ProductEntity> Products { get { return this.GetTable<ProductEntity>(); } }
        public Table<ProductCategoryEntity> ProductCategories { get { return this.GetTable<ProductCategoryEntity>(); } }
        public Table<TransactionBodyEntity> TransactionBody { get { return this.GetTable<TransactionBodyEntity>(); } }
        public Table<TransactionHeadEntity> TransactionHead { get { return this.GetTable<TransactionHeadEntity>(); } }

        public bool TableExists<T>()
        {
            TableAttribute attribute = (TableAttribute)typeof(T)
                                       .GetCustomAttributes(typeof(TableAttribute), true)
                                       .Single();

            var result = ExecuteQuery<bool>(
                          String.Format(
                            "IF OBJECT_ID('{0}', 'U') IS NOT NULL SELECT CAST(1 AS BIT) ELSE SELECT CAST(0 AS BIT)", attribute.Name));

            return result.First();
        }
        public void CreateTable<T>()
        {
            var metaTable = Mapping.GetTable(typeof(T));
            var typeName = "System.Data.Linq.SqlClient.SqlBuilder";
            var type = typeof(DataContext).Assembly.GetType(typeName);
            var bf = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            var sql = type.InvokeMember("GetCreateTableCommand", bf, null, null, new[] { metaTable });
            var sqlAsString = sql.ToString();
            ExecuteCommand(sqlAsString);
        }

        public bool DatabaseExists(string Directory, string Database)
        {
            string Identifier = Generate_Identifier_Name(Directory, Database);
            var result = ExecuteQuery<bool>(
              String.Format(
                "IF DB_ID('{0}') IS NOT NULL SELECT CAST(1 AS BIT) ELSE SELECT CAST(0 AS BIT)", Identifier));
            return result.First();
        }

        public bool CreateDatabase(string Directory,string Database)
        {
            string DbName = Regex.Replace(Database, "\\s+", "");
            string Identifier = Generate_Identifier_Name(Directory, Database);
            bool lSuccess = false;

            if (DatabaseExists(Directory, Database))
            {
                if (!File.Exists(Directory + Database + ".MDF"))
                {
                    string sql = string.Format(@"DROP DATABASE [{0}]", Identifier);
                    try
                    {
                        ExecuteCommand(sql);
                        lSuccess = true;
                    }
                    catch /*(Exception e)*/ { }
                }
            }
            else
                lSuccess = true;

            if (lSuccess)
            {
                string sql = string.Format(@"CREATE DATABASE [{3}] ON PRIMARY ( NAME={2}_data, FILENAME = '{0}{1}.MDF' ) LOG ON ( NAME={2}_log, FILENAME = '{0}{1}.ldf' )", Directory, Database, DbName, Identifier);
                lSuccess = false;
                try
                {
                    ExecuteCommand(sql);
                    lSuccess = true;
                }
                catch /*(Exception e)*/ { }
            }
            return lSuccess;
        }

        public static string Generate_Identifier_Name(string Directory, string Database)
        {
            byte[] salt = { };
            string Identifier = "Red_Inventory_Management@" + EncriptionProvider.ComputeHash(Directory + Database, EncriptionProvider.Supported_HA.SHA256, salt);
            return Identifier;
        }
    }
}