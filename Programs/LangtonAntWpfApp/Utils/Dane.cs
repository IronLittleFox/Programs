using LangtonAntWpfApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LangtonAntWpfApp.Utils
{
    public static class Dane
    {
        private static int columnCount = 40;
        public static int ColumnCount
        {
            get { return columnCount; }
        }

        private static int rowCount = 40;
        public static int RowCount
        {
            get { return rowCount; }
        }

        public static List<DirectionChange> Directions { get; } = new List<DirectionChange>()
        {
            new DirectionChange() { ColumnStep = 0, RowStep = -1 },//Góra
            new DirectionChange() { ColumnStep = -1, RowStep = 0 }, //Lewo
            new DirectionChange() { ColumnStep = 0, RowStep = 1 }, //Dół
            new DirectionChange() { ColumnStep = 1, RowStep = 0 } //Prawo
        };

        public static ObservableCollection<BoardField> Board { get; set; } = new ObservableCollection<BoardField>();

        static Dane()
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    Board.Add(new BoardField() { ColumnIndex = i, RowIndex = j, IsWhite = true });
                }
            }
        }
    };
}

