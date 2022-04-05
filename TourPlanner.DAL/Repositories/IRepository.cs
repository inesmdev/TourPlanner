using System;
using System.Collections.Generic;

namespace TourPlanner.DAL
{
    public interface IRepository<T>
    {
        /*
         *  Create
         */
        public bool Create(T entity);

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
        public bool Delete(T entity);
    }
}
