using GameUtils.Interfaces;
using GameUtils.Utils;
using MinesweeperWpfGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MinesweeperWpfGame.ViewModel
{
    public class MinesweeperViewModel : ViewObserver, IGameViewModel
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

        #region Time

        private string gameTime;
        public string GameTime
        {
            get { return gameTime; }
            set
            {
                gameTime = value;
                OnPropertyChanged(nameof(GameTime));
            }
        }

        private Task timeTask;

        #endregion

        #region Board

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

        private int minesCount;
        public int MinesCount
        {
            get { return minesCount; }
            set
            {
                minesCount = value;
                OnPropertyChanged(nameof(MinesCount));
            }
        }

        private ObservableCollection<PlayingField> _listOfField;
        public ObservableCollection<PlayingField> ListOfPlayingField
        {
            get { return _listOfField; }
            set
            {
                _listOfField = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private bool isEndGame;
        public bool IsEndGame
        {
            get { return isEndGame; }
            set
            {
                isEndGame = value;
                OnPropertyChanged(nameof(IsEndGame));
            }
        }

        private bool runGame;
        public bool RunGame
        {
            get { return runGame; }
            set
            {
                runGame = value;
                OnPropertyChanged(nameof(RunGame));
            }
        }

        private ICommand newGameCommand;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                    newGameCommand = new RelayCommand<object>(
                        o =>
                        {
                            if (ShowGameScore)
                                return;
                            RunNewGame();
                        }
                        );
                return newGameCommand;
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
                            if (IsEndGame)
                                return;

                            if (playingField.HiddenText == "M")
                            {
                                playingField.Text = playingField.HiddenText;
                                ShowMessageScore = "Koniec gry.\nPrzegrana.";
                                ShowGameScore = true;
                                IsEndGame = true;
                                return;
                            }

                            GameMove(playingField);

                            if (ListOfPlayingField.Where(x => x.Text == "" || x.Text == "?").Count() == MinesCount)
                            {
                                ShowMessageScore = "Koniec gry.\nWygrana.";
                                ShowGameScore = true;
                                IsEndGame = true;
                                return;
                            }
                        }
                        );
                return boardFieldCommand;
            }
        }

        private ICommand mineProbabilityCommand;
        public ICommand MineProbabilityCommand
        {
            get
            {
                if (mineProbabilityCommand == null)
                    mineProbabilityCommand = new RelayCommand<PlayingField>(
                        playingField =>
                        {
                            if (IsEndGame)
                                return;

                            if (playingField.Text == "")
                                playingField.Text = "?";
                            else if (playingField.Text == "?")
                                playingField.Text = "";
                        }
                        );
                return mineProbabilityCommand;
            }
        }

        private int selectedOptionRow;
        public int SelectedOptionRow
        {
            get { return selectedOptionRow; }
            set
            {
                selectedOptionRow = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<int> listOfRows;
        public ObservableCollection<int> ListOfRows
        {
            get { return listOfRows; }
            set
            {
                listOfRows = value;
                OnPropertyChanged();
            }
        }

        private int selectedOptionCol;
        public int SelectedOptionCol
        {
            get { return selectedOptionCol; }
            set
            {
                selectedOptionCol = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<int> listOfCols;
        public ObservableCollection<int> ListOfCols
        {
            get { return listOfCols; }
            set
            {
                listOfCols = value;
                OnPropertyChanged();
            }
        }

        private int selectedOptionPercentMine;
        public int SelectedOptionPercentMine
        {
            get { return selectedOptionPercentMine; }
            set
            {
                selectedOptionPercentMine = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<int> listOfPercentMins;
        public ObservableCollection<int> ListOfPercentMins
        {
            get { return listOfPercentMins; }
            set
            {
                listOfPercentMins = value;
                OnPropertyChanged();
            }
        }

        public MinesweeperViewModel()
        {
            IsEndGame = false;
            ShowGameScore = false;
            RunGame = false;

            ListOfRows = new ObservableCollection<int>() { 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionRow = ListOfRows.First();
            ListOfCols = new ObservableCollection<int>() { 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionCol = ListOfCols.First();
            ListOfPercentMins = new ObservableCollection<int>() { 10, 15, 20, 25, 30, 40, 50 };
            SelectedOptionPercentMine = ListOfPercentMins.First();

        }

        private void RunNewGame()
        {
            IsEndGame = true;
            if (timeTask != null)
                timeTask.Wait();
            IsEndGame = false;
            RunGame = true;

            RowCount = SelectedOptionRow;
            ColumnCount = SelectedOptionCol;
            MinesCount = RowCount * ColumnCount * SelectedOptionPercentMine / 100;

            ListOfPlayingField = new ObservableCollection<PlayingField>();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    ListOfPlayingField.Add(new PlayingField()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        Text = "",
                        HiddenText = "",
                        BoardFieldCommand = BoardFieldCommand,
                        MineProbabilityCommand = MineProbabilityCommand
                    });
                }

            //stawiam miny
            Random rand = new Random();
            ListOfPlayingField.OrderBy(x => rand.Next()).Take(MinesCount).ForAll(x => x.HiddenText = "M");
            //obliczam ile jest min dookoła
            ListOfPlayingField.Where(x => x.HiddenText != "M").ForAll(x =>
            {

                var fieldsAroundList = ListOfPlayingField.Where(y => (y.RowIndex == x.RowIndex - 1
                                                  && (y.ColumnIndex == x.ColumnIndex - 1
                                                    || y.ColumnIndex == x.ColumnIndex
                                                    || y.ColumnIndex == x.ColumnIndex + 1))
                                                || (y.RowIndex == x.RowIndex
                                                     && (y.ColumnIndex == x.ColumnIndex - 1
                                                      || y.ColumnIndex == x.ColumnIndex + 1))
                                                || (y.RowIndex == x.RowIndex + 1
                                                  && (y.ColumnIndex == x.ColumnIndex - 1
                                                    || y.ColumnIndex == x.ColumnIndex
                                                    || y.ColumnIndex == x.ColumnIndex + 1))
                                                    );
                int coutOfMines = fieldsAroundList.Where(y => y.HiddenText == "M").Count();
                x.HiddenText = coutOfMines.ToString();
            });
            timeTask = Task.Run(CountGameTime);
        }

        private void GameMove(PlayingField playingField)
        {
            playingField.Text = playingField.HiddenText;

            if (playingField.Text == "M")
                return;

            if (playingField.Text == "0")
            {
                var fieldsAroundList = ListOfPlayingField.Where(x => (x.RowIndex == playingField.RowIndex - 1
                                                              && (x.ColumnIndex == playingField.ColumnIndex - 1
                                                                || x.ColumnIndex == playingField.ColumnIndex
                                                                || x.ColumnIndex == playingField.ColumnIndex + 1))
                                                            || (x.RowIndex == playingField.RowIndex
                                                                 && (x.ColumnIndex == playingField.ColumnIndex - 1
                                                                  || x.ColumnIndex == playingField.ColumnIndex + 1))
                                                            || (x.RowIndex == playingField.RowIndex + 1
                                                              && (x.ColumnIndex == playingField.ColumnIndex - 1
                                                                || x.ColumnIndex == playingField.ColumnIndex
                                                                || x.ColumnIndex == playingField.ColumnIndex + 1))
                                                                )
                                                   .Where(x => x.Text == "" || x.Text == "?");
                fieldsAroundList.ForAll(x => x.BoardFieldCommand.Execute(x));
            }
        }

        #region Time method

        private void CountGameTime()
        {
            DateTime startGameDateTime = DateTime.Now;
            while (!IsEndGame)
            {
                TimeSpan t = DateTime.Now - startGameDateTime;
                GameTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                Thread.Sleep(1);
            }
        }
        #endregion
    }
}
