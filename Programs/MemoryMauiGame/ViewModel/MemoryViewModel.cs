using MemoryMauiGame.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UtilsMaui.Utils;
using UtilsMaui.Interfaces;

namespace MemoryMauiGame.ViewModel
{
    public class MemoryViewModel : BindableObject, IGameViewModel
    {
        private Task? timeTask;
        public int NumberOfSets => RowCount * ColumnCount / SetNumbersOfItem;
        private List<PlayingField> listOfDiscoverField = new List<PlayingField>();

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
                OnPropertyChanged(nameof(NumberOfSets));
            }
        }

        private ObservableCollection<PlayingField> listOfField = new();
        public ObservableCollection<PlayingField> ListOfPlayingField
        {
            get { return listOfField; }
            set
            {
                listOfField = value;
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

        private ICommand? newGameCommand;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                    newGameCommand = new Command<object>(
                        o =>
                        {
                            RunNewGame();
                        }
                        );
                return newGameCommand;
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
                            if (IsEndGame
                            || DiscoverItemCount == NumberOfSets)
                                return;

                            if (listOfDiscoverField.Count >= SetNumbersOfItem)
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

        private string gameTime = "";
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

        private int selectedOptionNumberOfItem;
        public int SelectedOptionNumberOfItem
        {
            get { return selectedOptionNumberOfItem; }
            set
            {
                selectedOptionNumberOfItem = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<int> listOfNumbersOfItem = new();
        public ObservableCollection<int> ListOfNumbersOfItem
        {
            get { return listOfNumbersOfItem; }
            set
            {
                listOfNumbersOfItem = value;
                OnPropertyChanged();
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

        private ObservableCollection<int> listOfRows = new();
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

        private ObservableCollection<int> listOfCols = new();
        public ObservableCollection<int> ListOfCols
        {
            get { return listOfCols; }
            set
            {
                listOfCols = value;
                OnPropertyChanged();
            }
        }

        public MemoryViewModel()
        {
            IsEndGame = false;
            RunGame = false;
            ListOfNumbersOfItem = new ObservableCollection<int>() { 2, 3, 4, 5 };
            SelectedOptionNumberOfItem = ListOfNumbersOfItem.First();

            ListOfRows = new ObservableCollection<int>() { 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionRow = ListOfRows.First();
            ListOfCols = new ObservableCollection<int>() { 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionCol = ListOfCols.First();
            RunNewGame();
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
            SetNumbersOfItem = SelectedOptionNumberOfItem;
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

        public void Dispose()
        { 
        }
    }
}
