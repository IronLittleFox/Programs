using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMauiGame.Enums;
using UtilsMaui.Utils;

namespace ChessMauiGame.Model.ChessPieces
{
    public class Bishop : ChessPiece
    {
        public Bishop(string color) : base("bishop", color)
        {

        }
        public override List<BoardSquare> GetListOfMoves(ObservableCollection<BoardSquare> boardToCheck, BoardSquare boardSquare, string emptyColor)
        {
            List<BoardSquare> listOfMoves = new();
            List<(int dRow, int dCol)> directions = new List<(int dRow, int dCol)>()
            {
                (-1, 1),    //skos w góra prawo
                (1, 1),     //skos dół prawo
                (1, -1),    //skos dół lewo
                (-1, -1)    //skos góra lewo
            };

            directions.ForAll(direction =>
            {
                for (int row = boardSquare.RowIndex + direction.dRow, col = boardSquare.ColumnIndex + direction.dCol; ; row += direction.dRow, col += direction.dCol)
                {
                    BoardSquare? newPosition = boardToCheck.FirstOrDefault(bs => bs.RowIndex == row && bs.ColumnIndex == col);
                    if (newPosition == null)
                        break;

                    if (newPosition.ChessPiece.Color == Color)
                        break;

                    listOfMoves.Add(newPosition);

                    if (newPosition.ChessPiece.Color != emptyColor)
                        break;
                }
            });
            /*
            //skos w góra prawo
            for (int row = boardSquare.RowIndex - 1, col = boardSquare.ColumnIndex + 1; ; row--, col++)
            {
                BoardSquare? newPosition = boardToCheck.FirstOrDefault(bs => bs.RowIndex == row && bs.ColumnIndex == col);
                if (newPosition == null)
                    break;

                if (newPosition.ChessPiece.Color == Color)
                    break;

                //move.Add(newPosition);
                listOfMoves.Add(newPosition);

                if (newPosition.ChessPiece.Color != emptyColor)
                    break;
            }

            //skos dół prawo
            for (int row = boardSquare.RowIndex + 1, col = boardSquare.ColumnIndex + 1; ; row++, col++)
            {
                BoardSquare? newPosition = boardToCheck.FirstOrDefault(bs => bs.RowIndex == row && bs.ColumnIndex == col);
                if (newPosition == null)
                    break;

                if (newPosition.ChessPiece.Color == Color)
                    break;

                //move.Add(newPosition);
                listOfMoves.Add(newPosition);

                if (newPosition.ChessPiece.Color != emptyColor)
                    break;
            }

            //skos dół lewo
            for (int row = boardSquare.RowIndex + 1, col = boardSquare.ColumnIndex - 1; ; row++, col--)
            {
                BoardSquare? newPosition = boardToCheck.FirstOrDefault(bs => bs.RowIndex == row && bs.ColumnIndex == col);
                if (newPosition == null)
                    break;

                if (newPosition.ChessPiece.Color == Color)
                    break;

                //move.Add(newPosition);
                listOfMoves.Add(newPosition);

                if (newPosition.ChessPiece.Color != emptyColor)
                    break;
            }

            //skos góra lewo
            for (int row = boardSquare.RowIndex - 1, col = boardSquare.ColumnIndex - 1; ; row--, col--)
            {
                BoardSquare? newPosition = boardToCheck.FirstOrDefault(bs => bs.RowIndex == row && bs.ColumnIndex == col);
                if (newPosition == null)
                    break;

                if (newPosition.ChessPiece.Color == Color)
                    break;

                //move.Add(newPosition);
                listOfMoves.Add(newPosition);

                if (newPosition.ChessPiece.Color != emptyColor)
                    break;
            }

            */
            return listOfMoves;
        }
    }
}
