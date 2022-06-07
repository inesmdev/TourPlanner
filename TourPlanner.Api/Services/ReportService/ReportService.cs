using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Extensions.Logging;
using TourPlanner.Models;
using TourPlanner.DAL.Repositories;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.IO;
using iText.IO.Image;

namespace TourPlanner.Api.Services.ReportService
{
    public class ReportService : IReportService
    {
        // const string TARGET_PDF = "../../../target.pdf";
        const string TARGET_PDF = "./Pdfs/";

        ILogger<ReportService> _logger;
        ITourLogRepository _logRepository;

        public ReportService(ITourLogRepository tourlogrepository, ILogger<ReportService> logger)
        {
            _logger = logger;
            _logRepository = tourlogrepository;
        }

        public ReportService() { }

        public void GeneratePdfReport(Tour tour, ObservableCollection<TourLog> tourLogs)
        {
            var path = Directory.GetCurrentDirectory() + "\\StaticFiles\\" + tour.Id.ToString() + ".jpg";
            PdfWriter writer = new PdfWriter(TARGET_PDF + $"{ tour.Id }.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph ReportHeader = new Paragraph("Report:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLACK);
            document.Add(ReportHeader);
            if (File.Exists(path))
            {
                ImageData mapData = ImageDataFactory.Create(path);
                Image map = new Image(mapData);
                document.Add(map);
            }
            document.Add(new Paragraph("Tour Summary:"));
            document.Add(new Paragraph(tour.Summary));
            document.Add(new Paragraph("Tour logs: "));
            if(tourLogs.Count != 0)
            {
                foreach (TourLog log in tourLogs)
                {
                    document.Add(new Paragraph("ID: " + log.Id.ToString() + ", Date and time: " + log.DateTime.ToString("D", CultureInfo.GetCultureInfo("de-DE")) + ", Difficulty: " + log.TourDifficulty.ToString() + ", Rating: " + log.TourRating.ToString() + ", Total Time: " + log.TotalTime.ToString() + ", Comment: " + log.Comment));
                }
            }
            else
            {
                document.Add(new Paragraph("No Logs have been added yet"));
            }
            document.Close();
            if(_logger != null)
            {
                _logger.LogInformation($"Report");
            }
        }
    }
}
