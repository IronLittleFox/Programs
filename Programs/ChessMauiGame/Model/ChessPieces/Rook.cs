using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMauiGame.Enums;

namespace ChessMauiGame.Model.ChessPieces
{
    public class Rook : ChessPiece
    {
        public Rook(string color) : base("rook", color)
        {

        }
        public override List<BoardSquare> GetListOfMoves(ObservableCollection<BoardSquare> boardToCheck, BoardSquare boardSquare, string emptyColor)
        {
            List<BoardSquare> listOfMoves = new();

            List<List<BoardSquare>> listOfBoardSquare = new();

            //do góry
            listOfBoardSquare.Add(boardToCheck.Where(bs => bs.RowIndex < boardSquare.RowIndex
                                                         && bs.ColumnIndex == boardSquare.ColumnIndex)
                                .OrderByDescending(bs => bs.RowIndex).ToList());
            //w prawo
            listOfBoardSquare.Add(boardToCheck.Where(bs => bs.ColumnIndex > boardSquare.ColumnIndex
                                                         && bs.RowIndex == boardSquare.RowIndex)
                                .OrderBy(bs => bs.ColumnIndex).ToList());
            //na dół
            listOfBoardSquare.Add(boardToCheck.Where(bs => bs.RowIndex > boardSquare.RowIndex
                                                         && bs.ColumnIndex == boardSquare.ColumnIndex)
                                .OrderBy(bs => bs.RowIndex).ToList());
            //w lewo
            listOfBoardSquare.Add(boardToCheck.Where(bs => bs.ColumnIndex < boardSquare.ColumnIndex
                                                         && bs.RowIndex == boardSquare.RowIndex)
                                .OrderByDescending(bs => bs.ColumnIndex).ToList());
            foreach (var line in listOfBoardSquare)
            {
                foreach (var bs in line)
                {
                    //trafiony swój kolor
                    if (bs.ChessPiece.Color == Color)
                        break;
                    //trafiony kolor przeciwnika
                    if (bs.ChessPiece.Color != Color
                        && bs.ChessPiece.Color != emptyColor)
                    {
                        listOfMoves.Add(bs);
                        break;
                    }
                    listOfMoves.Add(bs);
                }
            }

            return listOfMoves;
        }
    }
}
