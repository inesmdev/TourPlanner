using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;

namespace TourPlanner.BL.TourService
{
    public class TourService : ITourService
    {
        ITourRepository _repository; // _  -> being created externaly
                                     

        public TourService(ITourRepository repository) =>
            _repository = repository;


        public Tour AddTour(TourInputData tourinput)
        {
            Tour tour = new Tour();
            tour.TourId = Guid.NewGuid();
            tour.Tourname = tourinput.Tourname;
            
            
            // Parse the Data


            // Call Map Quest API


            // Save new Tour in DB


            _repository.Create(tour);

            return tour;
        }


        public List<Tour> GetAllTours() =>
            _repository.GetAll().ToList();




        // Private stuff


        
    }
}
