namespace FOS.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExternalServiceAPIs",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    TypeService = c.String(),
                    JSONData = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.FavoriteRestaurants",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    RestaurantId = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.LoggingData",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    Thread = c.String(nullable: false),
                    Level = c.String(nullable: false),
                    Logger = c.String(nullable: false),
                    Message = c.String(),
                    Exception = c.String(),
                    SessionId = c.String(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Orders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    OrderDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Orders");
            DropTable("dbo.FavoriteRestaurants");
            DropTable("dbo.ExternalServiceAPIs");
            DropTable("dbo.LoggingData");
        }
    }
}
