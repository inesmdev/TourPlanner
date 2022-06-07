using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TourPlanner.Api.Services.ImportService;
using TourPlanner.UI.Models;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importservice;
        ILogger<ImportController> _logger;

        /*
         *  Constructor
         */
        public ImportController(IImportService importservice, ILogger<ImportController> logger)
        {
            _importservice = importservice;
            _logger = logger;
        }


        [HttpPut]
        public IActionResult ImportTourData(List<TourUI> tours)
        {
            if (_importservice.ImportTours(tours))
            {
                _logger.LogInformation("Tours imported succesfully");
                return Ok(tours);
            }
            else
            {
                _logger.LogError("Tours import failed.");
                return BadRequest();
            }
        }
    }
}
