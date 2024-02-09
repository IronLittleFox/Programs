using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsMaui.Interfaces;
using UtilsMaui.Utils;

namespace ImportantDatesMauiGame.ViewModel
{
    public class ImportantDatesViewModel : BindableObject, IGameViewModel
    {
        private string timeToEaster = "";
        public string TimeToEaster
        {
            get { return timeToEaster; }
            set
            {
                timeToEaster = value;
                OnPropertyChanged();
            }
        }

        private string timeToIssuingGradesRorFifthGrades = "";
        public string TimeToIssuingGradesRorFifthGrades
        {
            get { return timeToIssuingGradesRorFifthGrades; }
            set
            {
                timeToIssuingGradesRorFifthGrades = value;
                OnPropertyChanged();
            }
        }

        private string timeUntilTheEndOfTheSchoolYearForFifthGraders = "";
        public string TimeUntilTheEndOfTheSchoolYearForFifthGraders
        {
            get { return timeUntilTheEndOfTheSchoolYearForFifthGraders; }
            set
            {
                timeUntilTheEndOfTheSchoolYearForFifthGraders = value;
                OnPropertyChanged();
            }
        }

        private string timeUntilTheLongMayWeekend = "";
        public string TimeUntilTheLongMayWeekend
        {
            get { return timeUntilTheLongMayWeekend; }
            set
            {
                timeUntilTheLongMayWeekend = value;
                OnPropertyChanged();
            }
        }

        private string timeUntilTheExamsStart = "";
        public string TimeUntilTheExamsStart
        {
            get { return timeUntilTheExamsStart; }
            set
            {
                timeUntilTheExamsStart = value;
                OnPropertyChanged();
            }
        }

        private string timeToIssueGradesForTheRemainingClasses = "";
        public string TimeToIssueGradesForTheRemainingClasses
        {
            get { return timeToIssueGradesForTheRemainingClasses; }
            set
            {
                timeToIssueGradesForTheRemainingClasses = value;
                OnPropertyChanged();
            }
        }

        private string timeUntilTheEndOfTheSchoolYearForTheRemainingClasses = "";
        public string TimeUntilTheEndOfTheSchoolYearForTheRemainingClasses
        {
            get { return timeUntilTheEndOfTheSchoolYearForTheRemainingClasses; }
            set
            {
                timeUntilTheEndOfTheSchoolYearForTheRemainingClasses = value;
                OnPropertyChanged();
            }
        }

        private string timeUntilTheEndOfTheHoliday = "";
        public string TimeUntilTheEndOfTheHoliday
        {
            get { return timeUntilTheEndOfTheHoliday; }
            set
            {
                timeUntilTheEndOfTheHoliday = value;
                OnPropertyChanged();
            }
        }

        private Task? timeToEasterTask;
        private Task? timeToIssuingGradesRorFifthGradesTask;
        private Task? timeUntilTheEndOfTheSchoolYearForFifthGradersTask;
        private Task? timeUntilTheLongMayWeekendTask;
        private Task? timeUntilTheExamsStartTask;
        private Task? timeToIssueGradesForTheRemainingClassesTask;
        private Task? timeUntilTheEndOfTheSchoolYearForTheRemainingClassesTask;
        private Task? timeUntilTheEndOfTheHolidayTask;
        private bool stopTasks = false;
        public ImportantDatesViewModel()
        {
            timeToEasterTask = Task.Run(CountTimeToEaster);
            timeToIssuingGradesRorFifthGradesTask = Task.Run(CountTimeToIssuingGradesRorFifthGrades);
            timeUntilTheEndOfTheSchoolYearForFifthGradersTask = Task.Run(CountTimeUntilTheEndOfTheSchoolYearForFifthGraders);
            timeUntilTheLongMayWeekendTask = Task.Run(CountTimeUntilTheLongMayWeekend);
            timeUntilTheExamsStartTask = Task.Run(CountTimeUntilTheExamsStart);
            timeToIssueGradesForTheRemainingClassesTask = Task.Run(CountTimeToIssueGradesForTheRemainingClasses);
            timeUntilTheEndOfTheSchoolYearForTheRemainingClassesTask = Task.Run(CountTimeUntilTheEndOfTheSchoolYearForTheRemainingClasses);
            timeUntilTheEndOfTheHolidayTask = Task.Run(CountTimeUntilTheEndOfTheHoliday);
        }

