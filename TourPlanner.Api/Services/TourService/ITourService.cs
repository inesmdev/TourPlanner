using System;
using System.Collections.Generic;
using System.IO;
using TourPlanner.Models;

namespace TourPlanner.Api.Services.TourService
{
    public interface ITourService
    {
        List<Tour> GetAll();

        Tour Get(Guid id);

        Tour Add(TourInput tourinput);

        MemoryStream GetMap(string coordinates);

        public Tour Update(Tour tour);

        bool Delete(Guid id);
    }
}
