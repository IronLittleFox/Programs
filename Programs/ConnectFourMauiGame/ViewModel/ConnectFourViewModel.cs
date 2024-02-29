using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsMaui.Interfaces;
using ConnectFourMauiGame.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;

namespace ConnectFourMauiGame.ViewModel
{
    public class ConnectFourViewModel : BindableObject, IGameViewModel
    {
        private bool runGame = true;
        public bool RunGame
        {
            get { return runGame; }
            set
            {
                runGame = value;
                OnPropertyChanged(nameof(RunGame));
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

        private ObservableCollection<PlayingField> _listOfField = new();
        public ObservableCollection<PlayingField> ListOfPlayingField
        {
            get { return _listOfField; }
            set
            {
                _listOfField = value;
                OnPropertyChanged();
            }
        }

        private int currentPlayerNumber = 0;

        private Player currentPlayer;
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        private ICommand? boardFieldCommand = null;
        public ICommand BoardFieldCommand
        {
            get
            {
                if (boardFieldCommand == null)
                    boardFieldCommand = new Command<PlayingField>(
                        playingField =>
                        {
                            if (isEndGame)
                                return;

                            PlayingField? firstFreePlayingFieldInRow
                            = ListOfPlayingField.Where(pf => pf.ColumnIndex == playingField.ColumnIndex
                                                             && pf.FieldColor == emptyColorField)
                            .OrderByDescending(pf => pf.RowIndex).FirstOrDefault();
                            if (firstFreePlayingFieldInRow != null)
                            {
                                firstFreePlayingFieldInRow.FieldColor = CurrentPlayer.PlayerColor;
                                if (CheckWeen(firstFreePlayingFieldInRow))
                                {
                                    isEndGame = true;
                                    popupService.ShowPopupAsync<ConnectFourPopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Koniec gry.\nWygrana.\n";
                                        //vm.ImageSymbol = currentPlayer.Name;
                                    });
                                    return;
                                }
                                if (CheckDraw())
                                {
                                    isEndGame = true;
                                    popupService.ShowPopupAsync<ConnectFourPopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Koniec gry.\nRemis.\n";
                                        //vm.ImageSymbol = currentPlayer.Name;
                                    });
                                    return;
                                }
                                currentPlayerNumber = (currentPlayerNumber + 1) % _players.Count;
                                CurrentPlayer = _players[currentPlayerNumber];
                            }
                        }
                        );
                return boardFieldCommand;
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
                            NewGame();
                        }
                        );
                return newGameCommand;
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

        private List<Player> _players;
        private string emptyColorField = "white";
        private bool isEndGame = false;
        private int winLineLength = 4;
        private IPopupService popupService;

        public ConnectFourViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

            ListOfRows = new ObservableCollection<int>() { 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionRow = ListOfRows.First();
            ListOfCols = new ObservableCollection<int>() { 4, 5, 6, 7, 8, 9, 10 };
            SelectedOptionCol = ListOfCols.First();

            _players = new List<Player>();
            _players.Add(new Player() { PlayerColor = "Red" });
            _players.Add(new Player() { PlayerColor = "Blue" });
            CurrentPlayer = currentPlayer = _players.First();

            NewGame();
        }

        private void NewGame()
        {
            RowCount = SelectedOptionRow;
            ColumnCount = SelectedOptionCol;

            ListOfPlayingField = new ObservableCollection<PlayingField>();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    ListOfPlayingField.Add(new PlayingField()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        FieldColor = emptyColorField,
                        BoardFieldCommand = BoardFieldCommand,
                    });
                }
            isEndGame = false;
        }

        private bool CheckDraw()
        {
            return !ListOfPlayingField.Any(pf => pf.FieldColor == emptyColorField);
        }

        private bool CheckWeen(PlayingField firstFreePlayingFieldInRow)
        {
            var horizontalLine = ListOfPlayingField.Where(pf => pf.RowIndex == firstFreePlayingFieldInRow.RowIndex
                                                              && pf.ColumnIndex > firstFreePlayingFieldInRow.ColumnIndex - winLineLength
                                                              && pf.ColumnIndex < firstFreePlayingFieldInRow.ColumnIndex + winLineLength
                                                              && pf.FieldColor == CurrentPlayer.PlayerColor
            ).OrderBy(pf => pf.ColumnIndex).ToList();

            int currentLineLength = 1;
            for (int i = 1; i < horizontalLine.Count; i++)
            {
                if (horizontalLine[i].ColumnIndex == horizontalLine[i - 1].ColumnIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            var verticalLine = ListOfPlayingField.Where(pf => pf.ColumnIndex == firstFreePlayingFieldInRow.ColumnIndex
                                                              && pf.RowIndex > firstFreePlayingFieldInRow.RowIndex - winLineLength
                                                              && pf.RowIndex < firstFreePlayingFieldInRow.RowIndex + winLineLength
                                                              && pf.FieldColor == CurrentPlayer.PlayerColor
            ).OrderBy(pf => pf.RowIndex).ToList();

            currentLineLength = 1;
            for (int i = 1; i < verticalLine.Count; i++)
            {
                if (verticalLine[i].RowIndex == verticalLine[i - 1].RowIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            List<PlayingField> firstDiagonalLine = new List<PlayingField>();

            for (int row = firstFreePlayingFieldInRow.RowIndex - winLineLength + 1, column = firstFreePlayingFieldInRow.ColumnIndex - winLineLength + 1; row < firstFreePlayingFieldInRow.RowIndex + winLineLength; row++, column++)
            {
                if (row < 0 || row >= RowCount || column < 0 || column >= ColumnCount)
                    continue;
                PlayingField? playerField = ListOfPlayingField.FirstOrDefault(pf => pf.RowIndex == row && pf.ColumnIndex == column);
                if (playerField != null && playerField.FieldColor == CurrentPlayer.PlayerColor)
                    firstDiagonalLine.Add(playerField);
            }
            firstDiagonalLine = firstDiagonalLine.OrderBy(pf => pf.RowIndex).ToList();

            currentLineLength = 1;
            for (int i = 1; i < firstDiagonalLine.Count; i++)
            {
                if (firstDiagonalLine[i].RowIndex == firstDiagonalLine[i - 1].RowIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            List<PlayingField> secondDiagonalLine = new List<PlayingField>();

            for (int row = firstFreePlayingFieldInRow.RowIndex - winLineLength + 1, column = firstFreePlayingFieldInRow.ColumnIndex + winLineLength - 1; row < firstFreePlayingFieldInRow.RowIndex + winLineLength; row++, column--)
            {
                if (row < 0
                    || row >= RowCount
                    || column < 0
                    || column >= ColumnCount)
                    continue;
                PlayingField? playerField = ListOfPlayingField.FirstOrDefault(pf => pf.RowIndex == row && pf.ColumnIndex == column);
                if (playerField != null && playerField.FieldColor == CurrentPlayer.PlayerColor)
                    secondDiagonalLine.Add(playerField);
            }

            secondDiagonalLine = secondDiagonalLine.OrderBy(pf => pf.RowIndex).ToList();

            currentLineLength = 1;
            for (int i = 1; i < secondDiagonalLine.Count; i++)
            {
                if (secondDiagonalLine[i].RowIndex == secondDiagonalLine[i - 1].RowIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            return false;
        }

        public void Dispose()
        {

        }
    }
}
