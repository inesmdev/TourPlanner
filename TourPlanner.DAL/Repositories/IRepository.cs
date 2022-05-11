using System;
using System.Collections.Generic;

namespace TourPlanner.Repositories
{
    public interface IRepository<T>
    {
        /*
         *  Create
         */
        public void Create(T entity);

        /*
         *  Read
         */
        public IEnumerable<T> GetAll(/*T criteria*/);

        public T GetByID(Guid id);

        /*
         * Update
         */
        public bool Update(T entity); 

        /*
         * Delete
         */
        public bool Delete(Guid id);
    }
}
