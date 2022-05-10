using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;
using Serilog;
using TourPlanner.Api.Services.MapQuestService;
using Microsoft.Extensions.Logging;

namespace TourPlanner.Api.Services
{
    public class TourService : ITourService
    {
        ITourRepository _repository; // _  -> being created externaly
        IMapQuestService _mapQuestService;
        ILogger<TourService> _tourlogger;
                                     
        public TourService(ITourRepository repository, IMapQuestService mapapi, ILogger<TourService> logger)
        {
            _repository = repository;
            _mapQuestService = mapapi;
            _tourlogger = logger;
        }

        public Tour? Add(TourInput tourinput)
        {
            Tour tour = new Tour();
            tour.Id = Guid.NewGuid();
            tour.Name = tourinput.Name;
            tour.Description = tourinput.Description;
            tour.From = tourinput.From;
            tour.To = tourinput.To;

            try
            {
                string res = _mapQuestService.GetTour(new Location(tourinput.From), new Location(tourinput.To)).Result;
                tour.Description = res;
            }
            catch
            {
                return null;
            }

            
            

            try
            {
                _repository.Create(tour);
                tour.GenerateSummary();
                _tourlogger.LogInformation("Test");
                return tour;
            }
            catch
            {
                return null;
            }
        }


        public List<Tour> GetAll() =>
            _repository.GetAll().ToList();


        public Tour Get(Guid id)
        {
            try
            {
                return _repository.GetByID(id);
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public bool Delete(Guid id)
        {
            _tourlogger.LogInformation("Hallo :D");
            return _repository.Delete(id);
        }

        public Tour? Update(Tour tour)
        {
            try
            {

                //? Call Api? 
                // Edit Tour what can be change?

                _repository.Update(tour);
                return tour;
            }
            catch
            {
                return null;
            }


        }
    }
}
