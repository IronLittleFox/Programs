using CommunityToolkit.Maui.Core;
using SudokuMauiGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilsMaui.Interfaces;
using UtilsMaui.Utils;

namespace SudokuMauiGame.ViewModel
{
    public class SudokuViewModel : BindableObject, IGameViewModel
    {
        private ObservableCollection<SquareField> listOfSqure = new();
        public ObservableCollection<SquareField> ListOfSqure
        {
            get { return listOfSqure; }
            set
            {
                listOfSqure = value;
                OnPropertyChanged(nameof(ListOfSqure));
            }
        }

        private ObservableCollection<NumberToChoose> listOfNumbers = new();
        public ObservableCollection<NumberToChoose> ListOfNumbers
        {
            get { return listOfNumbers; }
            set
            {
                listOfNumbers = value;
                OnPropertyChanged(nameof(ListOfNumbers));
            }
        }

        private ICommand? boardFieldCommand;
        public ICommand BoardFieldCommand
        {
            get
            {
                if (boardFieldCommand == null)
                    boardFieldCommand = new Command<Field>(
                        field =>
                        {
                            if (isEndGame)
                                return;
                            if (!field.IsEmptyWhenStart)
                                return;

                            //field.Number = field.NumberHide;
                            field.Number = numberToChoose.Number.ToString();

                            if (ListOfSqure.All(sq => sq.Fields.All(f => f.Number != "")))
                            {
                                if (listOfSqure.All(sq => sq.Fields.All(f => f.Number == f.NumberHide)))
                                {
                                    isEndGame = true;
                                    popupService.ShowPopupAsync<SudokuPopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Gratulacje!!!\nPlansza ułożona prawidłowo.";
                                        //vm.ImageSymbol = currentPlayer.Name;
                                    });
                                    return;
                                }
                                else
                                {
                                    popupService.ShowPopupAsync<SudokuPopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Wstawiłeś nieprawidłowe liczby na planszy.";
                                        //vm.ImageSymbol = currentPlayer.Name;
                                    });
                                    return;
                                }
                            }
                        }
                        );
                return boardFieldCommand;
            }
        }

        private ICommand? numberToChooseCommand;
        public ICommand NumberToChooseCommand
        {
            get
            {
                if (numberToChooseCommand == null)
                    numberToChooseCommand = new Command<NumberToChoose>(
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

        private ICommand? resetBoardCommand;
        public ICommand ResetBoardCommand
        {
            get
            {
                if (resetBoardCommand == null)
                    resetBoardCommand = new Command<object>(
                        o =>
                        {
                            ListOfSqure.ForAll(sq => sq.Fields.Where(f => f.IsEmptyWhenStart).ForAll(f => f.Number = ""));
                            isEndGame = false;
                        }
                        );
                return resetBoardCommand;
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

        private NumberToChoose numberToChoose;
        private bool isEndGame = false;
        private IPopupService popupService;

        public SudokuViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

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

        public void Dispose()
        {
        }
    }
}
