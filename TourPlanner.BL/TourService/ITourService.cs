using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BL.TourService
{
    public interface ITourService
    {
        List<Tour> GetAllTours();

        Tour AddTour(TourInputData tour);

    }
}
