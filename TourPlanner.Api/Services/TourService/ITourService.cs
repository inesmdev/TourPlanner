using System;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.Api.Services
{
    public interface ITourService
    {
        List<Tour> GetAll();

        Tour Get(Guid id);

        Tour Add(TourInput tourinput);

        public Tour Update(Tour tour);

        bool Delete(Guid id);
    }
}
