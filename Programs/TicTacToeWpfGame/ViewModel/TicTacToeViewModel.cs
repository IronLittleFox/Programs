using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TicTacToeWpfGame.Model;
using GameUtils.Interfaces;

namespace TicTacToeWpfGame.ViewModel
{
    public class TicTacToeViewModel: ViewObserver, IGameViewModel
    {
        #region GameScore
        private bool showGameScore = false;
        public bool ShowGameScore
        {
            get { return showGameScore; }
            set
            {
                showGameScore = value;
                OnPropertyChanged(nameof(ShowGameScore));
            }
        }

        private string showMessageScore;
        public string ShowMessageScore
        {
            get { return showMessageScore; }
            set
            {
                showMessageScore = value;
                OnPropertyChanged(nameof(ShowMessageScore));
            }
        }

        private ICommand closeGameScoreCommand;
        public ICommand CloseGameScoreCommand
        {
            get
            {
                if (closeGameScoreCommand == null)
                    closeGameScoreCommand = new RelayCommand<object>(
                        o =>
                        {
                            ShowGameScore = false;
                        }
                        );
                return closeGameScoreCommand;
            }
        }
        #endregion

        private bool startGame;
        public bool StartGame
        {
            get { return startGame; }
            set
            {
                startGame = value;
                OnPropertyChanged(nameof(StartGame));
            }
        }

        private CircularObservableCollection<Player> _listOfPlayers;
        public CircularObservableCollection<Player> ListOfPlayers
        {
            get { return _listOfPlayers; }
            set
            {
                _listOfPlayers = value;
                OnPropertyChanged();
            }
        }

        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        private ICommand boardFieldCommand;
        public ICommand BoardFieldCommand
        {
            get
            {
                if (boardFieldCommand == null)
                    boardFieldCommand = new RelayCommand<PlayingField>(
                        playingField =>
                        {
                            if (!startGame)
                                return;
                            if (playingField.Text != "")
                                return;

                            playingField.Text = currentPlayer.Name;

                            if (CheckWin(currentPlayer.Name))
                            {
                                ShowGameScore = true;
                                StartGame = false;
                                ShowMessageScore = "Koniec gry.\nWygrana " + currentPlayer.Name;
                                return;
                            }

                            if (CheckDraw())
                            {
                                StartGame = false;
                                ShowGameScore = true;
                                ShowMessageScore = "Koniec gry.\nRemis.";
                                return;
                            }

                            currentPlayer = ListOfPlayers.GetNext();
                        }
                        );
                return boardFieldCommand;
            }
        }

        private ICommand startGameCommand;
        public ICommand StartGameCommand
        {
            get
            {
                if (startGameCommand == null)
                    startGameCommand = new RelayCommand<object>(
                        o =>
                        {
                            if (ShowGameScore)
                                return;
                            NewGame();
                        }
                        );
                return startGameCommand;
            }
        }

        private ObservableCollection<PlayingField> _listOfField;
        public ObservableCollection<PlayingField> ListOfField
        {
            get { return _listOfField; }
            set
            {
                _listOfField = value;
                OnPropertyChanged();
            }
        }

        private int _columnCount;
        public int ColumnCount
        {
            get { return _columnCount; }
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

        private int _rowCount;
        public int RowCount
        {
            get { return _rowCount; }
            set
            {
                _rowCount = value;
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

        private ObservableCollection<int> listOfLines;
        public ObservableCollection<int> ListOfLines
        {
            get { return listOfLines; }
            set
            {
                listOfLines = value;
                OnPropertyChanged();
            }
        }

        private Player currentPlayer;

        public TicTacToeViewModel()
        {
            ShowGameScore = false;

            ListOfLines = new ObservableCollection<int>() { 3, 4, 5, 6, 7, 8 };
            SelectedOptionLines = ListOfLines.First();
            ListOfPlayers = new CircularObservableCollection<Player>()
            {
                new Player(){ Name = "X"},
                new Player(){ Name = "O"},
            };

            SelectedPlayer = ListOfPlayers.FirstOrDefault();

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
    }
}
