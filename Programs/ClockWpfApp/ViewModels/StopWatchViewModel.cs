using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilitiesWpf;

namespace ClockWpfApp.ViewModels
{
    class StopWatchViewModel : ObserverVM
    {
        private bool isStopWatchRunning = false;

        private string timeStorage;
        public string TimeStorage
        {
            get { return timeStorage; }
            set 
            { 
                timeStorage = value;
                OnPropertyChanged(nameof(TimeStorage));
            }
        }

        private ICommand startCommand;
        public ICommand StartCommand
        {
            get
            {
                if (startCommand == null)
                    startCommand = new RelayCommand<object>(
                        o =>
                        {
                            DateTime startTime = DateTime.Now;
                            isStopWatchRunning = true;
                            Task.Run(() =>
                            {
                                
                                //int stopWatchMilisecond = 0;
                                while (isStopWatchRunning)
                                {
                                    //stopWatchMilisecond++;
                                    //TimeSpan timeSpan = TimeSpan.FromMilliseconds(stopWatchMilisecond);

                                    TimeSpan timeSpan = DateTime.Now - startTime;
                                    string stringTime = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
                                    TimeStorage = stringTime;

                                    Thread.Sleep(1000);
                                    
                                }
                            });
                        },
                        o => !isStopWatchRunning
                        );
                return startCommand;
            }
        }

        private ICommand stopCommand;
        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                    stopCommand = new RelayCommand<object>(
                        o =>
                        {
                            isStopWatchRunning = false;
                        },
                        o => isStopWatchRunning
                        );
                return stopCommand;
            }
        }

        private int number;

        public int Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged(nameof(Number)); }
        }

        private ICommand numberCommand;
        public ICommand NumberCommand
        {
            get
            {
                if (numberCommand == null)
                    numberCommand = new RelayCommand<object>(
                        o =>
                        {
                            Number = 5;

                            Task.Run(() =>
                            {
                                Number = Number + 2;
                            });
                            Task.Run(() =>
                            {
                                Number = Number * 2;
                            });
                        }
                        );
                return numberCommand;
            }
        }
    }
}
