using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using TourPlanner.UI.Models;

namespace TourPlanner.UI.Search
{
    /*
     *  InMemory search through all tours & corresponding tourlogs 
     */
    public static class SearchService
    {     
        static public List<TourUI> Search(ObservableCollection<TourUI> list, string keyword)
        {
            List<TourUI> results = new List<TourUI>();
            Regex regex = new Regex(keyword, RegexOptions.Compiled | RegexOptions.IgnoreCase);

          
            foreach (var item in list)
            {
                // Serialize to string
                string itemString = JsonConvert.SerializeObject(item);

                // If there is a match in the string -> add to results list
                if (regex.IsMatch(itemString))
                    results.Add(item);
            }

            return results;
        }
    }
}
