using System.Collections.Generic;
using TourPlanner.DAL;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    public static class TourController
    {
        // Tour Repos
        static IRepository<Tour> tourrepos = new TourRepository();

        // Collection of Tours

        // Get Tour
        public static IEnumerable<Tour> GetAllTours()
        {
            IEnumerable<Tour> tours = tourrepos.GetAll();
            return tours;
        } 

        // Add Tour
        public static bool AddTour(Tour tour)
        {           
            // Add Tour
            tourrepos.Create(tour);

            // Fetch Tour by Id and return Tour 
            //?

            return true;
        }

        // Delete Tour

        // EditTour
    }
}
