using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TourPlanner.Api.Services.MapQuestService;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;

namespace TourPlanner.Api.Services.TourService
{
    public class TourService : ITourService
    {
        ITourRepository _repository;
        IMapQuestService _mapapi;
        ILogger<TourService> _logger;

        /*
         * Constructor
         */
        public TourService(ITourRepository repository, IMapQuestService mapapi, ILogger<TourService> logger)
        {
            _repository = repository;
            _mapapi = mapapi;
            _logger = logger;
        }


        /*
         *  Create new tour
         */
        public Tour Add(TourInput tourinput)
        {
            Tour tour = new Tour()
            {
                Id = Guid.NewGuid(),
                Name = tourinput.Name,
                Description = tourinput.Description,
                From = tourinput.From,
                To = tourinput.To,
                TransportType = tourinput.TransportType
            };

            // Call the MapQuest Api to get the missing information about the tour
            try
            {
                MapQuestTour res = _mapapi.GetTour(new Location(tourinput.From), new Location(tourinput.To), tour.Id.ToString()).Result;
                tour.EstimatedTime = res.EstimatedTime;
                tour.Distance = res.Distance;

                _repository.Create(tour);
                tour.GenerateSummary();

                return tour;
            }
            catch
            {
                _logger.LogError($"Could not create tour ({tour.Id}, \"{ tour.Name}\")");
                return null;
            }        
        }

        public MemoryStream GetMap(string coordinates)
        {
            MemoryStream map = null;
            string from = coordinates.Split("+")[0];
            string to = coordinates.Split("+")[1];
            //map = _mapapi.GetMap(from,to).Result;
            return map;
        }


        /*
         *  Get all tours
         */
        public List<Tour> GetAll() =>
            _repository.GetAll().ToList();



        /*
         *  Get a tour by id
         */
        public Tour Get(Guid id)
        {
            try
            {
                Tour tour = _repository.GetByID(id);
                tour.GenerateSummary();
                return tour;
            }
            catch
            {
                _logger.LogError($"Could not get tour ({id})");
                return null;
            }
        }


        /*
         * Delete tour by id
         */
        public bool Delete(Guid id)
        {
            return _repository.Delete(id);
        }


        /*
         *  Edit tour
         */
        public Tour Update(Tour tour)
        {
            try
            {
                _repository.Update(tour);
                tour.GenerateSummary();
                return tour;
            }
            catch
            {
                return null;
            }
        }
    }
}
