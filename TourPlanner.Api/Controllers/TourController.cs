using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TourPlanner.Api.Services.TourService;
using TourPlanner.Models;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourservice;
        ILogger _logger;

        /*
         *  Constructor
         */
        public TourController(ITourService tourservice, ILogger<TourController> logger)
        {
            _tourservice = tourservice;
            _logger = logger;
        }


        /*
         *  Get all tours from db
         */
        [HttpGet]
        public ActionResult<List<Tour>> GetAll()
        {
            var tours = _tourservice.GetAll();
            return Ok(tours);
        }


        /*
         *  Get specific tours by id
         */
        [HttpGet("{id}")]
         public ActionResult<Tour> Get(string id)
         {
            var tour = _tourservice.Get(Guid.Parse(id));

            if (tour == null)
                return NotFound();

            return Ok(tour);
         }


        /*
         *  Create new tour
         */
        [HttpPost]
        public IActionResult Create(TourInput tourinput)
        {      
            Tour tour = _tourservice.Add(tourinput);

            if(tour == null)
                return BadRequest();
            else
                return CreatedAtAction(nameof(Create), new {
                    Id = tour.Id, 
                    Name = tour.Name, 
                    Description = tour.Description, 
                    From=tour.From, 
                    To = tour.To, 
                    EstimatedTime = tour.EstimatedTime, 
                    Distance = tour.Distance, 
                    Summary = tour.Summary}, tour);
        }


        /*
         *  Update tour by id
         */
        [HttpPut("{id}")]
        public IActionResult Update(string id, Tour tour)
        {
            Guid idParsed = Guid.Parse(id);

            if (idParsed != tour.Id)
                return BadRequest();

            var existingTour = _tourservice.Get(idParsed);

            if (existingTour is null)
                return NotFound();

            Tour tourdb = _tourservice.Update(tour);

            if (tourdb is null)
                return BadRequest();
            else
                return Ok(tourdb); // 204?
        }

        
        /*
         *  Delete tour by id
         */
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var tour = _tourservice.Get(Guid.Parse(id));

            if (tour is null)
                return NotFound();

            if (_tourservice.Delete(Guid.Parse(id)))
                return NoContent();
            else
                return BadRequest();
        }


        /* 
        [HttpPost]
        public IActionResult ImportTourData(List<TourUI> tours)
        {
            Tour tour = _tourservice.Add(tourinput);

            if (tour == null)
                return BadRequest();
            else
                return CreatedAtAction(nameof(Create), new { Id = tour.Id, Name = tour.Name, Description = tour.Description, From = tour.From, To = tour.To, EstimatedTime = tour.EstimatedTime, Distance = tour.Distance, Summary = tour.Summary }, tour);
        }
        */
    }
}
