using GamesWpfApp.Model;
using GameUtils.Utils;
using MinesweeperWpfGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWpfGame.ViewModel;

namespace GamesWpfApp.ViewModel
{
    class MainWindowViewModel : ViewObserver
    {
        public ObservableCollection<GameInfo> ListOfGame { get; set; }

        private GameInfo selectedGame;
        public GameInfo SelectedGame
        {
            get { return selectedGame; }
            set
            {
                selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }

        public MainWindowViewModel()
        {
            ListOfGame = new ObservableCollection<GameInfo>();

            ListOfGame.Add(new GameInfo() { NameOfGame = "Kółko i krzyżyk"/*, GameViewModel = new TicTacToeViewModel()*/ });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Saper"/*, GameViewModel = new MinesweeperViewModel()*/ });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Memo"/*, GameViewModel = new MinesweeperViewModel()*/ });

            SelectedGame = ListOfGame.FirstOrDefault();
        }

    }
}
