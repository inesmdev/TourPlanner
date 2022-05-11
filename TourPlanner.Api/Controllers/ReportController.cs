using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TourPlanner.Api.Services;
using TourPlanner.Api.Services.ReportService;
using TourPlanner.DAL.Repositories;
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


        // POST ? 
        [HttpPost]
        public IActionResult Create(Tour tour)
        {
            // Error Handling?
            _reportservice.GeneratePdfReport(tour);

             return Ok();
        }



        

        
    }
}
