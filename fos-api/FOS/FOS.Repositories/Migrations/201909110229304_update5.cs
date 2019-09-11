namespace FOS.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update5 : DbMigration
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
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IdUser = c.String(),
                        IdEvent = c.String(),
                        IdRestaurant = c.Int(nullable: false),
                        IdDelivery = c.Int(nullable: false),
                        FoodDetail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
            DropTable("dbo.FavoriteRestaurants");
            DropTable("dbo.ExternalServiceAPIs");
        }
    }
}
