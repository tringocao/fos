using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Infrastructor
{
    public interface IDbFactory
    {
        APIExternalServiceEntities Init();
    }

    public class DbFactory : Disposable, IDbFactory
    {
        private APIExternalServiceEntities dbContext;

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        public APIExternalServiceEntities Init()
        {
            return dbContext ?? (dbContext = new APIExternalServiceEntities());
            //dbContext.Employees.Open();

        }

    }
}
