using System;
using TourPlanner.Models;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace TourPlanner.Api.Services.ReportService
{
    public class ReportService : IReportService
    {
        const string TARGET_PDF = "../../../target.pdf";

        public void GeneratePdfReport(Tour tour)
        {
            PdfWriter writer = new PdfWriter(TARGET_PDF);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph ReportHeader = new Paragraph("Report:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLACK);
            document.Add(ReportHeader);
            document.Add(new Paragraph("Tour Summary:"));
            document.Add(new Paragraph(tour.Summary));
            document.Close();

            //throw new NotImplementedException();
        }
    }
}
