using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TourPlanner.Api.Services.ReportService;
using TourPlanner.Models;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportservice;
        ILogger _logger;

        public ReportController(IReportService reportservice, ILogger<ReportController> logger)
        {
            _reportservice = reportservice;
            _logger = logger;
        }


        [HttpPost]
        public IActionResult Create(Tour tour)
        {
            // Error Handling?
            _reportservice.GeneratePdfReport(tour);

            return Ok();
        }
    }
}
