using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Infrastructor
{
    public interface IDbFactory
    {
        FosContext Init();
    }

    public class DbFactory : Disposable, IDbFactory
    {
        private FosContext dbContext;

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        public FosContext Init()
        {
            return dbContext ?? (dbContext = new FosContext());
            //dbContext.Employees.Open();

        }

    }
}
