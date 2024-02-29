using ChessMauiGame.Model.ChessPieces;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChessMauiGame.Model
{
    public class BoardSquare : BindableObject
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        private string squareColor = "White";
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ChessPiece chessPiece;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ChessPiece ChessPiece
        {
            get { return chessPiece; }
            set
            {
                chessPiece = value;
                OnPropertyChanged(nameof(ChessPiece));
            }
        }

        private bool isChessPieceMustMove = false;
        public bool IsChessPieceMustMove
        {
            get { return isChessPieceMustMove; }
            set
            {
                isChessPieceMustMove = value;
                OnPropertyChanged(nameof(IsChessPieceMustMove));
            }
        }

        private bool isPossibleMove = false;
        public bool IsPossibleMove
        {
            get { return isPossibleMove; }
            set
            {
                isPossibleMove = value;
                OnPropertyChanged();
            }
        }

    }
}
