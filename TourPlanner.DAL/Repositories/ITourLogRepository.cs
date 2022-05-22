using System;
using System.Collections.Generic;
using TourPlanner.Models;
using TourPlanner.Repositories;

namespace TourPlanner.DAL.Repositories
{
    public interface ITourLogRepository : IRepository<TourLog>
    {
        public IEnumerable<TourLog> GetAll(Guid id);
    }
}
