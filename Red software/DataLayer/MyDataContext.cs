using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using EntityLayer;
using System.Data.Linq.Mapping;
using System.Reflection;

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

    }
}
