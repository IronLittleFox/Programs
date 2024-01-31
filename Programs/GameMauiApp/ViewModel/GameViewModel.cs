using GameMauiApp.Model;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMauiGame.View;
using UtilsMaui.Interfaces;

namespace GameMauiApp.ViewModel
{
    public class GameViewModel : BindableObject
    {
        public ObservableCollection<GameInfo> ListOfGame { get; set; }

        private GameInfo selectedGame;
        public GameInfo SelectedGame
        {
            get { return selectedGame; }
            set
            {
                selectedGame = value;
                SelectedGameView?.Dispose();
                SelectedGameView = ServiceHelper.Services.GetService(selectedGame.GameType) as IDisposableGameView;
                //SelectedGameView = Activator.CreateInstance(selectedGame.GameType) as IDisposableGameView;
                OnPropertyChanged();
            }
        }

        private IDisposableGameView selectedGameView;
        public IDisposableGameView SelectedGameView
        {
            get { return selectedGameView; }
            set
            {
                selectedGameView = value;
                OnPropertyChanged();
            }
        }

        public GameViewModel()
        {
            ListOfGame = new ObservableCollection<GameInfo>();

            ListOfGame.Add(new GameInfo() { NameOfGame = "Kółko i krzyżyk", GameType = typeof(TicTacToeView) });
            /*ListOfGame.Add(new GameInfo() { NameOfGame = "Saper", PathToGame="MinesweeperGame" });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Memory", PathToGame="MemoryGame" });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Połącz czwórki", PathToGame="ConnectFourGame" });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Warcaby", PathToGame="CheckersGame" });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Sudoku", PathToGame="SudokuGame" });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Go", PathToGame="GoGame" });*/

            SelectedGame = ListOfGame.FirstOrDefault();
        }
    }
}
