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
    class CurrentTimeViewModel : ObserverVM
    {
        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        private ICommand _refreshCurrentTime;
        public ICommand RefreshCurrentTime
        {
            get
            {
                if (_refreshCurrentTime == null)
                    _refreshCurrentTime = new RelayCommand<object>(
                        o =>
                        {
                            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                        }
                        );
                return _refreshCurrentTime;
            }
        }

        public CurrentTimeViewModel()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");

            Task task = new Task(() =>
            {
                while (true)
                {
                    CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                    Thread.Sleep(1000);
                }

            });
            task.Start();
        }

        
    }
}
