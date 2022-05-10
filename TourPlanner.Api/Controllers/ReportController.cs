using Microsoft.AspNetCore.Mvc;
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

        public ReportController(IReportService reportservice)
        {
            _reportservice = reportservice;
        }


        // POST ? 
        [HttpPost]
        public IActionResult Create(Tour tour)
        {
            //??
            // Call the Report Service 


                return Ok();
        }



        

        
    }
}
