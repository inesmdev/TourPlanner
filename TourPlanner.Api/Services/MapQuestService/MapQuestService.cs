using System;
//using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using TourPlanner.Models;
using Newtonsoft.Json;

namespace TourPlanner.Api.Services.MapQuestService
{
    public class MapQuestService : IMapQuestService
    {
        public async Task<string> GetTour(Location from, Location to)
        {
            MapQuestTour tour = new MapQuestTour();
            var url = "http://www.mapquestapi.com/directions/v2/route?key=qJ4MqmQIdQbucdNJBPQGrn5g98Xsx6Qo&" + "from=" + from.Street + "," + from.City + "," + from.Country + "," + from.PostalCode + "&to=" + to.Street + "," + to.City + "," + to.Country + "," + to.PostalCode + "&time";
            using var client = new HttpClient();
            var response = await client.PostAsync(url, null);
            string result = response.Content.ReadAsStringAsync().Result;
            var deserialize = JsonConvert.DeserializeObject<dynamic>(result);
            int responseStatus = deserialize.info.statuscode;
            if(responseStatus == 0)
            {
                int time = deserialize.route.time;
                double distance = deserialize.route.distance;
                tour.Distance = distance;
                tour.EstimatedTime = time;
                Console.WriteLine(tour.EstimatedTime);
                Console.WriteLine(tour.Distance);
            }
            /*Tour tour = new Tour()
            {
                EstimatedTime = deserialize.formattedTime,
                Distance = deserialize.distance,
            };*/
            return result;
        }
    }
}
//var url = "http://www.mapquestapi.com/directions/v2/route?key=qJ4MqmQIdQbucdNJBPQGrn5g98Xsx6Qo&" + "from=" + start + "&to=" + end;