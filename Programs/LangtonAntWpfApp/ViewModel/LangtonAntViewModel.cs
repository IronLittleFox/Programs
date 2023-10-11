using GameUtils.Utils;
using LangtonAntWpfApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace LangtonAntWpfApp.ViewModel
{
    public class LangtonAntViewModel : ViewObserver
    {

        private int stepNumber = 0;
        public int StepNumber
        {
            get { return stepNumber; }
            set
            {
                stepNumber = value;
                OnPropertyChanged(nameof(StepNumber));
            }
        }

        private int columnCount = 40;
        public int ColumnCount
        {
            get { return columnCount; }
            set
            {
                columnCount = value;
                OnPropertyChanged(nameof(ColumnCount));
            }
        }

        private int rowCount = 40;
        public int RowCount
        {
            get { return rowCount; }
            set
            {
                rowCount = value;
                OnPropertyChanged(nameof(RowCount));
            }
        }

        private ICommand? nextAntStepCommand = null;
        public ICommand NextAntStepCommand
        {
            get
            {
                if (nextAntStepCommand == null)
                    nextAntStepCommand = new RelayCommand<object>(
                        o =>
                        {
                            StepNumber++;
                            currentBoardField.AntText = "";
                            currentDirection = (currentDirection + (currentBoardField.IsWhite ? 1 : -1) + directions.Count) % directions.Count;
                            currentBoardField.IsWhite = !currentBoardField.IsWhite;

                            int colNext = (currentBoardField.ColumnIndex + directions[currentDirection].ColumnStep + ColumnCount) % ColumnCount;
                            int rowNext = (currentBoardField.RowIndex + directions[currentDirection].RowStep + RowCount) % RowCount;
                            currentBoardField = Board.First(b => b.ColumnIndex == colNext && b.RowIndex == rowNext);
                            currentBoardField.AntText = "A";
                        }
                        );
                return nextAntStepCommand;
            }
        }

        public ObservableCollection<BoardField> Board { get; set; } = new ObservableCollection<BoardField>();

        private BoardField currentBoardField;
        private List<DirectionChange> directions = new List<DirectionChange>();
        private int currentDirection = 1;

        public LangtonAntViewModel()
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    Board.Add(new BoardField() { ColumnIndex = i, RowIndex = j, IsWhite = true });
                }
            }

            currentBoardField = Board[ColumnCount * RowCount / 2 + RowCount / 2];
            currentBoardField.AntText = "A";

            directions.Add(new DirectionChange() { ColumnStep = 0, RowStep = -1 }); //Góra
            directions.Add(new DirectionChange() { ColumnStep = -1, RowStep = 0 }); //Lewo
            directions.Add(new DirectionChange() { ColumnStep = 0, RowStep = 1 }); //Dół
            directions.Add(new DirectionChange() { ColumnStep = 1, RowStep = 0 }); //Prawo
        }
    }
}
