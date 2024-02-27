using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMauiGame.Enums;

namespace ChessMauiGame.Model.ChessPieces
{
    public class King : ChessPiece
    {
        public King(string color) : base("king", color)
        {

        }
        public override List<BoardSquare> GetListOfMoves(ObservableCollection<BoardSquare> boardToCheck, BoardSquare boardSquare, string emptyColor)
        {
            List<BoardSquare> listOfMoves;

            listOfMoves = boardToCheck.Where(bs => bs.RowIndex >= boardSquare.RowIndex-1 && bs.RowIndex <= boardSquare.RowIndex + 1
                                     && bs.ColumnIndex >= boardSquare.ColumnIndex - 1 && bs.ColumnIndex <= boardSquare.ColumnIndex + 1
                                     && bs.ChessPiece.Color != Color).ToList();

            return listOfMoves;
        }
    }
}
