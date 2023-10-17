using LangtonAntWpfApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangtonAntWpfApp.Model
{
    public class LangtonAnt
    {
        private int currentDirection = 1;
        private BoardField currentBoardField;
        private string antColor = "Red";

        public LangtonAnt(string antColor = "Red")
        {
            currentBoardField = Dane.Board[Dane.ColumnCount * Dane.RowCount / 2 + Dane.RowCount / 2];
            currentBoardField.AntText = "A";
            currentBoardField.AntColor = antColor;
            this.antColor = antColor;
        }

        public void Move()
        {
            currentBoardField.AntText = "";
            currentDirection = (currentDirection + (currentBoardField.IsWhite ? 1 : -1) + Dane.Directions.Count) % Dane.Directions.Count;
            currentBoardField.IsWhite = !currentBoardField.IsWhite;

            int colNext = (currentBoardField.ColumnIndex + Dane.Directions[currentDirection].ColumnStep + Dane.ColumnCount) % Dane.ColumnCount;
            int rowNext = (currentBoardField.RowIndex + Dane.Directions[currentDirection].RowStep + Dane.RowCount) % Dane.RowCount;
            currentBoardField = Dane.Board.First(b => b.ColumnIndex == colNext && b.RowIndex == rowNext);
            currentBoardField.AntText = "A";
            currentBoardField.AntColor = antColor;
        }
    }
}
