using EntityLayer;
using System.Data.Linq.Mapping;
using System.Data.Entity;

namespace DataLayer
{
    /// <summary>
    /// Database context for the Inventory database.
    /// </summary>
    public class InventoryContext : DbContext
    {
        private static MappingSource _mappingSource = new AttributeMappingSource();

        public InventoryContext() : base() { }

        public InventoryContext(string connectionstring) :base(connectionstring) { }

        public InventoryContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PartnerEntity> Partners { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        public DbSet<TransactionBodyEntity> TransactionBody { get; set; }
        public DbSet<TransactionHeadEntity> TransactionHeader { get; set; }
    }
}