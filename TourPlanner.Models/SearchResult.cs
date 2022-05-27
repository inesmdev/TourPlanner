using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class SearchResult
    {
        public string Title { get; set; }
        public ISearchData Data { get; set; }

    }
}
