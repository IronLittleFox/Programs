using ImportantDatesMauiGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsMaui.Interfaces;
using UtilsMaui.Utils;

namespace ImportantDatesMauiGame.ViewModel
{
    public class ImportantDatesViewModel : BindableObject, IGameViewModel
    {
        public ObservableCollection<ImportandDate> ImportandDates { get; set; }

        private Task? countTimeTask;
        private bool stopTasks = false;
        public ImportantDatesViewModel()
        {
            ImportandDates =
            [
                new ImportandDate()
                {
                    Description = "Dzień Edukacji Narodowej",
                    TimeToEvent = new DateTime(2024, 10, 14)
                },
                new ImportandDate()
                {
                    Description = "Wszystkich Świętych 01.11.2024",
                    TimeToEvent = new DateTime(2024, 11, 1)
                },
                new ImportandDate()
                {
                    Description = "Święto Niepodległości",
                    TimeToEvent = new DateTime(2024, 11, 11)
                },
                new ImportandDate()
                {
                    Description = "Termin wystawienia ocen dla klas maturalnych semestr 1",
                    TimeToEvent = new DateTime(2024, 12, 14)
                },
                new ImportandDate()
                {
                    Description = "Ferie bożonarodzeniowe",
                    TimeToEvent = new DateTime(2024, 12, 21)
                },
                new ImportandDate()
                {
                    Description = "Święto Trzech Króli",
                    TimeToEvent = new DateTime(2025, 1, 6)
                },
                new ImportandDate()
                {
                    Description = "Ferie",
                    TimeToEvent = new DateTime(2025, 01, 18)
                },
                new ImportandDate()
                {
                    Description = "Termin wystawienia ocen dla klas 1-4 semestr 1",
                    TimeToEvent = new DateTime(2025, 2, 8)
                },
                new ImportandDate()
                {
                    Description = "Wiosenna przerwa świąteczna (Wielkanoc)",
                    TimeToEvent = new DateTime(2025, 4, 17)
                },
                new ImportandDate()
                {
                    Description = "Długi majowy weekend",
                    TimeToEvent = new DateTime(2025, 5, 1)
                },
                new ImportandDate()
                {
                    Description = "Rozpoczęcie matur",
                    TimeToEvent = new DateTime(2025, 5, 5)
                },
                new ImportandDate()
                {
                    Description = "Rozpoczęcie wakacji",
                    TimeToEvent = new DateTime(2025, 6, 28)
                },
            ];
            countTimeTask = Task.Run(CountTime);
        }

        private void CountTime()
        {
            while (!stopTasks)
            {
                foreach (ImportandDate item in ImportandDates)
                {
                    TimeSpan t = item.TimeToEvent - DateTime.Now;
                    if (t >= TimeSpan.Zero)
                        item.RemainingTime = string.Format("{0:D2}d : {1:D2}h : {2:D2}m : {3:D2}s",
                                                                t.Days,
                                                                t.Hours,
                                                                t.Minutes,
                                                                t.Seconds);
                    else
                    {
                        item.RemainingTime = "Czas minął.";
                    }
                }
                Thread.Sleep(500);
            }
        }

        public void Dispose()
        {
            stopTasks = true;
            if (countTimeTask != null)
                countTimeTask.Wait();
        }
    }
}
