using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportantDatesMauiGame.Model
{
    public class ImportandDate : BindableObject
    {
        private string description = "";
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        private string remainingTime = "Czas minął.";
        public string RemainingTime
        {
            get { return remainingTime; }
            set { remainingTime = value; OnPropertyChanged(); }
        }

        private DateTime timeToEvent;
        public DateTime TimeToEvent
        {
            get { return timeToEvent; }
            set { timeToEvent = value; }
        }

    }
}
