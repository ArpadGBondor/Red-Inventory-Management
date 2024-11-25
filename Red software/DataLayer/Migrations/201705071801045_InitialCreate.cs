namespace DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Partners",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(),
                    Name = c.String(),
                    Customer = c.Boolean(nullable: false),
                    Dealer = c.Boolean(nullable: false),
                    Address = c.String(),
                    AccountNumber = c.String(),
                    Phone = c.String(),
                    Email = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdCategory",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Category = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(),
                    Name = c.String(),
                    CategoryId = c.Int(nullable: false),
                    CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    SellPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TransactionBody",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TransactionId = c.Int(nullable: false),
                    Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ProductId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TransactionHeader",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Incoming = c.Boolean(nullable: false),
                    PartnerId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Username = c.String(nullable: false, maxLength: 128),
                    Password = c.String(),
                })
                .PrimaryKey(t => t.Username);

        }

        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.TransactionHeader");
            DropTable("dbo.TransactionBody");
            DropTable("dbo.Products");
            DropTable("dbo.ProdCategory");
            DropTable("dbo.Partners");
        }
    }
}
