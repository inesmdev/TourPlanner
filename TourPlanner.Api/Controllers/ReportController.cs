using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
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
        public async Task<IActionResult> CreateAsync(Tour tour)
        {       
            _reportservice.GeneratePdfReport(tour); //return false if sth fails
            
            try
            {
                var filePath = $"./Pdfs/{tour.Id}.pdf"; 

                // Get content type
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }


                var bytes = await System.IO.File.ReadAllBytesAsync(filePath);

                return File(bytes, contentType, Path.GetFileName(filePath));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
