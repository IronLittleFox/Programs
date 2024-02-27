using ChessMauiGame.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMauiGame.Model.ChessPieces
{
    public abstract class ChessPiece
    {
        public string Color { get; }
        public string Type { get; }
        public bool IsFirstMove { get; set; } = true;

        public ChessPiece(string type, string color)
        {
            Type = type;
            Color = color;
        }

        public abstract List<BoardSquare> GetListOfMoves(ObservableCollection<BoardSquare> boardToCheck, BoardSquare boardSquare, string emptyColor);
    }
}
