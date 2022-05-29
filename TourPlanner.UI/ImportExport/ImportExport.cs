using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.UI.Models;

namespace TourPlanner.UI.ImportExport
{
    public class ImportExport
    {
        // Export Observable COllection to File
        public static bool Export(ObservableCollection<TourUI> tours, string path = "./tourdata.txt")
        {
            // Path has correct Format?
            try
            {
                string str = JsonConvert.SerializeObject(tours);
                System.IO.File.WriteAllText(path, str);
            }
            catch
            {
                return false;
            }
           

            return true;
        }

        public List<TourUI> Import()
        {
            return new List<TourUI>();
        }
    }
}
