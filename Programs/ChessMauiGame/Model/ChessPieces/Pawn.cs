using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMauiGame.Enums;

namespace ChessMauiGame.Model.ChessPieces
{
    public class Pawn : ChessPiece
    {
        private int dRow;
        public Pawn(string color, int dRow) : base("pawn", color)
        {
            this.dRow = dRow;
        }
        public override List<BoardSquare> GetListOfMoves(ObservableCollection<BoardSquare> boardToCheck, BoardSquare boardSquare, string emptyColor)
        {
            List<BoardSquare> listOfMoves = new();

            BoardSquare? newMoveBoardSquare;

            //do przodu
            newMoveBoardSquare = boardToCheck.FirstOrDefault(bs => bs.ColumnIndex == boardSquare.ColumnIndex
                                                                  && bs.RowIndex == boardSquare.RowIndex + dRow
                                                                  && bs.ChessPiece.Color == emptyColor);
            if (newMoveBoardSquare != null)
            {
                listOfMoves.Add(newMoveBoardSquare);
                if (IsFirstMove)
                {
                    newMoveBoardSquare = boardToCheck.FirstOrDefault(bs => bs.ColumnIndex == boardSquare.ColumnIndex
                                                                  && bs.RowIndex == boardSquare.RowIndex + dRow * 2
                                                                  && bs.ChessPiece.Color == emptyColor);
                    if (newMoveBoardSquare != null)
                        listOfMoves.Add(newMoveBoardSquare);
                }
            }

            //na prawo
            newMoveBoardSquare = boardToCheck.FirstOrDefault(bs => bs.ColumnIndex == boardSquare.ColumnIndex + 1
                                                                  && bs.RowIndex == boardSquare.RowIndex + dRow
                                                                  && bs.ChessPiece.Color != Color
                                                                  && bs.ChessPiece.Color != emptyColor);
            if (newMoveBoardSquare != null)
                listOfMoves.Add(newMoveBoardSquare);

            //na lewo
            newMoveBoardSquare = boardToCheck.FirstOrDefault(bs => bs.ColumnIndex == boardSquare.ColumnIndex - 1
                                                                  && bs.RowIndex == boardSquare.RowIndex + dRow
                                                                  && bs.ChessPiece.Color != Color
                                                                  && bs.ChessPiece.Color != emptyColor);
            if (newMoveBoardSquare != null)
                listOfMoves.Add(newMoveBoardSquare);

            return listOfMoves;
        }
    }
}
