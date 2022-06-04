using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Api.Services.MapQuestService
{
    // Directions API (MapQuest)
    public interface IMapQuestService
    {
        //Tour GetTour(string start, string end);
        public Task<MapQuestTour> GetTour(Location from, Location to);
        public Task<MemoryStream> GetMap(string from, string to);
    }
}
