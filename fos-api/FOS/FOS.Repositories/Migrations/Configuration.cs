using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<FosContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CommandTimeout = int.MaxValue;
        }
        protected override void Seed(FosContext context)
        {

        }
    }
}
