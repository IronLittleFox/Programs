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

        public ObservableCollection<PlayerScore> PlayerScoreList { get; set; }

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

                            playingField.Text = SelectedPlayer.Name;

                            if (CheckWin(SelectedPlayer.Name))
                            {
                                PlayerScoreList.FirstOrDefault(ps => ps.Player == SelectedPlayer).Score++;
                                ShowGameScore = true;
                                StartGame = false;
                                ShowMessageScore = "Koniec gry.\nWygrana " + SelectedPlayer.Name;
                                return;
                            }

                            if (CheckDraw())
                            {
                                StartGame = false;
                                ShowGameScore = true;
                                ShowMessageScore = "Koniec gry.\nRemis.";
                                return;
                            }

                            SelectedPlayer = ListOfPlayers.GetNext();
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
                            ListOfField.ForAll(pf => pf.Text = "");
                            StartGame = true;
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

        public TicTacToeViewModel()
        {
            ShowGameScore = false;
            RowCount = 3;
            ColumnCount = RowCount;

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

            ListOfPlayers = new CircularObservableCollection<Player>()
            {
                new Player(){ Name = "X"},
                new Player(){ Name = "O"},
            };

            SelectedPlayer = ListOfPlayers.FirstOrDefault();

            PlayerScoreList = ListOfPlayers.Select(p => new PlayerScore() { Player = p, Score = 0 }).ToObservableCollectio();
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
