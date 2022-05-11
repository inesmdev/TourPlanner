using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Api.Services.ReportService
{
    public interface IReportService
    {
        public void GeneratePdfReport(Tour tour);
        
    }
}
