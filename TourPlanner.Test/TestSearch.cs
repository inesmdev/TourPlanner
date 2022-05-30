using NUnit.Framework;
using TourPlanner.Models;
using TourPlanner.UI.Models;
using System.Collections.ObjectModel;

namespace TourPlanner.Test
{
    public class TestSearch
    {
        private ObservableCollection<TourUI> tours;

        [SetUp]
        public void Setup()
        {
            tours = new();

            tours.Add(new TourUI()
            {
                TourData = new Tour()
                {
                    Id = System.Guid.NewGuid(),
                    Name = "TestTour1"
                },
                Tourlogs = new ObservableCollection<TourLog>()
            });
        }

        [Test]
        public void TestSearch_Notfound()
        {
       

            Assert.Pass();
        }

        [Test]
        public void TestSearch_Match_in_TourLog()
        {


            Assert.Pass();
        }

        [Test]
        public void TestSearch_Match_in_Tour()
        {


            Assert.Pass();
        }

        [Test]
        public void TestSearch_EmptyTourList()
        {


            Assert.Pass();
        }
    }
}