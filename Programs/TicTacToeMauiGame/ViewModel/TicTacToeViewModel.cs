using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TicTacToeMauiGame.Model;
using UtilsMaui.Interfaces;
using UtilsMaui.Utils;

namespace TicTacToeMauiGame.ViewModel
{
    public class TicTacToeViewModel : BindableObject, IGameViewModel
    {
        private int columnCount;
        public int ColumnCount
        {
            get { return columnCount; }
            set
            {
                columnCount = value;
                OnPropertyChanged();
            }
        }

        private int rowCount;
        public int RowCount
        {
            get { return rowCount; }
            set
            {
                rowCount = value;
                OnPropertyChanged();
            }
        }

        private CircularObservableCollection<Player> listOfPlayers = new CircularObservableCollection<Player>();
        public CircularObservableCollection<Player> ListOfPlayers
        {
            get { return listOfPlayers; }
            set
            {
                listOfPlayers = value;
                OnPropertyChanged();
            }
        }

        private Player selectedPlayer = new Player();
        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set
            {
                selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        private ObservableCollection<int> listOfLines = new ObservableCollection<int>();
        public ObservableCollection<int> ListOfLines
        {
            get { return listOfLines; }
            set
            {
                listOfLines = value;
                OnPropertyChanged();
            }
        }

        private int selectedOptionLines;
        public int SelectedOptionLines
        {
            get { return selectedOptionLines; }
            set
            {
                selectedOptionLines = value;
                OnPropertyChanged();
            }
        }

        private ICommand? startGameCommand;
        public ICommand StartGameCommand
        {
            get
            {
                if (startGameCommand == null)
                    startGameCommand = new Command<object>(
                        o =>
                        {
                            NewGame();
                        }
                        );
                return startGameCommand;
            }
        }

        private ObservableCollection<PlayingField> listOfField = new ObservableCollection<PlayingField>();
        public ObservableCollection<PlayingField> ListOfField
        {
            get { return listOfField; }
            set
            {
                listOfField = value;
                OnPropertyChanged();
            }
        }

        private ICommand? boardFieldCommand;
        public ICommand BoardFieldCommand
        {
            get
            {
                if (boardFieldCommand == null)
                    boardFieldCommand = new Command<PlayingField>(
                        playingField =>
                        {
                            if (!startGame)
                                return;
                            if (playingField.Text != "")
                                return;

                            playingField.Text = currentPlayer.Name;

                            if (CheckWin(currentPlayer.Name))
                            {
                                StartGame = false;
                                popupService.ShowPopupAsync<TicTacToePopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Koniec gry.\nWygrywa:\n";
                                        vm.ImageSymbol = currentPlayer.Name;
                                    });
                                return;
                            }

                            if (CheckDraw())
                            {
                                StartGame = false;
                                //ShowMessageScore = "Koniec gry.\nRemis.";
                                popupService.ShowPopupAsync<TicTacToePopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Koniec gry.\nRemis.\n";
                                        vm.ImageSymbol = "Draw";
                                    });
                                return;
                            }

                            currentPlayer = ListOfPlayers.GetNext();
                        }
                        );
                return boardFieldCommand;
            }
        }

        private bool startGame = false;
        public bool StartGame
        {
            get { return startGame; }
            set
            {
                startGame = value;
                OnPropertyChanged(nameof(StartGame));
            }
        }

        private Player currentPlayer = new Player();
        IPopupService popupService;

        public TicTacToeViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

            ListOfPlayers = new CircularObservableCollection<Player>()
            {
                new Player(){ Name = "X"},
                new Player(){ Name = "O"},
            };

            SelectedPlayer = ListOfPlayers.First();

            ListOfLines = new ObservableCollection<int>() { 3, 4, 5, 6, 7, 8 };
            SelectedOptionLines = ListOfLines.First();

            NewGame();
        }

        private void NewGame()
        {
            StartGame = true;

            ListOfPlayers.SetCurrent(SelectedPlayer);
            currentPlayer = SelectedPlayer;
            RowCount = SelectedOptionLines;
            ColumnCount = SelectedOptionLines;

            ListOfField = new ObservableCollection<PlayingField>();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    ListOfField.Add(new PlayingField()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        Text = "",
                        BoardFieldCommand = BoardFieldCommand
                    }); ;
                }
        }

        private bool CheckDraw()
        {
            return !ListOfField.Any(x => x.Text == "");
        }

        private bool CheckWin(string player)
        {
            foreach (var columbGroup in ListOfField.GroupBy(x => x.ColumnIndex))
            {
                if (CheckLineWin(player, columbGroup.ToList()))
                    return true;
            }

            foreach (var rowGroup in ListOfField.GroupBy(x => x.RowIndex))
            {
                if (CheckLineWin(player, rowGroup.ToList()))
                    return true;
            }

            if (CheckLineWin(player, ListOfField.Where(x => x.RowIndex == x.ColumnIndex).ToList()))
                return true;

            if (CheckLineWin(player, ListOfField.Where(
                x => x.ColumnIndex == Math.Sqrt(ListOfField.Count) - 1 - x.RowIndex).ToList()))
                return true;

            return false;
        }

        private bool CheckLineWin(string player, List<PlayingField> column)
        {
            return column.All(playingField => playingField.Text == player);
        }

        public void Dispose()
        {

        }
    }
}
