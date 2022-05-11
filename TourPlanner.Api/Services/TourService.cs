using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;
using TourPlanner.Api.Services.MapQuestService;
using Microsoft.Extensions.Logging;

namespace TourPlanner.Api.Services
{
    public class TourService : ITourService
    {
        ITourRepository _repository; // _  -> being created externaly
        IMapQuestService _mapQuestService;
        ILogger<TourService> _tourlogger;
        
        /*
         * Constructor
         */
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
                MapQuestTour res = _mapQuestService.GetTour(new Location(tourinput.From), new Location(tourinput.To)).Result;
                tour.EstimatedTime = res.EstimatedTime;
                tour.Distance = res.Distance;
            }
            catch
            {
                return null;
            }

           
            try
            {
                _repository.Create(tour);
                tour.GenerateSummary();
                _tourlogger.LogInformation($"Tour successfully created, {tour.Summary}");
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
                Tour tour = _repository.GetByID(id);
                tour.GenerateSummary();
                return tour;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public bool Delete(Guid id)
        {
            return _repository.Delete(id);
        }

        // To Do
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
