using NUnit.Framework;
using TourPlanner.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using TourPlanner.UI.Models;
using TourPlanner.UI.Search;

namespace TourPlanner.Test
{
    public class TestSearch
    {
        private ObservableCollection<TourUI> TourList;
        private List<Tour> tours;

        [SetUp]
        public void Setup()
        {
            TourList = new();
            tours = new();

            tours.Add(new Tour()
            {
                Id = System.Guid.NewGuid(),
                Name = "TestTour1"
            });

            tours.Add(new Tour()
            {
                Id = System.Guid.NewGuid(),
                Name = "TestTour2"
            }); 
                  
            TourList.Add(new TourUI()
            {
                TourData = tours[0],
                Tourlogs = new ObservableCollection<TourLog>()
            });
        }

        [Test]
        public void TestSearch_Notfound()
        {
            string keyword = "asdf";

            SearchService.Search(TourList, keyword);

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