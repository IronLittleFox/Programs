using GameUtils.Interfaces;
using GameUtils.Utils;
using SudokuWpfGame.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SudokuWpfGame.ViewModel
{
    public class SudokuViewModel : ViewObserver, IGameViewModel
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


        private ObservableCollection<SquareField> listOfSqure;
        public ObservableCollection<SquareField> ListOfSqure
        {
            get { return listOfSqure; }
            set
            {
                listOfSqure = value;
                OnPropertyChanged(nameof(ListOfSqure));
            }
        }

        private ObservableCollection<NumberToChoose> listOfNumbers;
        public ObservableCollection<NumberToChoose> ListOfNumbers
        {
            get { return listOfNumbers; }
            set
            {
                listOfNumbers = value;
                OnPropertyChanged(nameof(ListOfNumbers));
            }
        }

        private ICommand boardFieldCommand;
        public ICommand BoardFieldCommand
        {
            get
            {
                if (boardFieldCommand == null)
                    boardFieldCommand = new RelayCommand<Field>(
                        field =>
                        {
                            if (isEndGame) 
                                return;
                            if (!field.IsEmptyWhenStart)
                                return;

                            //field.Number = field.NumberHide;
                            field.Number = numberToChoose.Number.ToString();

                            if (ListOfSqure.All(sq => sq.Fields.All(f=> f.Number != "")))
                            {
                                if (listOfSqure.All(sq => sq.Fields.All(f => f.Number == f.NumberHide)))
                                {
                                    isEndGame = true;
                                    ShowGameScore = true;
                                    ShowMessageScore = "Gratulacje!!!\nPlansza ułożona prawidłowo.";
                                    return;
                                }
                                else
                                {
                                    ShowGameScore = true;
                                    ShowMessageScore = "Wstawiłeś nieprawidłowe liczby na planszy.";
                                    return;
                                }
                            }
                        }
                        );
                return boardFieldCommand;
            }
        }

        private ICommand numberToChooseCommand;
        public ICommand NumberToChooseCommand
        {
            get
            {
                if (numberToChooseCommand == null)
                    numberToChooseCommand = new RelayCommand<NumberToChoose>(
                        n =>
                        {
                            numberToChoose.IsChoose = false;
                            numberToChoose = n;
                            numberToChoose.IsChoose = true;
                        }
                        );
                return numberToChooseCommand;
            }
        }

        private ICommand resetBoardCommand;
        public ICommand ResetBoardCommand
        {
            get
            {
                if (resetBoardCommand == null)
                    resetBoardCommand = new RelayCommand<object>(
                        o =>
                        {
                            ListOfSqure.ForAll(sq => sq.Fields.Where(f => f.IsEmptyWhenStart).ForAll(f => f.Number = ""));
                            isEndGame = false;
                        }
                        );
                return resetBoardCommand;
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
                            NewGame();
                        }
                        );
                return newGameCommand;
            }
        }

        private NumberToChoose numberToChoose;
        private bool isEndGame = false;

        public SudokuViewModel()
        {
            ListOfNumbers = new ObservableCollection<NumberToChoose>();
            Enumerable.Range(1, 9).ForAll(n => ListOfNumbers.Add(new NumberToChoose() { Number = n.ToString(), IsChoose = false, NumberToChooseCommand = this.NumberToChooseCommand }));
            ListOfNumbers.Insert(0, new NumberToChoose() { Number = "", IsChoose = true, NumberToChooseCommand = this.NumberToChooseCommand });
            numberToChoose = ListOfNumbers.First();

            ListOfSqure = new ObservableCollection<SquareField>();

            NewGame();
        }

        private void NewGame()
        {
            isEndGame = false;

            SudokuGenerator sudokuGenerator = new SudokuGenerator(3);
            ListOfSqure.Clear();
            for (int squareRow = 0; squareRow < 3; squareRow++)
            {
                for (int squareCol = 0; squareCol < 3; squareCol++)
                {
                    SquareField squareField = new SquareField();
                    squareField.RowIndex = squareRow;
                    squareField.ColumnIndex = squareCol;
                    squareField.Fields = new ObservableCollection<Field>();

                    for (int row = 0; row < 3; row++)
                    {
                        for (int col = 0; col < 3; col++)
                        {
                            Field field = new Field();
                            field.RowIndex = row;
                            field.ColumnIndex = col;
                            //field.NumberHide = (row * 3 + col).ToString();
                            field.NumberHide = sudokuGenerator.Sudoku[squareRow * 3 + row][squareCol * 3 + col].ToString();
                            field.Number = sudokuGenerator.SudokuWidthRemovingNumbers[squareRow * 3 + row][squareCol * 3 + col].ToString();
                            field.Number = field.Number == "0" ? "" : field.Number;
                            field.IsEmptyWhenStart = field.Number == "";
                            //field.Number = "";
                            field.BoardFieldCommand = BoardFieldCommand;
                            squareField.Fields.Add(field);
                        }
                    }
                    ListOfSqure.Add(squareField);
                }
            }
        }
    }
}
