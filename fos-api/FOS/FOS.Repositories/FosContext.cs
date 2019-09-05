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
        public DbSet<ExternalServiceAPI> ExternalServiceAPIs { get; set; }
        public DbSet<FavoriteRestaurant> FavoriteRestaurants { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}