using NUnit.Framework;
using TourPlanner.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using TourPlanner.UI.Models;
using TourPlanner.UI.Search;

namespace TourPlanner.TestUI
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
                Name = "CoolTour"
            }); 
                  
            TourList.Add(new TourUI()
            {
                TourData = tours[0],
                Tourlogs = new ObservableCollection<TourLog>()
            });

            TourList.Add(new TourUI()
            {
                TourData = tours[1],
                Tourlogs = new ObservableCollection<TourLog>()
            });
        }

        [Test]
        public void TestSearch_Notfound()
        {
            string keyword = "asdf";

            var res = SearchService.Search(TourList, keyword);

            Assert.AreEqual(new List<TourUI>(), res);
        }


        [Test]
        public void TestSearch_Match()
        {
            string keyword = "ool";

            var res = SearchService.Search(TourList, keyword);

            Assert.AreEqual(TourList[1], res[0]);
        } 
    }
}