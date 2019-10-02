using FOS.Repositories.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories
{
    public class FosContext : DbContext
    {
        public FosContext()
            : base("name=FosContext")
        {

        }

        public FosContext(string connStringOrName)
            : base(connStringOrName)
        {

        }

        public DbSet<DataModel.Order> Orders { get; set; }
        public DbSet<DataModel.FeedBack> FeedBacks { get; set; }
        public DbSet<ExternalServiceAPI> ExternalServiceAPIs { get; set; }
        public DbSet<FavoriteRestaurant> FavoriteRestaurants { get; set; }
        public DbSet<RecurrenceEvent> RecurrenceEvents { get; set; }
        public DbSet<LoggingData> LoggingDatas { get; set; }
        public DbSet<EventPromotion> EventPromotions { get; set; }

        public DbSet<ReportFile> ReportFiles { get; set; }
        public DbSet<CustomGroup> CustomGroups { get; set; }
        public DbSet<GraphUserInGroup> GraphUserInGroup { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}