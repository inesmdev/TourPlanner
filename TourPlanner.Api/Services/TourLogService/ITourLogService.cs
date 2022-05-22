using System;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.Api.Services.TourLogService
{
    public interface ITourLogService
    {
        List<TourLog> GetAll(Guid tourid);

        List<TourLog> GetAll();

        TourLog Get(Guid id);

        TourLog Add(TourLog tourlog);

        public TourLog Update(TourLog tour);

        bool Delete(Guid id);
    }
}
