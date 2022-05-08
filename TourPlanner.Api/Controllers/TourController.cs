using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TourPlanner.Api.Services;
using TourPlanner.DAL.Repositories;
using TourPlanner.Models;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourservice;

        public TourController(ITourService tourservice)
        {
            _tourservice = tourservice;
        }


        // GET all action
        [HttpGet]
        public ActionResult<List<Tour>> GetAll() =>
            _tourservice.GetAll();


           // GET by Id action
         [HttpGet("{id}")]
         public ActionResult<Tour> Get(string id)
         {
            var tour = _tourservice.Get(Guid.Parse(id));

            if (tour == null)
                return NotFound();

            return tour;
         }


        // POST 
        [HttpPost]
        public IActionResult Create(TourInput tourinput)
        {      
            Tour tour = _tourservice.Add(tourinput);

            if(tour == null)
                return BadRequest();
            else
                return CreatedAtAction(nameof(Create), new { Id = tour.Id, Name = tour.Name, Description = tour.Description }, tour);;
        }


        // PUT action -> Aktualisieren
        /*
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Tour tour)
        {
            if (id != tour.TourId)
                return BadRequest();

            var existingPizza = PizzaService.Get(id);
            if (existingPizza is null)
                return NotFound();

            TourService.Update(tour);

            return NoContent();
        }*/

        

        // DELETE action
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var tour = _tourservice.Get(Guid.Parse(id));

            if (tour is null)
                return NotFound();

            if (_tourservice.Delete(Guid.Parse(id)))
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
