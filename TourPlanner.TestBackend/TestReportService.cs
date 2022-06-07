using NUnit.Framework;
using TourPlanner.Api.Services.ReportService;
using TourPlanner.Models;
using System.IO;
using System;
using System.Collections.ObjectModel;

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
        public void TestReport_withLog()
        {
            Tour tour = new Tour() { Id = Guid.NewGuid() ,Name = "Report Test", From = "Someplace 6", To = "Anotherplace 5", Description = "This is a very authentic description", EstimatedTime = 9, Distance = 10 };
            TourLog tourLog = new TourLog()
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                Comment = "Very creative comment",
                TotalTime = 420,
                TourDifficulty = EnumTourDifficulty.expert,
                TourRating = EnumTourRating.five_star
            };
            ObservableCollection<TourLog> logs = new ObservableCollection<TourLog>();
            logs.Add(tourLog);
            string basepath = Directory.GetCurrentDirectory();
            string path = basepath + "\\Pdfs\\" + tour.Id.ToString() + ".pdf";

            tour.GenerateSummary();
            _reportService.GeneratePdfReport(tour, logs);

            Assert.IsTrue(File.Exists(path));
        }

        [Test]
        public void TestReport_withoutLog()
        {
            Tour tour = new Tour() { 
                Id = Guid.NewGuid(), 
                Name = "Report Test", 
                From = "Someplace 6", 
                To = "Anotherplace 5", 
                Description = "This is a very authentic description", 
                EstimatedTime = 9,
                Distance = 10 
            };
            ObservableCollection<TourLog> logs = new ObservableCollection<TourLog>();
            string basepath = Directory.GetCurrentDirectory();
            string path = basepath + "\\Pdfs\\" + tour.Id.ToString() + ".pdf";

            tour.GenerateSummary();
            _reportService.GeneratePdfReport(tour, logs);

            Assert.IsTrue(File.Exists(path));
        }
    }
}