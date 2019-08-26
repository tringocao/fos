using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Infrastructor
{
    public class Disposable : IDisposable
    {
        private Boolean disposed = false;
        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DisposeCore();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //this method is implemented by DbFactory
        protected virtual void DisposeCore()
        {

        }
    }
}
