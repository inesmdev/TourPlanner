using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TourPlanner.Api.Services.TourLogService;
using TourPlanner.Models;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourLogController : ControllerBase
    {
        private readonly ITourLogService _tourlogservice;
        ILogger _logger;


        public TourLogController(ITourLogService tourlogservice, ILogger<TourLogController> logger)
        {
            _tourlogservice = tourlogservice;
            _logger = logger;
        }


        [HttpGet("/all/{id}")]
        public ActionResult<List<TourLog>> GetAll(string id) =>
            Ok(_tourlogservice.GetAll(Guid.Parse(id)));
        

        [HttpGet]
        public ActionResult<List<TourLog>> GetAll() =>
            Ok(_tourlogservice.GetAll());
   
         
        [HttpGet("{id}")]
         public ActionResult<TourLog> Get(string id)
         {
            TourLog tourlog = _tourlogservice.Get(Guid.Parse(id));

            if (tourlog == null)
                return NotFound();

            return Ok(tourlog);
         }
               

        [HttpPost]
        public IActionResult Create(TourLogUserInput tourlogInput)
        {
            TourLog tourlog = _tourlogservice.Add(tourlogInput);

            if (tourlog == null)
                return BadRequest();

            else
                return CreatedAtAction(nameof(Create), new {
                    Id = tourlog.Id,
                    TourId = tourlog.TourId,
                    TourRating = tourlog.TourRating,
                    TourDifficulty = tourlog.TourDifficulty,
                    TotalTime = tourlog.TotalTime,
                    Comment = tourlog.Comment
                }, tourlog);
        }
        

        [HttpPut("{id}")]
        public IActionResult Update(string id, TourLog tourlog)
        {
            Guid idParsed = Guid.Parse(id);

            if (idParsed != tourlog.Id)
                return BadRequest();

            var existingTour = _tourlogservice.Get(idParsed);
            if (existingTour is null)
                return NotFound();

            TourLog tourlogdb = _tourlogservice.Update(tourlog);

            if (tourlogdb is null)
                return BadRequest();
            else
                return Ok(tourlogdb); // 204?
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            TourLog tourlog = _tourlogservice.Get(Guid.Parse(id));

            if (tourlog is null)
                return NotFound();

            if (_tourlogservice.Delete(Guid.Parse(id)))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
