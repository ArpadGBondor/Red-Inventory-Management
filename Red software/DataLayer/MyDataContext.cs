using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using EntityLayer;
using System.Data.Linq.Mapping;

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

    }
}
