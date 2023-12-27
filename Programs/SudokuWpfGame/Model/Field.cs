using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SudokuWpfGame.Model
{
    public class Field : ViewObserver
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

        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private string numberHide;
        public string NumberHide
        {
            get { return numberHide; }
            set
            {
                numberHide = value;
                OnPropertyChanged(nameof(NumberHide));
            }
        }

        private bool isEmptyWhenStart;
        public bool IsEmptyWhenStart
        {
            get { return isEmptyWhenStart; }
            set
            {
                isEmptyWhenStart = value;
                OnPropertyChanged(nameof(IsEmptyWhenStart));
            }
        }

        public ICommand BoardFieldCommand { get;set; }
    }
}
