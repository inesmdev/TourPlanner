using NUnit.Framework;
using TourPlanner.Api.Services.ReportService;
using TourPlanner.Models;
using TourPlanner.Test.Mocks;

namespace TourPlanner.Test
{
    public class TestReportService
    {
        private IReportService _reportService;

        [SetUp]
        public void Setup()
        {
            _reportService = new ReportService();
        }

        [Test]
        public void TestReport()
        {
            Tour tour = new Tour() { Name = "Report Test", From = "Someplace 6", To = "Anotherplace 5", Description = "This is a very authentic description", EstimatedTime = 9, Distance = 10};
            tour.GenerateSummary();
            _reportService.GeneratePdfReport(tour);
            Assert.Pass();
        }
    }
}