using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using UtilsMaui.Interfaces;
using SlidingPuzzleMauiGame.Model;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;
using UtilsMaui.Utils;

namespace SlidingPuzzleMauiGame.ViewModel
{
    public class SlidingPuzzleViewModel : BindableObject, IGameViewModel
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
                        async playingField =>
                        {
                            if (IsEndGame)
                                return;

                            PlayingField? destinationPlayingField =
                            ListOfPlayingField.FirstOrDefault(pf =>
                            pf.Text == ""
                            //góra
                            && ((pf.RowIndex + 1 == playingField.RowIndex && pf.ColumnIndex == playingField.ColumnIndex)
                                 //dół
                                 || (pf.RowIndex - 1 == playingField.RowIndex && pf.ColumnIndex == playingField.ColumnIndex)
                                 //prawo
                                 || (pf.RowIndex == playingField.RowIndex && pf.ColumnIndex - 1 == playingField.ColumnIndex)
                                 //lewo
                                 || (pf.RowIndex == playingField.RowIndex && pf.ColumnIndex + 1 == playingField.ColumnIndex)
                                 )
                            );

                            if (destinationPlayingField != null)
                            {
                                destinationPlayingField.Text = playingField.Text;
                                playingField.Text = "";
                            }

                            if (ListOfPlayingField.All(pf => pf.Text == "" || pf.Text == (pf.RowIndex * RowCount + pf.ColumnIndex).ToString()))
                            {
                                IsEndGame = true;
                                await popupService.ShowPopupAsync<SlidingPuzzlePopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Gratulacje.";
                                    });
                            }
                        }
                        );
                return boardFieldCommand;
            }
        }

        IPopupService popupService;

        public SlidingPuzzleViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

            IsEndGame = false;

            ListOfRows = new ObservableCollection<int>() {3, 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionRow = ListOfRows.First();
            ListOfCols = new ObservableCollection<int>() {3, 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionCol = ListOfCols.First();

            RunNewGame();
        }

        private void RunNewGame()
        {
            Random random = new();

            IsEndGame = false;

            RowCount = SelectedOptionRow;
            ColumnCount = SelectedOptionCol;

            List<int> listOfNumbers = new List<int>();
            for (int i = 0; i < RowCount * ColumnCount - 1; i++)
            {
                listOfNumbers.Add(i);
            }
            listOfNumbers = listOfNumbers.OrderBy(x => random.Next()).ToList();

            ListOfPlayingField = new ObservableCollection<PlayingField>();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    string text = "";
                    if (listOfNumbers.Count > 0)
                    {
                        text = listOfNumbers.First().ToString();
                        listOfNumbers.Remove(listOfNumbers.First());
                    }

                    ListOfPlayingField.Add(new PlayingField()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        Text = text,
                        BoardFieldCommand = BoardFieldCommand
                    });
                }
        }

        public void Dispose()
        {
        }
    }
}
