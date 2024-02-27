using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMauiGame.Enums;

namespace ChessMauiGame.Model.ChessPieces
{
    public class Queen : ChessPiece
    {
        public Queen(string color) : base("queen", color)
        {

        }
        public override List<BoardSquare> GetListOfMoves(ObservableCollection<BoardSquare> boardToCheck, BoardSquare boardSquare, string emptyColor)
        {
            List<BoardSquare> listOfMoves = new();

            listOfMoves.AddRange(new Rook(Color).GetListOfMoves(boardToCheck, boardSquare, emptyColor));
            listOfMoves.AddRange(new Bishop(Color).GetListOfMoves(boardToCheck, boardSquare, emptyColor));

            return listOfMoves;
        }
    }
}
