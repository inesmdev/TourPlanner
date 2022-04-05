using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.Dialogs.DialogCreateTour
{
    class TourInputData : IInputData
    {
        public string Tourname { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportType { get; set; } // Select -> Enum?

        public bool ValidateInput()
        {
           //??

            return true;
        }
    }
}
