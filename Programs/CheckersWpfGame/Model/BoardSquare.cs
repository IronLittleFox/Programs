using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckersWpfGame.Model
{
    public class BoardSquare : ViewObserver
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        private string squareColor = "white";
        public string SquareColor
        {
            get { return squareColor; }
            set
            {
                squareColor = value;
                OnPropertyChanged(nameof(SquareColor));
            }
        }

        public ICommand? SquareCommand { get; set; } = null;

        private CheckerPiece checkerPiece;
        public CheckerPiece CheckerPiece
        {
            get { return checkerPiece; }
            set
            {
                checkerPiece = value;
                OnPropertyChanged(nameof(CheckerPiece));
            }
        }

        private bool isCheckerPieceMustMove;
        public bool IsCheckerPieceMustMove
        {
            get { return isCheckerPieceMustMove; }
            set
            {
                isCheckerPieceMustMove = value;
                OnPropertyChanged(nameof(IsCheckerPieceMustMove));
            }
        }

        private bool isPossibleMove;
        public bool IsPossibleMove
        {
            get { return isPossibleMove; }
            set
            {
                isPossibleMove = value;
                OnPropertyChanged(nameof(IsPossibleMove));
            }
        }

    }
}
