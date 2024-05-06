using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AchiMauiGame.Model
{
    public class BoardSquare : BindableObject
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

        private bool isChoose;

        public bool IsChoose
        {
            get { return isChoose; }
            set { isChoose = value; OnPropertyChanged(); }
        }


        public ICommand? SquareCommand { get; set; } = null;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Pawn gamePawn;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Pawn GamePawn
        {
            get { return gamePawn; }
            set
            {
                gamePawn = value;
                OnPropertyChanged(nameof(GamePawn));
            }
        }
    }
}
