using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilsMaui.Interfaces;
using Create2048MauiGame.Model;
using CommunityToolkit.Maui.Core;
using Create2048MauiGame.Enums;

namespace Create2048MauiGame.ViewModel
{
    public class Create2048ViewModel : BindableObject, IGameViewModel
    {
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
                            if (IsEndGame)
                                return;


                        }
                        );
                return boardFieldCommand;
            }
        }

        private ICommand? movmentCommand;
        public ICommand MovmentCommand
        {
            get
            {
                if (movmentCommand == null)
                    movmentCommand = new Command<Move>(
                        move =>
                        {
                            if (IsEndGame)
                                return;

                            List<List<PlayingField>> collectionOfLines;

                            if (move == Move.Up || move == Move.Down)
                            {
                                var orderByDescending = move == Move.Down;
                                collectionOfLines = ListOfPlayingField
                                    .GroupBy(pf => pf.ColumnIndex)
                                    .Select(group => group.OrderBy(pf => orderByDescending ? -pf.RowIndex : pf.RowIndex).ToList())
                                    .ToList();
                            }
                            else
                            {
                                var orderByDescending = move == Move.Right;
                                collectionOfLines = ListOfPlayingField
                                    .GroupBy(pf => pf.RowIndex)
                                    .Select(group => group.OrderBy(pf => orderByDescending ? -pf.ColumnIndex : +pf.ColumnIndex).ToList())
                                    .ToList();
                            }

                            bool isMoveMade = false;
                            foreach (var line in collectionOfLines)
                            {
                                PlayingField? playingFieldHead = null;
                                while (true)
                                {
                                    playingFieldHead = line.FirstOrDefault();

                                    if (playingFieldHead == null)
                                        break;
                                    line.Remove(playingFieldHead);

                                    PlayingField? playingFieldFirst;
                                    if (playingFieldHead.Text != " ")
                                        playingFieldFirst = playingFieldHead;
                                    else
                                        playingFieldFirst = line.FirstOrDefault(pf => pf.Text != " ");

                                    if (playingFieldFirst == null)
                                        break;

                                    if (!isMoveMade
                                        && playingFieldHead.Text == " ")
                                        isMoveMade = true;

                                    int firstNumber = int.Parse(playingFieldFirst.Text);
                                    playingFieldFirst.Text = " ";
                                    playingFieldFirst.Color = colorForNumbers[playingFieldFirst.Text];

                                    PlayingField? playingFieldSecond = line.FirstOrDefault(pf => pf.Text != " ");

                                    int secondNumber = 0;
                                    if (playingFieldSecond != null
                                        && playingFieldSecond.Text == firstNumber.ToString())
                                    {
                                        secondNumber = int.Parse(playingFieldSecond.Text);
                                        playingFieldSecond.Text = " ";
                                        playingFieldSecond.Color = colorForNumbers[playingFieldSecond.Text];
                                        isMoveMade = true;
                                    }

                                    playingFieldHead.Text = (firstNumber + secondNumber).ToString();
                                    if (colorForNumbers.ContainsKey(playingFieldHead.Text))
                                        playingFieldHead.Color = colorForNumbers[playingFieldHead.Text];
                                    else
                                        playingFieldHead.Color = colorForNumbers["pozostałe"];
                                }
                            }

                            if (!isMoveMade)
                                return;

                            Random random = new Random();
                            PlayingField? playingFieldRandom = ListOfPlayingField
                            .Where(pf => pf.Text == " ")
                            .OrderBy(pf => random.Next())
                            .FirstOrDefault();

                            if (playingFieldRandom != null)
                            {
                                playingFieldRandom.Text = (random.Next(1, 3) * 2).ToString();
                                playingFieldRandom.Color = colorForNumbers[playingFieldRandom.Text];
                            }
                        }
                        );
                return movmentCommand;
            }
        }

        IPopupService popupService;
        Dictionary<string, string> colorForNumbers;

        public Create2048ViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

            ListOfRows = new ObservableCollection<int>() { 3, 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionRow = ListOfRows.First();
            ListOfCols = new ObservableCollection<int>() { 3, 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionCol = ListOfCols.First();

            colorForNumbers = new Dictionary<string, string>
            {
                { " ", "#787222" },
                { "2", "#d2fbe1" },
                { "4", "#ac2981" },
                { "8", "#a10fa3" },
                { "16", "#a5e32a" },
                { "32", "#6a8015" },
                { "64", "#df5bd8" },
                { "128", "#1a44a9" },
                { "256", "#7ea17a" },
                { "512", "#b57d7a" },
                { "1024", "#88a349" },
                { "2048", "#e60811" },
                { "4096", "#3b50e8" },
                { "8192", "#abfd9a" },
                { "16384", "#cbd90d" },
                { "32768", "#93a5e1" },
                { "65536", "#07e3dc" },
                { "131072", "#8919c7" },
                { "262144", "#e7f37b" },
                { "524288", "#d79814" },
                { "pozostałe", "#D1BFE9" }
            };

            RunNewGame();
        }

        private void RunNewGame()
        {
            IsEndGame = false;

            RowCount = SelectedOptionRow;
            ColumnCount = SelectedOptionCol;

            ListOfPlayingField = new ObservableCollection<PlayingField>();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    string text = " ";

                    ListOfPlayingField.Add(new PlayingField()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        Text = text,
                        Color = colorForNumbers[text],
                        BoardFieldCommand = BoardFieldCommand
                    }) ;
                }

            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                PlayingField? playingFieldRandom = ListOfPlayingField
                .Where(pf => pf.Text == " ")
                .OrderBy(pf => random.Next())
                .FirstOrDefault();

                if (playingFieldRandom != null)
                {
                    playingFieldRandom.Text = (random.Next(1, 3) * 2).ToString();
                    playingFieldRandom.Color = colorForNumbers[playingFieldRandom.Text];
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
