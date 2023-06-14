using GameUtils.Interfaces;
using GameUtils.Utils;
using MemoWpfGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoWpfGame.ViewModel
{
    class MemoViewModel : ViewObserver, IGameViewModel
    {
        private Task timeTask;
        public int NumberOfSets => RowCount * ColumnCount / SetNumbersOfItem;
        private List<PlayingField> listOfDiscoverField = new List<PlayingField>();

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

        private int discoverItemCount;
        public int DiscoverItemCount
        {
            get { return discoverItemCount; }
            set
            {
                discoverItemCount = value;
                OnPropertyChanged(nameof(DiscoverItemCount));
            }
        }

        private int setNumbersOfItem;
        public int SetNumbersOfItem
        {
            get { return setNumbersOfItem; }
            set
            {
                setNumbersOfItem = value;
                OnPropertyChanged(nameof(SetNumbersOfItem));
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

        private ICommand newGameCommand;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                    newGameCommand = new RelayCommand<object>(
                        o =>
                        {
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
                            if (IsEndGame
                            || DiscoverItemCount == NumberOfSets)
                                return;

                            if (listOfDiscoverField.Count == SetNumbersOfItem)
                            {
                                HideDiscoverField();
                            }
                            if (playingField.Text != "")
                                return;
                            listOfDiscoverField.Add(playingField);
                            playingField.Text = playingField.HiddenText;
                            if (listOfDiscoverField.Count == SetNumbersOfItem
                                && listOfDiscoverField.All(pf => pf.Text == playingField.Text))
                            {
                                listOfDiscoverField.Clear();
                                DiscoverItemCount++;
                            }

                        }
                        );
                return boardFieldCommand;
            }
        }

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

        public MemoViewModel()
        {
            IsEndGame = false;
            RunGame = false;
            RunNewGame();
        }

        private void RunNewGame()
        {
            IsEndGame = true;
            if (timeTask != null)
                timeTask.Wait();
            IsEndGame = false;
            RunGame = true;

            RowCount = 3;
            ColumnCount = 4;
            SetNumbersOfItem = 3;
            DiscoverItemCount = 0;

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
                    });
                }

            int count = 0;
            Random random = new Random();
            foreach (PlayingField item in ListOfPlayingField.OrderBy(pf => random.Next()).Take(NumberOfSets * SetNumbersOfItem))
            {
                item.HiddenText = (count++ % NumberOfSets + 1).ToString();
            }
            ListOfPlayingField.Where(pf => pf.HiddenText == "").ForAll(pf => pf.HiddenText = "?");

            timeTask = Task.Run(CountGameTime);
        }

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

        private void HideDiscoverField()
        {
            foreach (PlayingField item in listOfDiscoverField)
            {
                item.Text = "";
            }
            listOfDiscoverField.Clear();
        }

    }
}
