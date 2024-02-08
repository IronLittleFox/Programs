using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuMauiGame.Model
{
    public class SquareField : BindableObject
    {
        private int rowIndex;
        public int RowIndex
        {
            get { return rowIndex; }
            set
            {
                rowIndex = value;
                OnPropertyChanged(nameof(RowIndex));
            }
        }

        private int columnIndex;
        public int ColumnIndex
        {
            get { return columnIndex; }
            set
            {
                columnIndex = value;
                OnPropertyChanged(nameof(ColumnIndex));
            }
        }

        private ObservableCollection<Field> fields = new();
        public ObservableCollection<Field> Fields
        {
            get { return fields; }
            set
            {
                fields = value;
                OnPropertyChanged(nameof(Fields));
            }
        }
    }
}
