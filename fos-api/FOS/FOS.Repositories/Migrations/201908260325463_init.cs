namespace FOS.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodServiceAPIs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TypeService = c.String(),
                        JSONData = c.String(),
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

            CreateTable(
                "dbo.FavoriteRestaurants",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    RestaurantId = c.String(),
                })
                .PrimaryKey(t => t.ID);

        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
            DropTable("dbo.FoodServiceAPIs");
            DropTable("dbo.FavoriteRestaurants");
        }
    }
}
