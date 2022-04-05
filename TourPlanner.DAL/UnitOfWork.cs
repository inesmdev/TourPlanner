using System;

namespace TourPlanner.DAL
{
    class UnitOfWork : IDisposable
    {
        //Postgres Access
        private PostgresAccess db;

        UnitOfWork()
        {

        }


        /*
         *  IDisposeable
         */
        private bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //_conn.dispose
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //???
        }
    }
}
