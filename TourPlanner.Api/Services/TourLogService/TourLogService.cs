using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;

namespace TourPlanner.Api.Services.TourLogService
{
    public class TourLogService : ITourLogService
    {
        ITourLogRepository _repository;
        ILogger<TourLogService> _logger;

        /*
         *  Constructor
         */
        public TourLogService(ITourLogRepository repository, ILogger<TourLogService> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        /*
         *  Add new tourlog
         */
        public TourLog Add(TourLogInput tourlogInput)
        {
            TourLog tourlog = new TourLog()
            {
                Id = Guid.NewGuid(),
                TourId = tourlogInput.TourId,
                DateTime = tourlogInput.DateTime,
                TourRating = tourlogInput.TourRating,
                TourDifficulty = tourlogInput.TourDifficulty,
                TotalTime = tourlogInput.TotalTime,
                Comment = tourlogInput.Comment
            };

            try
            {
                _repository.Create(tourlog);
                _logger.LogInformation($"Tourlog ({tourlog.Id}) successfully created");
                return tourlog;
            }
            catch
            {
                _logger.LogError($"Could not create Tourlog ({tourlog.Id})");
                return null;
            }
        }

        /*
         *  Delete tourlog
         */
        public bool Delete(Guid id)
        {
            return _repository.Delete(id);
        }

        /*
         * Get tourlog by id
         */
        public TourLog Get(Guid id)
        {
            try
            {
                TourLog tourlog = _repository.GetByID(id);
                return tourlog;
            }
            catch
            {
                _logger.LogError($"Could not delete tour ({id})");
                return null;
            }
        }

        /*
         *  Get all tourlogs
         */
        public List<TourLog> GetAll() {
            return _repository.GetAll().ToList();
        }

        /*
         * Get all tourlogs for specific tour
         */
        public List<TourLog> GetAll(Guid tourid)
        {
            return _repository.GetAll(tourid).ToList();
        }

        /*
         * Update tourlog
         */
        public TourLog Update(TourLog tourlog)
        {
            try
            {
                _repository.Update(tourlog);
                _logger.LogInformation($"Tourlog ({tourlog.Id}) sucessfully updated.");
                return tourlog;
            }
            catch
            {
                _logger.LogError($"Could not update tourlog ({tourlog.Id})");
                return null;
            }
        }
    }
}
