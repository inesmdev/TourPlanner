using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourPlanner.UI.Models;

namespace TourPlanner.Api.Services.ImportService
{
    public interface IImportService
    {
        public bool ImportTours(List<TourUI> tours);
    }
}
