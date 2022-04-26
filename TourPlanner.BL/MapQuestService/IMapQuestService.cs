using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    // Directions API (MapQuest)
    public interface IMapQuestService
    {
        //Tour GetTour(string start, string end);
        Task<string> GetTour(Location from, Location to);
    }
}
