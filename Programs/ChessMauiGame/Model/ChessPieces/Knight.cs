using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMauiGame.Enums;

namespace ChessMauiGame.Model.ChessPieces
{
    public class Knight : ChessPiece
    {
        public List<(int dCol, int dRow)> ListOfDirections; 

        public Knight(string color) : base("knight", color)
        {
            ListOfDirections = new();
            //góra
            ListOfDirections.Add((-1, -2));
            ListOfDirections.Add((1, -2));
            //prawo
            ListOfDirections.Add((2, -1));
            ListOfDirections.Add((2, 1));
            //dół
            ListOfDirections.Add((-1, 2));
            ListOfDirections.Add((1, 2));
            //lewo
            ListOfDirections.Add((-2, -1));
            ListOfDirections.Add((-2, 1));
        }
        public override List<BoardSquare> GetListOfMoves(ObservableCollection<BoardSquare> boardToCheck, BoardSquare boardSquare, string emptyColor)
        {
            List<BoardSquare> listOfMoves = new();

            ListOfDirections.ForEach(((int dCol, int dRow) direction) =>
            {
                int newCol = boardSquare.ColumnIndex + direction.dCol;
                int newRow = boardSquare.RowIndex + direction.dRow;
                BoardSquare? newPositionBoardSquare = boardToCheck
                    .FirstOrDefault(bs => bs.ColumnIndex == newCol
                                          && bs.RowIndex == newRow);
                if (newPositionBoardSquare!= null
                    && newPositionBoardSquare.ChessPiece.Color != boardSquare.ChessPiece.Color) 
                {
                    listOfMoves.Add(newPositionBoardSquare);
                }
            });

            return listOfMoves;
        }
    }
}
