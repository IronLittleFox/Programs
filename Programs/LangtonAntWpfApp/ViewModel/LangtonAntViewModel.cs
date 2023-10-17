using GameUtils.Utils;
using LangtonAntWpfApp.Model;
using LangtonAntWpfApp.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace LangtonAntWpfApp.ViewModel
{
    public class LangtonAntViewModel : ViewObserver
    {
        public ObservableCollection<BoardField> Board { get { return Dane.Board; } }
        public int ColumnCount
        {
            get { return Dane.ColumnCount; }
        }

        public int RowCount
        {
            get { return Dane.RowCount; }
        }

        private ICommand? startNextAntCommand = null;
        public ICommand StartNextAntCommand
        {
            get
            {
                if (startNextAntCommand == null)
                    startNextAntCommand = new RelayCommand<object>(
                        o =>
                        {
                            Task.Run(() => 
                            {
                                currentAntColor = (currentAntColor + 1) % antcolors.Count;
                                LangtonAnt langtonAnt = new LangtonAnt(antcolors[currentAntColor]);
                                while (true)
                                {
                                    langtonAnt.Move();
                                    Application.Current.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
                                    Thread.Sleep(100);
                                }
                            });
                            
                        }
                        );
                return startNextAntCommand;
            }
        }

        private List<string> antcolors = new List<string>() { "Red", "Green", "Blue" };
        private int currentAntColor = -1;
    }
}
