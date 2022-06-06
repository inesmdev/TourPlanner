using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using TourPlanner.Api.Services.TourService;
using TourPlanner.Models;
using TourPlanner.UI.Models;

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
            _logger.LogInformation($"Send all tours from db: {tours}");
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
            {
                _logger.LogInformation($"Tour ({id}) not found.");
                return NotFound();
            }

            return Ok(tour);
         }


        /*
         *  Create new tour
         */
        [HttpPost]
        public IActionResult Create(TourInput tourinput)
        {
            Tour tour = _tourservice.Add(tourinput);

            if (tour == null)
            {
                _logger.LogError($"Could not create tour");
                return BadRequest();
            }
            else
            {
                _logger.LogError($"Tour ({tour.Id}) successfully created");

                return CreatedAtAction(nameof(Create), new
                {
                    Id = tour.Id,
                    Name = tour.Name,
                    Description = tour.Description,
                    From = tour.From,
                    To = tour.To,
                    EstimatedTime = tour.EstimatedTime,
                    Distance = tour.Distance,
                    Summary = tour.Summary
                }, tour);
            }      
        }

        // still neccesary?
        [HttpPost("/TourMap")]
        public IActionResult CreateMap(string addresses)
        {
            //addresses in form: "adresse1 + addresse2"
            MemoryStream map = _tourservice.GetMap(addresses);

            if (map == null)
                return BadRequest();
            else
                return base.File(map, "image/jpg");
        }

        /*
         *  Update tour by id
         */
        [HttpPut("{id}")]
        public IActionResult Update(string id, Tour tour)
        {
            Guid idParsed = Guid.Parse(id);

            if (idParsed != tour.Id)
            {
                _logger.LogError($"Could not update tour ({id}). Tourid does not match.");
                return BadRequest();
            }

            var existingTour = _tourservice.Get(idParsed);

            if (existingTour is null)
            {
                _logger.LogError($"Could not update tour ({id}). Tour not found.");
                return NotFound();
            }

            Tour tourdb = _tourservice.Update(tour);

            if (tourdb is null)
            {
                _logger.LogError($"Could not update tour ({id}). Update failed.");
                return BadRequest();
            }
            else
            {
                _logger.LogInformation($"Tour ({id}) successfully updated.");
                return Ok(tourdb);
            }
        }

        
        /*
         *  Delete tour by id
         */
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var tour = _tourservice.Get(Guid.Parse(id));

            if (tour is null)
            {
                _logger.LogError($"Could not delete tour ({id}). Tour not found.");
                return NotFound();
            }

            if (_tourservice.Delete(Guid.Parse(id)))
            {
                _logger.LogInformation($"Tour ({id}) deleted.");
                return NoContent();

            }
            else
            {
                _logger.LogError($"Could not delete tour ({id}). Deletion failed.");
                return BadRequest();
            }
        }            
    }
}
