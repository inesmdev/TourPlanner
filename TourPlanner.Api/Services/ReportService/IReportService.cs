using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Api.Services.ReportService
{
    public interface IReportService
    {
        public void GeneratePdfReport(Tour tour, ObservableCollection<TourLog> tourLogs);
        
    }
}
