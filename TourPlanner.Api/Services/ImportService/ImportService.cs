using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;
using TourPlanner.UI.Models;

namespace TourPlanner.Api.Services.ImportService
{
    public class ImportService : IImportService
    {
        ILogger<ImportService> _logger;
        ITourRepository _tourrepository;
        ITourLogRepository _tourlogrepository;


        public ImportService(ITourRepository tourrepository, ITourLogRepository tourlogrepository, ILogger<ImportService> logger)
        {
            _logger = logger;
            _tourrepository = tourrepository;
            _tourlogrepository = tourlogrepository;
        }


        public bool ImportTours(List<TourUI> tours)
        {
            // Remove all tours + tourlogs
            if (!_tourrepository.DeleteAll())
                return false;

            try
            {
                foreach (var tour in tours)
                {
                    // create tour
                    _tourrepository.Create(tour.TourData);

                    foreach (TourLog tourlog in tour.Tourlogs)
                    {
                        // create tourlog
                        _tourlogrepository.Create(tourlog);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