        private void CountTimeUntilTheEndOfTheHoliday()
        {
            DateTime time = new DateTime(2024, 9, 2);
            while (!stopTasks)
            {
                TimeSpan t = time - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeUntilTheEndOfTheHoliday = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeUntilTheEndOfTheHoliday = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        private void CountTimeUntilTheEndOfTheSchoolYearForTheRemainingClasses()
        {
            DateTime time = new DateTime(2024, 6, 21, 10, 0, 0);
            while (!stopTasks)
            {
                TimeSpan t = time - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeUntilTheEndOfTheSchoolYearForTheRemainingClasses = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeUntilTheEndOfTheSchoolYearForTheRemainingClasses = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        private void CountTimeToIssueGradesForTheRemainingClasses()
        {
            DateTime time = new DateTime(2024, 6, 15);
            while (!stopTasks)
            {
                TimeSpan t = time - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeToIssueGradesForTheRemainingClasses = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeToIssueGradesForTheRemainingClasses = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        private void CountTimeUntilTheExamsStart()
        {
            DateTime time = new DateTime(2024, 5, 5);
            while (!stopTasks)
            {
                TimeSpan t = time - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeUntilTheExamsStart = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeUntilTheExamsStart = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        private void CountTimeUntilTheLongMayWeekend()
        {
            DateTime time = new DateTime(2024, 4, 30);
            while (!stopTasks)
            {
                TimeSpan t = time - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeUntilTheLongMayWeekend = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeUntilTheLongMayWeekend = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        private void CountTimeUntilTheEndOfTheSchoolYearForFifthGraders()
        {
            DateTime time = new DateTime(2024, 4, 26, 10, 0, 0);
            while (!stopTasks)
            {
                TimeSpan t = time - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeUntilTheEndOfTheSchoolYearForFifthGraders = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeUntilTheEndOfTheSchoolYearForFifthGraders = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        private void CountTimeToIssuingGradesRorFifthGrades()
        {
            DateTime time = new DateTime(2024, 4, 20);
            while (!stopTasks)
            {
                TimeSpan t = time - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeToIssuingGradesRorFifthGrades = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeToIssuingGradesRorFifthGrades = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        private void CountTimeToEaster()
        {
            DateTime easterTime = new DateTime(2024, 3, 28);
            while (!stopTasks)
            {
                TimeSpan t = easterTime - DateTime.Now;
                if (t >= TimeSpan.Zero)
                    TimeToEaster = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                                                            t.Days,
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                else
                {
                    TimeToEaster = "Czas minął.";
                    break;
                }
                Thread.Sleep(1);
            }
        }

        public void Dispose()
        {
            stopTasks = true;
            if (timeToEasterTask != null)
                timeToEasterTask.Wait();
            if (timeToIssuingGradesRorFifthGradesTask != null)
                timeToIssuingGradesRorFifthGradesTask.Wait();
            if (timeUntilTheEndOfTheSchoolYearForFifthGradersTask != null)
                timeUntilTheEndOfTheSchoolYearForFifthGradersTask.Wait();
            if (timeUntilTheLongMayWeekendTask != null)
                timeUntilTheLongMayWeekendTask.Wait();
            if (timeUntilTheExamsStartTask != null)
                timeUntilTheExamsStartTask.Wait();
            if (timeToIssueGradesForTheRemainingClassesTask != null)
                timeToIssueGradesForTheRemainingClassesTask.Wait();
            if (timeUntilTheEndOfTheSchoolYearForTheRemainingClassesTask != null)
                timeUntilTheEndOfTheSchoolYearForTheRemainingClassesTask.Wait();
            if (timeUntilTheEndOfTheHolidayTask != null)
                timeUntilTheEndOfTheHolidayTask.Wait();
        }
    }
}
