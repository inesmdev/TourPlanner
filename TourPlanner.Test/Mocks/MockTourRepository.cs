using System;
using System.Collections.Generic;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;
using TourPlanner.Repositories;

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

        public bool Delete(Guid id)
        {
            foreach(Tour tour in tours)
            {
                if(tour.Id == id)
                {
                    tours.Remove(tour);
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Tour> GetAll()
        {
            return tours;
        }

        public Tour GetByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Tour entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<Tour>.Create(Tour entity)
        {
            throw new NotImplementedException();
        }
    }
}
