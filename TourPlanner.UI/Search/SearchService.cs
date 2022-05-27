using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using TourPlanner.UI.Models;

namespace TourPlanner.UI.Search
{
    public class SearchService
    {
        
        // Summary 
        static  public List<TourUI> Search(ObservableCollection<TourUI> list, string keyword)
        {
            List<TourUI> results = new List<TourUI>();
            Regex regex = new Regex(keyword, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (var item in list)
            {
                // Serialisieren -> Json to String
                string itemString = JsonConvert.SerializeObject(item);

                //TourLogs
                if (regex.IsMatch(itemString))
                {
                    results.Add(item);
                }
            }

            return results;
        }
    }
}
