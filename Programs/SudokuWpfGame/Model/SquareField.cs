using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuWpfGame.Model
{
    public class SquareField : ViewObserver
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

        private ObservableCollection<Field> fields;
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
