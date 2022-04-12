using System;
using System.Collections.Generic;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;

namespace TourPlanner.Test.Mocks
{
    public class MockTourRepository : ITourRepository
    {
        List<Tour> tours = new List<Tour>();


        public bool Create(Tour tour)
        {
            tours.Add(tour);

            return true;
        }

        public bool Delete(Tour entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tour> GetAll()
        {
            throw new NotImplementedException();
        }

        public Tour GetByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Tour entity)
        {
            throw new NotImplementedException();
        }
    }
}
